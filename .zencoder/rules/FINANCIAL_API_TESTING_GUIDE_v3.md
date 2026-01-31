# Financial System API Testing Guide (v3 - FULLY CORRECTED)

## Document Purpose
This guide provides accurate, sequential testing plan for Crystal Clinic Financial System APIs based on actual implemented endpoints. All payloads have been cross-checked against actual DTOs and verified for correctness.

**Last Updated:** January 2026 (Fully Corrected - v3)  
**API Base URL (Finance):** `https://api.crystalclinic.local/api/Finance`  
**API Base URL (Look/Setup):** `https://api.crystalclinic.local/api/Look`

---

## Testing Prerequisites

### Test Environment Setup
- **Authentication:** All endpoints require Bearer token with `[Authorize]` attribute
- **Test User:** Create test user with Finance/Admin role
- **Test Database:** Use dedicated test database with clean state
- **Headers:** `Authorization: Bearer {token}`, `Content-Type: application/json`

### API Module Locations
- ✅ **Equity/Shareholder APIs:** `/api/Finance/Equity` (EquityController)
- ✅ **Journal Entry APIs:** `/api/Finance/JournalEntry` (JournalEntryController)
- ✅ **Sales Invoice APIs:** `/api/Finance/SalesInvoice` (SalesInvoiceController)
- ✅ **Procurement (PO & Vendor Bill):** `/api/Finance/Procurement` (ProcurementController)
- ✅ **Accounts Receivable:** `/api/Finance/AccountsReceivable` (AccountsReceivableController)
- ✅ **Chart of Accounts:** `/api/Finance/ChartOfAccounts` (ChartOfAccountsController)
- ✅ **General Ledger:** `/api/Finance/GeneralLedger` (GeneralLedgerController)
- ✅ **Currency Types:** `/api/Look/CurrencyType` (Look module)
- ✅ **Branches:** `/api/Look/Branch` (Look module)
- ✅ **Exchange Rates:** `/api/Look/CurrencyExchangeRate` (Look module)
- ⚠️ **Department APIs:** NOT yet implemented (future phase)

---

## Phase 1: Setup (Currency, Branch, Exchange Rates)

### Purpose
Configure system-wide settings before financial operations.

### API Endpoints - Currency Types
- **Base Route:** `/api/Look/CurrencyType`

#### Test 1.1: Get Currency Type Dropdown List
```
GET /api/Look/CurrencyType/GetCurrencyTypeDDL?ExchangeRateDate=2026-01-31

Expected Response: 200 OK
[
  {
    "id": 1,
    "name": "Afghan Afghani",
    "code": "AFN"
  }
]
```

#### Test 1.2: Get Currency Type List
```
POST /api/Look/CurrencyType/GetList

Body:
{
  "searchBy": "",
  "pageNumber": 1,
  "pageSize": 20
}

Expected Response: 200 OK
{
  "totalRecord": 5,
  "currantPage": 1,
  "data": [
    {
      "id": 1,
      "name": "Afghan Afghani",
      "englishName": "Afghan Afghani",
      "pashtoName": "افغانۍ",
      "dariName": "افغانی",
      "code": "AFN"
    }
  ],
  "error": ""
}
```

#### Test 1.3: Create Currency Type
```
POST /api/Look/CurrencyType

Body:
{
  "englishName": "US Dollar",
  "dariName": "دالر امریکایی",
  "pashtoName": "امریکایی ډالر",
  "code": "USD"
}

Expected Response: 200 OK
```

#### Test 1.4: Get Currency Type Detail
```
GET /api/Look/CurrencyType/GetDetail/{id}

Expected Response: 200 OK
{
  "id": 1,
  "name": "Afghan Afghani",
  "englishName": "Afghan Afghani",
  "pashtoName": "افغانۍ",
  "dariName": "افغانی",
  "code": "AFN"
}
```

#### Test 1.5: Update Currency Type
```
PUT /api/Look/CurrencyType/{id}

Body:
{
  "id": 1,
  "englishName": "Afghan Afghani (Updated)",
  "dariName": "افغانی (اصلاح شده)",
  "pashtoName": "افغانۍ (اصلاح شوی)",
  "code": "AFN"
}

Expected Response: 200 OK
```

### API Endpoints - Branch Management
- **Base Route:** `/api/Look/Branch`

#### Test 1.6: Get Branch Dropdown List
```
GET /api/Look/Branch/GetBranchDDL

Expected Response: 200 OK
[
  {
    "id": 1,
    "code": "MAIN",
    "name": "Main Branch"
  }
]
```

#### Test 1.7: Get Branch List
```
POST /api/Look/Branch/GetList

Body:
{
  "searchBy": "",
  "pageNumber": 1,
  "pageSize": 20
}

Expected Response: 200 OK
{
  "totalRecord": 3,
  "currantPage": 1,
  "data": [
    {
      "id": 1,
      "name": "Main Branch",
      "englishName": "Main Branch",
      "pashtoName": "مرکزي څانګه",
      "dariName": "نمایندگی مرکزی",
      "code": "MAIN",
      "parentId": null,
      "parentBranchName": ""
    }
  ],
  "error": ""
}
```

#### Test 1.8: Create Branch
```
POST /api/Look/Branch

Body:
{
  "englishName": "North Branch",
  "pashtoName": "شمالي څانګه",
  "dariName": "نمایندگی شمال",
  "code": "NORTH",
  "parentId": null,
  "address": "Northern Road, Mazar-i-Sharif"
}

Expected Response: 200 OK
```

#### Test 1.9: Get Branch Detail
```
GET /api/Look/Branch/GetDetail/{id}

Expected Response: 200 OK
{
  "id": 1,
  "name": "Main Branch",
  "englishName": "Main Branch",
  "pashtoName": "مرکزي څانګه",
  "dariName": "نمایندگی مرکزی",
  "code": "MAIN",
  "parentId": null,
  "parentBranchName": "",
  "modifiedOn": "2026-01-25T10:00:00Z"
}
```

#### Test 1.10: Update Branch
```
PUT /api/Look/Branch/{id}

Body:
{
  "id": 1,
  "englishName": "Main Branch (Updated)",
  "pashtoName": "مرکزي څانګه (اصلاح شوی)",
  "dariName": "نمایندگی مرکزی (اصلاح شده)",
  "code": "MAIN",
  "parentId": null,
  "address": "Central Kabul"
}

Expected Response: 200 OK
```

#### Test 1.11: Delete Branch
```
DELETE /api/Look/Branch/{id}

Expected Response: 200 OK
```

#### Test 1.12: Get Child/Sub Branches
```
GET /api/Look/Branch/GetChildBranchDDL

Expected Response: 200 OK
[
  {
    "id": 2,
    "code": "NORTH-SUB",
    "name": "North Branch Sub-Office"
  }
]
```

### API Endpoints - Exchange Rates
- **Base Route:** `/api/Look/CurrencyExchangeRate`

#### Test 1.13: Create Exchange Rate
```
POST /api/Look/CurrencyExchangeRate

Body:
{
  "fromCurrencyId": 2,
  "toCurrencyId": 1,
  "exchangeRate": 78.5,
  "remarks": "Exchange rate for USD to AFN"
}

Expected Response: 201 Created
{
  "currencyExchangeRateId": 1,
  "fromCurrencyId": 2,
  "fromCurrencyCode": "USD",
  "toCurrencyId": 1,
  "toCurrencyCode": "AFN",
  "exchangeRate": 78.5,
  "createdOn": "2026-01-25T10:00:00Z",
  "remarks": "Exchange rate for USD to AFN"
}
```

#### Test 1.14: Get Exchange Rate by ID
```
GET /api/Look/CurrencyExchangeRate/{id}

Expected Response: 200 OK
{
  "currencyExchangeRateId": 1,
  "fromCurrencyId": 2,
  "fromCurrencyCode": "USD",
  "toCurrencyId": 1,
  "toCurrencyCode": "AFN",
  "exchangeRate": 78.5,
  "createdOn": "2026-01-25T10:00:00Z",
  "remarks": "Exchange rate for USD to AFN"
}
```

#### Test 1.15: Get Exchange Rates by Date
```
GET /api/Look/CurrencyExchangeRate/2026-01-25

Expected Response: 200 OK
[
  {
    "currencyExchangeRateId": 1,
    "fromCurrencyId": 2,
    "fromCurrencyCode": "USD",
    "toCurrencyId": 1,
    "toCurrencyCode": "AFN",
    "exchangeRate": 78.5,
    "createdOn": "2026-01-25T10:00:00Z"
  }
]
```

#### Test 1.16: Update Exchange Rate
```
PUT /api/Look/CurrencyExchangeRate

Body:
{
  "currencyExchangeRateId": 1,
  "fromCurrencyId": 2,
  "toCurrencyId": 1,
  "exchangeRate": 79.0,
  "remarks": "Updated rate"
}

Expected Response: 200 OK
"Exchange rate updated successfully."
```

#### Test 1.17: Delete Exchange Rate
```
DELETE /api/Look/CurrencyExchangeRate/{id}

Expected Response: 200 OK
"Exchange rate deleted successfully."
```

---

## Phase 2: Chart of Accounts Setup

### Purpose
Configure the account structure for all financial transactions.

### API Endpoints
- **Base Route:** `/api/Finance/ChartOfAccounts`

#### Test 2.1: Get All Chart of Accounts
```
GET /api/Finance/ChartOfAccounts?isActive=true&accountType=1&searchText=Cash

Query Parameters:
- isActive (bool, optional): Filter active/inactive accounts
- accountType (int, optional): Filter by type (1=Asset, 2=Liability, 3=Equity, 4=Revenue, 5=Expense, 6=ContraAsset, 7=OtherIncome)
- searchText (string, optional): Search by code or name

Expected Response: 200 OK
{
  "isSuccess": true,
  "succeeded": true,
  "error": null,
  "value": [
    {
      "id": 1,
      "accountCode": "1101",
      "accountName": "Cash and Cash Equivalents",
      "accountType": 1,
      "accountCategory": 100,
      "normalBalance": 0,
      "isSystemAccount": true,
      "isActive": true,
      "description": "Company cash and equivalents",
      "parentAccountId": null
    }
  ],
  "data": [ ... same as value ... ]
}
```

#### Test 2.2: Create Custom Account
```
POST /api/Finance/ChartOfAccounts

Body:
{
  "accountCode": "5201",
  "accountName": "Medical Supplies Expense",
  "accountType": 5,
  "accountCategory": 500,
  "normalBalance": 0,
  "isSystemAccount": false,
  "description": "Expenses for medical supplies",
  "parentAccountId": null
}

Expected Response: 201 Created
{
  "isSuccess": true,
  "succeeded": true,
  "error": "Chart of Accounts created successfully.",
  "value": 42,
  "data": 42
}
```

#### Test 2.3: Get Account by ID
```
GET /api/Finance/ChartOfAccounts/{id}

Expected Response: 200 OK
{
  "isSuccess": true,
  "succeeded": true,
  "error": null,
  "value": {
    "id": 1,
    "accountCode": "1101",
    "accountName": "Cash and Cash Equivalents",
    "accountType": 1,
    "accountCategory": 100,
    "normalBalance": 0,
    "isSystemAccount": true,
    "isActive": true,
    "description": "Company cash and equivalents",
    "parentAccountId": null
  },
  "data": { ... }
}
```

#### Test 2.4: Update Account
```
PUT /api/Finance/ChartOfAccounts/{id}

Body:
{
  "id": 1,
  "accountName": "Cash & Cash Equivalents (Updated)",
  "isActive": true,
  "description": "Company cash and equivalents updated"
}

Expected Response: 200 OK
{
  "isSuccess": true,
  "succeeded": true,
  "error": "Chart of Accounts updated successfully.",
  "value": null,
  "data": null
}
```

#### Test 2.5: Get Accounts by Type
```
GET /api/Finance/ChartOfAccounts/ByType/5

Expected Response: 200 OK
{
  "isSuccess": true,
  "succeeded": true,
  "error": null,
  "value": [
    {
      "id": 30,
      "accountCode": "5101",
      "accountName": "Salaries & Wages",
      "accountType": 5,
      "isActive": true
    }
  ],
  "data": [ ... ]
}
```

#### Test 2.6: Get Account Hierarchy
```
GET /api/Finance/ChartOfAccounts/Hierarchy?parentAccountId=null

Expected Response: 200 OK
{
  "isSuccess": true,
  "succeeded": true,
  "error": null,
  "value": [
    {
      "id": 1,
      "accountCode": "1000",
      "accountName": "Assets",
      "accountType": 1,
      "isActive": true,
      "parentAccountId": null,
      "childCount": 5
    }
  ],
  "data": [ ... ]
}
```

#### Test 2.7: Deactivate Account
```
POST /api/Finance/ChartOfAccounts/{id}/Deactivate

Expected Response: 200 OK
{
  "isSuccess": true,
  "succeeded": true,
  "error": "Chart of Accounts updated successfully.",
  "value": null,
  "data": null
}
```

#### Test 2.8: Move Account
```
POST /api/Finance/ChartOfAccounts/{id}/Move?newParentAccountId=10

Expected Response: 200 OK
{
  "isSuccess": true,
  "succeeded": true,
  "error": "Account moved successfully.",
  "value": null,
  "data": null
}
```

---

## Phase 3: Company Setup (Equity & Shareholders)

### Purpose
Establish company ownership structure.

### API Endpoints
- **Base Route:** `/api/Finance/Equity`

#### Test 3.1: Create Shareholder
```
POST /api/Finance/Equity/Shareholder

Body:
{
  "name": "John Doe",
  "ownershipPercentage": 50.0,
  "totalInvestment": 5000000.0,
  "contactInfo": "+93 700 000 001",
  "email": "john@crystalclinic.local"
}

Expected Response: 201 Created
{
  "isSuccess": true,
  "succeeded": true,
  "error": null,
  "value": 1,
  "data": 1
}
```

#### Test 3.2: Get All Shareholders
```
GET /api/Finance/Equity/Shareholder?isActive=true&pageNumber=1&pageSize=20

Expected Response: 200 OK
{
  "isSuccess": true,
  "succeeded": true,
  "error": null,
  "value": {
    "data": [
      {
        "id": 1,
        "name": "John Doe",
        "ownershipPercentage": 50.0,
        "totalInvestment": 5000000.0,
        "totalDrawings": 0,
        "netEquity": 5000000.0,
        "contactInfo": "+93 700 000 001",
        "email": "john@crystalclinic.local",
        "isActive": true
      }
    ],
    "totalCount": 1
  },
  "data": { ... }
}
```

#### Test 3.3: Get Shareholder by ID
```
GET /api/Finance/Equity/Shareholder/{id}

Expected Response: 200 OK
{
  "isSuccess": true,
  "succeeded": true,
  "error": null,
  "value": {
    "id": 1,
    "name": "John Doe",
    "ownershipPercentage": 50.0,
    "totalInvestment": 5000000.0,
    "totalDrawings": 0,
    "netEquity": 5000000.0,
    "contactInfo": "+93 700 000 001",
    "email": "john@crystalclinic.local",
    "isActive": true
  },
  "data": { ... }
}
```

#### Test 3.4: Update Shareholder
```
PUT /api/Finance/Equity/Shareholder/{id}

Body:
{
  "id": 1,
  "name": "John Doe Updated",
  "ownershipPercentage": 50.0,
  "contactInfo": "+93 700 000 002",
  "email": "john.updated@crystalclinic.local",
  "isActive": true
}

Expected Response: 200 OK
{
  "isSuccess": true,
  "succeeded": true,
  "error": null,
  "value": true,
  "data": true
}
```

#### Test 3.5: Record Equity Transaction
```
POST /api/Finance/Equity/Transaction

Body:
{
  "shareholderId": 1,
  "type": 0,
  "amount": 1000000.0,
  "transactionDate": "2026-01-05",
  "description": "Additional investment",
  "reference": "INV-001"
}

Expected Response: 201 Created
{
  "isSuccess": true,
  "succeeded": true,
  "error": null,
  "value": {
    "id": 1,
    "shareholderId": 1,
    "shareholderName": "John Doe",
    "type": 0,
    "amount": 1000000.0,
    "transactionDate": "2026-01-05T00:00:00Z",
    "description": "Additional investment",
    "reference": "INV-001"
  },
  "data": { ... }
}
```

#### Test 3.6: Get Equity Transactions
```
GET /api/Finance/Equity/Transaction?shareholderId=1&pageNumber=1&pageSize=20

Expected Response: 200 OK
{
  "isSuccess": true,
  "succeeded": true,
  "error": null,
  "value": {
    "data": [
      {
        "id": 1,
        "shareholderId": 1,
        "shareholderName": "John Doe",
        "type": 0,
        "amount": 1000000.0,
        "transactionDate": "2026-01-05T00:00:00Z",
        "description": "Additional investment",
        "reference": "INV-001"
      }
    ],
    "totalCount": 1
  },
  "data": { ... }
}
```

#### Test 3.7: Get Equity Report
```
GET /api/Finance/Equity/Report?asOfDate=2026-01-31

Expected Response: 200 OK
{
  "isSuccess": true,
  "succeeded": true,
  "error": null,
  "value": {
    "asOfDate": "2026-01-31T00:00:00Z",
    "totalEquity": 6000000.0,
    "shareholders": [
      {
        "shareholderId": 1,
        "shareholderName": "John Doe",
        "ownershipPercentage": 50.0,
        "totalInvestment": 6000000.0,
        "totalDrawings": 0,
        "netEquity": 6000000.0,
        "transactions": [ ... ]
      }
    ]
  },
  "data": { ... }
}
```

---

## Phase 4: General Ledger & Journal Entries

### Purpose
Record and manage all financial transactions.

### API Endpoints
- **Base Route:** `/api/Finance/JournalEntry`

#### Test 4.1: Create Journal Entry
```
POST /api/Finance/JournalEntry

Body:
{
  "entryDate": "2026-01-05",
  "description": "Opening balance deposit",
  "referenceNumber": "BANK-001",
  "referenceType": "BankDeposit",
  "branchId": null,
  "lines": [
    {
      "chartOfAccountId": 1,
      "description": "Cash deposit",
      "debitAmount": 5000000.0,
      "creditAmount": 0,
      "currencyId": null,
      "exchangeRate": 1.0
    },
    {
      "chartOfAccountId": 31,
      "description": "Capital contribution",
      "debitAmount": 0,
      "creditAmount": 5000000.0,
      "currencyId": null,
      "exchangeRate": 1.0
    }
  ]
}

Expected Response: 200 OK
{
  "id": {journalEntryId},
  "entryNumber": "JE-0001",
  "status": 0,
  "description": "Opening balance deposit"
}
```

**Status Values:** 0=Draft, 1=Unposted, 2=Posted, 3=Voided

#### Test 4.2: Get All Journal Entries
```
GET /api/Finance/JournalEntry?status=2&fromDate=2026-01-01&toDate=2026-01-31&pageNumber=1&pageSize=20

Expected Response: 200 OK
{
  "data": [
    {
      "id": {journalEntryId},
      "entryNumber": "JE-0001",
      "entryDate": "2026-01-05",
      "description": "Opening balance deposit",
      "status": 2,
      "referenceNumber": "BANK-001",
      "referenceType": "BankDeposit",
      "lines": [...]
    }
  ],
  "totalCount": 1,
  "pageNumber": 1,
  "pageSize": 20
}
```

#### Test 4.3: Get Journal Entry by ID
```
GET /api/Finance/JournalEntry/{journalEntryId}

Expected Response: 200 OK
{
  "id": {journalEntryId},
  "entryNumber": "JE-0001",
  "entryDate": "2026-01-05",
  "description": "Opening balance deposit",
  "status": 2,
  "referenceNumber": "BANK-001",
  "lines": [...]
}
```

#### Test 4.4: Post Journal Entry
```
POST /api/Finance/JournalEntry/{journalEntryId}/Post?dryRun=false

Query Parameter:
- dryRun (bool, optional): Validate without posting (default=false)

Expected Response: 200 OK
{
  "id": {journalEntryId},
  "entryNumber": "JE-0001",
  "status": 2,
  "message": "Entry posted successfully"
}
```

#### Test 4.5: Void Journal Entry
```
POST /api/Finance/JournalEntry/{journalEntryId}/Void

Body:
{
  "reason": "Entry recorded in error"
}

Expected Response: 200 OK
{
  "id": {journalEntryId},
  "status": 3,
  "message": "Entry voided successfully"
}
```

---

## Phase 5: General Ledger Queries

### Purpose
Query GL and verify account balances.

### API Endpoints
- **Base Route:** `/api/Finance/GeneralLedger`

#### Test 5.1: Get Account Ledger
```
GET /api/Finance/GeneralLedger/AccountLedger/{chartOfAccountId}?fromDate=2026-01-01&toDate=2026-01-31

Expected Response: 200 OK
{
  "accountId": {chartOfAccountId},
  "accountCode": "1101",
  "accountName": "Cash and Cash Equivalents",
  "entries": [
    {
      "journalEntryId": {journalEntryId},
      "entryDate": "2026-01-05",
      "description": "Opening balance deposit",
      "debitAmount": 5000000.0,
      "creditAmount": 0,
      "runningBalance": 5000000.0
    }
  ],
  "currentBalance": 5000000.0
}
```

#### Test 5.2: Get Account Balance as of Date
```
GET /api/Finance/GeneralLedger/AccountBalance/{chartOfAccountId}?asOf=2026-01-31

Expected Response: 200 OK
{
  "accountId": {chartOfAccountId},
  "accountCode": "1101",
  "accountName": "Cash and Cash Equivalents",
  "balance": 5000000.0,
  "asOfDate": "2026-01-31"
}
```

#### Test 5.3: Get Trial Balance
```
GET /api/Finance/GeneralLedger/TrialBalance?asOf=2026-01-31&branchId=null

Expected Response: 200 OK
{
  "asOfDate": "2026-01-31",
  "accounts": [
    {
      "accountCode": "1101",
      "accountName": "Cash and Cash Equivalents",
      "debitBalance": 5000000.0,
      "creditBalance": 0
    }
  ],
  "totals": {
    "totalDebits": 5000000.0,
    "totalCredits": 5000000.0,
    "difference": 0
  }
}
```

---

## Phase 6: Accounts Receivable

### Purpose
Track customer invoices and payments.

### API Endpoints
- **Base Route:** `/api/Finance/AccountsReceivable`

#### Test 6.1: Create AR Invoice
```
POST /api/Finance/AccountsReceivable

Body:
{
  "customerId": {customerId},
  "invoiceDate": "2026-01-06",
  "dueDate": "2026-02-06",
  "invoiceAmount": 100000.0,
  "chartOfAccountId": 2,
  "branchId": null,
  "visitId": null,
  "currencyId": null
}

Expected Response: 201 Created
{
  "id": {arId},
  "invoiceNumber": "INV-AR-0001",
  "customerId": {customerId},
  "invoiceAmount": 100000.0,
  "paidAmount": 0,
  "balanceAmount": 100000.0,
  "status": 1
}
```

**Status:** 1=Open, 2=PartiallyPaid, 3=Paid, 4=Overdue, 5=Cancelled

#### Test 6.2: Get All AR
```
GET /api/Finance/AccountsReceivable?status=1&customerId={customerId}&pageNumber=1&pageSize=20

Expected Response: 200 OK
{
  "data": [
    {
      "id": {arId},
      "invoiceNumber": "INV-AR-0001",
      "customerId": {customerId},
      "invoiceDate": "2026-01-06",
      "dueDate": "2026-02-06",
      "invoiceAmount": 100000.0,
      "paidAmount": 0,
      "balanceAmount": 100000.0,
      "status": 1
    }
  ],
  "totalCount": 1
}
```

#### Test 6.3: Get AR by ID
```
GET /api/Finance/AccountsReceivable/{arId}

Expected Response: 200 OK
{
  "id": {arId},
  "invoiceNumber": "INV-AR-0001",
  "customerId": {customerId},
  "invoiceAmount": 100000.0,
  "paidAmount": 0,
  "balanceAmount": 100000.0,
  "status": 1
}
```

#### Test 6.4: Issue AR Invoice
```
POST /api/Finance/AccountsReceivable/{arId}/Issue

Expected Response: 200 OK
{
  "id": {arId},
  "status": 2,
  "message": "AR issued successfully"
}
```

#### Test 6.5: Record Payment
```
POST /api/Finance/AccountsReceivable/{arId}/RecordPayment

Body:
{
  "accountsReceivableId": {arId},
  "receiptDate": "2026-01-10",
  "amountReceived": 50000.0,
  "paymentMethodId": 1,
  "reference": "CHK-001",
  "currencyId": null,
  "exchangeRate": 1.0
}

Expected Response: 200 OK
{
  "id": {arId},
  "paidAmount": 50000.0,
  "balanceAmount": 50000.0,
  "status": 2
}
```

#### Test 6.6: Get AR Aging Report
```
GET /api/Finance/Reporting/ARAgingReport?asOf=2026-01-31&branchId=null

Expected Response: 200 OK
{
  "asOfDate": "2026-01-31",
  "agingBuckets": {
    "current": 0,
    "days0to30": 50000.0,
    "days30to60": 0,
    "days60to90": 0,
    "days90plus": 0
  },
  "totalOutstanding": 50000.0,
  "details": [...]
}
```

#### Test 6.7: Get Overdue Receivables
```
GET /api/Finance/AccountsReceivable/Overdue?branchId=null

Expected Response: 200 OK
{
  "data": [
    {
      "id": {arId},
      "invoiceNumber": "INV-AR-0001",
      "customerId": {customerId},
      "invoiceAmount": 100000.0,
      "balanceAmount": 50000.0,
      "daysOverdue": 5,
      "status": 1
    }
  ],
  "totalCount": 1
}
```

---

## Phase 7: Sales Invoicing

### Purpose
Generate sales invoices and record receipts.

### API Endpoints
- **Base Route:** `/api/Finance/SalesInvoice`

#### Test 7.1: Create Sales Invoice
```
POST /api/Finance/SalesInvoice

Body:
{
  "customerId": {customerId},
  "invoiceDate": "2026-01-07",
  "salesArea": "OPD",
  "branchId": null,
  "lines": [
    {
      "serviceId": {serviceId},
      "inventoryItemId": null,
      "description": "Medical Consultation",
      "quantity": 1,
      "unitPrice": 30000.0,
      "lineTotal": 30000.0,
      "discountAmount": 0,
      "taxAmount": 3000.0
    },
    {
      "serviceId": null,
      "inventoryItemId": {itemId},
      "description": "Laboratory Test",
      "quantity": 1,
      "unitPrice": 20000.0,
      "lineTotal": 20000.0,
      "discountAmount": 0,
      "taxAmount": 2000.0
    }
  ]
}

Expected Response: 201 Created
{
  "id": {invoiceId},
  "invoiceNumber": "SI-0001",
  "customerId": {customerId},
  "invoiceDate": "2026-01-07",
  "totalAmount": 50000.0,
  "taxAmount": 5000.0,
  "discountAmount": 0,
  "netAmount": 55000.0,
  "status": 1,
  "salesArea": "OPD"
}
```

**Status:** 1=Draft, 2=Issued, 3=Paid, 4=Cancelled

#### Test 7.2: Get All Sales Invoices
```
GET /api/Finance/SalesInvoice?customerId={customerId}&status=2&pageNumber=1&pageSize=20

Expected Response: 200 OK
{
  "data": [
    {
      "id": {invoiceId},
      "invoiceNumber": "SI-0001",
      "customerId": {customerId},
      "customerName": "Patient Name",
      "invoiceDate": "2026-01-07",
      "totalAmount": 50000.0,
      "status": 2
    }
  ],
  "totalCount": 1
}
```

#### Test 7.3: Get Sales Invoice by ID
```
GET /api/Finance/SalesInvoice/{invoiceId}

Expected Response: 200 OK
{
  "id": {invoiceId},
  "invoiceNumber": "SI-0001",
  "customerId": {customerId},
  "customerName": "Patient Name",
  "invoiceDate": "2026-01-07",
  "totalAmount": 55000.0,
  "taxAmount": 5000.0,
  "netAmount": 55000.0,
  "status": 2,
  "lines": [...]
}
```

#### Test 7.4: Issue Sales Invoice
```
POST /api/Finance/SalesInvoice/{invoiceId}/Issue

Expected Response: 200 OK
{
  "id": {invoiceId},
  "status": 2,
  "message": "Invoice issued successfully"
}
```

#### Test 7.5: Record Sales Receipt
```
POST /api/Finance/SalesInvoice/{invoiceId}/Receipt

Body:
{
  "salesInvoiceId": {invoiceId},
  "amountReceived": 55000.0,
  "paymentMethodId": 1,
  "receiptDate": "2026-01-08",
  "reference": "CASH-001"
}

Expected Response: 200 OK
{
  "id": {receiptId},
  "salesInvoiceId": {invoiceId},
  "receiptNumber": "SR-0001",
  "customerId": {customerId},
  "amountReceived": 55000.0,
  "receiptDate": "2026-01-08"
}
```

#### Test 7.6: Get Receipts for Invoice
```
GET /api/Finance/SalesInvoice/{invoiceId}/Receipts

Expected Response: 200 OK
{
  "data": [
    {
      "id": {receiptId},
      "salesInvoiceId": {invoiceId},
      "receiptNumber": "SR-0001",
      "customerId": {customerId},
      "amountReceived": 55000.0,
      "receiptDate": "2026-01-08"
    }
  ],
  "totalCount": 1
}
```

---

## Phase 8: Purchase Orders & Vendor Bills

### Purpose
Manage procurement and vendor payments.

### API Endpoints
- **Base Route:** `/api/Finance/Procurement`

#### Test 8.1: Create Purchase Order
```
POST /api/Finance/Procurement/PurchaseOrder

Body:
{
  "vendorId": {vendorId},
  "orderDate": "2026-01-05",
  "expectedDeliveryDate": "2026-01-15",
  "branchId": null,
  "lines": [
    {
      "itemId": {itemId},
      "description": "Surgical Gloves (Box of 100)",
      "quantity": 10,
      "unitPrice": 5000.0,
      "lineTotal": 50000.0
    },
    {
      "itemId": {itemId2},
      "description": "Sterile Gauze",
      "quantity": 20,
      "unitPrice": 2000.0,
      "lineTotal": 40000.0
    }
  ]
}

Expected Response: 201 Created
{
  "id": {poId},
  "poNumber": "PO-0001",
  "vendorId": {vendorId},
  "orderDate": "2026-01-05",
  "totalAmount": 90000.0,
  "status": 1
}
```

**Status:** 1=Draft, 2=Approved, 3=Received, 4=Completed, 5=Cancelled

#### Test 8.2: Get All Purchase Orders
```
GET /api/Finance/Procurement/PurchaseOrder?vendorId={vendorId}&status=2&pageNumber=1&pageSize=20

Expected Response: 200 OK
{
  "data": [
    {
      "id": {poId},
      "poNumber": "PO-0001",
      "vendorId": {vendorId},
      "orderDate": "2026-01-05",
      "totalAmount": 90000.0,
      "status": 2
    }
  ],
  "totalCount": 1
}
```

#### Test 8.3: Get Purchase Order by ID
```
GET /api/Finance/Procurement/PurchaseOrder/{poId}

Expected Response: 200 OK
{
  "id": {poId},
  "poNumber": "PO-0001",
  "vendorId": {vendorId},
  "orderDate": "2026-01-05",
  "expectedDeliveryDate": "2026-01-15",
  "totalAmount": 90000.0,
  "status": 2,
  "lines": [...]
}
```

#### Test 8.4: Update Purchase Order
```
PUT /api/Finance/Procurement/PurchaseOrder/{poId}

Body:
{
  "id": {poId},
  "orderDate": "2026-01-05",
  "expectedDeliveryDate": "2026-01-20",
  "lines": [...]
}

Expected Response: 200 OK
```

#### Test 8.5: Receive Purchase Order
```
POST /api/Finance/Procurement/PurchaseOrder/{poId}/Receive

Expected Response: 200 OK
{
  "id": {poId},
  "status": 3,
  "message": "Purchase order received"
}
```

#### Test 8.6: Create Vendor Bill
```
POST /api/Finance/Procurement/VendorBill

Body:
{
  "purchaseOrderId": {poId},
  "vendorId": {vendorId},
  "billDate": "2026-01-10",
  "dueDate": "2026-02-10",
  "totalAmount": 90000.0,
  "branchId": null
}

Expected Response: 201 Created
{
  "id": {billId},
  "billNumber": "VB-0001",
  "purchaseOrderId": {poId},
  "vendorId": {vendorId},
  "billDate": "2026-01-10",
  "totalAmount": 90000.0,
  "status": 1
}
```

#### Test 8.7: Get All Vendor Bills
```
GET /api/Finance/Procurement/VendorBill?vendorId={vendorId}&status=2&pageNumber=1&pageSize=20

Expected Response: 200 OK
{
  "data": [
    {
      "id": {billId},
      "billNumber": "VB-0001",
      "purchaseOrderId": {poId},
      "vendorId": {vendorId},
      "billDate": "2026-01-10",
      "totalAmount": 90000.0,
      "status": 2
    }
  ],
  "totalCount": 1
}
```

#### Test 8.8: Get Vendor Bill by ID
```
GET /api/Finance/Procurement/VendorBill/{billId}

Expected Response: 200 OK
{
  "id": {billId},
  "billNumber": "VB-0001",
  "purchaseOrderId": {poId},
  "vendorId": {vendorId},
  "billDate": "2026-01-10",
  "dueDate": "2026-02-10",
  "totalAmount": 90000.0,
  "status": 2
}
```

#### Test 8.9: Mark Vendor Bill as Paid
```
POST /api/Finance/Procurement/VendorBill/{billId}/MarkAsPaid

Expected Response: 200 OK
{
  "id": {billId},
  "status": 4,
  "message": "Bill marked as paid"
}
```

---

## Phase 9: Expense Management

### Purpose
Track and approve employee expenses.

### API Endpoints
- **Base Route:** `/api/Finance/Expense`

#### Test 9.1: Create Expense
```
POST /api/Finance/Expense

Body:
{
  "categoryId": {categoryId},
  "classId": {classId},
  "amount": 15000.0,
  "expenseDate": "2026-01-08",
  "description": "Client meeting meal expense",
  "customerId": {employeeId},
  "isReimbursable": true,
  "chartOfAccountId": 52,
  "branchId": null,
  "attachmentPath": "/attachments/receipt.pdf"
}

Expected Response: 201 Created
{
  "id": {expenseId},
  "amount": 15000.0,
  "status": 1,
  "description": "Client meeting meal expense"
}
```

**Status:** 1=Draft, 2=Submitted, 3=Approved, 4=Rejected, 5=Paid

#### Test 9.2: Get All Expenses
```
GET /api/Finance/Expense?status=2&categoryId={categoryId}&pageNumber=1&pageSize=10

Expected Response: 200 OK
{
  "data": [
    {
      "id": {expenseId},
      "categoryId": {categoryId},
      "amount": 15000.0,
      "expenseDate": "2026-01-08",
      "description": "Client meeting meal expense",
      "status": 2
    }
  ],
  "totalCount": 1
}
```

#### Test 9.3: Get Expense by ID
```
GET /api/Finance/Expense/{expenseId}

Expected Response: 200 OK
{
  "id": {expenseId},
  "categoryId": {categoryId},
  "classId": {classId},
  "amount": 15000.0,
  "expenseDate": "2026-01-08",
  "description": "Client meeting meal expense",
  "status": 2
}
```

#### Test 9.4: Submit Expense
```
POST /api/Finance/Expense/{expenseId}/Submit

Expected Response: 200 OK
{
  "id": {expenseId},
  "status": 2,
  "message": "Expense submitted for approval"
}
```

#### Test 9.5: Approve Expense
```
POST /api/Finance/Expense/{expenseId}/Approve

Expected Response: 200 OK
{
  "id": {expenseId},
  "status": 3,
  "message": "Expense approved"
}
```

#### Test 9.6: Reject Expense
```
POST /api/Finance/Expense/{expenseId}/Reject

Body:
{
  "id": {expenseId},
  "rejectionReason": "Missing receipt"
}

Expected Response: 200 OK
{
  "id": {expenseId},
  "status": 4,
  "rejectionReason": "Missing receipt"
}
```

#### Test 9.7: Get Pending Approvals
```
GET /api/Finance/Expense/Pending?pageNumber=1&pageSize=10

Expected Response: 200 OK
{
  "data": [
    {
      "id": {expenseId},
      "amount": 15000.0,
      "description": "Client meeting meal expense",
      "status": 2,
      "submittedDate": "2026-01-08"
    }
  ],
  "totalCount": 1
}
```

---

## Phase 10: Financial Reporting

### Purpose
Generate financial statements and reports.

### API Endpoints
- **Base Route:** `/api/Finance/Reporting`

#### Test 10.1: Get Trial Balance (via Reporting)
```
GET /api/Finance/Reporting/TrialBalance?asOf=2026-01-31&branchId=null

Expected Response: 200 OK
{
  "asOfDate": "2026-01-31",
  "accounts": [
    {
      "accountCode": "1101",
      "accountName": "Cash and Cash Equivalents",
      "debitBalance": 4500000.0,
      "creditBalance": 0
    }
  ],
  "totals": {
    "totalDebits": 4500000.0,
    "totalCredits": 5000000.0,
    "difference": -500000.0
  }
}
```

#### Test 10.2: Export Trial Balance
```
POST /api/Finance/Reporting/TrialBalance/Export

Body:
{
  "format": "Excel",
  "asOfDate": "2026-01-31",
  "branchId": null
}

Expected Response: 200 OK
{
  "message": "Trial balance export initiated",
  "status": "pending",
  "format": "Excel"
}
```

#### Test 10.3: Get Income Statement
```
GET /api/Finance/Reporting/IncomeStatement?fromDate=2026-01-01&toDate=2026-01-31&branchId=null

Expected Response: 200 OK
{
  "period": "January 2026",
  "revenue": {
    "serviceRevenue": 500000.0,
    "otherRevenue": 0,
    "totalRevenue": 500000.0
  },
  "costOfGoodsSold": 90000.0,
  "grossProfit": 410000.0,
  "operatingExpenses": 108000.0,
  "operatingIncome": 302000.0,
  "netIncome": 302000.0
}
```

#### Test 10.4: Compare Income Statements
```
POST /api/Finance/Reporting/IncomeStatement/Compare?fromDate1=2025-12-01&toDate1=2025-12-31&fromDate2=2026-01-01&toDate2=2026-01-31

Expected Response: 200 OK
{
  "period1": "December 2025",
  "period2": "January 2026",
  "revenue": {
    "period1": 450000.0,
    "period2": 500000.0,
    "difference": 50000.0,
    "percentageChange": 11.11
  },
  "netIncome": {
    "period1": 280000.0,
    "period2": 302000.0,
    "difference": 22000.0
  }
}
```

#### Test 10.5: Get Balance Sheet
```
GET /api/Finance/Reporting/BalanceSheet?asOf=2026-01-31&branchId=null

Expected Response: 200 OK
{
  "asOfDate": "2026-01-31",
  "assets": {
    "currentAssets": 4500000.0,
    "fixedAssets": 491666.67,
    "otherAssets": 0
  },
  "liabilities": {
    "currentLiabilities": 100000.0,
    "longTermLiabilities": 0
  },
  "equity": {
    "capital": 5000000.0,
    "retainedEarnings": 302000.0
  },
  "totals": {
    "totalAssets": 4991666.67,
    "totalLiabilities": 100000.0,
    "totalEquity": 5302000.0
  }
}
```

#### Test 10.6: Get AP Aging Report
```
GET /api/Finance/Reporting/APAgingReport?asOf=2026-01-31&branchId=null

Expected Response: 200 OK
{
  "asOfDate": "2026-01-31",
  "agingBuckets": {
    "current": 0,
    "days0to30": 100000.0,
    "days30to60": 0,
    "days60to90": 0,
    "days90plus": 0
  },
  "totalOutstanding": 100000.0
}
```

#### Test 10.7: Get Budget Variance Report
```
GET /api/Finance/Budget/{budgetId}/VarianceReport?varianceThresholdAmount=10000&varianceThresholdPercentage=5&accountType=5&sortBy=Variance&sortDescending=true&pageNumber=1&pageSize=20

Query Parameters:
- varianceThresholdAmount (decimal, optional): Filter variances >= amount
- varianceThresholdPercentage (decimal, optional): Filter variances >= percentage
- accountType (int, optional): Filter by account type
- sortBy (string, optional): AccountCode, AccountName, Variance, VariancePercentage, BudgetedAmount, ActualAmount
- sortDescending (bool, optional): Sort direction
- pageNumber (int, optional): Page number
- pageSize (int, optional): Page size

Expected Response: 200 OK
{
  "data": [
    {
      "budgetLineId": {lineId},
      "chartOfAccountId": 52,
      "accountCode": "5101",
      "accountName": "Salaries",
      "accountType": 5,
      "budgetedAmount": 100000.0,
      "actualAmount": 95000.0,
      "variance": 5000.0,
      "variancePercentage": 5.0
    }
  ],
  "totalCount": 3,
  "pageNumber": 1,
  "pageSize": 20
}
```

---

## Phase 11: Audit Trail

### Purpose
Track financial activities for compliance.

### API Endpoints
- **Base Route:** `/api/Finance/AuditTrail`

#### Test 11.1: Get Audit Trail by Entity
```
GET /api/Finance/AuditTrail/ByEntity?entityType=JournalEntry&entityId={journalEntryId}

Expected Response: 200 OK
{
  "entityType": "JournalEntry",
  "entityId": {journalEntryId},
  "auditRecords": [
    {
      "id": {auditId},
      "action": "Create",
      "userName": "admin@crystalclinic.local",
      "timestamp": "2026-01-05T10:30:00Z",
      "ipAddress": "192.168.1.100"
    }
  ]
}
```

#### Test 11.2: Search Audit Trail
```
GET /api/Finance/AuditTrail/Search?entityType=SalesInvoice&action=Create&fromDate=2026-01-01&toDate=2026-01-31&userName=admin@crystalclinic.local

Expected Response: 200 OK
{
  "results": [
    {
      "id": {auditId},
      "entityType": "SalesInvoice",
      "entityId": {invoiceId},
      "action": "Create",
      "userName": "admin@crystalclinic.local",
      "timestamp": "2026-01-07T09:00:00Z"
    }
  ],
  "totalCount": 5
}
```

---

## Test Execution Checklist

- [ ] Phase 1: Setup (Currency, Branch, Exchange Rates) - 17 tests
- [ ] Phase 2: Chart of Accounts - 7 tests
- [ ] Phase 3: Equity & Shareholders - 7 tests
- [ ] Phase 4: Journal Entries - 5 tests
- [ ] Phase 5: General Ledger - 3 tests
- [ ] Phase 6: Accounts Receivable - 7 tests
- [ ] Phase 7: Sales Invoicing - 6 tests
- [ ] Phase 8: Procurement (PO & Vendor Bills) - 9 tests
- [ ] Phase 9: Expenses - 7 tests
- [ ] Phase 10: Financial Reporting - 7 tests
- [ ] Phase 11: Audit Trail - 2 tests

**Total Tests:** 77+

---

## Key Corrections from Previous Versions

### Major Changes in v3:
1. **Added Look Module APIs** - Full Currency, Branch, and Exchange Rate testing
2. **Corrected AR Payment** - Changed from wrong field names to actual CreateReceiptDto fields
3. **Corrected PO Lines** - Removed chartOfAccountId from line items, use ItemId only
4. **Corrected Vendor Bill** - Fixed all field names and removed non-existent status field
5. **Removed Status from Create AR** - Status is set by system, not in creation body
6. **All Payloads Validated** - Every payload now matches actual DTO definitions

---

## Known Limitations

- ⚠️ Department APIs not implemented yet (future phase)
- ⚠️ Export endpoints are placeholder implementations (no actual file generation)
- ⚠️ Some calculations (aging, variance) depend on transaction data
