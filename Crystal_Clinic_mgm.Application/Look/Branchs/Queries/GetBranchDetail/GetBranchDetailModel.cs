using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.Look.Branchs.Queries.GetDepartmentDetail
{
    public class GetBranchDetailModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string PashtoName { get; set; } = string.Empty;
        public string DariName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public int? ParentId { get; set; }
        public string ParentBranchName { get; set; } = string.Empty;
        public DateTime? ModifiedOn { get; set; }


        public static Expression<Func<Branch, string, GetBranchDetailModel>> Projection
        {
            get
            {
                Localization localize = new();
                return (tr, language) => new GetBranchDetailModel
                {
                    Id = tr.ID,
                    Name = localize.GetName(language, tr),
                    EnglishName = tr.EnglishName,
                    DariName = tr.DariName,
                    PashtoName = tr.PashtoName,
                    Code = tr.Code,
                    ParentBranchName = localize.GetName(language, tr.Parent),
                    ParentId = tr.ParentId,
                    ModifiedOn = tr.ModifiedOn
                };
            }
        }

    }
}
