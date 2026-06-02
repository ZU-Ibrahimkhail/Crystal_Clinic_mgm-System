using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;

namespace Crystal_Clinic_Mgm.Application.Accounting.DTOs
{
    public class PurchaseOrderDto
    {
        public int Id { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public int VendorId { get; set; }
        public string? VendorName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public decimal TotalAmount { get; set; }
        public POStatus Status { get; set; }
        public int? BranchId { get; set; }
        public string? BranchName { get; set; }
        public string? Attachment { get; set; } = string.Empty;
        public List<POLineDto>? Lines { get; set; }

    }

    public class CreatePurchaseOrderDto
    {
        public int VendorId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public int? BranchId { get; set; }
        public string? Attachment { get; set; } = string.Empty;
        public List<POLineDto> Lines { get; set; } = new();
    }

    public class UpdatePurchaseOrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public List<POLineDto> Lines { get; set; } = new();
    }

    public class POLineDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
        public DateTime? ItemExpiry { get; set; }
        public string? BarCode { get; set; } = string.Empty;
        public decimal? ExpectedSalePrice { get; set; }
    }

    public class VendorBillDto
    {
        public int Id { get; set; }
        public string BillNumber { get; set; } = string.Empty;
        public int? PurchaseOrderId { get; set; }
        public string PONumber { get; set; } = string.Empty;
        public int VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public BillStatus Status { get; set; }
        public int? BranchId { get; set; }
    }

    public class CreateVendorBillDto
    {
        public int? PurchaseOrderId { get; set; }
        public int VendorId { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int? BranchId { get; set; }
    }

    public class UpdateVendorBillDto
    {
        public int Id { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
