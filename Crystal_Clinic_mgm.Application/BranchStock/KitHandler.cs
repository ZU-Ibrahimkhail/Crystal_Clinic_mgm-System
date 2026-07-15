using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.Events;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Crystal_Clinic_Mgm.Application.BranchStock
{
   
    #region Create Kit
    public class CreateKitCommand : IRequest<Result>
    {
        public CreateKitRequest Dto { get; set; } = null!;
    }
    public class CreateKitCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateKitCommand, Result>
    {
        public async Task<Result> Handle(CreateKitCommand request, CancellationToken cancellationToken)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var kit = new InventoryKit
                    {
                        KitName = request.Dto.KitName,
                        BranchId = request.Dto.BranchId,
                        IsActive = true,
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.UtcNow,
                        KitLines = request.Dto.Lines.Select(line => new InventoryKitLine
                        {
                            ItemId = line.ItemId,
                            Quantity = line.Quantity,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        }).ToList()
                    };

                    await context.InventoryKits.AddAsync(kit, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return Domain.Entities.Result.Success("Inventory kit created successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Domain.Entities.Result.Fail("Failed to create inventory kit");
                }
            });
        }
    }
    #endregion

    #region Update Kit
    public class UpdateKitCommand : IRequest<Result>
    {
        public UpdateKitRequest Dto { get; set; } = null!;
    }
    public class UpdateKitCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateKitCommand, Result>
    {
        public async Task<Result> Handle(UpdateKitCommand request, CancellationToken cancellationToken)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var kit = await context.InventoryKits.FirstOrDefaultAsync(x => x.Id == request.Dto.Id, cancellationToken);
                    if (kit == null)
                    {
                        return Domain.Entities.Result.Fail("Kit not found");
                    }

                    kit.KitName = request.Dto.KitName;
                    kit.Description = request.Dto.Description;
                    kit.IsFreeForPatient = request.Dto.IsFreeForPatient;
                    kit.IsActive = request.Dto.IsActive;
                    kit.BranchId = request.Dto.BranchId;

                    var existingLines = context.InventoryKitLines.Where(x => x.KitId == request.Dto.Id);
                    context.InventoryKitLines.RemoveRange(existingLines);

                    kit.KitLines = request.Dto.Lines.Select(l => new InventoryKitLine
                    {
                        KitId = kit.Id,
                        ItemId = l.ItemId,
                        Quantity = l.Quantity,
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.UtcNow
                    }).ToList();

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync(cancellationToken);
                    return Domain.Entities.Result.Success("Kit updated successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Domain.Entities.Result.Fail("Failed to update kit");
                }
            });
        }
    }
    #endregion

    #region Delete Kit By Id
    public class DeleteKitCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class DeleteKitCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<DeleteKitCommand, Result>
    {
        public async Task<Result> Handle(DeleteKitCommand request, CancellationToken cancellationToken)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var kit = await context.InventoryKits.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                    if (kit == null)
                    {
                        return Domain.Entities.Result.Fail("Kit not found");
                    }
                    kit.IsDeleted = true;
                    kit.ModifiedBy = loggedInUser.Id;
                    kit.ModifiedOn = DateTime.UtcNow;
                    await context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return Domain.Entities.Result.Success("Kit deleted successfully");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Domain.Entities.Result.Fail("Failed to delete kit");
                }
            });
        }
    }
    #endregion

    #region Get Kit by Id
    public class GetKitByIdQuery : IRequest<Result>
    {
        public int Id { get; set; }
    }
    public class GetKitByIdQueryHandler(ERP_DbContext context) : IRequestHandler<GetKitByIdQuery, Result>
    {
        public async Task<Result> Handle(GetKitByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var kit = await context.InventoryKits
                .Include(k => k.KitLines)
                .ThenInclude(kl => kl.Item)
                .FirstOrDefaultAsync(k => k.Id == request.Id && !k.IsDeleted, cancellationToken);

                if (kit == null)
                {
                    return Domain.Entities.Result.Fail("Kit not found");
                }

                var kitDto = new InventoryKitDto
                {
                    Id = kit.Id,
                    KitName = kit.KitName,
                    Description = kit.Description,
                    IsFreeForPatient = kit.IsFreeForPatient,
                    IsActive = kit.IsActive,
                    BranchId = kit.BranchId,
                    Lines = kit.KitLines.Select(kl => new KitLines
                    {
                        KitId = kl.KitId,
                        ItemId = kl.ItemId,
                        Quantity = kl.Quantity
                    }).ToList()
                };

                return Domain.Entities.Result.Success(kitDto);
            }
            catch (Exception ex)
            {
                return Domain.Entities.Result.Fail("Error retrieving kit: " + ex.Message);
            }
        }
    }
    #endregion

    #region Get Kits List
    public class GetKitListQuery : IRequest<Result>
    {
        public int? BranchId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetKitListQueryHandler(ERP_DbContext context) : IRequestHandler<GetKitListQuery, Result>
    {
        public async Task<Result> Handle(GetKitListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var query = context.InventoryKits
                    .Where(k => k.IsActive && !k.IsDeleted)
                    .Include(k => k.KitLines)
                    .AsQueryable();

                if (request.BranchId.HasValue)
                {
                    query = query.Where(k => k.BranchId == request.BranchId.Value);
                }

                query = query.OrderBy(k => k.Id).Take(request.PageSize);

                var kits = await query.Select(k => new InventoryKitDto
                {
                    Id = k.Id,
                    KitName = k.KitName,
                    Description = k.Description,
                    IsFreeForPatient = k.IsFreeForPatient,
                    IsActive = k.IsActive,
                    BranchId = k.BranchId,
                    Lines = k.KitLines.Select(kl => new KitLines
                    {
                        KitId = kl.KitId,
                        ItemId = kl.ItemId,
                        Quantity = kl.Quantity
                    }).ToList()
                })
                .ToListAsync(cancellationToken);
                return Result.Success(kits);
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error retrieving Kits: {ex.Message}");
            }
        }
    }

    #endregion

    #region Consume Kit
    public class ConsumeKitCommand : IRequest<KitConsumptionResult>
    {
        public int KitId { get; set; }
        public int Quantity { get; set; } = 1;
        public string ReferenceId { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public int VisitKitId { get;  set; }
    }

    public class ConsumeKitCommandHandler(
        ERP_DbContext context, 
        ILoggedInUser loggedInUser, 
        IMediator mediator, 
        ILogger<IKitService> _logger,
        IInventoryService inventoryService) : IRequestHandler<ConsumeKitCommand, KitConsumptionResult>
    {
        public async Task<KitConsumptionResult> Handle(ConsumeKitCommand request, CancellationToken cancellationToken)
        {
            var kit = await context.VisitKits
               .Include(k => k.InventoryKit)
               .ThenInclude(kl => kl.KitLines)
               .ThenInclude(kl => kl.Item)
               .FirstOrDefaultAsync(k => k.Id == request.VisitKitId && k.InventoryKit.IsActive && !k.InventoryKit.IsDeleted, cancellationToken);

            if (kit == null)
            {
                return new KitConsumptionResult
                {
                    Success = false,
                    ErrorMessage = "Kit not found or inactive"
                };
            }

            var strategy = context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                var movements = new List<MovementResult>();
                decimal totalCost = 0;

                await using var transaction =
                    await context.Database.BeginTransactionAsync(cancellationToken);

                try
                {
                    foreach (var kitLine in kit.InventoryKit.KitLines)
                    {
                        var requiredQuantity = kitLine.Quantity * request.Quantity;

                        var movementRequest = new MovementRequest
                        {
                            ItemId = kitLine.ItemId,
                            Quantity = requiredQuantity,
                            BranchId = 1,
                            Type = MovementType.Out,
                            Reason = MovementReason.SaleDeduction,
                            ReferenceId = request.ReferenceId,
                            Notes = $"Kit consumption: {kit.InventoryKit.KitName}"
                        };

                        var result = await inventoryService.RegisterMovementAsync(movementRequest, cancellationToken);

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

                    await mediator.Publish(new InventoryKitConsumedEvent
                    {
                        KitId = request.KitId,
                        KitName = kit.InventoryKit.KitName,
                        Quantity = request.Quantity,
                        TotalCost = totalCost,
                        ReferenceId = request.ReferenceId
                    }, cancellationToken);

                    // FIX: Use ExecuteUpdateAsync instead of ForEachAsync + SaveChanges.
                    // This performs a direct SQL UPDATE without loading the entity or keeping a reader open.
                    await context.VisitKits
                        .Where(x => x.Id == request.VisitKitId)
                        .ExecuteUpdateAsync(setters => setters.SetProperty(k => k.IsConsumed, true), cancellationToken);

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

                    _logger.LogError(ex, "Error consuming kit {KitId}", request.KitId);

                    return new KitConsumptionResult
                    {
                        Success = false,
                        ErrorMessage = "Failed to consume kit"
                    };
                }
            });
        }
    }
    #endregion


    public class InventoryKitDto
    {
        public int Id { get; set; }
        public string KitName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsFreeForPatient { get; set; }
        public bool IsActive { get; set; }
        public int BranchId { get; set; }
        public bool IsInvoiceGenerated { get; set; }
        public List<KitLines> Lines { get; set; } = new List<KitLines>();
        public bool IsConsumed { get; set; }
    }

    public class CreateKitRequest
    {
        public string? KitName { get; set; }
        public string? Description { get; set; }
        public bool IsFreeForPatient { get; set; }
        public int BranchId { get; set; }
        public List<CreateKitLines>? Lines { get; set; }
    }

    public class KitLines
    {
        public int KitId { get; set; }
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
    }

    public class CreateKitLines
    {
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
    }

    public class UpdateKitRequest
    {
        public int Id { get; set; }
        public string? KitName { get; set; }
        public string? Description { get; set; }
        public int BranchId { get; set; }
        public bool IsFreeForPatient { get; set; }
        public bool IsActive { get; set; }
        public List<KitLines>? Lines { get; set; }
    }
    
}
