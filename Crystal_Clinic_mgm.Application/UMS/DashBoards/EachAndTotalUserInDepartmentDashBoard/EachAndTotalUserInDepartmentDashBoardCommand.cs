using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Crystal_Clinic_Mgm.Application.UMS.DashBoards.EachAndTotalUserInDepartmentDashBoard
{
    public class EachAndTotalUserInBranchDashBoardCommand : IRequest<JsonResult>
    {
        [JsonIgnore]
        public string? Language { get; set; }
    }
}
