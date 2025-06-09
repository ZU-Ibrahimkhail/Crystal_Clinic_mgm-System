using MediatR;
using MEW_ERP.Application.Common.RBAC;
using MEW_ERP.Application.Common.ViewModels;
using MEW_ERP.Application.Look.Contacts.Command.Create;
using MEW_ERP.Application.Look.Contacts.Command.Delete;
using MEW_ERP.Application.Look.Contacts.Command.Update;
using MEW_ERP.Application.Look.Contacts.Queries.GetContactList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MEW_ERP.UI.Controllers.Look
{
    [Authorize]
    [RBAC]
    public class ContactController : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateContactCommand command)
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
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> Update(UpdateContactCommand command, int Id)
        {
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
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(DeletContactCommand command, int Id)
        {
            if (Id <= 0)
            {
                return BadRequest("Not valid Id !");
            }
            else
            {
                command.Id = Id;
                return await Mediator.Send(command);
            }
        }
        //search by block name and block address
        [HttpPost("GetList")]
        public async Task<ResponseDataTable<GetContactListModel>> GetList(GetContactListQuery query)
        {
            return await Mediator.Send(query);
        }


    }
}
