// Updated CreateOrderHandler with Rental Availability Validation
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands.Helpers;
using Crystal_Clinic_Mgm.Common.CommonLocalizations;
using Crystal_Clinic_Mgm.Common.Message;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderCRUD.Commands
{
    #region Request
    public class CreateOrderCommand : IRequest<JsonResult>
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;

        public OrderType OrderType { get; set; }
        public DateTime? RentalStartDate { get; set; }
        public DateTime? RentalEndDate { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public List<OrderServiceDto> Services { get; set; } = new();
        public decimal? ManualTotalOverride { get; set; }
        public string PriceAdjustmentReason { get; set; } = string.Empty;
    }

    public class OrderItemDto
    {
        public int ItemId { get; set; }
        public decimal Quantity { get; set; }
        public int? UnitId { get; set; }
        public bool IsRental { get; set; }
        public int? RentalDays { get; set; }
        public decimal? ManualPriceOverride { get; set; }
    }

    public class OrderServiceDto
    {
        public int ServiceId { get; set; }
        public decimal Quantity { get; set; }
        public int Duration { get; set; }
        public DurationType DurationType { get; set; }
        public decimal? ManualPriceOverride { get; set; }
    }
    #endregion

    #region Handler
    public class CreateOrderHandler(
        IGenericRepositoryAsync<ERP_DbContext, Orders> orderRepo,
        IGenericRepositoryAsync<ERP_DbContext, Customer> customerRepo,
        ERP_DbContext context,
        CreateOrderProcessor processor,
        RentalAvailabilityValidator rentalValidator,
        IStringLocalizer<CommonValidationResource> localizer,
        IMessage message,
        ILoggedInUser loggedInUser) : IRequestHandler<CreateOrderCommand, JsonResult>
    {
        public async Task<JsonResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var validationResult = new CreateOrderValidator(localizer).Validate(request);
            if (!validationResult.IsValid)
                return message.CheckCCValidationError(validationResult.Errors);

            int customerId = request.CustomerId;
            if (customerId == 0)
            {
                if (string.IsNullOrWhiteSpace(request.CustomerName))
                    return message.InternalSystemError("Customer name is required for new customer.");

                var newCustomer = new Customer
                {
                    Name = request.CustomerName,
                    Phone = request.CustomerPhone,
                    Email = request.CustomerEmail,
                    Address = request.CustomerAddress,
                    Type = CustomerType.Regular,
                    CreditLimit = 0,
                    IsBlacklisted = false,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.Now
                };
                await customerRepo.AddAsync(newCustomer, cancellationToken);
                customerId = newCustomer.CustomerId;
            }

            // Validate rental availability
            foreach (var item in request.Items.Where(i => i.IsRental))
            {
                var itemEntity = await context.Items.FirstOrDefaultAsync(i => i.ItemId == item.ItemId);
                if (itemEntity == null)
                    return message.RecordNotFound($"Item {item.ItemId} not found.");

                bool available = await rentalValidator.IsRentalAvailableAsync(
                    itemId: item.ItemId,
                    branchId: itemEntity.BranchId,
                    requestedQuantity: item.Quantity,
                    startDate: request.RentalStartDate!.Value,
                    endDate: request.RentalEndDate!.Value);

                if (!available)
                {
                    return message.InternalSystemError($"Insufficient rental availability for item '{itemEntity.Name}' between {request.RentalStartDate:yyyy-MM-dd} and {request.RentalEndDate:yyyy-MM-dd}.");
                }
            }

            var result = await processor.ProcessOrderAsync(request);

            var order = new Orders
            {
                CustomerId = customerId,
                OrderType = request.OrderType,
                OriginalTotal = result.OriginalTotal,
                AdjustedTotal = result.FinalTotal,
                Status = OrderStatus.Pending,
                OrderItems = result.OrderItems,
                OrderServices = result.OrderServices,
                BranchId = loggedInUser.BranchId,
                Adjustments = [],
                CreatedBy = loggedInUser.Id,
                CreatedOn = DateTime.Now,
                ScheduledDate = request.RentalStartDate
            };
            order.BranchDetailId = context.BranchDetails.Where(x=>x.BranchId == order.BranchId && x.IsActive).FirstOrDefault()?.Id;
            if (loggedInUser.EmployeeId > 0) order.EmployeeId = loggedInUser.EmployeeId;
            orderRepo.SaveAsync(order, cancellationToken);

            foreach (var reservation in result.RentalReservations)
            {
                reservation.OrderId = order.OrderId;
                context.RentalReservations.Add(reservation);
            }
            foreach (var OrderItems in result.OrderItems)
            {
                OrderItems.OrderId = order.OrderId;
                context.OrderItems.Add(OrderItems);
            }
            foreach (var OrderServices in result.OrderServices)
            {
                OrderServices.OrderId = order.OrderId;
                context.OrderServices.Add(OrderServices);
            }

            await context.SaveChangesAsync(cancellationToken);


            return message.Saved(order);
        }
    }
    #endregion

    #region Validation
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator(IStringLocalizer<CommonValidationResource> localizer)
        {
            RuleFor(x => x.OrderType).IsInEnum().WithMessage(localizer["InvalidOrderType"]);

            When(x => x.CustomerId == 0, () =>
            {
                RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Customer name is required.");
            });

            When(x => x.OrderType == OrderType.Rental || x.OrderType == OrderType.Mixed, () =>
            {
                RuleFor(x => x.RentalStartDate)
                    .NotNull().WithMessage(localizer["RentalStartDateRequired"])
                    .GreaterThanOrEqualTo(DateTime.Today).WithMessage(localizer["InvalidStartDate"]);

                RuleFor(x => x.RentalEndDate)
                    .NotNull().WithMessage(localizer["RentalEndDateRequired"])
                    .GreaterThan(x => x.RentalStartDate).WithMessage(localizer["InvalidEndDate"]);
            });

            RuleFor(x => x.Items).NotEmpty().WithMessage(localizer["ItemsRequired"]);
            RuleForEach(x => x.Items).SetValidator(new OrderItemValidator(localizer));
            RuleForEach(x => x.Services).SetValidator(new OrderServiceValidator(localizer));

            When(x => x.ManualTotalOverride.HasValue, () =>
            {
                RuleFor(x => x.PriceAdjustmentReason)
                    .NotEmpty().WithMessage(localizer["AdjustmentReasonRequired"])
                    .MaximumLength(500).WithMessage(localizer["ReasonTooLong"]);
            });
        }

        public class OrderItemValidator : AbstractValidator<OrderItemDto>
        {
            public OrderItemValidator(IStringLocalizer<CommonValidationResource> localizer)
            {
                RuleFor(x => x.ItemId).NotEmpty();
                RuleFor(x => x.Quantity).GreaterThan(0);
                RuleFor(x => x.RentalDays)
                    .GreaterThan(0)
                    .When(x => x.IsRental)
                    .WithMessage(localizer["RentalDaysRequired"]);
            }
        }

        public class OrderServiceValidator : AbstractValidator<OrderServiceDto>
        {
            public OrderServiceValidator(IStringLocalizer<CommonValidationResource> localizer)
            {
                RuleFor(x => x.ServiceId).NotEmpty();
                RuleFor(x => x.Quantity).GreaterThan(0);
                RuleFor(x => x.Duration).GreaterThan(0);
                RuleFor(x => x.DurationType).IsInEnum();
            }
        }
    }
    #endregion
}
