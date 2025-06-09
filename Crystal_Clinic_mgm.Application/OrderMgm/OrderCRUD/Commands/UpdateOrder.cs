// Final Clean UpdateOrderHandler
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands.Helpers;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands
{
    #region Request
    public class UpdateOrderCommand : IRequest<JsonResult>
    {
        public int OrderId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public List<OrderServiceDto> Services { get; set; } = new();
        public decimal? ManualTotalOverride { get; set; }
        public string PriceAdjustmentReason { get; set; } = string.Empty;
    }
    #endregion

    #region Handler
    public class UpdateOrderHandler(
        IGenericRepositoryAsync<ERP_DbContext, Orders> orderRepo,
        ERP_DbContext context,
        UpdateOrderProcessor orderProcessor,
        RentalAvailabilityValidator rentalValidator,
        IStringLocalizer<CommonValidationResource> localizer,
        IMessage message) : IRequestHandler<UpdateOrderCommand, JsonResult>
    {
        public async Task<JsonResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var existingOrder = await orderRepo.FindByCondition(x => x.OrderId == request.OrderId)
                .Include(x => x.OrderItems)
                .Include(x => x.OrderServices)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingOrder == null)
                return message.RecordNotFound("Order ID: " + request.OrderId);

            var validation = new UpdateOrderValidator(localizer).Validate(request).Errors;
            if (validation.Count != 0)
                return message.CheckCCValidationError(validation);

            var existingItems = existingOrder.OrderItems.ToDictionary(x => x.ItemId);

            // Validate rental and sellable stock
            foreach (var itemDto in request.Items)
            {
                var itemEntity = await context.Items.FirstOrDefaultAsync(i => i.ItemId == itemDto.ItemId);
                if (itemEntity == null)
                    return message.RecordNotFound($"Item {itemDto.ItemId} not found.");

                existingItems.TryGetValue(itemDto.ItemId, out var existingOrderItem);
                decimal additionalQty = existingOrderItem == null ? itemDto.Quantity : itemDto.Quantity - existingOrderItem.Quantity;

                if (itemDto.IsRental)
                {

                    if (existingOrderItem == null || itemDto.Quantity > existingOrderItem.Quantity)
                    {
                        bool available = await rentalValidator.IsRentalAvailableAsync(
                            itemDto.ItemId,
                            itemEntity.BranchId,
                            additionalQty,
                            existingOrder.ScheduledDate!.Value,
                            existingOrder.OrderItems.FirstOrDefault()?.RentalEndDate ?? existingOrder.ScheduledDate!.Value);
                        if (!available)
                            return message.InternalSystemError($"Insufficient rental availability for item '{itemEntity.Name}' during {existingOrder.ScheduledDate:yyyy-MM-dd}.");
                    }
                }
                else
                {
                    if (existingOrderItem == null || itemDto.Quantity > existingOrderItem.Quantity)
                    {
                        if (itemEntity.CurrentStock < additionalQty)
                        {
                            return message.InternalSystemError($"Insufficient sellable stock for item '{itemEntity.Name}'. Available: {itemEntity.CurrentStock}");
                        }
                    }
                }

                if (existingOrder.Status == OrderStatus.InProgress)
                {

                    if (!itemDto.IsRental)
                    {
                        itemEntity.CurrentStock -= additionalQty;
                    }
                    itemEntity.RealTimeAvailableStock -= additionalQty;

                    // Optional: Log the deduction as a StockMovement
                    context.StockMovements.Add(new StockMovement
                    {
                        ItemId = itemDto.ItemId,
                        Quantity = -additionalQty,
                        MovementType = MovementType.Out,
                        Reason = MovementReason.OrderUpdate,
                        OrderId = existingOrder.OrderId,
                        ReferenceId = $"OrderUpdate-{existingOrder.OrderId}",
                        Date = DateTime.Now,
                        Notes = "Extra quantity deducted after order update"
                    });
                }

            }

            // Pass to Processor for update + cleanup
            var result = await orderProcessor.ProcessOrderAsync(request, existingOrder);

            existingOrder.OriginalTotal = result.OriginalTotal;
            existingOrder.AdjustedTotal = result.FinalTotal;
            existingOrder.ModifiedOn = DateTime.Now;
            existingOrder.BranchDetailId ??= context.BranchDetails.Where(x => x.BranchId == existingOrder.BranchId && x.IsActive).FirstOrDefault()?.Id;

            await orderRepo.UpdateAsync(existingOrder, cancellationToken);
            return message.Update(existingOrder);
        }
    }
    #endregion

    #region Validation
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator(IStringLocalizer<CommonValidationResource> localizer)
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.Items).NotEmpty().WithMessage(localizer["ItemsRequired"]);
            RuleForEach(x => x.Items).SetValidator(new CreateOrderValidator.OrderItemValidator(localizer));
            RuleForEach(x => x.Services).SetValidator(new CreateOrderValidator.OrderServiceValidator(localizer));

            When(x => x.ManualTotalOverride.HasValue, () =>
            {
                RuleFor(x => x.PriceAdjustmentReason)
                    .NotEmpty().WithMessage(localizer["AdjustmentReasonRequired"])
                    .MaximumLength(500).WithMessage(localizer["ReasonTooLong"]);
            });
        }
    }
    #endregion
}
