// DeleteOrderCommand.cs
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands
{
    #region Request
    public class DeleteOrderCommand : IRequest<JsonResult>
    {
        public int OrderId { get; set; }
    }
    #endregion

    #region Handler
    public class DeleteOrderHandler(
        IGenericRepositoryAsync<ERP_DbContext, Orders> orderRepo,
        IMessage message,
        IStringLocalizer<CommonValidationResource> localizer) : IRequestHandler<DeleteOrderCommand, JsonResult>
    {
        public async Task<JsonResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepo.GetDetailAsync(request.OrderId);
            if (order == null)
                return message.RecordNotFound("Order ID: " + request.OrderId);

            if (order.Status == OrderStatus.Completed || order.Status == OrderStatus.InProgress)
                return message.InternalServerError(localizer["CannotDeleteInProgressOrCompleted"]);

            await orderRepo.DeleteAsync(order, cancellationToken);
            return message.Delete(order);
        }
    }
    #endregion

    #region Validator
    public class DeleteOrderValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderValidator(IStringLocalizer<CommonValidationResource> localizer)
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage(localizer["OrderIdRequired"]);
        }
    }
    #endregion
}
