namespace Crystal_Clinic_Mgm.Application.HR.HR.Report.Queries
{
    public class EmployeeReportModel
    {
        public int Id { get; set; }
        public string? firstName { get; set; }
        public string? surName { get; set; }
        public string? fullName { get; set; }
        public string? fatherName { get; set; }
        public string? grandFatherName { get; set; }
        public string? genderName { get; set; }
        public string? tazkiraNo { get; set; }
        public string? dateOfBirth { get; set; }
        public string? temporaryAddress { get; set; }
        public string? permenantAddress { get; set; }
        public string? branchName { get; set; }
        public string? ProvinceName { get; set; }
        public string? employeeHealthState { get; set; }
        public int? attendanceId { get; set; } = 0;
        public string? bloodGroup { get; set; }
        public string? personalEmail { get; set; }
        public string? officialEmail { get; set; }
        public string? phoneNumber { get; set; }
        public string? photoPath { get; set; }
        public bool? hasAccount { get; set; }
        public string? rfidNumber { get; set; }
        public bool? isCurrent { get; set; }


    }
}
