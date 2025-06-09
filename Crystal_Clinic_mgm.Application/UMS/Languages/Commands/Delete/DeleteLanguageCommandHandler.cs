using MediatR;
using Microsoft.AspNetCore.Mvc;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain.Entities.UMS;
using Crystal_Clinic_Mgm.Persistence.Contexts;
namespace Crystal_Clinic_Mgm.Application.UMS.Languages.Commands.Delete
{
    public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, JsonResult>
    {
        private readonly IMessage _message;
        private readonly IGenericRepositoryAsync<UMS_DbContext, Language> _genericRepository;
        public DeleteLanguageCommandHandler(IMessage message, IGenericRepositoryAsync<UMS_DbContext, Language> genericRepository)
        {
            _message = message;
            _genericRepository = genericRepository;
        }
        public async Task<JsonResult> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
        {
            if (request.ID > 0)
            {
                var language = await _genericRepository.GetDetailAsync(request.ID);
                if (language != null && language.IsDeleted == false)
                {
                    language.IsDeleted = true;
                    return await _genericRepository.DeleteAsync(language, cancellationToken);
                }
                else
                {
                    return _message.RecordNotFound();
                }
            }
            else
            {
                return _message.IdErrorMessage(request.ID);
            }
        }
    }
}
