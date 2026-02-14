using Crystal_Clinic_Mgm.Application.AssetMS.MainAssets.Commands;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Application.Events;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock.Look;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Crystal_Clinic_Mgm.Application.BranchStock
{
    public class InventoryService : IInventoryService
    {
        private readonly ERP_DbContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<InventoryService> _logger;
        private readonly ILoggedInUser _loggedInUser;

        public InventoryService(
            ERP_DbContext context,
            IMediator mediator,
            ILogger<InventoryService> logger,
            ILoggedInUser loggedInUser)
        {
            _context = context;
            _mediator = mediator;
            _logger = logger;
            _loggedInUser = loggedInUser;
        }

        public async Task<MovementResult> RegisterMovementAsync(
             MovementRequest request,
             CancellationToken cancellationToken = default)
            {
                try
            {
                var validationResult = await ValidateMovementRequest(request);
                if (!validationResult.IsValid)
                    return new MovementResult { Success = false, ErrorMessage = validationResult.ErrorMessage };

                var (movements, totalQuantity, weightedAverageCost) =
                    await ProcessMultiLotMovement(request, cancellationToken);

                if (!movements.Any())
                {
                    var diagnosticMessage =
                        await DiagnoseStockIssue(request.ItemId, request.BranchId, cancellationToken);

                    return new MovementResult { Success = false, ErrorMessage = diagnosticMessage };
                }

                var movementIds = new List<int>();

                foreach (var multiLotMove in movements)
                {
                    var movement = new StockMovement
                    {
                        Date = DateTime.UtcNow,
                        MovementType = request.Type,
                        Reason = request.Reason,
                        ItemId = request.ItemId,
                        StockId = multiLotMove.StockId,
                        Quantity = multiLotMove.MovedQuantity,
                        UnitCost = multiLotMove.UnitCost,
                        TotalCost = multiLotMove.MovedQuantity * multiLotMove.UnitCost,
                        ProcessedBy = request.ProcessedBy ?? _loggedInUser.Id,
                        ReferenceId = request.ReferenceId ?? string.Empty,
                        Notes = $"{request.Notes ?? ""} [Lot: {multiLotMove.LotNumber}]",
                        SourceBranchId = request.BranchId
                    };

                    _context.StockMovements.Add(movement);
                }

                await _context.SaveChangesAsync(cancellationToken);

                foreach (var movement in _context.StockMovements.Local)
                {
                    await PublishMovementEvent(movement, cancellationToken);
                }

                return new MovementResult
                {
                    Success = true,
                    NewBalance = movements.Last().RemainingAfter,
                    MovementId = movements.First().StockId,
                    UnitCost = weightedAverageCost,
                    TotalCost = totalQuantity * weightedAverageCost,
                    Quantity = totalQuantity
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering inventory movement for item {ItemId}", request.ItemId);
                return new MovementResult
                {
                    Success = false,
                    ErrorMessage = "Internal error occurred while processing movement"
                };
            }
        }


        public async Task<ReservationResult> ReserveItemsAsync(ReservationRequest request, CancellationToken cancellationToken = default)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var existingReservation = await _context.InventoryReservations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.IdempotencyToken == request.IdempotencyToken, cancellationToken);

                if (existingReservation != null)
                {
                    if (existingReservation.ExpiresAt > DateTime.UtcNow)
                    {
                        await transaction.CommitAsync(cancellationToken);
                        var existingLots = await GetReservedLots(existingReservation.Id, cancellationToken);
                        return new ReservationResult
                        {
                            Success = true,
                            ReservationId = existingReservation.Id,
                            ExpiresAt = existingReservation.ExpiresAt,
                            ReservedLots = existingLots
                        };
                    }
                    else
                    {
                        await ReleaseReservationAsync(existingReservation.Id, cancellationToken);
                    }
                }

                var availabilityCheck = await CheckItemsAvailability(request.Items, cancellationToken);
                if (!availabilityCheck.IsAvailable)
                {
                    await transaction.CommitAsync(cancellationToken);
                    return new ReservationResult
                    {
                        Success = false,
                        ErrorMessage = "Insufficient stock for one or more items",
                        Shortages = availabilityCheck.Shortages
                    };
                }

                var reservation = new InventoryReservation
                {
                    VisitId = request.VisitId,
                    ServiceId = request.ServiceId,
                    IdempotencyToken = request.IdempotencyToken,
                    ExpiresAt = DateTime.UtcNow.AddSeconds(request.TtlSeconds),
                    RequestedBy = request.RequestedBy ?? _loggedInUser.Id,
                    Status = Crystal_Clinic_Mgm.Domain.Entities.BranchStock.ReservationStatus.Active,
                    BranchId = _loggedInUser.BranchId
                };

                _context.InventoryReservations.Add(reservation);
                await _context.SaveChangesAsync(cancellationToken);

                var reservedItems = new List<ReservedItem>();
                foreach (var item in request.Items)
                {
                    var lots = await ReserveItemLots(reservation.Id, item, cancellationToken);
                    reservedItems.AddRange(lots);
                }

                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Created reservation {ReservationId} with idempotency token {Token} for visit {VisitId}",
                    reservation.Id, request.IdempotencyToken, request.VisitId);

                var reservedLots = reservedItems.Select(ri => new ReservedLot
                {
                    StockId = ri.StockId,
                    ItemId = ri.ItemId,
                    ReservedQuantity = ri.ReservedQuantity,
                    UnitCost = ri.UnitCost
                });

                return new ReservationResult
                {
                    Success = true,
                    ReservationId = reservation.Id,
                    ExpiresAt = reservation.ExpiresAt,
                    ReservedLots = reservedLots
                };
            }
            catch (DbUpdateException dbex) when (dbex.InnerException?.Message.Contains("UNIQUE") == true)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogWarning(dbex, "Duplicate idempotency token detected: {Token}", request.IdempotencyToken);
                
                var retry = await _context.InventoryReservations
                    .FirstOrDefaultAsync(r => r.IdempotencyToken == request.IdempotencyToken, cancellationToken);
                
                if (retry != null && retry.ExpiresAt > DateTime.UtcNow)
                {
                    var reservedLots = await GetReservedLots(retry.Id, cancellationToken);
                    return new ReservationResult
                    {
                        Success = true,
                        ReservationId = retry.Id,
                        ExpiresAt = retry.ExpiresAt,
                        ReservedLots = reservedLots
                    };
                }

                return new ReservationResult
                {
                    Success = false,
                    ErrorMessage = "Failed to create reservation: duplicate request detected"
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error creating reservation for visit {VisitId}", request.VisitId);

                return new ReservationResult
                {
                    Success = false,
                    ErrorMessage = "Failed to create reservation"
                };
            }
        }

        public async Task<Crystal_Clinic_Mgm.Domain.Entities.Result> CommitReservationAsync(int reservationId, CancellationToken cancellationToken = default)
        {
            var reservation = await _context.InventoryReservations
                .Include(r => r.ReservedItems)
                .FirstOrDefaultAsync(r => r.Id == reservationId, cancellationToken);
            if (reservation == null || reservation.Status != Crystal_Clinic_Mgm.Domain.Entities.BranchStock.ReservationStatus.Active)
            {
                return Crystal_Clinic_Mgm.Domain.Entities.Result.Fail("Reservation not found or not active");
            }

            if (reservation.ExpiresAt < DateTime.UtcNow)
            {
                return Crystal_Clinic_Mgm.Domain.Entities.Result.Fail("Reservation has expired");
            }

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                // Get reserved items
                var reservedItems = await _context.ReservedItems
                    .Where(ri => ri.ReservationId == reservationId)
                    .ToListAsync(cancellationToken);

                // Convert reservations to actual movements
                foreach (var reservedItem in reservedItems)
                {
                    var movementRequest = new MovementRequest
                    {
                        ItemId = reservedItem.ItemId,
                        StockId = reservedItem.StockId,
                        Quantity = reservedItem.ReservedQuantity,
                        Type = MovementType.Out,
                        Reason = MovementReason.SaleDeduction,
                        ReferenceId = $"VIS-{reservation.VisitId}",
                        ProcessedBy = reservation.RequestedBy
                    };

                    var result = await RegisterMovementAsync(movementRequest, cancellationToken);
                    if (!result.Success)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Crystal_Clinic_Mgm.Domain.Entities.Result.Fail($"Failed to process movement: {result.ErrorMessage}");
                    }
                }

                reservation.Status = Crystal_Clinic_Mgm.Domain.Entities.BranchStock.ReservationStatus.Committed;
                
                try
                {
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Crystal_Clinic_Mgm.Domain.Entities.Result.Fail("Reservation has been modified by another request. Please retry.");
                }

                await transaction.CommitAsync(cancellationToken);
                return Crystal_Clinic_Mgm.Domain.Entities.Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error committing reservation {ReservationId}", reservationId);
                return Crystal_Clinic_Mgm.Domain.Entities.Result.Fail("Failed to commit reservation");
            }
        }

        public async Task<Crystal_Clinic_Mgm.Domain.Entities.Result> ReleaseReservationAsync(int reservationId, CancellationToken cancellationToken = default)
        {
            var reservation = await _context.InventoryReservations
                .Include(r => r.ReservedItems)
                .FirstOrDefaultAsync(r => r.Id == reservationId, cancellationToken);

            if (reservation == null)
            {
                return Crystal_Clinic_Mgm.Domain.Entities.Result.Fail("Reservation not found");
            }

            if (reservation.Status == Crystal_Clinic_Mgm.Domain.Entities.BranchStock.ReservationStatus.Committed)
            {
                return Crystal_Clinic_Mgm.Domain.Entities.Result.Fail("Cannot release a committed reservation. Reserved items have already been deducted.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                foreach (var reservedItem in reservation.ReservedItems)
                {
                    var stock = await _context.Stocks.FindAsync(reservedItem.StockId);
                    if (stock != null)
                    {
                        stock.QuantityRemaining += reservedItem.ReservedQuantity;
                        _context.Stocks.Update(stock);
                    }
                }

                reservation.Status = Crystal_Clinic_Mgm.Domain.Entities.BranchStock.ReservationStatus.Released;
                _context.ReservedItems.RemoveRange(reservation.ReservedItems);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Released reservation {ReservationId} with {Count} items", 
                    reservationId, reservation.ReservedItems.Count);

                await transaction.CommitAsync(cancellationToken);

                return Crystal_Clinic_Mgm.Domain.Entities.Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error releasing reservation {ReservationId}", reservationId);
                return Crystal_Clinic_Mgm.Domain.Entities.Result.Fail("Failed to release reservation");
            }
        }

        public async Task<StockLevel> GetStockLevelAsync(int itemId, int? branchId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.Stocks
                .Where(s => s.ItemId == itemId && !s.IsDeleted);

            if (branchId.HasValue)
            {
                query = query.Where(s => s.BranchId == branchId.Value);
            }

            var stocks = await query
                .Include(s => s.Item)
                .ToListAsync(cancellationToken);

            var totalQuantity = stocks.Sum(s => s.Quantity);
            var usableQuantity = stocks.Sum(s => s.QuantityRemaining);
            var reservedQuantity = await GetReservedQuantity(itemId, branchId, cancellationToken);

            return new StockLevel
            {
                ItemId = itemId,
                ItemName = stocks.FirstOrDefault()?.Item?.Name ?? "Unknown",
                TotalQuantity = totalQuantity,
                UsableQuantity = usableQuantity,
                ReservedQuantity = reservedQuantity,
                AvailableQuantity = usableQuantity - reservedQuantity,
                Batches = stocks.Select(s => new StockBatch
                {
                    StockId = s.StockId,
                    LotNumber = s.LotNumber,
                    Quantity = s.Quantity,
                    QuantityRemaining = s.QuantityRemaining,
                    ExpiryDate = s.ExpiryDate,
                    IsExpired = s.IsExpired,
                    UnitCost = s.PurchasePrice
                })
            };
        }

        public async Task<Crystal_Clinic_Mgm.Domain.Entities.Result> PerformStockTakeAsync(StockTakeRequest request, CancellationToken cancellationToken = default)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                var stock = await _context.Stocks.FindAsync(new object[] { request.StockId }, cancellationToken);
                if (stock == null)
                {
                    return Crystal_Clinic_Mgm.Domain.Entities.Result.Fail("Stock batch not found");
                }

                var difference = request.ActualQuantity - stock.QuantityRemaining;

                if (difference == 0)
                {
                    return Crystal_Clinic_Mgm.Domain.Entities.Result.Success(); // No adjustment needed
                }

                var movementType = difference > 0 ? MovementType.In : MovementType.Out;
                var movementReason = MovementReason.Adjustment;

                var movementRequest = new MovementRequest
                {
                    ItemId = stock.ItemId ?? 0,
                    StockId = stock.StockId,
                    Quantity = Math.Abs(difference),
                    Type = movementType,
                    Reason = movementReason,
                    ReferenceId = $"STOCKTAKE-{request.StockId}",
                    Notes = $"Stock take adjustment: {request.Reason}. {request.Notes}",
                    ProcessedBy = request.ProcessedBy ?? _loggedInUser.Id
                };

                var result = await RegisterMovementAsync(movementRequest, cancellationToken);
                return result.Success ? Crystal_Clinic_Mgm.Domain.Entities.Result.Success() : Crystal_Clinic_Mgm.Domain.Entities.Result.Fail(result.ErrorMessage ?? "Stock take failed");
            });
        }

        public async Task<IEnumerable<InventoryKit>> GetAvailableKitsAsync(int? branchId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.InventoryKits.Where(k => k.IsActive && !k.IsDeleted);

            if (branchId.HasValue)
            {
                query = query.Where(k => k.BranchId == branchId.Value);
            }

            return await query
                .Include(k => k.KitLines)
                .ThenInclude(kl => kl.Item)
                .ToListAsync(cancellationToken);
        }

        public async Task<KitConsumptionResult> ConsumeKitAsync(int kitId, int quantity, string referenceId, CancellationToken cancellationToken = default)
        {
            var kit = await _context.InventoryKits
                .Include(k => k.KitLines)
                .ThenInclude(kl => kl.Item)
                .FirstOrDefaultAsync(k => k.Id == kitId && k.IsActive && !k.IsDeleted, cancellationToken);

            if (kit == null)
            {
                return new KitConsumptionResult
                {
                    Success = false,
                    ErrorMessage = "Kit not found or inactive"
                };
            }

            var movements = new List<MovementResult>();
            decimal totalCost = 0;

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                foreach (var kitLine in kit.KitLines)
                {
                    var requiredQuantity = kitLine.Quantity * quantity;

                    var movementRequest = new MovementRequest
                    {
                        ItemId = kitLine.ItemId,
                        Quantity = requiredQuantity,
                        Type = MovementType.Out,
                        Reason = MovementReason.SaleDeduction,
                        ReferenceId = referenceId,
                        Notes = $"Kit consumption: {kit.KitName}"
                    };

                    var result = await RegisterMovementAsync(movementRequest, cancellationToken);
                    if (!result.Success)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return new KitConsumptionResult
                        {
                            Success = false,
                            ErrorMessage = $"Failed to consume {kitLine.Item?.Name}: {result.ErrorMessage}"
                        };
                    }

                    movements.Add(result);
                    totalCost += result.TotalCost;
                }

                // Publish kit consumption event
                var consumedItems = new List<ConsumedItem>();
                for (int i = 0; i < kit.KitLines.Count; i++)
                {
                    var kitLine = kit.KitLines.ElementAt(i);
                    var movement = movements[i];

                    consumedItems.Add(new ConsumedItem
                    {
                        ItemId = kitLine.ItemId,
                        ItemName = kitLine.Item?.Name ?? "",
                        Quantity = movement.Quantity,
                        UnitCost = movement.UnitCost,
                        TotalCost = movement.TotalCost
                    });
                }

                var kitEvent = new InventoryKitConsumedEvent
                {
                    KitId = kitId,
                    KitName = kit.KitName,
                    Quantity = quantity,
                    TotalCost = totalCost,
                    ReferenceId = referenceId,
                    ConsumedItems = consumedItems
                };

                await _mediator.Publish(kitEvent, cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new KitConsumptionResult
                {
                    Success = true,
                    ItemMovements = movements,
                    TotalCost = totalCost
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                _logger.LogError(ex, "Error consuming kit {KitId}", kitId);

                return new KitConsumptionResult
                {
                    Success = false,
                    ErrorMessage = "Failed to consume kit"
                };
            }
        }

        // Private helper methods
        private async Task<(bool IsValid, string? ErrorMessage)> ValidateMovementRequest(MovementRequest request)
        {
            if (request.Quantity <= 0)
                return (false, "Quantity must be positive");

            var item = await _context.Items.FindAsync(request.ItemId);
            if (item == null || !item.IsActive)
                return (false, "Item not found or inactive");

            if (request.Type == MovementType.Out)
            {
                var stockLevel = await GetStockLevelAsync(request.ItemId, request.BranchId);
                if (stockLevel.AvailableQuantity < request.Quantity)
                    return (false, "Insufficient stock available");
            }

            return (true, null);
        }



        private async Task PublishMovementEvent(StockMovement movement, CancellationToken cancellationToken)
        {
            // Publish domain events for financial integration
            var _event = new InventoryStockAdjustedEvent
            {
                ItemId = movement.ItemId,
                StockId = movement.StockId,
                Quantity = movement.Quantity,
                MovementType = movement.MovementType,
                UnitCost = movement.UnitCost,
                TotalCost = movement.TotalCost,
                ReferenceId = movement.ReferenceId
            };

            await _mediator.Publish(_event, cancellationToken);
        }

        private async Task<AvailabilityCheckResult> CheckItemsAvailability(IEnumerable<ReservationItem> items, CancellationToken cancellationToken)
        {
            var shortages = new List<ShortageItem>();

            foreach (var item in items)
            {
                var stockLevel = await GetStockLevelAsync(item.ItemId, _loggedInUser.BranchId, cancellationToken);
                if (stockLevel.AvailableQuantity < item.Quantity)
                {
                    shortages.Add(new ShortageItem
                    {
                        ItemId = item.ItemId,
                        RequestedQuantity = item.Quantity,
                        AvailableQuantity = stockLevel.AvailableQuantity,
                        ItemName = stockLevel.ItemName
                    });
                }
            }

            return new AvailabilityCheckResult
            {
                IsAvailable = !shortages.Any(),
                Shortages = shortages
            };
        }

        private async Task<IEnumerable<ReservedItem>> ReserveItemLots(int reservationId, ReservationItem item, CancellationToken cancellationToken)
        {
            var reservedItems = new List<ReservedItem>();
            var remainingToReserve = item.Quantity;

            var availableBatches = await _context.Stocks
                .Where(s => s.ItemId == item.ItemId && s.QuantityRemaining > 0 && !s.IsExpired && !s.IsDeleted && s.BranchId == _loggedInUser.BranchId)
                .OrderBy(s => s.PurchaseDate)
                .ToListAsync(cancellationToken);

            foreach (var batch in availableBatches)
            {
                if (remainingToReserve <= 0) break;

                var reserveFromBatch = Math.Min(remainingToReserve, batch.QuantityRemaining);

                var reservedItem = new ReservedItem
                {
                    ReservationId = reservationId,
                    ItemId = item.ItemId,
                    StockId = batch.StockId,
                    ReservedQuantity = reserveFromBatch,
                    UnitCost = batch.PurchasePrice
                };

                _context.ReservedItems.Add(reservedItem);
                batch.QuantityRemaining -= reserveFromBatch;
                _context.Stocks.Update(batch);
                await _context.SaveChangesAsync(cancellationToken);

                reservedItems.Add(reservedItem);
                remainingToReserve -= reserveFromBatch;
            }

            return reservedItems;
        }

        private async Task<IEnumerable<ReservedLot>> GetReservedLots(int reservationId, CancellationToken cancellationToken)
        {
            return await _context.ReservedItems
                .Where(ri => ri.ReservationId == reservationId)
                .Select(ri => new ReservedLot
                {
                    StockId = ri.StockId,
                    ItemId = ri.ItemId,
                    ReservedQuantity = ri.ReservedQuantity,
                    UnitCost = ri.UnitCost
                })
                .ToListAsync(cancellationToken);
        }

        private async Task<decimal> GetReservedQuantity(int itemId, int? branchId, CancellationToken cancellationToken)
        {
            var query = _context.ReservedItems
                .Where(ri => ri.ItemId == itemId);

            if (branchId.HasValue)
            {
                query = query.Where(ri => ri.Reservation!.BranchId == branchId.Value);
            }

            return await query.SumAsync(ri => ri.ReservedQuantity, cancellationToken);
        }

        private async Task<(List<MultiLotMovementInfo>, decimal, decimal)> ProcessMultiLotMovement(
            MovementRequest request, CancellationToken cancellationToken)
        {
            var movements = new List<MultiLotMovementInfo>();
            decimal remainingToProcess = request.Quantity;
            decimal totalCostValue = 0;

            if (request.StockId.HasValue)
            {
                var stock = await _context.Stocks.FindAsync(new object[] { request.StockId.Value }, cancellationToken);
                if (stock == null || stock.IsExpired)
                {
                    return (movements, 0, 0);
                }

                decimal moveQuantity = request.Type == MovementType.Out 
                    ? Math.Min(remainingToProcess, stock.QuantityRemaining)
                    : remainingToProcess;

                if (moveQuantity > 0)
                {
                    if (request.Type == MovementType.Out)
                    {
                        stock.QuantityRemaining -= moveQuantity;
                    }
                    else
                    {
                        stock.QuantityRemaining += moveQuantity;
                    }

                    movements.Add(new MultiLotMovementInfo
                    {
                        StockId = stock.StockId,
                        LotNumber = stock.LotNumber,
                        MovedQuantity = moveQuantity,
                        UnitCost = stock.PurchasePrice,
                        RemainingAfter = stock.QuantityRemaining
                    });

                    _context.Stocks.Update(stock);
                    await _context.SaveChangesAsync(cancellationToken);

                    totalCostValue += moveQuantity * stock.PurchasePrice;
                }

                return (movements, moveQuantity, stock.PurchasePrice);
            }

            var batches = await _context.Stocks
                .Where(s => s.ItemId == request.ItemId && !s.IsDeleted && !s.IsExpired && s.QuantityRemaining > 0)
                .Where(s => !request.BranchId.HasValue || s.BranchId == request.BranchId.Value)
                .OrderBy(s => s.PurchaseDate)
                .ToListAsync(cancellationToken);

            if (!batches.Any())
            {
                return (movements, 0, 0);
            }

            foreach (var batch in batches)
            {
                if (remainingToProcess <= 0) break;

                decimal moveQuantity = request.Type == MovementType.Out
                    ? Math.Min(remainingToProcess, batch.QuantityRemaining)
                    : remainingToProcess;

                if (request.Type == MovementType.Out)
                {
                    batch.QuantityRemaining -= moveQuantity;
                }
                else
                {
                    batch.QuantityRemaining += moveQuantity;
                }

                movements.Add(new MultiLotMovementInfo
                {
                    StockId = batch.StockId,
                    LotNumber = batch.LotNumber,
                    MovedQuantity = moveQuantity,
                    UnitCost = batch.PurchasePrice,
                    RemainingAfter = batch.QuantityRemaining
                });

                _context.Stocks.Update(batch);
                totalCostValue += moveQuantity * batch.PurchasePrice;
                remainingToProcess -= moveQuantity;
            }

            await _context.SaveChangesAsync(cancellationToken);

            decimal totalQuantity = movements.Sum(m => m.MovedQuantity);
            decimal weightedAverageCost = totalQuantity > 0 ? totalCostValue / totalQuantity : 0;

            return (movements, totalQuantity, weightedAverageCost);
        }

        public async Task<ReservationDetail> GetReservationDetailsAsync(int reservationId, CancellationToken cancellationToken = default)
        {
            var reservation = await _context.InventoryReservations
                .Include(r => r.ReservedItems)
                .FirstOrDefaultAsync(r => r.Id == reservationId, cancellationToken);

            if (reservation == null)
            {
                return new ReservationDetail();
            }

            var reservedItems = await _context.ReservedItems
                .Where(ri => ri.ReservationId == reservationId)
                .Join(_context.Stocks,
                    ri => ri.StockId,
                    s => s.StockId,
                    (ri, s) => new { ReservedItem = ri, Stock = s })
                .Join(_context.Items,
                    x => x.ReservedItem.ItemId,
                    i => i.ItemId,
                    (x, i) => new ReservedItemDetail
                    {
                        ItemId = x.ReservedItem.ItemId,
                        ItemName = i.Name,
                        StockId = x.ReservedItem.StockId,
                        LotNumber = x.Stock.LotNumber,
                        ReservedQuantity = x.ReservedItem.ReservedQuantity,
                        UnitCost = x.ReservedItem.UnitCost,
                        TotalCost = x.ReservedItem.ReservedQuantity * x.ReservedItem.UnitCost
                    })
                .ToListAsync(cancellationToken);

            return new ReservationDetail
            {
                Id = reservation.Id,
                VisitId = reservation.VisitId,
                ServiceId = reservation.ServiceId,
                Status = reservation.Status.ToString(),
                ExpiresAt = reservation.ExpiresAt,
                RequestedBy = reservation.RequestedBy,
                CreatedOn = reservation.CreatedOn,
                ReservedItems = reservedItems
            };
        }

        public async Task<IEnumerable<ReservationSummary>> ListActiveReservationsAsync(int? branchId = null, CancellationToken cancellationToken = default)
        {
            var query = _context.InventoryReservations
                .Where(r => r.Status == Crystal_Clinic_Mgm.Domain.Entities.BranchStock.ReservationStatus.Active);

            if (branchId.HasValue)
            {
                query = query.Where(r => r.BranchId == branchId.Value);
            }

            var reservations = await query.ToListAsync(cancellationToken);
            var result = new List<ReservationSummary>();

            foreach (var reservation in reservations)
            {
                var reservedItems = await _context.ReservedItems
                    .Where(ri => ri.ReservationId == reservation.Id)
                    .ToListAsync(cancellationToken);

                var totalValue = reservedItems.Sum(ri => ri.ReservedQuantity * ri.UnitCost);
                var minutesUntilExpiry = (int)(reservation.ExpiresAt - DateTime.UtcNow).TotalMinutes;

                result.Add(new ReservationSummary
                {
                    Id = reservation.Id,
                    VisitId = reservation.VisitId,
                    Status = reservation.Status.ToString(),
                    ExpiresAt = reservation.ExpiresAt,
                    MinutesUntilExpiry = Math.Max(0, minutesUntilExpiry),
                    TotalReservedItems = reservedItems.Count,
                    TotalReservedValue = totalValue,
                    RequestedBy = reservation.RequestedBy
                });
            }

            return result.OrderBy(r => r.ExpiresAt);
        }

        public async Task<IEnumerable<MovementHistoryRecord>> GetMovementHistoryAsync(int? itemId = null, int? branchId = null, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
        {
            var query = _context.StockMovements.AsQueryable();

            if (itemId.HasValue)
            {
                query = query.Where(m => m.ItemId == itemId.Value);
            }

            if (branchId.HasValue)
            {
                query = query.Where(m => m.SourceBranchId == branchId.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(m => m.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(m => m.Date <= endDate.Value.AddDays(1));
            }

            var movements = await query
                .Include(m => m.Item)
                .Include(m => m.Stock)
                .OrderByDescending(m => m.Date)
                .ToListAsync(cancellationToken);

            return movements.Select(m => new MovementHistoryRecord
            {
                MovementId = m.StockMovementId,
                ItemId = m.ItemId,
                ItemName = m.Item?.Name ?? "Unknown",
                StockId = m.StockId ?? 0,
                LotNumber = m.Stock?.LotNumber ?? "Unknown",
                Type = m.MovementType.ToString(),
                Reason = m.Reason.ToString(),
                Quantity = m.Quantity,
                UnitCost = m.UnitCost,
                TotalCost = m.TotalCost,
                ReferenceId = m.ReferenceId,
                ProcessedBy = m.ProcessedBy ?? Guid.Empty,
                ProcessedDate = m.Date,
                Notes = m.Notes ?? string.Empty
            });
        }

        public async Task<IEnumerable<ExpiringStock>> GetExpiringStockAsync(int daysUntilExpiry = 30, int? branchId = null, CancellationToken cancellationToken = default)
        {
            var futureDate = DateTime.UtcNow.AddDays(daysUntilExpiry);
            var today = DateTime.UtcNow.Date;

            var query = _context.Stocks
                .Where(s => s.ExpiryDate <= futureDate && s.ExpiryDate >= today && s.QuantityRemaining > 0 && !s.IsDeleted);

            if (branchId.HasValue)
            {
                query = query.Where(s => s.BranchId == branchId.Value);
            }

            var stocks = await query
                .Include(s => s.Item)
                .OrderBy(s => s.ExpiryDate)
                .ToListAsync(cancellationToken);

            return stocks.Select(s =>
            {
                var daysRemaining = (int)(s.ExpiryDate.Date - today).TotalDays;
                var urgency = daysRemaining <= 7 ? "Critical" : daysRemaining <= 14 ? "High" : "Warning";

                return new ExpiringStock
                {
                    StockId = s.StockId,
                    ItemId = s.ItemId ?? 0,
                    ItemName = s.Item?.Name ?? "Unknown",
                    LotNumber = s.LotNumber,
                    Quantity = s.QuantityRemaining,
                    ExpiryDate = s.ExpiryDate,
                    DaysUntilExpiry = daysRemaining,
                    Urgency = urgency,
                    TotalValue = s.QuantityRemaining * s.PurchasePrice,
                    UnitCost = s.PurchasePrice
                };
            });
        }

        public async Task<StockValuationReport> GetStockValuationReportAsync(DateTime? asOfDate = null, int? branchId = null, CancellationToken cancellationToken = default)
        {
            var valuationDate = asOfDate ?? DateTime.UtcNow;

            var query = _context.Stocks
                .Where(s => s.QuantityRemaining > 0 && !s.IsDeleted && !s.IsExpired);

            if (branchId.HasValue)
            {
                query = query.Where(s => s.BranchId == branchId.Value);
            }

            var stocks = await query
                .Include(s => s.Item)
                .GroupBy(s => s.ItemId)
                .ToListAsync(cancellationToken);

            var valuatedItems = new List<ValuatedItem>();
            decimal totalValue = 0;

            foreach (var itemGroup in stocks)
            {
                var itemId = itemGroup.Key;
                var item = itemGroup.First().Item;
                var itemName = item?.Name ?? "Unknown";

                var batches = itemGroup
                    .OrderBy(s => s.PurchaseDate)
                    .Select(s => new ValuatedBatch
                    {
                        LotNumber = s.LotNumber,
                        Quantity = s.QuantityRemaining,
                        UnitCost = s.PurchasePrice,
                        BatchValue = s.QuantityRemaining * s.PurchasePrice
                    })
                    .ToList();

                var totalQuantity = batches.Sum(b => b.Quantity);
                var totalItemValue = batches.Sum(b => b.BatchValue);
                var weightedAverageCost = totalQuantity > 0 ? totalItemValue / totalQuantity : 0;

                totalValue += totalItemValue;

                valuatedItems.Add(new ValuatedItem
                {
                    ItemId = itemId ?? 0,
                    ItemName = itemName,
                    TotalQuantity = totalQuantity,
                    TotalValue = totalItemValue,
                    WeightedAverageCost = weightedAverageCost,
                    Batches = batches
                });
            }

            return new StockValuationReport
            {
                AsOfDate = valuationDate,
                ValuationMethod = "FIFO",
                BranchId = branchId,
                Items = valuatedItems.OrderByDescending(i => i.TotalValue),
                TotalInventoryValue = totalValue,
                CurrencyCode = "PKR"
            };
        }

        private async Task<string> DiagnoseStockIssue(int itemId, int? branchId, CancellationToken cancellationToken)
        {
            var allStock = await _context.Stocks
                .Where(s => s.ItemId == itemId && !s.IsDeleted)
                .ToListAsync(cancellationToken);

            if (!allStock.Any())
            {
                return "Item not found in inventory";
            }

            var expiredCount = allStock.Count(s => s.IsExpired);
            if (expiredCount == allStock.Count)
            {
                return $"All available stock is expired ({expiredCount} batches)";
            }

            var nonExpiredStock = allStock.Where(s => !s.IsExpired).ToList();
            var zeroQuantityCount = nonExpiredStock.Count(s => s.QuantityRemaining <= 0);
            if (zeroQuantityCount == nonExpiredStock.Count)
            {
                return $"Stock quantity exhausted (0 units available)";
            }

            if (branchId.HasValue)
            {
                var wrongBranchCount = nonExpiredStock.Count(s => s.BranchId != branchId && s.QuantityRemaining > 0);
                if (wrongBranchCount > 0)
                {
                    var availableInOtherBranches = nonExpiredStock.Where(s => s.BranchId != branchId && s.QuantityRemaining > 0).Sum(s => s.QuantityRemaining);
                    return $"Stock exists in other branches ({availableInOtherBranches} units across {wrongBranchCount} batches)";
                }
            }

            return "No suitable stock batches found";
        }
    }

    public class MultiLotMovementInfo
    {
        public int StockId { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public decimal MovedQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal RemainingAfter { get; set; }
    }

    public class AvailabilityCheckResult
    {
        public bool IsAvailable { get; set; }
        public IEnumerable<ShortageItem> Shortages { get; set; } = new List<ShortageItem>();
    }


}