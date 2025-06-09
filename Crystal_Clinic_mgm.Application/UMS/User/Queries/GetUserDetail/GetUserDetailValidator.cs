using FluentValidation;

namespace Crystal_Clinic_Mgm.Application.UMS.User.Queries.GetUserDetail
{
    public class GetUserDetailValidator : AbstractValidator<GetUserDetailQuery>
    {
        public GetUserDetailValidator()
        {
            RuleFor(User => User.Id).NotNull();
        }
    }


}
