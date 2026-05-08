using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.BranchStock;

namespace Crystal_Clinic_Mgm.Application.BranchStock
{
    public interface IInventoryService
    {
        /// <summary>
        /// Registers a stock movement with atomic updates and validation
        /// </summary>
        Task<MovementResult> RegisterMovementAsync(MovementRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Reserves items for a clinic service request with idempotency
        /// </summary>
        Task<ReservationResult> ReserveItemsAsync(ReservationRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits a reservation, deducting the reserved quantities
        /// </summary>
        Task<Crystal_Clinic_Mgm.Domain.Entities.Result> CommitReservationAsync(int reservationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Releases a reservation without deduction
        /// </summary>
        Task<Crystal_Clinic_Mgm.Domain.Entities.Result> ReleaseReservationAsync(int reservationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets current stock levels for an item
        /// </summary>
        Task<StockLevel> GetStockLevelAsync(int itemId, int? branchId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Performs stock take adjustment
        /// </summary>
        Task<Crystal_Clinic_Mgm.Domain.Entities.Result> PerformStockTakeAsync(StockTakeRequest request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Consumes an inventory kit
        /// </summary>
        Task<KitConsumptionResult> ConsumeKitAsync(int kitId, int quantity, string referenceId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets reservation details by ID
        /// </summary>
        Task<ReservationDetail> GetReservationDetailsAsync(int reservationId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Lists active reservations for a branch
        /// </summary>
        Task<IEnumerable<ReservationSummary>> ListActiveReservationsAsync(int? branchId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets stock movement history with filtering
        /// </summary>
        Task<IEnumerable<MovementHistoryRecord>> GetMovementHistoryAsync(int? itemId = null, int? branchId = null, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets expiring stock within specified days
        /// </summary>
        Task<IEnumerable<ExpiringStock>> GetExpiringStockAsync(int daysUntilExpiry = 30, int? branchId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets stock valuation report
        /// </summary>
        Task<StockValuationReport> GetStockValuationReportAsync(DateTime? asOfDate = null, int? branchId = null, CancellationToken cancellationToken = default);

        Task<Result> AddKitToVisitAsync(AddKitToVisitRequest request, CancellationToken cancellationToken = default);
    }
    public class KitConsumptionResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public IEnumerable<MovementResult> ItemMovements { get; set; } = new List<MovementResult>();
        public decimal TotalCost { get; set; }
    }

    public class AddKitToVisitRequest
    {
        public int VisitId { get; set; }
        public int ServiceSessionId { get; set; }
        public int KitId { get; set; }
    }
        
    public class MovementRequest
    {
        public int ItemId { get; set; }
        public int? StockId { get; set; } // Specific batch, null for any batch
        public decimal Quantity { get; set; }
        public MovementType Type { get; set; }
        public MovementReason Reason { get; set; }
        public string? ReferenceId { get; set; }
        public string? Notes { get; set; }
        public Guid? ProcessedBy { get; set; }
        public int? BranchId { get; set; }
    }

    public class MovementResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public decimal NewBalance { get; set; }
        public int MovementId { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal Quantity { get; set; }
    }

    public class ReservationRequest
    {
        public int VisitId { get; set; }
        public int ServiceId { get; set; }
        public IEnumerable<ReservationItem> Items { get; set; } = new List<ReservationItem>();
        public string IdempotencyToken { get; set; } = string.Empty;
        public int TtlSeconds { get; set; } = 300; // 5 minutes default
        public Guid? RequestedBy { get; set; }
    }

    public class ReservationItem
    {
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
    }

    public class ReservationResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int ReservationId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public IEnumerable<ReservedLot> ReservedLots { get; set; } = new List<ReservedLot>();
        public IEnumerable<ShortageItem> Shortages { get; set; } = new List<ShortageItem>();
    }

    public class ReservedLot
    {
        public int StockId { get; set; }
        public int ItemId { get; set; }
        public decimal ReservedQuantity { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class ShortageItem
    {
        public int ItemId { get; set; }
        public decimal RequestedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public string ItemName { get; set; } = string.Empty;
    }

    public class StockLevel
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal TotalQuantity { get; set; }
        public decimal UsableQuantity { get; set; }
        public decimal ReservedQuantity { get; set; }
        public decimal AvailableQuantity { get; set; }
        public IEnumerable<StockBatch> Batches { get; set; } = new List<StockBatch>();
    }

    public class StockBatch
    {
        public int StockId { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal QuantityRemaining { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsExpired { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class StockTakeRequest
    {
        public int StockId { get; set; }
        public decimal ActualQuantity { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public Guid? ProcessedBy { get; set; }
    }

    public class ReservationDetail
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public int ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public Guid RequestedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public IEnumerable<ReservedItemDetail> ReservedItems { get; set; } = new List<ReservedItemDetail>();
    }

    public class ReservedItemDetail
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int StockId { get; set; }
        public bool IsInvoiceGenerated { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public decimal ReservedQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
    }

    public class ReservationSummary
    {
        public int Id { get; set; }
        public int VisitId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public int MinutesUntilExpiry { get; set; }
        public int TotalReservedItems { get; set; }
        public decimal TotalReservedValue { get; set; }
        public Guid RequestedBy { get; set; }
    }

    public class MovementHistoryRecord
    {
        public int MovementId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public int StockId { get; set; }
        public string LotNumber { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
        public string ReferenceId { get; set; } = string.Empty;
        public Guid ProcessedBy { get; set; }
        public DateTime ProcessedDate { get; set; }
        public string Notes { get; set; } = string.Empty;
    }

    public class ExpiringStock
    {
        public int StockId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string LotNumber { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DaysUntilExpiry { get; set; }
        public string Urgency { get; set; } = string.Empty;
        public decimal TotalValue { get; set; }
        public decimal UnitCost { get; set; }
    }

    public class StockValuationReport
    {
        public DateTime AsOfDate { get; set; }
        public string ValuationMethod { get; set; } = "FIFO";
        public int? BranchId { get; set; }
        public IEnumerable<ValuatedItem> Items { get; set; } = new List<ValuatedItem>();
        public decimal TotalInventoryValue { get; set; }
        public string CurrencyCode { get; set; } = "PKR";
    }

    public class ValuatedItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal TotalQuantity { get; set; }
        public decimal TotalValue { get; set; }
        public decimal WeightedAverageCost { get; set; }
        public IEnumerable<ValuatedBatch> Batches { get; set; } = new List<ValuatedBatch>();
    }

    public class ValuatedBatch
    {
        public string LotNumber { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal BatchValue { get; set; }
    }
}