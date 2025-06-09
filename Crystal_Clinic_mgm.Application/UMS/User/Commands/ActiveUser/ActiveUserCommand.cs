using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.Application.UMS.User.Commands.ActiveUser
{
    public class ActiveUserCommand : IRequest<JsonResult>
    {
        public Guid? ID { get; set; }
        public bool IsActive { get; set; }
    }
}
