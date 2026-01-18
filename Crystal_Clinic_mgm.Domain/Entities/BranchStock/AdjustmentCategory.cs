using System.ComponentModel.DataAnnotations;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class AdjustmentCategory : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool AffectsFinancials { get; set; } // Whether adjustment creates journal entries
        public bool RequiresApproval { get; set; }
        public bool IsActive { get; set; } = true;
        public int? ApprovalLevel { get; set; } // 1=Supervisor, 2=Manager, 3=Director

        // Navigation properties
        public ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
}