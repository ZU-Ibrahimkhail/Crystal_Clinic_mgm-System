using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<JsonResult>
    {
        public Guid Id { get; set; }
        public int? EmployeeId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? UserRoles { get; set; }
        public string? AllowedbranchlevelModels { get; set; }
        public bool? IsBranchAdmin { get; set; }

    }
}
