namespace Crystal_Clinic_Mgm.Application.Common.Services.IRepositories
{
    public interface ILoggedInUser
    {
        Guid Id { get; }
        bool IsAuthenticated { get; }
        string UserName { get; }
        string EmpEnglishName { get; }
        string EmpDariName { get; }
        string Email { get; }
        string PhotoPath { get; }
        int BranchId { get; }
        bool IsSuperAdmin { get; }
        bool IsBranchAdmin { get; }
        int EmployeeId { get; }
        string? AllowedBranch { get; }
        string Path { get; }
        string Host { get; }
    }
}
