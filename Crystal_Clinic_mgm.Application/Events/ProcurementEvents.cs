using MediatR;

namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting.Events
{
    public class ProcurementPOReceivedEvent : INotification
    {
        public int PurchaseOrderId { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public int VendorId { get; set; }
        public IEnumerable<ReceivedItem> ReceivedItems { get; set; } = new List<ReceivedItem>();
        public decimal TotalReceivedValue { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }

    public class ProcurementExpenseRecordedEvent : INotification
    {
        public int ExpenseId { get; set; }
        public string ExpenseDescription { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public bool IsReimbursable { get; set; }
        public int? PatientId { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    }

    public class ReceivedItem
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal ReceivedQuantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }
    }
}