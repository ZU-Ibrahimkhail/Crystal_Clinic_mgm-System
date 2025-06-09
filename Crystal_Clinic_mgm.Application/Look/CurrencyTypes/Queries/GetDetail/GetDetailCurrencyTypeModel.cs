using Crystal_Clinic_Mgm.Common.Localizations;
using Crystal_Clinic_Mgm.Domain.Entities.Look;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.Look.CurrencyTypes.Queries.GetDetail
{
    public class GetCurrencyTypeDetailModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public string PashtoName { get; set; } = string.Empty;
        public string DariName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;


        public static Expression<Func<CurrencyType, string, GetCurrencyTypeDetailModel>> Projection
        {
            get
            {
                Localization localize = new();
                return (tr, language) => new GetCurrencyTypeDetailModel
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
