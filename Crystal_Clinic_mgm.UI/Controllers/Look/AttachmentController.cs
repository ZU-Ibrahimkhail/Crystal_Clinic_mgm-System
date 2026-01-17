using Crystal_Clinic_Mgm.Application.AssetMS.MainAssets.Commands;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.Look.Attachment;
using Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Commands.Create;
using Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Commands.Update;
using Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Queries.GetDDL;
using Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Queries.GetDetail;
using Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Queries.GetList;
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
    public async Task<Result> Create(CreateAttachmentCommand command)
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
    /// <summary>
    /// Update Attachment By ID
    /// </summary>
    /// <param name="command"></param>
    /// <param name="Id">ID is require</param>
    /// <returns></returns>
    [HttpPut("{Id:int}")]
    public async Task<Result> Update(UpdateAttachmentCommand command, int Id)
    {
       
        if (ModelState.IsValid)
        {
            command.Dto.Id = Id;
            return await Mediator.Send(command);
        }
       
    }

    /// <summary>
    /// Get Attachmnet by ID
    /// </summary>
    /// <param name="Id">ID is require </param>
    /// <returns></returns>      
    [HttpGet("GetAttachment/{Id:int}")]
    public async Task<IActionResult> GetDetail(int Id)
    {
        var model = new GetAttachmentQuery
        {
            Id = Id,
        };
        return await Mediator.Send(model);
    }
}

