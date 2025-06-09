using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.Look.LoanTypes.Queries.GetDetail
{
    public class GetLoanTypeDetailModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string PashtoName { get; set; } = string.Empty;
        public string DariName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;


        public static Expression<Func<LoanType, string, GetLoanTypeDetailModel>> Projection
        {
            get
            {
                Localization localize = new();
                return (tr, language) => new GetLoanTypeDetailModel
                {
                    Id = tr.ID,
                    Name = localize.GetName(language, tr),
                    EnglishName = tr.EnglishName,
                    DariName = tr.DariName,
                    PashtoName = tr.PashtoName,
                    Code = tr.Code,
                };
            }
        }

    }
}
