using Crystal_Clinic_Mgm.Domain.Entities.Look;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;

namespace Crystal_Clinic_Mgm.Domain.Entities.HR.HR
{
    public class EmployeeProfile : AuditableEntity
    {
        public int ID { get; set; }
        public string EnglishFirstName { get; set; } = string.Empty;
        public string PashtoFirstName { get; set; } = string.Empty;
        public string EnglishSurName { get; set; } = string.Empty;
        public string PashtoSurName { get; set; } = string.Empty;
        public string EnglishFatherName { get; set; } = string.Empty;
        public string PashtoFatherName { get; set; } = string.Empty;
        public string EnglishGrandFatherName { get; set; } = string.Empty;
        public string PashtoGrandFatherName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        public int TazkiraTypeId { get; set; }
        public string TazkiraNo { get; set; } = string.Empty;
        public string? JoldNo { get; set; } = string.Empty;
        public string? PageNo { get; set; } = string.Empty;
        public string? RegNo { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
        public string TemporaryAddress { get; set; } = string.Empty;
        public string PermenantAddress { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public int? CurrencyTypeId { get; set; }
        public CurrencyType? CurrencyType { get; set; }
        public string? BloodGroup { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public DateTime? LeaveDate { get; set; }
        public string? LeaveRemark { get; set; } = string.Empty;
        public string PersonalEmail { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EmergencyPhoneNumber { get; set; } = string.Empty;
        public string PhotoPath { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool HasAccount { get; set; } = false;

        // Phase 1 Enhancements
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int? ManagerId { get; set; }
        public EmployeeProfile? Manager { get; set; }
        public ICollection<EmployeeProfile> DirectReports { get; set; } = new List<EmployeeProfile>();

        public EmploymentStatus EmploymentStatus { get; set; } = EmploymentStatus.Active;
        public string? BankAccountNo { get; set; }
        public string? EmployeeCode { get; set; }
        public string? WorkEmail { get; set; }
        public string? PreferredPaymentMethod { get; set; }

        // Navigation properties for Phase 1
        public ICollection<PayrollContract> PayrollContracts { get; set; } = new List<PayrollContract>();
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
        public ICollection<EmployeePayrollComponent> PayrollComponents { get; set; } = new List<EmployeePayrollComponent>();
    }
}
