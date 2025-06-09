using Crystal_Clinic_Mgm.Common.Constants;
using Crystal_Clinic_Mgm.Domain.Entities.HR.HRLooks;
using System.Linq.Expressions;

namespace Crystal_Clinic_Mgm.Application.HR.HRLooks.Partner.Queries.GetDetail
{
    public class GetPartnersDetailModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameInEnglish { get; set; } = string.Empty;
        public string NameInPashto { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }


        public static Expression<Func<Partners, string, GetPartnersDetailModel>> Projection
        {
            get
            {
                return (tr, language) => new GetPartnersDetailModel
                {
                    Id = tr.ID,
                    Name = language == Constants.Language.English ? tr.NameInEnglish : tr.NameInPashto,
                    NameInEnglish = tr.NameInEnglish,
                    NameInPashto = tr.NameInPashto,
                    Email = tr.Email,
                    Phone = tr.Phone,
                    CreatedOn = tr.CreatedOn,
                };
            }
        }

    }
}
