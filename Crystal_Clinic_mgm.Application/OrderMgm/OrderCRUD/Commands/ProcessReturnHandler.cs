using Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands.Helpers;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands
{
    #region Command and Handler
    public class ProcessReturnCommand : IRequest<JsonResult>
    {
        public int OrderId { get; set; }
        public List<ReturnItemDto> ReturnedItems { get; set; } = [];
        public decimal ManualDamageFee { get; set; }
    }

    public class ProcessReturnHandler(ReturnProcessor returnProcessor, ERP_DbContext context, IMessage message) : IRequestHandler<ProcessReturnCommand, JsonResult>
    {
        public async Task<JsonResult> Handle(ProcessReturnCommand request, CancellationToken cancellationToken)
        {
            var order = await context.Orders
                                     .Include(o => o.OrderItems).ThenInclude(oi => oi.Unit)
                                     .Include(o => o.OrderItems).ThenInclude(oi => oi.Item)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync(o => o.OrderId == request.OrderId);

            if (order == null)
                return message.RecordNotFound("Failed: Order not found");

            // Step 1: Create lookup from order
            var orderedQtyMap = order.OrderItems.ToDictionary(i => i.ItemId, i => i.Quantity - i.ReturnedQuantity - i.DamagedQuantity);

            // Step 2: Instantiate validator
            var validator = new ProcessReturnCommandValidator(orderedQtyMap).Validate(request).Errors;

            if (validator.Count > 0)
                return message.CheckCCValidationError(validator);

            return new JsonResult(await returnProcessor.ProcessReturnAsync(order, request.ReturnedItems, request.ManualDamageFee));
        }
    }

    public class ProcessReturnCommandValidator : AbstractValidator<ProcessReturnCommand>
    {
        public ProcessReturnCommandValidator(Dictionary<int, decimal> orderedQuantities)
        {
            RuleFor(x => x.OrderId)
                .NotEmpty()
                .WithMessage("Order Id is requird");
            RuleFor(x => x.ManualDamageFee)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Damage fee can not be negitive");
            RuleForEach(x => x.ReturnedItems).SetValidator(new ReturnItemDtoValidator(orderedQuantities));
        }
    }
    public class ReturnItemDtoValidator : AbstractValidator<ReturnItemDto>
    {

        public ReturnItemDtoValidator(Dictionary<int, decimal> orderedQuantities)
        {
            RuleFor(x => x.ItemId)
                .Must(itemId => orderedQuantities.ContainsKey(itemId))
                .WithMessage("Invalid item. Item was not part of the order.");

            RuleFor(x => x.CleanReturnedQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Clean returned quantity cannot be negative.");

            RuleFor(x => x.DamagedQuantity)
                .GreaterThanOrEqualTo(0).When(x => x.DamagedQuantity.HasValue)
                .WithMessage("Damaged quantity cannot be negative.");

            RuleFor(x => x.KeptForExtraDaysQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Extended quantity cannot be negative.");

            RuleFor(x => x)
                .Must(x =>
                {
                    var total = x.CleanReturnedQuantity + (x.DamagedQuantity ?? 0) + x.KeptForExtraDaysQuantity;
                    var ordered = orderedQuantities.TryGetValue(x.ItemId, out var q) ? q : 0;
                    return total <= ordered;
                })
                .WithMessage("Total of clean, damaged, and extended quantities cannot exceed ordered quantity.");

            RuleFor(x => x.ExtendedDays)
                .GreaterThan(0).When(x => x.KeptForExtraDaysQuantity > 0)
                .WithMessage("Extended Days should be at least 1 day");

        }
    }

    #endregion

    #region DTOs
    public class ResultOfProcessReturn
    {
        public string Message { get; set; } = "Success!!";
        public Return? ReturnRecord { get; set; }
        public List<StockMovement> StockMovements { get; set; } = [];
    }

    public class ReturnItemDto
    {
        public int ItemId { get; set; }
        public int CleanReturnedQuantity { get; set; }
        public int WillBeAvailableAfterHours { get; set; } = 24;
        public int? DamagedQuantity { get; set; }
        public int KeptForExtraDaysQuantity { get; set; } = 0;
        public int? ExtendedDays { get; set; }


    }
    #endregion
}
