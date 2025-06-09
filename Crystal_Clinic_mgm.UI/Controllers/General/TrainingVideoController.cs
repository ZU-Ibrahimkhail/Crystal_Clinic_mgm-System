using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.RBAC;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;
using Crystal_Clinic_Mgm.Application.General.TrainingVideos.Commands.Create;
using Crystal_Clinic_Mgm.Application.General.TrainingVideos.Commands.Delete;
using Crystal_Clinic_Mgm.Application.General.TrainingVideos.Commands.Update;
using Crystal_Clinic_Mgm.Application.General.TrainingVideos.Queries.GetList;

namespace Crystal_Clinic_Mgm.UI.Controllers.General
{
    [RBAC]
    public class TrainingVideoController : BaseController
    {

        [Authorize]

        [HttpPost]
        [RequestSizeLimit(1500 * 1024 * 1024)]
        //-TrainingVideo
        public async Task<IActionResult> Create([FromForm] CreateTrainingVideoCommand command)
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
        [DisableRBAC]
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<TrainingVideoListModel>> GetList(TrainingVideoListQuery query)
        {
            return await Mediator.Send(query);
        }

        [Authorize]
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeleteTrainingVideoCommand command, int Id)
        {
            if (Id <= 0)
                return BadRequest("Not valid Id");
            if (ModelState.IsValid)
            {
                command.Id = Id;
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [Authorize]
        [HttpPut("{Id:int}")]
        [RequestSizeLimit(1500 * 1024 * 1024)]
        public async Task<IActionResult> Update([FromForm] UpdateTrainingVideoCommand command, int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("Not valid Id");
            }
            if (ModelState.IsValid)
            {
                command.id = Id;
                return await Mediator.Send(command);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}