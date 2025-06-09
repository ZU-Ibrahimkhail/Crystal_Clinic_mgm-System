using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.UMS.DashBoards.UserInRolesDashBoard
{
    public class UserInRoleCommand : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string? Language { get; set; }
        //  public int? ProcessStatusID { get; set; }
    }
}
