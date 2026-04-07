using Crystal_Clinic_Mgm.Application.Accounting.DTOs;
using Crystal_Clinic_Mgm.Application.Common.Services.IRepositories;
using Crystal_Clinic_Mgm.Domain;
using Crystal_Clinic_Mgm.Domain.Entities;
using Crystal_Clinic_Mgm.Domain.Entities.Accounting;
using Crystal_Clinic_Mgm.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Crystal_Clinic_Mgm.Application.Accounting.Commands
{
    #region Create Purchase Order
    public class CreatePurchaseOrderCommand : IRequest<Result>
    {
        public CreatePurchaseOrderDto Dto { get; set; } = null!;
    }

    public class CreatePurchaseOrderCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreatePurchaseOrderCommand, Result>
    {
        public async Task<Result> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var vendor = await context.Supplier
                    .FirstOrDefaultAsync(v => v.Id == request.Dto.VendorId, cancellationToken);

                if (vendor == null)
                    return Result.Fail("Vendor not found.");

                var poNumber = GeneratePONumber();
                var po = new PurchaseOrder
                {
                    PONumber = poNumber,
                    VendorId = request.Dto.VendorId,
                    OrderDate = request.Dto.OrderDate,
                    ExpectedDeliveryDate = request.Dto.ExpectedDeliveryDate,
                    BranchId = request.Dto.BranchId,
                    Status = POStatus.Draft,
                    Attachment = request.Dto.Attachment,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };
                context.PurchaseOrders.Add(po);
                await context.SaveChangesAsync(cancellationToken);

                if (request.Dto.Lines.Any())
                {
                    foreach (var lineDto in request.Dto.Lines)
                    {
                        var line = new POLine
                        {
                            PurchaseOrderId = po.Id,
                            ItemId = lineDto.ItemId,
                            ItemDescription = lineDto.Description,
                            Quantity = lineDto.Quantity,
                            UnitPrice = lineDto.UnitPrice,
                            LineTotal = lineDto.Quantity * lineDto.UnitPrice,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.Now
                        };
                        po.Lines.Add(line);
                    }

                    po.TotalAmount = po.Lines.Sum(l => l.LineTotal);
                    context.Update(po);
                    await context.SaveChangesAsync(cancellationToken);

                }
                return Result.Success(po.Id, $"Purchase Order {poNumber} created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Purchase Order: {ex.Message}");
            }
        }

        private string GeneratePONumber()
        {
            return $"PO-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion

    #region Update Purchase Order
    public class UpdatePurchaseOrderCommand : IRequest<Result>
    {
        public UpdatePurchaseOrderDto Dto { get; set; } = null!;
    }

    public class UpdatePurchaseOrderCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdatePurchaseOrderCommand, Result>
    {
        public async Task<Result> Handle(UpdatePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var po = await context.PurchaseOrders
                    .Include(p => p.Lines)
                    .FirstOrDefaultAsync(p => p.Id == request.Dto.Id && !p.IsDeleted, cancellationToken);

                if (po == null)
                    return Result.Fail("Purchase Order not found.");

                if (po.Status != POStatus.Open)
                    return Result.Fail("Only open POs can be updated.");

                po.OrderDate = request.Dto.OrderDate;
                po.ExpectedDeliveryDate = request.Dto.ExpectedDeliveryDate;
                po.ModifiedBy = loggedInUser.Id;
                po.ModifiedOn = DateTime.UtcNow;

                context.PurchaseOrders.Update(po);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Purchase Order updated successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error updating Purchase Order: {ex.Message}");
            }
        }
    }
    #endregion

    #region Receive Purchase Order
    public class ReceivePurchaseOrderCommand : IRequest<Result>
    {
        public int PurchaseOrderId { get; set; }
    }

    public class ReceivePurchaseOrderCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<ReceivePurchaseOrderCommand, Result>
    {
        public async Task<Result> Handle(ReceivePurchaseOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var apAccount = await context.ChartOfAccounts
                    .FirstOrDefaultAsync(a => a.AccountName == "Accounts Payable" && !a.IsDeleted, cancellationToken);

                if (apAccount == null)
                    return Result.Fail("Accounts Payable account not found.");

                var po = await context.PurchaseOrders
                    .FirstOrDefaultAsync(p => p.Id == request.PurchaseOrderId && !p.IsDeleted, cancellationToken);

                if (po == null)
                    return Result.Fail("Purchase Order not found.");

                po.Status = POStatus.Received;
                po.ModifiedBy = loggedInUser.Id;
                po.ModifiedOn = DateTime.UtcNow;

                var aP = new AccountsPayable
                {
                    InvoiceNumber = GenerateInvoiceNumber(),
                    VendorId = po.VendorId,
                    PurchaseOrderId = po.Id,
                    InvoiceDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow.AddDays(30),
                    InvoiceAmount = po.TotalAmount,
                    BalanceAmount = po.TotalAmount,
                    Status = APStatus.Draft,
                    Type = APType.Purchase,
                    ChartOfAccountId = apAccount.Id,
                    CurrencyId = 1,
                    CurrencyRate = 1,
                    Attachment = null,
                    Description =  $"AP created from PO #{po.Id}",
                    Reference = po.PONumber,
                    BranchId = po.BranchId,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };


                context.AccountsPayables.Add(aP);
                context.PurchaseOrders.Update(po);
                await context.SaveChangesAsync(cancellationToken);

                return Result.Success("Purchase Order marked as received.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error receiving Purchase Order: {ex.Message}");
            }
        }
        private string GenerateInvoiceNumber()
        {
            return $"AP-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }

    #endregion

    #region Create Vendor Bill
    public class CreateVendorBillCommand : IRequest<Result>
    {
        public CreateVendorBillDto Dto { get; set; } = null!;
    }

    public class CreateVendorBillCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<CreateVendorBillCommand, Result>
    {
        public async Task<Result> Handle(CreateVendorBillCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var vendor = await context.Supplier
                    .FirstOrDefaultAsync(v => v.Id == request.Dto.VendorId, cancellationToken);

                if (vendor == null)
                    return Result.Fail("Vendor not found.");

                var billNumber = GenerateBillNumber();
                var bill = new VendorBill
                {
                    BillNumber = billNumber,
                    PurchaseOrderId = request.Dto.PurchaseOrderId,
                    paymentId = request.Dto.PaymentId,
                    VendorId = request.Dto.VendorId,
                    BillDate = request.Dto.BillDate,
                    DueDate = request.Dto.DueDate,
                    TotalAmount = request.Dto.TotalAmount,
                    BranchId = request.Dto.BranchId,
                    Status = BillStatus.Unpaid,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.VendorBills.Add(bill);


                var ap = new AccountsPayable
                {
                    VendorBillId = bill.Id,
                    InvoiceNumber = billNumber,
                    VendorId = bill.VendorId,
                    InvoiceDate = bill.BillDate,
                    DueDate = bill.DueDate,
                    InvoiceAmount = bill.TotalAmount,
                    PaidAmount = 0,
                    BalanceAmount = bill.TotalAmount,
                    Status = APStatus.Pending,
                    BranchId = bill.BranchId,
                    Description = $"AP created for Vendor Bill {bill.BillNumber}",
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.AccountsPayables.Add(ap);

                var je = new JournalEntry
                {
                    EntryNumber = $"JE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                    EntryDate = bill.BillDate,
                    Description = $"Journal Entry for Vendor Bill {bill.BillNumber}",
                    Status = JournalEntryStatus.Posted,
                    ReferenceNumber = bill.BillNumber,
                    ReferenceType = "VendorBill",
                    BranchId = bill.BranchId,
                    ApprovedBy = loggedInUser.Id,
                    ApprovedDate = DateTime.UtcNow,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);

                je.JournalEntryLines.Add(new JournalEntryLine
                {
                    ChartOfAccountId  = companyProfile.PurchaseExpenseAccountId,
                    Description = $"Debit for Vendor Bill {bill.BillNumber}",
                    DebitAmount = bill.TotalAmount,
                    CreditAmount = 0,
                    CurrencyId = companyProfile.BaseCurrencyId,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                });

                je.JournalEntryLines.Add(new JournalEntryLine
                {
                    ChartOfAccountId = companyProfile.AccountsPayableAccountId,
                    Description = $"Credit for Vendor Bill {bill.BillNumber}",
                    DebitAmount = 0,
                    CreditAmount = bill.TotalAmount,
                    CurrencyId = companyProfile.BaseCurrencyId,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                });

                context.JournalEntries.Add(je);
                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return Result.Success(bill.Id, $"Vendor Bill {billNumber} created successfully.");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error creating Vendor Bill: {ex.Message}");
            }
        }

        private string GenerateBillNumber()
        {
            return $"VB-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion

    #region Update Vendor Bill
    public class UpdateVendorBillCommand : IRequest<Result>
    {
        public UpdateVendorBillDto Dto { get; set; } = null!;
    }

    public class UpdateVendorBillCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<UpdateVendorBillCommand, Result>
    {
        public async Task<Result> Handle(UpdateVendorBillCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var bill = await context.VendorBills
                    .FirstOrDefaultAsync(b => b.Id == request.Dto.Id && !b.IsDeleted, cancellationToken);

                if (bill == null)
                    return Result.Fail("Vendor Bill not found.");

                if (bill.Status != BillStatus.Unpaid)
                    return Result.Fail("Only unpaid bills can be updated.");


                var ap = await context.AccountsPayables
                    .FirstOrDefaultAsync(a => a.VendorBillId == bill.Id && !a.IsDeleted, cancellationToken);
                
                if (ap != null && ap.PaidAmount > 0)
                    return Result.Fail("Cannot update bill after payment has been made.");

                bill.BillDate = request.Dto.BillDate;
                bill.DueDate = request.Dto.DueDate;
                bill.TotalAmount = request.Dto.TotalAmount;
                bill.ModifiedBy = loggedInUser.Id;
                bill.ModifiedOn = DateTime.UtcNow;

                context.VendorBills.Update(bill);

                if (ap != null)
                {
                    ap.InvoiceAmount = bill.TotalAmount;
                    ap.BalanceAmount = ap.InvoiceAmount - ap.PaidAmount;
                    ap.InvoiceDate = bill.BillDate;
                    ap.DueDate = bill.DueDate;
                }

                var existingJe = await context.JournalEntries
                     .Include(j => j.JournalEntryLines)
                     .Where(j =>
                         j.ReferenceNumber == bill.BillNumber &&
                         j.ReferenceType == "VendorBill" &&
                         j.Status == JournalEntryStatus.Posted)
                     .OrderByDescending(j => j.Id)
                     .FirstOrDefaultAsync(cancellationToken);

                if (existingJe != null)
                {
                    existingJe.Status = JournalEntryStatus.Voided;
                    existingJe.ModifiedBy = loggedInUser.Id;
                    existingJe.ModifiedOn = DateTime.UtcNow;

                    var reversalJe = new JournalEntry
                    {
                        EntryNumber = $"REV-{existingJe.EntryNumber}",
                        EntryDate = DateTime.UtcNow,
                        Description = $"Reversal of {existingJe.EntryNumber}",
                        Status = JournalEntryStatus.Posted,
                        ReferenceNumber = bill.BillNumber,
                        ReferenceType = "VendorBill-Reversal",
                        BranchId = bill.BranchId,
                        CreatedBy = loggedInUser.Id,
                        CreatedOn = DateTime.UtcNow
                    };

                    foreach (var line in existingJe.JournalEntryLines)
                    {
                        reversalJe.JournalEntryLines.Add(new JournalEntryLine
                        {
                            ChartOfAccountId = line.ChartOfAccountId,
                            Description = "Reversal entry",
                            DebitAmount = line.CreditAmount,   // swapped
                            CreditAmount = line.DebitAmount,   // swapped
                            CurrencyId = line.CurrencyId,
                            CreatedBy = loggedInUser.Id,
                            CreatedOn = DateTime.UtcNow
                        });
                    }
                    context.JournalEntries.Add(reversalJe);
                }


                var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                if (companyProfile == null)
                    throw new Exception("Company profile not configured");

                var newJe = new JournalEntry
                {
                    EntryNumber = $"JE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                    EntryDate = bill.BillDate,
                    Description = $"Updated Vendor Bill {bill.BillNumber}",
                    Status = JournalEntryStatus.Posted,
                    ReferenceNumber = bill.BillNumber,
                    ReferenceType = "VendorBill",
                    BranchId = bill.BranchId,
                    ApprovedBy = loggedInUser.Id,
                    ApprovedDate = DateTime.UtcNow,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                // Debit Expense
                newJe.JournalEntryLines.Add(new JournalEntryLine
                {
                    ChartOfAccountId = companyProfile.PurchaseExpenseAccountId,
                    Description = $"Debit for updated Vendor Bill {bill.BillNumber}",
                    DebitAmount = bill.TotalAmount,
                    CreditAmount = 0,
                    CurrencyId = companyProfile.BaseCurrencyId,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                });

                // Credit AP
                newJe.JournalEntryLines.Add(new JournalEntryLine
                {
                    ChartOfAccountId = companyProfile.AccountsPayableAccountId,
                    Description = $"Credit for updated Vendor Bill {bill.BillNumber}",
                    DebitAmount = 0,
                    CreditAmount = bill.TotalAmount,
                    CurrencyId = companyProfile.BaseCurrencyId,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                });

                context.JournalEntries.Add(newJe);

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
                return Result.Success("Vendor Bill updated successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Fail($"Error updating Vendor Bill: {ex.Message}");
            }
        }
    }
    #endregion
    
    #region Pay Vendor Bill
    public class PayVendorBillCommand : IRequest<Result>
    {
        public int VendorBillId { get; set; }
        public decimal PaymentAmount { get; set; }
        public int PaymentMethodId { get; set; }
        public string? Reference { get; set; }
    }

    public class PayVendorBillCommandHandler(ERP_DbContext context, ILoggedInUser loggedInUser) : IRequestHandler<PayVendorBillCommand, Result>
    {
        public async Task<Result> Handle(PayVendorBillCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var ap = await context.AccountsPayables.FirstOrDefaultAsync(a => a.VendorBillId == request.VendorBillId && !a.IsDeleted, cancellationToken);

                if (ap == null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result.Fail("Accounts Payable not found.");
                }

                if (request.PaymentAmount <= 0)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result.Fail("Invalid payment amount.");
                }

                if (request.PaymentAmount > ap.BalanceAmount)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result.Fail("Payment exceeds remaining balance.");
                }

                if (request.PaymentMethodId <= 0)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result.Fail("Invalid payment method.");
                }


                var rate = ap.CurrencyRate == 0 ? 1 : (decimal)ap.CurrencyRate;

                var payment = new Payment
                {
                    AccountsPayableId = ap.Id,
                    PaymentNumber = GeneratePaymentNumber(), 
                    PaymentDate = DateTime.UtcNow,
                    AmountPaid = request.PaymentAmount,
                    PaymentMethodId = request.PaymentMethodId,
                    Reference = request.Reference ?? $"Payment for Bill {ap.InvoiceNumber}",
                    CurrencyId = ap.CurrencyId,
                    ExchangeRate = rate,
                    AmountInBaseCurrency = request.PaymentAmount * rate,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                context.Payments.Add(payment);

                ap.PaidAmount += request.PaymentAmount;

                ap.BalanceAmount = ap.InvoiceAmount - ap.PaidAmount;

                if (ap.BalanceAmount == 0)
                {
                    ap.Status = APStatus.Paid;
                    var bill = await context.VendorBills.FirstOrDefaultAsync(b => b.Id == request.VendorBillId && !b.IsDeleted, cancellationToken);
                    if (bill == null)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return Result.Fail("Vendor Bill not found.");
                    }
                    bill.Status = BillStatus.Paid;
                }
                else
                {
                    ap.Status = APStatus.PartiallyPaid;
                }

                var companyProfile = await context.CompanyProfile.FirstOrDefaultAsync(cancellationToken);
                if (companyProfile == null)
                    throw new Exception("Company profile not configured");
                var je = new JournalEntry
                {
                    EntryNumber = $"JE-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}",
                    EntryDate = payment.PaymentDate,
                    Description = payment.Reference,
                    Status = JournalEntryStatus.Posted,
                    ReferenceNumber = payment.PaymentNumber,
                    ReferenceType = "Vendor Payment",
                    BranchId = ap.BranchId,
                    ApprovedBy = loggedInUser.Id,
                    ApprovedDate = DateTime.UtcNow,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                };

                je.JournalEntryLines.Add(new JournalEntryLine
                {
                    ChartOfAccountId = companyProfile.AccountsPayableAccountId,
                    Description = $"Debited AP for payment {payment.PaymentNumber}",
                    DebitAmount = request.PaymentAmount,
                    CreditAmount = 0,
                    CurrencyId = ap.CurrencyId,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                });

                je.JournalEntryLines.Add(new JournalEntryLine
                {
                    ChartOfAccountId = companyProfile.CashAccountId,
                    Description = $"Credit for payment {payment.PaymentNumber}",
                    DebitAmount = 0,
                    CreditAmount = request.PaymentAmount,
                    CurrencyId = ap.CurrencyId,
                    CreatedBy = loggedInUser.Id,
                    CreatedOn = DateTime.UtcNow
                });

                context.JournalEntries.Add(je);

                await context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return Result.Success("Vendor Bill marked as paid.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return Result.Fail($"Error processing payment: {ex.Message}");
            }
        }
        private string GeneratePaymentNumber()
        {
            return $"PMT-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
    #endregion
}
