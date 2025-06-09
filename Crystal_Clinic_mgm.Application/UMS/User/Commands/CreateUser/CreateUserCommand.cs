using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<JsonResult>
    {
        public int? EmployeeId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? UserRoles { get; set; }
        public string? AllowedBranchs { get; set; }
        public bool? IsBranchAdmin { get; set; }

    }
}
