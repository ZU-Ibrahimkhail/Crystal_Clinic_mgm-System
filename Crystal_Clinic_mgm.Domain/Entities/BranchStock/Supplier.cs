using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crystal_Clinic_Mgm.Domain.Entities.BranchStock
{
    public class Supplier : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }

    }
}
