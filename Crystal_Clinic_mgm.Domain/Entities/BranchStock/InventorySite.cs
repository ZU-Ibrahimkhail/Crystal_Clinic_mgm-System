using System.ComponentModel.DataAnnotations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class InventorySite : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public string SiteCode { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public bool IsActive { get; set; } = true;
        public string Address { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // Navigation properties
        public Branch? Branch { get; set; }
        public ICollection<Stock> Stocks { get; set; } = new List<Stock>();
    }
}