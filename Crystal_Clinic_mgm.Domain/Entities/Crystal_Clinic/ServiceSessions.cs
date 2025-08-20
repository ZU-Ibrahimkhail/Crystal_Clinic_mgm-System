using System.ComponentModel.DataAnnotations;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HR;

namespace Crystal_Clinic_Mgm.Domain.Entities.Crystal_Clinic
{
    public class ServiceSessions
    {
        [Key]
        public int Id { get; set; }
        public int BranchId { get; set; } = 1;
        public int visitServiceId { get; set; }
        public int visitId { get; set; }
        public int serviceId { get; set; }
        public string serviceName { get; set; } = string.Empty;
        public string patientName { get; set; } = string.Empty;
        public string contactInfo { get; set; } = string.Empty;
        public int sessionNumber { get; set; }
        public decimal PriceInAFN { get; set; }
        public bool IsImplemented { get; set; } = false;
        public DateTime? ImplementationDate { get; set; }
        public int? ImplementorEmployeeId { get; set; }
        public EmployeeProfile? ImplementorEmployee { get; set; }
    }
}
