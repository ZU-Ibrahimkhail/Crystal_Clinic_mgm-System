using System.Linq.Expressions;
using MEW_ERP.Common.Localizations;
using MEW_ERP.Domain.Entities.UMS;
namespace MEW_ERP.Application.DMTS.Languages.Queries.GetLanguageList
{

    public class GetLanguageListModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        Localization getLocalName = new();


        public DateTime? ModifiedOn { get; set; }
        public static Expression<Func<Language, GetLanguageListModel>> Projection
        {
            get
            {
                return language => new GetLanguageListModel
                {
                    Id = language.ID,
                    Name = getLocalName.GetName(language),// language.Name,
                    IsDeleted = language.IsDeleted

                };
            }
        }

    }
}
