---
description: Detailed Asset Management System Analysis
alwaysApply: true
---

# Detailed Asset Management System Analysis

This document provides an in-depth analysis of the Asset Management (AssetMS) system in the Crystal Clinic Service Management System, covering database structure, API endpoints, code behavior, and system weaknesses.

## Database Structure

### MainAccount Table
**Primary Key**: ID (Guid)

**Columns**:
- ID (Guid) - Primary key
- CurrencyTypeId (int) - Currency for the account
- DepositDate (DateTime) - Initial deposit date
- BranchId (int) - Associated branch
- Description (string) - Account description
- OwnerUserId (Guid) - Account owner user ID
- BalanceAmount (double) - Current account balance

**Relationships**:
- CurrencyType (many-to-one)
- Branch (many-to-one)
- ApplicationUser (many-to-one via OwnerUserId)

**Audit Fields**: CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted

### ExpenseTracking Table
**Primary Key**: ID (int)

**Columns**:
- ID (int) - Primary key
- CurrencyTypeId (int) - Expense currency
- ExpenseTypeId (int) - Type of expense
- MainAccountId (Guid) - Account used for expense
- Amount (float) - Expense amount
- Date (DateTime) - Expense date
- Description (string) - Expense description
- InvoiceNumber (string) - Invoice reference number
- AttachmentPath (string) - Path to invoice attachment
- BranchId (int) - Branch where expense occurred
- UserId (Guid) - User who recorded the expense

**Relationships**:
- CurrencyType (many-to-one)
- ExpenseType (many-to-one)
- MainAccount (many-to-one)
- Branch (many-to-one)
- ApplicationUser (many-to-one via UserId)

**Audit Fields**: CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted

### WithdrawalTracking Table
**Primary Key**: ID (int)

**Columns**:
- ID (int) - Primary key
- CurrencyTypeId (int) - Withdrawal currency
- MainAccountId (Guid) - Account from which withdrawal was made
- BranchId (int) - Branch location
- Date (DateTime) - Withdrawal date
- Description (string) - Withdrawal description
- UserId (Guid) - User who performed the withdrawal
- WithdrawalAmount (double) - Amount withdrawn (default: double.MinValue)
- DepositAmount (double) - Amount deposited (default: double.MinValue)

**Relationships**:
- CurrencyType (many-to-one)
- MainAccount (many-to-one)
- Branch (many-to-one)
- ApplicationUser (many-to-one via UserId)

**Audit Fields**: CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted

### TradeTracking Table
**Primary Key**: ID (int)

**Columns**:
- ID (int) - Primary key
- CurrencyTypeId (int) - Trade currency
- MainAccountId (Guid) - Account used for trade
- Date (DateTime) - Trade date
- Description (string) - Trade description
- BranchId (int) - Branch location
- UserId (Guid) - User who recorded the trade
- TradeAmount (float) - Total trade amount
- ProfitAmount (float) - Profit from trade
- LossAmount (float) - Loss from trade

**Relationships**:
- CurrencyType (many-to-one)
- MainAccount (many-to-one)
- Branch (many-to-one)
- ApplicationUser (many-to-one via UserId)

**Audit Fields**: CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted

### AccountTracking Table
**Primary Key**: ID (int)

**Columns**:
- ID (int) - Primary key
- UserId (Guid) - User initiating the transaction
- MainAccountId (Guid) - Account involved
- DebitAmount (double) - Debit amount
- CurrencyTypeId (int) - Transaction currency
- BalanceAmount (double) - Account balance after transaction
- TransactionDate (DateTime) - Transaction timestamp
- Description (string) - Transaction description
- trackType (enum) - Type of tracking (TRANSFER, etc.)
- transactionStatus (enum) - Status (PENDING, APPROVED, etc.)
- toUserId (Guid, nullable) - Recipient user for transfers
- fromUserId (Guid) - Sender user for transfers

**Relationships**:
- ApplicationUser (many-to-one via UserId)
- MainAccount (many-to-one)
- CurrencyType (many-to-one)
- ApplicationUser (many-to-one via toUserId)
- ApplicationUser (many-to-one via fromUserId)

**Audit Fields**: CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted

## API Endpoints

### MainAssetController
**Base Route**: `/api/MainAsset`

**Endpoints**:
- `POST /` - Create main account
- `PUT /{Id:Guid}` - Update main account
- `DELETE /{Id:Guid}` - Delete main account
- `GET /GetList` - Get accounts list with filters
- `GET /GetDetail/{Id:Guid}` - Get account details
- `GET /GetDDL` - Get accounts dropdown
- `GET /GetChildDDL/{parentId:Guid}` - Get child accounts dropdown
- `PUT /DepositToUser` - Deposit to user account

### ExpenseTrackingController
**Base Route**: `/api/ExpenseTracking`

**Endpoints**:
- `POST /` - Create expense tracking (form data with attachment)
- `PUT /{Id:int}` - Update expense tracking (form data with attachment)
- `DELETE /{Id:int}` - Delete expense tracking
- `GET /GetList` - Get expenses list with filters
- `GET /GetDetail/{Id:int}` - Get expense details

### TransactionsController
**Base Route**: `/api/Transactions`

**Endpoints**:
- `POST /initiate` - Initiate transfer transaction
- `PUT /update` - Update pending transaction
- `POST /approve` - Approve transaction
- `POST /confirm` - Confirm transaction
- `POST /reject` - Reject transaction
- `GET /pending` - Get pending transactions
- `GET /history` - Get transaction history

### WithdrawalTrackingController
**Base Route**: `/api/WithdrawalTracking`

**Endpoints**:
- `POST /` - Create withdrawal/deposit tracking
- `PUT /{Id:int}` - Update withdrawal tracking
- `DELETE /{Id:int}` - Delete withdrawal tracking
- `GET /GetList` - Get withdrawals list
- `GET /GetDetail/{Id:int}` - Get withdrawal details

### DashboardAndReportController
**Base Route**: `/api/DashboardAndReport`

**Endpoints**:
- `GET /ExpenseTrackingChart` - Expense analytics chart
- `GET /TradeTrackingChart` - Trade performance chart
- `GET /RealTimeDashboard` - Real-time financial dashboard
- `GET /BranchMainAssetDashboard` - Branch asset overview
- `GET /BarChartofAssetByAssetTypesAndUser` - Asset distribution chart

## Code Behavior

### Account Management

#### CreateMainAccountCommand
```csharp
public class CreateMainAccountCommand : IRequest<JsonResult>
{
    public int CurrencyTypeId { get; set; }
    public DateTime DepositDate { get; set; }
    public int BranchId { get; set; }
    public string Description { get; set; }
    public Guid OwnerUserId { get; set; }
    public double BalanceAmount { get; set; }
}
```

**Handler Logic**:
```csharp
public async Task<JsonResult> Handle(CreateMainAccountCommand request, CancellationToken ct)
{
    // Validate branch access
    var branchAccess = await _userService.CheckBranchAccess(loggedInUser.Id, request.BranchId);
    if (!branchAccess) throw new UnauthorizedAccessException();

    // Validate currency and owner
    var currency = await context.CurrencyType.FindAsync(request.CurrencyTypeId);
    var owner = await context.Users.FindAsync(request.OwnerUserId);
    if (currency == null || owner == null) {
        throw new ValidationException("Invalid currency or owner");
    }

    var account = new MainAccount {
        ID = Guid.NewGuid(),
        CurrencyTypeId = request.CurrencyTypeId,
        DepositDate = request.DepositDate,
        BranchId = request.BranchId,
        Description = request.Description,
        OwnerUserId = request.OwnerUserId,
        BalanceAmount = request.BalanceAmount,
        CreatedBy = loggedInUser.Id,
        CreatedOn = DateTime.Now
    };

    context.MainAccount.Add(account);
    await context.SaveChangesAsync(ct);

    return new JsonResult(account);
}
```

### Expense Tracking

#### CreateExpenseTrackingCommand
```csharp
public class CreateExpenseTrackingCommand : IRequest<JsonResult>
{
    public Guid MainAccountId { get; set; }
    public int ExpenseTypeId { get; set; }
    public float Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string InvoiceNumber { get; set; }
    public IFormFile? Attachment { get; set; }
}
```

**Handler Logic**:
```csharp
public async Task<JsonResult> Handle(CreateExpenseTrackingCommand request, CancellationToken ct)
{
    // Validate account and balance
    var account = await context.MainAccount.FindAsync(request.MainAccountId);
    if (account == null) throw new KeyNotFoundException("Account not found");

    if (account.BalanceAmount < request.Amount) {
        throw new ValidationException("Insufficient account balance");
    }

    // Handle file upload
    string attachmentPath = "";
    if (request.Attachment != null) {
        attachmentPath = await _storageService.UploadFile(request.Attachment, "expense-attachments");
    }

    var expense = new ExpenseTracking {
        CurrencyTypeId = account.CurrencyTypeId,
        ExpenseTypeId = request.ExpenseTypeId,
        MainAccountId = request.MainAccountId,
        Amount = request.Amount,
        Date = request.Date,
        Description = request.Description,
        InvoiceNumber = request.InvoiceNumber,
        AttachmentPath = attachmentPath,
        BranchId = account.BranchId,
        UserId = loggedInUser.Id,
        CreatedBy = loggedInUser.Id,
        CreatedOn = DateTime.Now
    };

    // Update account balance
    account.BalanceAmount -= request.Amount;
    account.ModifiedBy = loggedInUser.Id;
    account.ModifiedOn = DateTime.Now;

    context.ExpenseTracking.Add(expense);
    await context.SaveChangesAsync(ct);

    return new JsonResult(expense);
}
```

### Transaction Processing

#### InitiateTransferCommand
```csharp
public class InitiateTransferCommand : IRequest<Result>
{
    public Guid FromAccountId { get; set; }
    public Guid ToUserId { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }
}
```

**Handler Logic**:
```csharp
public async Task<Result> Handle(InitiateTransferCommand request, CancellationToken ct)
{
    if (request.Amount <= 0)
        return Result.Fail("Invalid transfer amount.");

    var fromAccount = await context.MainAccount.FindAsync(request.FromAccountId);
    if (fromAccount == null)
        return Result.Fail("Account Not found: " + request.FromAccountId);

    if (fromAccount.BalanceAmount < request.Amount)
        return Result.Fail("Insufficient balance.");

    var transaction = new AccountTracking {
        UserId = loggedInUser.Id,
        MainAccountId = fromAccount.ID,
        DebitAmount = request.Amount,
        CurrencyTypeId = fromAccount.CurrencyTypeId,
        BalanceAmount = fromAccount.BalanceAmount - request.Amount,
        TransactionDate = DateTime.UtcNow,
        Description = request.Description,
        trackType = TrackType.TRANSFER,
        transactionStatus = TransactionStatus.PENDING,
        toUserId = request.ToUserId,
        fromUserId = loggedInUser.Id,
        CreatedOn = DateTime.Now,
        CreatedBy = loggedInUser.Id
    };

    await context.AccountTracking.AddAsync(transaction, ct);
    await context.SaveChangesAsync(ct);

    return Result.Success(transaction.ID);
}
```

#### ApproveTransferCommand
**Handler Logic**:
```csharp
public async Task<Result> Handle(ApproveTransferCommand request, CancellationToken ct)
{
    var transaction = await context.AccountTracking.FindAsync(request.TransactionId);
    if (transaction == null || transaction.transactionStatus != TransactionStatus.PENDING)
        return Result.Fail("Transaction not found or not pending");

    // Check if approver has permission
    var approverAccount = await context.MainAccount
        .FirstOrDefaultAsync(a => a.OwnerUserId == loggedInUser.Id);

    if (approverAccount == null || approverAccount.BalanceAmount < transaction.DebitAmount)
        return Result.Fail("Insufficient funds or no account");

    transaction.transactionStatus = TransactionStatus.APPROVED;
    transaction.ModifiedBy = loggedInUser.Id;
    transaction.ModifiedOn = DateTime.Now;

    await context.SaveChangesAsync(ct);
    return Result.Success("Transaction approved");
}
```

### Withdrawal/Deposit Tracking

#### CreateWithdrawalTrackingCommand
**Handler Logic**:
```csharp
public async Task<JsonResult> Handle(CreateWithdrawalTrackingCommand request, CancellationToken ct)
{
    var account = await context.MainAccount.FindAsync(request.MainAccountId);
    if (account == null) throw new KeyNotFoundException("Account not found");

    // Validate amounts (only one should be set)
    if ((request.WithdrawalAmount > 0 && request.DepositAmount > 0) ||
        (request.WithdrawalAmount <= 0 && request.DepositAmount <= 0)) {
        throw new ValidationException("Specify either withdrawal or deposit amount");
    }

    // Check balance for withdrawals
    if (request.WithdrawalAmount > 0 && account.BalanceAmount < request.WithdrawalAmount) {
        throw new ValidationException("Insufficient balance");
    }

    var tracking = new WithdrawalTracking {
        CurrencyTypeId = request.CurrencyTypeId,
        MainAccountId = request.MainAccountId,
        BranchId = request.BranchId,
        Date = request.Date,
        Description = request.Description,
        UserId = loggedInUser.Id,
        WithdrawalAmount = request.WithdrawalAmount,
        DepositAmount = request.DepositAmount,
        CreatedBy = loggedInUser.Id,
        CreatedOn = DateTime.Now
    };

    // Update account balance
    if (request.WithdrawalAmount > 0) {
        account.BalanceAmount -= request.WithdrawalAmount;
    } else {
        account.BalanceAmount += request.DepositAmount;
    }

    context.WithdrawalTracking.Add(tracking);
    await context.SaveChangesAsync(ct);

    return new JsonResult(tracking);
}
```

## Weaknesses of the Asset Management System

### 1. Limited Asset Lifecycle Management
- **Missing Features**: No asset acquisition, depreciation tracking, maintenance scheduling, or disposal workflows
- **Impact**: Cannot track asset value over time or plan for replacements
- **Recommendation**: Implement comprehensive fixed asset management

### 2. Basic Financial Tracking
- **Current State**: Simple account balances and transactions
- **Issues**: No budgeting, forecasting, or financial reporting capabilities
- **Impact**: Limited financial insights and planning
- **Recommendation**: Add financial planning and reporting modules

### 3. Inadequate Transaction Security
- **Current State**: Basic approval workflow for transfers
- **Issues**: No multi-level approvals, no transaction limits, no fraud detection
- **Impact**: Potential financial losses from unauthorized transactions
- **Recommendation**: Implement robust transaction approval workflows and monitoring

### 4. Poor Document Management
- **Current State**: Basic file uploads for invoices
- **Issues**: No document versioning, no OCR for data extraction, no secure storage
- **Impact**: Difficulty retrieving and verifying financial documents
- **Recommendation**: Integrate document management with OCR and secure cloud storage

### 5. Limited Reporting and Analytics
- **Current State**: Basic charts and dashboards
- **Missing**: Advanced financial analytics, trend analysis, predictive insights
- **Impact**: Limited decision-making capabilities
- **Recommendation**: Add comprehensive business intelligence and reporting tools

### 6. Currency Handling Issues
- **Current State**: Basic currency support
- **Issues**: No automatic exchange rate updates, no multi-currency reporting
- **Impact**: Manual currency conversions, potential errors
- **Recommendation**: Integrate real-time currency exchange services

### 7. Inventory Integration Gaps
- **Current State**: Separate from stock management
- **Issues**: No integration between asset purchases and inventory
- **Impact**: Duplicate data entry, inconsistent tracking
- **Recommendation**: Integrate asset and inventory management systems

### 8. Compliance and Audit Issues
- **Current State**: Basic audit trails
- **Issues**: No compliance reporting, no automated audit checks
- **Impact**: Potential regulatory compliance issues
- **Recommendation**: Add compliance monitoring and automated audit reporting

### 9. User Experience Problems
- **Current State**: Complex transaction workflows
- **Issues**: No mobile access, no self-service features, confusing approval processes
- **Impact**: Administrative burden and user frustration
- **Recommendation**: Simplify workflows and add mobile access

### 10. Scalability and Performance Issues
- **Current State**: Basic database design
- **Issues**: No partitioning, no archiving, potential performance issues with large datasets
- **Impact**: System slowdowns as data grows
- **Recommendation**: Implement data archiving and performance optimization strategies