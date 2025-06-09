using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using System.Linq.Expressions;
namespace Crystal_Clinic_Mgm.Application.UMS.Application.Queries.GetApplicationList
{
    public class GetApplicationListLookupModel
    {
        public int ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Abbrevation { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
        public string DefaultRoute { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public static Expression<Func<Applications, GetApplicationListLookupModel>> Projection
        {
            get
            {

                return (tr) => new GetApplicationListLookupModel
                {
                    Title = tr.Title,
                    Abbrevation = tr.Abbrevation,
                    IconClass = tr.IconClass,
                    DefaultRoute = tr.DefaultRoute,
                    Area = tr.Area,
                    ID = tr.ID,
                    Description = tr.Description,
                    CreatedOn = tr.ModifiedOn,
                    ModifiedOn = tr.CreatedOn
                };
            }
        }
    }
}
