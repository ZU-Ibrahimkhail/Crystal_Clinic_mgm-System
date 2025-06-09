using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.DeleteUser
{
    public class DeleteUserCommand : IRequest<JsonResult>
    {
        public Guid Id { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
