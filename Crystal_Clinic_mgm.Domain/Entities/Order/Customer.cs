using System.ComponentModel.DataAnnotations;

namespace Crystal_Clinic_Mgm.Domain.Entities.Order
{
    public class Customer : AuditableEntity
    {
        [Key] 
        public int CustomerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public CustomerType Type { get; set; }       
        public decimal CreditLimit { get; set; }     
        public bool IsBlacklisted { get; set; }      
        public ICollection<Orders> Orders { get; set; } = [];
    }
}
