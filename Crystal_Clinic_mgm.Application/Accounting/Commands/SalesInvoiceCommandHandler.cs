using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Accounting.Services;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Commands
{
    #region Create Sales Invoice
    public class CreateSalesInvoiceCommand : IRequest<Result>
    {
        public CreateSalesInvoiceDto Dto { get; set; } = null!;
    }

    public class CreateSalesInvoiceCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateSalesInvoiceCommand, Result>
    {
        public async Task<Result> Handle(CreateSalesInvoiceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await context.Patient
                    .FirstOrDefaultAsync(p => p.patientId == request.Dto.CustomerId, cancellationToken);

                if (customer == null)
                    return Result.Fail("Customer not found.");

                var invoiceNumber = GenerateInvoiceNumber();
                
                decimal invoiceSubtotal = 0;
                
                if (request.Dto.Lines.Any())
                {
                    foreach (var lineDto in request.Dto.Lines)
                    {
                        var baseLineAmount = lineDto.Quantity * lineDto.UnitPrice;
                        invoiceSubtotal += baseLineAmount;
                    }
                }
                
                var invoice = new SalesInvoice
                {
                    InvoiceNumber = invoiceNumber,
                    CustomerId = request.Dto.CustomerId,
                    VisitId = request.Dto.VisitId,
                    InvoiceDate = request.Dto.InvoiceDate,
                    DueDate = request.Dto.DueDate,
                    SalesArea = request.Dto.SalesArea,
                    BranchId = request.Dto.BranchId,
                    Status = SalesStatus.Draft,
                    Attachment = request.Dto.Attachment,
                    TotalAmount = invoiceSubtotal,
                    DiscountAmount = request.Dto.DiscountAmount,
                    TaxAmount = request.Dto.TaxAmount,
                    NetAmount = invoiceSubtotal - request.Dto.DiscountAmount + request.Dto.TaxAmount,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.SalesInvoices.Add(invoice);
                await context.SaveChangesAsync(cancellationToken);
                
                if (request.Dto.Lines.Any())
                {
                    foreach (var lineDto in request.Dto.Lines)
                    {
                        var baseLineAmount = lineDto.Quantity * lineDto.UnitPrice;
                        
                        var lineDiscount = lineDto.DiscountAmount;
                        var lineTax = lineDto.TaxAmount;
                        
                        if (lineDiscount < 0)
                            lineDiscount = 0;
                        
                        if (lineDiscount > baseLineAmount)
                            lineDiscount = baseLineAmount;
                        
                        if (lineTax < 0)
                            lineTax = 0;
                        
                        var calculatedLineTotal = baseLineAmount - lineDiscount + lineTax;
                        
                        var line = new SalesInvoiceLine
                        {
                            SalesInvoiceId = invoice.Id,
                            ServiceId = lineDto.ServiceId,
                            InventoryItemId = lineDto.InventoryItemId,
                            KitId = lineDto.KitId,
                            Description = lineDto.Description,
                            Quantity = lineDto.Quantity,
                            UnitPrice = lineDto.UnitPrice,
                            LineTotal = calculatedLineTotal,
                            DiscountAmount = lineDiscount,
                            TaxAmount = lineTax,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };
                        invoice.Lines.Add(line);
                    }

                    await context.SaveChangesAsync(cancellationToken);
                }


                return Result.Success(invoice.Id, $"Sales Invoice {invoiceNumber} created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Sales Invoice: {ex.Message}");
            }
        }

        private string GenerateInvoiceNumber()
        {
            return $"SI-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion

    #region Issue Sales Invoice
    public class IssueSalesInvoiceCommand : IRequest<Result>
    {
        public int SalesInvoiceId { get; set; }
    }

    public class IssueSalesInvoiceCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<IssueSalesInvoiceCommand, Result>
    {
        public async Task<Result> Handle(IssueSalesInvoiceCommand request, CancellationToken cancellationToken)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            try
            {
                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                    try
                    {
                        var invoice = await context.SalesInvoices
                            .FirstOrDefaultAsync(i => i.Id == request.SalesInvoiceId && !i.IsDeleted, cancellationToken);

                        if (invoice == null)
                            return Result.Fail("Sales Invoice not found.");

                        if (invoice.Status != SalesStatus.Draft)
                            return Result.Fail("Only draft invoices can be issued.");

                        var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                        if (companyProfile == null)
                            return Result.Fail("Company profile not configured.");

                        invoice.Status = SalesStatus.Issued;
                        invoice.ModifiedBy = loggedInUser.Id;
                        invoice.ModifiedOn = DateTime.UtcNow;
                        context.SalesInvoices.Update(invoice);

                        var ar = new AccountsReceivable
                        {
                            InvoiceNumber = invoice.InvoiceNumber,
                            CustomerId = invoice.CustomerId,
                            InvoiceDate = invoice.InvoiceDate,
                            DueDate = invoice.DueDate ?? invoice.InvoiceDate.AddDays(30),
                            InvoiceAmount = invoice.NetAmount,
                            BalanceAmount = invoice.NetAmount,
                            PaidAmount = 0,
                            Status = ARStatus.Open,
                            ChartOfAccountId = companyProfile.AccountsReceivableAccountId,
                            BranchId = invoice.BranchId,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            CurrencyRate = 1,
                            Description = $"AR for Sales Invoice {invoice.InvoiceNumber}",
                            Reference = invoice.InvoiceNumber,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };
                        context.AccountsReceivables.Add(ar);

                        var je = new JournalEntry
                        {
                            EntryNumber = LedgerPostingService.GenerateJournalEntryNumber(),
                            EntryDate = invoice.InvoiceDate,
                            Description = $"Sales Invoice issued: {invoice.InvoiceNumber}",
                            Status = JournalEntryStatus.Posted,
                            ReferenceNumber = invoice.InvoiceNumber,
                            ReferenceType = "SalesInvoice",
                            BranchId = invoice.BranchId,
                            ApprovedBy = loggedInUser.Id,
                            ApprovedDate = DateTime.UtcNow,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        je.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = companyProfile.AccountsReceivableAccountId,
                            Description = $"DR Accounts Receivable - {invoice.InvoiceNumber}",
                            DebitAmount = invoice.NetAmount,
                            CreditAmount = 0,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            Status = JournalEntryStatus.Posted,
                            ExchangeRate = 1,
                            AmountInBaseCurrency = invoice.NetAmount,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        je.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = companyProfile.SalesRevenueAccountId,
                            Description = $"CR Sales Revenue - {invoice.InvoiceNumber}",
                            DebitAmount = 0,
                            CreditAmount = invoice.NetAmount,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            Status = JournalEntryStatus.Posted,
                            ExchangeRate = 1,
                            AmountInBaseCurrency = invoice.NetAmount,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        if (je.JournalEntryLines.Sum(x => x.DebitAmount) != je.JournalEntryLines.Sum(x => x.CreditAmount))
                            throw new InvalidOperationException("Journal entry is not balanced.");

                        context.JournalEntries.Add(je);
                        await context.SaveChangesAsync(cancellationToken);

                        await LedgerPostingService.PostToGeneralLedgerAsync(context, je, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);
                        return Result.Success("Sales Invoice issued successfully.");
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error issuing Sales Invoice: {ex.Message}");
            }
        }
    }
    #endregion

    #region Record Sales Receipt
    public class RecordSalesReceiptCommand : IRequest<Result>
    {
        public CreateSalesReceiptDto Dto { get; set; } = null!;
    }

    public class RecordSalesReceiptCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<RecordSalesReceiptCommand, Result>
    {
        public async Task<Result> Handle(RecordSalesReceiptCommand request, CancellationToken cancellationToken)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            try
            {
                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                    try
                    {
                        var invoice = await context.SalesInvoices
                            .Include(i => i.Receipts)
                            .FirstOrDefaultAsync(i => i.Id == request.Dto.SalesInvoiceId && !i.IsDeleted, cancellationToken);

                        if (invoice == null)
                            return Result.Fail("Sales Invoice not found.");

                        var totalReceived = invoice.Receipts.Sum(r => r.AmountReceived) + request.Dto.AmountReceived;
                        if (totalReceived > invoice.NetAmount)
                            return Result.Fail("Receipt amount exceeds invoice total.");

                        var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                        if (companyProfile == null)
                            return Result.Fail("Company profile not configured.");

                        var receiptNumber = GenerateReceiptNumber();
                        var receipt = new SalesReceipt
                        {
                            SalesInvoiceId = request.Dto.SalesInvoiceId,
                            ReceiptNumber = receiptNumber,
                            CustomerId = invoice.CustomerId,
                            AmountReceived = request.Dto.AmountReceived,
                            PaymentMethod = request.Dto.PaymentMethod,
                            ReceiptDate = request.Dto.ReceiptDate,
                            Reference = request.Dto.Reference ?? string.Empty,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        if (totalReceived == invoice.NetAmount)
                            invoice.Status = SalesStatus.Paid;
                        else
                            invoice.Status = SalesStatus.Partial;

                        invoice.ModifiedBy = loggedInUser.Id;
                        invoice.ModifiedOn = DateTime.UtcNow;

                        context.SalesReceipts.Add(receipt);
                        context.SalesInvoices.Update(invoice);
                        await context.SaveChangesAsync(cancellationToken);

                        var ar = await context.AccountsReceivables
                            .FirstOrDefaultAsync(a => a.Reference == invoice.InvoiceNumber && !a.IsDeleted, cancellationToken);

                        if (ar != null)
                        {
                            ar.PaidAmount += request.Dto.AmountReceived;
                            ar.BalanceAmount -= request.Dto.AmountReceived;
                            ar.Status = ar.BalanceAmount <= 0 ? ARStatus.Paid : ARStatus.PartiallyPaid;
                            ar.ModifiedBy = loggedInUser.Id;
                            ar.ModifiedOn = DateTime.UtcNow;
                            context.AccountsReceivables.Update(ar);
                            await context.SaveChangesAsync(cancellationToken);
                        }

                        var paymentMethod = request.Dto.PaymentMethod;
                        
                        int cashAccountId;
                        if (paymentMethod == PaymentMethod.Cash)
                        {
                            cashAccountId = companyProfile.CashAccountId;
                        }
                        else if (paymentMethod == PaymentMethod.BankTransfer || 
                                 paymentMethod == PaymentMethod.CreditCard || 
                                 paymentMethod == PaymentMethod.Check)
                        {
                            cashAccountId = companyProfile.BankAccountId;
                        }
                        else
                        {
                            return Result.Fail($"Unsupported payment method: {paymentMethod}");
                        }

                        var je = new JournalEntry
                        {
                            EntryNumber = LedgerPostingService.GenerateJournalEntryNumber(),
                            EntryDate = request.Dto.ReceiptDate,
                            Description = $"Sales Receipt recorded: {receiptNumber}",
                            Status = JournalEntryStatus.Posted,
                            ReferenceNumber = receiptNumber,
                            ReferenceType = "SalesReceipt",
                            BranchId = invoice.BranchId,
                            SalesReceiptId = receipt.Id,
                            ApprovedBy = loggedInUser.Id,
                            ApprovedDate = DateTime.UtcNow,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        je.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = cashAccountId,
                            Description = $"DR Cash/Bank - {receiptNumber}",
                            DebitAmount = request.Dto.AmountReceived,
                            CreditAmount = 0,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            SalesReceiptId = receipt.Id,
                            Status = JournalEntryStatus.Posted,
                            ExchangeRate = 1,
                            AmountInBaseCurrency = request.Dto.AmountReceived,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        je.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = companyProfile.AccountsReceivableAccountId,
                            Description = $"CR Accounts Receivable - {receiptNumber}",
                            DebitAmount = 0,
                            CreditAmount = request.Dto.AmountReceived,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            SalesReceiptId = receipt.Id,
                            Status = JournalEntryStatus.Posted,
                            ExchangeRate = 1,
                            AmountInBaseCurrency = request.Dto.AmountReceived,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        if (je.JournalEntryLines.Sum(x => x.DebitAmount) != je.JournalEntryLines.Sum(x => x.CreditAmount))
                            throw new InvalidOperationException("Journal entry is not balanced.");

                        context.JournalEntries.Add(je);
                        await context.SaveChangesAsync(cancellationToken);

                        await LedgerPostingService.PostToGeneralLedgerAsync(context, je, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);
                        return Result.Success(receipt.Id, "Sales Receipt recorded successfully.");
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error recording Sales Receipt: {ex.Message}");
            }
        }

        private string GenerateReceiptNumber()
        {
            return $"SR-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion

    #region Delete Sales Invoice
    public class DeleteSalesInvoiceCommand : IRequest<Result>
    {
        public int SalesInvoiceId { get; set; }
    }

    public class DeleteSalesInvoiceCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<DeleteSalesInvoiceCommand, Result>
    {
        public async Task<Result> Handle(DeleteSalesInvoiceCommand request, CancellationToken cancellationToken)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            try
            {
                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                    try
                    {
                        var invoice = await context.SalesInvoices
                            .Include(i => i.Lines)
                            .FirstOrDefaultAsync(i => i.Id == request.SalesInvoiceId && !i.IsDeleted, cancellationToken);

                        if (invoice == null)
                            return Result.Fail("Sales Invoice not found.");

                        if (invoice.Status != SalesStatus.Draft)
                            return Result.Fail($"Only draft invoices can be deleted. Current status: {invoice.Status}");

                        invoice.IsDeleted = true;
                        invoice.ModifiedBy = loggedInUser.Id;
                        invoice.ModifiedOn = DateTime.UtcNow;
                        context.SalesInvoices.Update(invoice);

                        if (invoice.Lines.Any())
                        {
                            foreach (var line in invoice.Lines)
                            {
                                line.IsDeleted = true;
                                line.ModifiedBy = loggedInUser.Id;
                                line.ModifiedOn = DateTime.UtcNow;
                                context.SalesInvoiceLines.Update(line);
                            }
                        }

                        await context.SaveChangesAsync(cancellationToken);

                        var draftJournalEntries = await context.JournalEntries
                            .Where(je => je.ReferenceNumber == invoice.InvoiceNumber && 
                                        je.Status == JournalEntryStatus.Draft && 
                                        !je.IsDeleted)
                            .ToListAsync(cancellationToken);

                        foreach (var je in draftJournalEntries)
                        {
                            je.IsDeleted = true;
                            je.ModifiedBy = loggedInUser.Id;
                            je.ModifiedOn = DateTime.UtcNow;
                            context.JournalEntries.Update(je);

                            var jeLines = await context.JournalEntryLines
                                .Where(jel => jel.JournalEntryId == je.Id)
                                .ToListAsync(cancellationToken);

                            foreach (var jel in jeLines)
                            {
                                jel.IsDeleted = true;
                                jel.Status = JournalEntryStatus.Unposted;
                                jel.ModifiedBy = loggedInUser.Id;
                                jel.ModifiedOn = DateTime.UtcNow;
                                context.JournalEntryLines.Update(jel);
                            }
                        }

                        await context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);
                        return Result.Success($"Sales Invoice {invoice.InvoiceNumber} deleted successfully.");
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error deleting Sales Invoice: {ex.Message}");
            }
        }
    }
    #endregion

    #region Void Sales Invoice
    public class VoidSalesInvoiceCommand : IRequest<Result>
    {
        public int SalesInvoiceId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class VoidSalesInvoiceCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<VoidSalesInvoiceCommand, Result>
    {
        public async Task<Result> Handle(VoidSalesInvoiceCommand request, CancellationToken cancellationToken)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            try
            {
                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                    try
                    {
                        var invoice = await context.SalesInvoices
                            .Include(i => i.Lines)
                            .Include(i => i.Receipts)
                            .FirstOrDefaultAsync(i => i.Id == request.SalesInvoiceId && !i.IsDeleted, cancellationToken);

                        if (invoice == null)
                            return Result.Fail("Sales Invoice not found.");

                        if (invoice.Status == SalesStatus.Draft)
                            return Result.Fail("Draft invoices cannot be voided. Use delete instead.");

                        if (invoice.Status == SalesStatus.Void)
                            return Result.Fail("Invoice is already voided.");

                        if (invoice.Status == SalesStatus.Paid && invoice.Receipts.Any())
                            return Result.Fail("Paid invoices cannot be voided. Use refund instead.");

                        var previousStatus = invoice.Status;
                        invoice.Status = SalesStatus.Void;
                        invoice.ModifiedBy = loggedInUser.Id;
                        invoice.ModifiedOn = DateTime.UtcNow;
                        context.SalesInvoices.Update(invoice);

                        await context.SaveChangesAsync(cancellationToken);

                        var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                        if (companyProfile == null)
                            return Result.Fail("Company profile not configured.");

                        var postedJournalEntries = await context.JournalEntries
                            .Where(je => je.ReferenceNumber == invoice.InvoiceNumber &&
                                        je.Status == JournalEntryStatus.Posted &&
                                        !je.IsDeleted)
                            .ToListAsync(cancellationToken);

                        foreach (var postedJe in postedJournalEntries)
                        {
                            var reversalJe = new JournalEntry
                            {
                                EntryNumber = LedgerPostingService.GenerateJournalEntryNumber(),
                                EntryDate = DateTime.UtcNow,
                                Description = $"Reversal Entry - Void Invoice {invoice.InvoiceNumber}. Reason: {request.Reason}",
                                Status = JournalEntryStatus.Posted,
                                ReferenceNumber = invoice.InvoiceNumber,
                                ReferenceType = "VoidReversal",
                                BranchId = invoice.BranchId,
                                ApprovedBy = loggedInUser.Id,
                                ApprovedDate = DateTime.UtcNow,
                                CreatedBy = loggedInUser.Id,
                                CreatedOn = DateTime.UtcNow
                            };

                            var originalLines = await context.JournalEntryLines
                                .Where(jel => jel.JournalEntryId == postedJe.Id)
                                .ToListAsync(cancellationToken);

                            foreach (var originalLine in originalLines)
                            {
                                reversalJe.JournalEntryLines.Add(new JournalEntryLine
                                {
                                    ChartOfAccountId = originalLine.ChartOfAccountId,
                                    Description = $"Reversal - {originalLine.Description}",
                                    DebitAmount = originalLine.CreditAmount,
                                    CreditAmount = originalLine.DebitAmount,
                                    Status = JournalEntryStatus.Posted,
                                    CurrencyId = originalLine.CurrencyId,
                                    ExchangeRate = originalLine.ExchangeRate,
                                    AmountInBaseCurrency = originalLine.AmountInBaseCurrency,
                                    CreatedBy = loggedInUser.Id,
                                    CreatedOn = DateTime.UtcNow
                                });
                            }

                            context.JournalEntries.Add(reversalJe);
                        }

                        await context.SaveChangesAsync(cancellationToken);

                        foreach (var reversalJe in await context.JournalEntries
                            .Where(je => je.ReferenceType == "VoidReversal" && je.ReferenceNumber == invoice.InvoiceNumber)
                            .ToListAsync(cancellationToken))
                        {
                            await LedgerPostingService.PostToGeneralLedgerAsync(context, reversalJe, cancellationToken);
                        }

                        var arRecord = await context.AccountsReceivables
                            .FirstOrDefaultAsync(a => a.Reference == invoice.InvoiceNumber && !a.IsDeleted, cancellationToken);

                        if (arRecord != null)
                        {
                            arRecord.Status = ARStatus.Paid;
                            arRecord.BalanceAmount = 0;
                            arRecord.ModifiedBy = loggedInUser.Id;
                            arRecord.ModifiedOn = DateTime.UtcNow;
                            context.AccountsReceivables.Update(arRecord);
                        }

                        await context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);
                        return Result.Success($"Sales Invoice {invoice.InvoiceNumber} voided successfully. Reason: {request.Reason}");
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error voiding Sales Invoice: {ex.Message}");
            }
        }
    }
    #endregion

    #region Refund Sales Invoice
    public class RefundSalesInvoiceCommand : IRequest<Result>
    {
        public int SalesInvoiceId { get; set; }
        public decimal RefundAmount { get; set; }
        public string Reason { get; set; } = string.Empty;
        public int PaymentMethod { get; set; }
    }

    public class RefundSalesInvoiceCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<RefundSalesInvoiceCommand, Result>
    {
        public async Task<Result> Handle(RefundSalesInvoiceCommand request, CancellationToken cancellationToken)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            try
            {
                return await strategy.ExecuteAsync(async () =>
                {
                    using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
                    try
                    {
                        var invoice = await context.SalesInvoices
                            .Include(i => i.Receipts)
                            .FirstOrDefaultAsync(i => i.Id == request.SalesInvoiceId && !i.IsDeleted, cancellationToken);

                        if (invoice == null)
                            return Result.Fail("Sales Invoice not found.");

                        if (invoice.Status != SalesStatus.Paid && invoice.Status != SalesStatus.Partial)
                            return Result.Fail($"Only paid or partially paid invoices can be refunded. Current status: {invoice.Status}");

                        var totalReceivedAmount = invoice.Receipts.Sum(r => r.AmountReceived);
                        if (request.RefundAmount > totalReceivedAmount)
                            return Result.Fail($"Refund amount ({request.RefundAmount}) exceeds received amount ({totalReceivedAmount}).");

                        if (request.RefundAmount <= 0)
                            return Result.Fail("Refund amount must be greater than zero.");

                        var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                        if (companyProfile == null)
                            return Result.Fail("Company profile not configured.");

                        invoice.ModifiedBy = loggedInUser.Id;
                        invoice.ModifiedOn = DateTime.UtcNow;
                        if (totalReceivedAmount - request.RefundAmount == 0)
                            invoice.Status = SalesStatus.Refunded;
                        else if (totalReceivedAmount - request.RefundAmount > 0)
                            invoice.Status = SalesStatus.Partial;

                        context.SalesInvoices.Update(invoice);
                        await context.SaveChangesAsync(cancellationToken);

                        int cashAccountId = request.PaymentMethod == (int)PaymentMethod.Cash 
                            ? companyProfile.CashAccountId 
                            : companyProfile.BankAccountId;

                        var refundEntryNumber = LedgerPostingService.GenerateJournalEntryNumber();
                        var je = new JournalEntry
                        {
                            EntryNumber = refundEntryNumber,
                            EntryDate = DateTime.UtcNow,
                            Description = $"Refund processed for Invoice: {invoice.InvoiceNumber}. Reason: {request.Reason}",
                            Status = JournalEntryStatus.Posted,
                            ReferenceNumber = invoice.InvoiceNumber,
                            ReferenceType = "RefundEntry",
                            BranchId = invoice.BranchId,
                            ApprovedBy = loggedInUser.Id,
                            ApprovedDate = DateTime.UtcNow,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        };

                        je.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = companyProfile.SalesRevenueAccountId,
                            Description = $"DR Sales Revenue (Refund) - Invoice {invoice.InvoiceNumber}",
                            DebitAmount = request.RefundAmount,
                            CreditAmount = 0,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            Status = JournalEntryStatus.Posted,
                            ExchangeRate = 1,
                            AmountInBaseCurrency = request.RefundAmount,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        je.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = cashAccountId,
                            Description = $"CR Cash/Bank (Refund) - Invoice {invoice.InvoiceNumber}",
                            DebitAmount = 0,
                            CreditAmount = request.RefundAmount,
                            CurrencyId = companyProfile.BaseCurrencyId,
                            Status = JournalEntryStatus.Posted,
                            ExchangeRate = 1,
                            AmountInBaseCurrency = request.RefundAmount,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });

                        if (je.JournalEntryLines.Sum(x => x.DebitAmount) != je.JournalEntryLines.Sum(x => x.CreditAmount))
                            throw new InvalidOperationException("Journal entry is not balanced.");

                        context.JournalEntries.Add(je);
                        await context.SaveChangesAsync(cancellationToken);

                        await LedgerPostingService.PostToGeneralLedgerAsync(context, je, cancellationToken);
                        await context.SaveChangesAsync(cancellationToken);

                        var arRecord = await context.AccountsReceivables
                            .FirstOrDefaultAsync(a => a.Reference == invoice.InvoiceNumber && !a.IsDeleted, cancellationToken);

                        if (arRecord != null)
                        {
                            arRecord.PaidAmount -= request.RefundAmount;
                            arRecord.BalanceAmount += request.RefundAmount;
                            arRecord.Status = arRecord.BalanceAmount > 0 
                                ? (arRecord.BalanceAmount == arRecord.InvoiceAmount ? ARStatus.Open : ARStatus.PartiallyPaid)
                                : ARStatus.Paid;
                            arRecord.ModifiedBy = loggedInUser.Id;
                            arRecord.ModifiedOn = DateTime.UtcNow;
                            context.AccountsReceivables.Update(arRecord);
                        }

                        await context.SaveChangesAsync(cancellationToken);

                        await transaction.CommitAsync(cancellationToken);
                        return Result.Success($"Refund of {request.RefundAmount} processed successfully for Invoice {invoice.InvoiceNumber}");
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                });
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error processing refund: {ex.Message}");
            }
        }
    }
    #endregion
}
