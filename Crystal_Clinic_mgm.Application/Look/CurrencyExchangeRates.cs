using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Crystal_Clinic_Mgm.Domain.Entities.Look;

namespace Crystal_Clinic_Mgm.Application.Look.CurrencyExchangeRates
{
    public class CurrencyExchangeRateDto
    {
        public int CurrencyExchangeRateId { get; set; }
        public int FromCurrencyId { get; set; }
        public string FromCurrencyCode { get; set; } = string.Empty;
        public int ToCurrencyId { get; set; }
        public string ToCurrencyCode { get; set; } = string.Empty;
        public decimal ExchangeRate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }

    #region Create 
    public class CreateCurrencyExchangeRateCommand : IRequest<CurrencyExchangeRateDto>
    {
        public int FromCurrencyId { get; set; }
        public int ToCurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public string? Remarks { get; set; }
    }

    public class CreateCurrencyExchangeRateCommandValidator : AbstractValidator<CreateCurrencyExchangeRateCommand>
    {
        public CreateCurrencyExchangeRateCommandValidator()
        {
            RuleFor(x => x.FromCurrencyId).GreaterThan(0).WithMessage("FromCurrencyId must be greater than 0.");
            RuleFor(x => x.ToCurrencyId).GreaterThan(0).WithMessage("ToCurrencyId must be greater than 0.");
            RuleFor(x => x.ExchangeRate).GreaterThan(0).WithMessage("ExchangeRate must be greater than 0.");
            RuleFor(x => x).Must(x => x.FromCurrencyId != x.ToCurrencyId).WithMessage("FromCurrencyId and ToCurrencyId must be different.");
        }
    }

    public class CreateCurrencyExchangeRateHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateCurrencyExchangeRateCommand, CurrencyExchangeRateDto>
    {
        public async Task<CurrencyExchangeRateDto> Handle(CreateCurrencyExchangeRateCommand request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var fromCurrency = await context.CurrencyType
                    .FirstOrDefaultAsync(c => c.ID == request.FromCurrencyId, cancellationToken)
                    ?? throw new KeyNotFoundException($"FromCurrency with ID {request.FromCurrencyId} not found.");

                var toCurrency = await context.CurrencyType
                    .FirstOrDefaultAsync(c => c.ID == request.ToCurrencyId, cancellationToken)
                    ?? throw new KeyNotFoundException($"ToCurrency with ID {request.ToCurrencyId} not found.");

                var exchangeRate = new CurrencyExchangeRate
                {
                    FromCurrencyId = request.FromCurrencyId,
                    ToCurrencyId = request.ToCurrencyId,
                    ExchangeRate = request.ExchangeRate,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow,
                    IsDeleted = false,
                    Remarks = request.Remarks,
                };

                context.CurrencyExchangeRates.Add(exchangeRate);
                await context.SaveChangesAsync(cancellationToken);

                return new CurrencyExchangeRateDto
                {
                    CurrencyExchangeRateId = exchangeRate.CurrencyExchangeRateId,
                    FromCurrencyId = exchangeRate.FromCurrencyId,
                    FromCurrencyCode = fromCurrency.Code,
                    ToCurrencyId = exchangeRate.ToCurrencyId,
                    ToCurrencyCode = toCurrency.Code,
                    ExchangeRate = exchangeRate.ExchangeRate,
                    CreatedOn = exchangeRate.CreatedOn,
                    ModifiedOn = exchangeRate.ModifiedOn,
                    Remarks = exchangeRate.Remarks ?? ""
                };
            });
        }
    }
    #endregion

    #region Update 
    public class UpdateCurrencyExchangeRateCommand : IRequest<bool>
    {
        public int CurrencyExchangeRateId { get; set; }
        public int FromCurrencyId { get; set; }
        public int ToCurrencyId { get; set; }
        public decimal ExchangeRate { get; set; }
        public string? Remarks { get; set; }
    }

    public class UpdateCurrencyExchangeRateCommandValidator : AbstractValidator<UpdateCurrencyExchangeRateCommand>
    {
        public UpdateCurrencyExchangeRateCommandValidator()
        {
            RuleFor(x => x.CurrencyExchangeRateId).GreaterThan(0).WithMessage("CurrencyExchangeRateId must be greater than 0.");
            RuleFor(x => x.FromCurrencyId).GreaterThan(0).WithMessage("FromCurrencyId must be greater than 0.");
            RuleFor(x => x.ToCurrencyId).GreaterThan(0).WithMessage("ToCurrencyId must be greater than 0.");
            RuleFor(x => x.ExchangeRate).GreaterThan(0).WithMessage("ExchangeRate must be greater than 0.");
            RuleFor(x => x).Must(x => x.FromCurrencyId != x.ToCurrencyId).WithMessage("FromCurrencyId and ToCurrencyId must be different.");
        }
    }

    public class UpdateCurrencyExchangeRateHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateCurrencyExchangeRateCommand, bool>
    {
        public async Task<bool> Handle(UpdateCurrencyExchangeRateCommand request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var exchangeRate = await context.CurrencyExchangeRates
                    .FirstOrDefaultAsync(e => !e.IsDeleted && e.CurrencyExchangeRateId == request.CurrencyExchangeRateId, cancellationToken)
                    ?? throw new KeyNotFoundException($"CurrencyExchangeRate with ID {request.CurrencyExchangeRateId} not found.");

                var fromCurrency = await context.CurrencyType
                    .FirstOrDefaultAsync(c => c.ID == request.FromCurrencyId, cancellationToken)
                    ?? throw new KeyNotFoundException($"FromCurrency with ID {request.FromCurrencyId} not found.");

                var toCurrency = await context.CurrencyType
                    .FirstOrDefaultAsync(c => c.ID == request.ToCurrencyId, cancellationToken)
                    ?? throw new KeyNotFoundException($"ToCurrency with ID {request.ToCurrencyId} not found.");

                exchangeRate.FromCurrencyId = request.FromCurrencyId;
                exchangeRate.ToCurrencyId = request.ToCurrencyId;
                exchangeRate.ExchangeRate = request.ExchangeRate;
                exchangeRate.ModifiedBy = loggedInUser.Id;
                exchangeRate.ModifiedOn = DateTime.UtcNow;
                exchangeRate.Remarks = request.Remarks;

                context.CurrencyExchangeRates.Update(exchangeRate);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            });
        }
    }

    #endregion

    #region Delete 
    public class DeleteCurrencyExchangeRateCommand : IRequest<bool>
    {
        public int CurrencyExchangeRateId { get; set; }
    }

    public class DeleteCurrencyExchangeRateCommandValidator : AbstractValidator<DeleteCurrencyExchangeRateCommand>
    {
        public DeleteCurrencyExchangeRateCommandValidator()
        {
            RuleFor(x => x.CurrencyExchangeRateId).GreaterThan(0).WithMessage("CurrencyExchangeRateId must be greater than 0.");
        }
    }

    public class DeleteCurrencyExchangeRateHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<DeleteCurrencyExchangeRateCommand, bool>
    {
        public async Task<bool> Handle(DeleteCurrencyExchangeRateCommand request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var exchangeRate = await context.CurrencyExchangeRates
                    .FirstOrDefaultAsync(e => !e.IsDeleted && e.CurrencyExchangeRateId == request.CurrencyExchangeRateId, cancellationToken)
                    ?? throw new KeyNotFoundException($"CurrencyExchangeRate with ID {request.CurrencyExchangeRateId} not found.");

                exchangeRate.IsDeleted = true;
                exchangeRate.ModifiedBy = loggedInUser.Id;
                exchangeRate.ModifiedOn = DateTime.UtcNow;

                context.CurrencyExchangeRates.Update(exchangeRate);
                await context.SaveChangesAsync(cancellationToken);
                return true;
            });
        }
    }
    #endregion

    #region Get Rate By Id 
    public class GetCurrencyExchangeRateByIdQuery : IRequest<CurrencyExchangeRateDto>
    {
        public int CurrencyExchangeRateId { get; set; }
    }

    public class GetCurrencyExchangeRateByIdQueryValidator : AbstractValidator<GetCurrencyExchangeRateByIdQuery>
    {
        public GetCurrencyExchangeRateByIdQueryValidator()
        {
            RuleFor(x => x.CurrencyExchangeRateId).GreaterThan(0).WithMessage("CurrencyExchangeRateId must be greater than 0.");
        }
    }

    public class GetCurrencyExchangeRateByIdHandler(ERP_DbContext context) : IRequestHandler<GetCurrencyExchangeRateByIdQuery, CurrencyExchangeRateDto>
    {
        public async Task<CurrencyExchangeRateDto> Handle(GetCurrencyExchangeRateByIdQuery request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                var exchangeRate = await context.CurrencyExchangeRates
                    .Include(e => e.FromCurrency)
                    .Include(e => e.ToCurrency)
                    .FirstOrDefaultAsync(e => !e.IsDeleted && e.CurrencyExchangeRateId == request.CurrencyExchangeRateId, cancellationToken)
                    ?? throw new KeyNotFoundException($"CurrencyExchangeRate with ID {request.CurrencyExchangeRateId} not found.");

                return new CurrencyExchangeRateDto
                {
                    CurrencyExchangeRateId = exchangeRate.CurrencyExchangeRateId,
                    FromCurrencyId = exchangeRate.FromCurrencyId,
                    FromCurrencyCode = exchangeRate.FromCurrency.Code,
                    ToCurrencyId = exchangeRate.ToCurrencyId,
                    ToCurrencyCode = exchangeRate.ToCurrency.Code,
                    ExchangeRate = exchangeRate.ExchangeRate,
                    CreatedOn = exchangeRate.CreatedOn,
                    ModifiedOn = exchangeRate.ModifiedOn,
                    Remarks = exchangeRate.Remarks ?? ""
                };
            });
        }
    }
    #endregion

    #region Get All Rates 
    public class GetAllCurrencyExchangeRatesQuery : IRequest<List<CurrencyExchangeRateDto>> 
    {
        public DateTime date { get; set; }
    }

    public class GetAllCurrencyExchangeRatesHandler(ERP_DbContext context) : IRequestHandler<GetAllCurrencyExchangeRatesQuery, List<CurrencyExchangeRateDto>>
    {
        public async Task<List<CurrencyExchangeRateDto>> Handle(GetAllCurrencyExchangeRatesQuery request, CancellationToken cancellationToken)
        {
            var executionStrategy = context.Database.CreateExecutionStrategy();
            return await executionStrategy.ExecuteAsync(async () =>
            {
                return await context.CurrencyExchangeRates
                    .Include(e => e.FromCurrency)
                    .Include(e => e.ToCurrency)
                    .Where(e => !e.IsDeleted && e.CreatedOn.Date == request.date.Date)
                    .Select(e => new CurrencyExchangeRateDto
                    {
                        CurrencyExchangeRateId = e.CurrencyExchangeRateId,
                        FromCurrencyId = e.FromCurrencyId,
                        FromCurrencyCode = e.FromCurrency.Code,
                        ToCurrencyId = e.ToCurrencyId,
                        ToCurrencyCode = e.ToCurrency.Code,
                        ExchangeRate = e.ExchangeRate,
                        CreatedOn = e.CreatedOn,
                        ModifiedOn = e.ModifiedOn,
                        Remarks = e.Remarks ?? ""
                    })
                    .ToListAsync(cancellationToken);
            });
        }
    }
    #endregion

}