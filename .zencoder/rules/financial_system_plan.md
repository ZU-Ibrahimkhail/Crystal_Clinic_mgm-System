---
description: Comprehensive Financial System Implementation Plan
alwaysApply: true
---

# Comprehensive Financial System Implementation Plan

## Executive Summary

This document outlines a comprehensive plan to implement a robust financial accounting system for the Crystal Clinic Service Management System. The current system has basic financial tracking but lacks proper double-entry accounting, chart of accounts, general ledger, and formal financial reporting capabilities.

**Legacy Asset Module Decommissioning**: The standalone AssetMS module will be retired. Asset lifecycle (acquisition, depreciation, maintenance) migrates into this financial core, while operational consumption is handled by the enhanced inventory module. All schemas and APIs in downstream plans must reference the financial module’s fixed-asset entities instead of recreating tables.

## System Role and Integration

### Role of the Financial System
The financial system serves as the core accounting engine for the clinic, managing all monetary transactions, balances, and reporting in compliance with IFRS and IAS standards. It implements double-entry bookkeeping with a comprehensive chart of accounts, general ledger, automated journal entries, cash/bank books, and financial planning capabilities. The system handles revenue recognition, expense tracking, payroll processing, asset valuation, fixed assets management, and risk management, providing real-time financial insights and regulatory compliance.

### Interaction with Other Systems
The financial system integrates event-driven with all other modules to automate financial impacts of operational activities, incorporating IFRS/IAS compliant features like journal entries, cash/bank books, and financial planning.

#### HR System
- **Payroll Processing**: HR system generates payroll events → Financial system creates journal entries for salaries, deductions, taxes, and charges.
- **Expense Reimbursements**: Employee advances, rewards, and reimbursements trigger financial transactions.
- **Integration Points**:
  - HR payroll → Financial journal entries for compensation, overtime, and benefits.
  - Employee contracts → Financial planning for salary budgets.
  - Attendance and leaves → Financial tracking of paid time off costs.

#### Clinic System (Services/Sales/Customers)
- **Revenue and Billing**: Patient visits, services, invoices, and sales generate revenue events → Financial system records receivables, cash receipts, and taxation.
- **Expense Tracking**: Clinic operations, refunds, commissions, and discounts create expense entries.
- **Integration Points**:
  - Service completion → Automatic invoicing, AR/AP entries, and sales receipts.
  - Insurance claims → Financial tracking of receivables and payments.
  - Customer pricing → Advanced pricing levels and discounts in financial journals.

#### Inventory System
- **Cost Accounting**: Inventory purchases, transfers, adjustments, and issues trigger journal entries for inventory valuation and cost of goods sold.
- **Asset Management**: Fixed asset acquisitions, depreciation, and disposals automated through financial journals.
- **Integration Points**:
  - Stock movements → Financial updates for inventory value, COGS, and adjustments.
  - Supplier payments → AP entries linked to purchase orders and bills.
  - Inventory transfers → Inter-branch financial reconciliations.

### Relation to Assets
- **Current Assets**: Directly manages liquid assets (cash, receivables, inventory value) through account balances, cash/bank books, and daily transactions.
- **Fixed Assets**: Comprehensive fixed assets management with depreciation, risk management, and journal entries, integrated with inventory for physical asset lifecycle.
- **Liability and Equity**: Manages accounts payable, loans, equity, and shareholders through proper classification in chart of accounts and financial planning.

This creates a unified IFRS/IAS compliant financial foundation where all operational activities automatically update the books, ensuring accurate P&L, balance sheet, cash flow, and financial planning reporting.

## Current Financial Structure Analysis

### Existing Tables and Their Purpose

#### MainAccount Table
**Current Structure:**
- ID (Guid) - Primary key
- CurrencyTypeId (int) - Currency reference
- DepositDate (DateTime) - Initial deposit date
- Description (string) - Account description
- Code (string) - Account code
- OwnerUserId (Guid) - Account owner
- TotalDebitAmount (double) - Total debits
- TotalCreditAmount (double) - Total credits
- BalanceAmount (double) - Current balance
- BranchId (int, nullable) - Branch association
- ParentId (Guid, nullable) - Parent account for hierarchy

**Issues:**
- No account type classification
- No chart of accounts integration
- Basic balance tracking only
- No proper account categorization

#### AccountTracking Table
**Current Structure:**
- ID (int) - Primary key
- CurrencyTypeId (int) - Currency reference
- TransactionDate (DateTime) - Transaction date
- Description (string) - Transaction description
- UserId (Guid) - User who performed transaction
- DebitAmount (double) - Debit amount
- CreditAmount (double) - Credit amount
- BalanceAmount (double) - Running balance
- MainAccountId (Guid) - Associated account
- trackType (enum) - Transaction type (EXPENSE, WITHDRAW, DEPOSIT, PAYROLL, TRANSFER, INCOME)
- transactionStatus (enum) - Status (PENDING, APPROVED, COMPLETED, REJECTED, CANCELED)

**Issues:**
- Not a proper journal entry system
- No double-entry accounting
- Limited transaction categorization
- No general ledger integration

#### ExpenseTracking Table
**Current Structure:**
- ID (int) - Primary key
- CurrencyTypeId (int) - Currency reference
- ExpenseTypeId (int) - Expense category
- MainAccountId (Guid) - Account used
- Amount (float) - Expense amount
- Date (DateTime) - Expense date
- Description (string) - Expense description
- InvoiceNumber (string) - Invoice reference
- AttachmentPath (string) - Invoice attachment
- BranchId (int) - Branch location
- UserId (Guid) - User who recorded expense

**Issues:**
- Duplicate of AccountTracking functionality
- No integration with general ledger
- Limited expense categorization

#### PayrollTracking Table
**Current Structure:**
- ID (int) - Primary key
- EmployeeId (int) - Employee reference
- ContractDetailsId (int) - Contract reference
- PayTypeId (int, nullable) - Payment type
- BranchId (int, nullable) - Branch location
- CurrencyTypeId (int, nullable) - Currency
- Date (DateTime) - Payment date
- BaseSalary (double) - Base salary amount
- AdvanceDeduction (double) - Advance deductions
- NetSalary (double) - Net payment amount
- PayedBy (Guid, nullable) - User who processed payment
- IsPayed (bool) - Payment status

**Issues:**
- No integration with general ledger
- Manual advance deduction tracking
- No proper salary expense accounting

#### AdvancePayment Table
**Current Structure:**
- ID (int) - Primary key
- EmployeeId (int) - Employee reference
- PayTypeId (int) - Payment type
- CurrencyTypeId (int) - Currency
- MainAccountId (Guid) - Account used
- AdvanceDate (DateTime) - Advance date
- AdvanceAmount (double) - Advance amount
- RemainingBalance (double) - Remaining balance
- EachInstallmentAmount (double) - Installment amount
- PayedBy (Guid) - User who approved

**Issues:**
- No proper loan/liability accounting
- Manual balance tracking
- No integration with payroll system

#### VisitPayment Table
**Current Structure:**
- visitPaymentId (int) - Primary key
- visitId (int) - Visit reference
- serviceId (int, nullable) - Service reference
- CurrencyTypeId (int, nullable) - Currency
- ExchangeRateToAFN (decimal) - Exchange rate
- sessionNumber (int) - Session number
- amountPaid (decimal) - Amount paid
- paymentStatus (enum) - Payment status
- paymentType (enum) - Payment type (Service, Medication, General)
- paymentDate (DateTime) - Payment date
- AmountInAFN (decimal) - Amount in AFN
- RefundAmountInAFN (decimal) - Refund amount

**Issues:**
- No revenue recognition accounting
- Limited payment tracking
- No accounts receivable integration

## Proposed Financial System Architecture

### 1. Chart of Accounts Structure

#### Account Types
```csharp
public enum AccountType
{
    Asset = 1,      // Assets
    Liability = 2,  // Liabilities
    Equity = 3,     // Equity
    Revenue = 4,    // Revenue
    Expense = 5,    // Expenses
    ContraAsset = 6 // Contra Assets
}

public enum AccountCategory
{
    // Asset Categories
    CurrentAsset = 100,
    FixedAsset = 101,
    OtherAsset = 102,

    // Liability Categories
    CurrentLiability = 200,
    LongTermLiability = 201,

    // Equity Categories
    RetainedEarnings = 300,
    Capital = 301,

    // Revenue Categories
    ServiceRevenue = 400,
    OtherRevenue = 401,

    // Expense Categories
    OperatingExpense = 500,
    AdministrativeExpense = 501,
    FinancialExpense = 502
}
```

#### Chart of Accounts Hierarchy
```
1XXX - Assets
    11XX - Current Assets
        1101 - Cash and Cash Equivalents
        1102 - Accounts Receivable
        1103 - Inventory
        1104 - Prepaid Expenses
    12XX - Fixed Assets
        1201 - Property, Plant & Equipment
        1202 - Accumulated Depreciation
    13XX - Other Assets

2XXX - Liabilities
    21XX - Current Liabilities
        2101 - Accounts Payable
        2102 - Employee Advances
        2103 - Taxes Payable
    22XX - Long-term Liabilities

3XXX - Equity
    31XX - Capital
    32XX - Retained Earnings

4XXX - Revenue
    41XX - Service Revenue
    42XX - Other Revenue

5XXX - Expenses
    51XX - Operating Expenses
        5101 - Salaries & Wages
        5102 - Medical Supplies
        5103 - Utilities
    52XX - Administrative Expenses
    53XX - Financial Expenses
```

### 2. Database Schema Modifications and New Tables

#### New Tables Required

##### ChartOfAccounts Table (New)
**Purpose:** System-defined chart of accounts, initialized on system setup, read-only for most operations.

```csharp
public class ChartOfAccounts : AuditableEntity
{
    public int Id { get; set; }
    public string AccountCode { get; set; } = string.Empty; // e.g., "1101"
    public string AccountName { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public AccountCategory AccountCategory { get; set; }
    public NormalBalanceType NormalBalance { get; set; }
    public bool IsSystemAccount { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public string Description { get; set; } = string.Empty;
    public int? ParentAccountId { get; set; }
    public ChartOfAccounts? ParentAccount { get; set; }
    public ICollection<ChartOfAccounts> ChildAccounts { get; set; } = new List<ChartOfAccounts>();
}

public enum NormalBalanceType
{
    Debit,
    Credit
}
```

**Removed:** No separate Account table needed. Journal entries reference ChartOfAccounts directly, with branch and currency handled at transaction level.

##### AccountTracking Table Modifications
**New Columns to Add:**
- JournalEntryId (int, nullable) - Link to journal entry
- ChartOfAccountId (int) - Replace MainAccountId for consistency
- ReferenceNumber (string) - Transaction reference
- ReferenceType (string) - Type of reference (Invoice, Payment, etc.)

**Code Behavior:**
- Migrate to proper journal entry system
- Remove balance calculation (handled by General Ledger)
- Add proper double-entry validation

#### New Tables Required

##### JournalEntry Table
```csharp
public class JournalEntry : AuditableEntity
{
    public int Id { get; set; }
    public string EntryNumber { get; set; } = string.Empty; // Auto-generated
    public DateTime EntryDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public JournalEntryStatus Status { get; set; } = JournalEntryStatus.Unposted;
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string ReferenceNumber { get; set; } = string.Empty;
    public string ReferenceType { get; set; } = string.Empty; // INVOICE, PAYMENT, ADJUSTMENT, etc.
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public ICollection<JournalEntryLine> JournalEntryLines { get; set; } = new List<JournalEntryLine>();
}

public enum JournalEntryStatus
{
    Draft,
    Unposted,
    Posted,
    Voided
}
```

##### JournalEntryLine Table
```csharp
public class JournalEntryLine : AuditableEntity
{
    public int Id { get; set; }
    public int JournalEntryId { get; set; }
    public JournalEntry JournalEntry { get; set; } = null!;
    public int ChartOfAccountId { get; set; }
    public ChartOfAccounts ChartOfAccount { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public decimal DebitAmount { get; set; } = 0;
    public decimal CreditAmount { get; set; } = 0;
    public int? CurrencyId { get; set; }
    public CurrencyType? Currency { get; set; }
    public decimal ExchangeRate { get; set; } = 1;
    public decimal AmountInBaseCurrency { get; set; } = 0;
}
```

##### GeneralLedger Table
```csharp
public class GeneralLedger : AuditableEntity
{
    public int Id { get; set; }
    public int ChartOfAccountId { get; set; }
    public ChartOfAccounts ChartOfAccount { get; set; } = null!;
    public int JournalEntryId { get; set; }
    public JournalEntry JournalEntry { get; set; } = null!;
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public DateTime TransactionDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal DebitAmount { get; set; } = 0;
    public decimal CreditAmount { get; set; } = 0;
    public decimal Balance { get; set; } = 0; // Running balance
}
```

**Removed:** FiscalPeriod and FiscalYear tables are not needed. Reporting can use date ranges instead of predefined periods.

##### AccountsReceivable Table (Replaces VisitPayment for receivables tracking)
```csharp
public class AccountsReceivable : AuditableEntity
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty; // Auto-generated
    public int CustomerId { get; set; } // Patient ID
    public Patient? Customer { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal InvoiceAmount { get; set; }
    public decimal PaidAmount { get; set; } = 0;
    public decimal BalanceAmount { get; set; } // Calculated: InvoiceAmount - PaidAmount
    public ARStatus Status { get; set; } = ARStatus.Open;
    public int? ChartOfAccountId { get; set; } // AR account (1102 - Accounts Receivable)
    public ChartOfAccounts? ChartOfAccount { get; set; }
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int? VisitId { get; set; } // Link to visit
    public Visit? Visit { get; set; }
    public ICollection<Receipt> Receipts { get; set; } = new List<Receipt>(); // Payment records
}

public enum ARStatus
{
    Open,
    PartiallyPaid,
    Paid,
    Overdue,
    WrittenOff
}
```

##### Sales & Revenue Entities (New)
```csharp
public class SalesInvoice : AuditableEntity
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal NetAmount { get; set; }
    public SalesStatus Status { get; set; }
    public string SalesArea { get; set; } = string.Empty;
    public ICollection<SalesInvoiceLine> Lines { get; set; } = new List<SalesInvoiceLine>();
}

public class SalesReceipt : AuditableEntity
{
    public int Id { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public decimal AmountReceived { get; set; }
    public int PaymentMethodId { get; set; }
    public DateTime ReceiptDate { get; set; }
    public string Reference { get; set; } = string.Empty;
}

public enum SalesStatus { Draft, Issued, Paid, Partial, Void, Refunded }
```

##### Procurement & Payables (New)
```csharp
public class PurchaseOrder : AuditableEntity
{
    public int Id { get; set; }
    public string PONumber { get; set; } = string.Empty;
    public int VendorId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public POStatus Status { get; set; }
    public ICollection<POLine> Lines { get; set; } = new List<POLine>();
}

public class VendorBill : AuditableEntity
{
    public int Id { get; set; }
    public string BillNumber { get; set; } = string.Empty;
    public int VendorId { get; set; }
    public DateTime BillDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal TotalAmount { get; set; }
    public BillStatus Status { get; set; }
}

public enum POStatus { Open, Received, Cancelled, Closed }
public enum BillStatus { Unpaid, Partial, Paid, Overdue }
```

##### Expenses & Operations (New)
```csharpsharp
public class Expense : AuditableEntity
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public int? ClassId { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? CustomerId { get; set; } // For billable expenses
    public bool IsReimbursable { get; set; }
}
```

##### Equity & Company Organization (New)
```csharp
public class Shareholder : AuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal OwnershipPercentage { get; set; }
    public decimal TotalInvestment { get; set; }
    public decimal TotalDrawings { get; set; }
}

public class EquityTransaction : AuditableEntity
{
    public int Id { get; set; }
    public int ShareholderId { get; set; }
    public EquityTransactionType Type { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
}

public enum EquityTransactionType { Investment, Drawing, Dividend, ProfitShare }
```

##### Fixed Asset Management (New)
```csharp
public class FixedAsset : AuditableEntity
{
    public int Id { get; set; }
    public string AssetCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public decimal PurchaseValue { get; set; }
    public decimal ResidualValue { get; set; }
    public int UsefulLifeMonths { get; set; }
    public DateTime AcquisitionDate { get; set; }
    public decimal AccumulatedDepreciation { get; set; }
}
```

##### Multi-Currency & Exchange Rate Management
The financial system uses a dual-amount approach for all transactions to maintain IFRS compliance and accurate reporting in the clinic's local currency.

- **Base Currency**: The system designates one currency (e.g., AFN) as the functional currency for the General Ledger.
- **Transaction Currency**: The currency in which the actual transaction occurred (USD, EUR, etc.).
- **Exchange Rate Application**: Every `JournalEntryLine` records the `ExchangeRate` at the time of the transaction.
- **Conversion Logic**: `AmountInBaseCurrency = TransactionAmount * ExchangeRate`.
- **Entity Reuse**:
    - **`CurrencyType`**: Used to identify currencies across all modules.
    - **`CurrencyExchangeRate`**: Used to store daily or historical rates.
- **Automated Rate Fetching**: The `IFinancialService` will utilize the existing `ERP_DbContext.GetExchangeRate` method to automatically populate rates during entry creation.
- **Rate Logic**:
    - **Cross-Currency**: If the transaction currency differs from the Base Currency, the system **must** fetch and use the latest record from the `CurrencyExchangeRate` table.
    - **Base-to-Base**: If the transaction is in the Base Currency, the `ExchangeRate` defaults to **1.0** automatically.

##### Two-Phase Journal Lifecycle (Draft & Posted)
To ensure auditability and prevent accidental errors, all financial transactions follow a mandatory two-phase process:

1.  **Phase 1: Draft State (Unposted)**:
    - **Trigger**: Any operational event (Medication Scan, Lab Order, Visit Progress).
    - **Action**: A `JournalEntry` is created with `Status = JournalEntryStatus.Draft`.
    - **Impact**: These entries show up in "Pending Reports" but **do not** affect the final Balance Sheet or P&L. They can be edited or deleted if a mistake is made during the visit.

2.  **Phase 2: Posted State**:
    - **Trigger**: Official approval or the `Visit/Complete` finalization.
    - **Action**: The `JournalEntry` is moved to `Status = JournalEntryStatus.Posted`.
    - **Impact**: The amounts are moved to the `GeneralLedger` table. Once in this state, the entry is **immutable** (cannot be changed). Any corrections must be made via a "Reversal" or "Adjustment" entry.

##### Event-Driven Clinical Integration (Hybrid Alignment)
To align with the `clinic_system_enhancement_plan.md`, the financial system processes transactions in real-time as clinical steps are completed:

1.  **Medication Scanned (Real-time)**:
    - **Trigger**: Pharmacist scans QR code.
    - **Journal Entry**: `Debit: Cost of Goods Sold` | `Credit: Inventory`.
    - **Billing**: Item is added to the "Pending Charges" for the visit.

2.  **Procedure Logged (Real-time)**:
    - **Trigger**: Resource logging (`fixedAssetId`, `roomId`).
    - **Financial Impact**: System checks if the `fixedAssetId` (Fixed Asset) requires a usage fee or depreciation entry.

3.  **Lab Order Issued**:
    - **Trigger**: `labOrders` template selected.
    - **Journal Entry**: `Debit: Lab Receivables` | `Credit: Lab Revenue`.

4.  **Visit Completion (The Final Batch)**:
    - **Trigger**: `POST /api/Clinic/Visit/Complete`.
    - **Action**: Sums all "Pending Charges" (Scanned Meds + Logged Procedures + Lab Orders).
    - **Journal Entry**: `Debit: Accounts Receivable (Patient)` | `Credit: Total Service Revenue`.

##### Recurring Transaction Templates (New)
```csharp
public class RecurringJournalTemplate : AuditableEntity
{
    public int Id { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public RecurringFrequency Frequency { get; set; } // Monthly, Weekly
    public DateTime NextRunDate { get; set; }
    public ICollection<RecurringJournalLine> Lines { get; set; } = new List<RecurringJournalLine>();
}

public enum RecurringFrequency { Weekly, Monthly, Quarterly, Yearly }
```

##### AccountsPayable Table (Replaces SupplierDue for payables tracking)
```csharp
public class AccountsPayable : AuditableEntity
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty; // Supplier invoice number
    public int VendorId { get; set; } // Supplier ID
    public Supplier? Vendor { get; set; }
    public DateTime InvoiceDate { get; set; }
    public DateTime DueDate { get; set; }
    public decimal InvoiceAmount { get; set; }
    public decimal PaidAmount { get; set; } = 0;
    public decimal BalanceAmount { get; set; } // Calculated: InvoiceAmount - PaidAmount
    public APStatus Status { get; set; } = APStatus.Open;
    public int? ChartOfAccountId { get; set; } // AP account (2101 - Accounts Payable)
    public ChartOfAccounts? ChartOfAccount { get; set; }
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public ICollection<Payment> Payments { get; set; } = new List<Payment>(); // Payment records
}

public enum APStatus
{
    Open,
    PartiallyPaid,
    Paid,
    Overdue
}
```

##### Budget Table
```csharp
public class Budget : AuditableEntity
{
    public int Id { get; set; }
    public string BudgetName { get; set; } = string.Empty;
    public int FiscalYearId { get; set; }
    public FiscalYear FiscalYear { get; set; } = null!;
    public BudgetStatus Status { get; set; } = BudgetStatus.Draft;
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public ICollection<BudgetLine> BudgetLines { get; set; } = new List<BudgetLine>();
}

public enum BudgetStatus
{
    Draft,
    Submitted,
    Approved,
    Active,
    Closed
}
```

##### BudgetLine Table
```csharp
public class BudgetLine : AuditableEntity
{
    public int Id { get; set; }
    public int BudgetId { get; set; }
    public Budget Budget { get; set; } = null!;
    public int ChartOfAccountId { get; set; }
    public ChartOfAccounts ChartOfAccount { get; set; } = null!;
    public int? BranchId { get; set; }
    public Branch? Branch { get; set; }
    public int PeriodId { get; set; } // Monthly periods
    public decimal BudgetedAmount { get; set; }
    public decimal ActualAmount { get; set; } = 0;
    public decimal Variance { get; set; } = 0;
}
```

### 3. API Changes and New Endpoints

#### New API Controllers and Endpoints

##### ChartOfAccountsController (Read-Only)
**Endpoints:**
- `GET /api/ChartOfAccounts` - List all COA entries
- `GET /api/ChartOfAccounts/{id}` - Get COA details
- `GET /api/ChartOfAccounts/by-type/{accountType}` - Get COA by account type

**Code Behavior:**
- No POST/PUT/DELETE endpoints - COA is system-initialized
- Used for reference when creating Account instances

**Removed:** No AccountController needed. ChartOfAccounts is read-only, and transactions are handled through JournalEntryController.

##### AccountTracking Controller Modifications
**Deprecation Plan:**
- Mark existing endpoints as deprecated
- Redirect to new Journal Entry endpoints
- Maintain backward compatibility during transition

#### New API Controllers and Endpoints

##### GeneralLedgerController
**Endpoints:**
- `GET /api/GeneralLedger/{chartOfAccountId}` - Get account ledger
- `GET /api/GeneralLedger/trial-balance` - Generate trial balance
- `GET /api/GeneralLedger/balance-sheet` - Generate balance sheet
- `GET /api/GeneralLedger/income-statement` - Generate income statement

**Code Behavior:**
```csharp
[HttpGet("trial-balance")]
public async Task<IActionResult> GetTrialBalance([FromQuery] DateTime asOfDate, [FromQuery] int? branchId = null)
{
    var trialBalance = await _generalLedgerService.GenerateTrialBalanceAsync(asOfDate, branchId);
    return Ok(trialBalance);
}

[HttpGet("balance-sheet")]
public async Task<IActionResult> GetBalanceSheet([FromQuery] DateTime asOfDate, [FromQuery] int? branchId = null)
{
    var balanceSheet = await _generalLedgerService.GenerateBalanceSheetAsync(asOfDate, branchId);
    return Ok(balanceSheet);
}

public class TrialBalanceDto
{
    public List<TrialBalanceLineDto> Lines { get; set; } = new List<TrialBalanceLineDto>();
    public decimal TotalDebits { get; set; }
    public decimal TotalCredits { get; set; }
}

public class TrialBalanceLineDto
{
    public string AccountCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal DebitBalance { get; set; }
    public decimal CreditBalance { get; set; }
}
```

##### JournalEntryController
**Endpoints:**
- `GET /api/JournalEntry` - List journal entries with filtering
- `GET /api/JournalEntry/{id}` - Get journal entry details
- `POST /api/JournalEntry` - Create new journal entry
- `PUT /api/JournalEntry/{id}/post` - Post journal entry
- `PUT /api/JournalEntry/{id}/approve` - Approve journal entry
- `DELETE /api/JournalEntry/{id}` - Void journal entry

**Code Behavior:**
```csharp
[HttpPost]
public async Task<IActionResult> CreateJournalEntry(CreateJournalEntryCommand command)
{
    // Validate double-entry accounting
    var totalDebits = command.Lines.Sum(l => l.DebitAmount);
    var totalCredits = command.Lines.Sum(l => l.CreditAmount);

    if (totalDebits != totalCredits)
    {
        return BadRequest("Journal entry must balance (total debits must equal total credits)");
    }

    var result = await _mediator.Send(command);
    return Ok(result);
}
```

##### AccountsReceivableController
**Endpoints:**
- `GET /api/AccountsReceivable` - List AR transactions
- `GET /api/AccountsReceivable/{id}` - Get AR details
- `POST /api/AccountsReceivable` - Create invoice/receivable
- `POST /api/AccountsReceivable/{id}/receipt` - Record payment receipt
- `GET /api/AccountsReceivable/aging` - Generate AR aging report

**Code Behavior:**
```csharp
[HttpPost("{id}/receipt")]
public async Task<IActionResult> RecordReceipt(int id, RecordReceiptCommand command)
{
    // Update AR balance
    var ar = await _arService.GetByIdAsync(id);
    ar.PaidAmount += command.Amount;
    ar.BalanceAmount = ar.InvoiceAmount - ar.PaidAmount;

    if (ar.BalanceAmount == 0)
        ar.Status = ARStatus.Paid;
    else if (ar.BalanceAmount < ar.InvoiceAmount)
        ar.Status = ARStatus.PartiallyPaid;

    // Create journal entry for payment
    await _journalEntryService.CreateReceiptEntry(ar, command);

    await _arService.UpdateAsync(ar);
    return Ok();
}
```

##### AccountsPayableController
**Endpoints:**
- `GET /api/AccountsPayable` - List AP transactions
- `GET /api/AccountsPayable/{id}` - Get AP details
- `POST /api/AccountsPayable` - Create bill/payable
- `POST /api/AccountsPayable/{id}/payment` - Record payment
- `GET /api/AccountsPayable/aging` - Generate AP aging report

##### BudgetController
**Endpoints:**
- `GET /api/Budget` - List budgets
- `GET /api/Budget/{id}` - Get budget details
- `POST /api/Budget` - Create budget
- `PUT /api/Budget/{id}/submit` - Submit for approval
- `PUT /api/Budget/{id}/approve` - Approve budget
- `GET /api/Budget/{id}/variance` - Get budget vs actual report

#### APIs to Remove/Deprecate

##### ExpenseTracking Controller
**Action:** Deprecate and redirect to JournalEntry
- Existing expense entries will be migrated to journal entries
- New expenses will be recorded through journal entries

##### WithdrawalTracking Controller
**Action:** Deprecate and redirect to JournalEntry
- Deposits and withdrawals will be handled through journal entries
- Maintain historical data for auditing

##### PayrollTracking Modifications
**Action:** Modify to integrate with General Ledger via event-driven accounting
- Add event handlers for payroll processing that create journal entries
- Link to employee advance deductions automatically
- Maintain existing payroll functionality but trigger accounting events

##### SupplierDue & DuePayment Tables
**Action:** Deprecate and replace with AccountsPayable
- SupplierDue functionality moves to AccountsPayable
- DuePayment records become Payment collections under AccountsPayable
- Historical data will be migrated during implementation

##### VisitPayment Table Modifications
**Action:** Modify to focus on payment processing, integrate with AccountsReceivable
- Remove financial tracking columns (amountPaid, AmountInAFN, etc.)
- Add JournalEntryId to link payments to accounting entries
- Keep visit and service tracking
- Outstanding balances tracked in AccountsReceivable

### 4. Business Logic Implementation

#### Event-Driven Accounting
**Key Principle:** Accounting entries are created automatically through domain events, not manual CRUD operations.

**Supported Events:**
- InvoiceCreated -> Create AR entry
- PaymentReceived -> Update AR and create cash entry
- BillCreated -> Create AP entry
- PaymentMade -> Update AP and create cash entry
- PayrollProcessed -> Create salary expense entries
- AdvancePaid -> Create advance liability entries
- AdvanceDeducted -> Reduce liability and expense

**Implementation:**
```csharp
public class AccountingEventHandler : INotificationHandler<InvoiceCreatedEvent>
{
    public async Task Handle(InvoiceCreatedEvent notification, CancellationToken cancellationToken)
    {
        var journalEntry = new JournalEntry
        {
            EntryNumber = await GenerateEntryNumberAsync("INV"),
            EntryDate = notification.InvoiceDate,
            Description = $"Invoice {notification.InvoiceNumber}",
            ReferenceNumber = notification.InvoiceNumber,
            ReferenceType = "INVOICE",
            BranchId = notification.BranchId,
            Status = JournalEntryStatus.Unposted,
            JournalEntryLines = new List<JournalEntryLine>
            {
                new JournalEntryLine
                {
                    ChartOfAccountId = await GetARAccountIdAsync(),
                    Description = $"AR - Invoice {notification.InvoiceNumber}",
                    DebitAmount = notification.Amount,
                    CreditAmount = 0,
                    CurrencyId = notification.CurrencyId
                },
                new JournalEntryLine
                {
                    ChartOfAccountId = await GetRevenueAccountIdAsync(notification.ServiceType),
                    Description = $"Revenue - Invoice {notification.InvoiceNumber}",
                    DebitAmount = 0,
                    CreditAmount = notification.Amount,
                    CurrencyId = notification.CurrencyId
                }
            }
        };

        await _journalEntryService.CreateJournalEntryAsync(journalEntry);
    }
}
```

#### Journal Entry Service
```csharp
public class JournalEntryService : IJournalEntryService
{
    public async Task<JournalEntry> CreateJournalEntryAsync(CreateJournalEntryCommand command)
    {
        // Validate balancing
        ValidateJournalEntry(command);

        // Generate entry number
        var entryNumber = await GenerateEntryNumberAsync();

        // Create journal entry
        var journalEntry = new JournalEntry
        {
            EntryNumber = entryNumber,
            EntryDate = command.EntryDate,
            Description = command.Description,
            ReferenceNumber = command.ReferenceNumber,
            ReferenceType = command.ReferenceType,
            BranchId = command.BranchId,
            JournalEntryLines = command.Lines.Select(l => new JournalEntryLine
            {
                AccountId = l.AccountId,
                Description = l.Description,
                DebitAmount = l.DebitAmount,
                CreditAmount = l.CreditAmount,
                CurrencyId = l.CurrencyId,
                ExchangeRate = l.ExchangeRate,
                AmountInBaseCurrency = CalculateBaseAmount(l)
            }).ToList()
        };

        await _repository.AddAsync(journalEntry);
        return journalEntry;
    }

    public async Task PostJournalEntryAsync(int journalEntryId, Guid approvedBy)
    {
        var journalEntry = await _repository.GetByIdAsync(journalEntryId);
        journalEntry.Status = JournalEntryStatus.Posted;
        journalEntry.ApprovedBy = approvedBy;
        journalEntry.ApprovedDate = DateTime.UtcNow;

        // Update general ledger
        await UpdateGeneralLedgerAsync(journalEntry);

        await _repository.UpdateAsync(journalEntry);
    }

    private async Task UpdateGeneralLedgerAsync(JournalEntry journalEntry)
    {
        foreach (var line in journalEntry.JournalEntryLines)
        {
            var ledgerEntry = new GeneralLedger
            {
                AccountId = line.AccountId,
                JournalEntryId = journalEntry.Id,
                TransactionDate = journalEntry.EntryDate,
                Description = line.Description,
                DebitAmount = line.DebitAmount,
                CreditAmount = line.CreditAmount,
                FiscalPeriodId = await GetFiscalPeriodIdAsync(journalEntry.EntryDate)
            };

            // Calculate running balance
            ledgerEntry.Balance = await CalculateRunningBalanceAsync(
                line.ChartOfAccountId, ledgerEntry.DebitAmount, ledgerEntry.CreditAmount, journalEntry.BranchId);

            await _ledgerRepository.AddAsync(ledgerEntry);
        }
    }

    private void ValidateJournalEntry(CreateJournalEntryCommand command)
    {
        var totalDebits = command.Lines.Sum(l => l.DebitAmount);
        var totalCredits = command.Lines.Sum(l => l.CreditAmount);

        if (totalDebits != totalCredits)
        {
            throw new ValidationException(
                $"Journal entry does not balance. Debits: {totalDebits}, Credits: {totalCredits}");
        }

        if (!command.Lines.Any())
        {
            throw new ValidationException("Journal entry must have at least one line");
        }
    }
}
```

#### General Ledger Service
```csharp
public class GeneralLedgerService : IGeneralLedgerService
{
    public async Task<TrialBalanceDto> GenerateTrialBalanceAsync(DateTime asOfDate, int? branchId = null)
    {
        var ledgerEntries = await _ledgerRepository.GetEntriesUpToDateAsync(asOfDate, branchId);

        var groupedEntries = ledgerEntries
            .GroupBy(e => new { e.ChartOfAccountId, e.BranchId })
            .Select(g => new TrialBalanceLineDto
            {
                AccountCode = g.First().ChartOfAccount.AccountCode,
                AccountName = $"{g.First().ChartOfAccount.AccountName} {(g.Key.BranchId.HasValue ? $"- Branch {g.Key.BranchId}" : "")}",
                DebitBalance = g.Sum(e => e.DebitAmount),
                CreditBalance = g.Sum(e => e.CreditAmount)
            })
            .Where(l => l.DebitBalance != 0 || l.CreditBalance != 0)
            .OrderBy(l => l.AccountCode)
            .ToList();

        return new TrialBalanceDto
        {
            Lines = groupedEntries,
            TotalDebits = groupedEntries.Sum(l => l.DebitBalance),
            TotalCredits = groupedEntries.Sum(l => l.CreditBalance)
        };
    }

    public async Task<BalanceSheetDto> GenerateBalanceSheetAsync(DateTime asOfDate, int? branchId = null)
    {
        var trialBalance = await GenerateTrialBalanceAsync(asOfDate, branchId);

        var assets = trialBalance.Lines
            .Where(l => l.AccountCode.StartsWith("1"))
            .ToList();

        var liabilities = trialBalance.Lines
            .Where(l => l.AccountCode.StartsWith("2"))
            .ToList();

        var equity = trialBalance.Lines
            .Where(l => l.AccountCode.StartsWith("3"))
            .ToList();

        return new BalanceSheetDto
        {
            AsOfDate = asOfDate,
            Assets = assets,
            Liabilities = liabilities,
            Equity = equity,
            TotalAssets = assets.Sum(a => Math.Max(a.DebitBalance - a.CreditBalance, 0)),
            TotalLiabilities = liabilities.Sum(l => Math.Max(l.CreditBalance - l.DebitBalance, 0)),
            TotalEquity = equity.Sum(e => Math.Max(e.CreditBalance - e.DebitBalance, 0))
        };
    }
}
```

### 5. Migration Strategy

#### Data Migration Plan
1. **Chart of Accounts Setup**
   - Create standard chart of accounts
   - Map existing MainAccount records to appropriate account types
   - Assign account codes to existing accounts

2. **Historical Data Migration**
   - Convert AccountTracking records to JournalEntry records
   - Migrate ExpenseTracking to journal entries
   - Convert PayrollTracking to proper expense entries
   - Migrate VisitPayment to AccountsReceivable

3. **System Integration**
   - Update all financial transaction creation to use new journal entry system
   - Modify reporting to use General Ledger data
   - Update dashboards to reflect new account structure

#### Migration Scripts
```csharp
public class FinancialSystemMigration
{
    public async Task InitializeChartOfAccountsAsync()
    {
        // Create standard COA entries
        var coaEntries = new List<ChartOfAccounts>
        {
            new ChartOfAccounts
            {
                AccountCode = "1101",
                AccountName = "Cash and Cash Equivalents",
                AccountType = AccountType.Asset,
                AccountCategory = AccountCategory.CurrentAsset,
                NormalBalance = NormalBalanceType.Debit,
                IsSystemAccount = true,
                IsActive = true,
                Description = "Cash on hand and in banks"
            },
            // Add all standard COA entries...
        };

        foreach (var coa in coaEntries)
        {
            await _coaRepository.AddAsync(coa);
        }
    }

**Removed:** No account creation needed per currency. ChartOfAccounts is universal.

    public async Task MigrateHistoricalTransactionsAsync()
    {
        // Migrate AccountTracking to JournalEntry
        var accountTrackingRecords = await _accountTrackingRepository.GetAllAsync();

        foreach (var tracking in accountTrackingRecords)
        {
            // Map old MainAccountId to ChartOfAccountId (requires mapping logic)
            var chartOfAccountId = await MapMainAccountToChartOfAccountAsync(tracking.MainAccountId);

            var journalEntry = new JournalEntry
            {
                EntryNumber = $"MIG-{tracking.ID}",
                EntryDate = tracking.TransactionDate,
                Description = tracking.Description,
                Status = JournalEntryStatus.Posted,
                ReferenceType = "MIGRATION",
                BranchId = tracking.BranchId, // Assuming AccountTracking has BranchId
                JournalEntryLines = new List<JournalEntryLine>
                {
                    new JournalEntryLine
                    {
                        ChartOfAccountId = chartOfAccountId,
                        Description = tracking.Description,
                        DebitAmount = tracking.DebitAmount,
                        CreditAmount = tracking.CreditAmount,
                        CurrencyId = tracking.CurrencyTypeId
                    }
                }
            };

            await _journalEntryRepository.AddAsync(journalEntry);
        }
    }
}
```

### 6. Implementation Timeline

#### Phase 1: Foundation (Weeks 1-3)
- Create new database tables (ChartOfAccounts, JournalEntry, GeneralLedger, AR/AP, Budget)
- Implement chart of accounts initialization
- Set up basic journal entry system
- Remove unnecessary tables (ExpenseTracking, WithdrawalTracking, AccountTracking)

#### Phase 2: Core Accounting (Weeks 4-7)
- Implement Accounts Receivable/Payable
- Create financial reporting services
- Implement event-driven accounting handlers
- Modify existing transaction creation

#### Phase 3: Integration & Contracts (Weeks 8-10)
- Update all existing APIs to invoke journal services and shared DTOs
- Publish versioned REST/event contracts for Inventory, Sales, Clinic, and HR consumers
- Execute contract and end-to-end simulations for FinancialBridge, IValuationService, and payroll hooks
- Implement comprehensive validation and approval workflows

#### Phase 4: Reporting & Optimization (Weeks 11-13)
- Create financial dashboards
- Implement budget vs actual reporting
- Add advanced analytics
- Performance optimization

### UI/UX & Deployment Strategy
- Rebuild finance-facing forms and dashboards where necessary to expose the new GL/AR/AP capabilities; legacy screens can be discarded instead of retrofitted.
- Because the platform is being relaunched greenfield, skip legacy data migration and focus on seeding master data (currencies, branches, chart of accounts) plus automated smoke demos.
- Coordinate UI changes with Inventory, Sales, Clinic, and HR teams so shared components reuse the same financial widgets and avoid duplicated effort.

### 7. Testing Strategy

#### Unit Tests
- Journal entry validation
- Account balance calculations
- Financial report generation
- Business rule enforcement

#### Integration Tests
- End-to-end transaction processing
- Multi-currency handling
- Cross-module financial operations
- Contract tests for REST/event payloads consumed by Inventory, Sales, Clinic, and HR services
- Report accuracy validation under concurrent load

#### User Acceptance Testing
- Accounting workflow validation
- Report accuracy verification
- System performance under load
- Data migration validation

## Conclusion

This comprehensive financial system implementation will transform the Crystal Clinic Service Management System from basic transaction tracking to a full-featured double-entry accounting system. The implementation provides:

- **Unified Chart of Accounts:** System-defined COA with read-only access, initialized on system setup
- **Event-Driven Accounting:** Automatic journal entries triggered by business events (invoices, payments, payroll)
- **Branch/Currency Dimensions:** Handled at transaction level, not account level
- **Proper Double-Entry Accounting:** Journal entries with validation and general ledger
- **Comprehensive Financial Reporting:** Balance sheet, income statement, trial balance
- **Accounts Receivable/Payable Management:** Automated AR/AP processing
- **Budgeting and Variance Analysis:** Multi-year budgeting with actual vs budget reporting
- **Audit Trails and Approval Workflows:** Complete transaction traceability
- **Multi-Currency Support:** Exchange rate handling and base currency reporting
- **Simplified Architecture:** Removed unnecessary tables (ExpenseTracking, WithdrawalTracking, AccountTracking, FiscalPeriod, FiscalYear)
- **Enhanced AR/AP:** AccountsReceivable replaces VisitPayment receivables tracking, AccountsPayable replaces SupplierDue payables tracking

The phased approach ensures minimal disruption to existing operations while providing a solid foundation for future financial management needs. Accounting entries are now event-driven, eliminating manual journal entry creation and ensuring accuracy. The simplified table structure reduces complexity and maintenance overhead.