using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Crystal_Clinic_Mgm.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMediator mediator = null!;
        protected IMediator Mediator => mediator ?? (mediator = HttpContext.RequestServices.GetService<IMediator>()!);


    }
}
