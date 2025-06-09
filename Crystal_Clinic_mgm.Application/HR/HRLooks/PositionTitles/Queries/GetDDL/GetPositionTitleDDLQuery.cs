using MediatR;
using Newtonsoft.Json;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.PositionTitles.Queries.GetDDL
{
    public class GetPositionTitleDDLQuery : IRequest<List<GetPositionTitleDDLModel>>
    {
        [JsonIgnore]
        public int BranchId { get; set; }
    }
}
