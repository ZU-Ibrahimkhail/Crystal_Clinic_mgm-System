using Microsoft.AspNetCore.Mvc;

namespace Crystal_Clinic_Mgm.UI.Controllers.Look;


public class AttachmentController : BaseController
{

    /// <summary>
    /// Create Attachment
    /// </summary>
    /// <param name="command"></param>
    /// <returns>Json Record</returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateAttachmentCommand command)
    {
        if (ModelState.IsValid)
        {
            return await Mediator.Send(command);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }
}

