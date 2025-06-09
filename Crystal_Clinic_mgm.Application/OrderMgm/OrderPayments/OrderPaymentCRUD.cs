// OrderPayment CRUD Commands & Queries
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities.Order;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.OrderMgm.OrderPayments
{
    #region CreateOrderPayment
    public class CreateOrderPaymentCommand : IRequest<int>
    {
        public int OrderId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; }
        public string TransactionReference { get; set; } = string.Empty;
        public bool IsAdvancePayment { get; set; }
    }

    public class CreateOrderPaymentHandler(ERP_DbContext context,ILoggedInUser loggedInUser) : IRequestHandler<CreateOrderPaymentCommand, int>
    {
        public async Task<int> Handle(CreateOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            var order = context.Orders.Where(x => x.OrderId == request.OrderId).FirstOrDefault();
            if (order == null) {
                throw new KeyNotFoundException(nameof(order));
            }
            if (request.Amount > order.AdjustedTotal - order.PaidAmount)
            {
                throw new Exception("Amount can not be greater than remaining of order !!");
            }
            var payment = new OrderPayment
            {
                OrderId = request.OrderId,
                PaymentDate = request.PaymentDate,
                Amount = request.Amount,
                Method = request.Method,
                TransactionReference = request.TransactionReference,
                IsAdvancePayment = request.IsAdvancePayment
            };

            context.OrderPayments.Add(payment);

                order.PaidAmount += request.Amount;
                order.ModifiedBy = loggedInUser.Id;
                order.ModifiedOn = DateTime.Now;
            await context.SaveChangesAsync(cancellationToken);
            return payment.PaymentId;
        }
    }
    #endregion

    #region DeleteOrderPayment
    public class DeleteOrderPaymentCommand : IRequest<bool>
    {
        public int PaymentId { get; set; }
    }

    public class DeleteOrderPaymentHandler(ERP_DbContext context) : IRequestHandler<DeleteOrderPaymentCommand, bool>
    {
        public async Task<bool> Handle(DeleteOrderPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await context.OrderPayments.FindAsync(request.PaymentId);
            if (payment == null) return false;

            context.OrderPayments.Remove(payment);
            var order = context.Orders.Where(x => x.OrderId == payment.OrderId).FirstOrDefault();
            if (order != null && order.PaidAmount >=  payment.Amount)
            {
                order.PaidAmount -= payment.Amount;
                context.Orders.Update(order);
            }
            await context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
    #endregion

    #region GetOrderPaymentsByOrderId
    public class GetOrderPaymentsByOrderIdQuery : IRequest<List<OrderPaymentDto>>
    {
        public int OrderId { get; set; }
    }

    public class OrderPaymentDto
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod Method { get; set; }
        public string TransactionReference { get; set; } = string.Empty;
        public bool IsAdvancePayment { get; set; }
    }

    public class GetOrderPaymentsByOrderIdHandler(ERP_DbContext context) : IRequestHandler<GetOrderPaymentsByOrderIdQuery, List<OrderPaymentDto>>
    {
        public async Task<List<OrderPaymentDto>> Handle(GetOrderPaymentsByOrderIdQuery request, CancellationToken cancellationToken)
        {
            return await context.OrderPayments
                .Where(p => p.OrderId == request.OrderId)
                .OrderBy(p => p.PaymentDate)
                .Select(p => new OrderPaymentDto
                {
                    PaymentId = p.PaymentId,
                    Amount = p.Amount,
                    PaymentDate = p.PaymentDate,
                    Method = p.Method,
                    TransactionReference = p.TransactionReference,
                    IsAdvancePayment = p.IsAdvancePayment
                })
                .ToListAsync(cancellationToken);
        }
    }
    #endregion

    #region ListAllOrderPayments (Optional Admin)
    public class ListAllOrderPaymentsQuery : IRequest<List<OrderPaymentDto>>
    {
        public int? LastPaymentId { get; set; }
        public int? OrderId { get; set; }
        public int PageSize { get; set; } = 20;
    }

    public class ListAllOrderPaymentsHandler(ERP_DbContext context) : IRequestHandler<ListAllOrderPaymentsQuery, List<OrderPaymentDto>>
    {
        public async Task<List<OrderPaymentDto>> Handle(ListAllOrderPaymentsQuery request, CancellationToken cancellationToken)
        {
            var query = context.OrderPayments.AsQueryable();

            if (request.LastPaymentId.HasValue)
            {
                query = query.Where(p => p.PaymentId > request.LastPaymentId.Value);
            }
            if (request.OrderId.HasValue)
            {
                query = query.Where(p=>p.OrderId == request.OrderId.Value);
            }

            query = query.OrderBy(p => p.PaymentId).Take(request.PageSize);

            return await query.Select(p => new OrderPaymentDto
            {
                PaymentId = p.PaymentId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                Method = p.Method,
                TransactionReference = p.TransactionReference,
                IsAdvancePayment = p.IsAdvancePayment
            }).ToListAsync(cancellationToken);
        }
    }
    #endregion
}
