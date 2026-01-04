namespace Crystal_Clinic_Mgm.Domain.Entities.Accounting
{
    public class ServiceInventoryLink : AuditableEntity
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int InventoryItemId { get; set; }
        public decimal QuantityRequired { get; set; }
        public bool IsActive { get; set; } = true;
        public string Notes { get; set; } = string.Empty;
    }
}
