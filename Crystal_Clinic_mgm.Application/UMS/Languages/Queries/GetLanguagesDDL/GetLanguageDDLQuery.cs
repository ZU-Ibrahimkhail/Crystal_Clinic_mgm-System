using MediatR;
using Crystal_Clinic_Mgm.Application.Common.ViewModels;

namespace Crystal_Clinic_Mgm.Application.UMS.Languages.Queries.GetLanguagesDDL
{
    public class GetLanguageDDLQuery : IRequest<List<GetDropDownGeneralModel>>
    {
    }
}