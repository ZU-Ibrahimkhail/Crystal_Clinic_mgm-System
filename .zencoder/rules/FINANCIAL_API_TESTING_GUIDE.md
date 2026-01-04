# Financial System API Testing Guide

## Document Purpose
This guide provides a comprehensive, sequential testing plan for the Crystal Clinic Financial System APIs. Tests are organized by business workflow order, starting with foundational setup and progressing through operational features. Each section includes prerequisites, API endpoints, and practical test scenarios.

**Last Updated:** January 2026  
**Target System:** Crystal Clinic Service Management System - Financial Module  
**API Base URL:** `https://api.crystalclinic.local/api/Finance`

---

## Table of Contents
1. [Testing Prerequisites](#testing-prerequisites)
2. [Phase 1: Company Setup (Equity & Shareholders)](#phase-1-company-setup)
3. [Phase 2: Chart of Accounts Setup](#phase-2-chart-of-accounts)
4. [Phase 3: General Ledger & Transactions](#phase-3-general-ledger)
5. [Phase 4: Accounts Receivable (AR)](#phase-4-accounts-receivable)
6. [Phase 5: Accounts Payable (AP)](#phase-5-accounts-payable)
7. [Phase 6: Sales Invoicing & Receipts](#phase-6-sales-invoicing)
8. [Phase 7: Purchase Orders & Vendor Bills](#phase-7-purchase-orders)
9. [Phase 8: Expense Management](#phase-8-expense-management)
10. [Phase 9: Fixed Assets](#phase-9-fixed-assets)
11. [Phase 10: Budget Management](#phase-10-budget-management)
12. [Phase 11: Bank Reconciliation](#phase-11-bank-reconciliation)
13. [Phase 12: Financial Reporting](#phase-12-financial-reporting)
14. [Phase 13: Audit & Compliance](#phase-13-audit-compliance)

---

## Testing Prerequisites

### Test Environment Setup
- **Authentication:** All endpoints require Bearer token with `[Authorize]` attribute
- **Test User:** Create test user with Admin role
- **Test Database:** Use dedicated test database with clean state
- **Base Currency:** AFN (Afghan Afghani)

### Prerequisite API Calls Before Testing

#### 1. Authentication (Required for all tests)
```
POST /Identity/Auth/Login
Body: {
  "email": "admin@crystalclinic.local",
  "password": "TestPassword123!"
}
Response: Bearer token (save for all subsequent requests)
Header: Authorization: Bearer {token}
```

#### 2. Create Test Branch (if not exists)
```
POST /Setup/Branch
Body: {
  "branchCode": "HQ",
  "branchName": "Headquarters",
  "address": "Kabul",
  "branchType": 1,
  "isActive": true
}
Response: branchId (required for branch-specific operations)
```

#### 3. Create Test Currency (if not exists)
```
POST /Setup/Currency
Body: {
  "currencyCode": "AFN",
  "currencyName": "Afghan Afghani",
  "symbol": "؋"
}
```

#### 4. Create Test Department
```
POST /HR/Department
Body: {
  "departmentName": "Finance",
  "departmentCode": "FIN",
  "branchId": {branchId}
}
```

---

## Phase 1: Company Setup (Equity & Shareholders)

### Purpose
Establish company ownership structure and shareholder information.

### Business Flow
1. Register shareholders/owners
2. Track ownership percentages
3. Record share issuance
4. Manage equity transactions

### Test Scenarios

#### Test 1.1: Create Shareholder
```
POST /Finance/Equity/Shareholders
Body: {
  "shareholderName": "John Doe",
  "shareholderType": 1,
  "ownershipPercentage": 50.0,
  "emailAddress": "john@crystalclinic.local",
  "phoneNumber": "+93 700 000 001",
  "address": "Kabul",
  "countryId": 1,
  "isPrimaryOwner": true
}

Expected Response: 201 Created
{
  "id": {shareholderId},
  "shareholderName": "John Doe",
  "ownershipPercentage": 50.0,
  "status": "Active"
}
```

**Validation Points:**
- ✓ Shareholder created with unique ID
- ✓ Ownership percentage saved correctly
- ✓ Primary owner flag set
- ✓ Status defaults to "Active"

#### Test 1.2: Create Additional Shareholder
```
POST /Finance/Equity/Shareholders
Body: {
  "shareholderName": "Jane Smith",
  "shareholderType": 1,
  "ownershipPercentage": 50.0,
  "emailAddress": "jane@crystalclinic.local",
  "phoneNumber": "+93 700 000 002",
  "address": "Kabul",
  "countryId": 1,
  "isPrimaryOwner": false
}

Expected Response: 201 Created
```

**Validation Points:**
- ✓ Second shareholder can own same percentage
- ✓ Only one primary owner allowed
- ✓ Total ownership can be 100% or more (for testing)

#### Test 1.3: Get All Shareholders
```
GET /Finance/Equity/Shareholders

Expected Response: 200 OK
{
  "data": [
    {
      "id": {shareholderId},
      "shareholderName": "John Doe",
      "ownershipPercentage": 50.0,
      "isPrimaryOwner": true
    },
    {
      "id": {shareholderId2},
      "shareholderName": "Jane Smith",
      "ownershipPercentage": 50.0,
      "isPrimaryOwner": false
    }
  ],
  "totalCount": 2
}
```

#### Test 1.4: Record Equity Transaction (Share Issuance)
```
POST /Finance/Equity/Transactions
Body: {
  "transactionType": 1,
  "shareholderId": {shareholderId},
  "sharesIssued": 1000,
  "sharePrice": 1000.0,
  "totalAmount": 1000000.0,
  "transactionDate": "2026-01-01",
  "description": "Initial share issuance"
}

Expected Response: 201 Created
{
  "id": {transactionId},
  "transactionType": "ShareIssuance",
  "totalAmount": 1000000.0,
  "status": "Completed"
}
```

#### Test 1.5: Get Equity Report
```
GET /Finance/Equity/Report

Expected Response: 200 OK
{
  "totalEquity": 2000000.0,
  "shareholders": [
    {
      "shareholderName": "John Doe",
      "ownershipPercentage": 50.0,
      "equityValue": 1000000.0
    },
    {
      "shareholderName": "Jane Smith",
      "ownershipPercentage": 50.0,
      "equityValue": 1000000.0
    }
  ]
}
```

---

## Phase 2: Chart of Accounts Setup

### Purpose
Configure the account structure that all financial transactions will reference.

### Business Flow
1. Review system-provided accounts
2. Create custom user accounts (if needed)
3. Establish account hierarchy
4. Verify normal balances and types

### Test Scenarios

#### Test 2.1: Get Chart of Accounts (System Accounts)
```
GET /Finance/ChartOfAccounts?isSystemAccount=true

Expected Response: 200 OK
{
  "data": [
    {
      "id": 1,
      "accountCode": "1101",
      "accountName": "Cash and Cash Equivalents",
      "accountType": 1,
      "normalBalance": 0,
      "isSystemAccount": true,
      "isActive": true
    },
    {
      "id": 2,
      "accountCode": "1102",
      "accountName": "Accounts Receivable",
      "accountType": 1,
      "normalBalance": 0,
      "isSystemAccount": true
    },
    ...
  ],
  "totalCount": 25
}
```

**Validation Points:**
- ✓ All required accounts present (Assets, Liabilities, Equity, Revenue, Expense)
- ✓ Account types correct (1=Asset, 2=Liability, 3=Equity, 4=Revenue, 5=Expense)
- ✓ Normal balance types set (0=Debit, 1=Credit)
- ✓ System accounts cannot be deleted (test delete attempt should fail)

#### Test 2.2: Create Custom Account
```
POST /Finance/ChartOfAccounts
Body: {
  "accountCode": "5201",
  "accountName": "Medical Supplies Expense",
  "accountType": 5,
  "accountCategory": 500,
  "normalBalance": 0,
  "description": "Expenses for medical supplies",
  "parentAccountId": null,
  "isActive": true
}

Expected Response: 201 Created
{
  "id": {accountId},
  "accountCode": "5201",
  "accountName": "Medical Supplies Expense",
  "accountType": 5,
  "isActive": true
}
```

#### Test 2.3: Update Custom Account
```
PUT /Finance/ChartOfAccounts/{accountId}
Body: {
  "accountName": "Medical & Surgical Supplies Expense",
  "description": "Updated description"
}

Expected Response: 200 OK
```

#### Test 2.4: Get Account Hierarchy
```
GET /Finance/ChartOfAccounts/Hierarchy

Expected Response: 200 OK
{
  "id": 1,
  "accountCode": "1000",
  "accountName": "Assets",
  "children": [
    {
      "id": 10,
      "accountCode": "1100",
      "accountName": "Current Assets",
      "children": [
        {
          "id": 1,
          "accountCode": "1101",
          "accountName": "Cash and Cash Equivalents"
        }
      ]
    }
  ]
}
```

#### Test 2.5: Move Account in Hierarchy
```
POST /Finance/ChartOfAccounts/{accountId}/Move
Body: {
  "newParentAccountId": {newParentId}
}

Expected Response: 200 OK
{
  "id": {accountId},
  "parentAccountId": {newParentId}
}
```

---

## Phase 3: General Ledger & Transactions

### Purpose
Record and manage all financial transactions through journal entries.

### Business Flow
1. Create journal entries (debits/credits)
2. Post entries to GL
3. Verify GL balances
4. Generate trial balance

### Test Scenarios

#### Test 3.1: Create Journal Entry (Unposted)
```
POST /Finance/JournalEntry
Body: {
  "entryDate": "2026-01-05",
  "description": "Opening balance deposit",
  "referenceNumber": "BANK-001",
  "referenceType": "BankDeposit",
  "branchId": {branchId},
  "lines": [
    {
      "chartOfAccountId": 1,
      "debitAmount": 5000000.0,
      "creditAmount": 0,
      "description": "Cash deposit",
      "currencyId": 1
    },
    {
      "chartOfAccountId": 31,
      "debitAmount": 0,
      "creditAmount": 5000000.0,
      "description": "Capital contribution",
      "currencyId": 1
    }
  ]
}

Expected Response: 201 Created
{
  "id": {journalEntryId},
  "entryNumber": "JE-0001",
  "status": 1,
  "description": "Opening balance deposit"
}
```

**Validation Points:**
- ✓ Journal entry created in Draft status
- ✓ Entry number auto-generated
- ✓ Two lines with balanced debits and credits
- ✓ Total debits = Total credits (5000000.0)

#### Test 3.2: Post Journal Entry
```
POST /Finance/JournalEntry/{journalEntryId}/Post
Body: {}

Expected Response: 200 OK
{
  "id": {journalEntryId},
  "status": 2,
  "message": "Entry posted successfully"
}
```

**Validation Points:**
- ✓ Status changes from Draft (1) to Posted (2)
- ✓ GL entries created for both accounts
- ✓ GL balances updated

#### Test 3.3: Get General Ledger by Account
```
GET /Finance/GeneralLedger/Account/{accountId}

Expected Response: 200 OK
{
  "accountId": {accountId},
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

#### Test 3.4: Get Account Balance as of Date
```
GET /Finance/GeneralLedger/Account/{accountId}/Balance?asOfDate=2026-01-05

Expected Response: 200 OK
{
  "accountId": {accountId},
  "accountCode": "1101",
  "accountName": "Cash and Cash Equivalents",
  "balance": 5000000.0,
  "asOfDate": "2026-01-05"
}
```

#### Test 3.5: Get Trial Balance
```
GET /Finance/Reporting/TrialBalance?asOf=2026-01-05&branchId={branchId}

Expected Response: 200 OK
{
  "asOfDate": "2026-01-05",
  "accounts": [
    {
      "accountCode": "1101",
      "accountName": "Cash and Cash Equivalents",
      "debitBalance": 5000000.0,
      "creditBalance": 0
    },
    {
      "accountCode": "3101",
      "accountName": "Capital",
      "debitBalance": 0,
      "creditBalance": 5000000.0
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

## Phase 4: Accounts Receivable (AR)

### Purpose
Track customer invoices and payments.

### Business Flow
1. Create AR invoice
2. Record customer payments
3. Generate AR aging report
4. Identify overdue accounts

### Test Scenarios

#### Test 4.1: Create Accounts Receivable Invoice
```
POST /Finance/AccountsReceivable
Body: {
  "customerId": {customerId},
  "invoiceDate": "2026-01-06",
  "dueDate": "2026-02-06",
  "invoiceAmount": 100000.0,
  "description": "Medical services provided",
  "branchId": {branchId},
  "chartOfAccountId": 2,
  "visitId": {visitId}
}

Expected Response: 201 Created
{
  "id": {arId},
  "invoiceNumber": "INV-0001",
  "customerId": {customerId},
  "invoiceAmount": 100000.0,
  "paidAmount": 0,
  "balanceAmount": 100000.0,
  "status": 1
}
```

**Validation Points:**
- ✓ Invoice number auto-generated
- ✓ Status set to "Open" (1)
- ✓ Balance = Invoice amount initially
- ✓ Invoice posted to AR account (1102)

#### Test 4.2: Record Partial Payment
```
POST /Finance/AccountsReceivable/{arId}/Payment
Body: {
  "paymentAmount": 50000.0,
  "paymentDate": "2026-01-10",
  "description": "Partial payment received",
  "referenceNumber": "CHK-001"
}

Expected Response: 200 OK
{
  "id": {arId},
  "paidAmount": 50000.0,
  "balanceAmount": 50000.0,
  "status": 2
}
```

**Validation Points:**
- ✓ Status changes to "PartiallyPaid" (2)
- ✓ GL entries created for payment
- ✓ Cash account credited
- ✓ AR account debited

#### Test 4.3: Record Full Payment
```
POST /Finance/AccountsReceivable/{arId}/Payment
Body: {
  "paymentAmount": 50000.0,
  "paymentDate": "2026-01-15",
  "description": "Final payment"
}

Expected Response: 200 OK
{
  "id": {arId},
  "paidAmount": 100000.0,
  "balanceAmount": 0,
  "status": 3
}
```

**Validation Points:**
- ✓ Status changes to "Paid" (3)
- ✓ Balance becomes zero

#### Test 4.4: Get AR Aging Report
```
GET /Finance/Reporting/ARAgingReport?asOf=2026-01-20&branchId={branchId}

Expected Response: 200 OK
{
  "asOfDate": "2026-01-20",
  "agingBuckets": {
    "current": 0,
    "days0to30": 50000.0,
    "days30to60": 0,
    "days60to90": 0,
    "days90plus": 0
  },
  "totalOutstanding": 50000.0,
  "details": [
    {
      "invoiceNumber": "INV-0001",
      "customerId": {customerId},
      "dueDate": "2026-02-06",
      "balanceAmount": 50000.0,
      "daysOverdue": -17
    }
  ]
}
```

#### Test 4.5: Create Overdue Scenario
Create AR invoice with past due date and get overdue list:
```
GET /Finance/AccountsReceivable/Overdue?branchId={branchId}

Expected Response: 200 OK
{
  "data": [
    {
      "id": {arId},
      "invoiceNumber": "INV-0002",
      "customerId": {customerId},
      "invoiceAmount": 50000.0,
      "balanceAmount": 50000.0,
      "daysOverdue": 5,
      "status": 1
    }
  ],
  "totalCount": 1
}
```

---

## Phase 5: Accounts Payable (AP)

### Purpose
Track vendor invoices and payments owed.

### Business Flow
1. Create AP invoice
2. Record vendor payments
3. Generate AP aging report
4. Identify overdue payments

### Test Scenarios

#### Test 5.1: Create Accounts Payable Invoice
```
POST /Finance/AccountsPayable
Body: {
  "vendorId": {vendorId},
  "invoiceDate": "2026-01-06",
  "dueDate": "2026-02-06",
  "invoiceAmount": 200000.0,
  "invoiceNumber": "VND-0001",
  "description": "Medical supplies purchase",
  "branchId": {branchId},
  "chartOfAccountId": 51,
  "poId": {poId}
}

Expected Response: 201 Created
{
  "id": {apId},
  "vendorId": {vendorId},
  "invoiceAmount": 200000.0,
  "paidAmount": 0,
  "balanceAmount": 200000.0,
  "status": 1
}
```

#### Test 5.2: Record Partial Payment
```
POST /Finance/AccountsPayable/{apId}/Payment
Body: {
  "paymentAmount": 100000.0,
  "paymentDate": "2026-01-12",
  "description": "Partial payment to vendor",
  "referenceNumber": "CHK-002"
}

Expected Response: 200 OK
{
  "id": {apId},
  "paidAmount": 100000.0,
  "balanceAmount": 100000.0,
  "status": 2
}
```

**Validation Points:**
- ✓ Status changes to "PartiallyPaid"
- ✓ GL entries: AP account debited, Cash credited

#### Test 5.3: Get AP Aging Report
```
GET /Finance/Reporting/APAgingReport?asOf=2026-01-20&branchId={branchId}

Expected Response: 200 OK
{
  "asOfDate": "2026-01-20",
  "agingBuckets": {
    "current": 0,
    "days0to30": 100000.0,
    "days30to60": 0,
    "days60to90": 0,
    "days90plus": 0
  },
  "totalOutstanding": 100000.0,
  "details": [
    {
      "invoiceNumber": "VND-0001",
      "vendorId": {vendorId},
      "dueDate": "2026-02-06",
      "balanceAmount": 100000.0,
      "daysOverdue": -17
    }
  ]
}
```

#### Test 5.4: Get Overdue Payables
```
GET /Finance/AccountsPayable/Overdue?branchId={branchId}

Expected Response: 200 OK
{
  "data": [
    {
      "id": {apId},
      "invoiceNumber": "VND-0002",
      "vendorId": {vendorId},
      "invoiceAmount": 150000.0,
      "balanceAmount": 150000.0,
      "daysOverdue": 3,
      "status": 1
    }
  ],
  "totalCount": 1
}
```

---

## Phase 6: Sales Invoicing & Receipts

### Purpose
Generate sales invoices from visits and record cash receipts.

### Business Flow
1. Convert visit to sales invoice
2. Record receipt payment
3. Auto-create GL entries
4. Track revenue recognition

### Test Scenarios

#### Test 6.1: Create Sales Invoice
```
POST /Finance/SalesInvoice
Body: {
  "visitId": {visitId},
  "invoiceDate": "2026-01-07",
  "dueDate": "2026-02-07",
  "totalAmount": 50000.0,
  "taxAmount": 5000.0,
  "description": "Clinic visit - Dr. Ahmed",
  "branchId": {branchId},
  "customerId": {customerId},
  "invoiceStatus": 1,
  "items": [
    {
      "description": "Medical Consultation",
      "unitPrice": 30000.0,
      "quantity": 1,
      "amount": 30000.0,
      "chartOfAccountId": 41
    },
    {
      "description": "Laboratory Test",
      "unitPrice": 20000.0,
      "quantity": 1,
      "amount": 20000.0,
      "chartOfAccountId": 41
    }
  ]
}

Expected Response: 201 Created
{
  "id": {invoiceId},
  "invoiceNumber": "SI-0001",
  "visitId": {visitId},
  "totalAmount": 50000.0,
  "invoiceStatus": 1
}
```

**Validation Points:**
- ✓ Invoice created with auto-generated number
- ✓ Status set to Draft/Pending
- ✓ Items linked to revenue accounts
- ✓ Total = sum of items + tax

#### Test 6.2: Post Sales Invoice
```
POST /Finance/SalesInvoice/{invoiceId}/Post
Body: {}

Expected Response: 200 OK
{
  "id": {invoiceId},
  "invoiceStatus": 2,
  "glEntries": {
    "revenuePosted": 50000.0,
    "arCreated": 50000.0
  }
}
```

**Validation Points:**
- ✓ Status changes to Posted
- ✓ GL entries created: Debit AR (1102), Credit Revenue (41xx)
- ✓ AR record created for customer

#### Test 6.3: Create Sales Receipt (Payment)
```
POST /Finance/SalesReceipt
Body: {
  "invoiceId": {invoiceId},
  "paymentAmount": 50000.0,
  "paymentDate": "2026-01-08",
  "paymentMethod": 1,
  "referenceNumber": "CASH-001",
  "description": "Cash payment for invoice",
  "branchId": {branchId}
}

Expected Response: 201 Created
{
  "id": {receiptId},
  "receiptNumber": "SR-0001",
  "paymentAmount": 50000.0,
  "paymentStatus": 1
}
```

**Validation Points:**
- ✓ Receipt created with unique number
- ✓ GL entries: Debit Cash, Credit AR
- ✓ AR balance updated to zero

#### Test 6.4: Get Sales Invoice Details
```
GET /Finance/SalesInvoice/{invoiceId}

Expected Response: 200 OK
{
  "id": {invoiceId},
  "invoiceNumber": "SI-0001",
  "customerId": {customerId},
  "totalAmount": 50000.0,
  "paidAmount": 50000.0,
  "status": 2,
  "items": [...],
  "receipts": [
    {
      "receiptNumber": "SR-0001",
      "paymentAmount": 50000.0,
      "paymentDate": "2026-01-08"
    }
  ]
}
```

---

## Phase 7: Purchase Orders & Vendor Bills

### Purpose
Manage purchase orders and vendor invoice processing.

### Business Flow
1. Create purchase order
2. Approve/authorize PO
3. Receive goods
4. Create vendor bill
5. Process vendor payment

### Test Scenarios

#### Test 7.1: Create Purchase Order
```
POST /Finance/PurchaseOrder
Body: {
  "vendorId": {vendorId},
  "poDate": "2026-01-05",
  "expectedDeliveryDate": "2026-01-15",
  "description": "Medical supplies order",
  "branchId": {branchId},
  "poStatus": 1,
  "items": [
    {
      "itemId": {itemId},
      "description": "Surgical Gloves (Box of 100)",
      "quantity": 10,
      "unitPrice": 5000.0,
      "amount": 50000.0,
      "chartOfAccountId": 51
    },
    {
      "itemId": {itemId2},
      "description": "Sterile Gauze",
      "quantity": 20,
      "unitPrice": 2000.0,
      "amount": 40000.0,
      "chartOfAccountId": 51
    }
  ]
}

Expected Response: 201 Created
{
  "id": {poId},
  "poNumber": "PO-0001",
  "vendorId": {vendorId},
  "totalAmount": 90000.0,
  "poStatus": 1
}
```

#### Test 7.2: Approve Purchase Order
```
POST /Finance/PurchaseOrder/{poId}/Approve
Body: {
  "approvedBy": "{userId}",
  "approvalNotes": "Approved by Finance Manager"
}

Expected Response: 200 OK
{
  "id": {poId},
  "poStatus": 2,
  "approvedDate": "2026-01-05"
}
```

#### Test 7.3: Create Vendor Bill from PO
```
POST /Finance/VendorBill
Body: {
  "poId": {poId},
  "vendorId": {vendorId},
  "invoiceDate": "2026-01-10",
  "dueDate": "2026-02-10",
  "invoiceAmount": 90000.0,
  "invoiceNumber": "VB-0001",
  "description": "Vendor bill for PO-0001",
  "branchId": {branchId},
  "billStatus": 1
}

Expected Response: 201 Created
{
  "id": {billId},
  "poId": {poId},
  "invoiceAmount": 90000.0,
  "billStatus": 1
}
```

**Validation Points:**
- ✓ Bill linked to original PO
- ✓ Amount matches PO total
- ✓ GL entries: Debit Expense/Asset (51xx), Credit AP (2101)

#### Test 7.4: Post Vendor Bill
```
POST /Finance/VendorBill/{billId}/Post
Body: {}

Expected Response: 200 OK
{
  "id": {billId},
  "billStatus": 2,
  "glEntries": {
    "expensePosted": 90000.0,
    "apCreated": 90000.0
  }
}
```

#### Test 7.5: Record Vendor Payment
```
POST /Finance/VendorBill/{billId}/Payment
Body: {
  "paymentAmount": 90000.0,
  "paymentDate": "2026-01-20",
  "referenceNumber": "CHK-005"
}

Expected Response: 200 OK
{
  "id": {billId},
  "paidAmount": 90000.0,
  "billStatus": 3
}
```

---

## Phase 8: Expense Management

### Purpose
Track and approve employee expenses.

### Business Flow
1. Create expense draft
2. Submit for approval
3. Approve/Reject expense
4. Post to GL
5. Process reimbursement

### Test Scenarios

#### Test 8.1: Create Expense Draft
```
POST /Finance/Expense
Body: {
  "customerId": {employeeId},
  "categoryId": {categoryId},
  "amount": 15000.0,
  "expenseDate": "2026-01-08",
  "description": "Client meeting meal expense",
  "isReimbursable": true,
  "chartOfAccountId": 52,
  "branchId": {branchId},
  "attachmentPath": "/attachments/receipt.pdf"
}

Expected Response: 201 Created
{
  "id": {expenseId},
  "amount": 15000.0,
  "status": 1,
  "createdBy": {userId}
}
```

**Validation Points:**
- ✓ Expense created in Draft status (1)
- ✓ Created by user ID captured
- ✓ Attachment path stored

#### Test 8.2: Submit Expense for Approval
```
POST /Finance/Expense/{expenseId}/Submit
Body: {}

Expected Response: 200 OK
{
  "id": {expenseId},
  "status": 2,
  "submittedBy": {userId},
  "submittedDate": "2026-01-08"
}
```

**Validation Points:**
- ✓ Status changes to Submitted (2)
- ✓ Submitted by user recorded

#### Test 8.3: Approve Expense
```
POST /Finance/Expense/{expenseId}/Approve
Body: {}

Expected Response: 200 OK
{
  "id": {expenseId},
  "status": 3,
  "approvedBy": {userId},
  "glEntries": {
    "expensePosted": 15000.0,
    "accrualCreated": 15000.0
  }
}
```

**Validation Points:**
- ✓ Status changes to Approved (3)
- ✓ GL entries created: Debit Expense, Credit Accrual/Payable
- ✓ Approved by user recorded

#### Test 8.4: Reject Expense
Create another expense and reject:
```
POST /Finance/Expense/{expenseId2}/Reject
Body: {
  "rejectionReason": "Receipt missing"
}

Expected Response: 200 OK
{
  "id": {expenseId2},
  "status": 5,
  "rejectionReason": "Receipt missing"
}
```

#### Test 8.5: Get Pending Approvals
```
GET /Finance/Expense/Pending?branchId={branchId}

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

## Phase 9: Fixed Assets

### Purpose
Track acquisition, depreciation, and disposal of fixed assets.

### Business Flow
1. Record asset acquisition
2. Set depreciation method
3. Calculate periodic depreciation
4. Dispose of assets
5. Generate fixed asset report

### Test Scenarios

#### Test 9.1: Create Fixed Asset
```
POST /Finance/FixedAsset
Body: {
  "assetName": "Ultrasound Machine",
  "assetCode": "FA-001",
  "assetCategory": 101,
  "purchaseDate": "2026-01-01",
  "purchasePrice": 500000.0,
  "usefulLifeYears": 5,
  "residualValue": 50000.0,
  "depreciationMethod": 1,
  "branchId": {branchId},
  "chartOfAccountId": 12,
  "depreciationAccountId": 13,
  "description": "Medical diagnostic equipment"
}

Expected Response: 201 Created
{
  "id": {assetId},
  "assetCode": "FA-001",
  "assetName": "Ultrasound Machine",
  "purchasePrice": 500000.0,
  "assetStatus": 1,
  "accumulatedDepreciation": 0
}
```

**Validation Points:**
- ✓ Asset created with unique code
- ✓ Status set to Active (1)
- ✓ Depreciation parameters saved

#### Test 9.2: Calculate and Post Depreciation
```
POST /Finance/FixedAsset/{assetId}/CalculateDepreciation?asOfDate=2026-01-31
Body: {}

Expected Response: 200 OK
{
  "assetId": {assetId},
  "depreciationAmount": 8333.33,
  "accumulatedDepreciation": 8333.33,
  "glEntries": {
    "depreciationExpense": 8333.33,
    "accumulatedDepreciation": 8333.33
  }
}
```

**Validation Points:**
- ✓ Depreciation calculated: (500000 - 50000) / 5 years / 12 months ≈ 8333.33
- ✓ GL entries: Debit Depreciation Expense, Credit Accumulated Depreciation
- ✓ Accumulated depreciation tracked

#### Test 9.3: Get Fixed Asset Details
```
GET /Finance/FixedAsset/{assetId}

Expected Response: 200 OK
{
  "id": {assetId},
  "assetCode": "FA-001",
  "assetName": "Ultrasound Machine",
  "purchasePrice": 500000.0,
  "purchaseDate": "2026-01-01",
  "accumulatedDepreciation": 8333.33,
  "bookValue": 491666.67,
  "assetStatus": 1
}
```

#### Test 9.4: Dispose of Asset
```
POST /Finance/FixedAsset/{assetId}/Dispose
Body: {
  "disposalDate": "2026-02-01",
  "disposalPrice": 450000.0,
  "disposalReason": "Sold to another clinic"
}

Expected Response: 200 OK
{
  "id": {assetId},
  "assetStatus": 4,
  "disposalGain": -33333.33
}
```

**Validation Points:**
- ✓ Status changes to Disposed (4)
- ✓ Gain/loss calculated: Disposal price - Book value
- ✓ GL entries: Debit Cash, Credit Asset, Post gain/loss

---

## Phase 10: Budget Management

### Purpose
Create budgets and track variance against actuals.

### Business Flow
1. Create annual budget
2. Allocate budget by account
3. Monitor actual spending
4. Generate variance reports
5. Analyze budget performance

### Test Scenarios

#### Test 10.1: Create Budget
```
POST /Finance/Budget
Body: {
  "budgetName": "2026 Annual Budget",
  "fiscalYear": 2026,
  "branchId": {branchId},
  "description": "Main clinic annual operating budget",
  "lines": [
    {
      "chartOfAccountId": 51,
      "periodId": 1,
      "budgetedAmount": 100000.0
    },
    {
      "chartOfAccountId": 52,
      "periodId": 1,
      "budgetedAmount": 50000.0
    },
    {
      "chartOfAccountId": 53,
      "periodId": 1,
      "budgetedAmount": 30000.0
    }
  ]
}

Expected Response: 201 Created
{
  "id": {budgetId},
  "budgetName": "2026 Annual Budget",
  "fiscalYear": 2026,
  "budgetStatus": 1
}
```

#### Test 10.2: Approve Budget
```
POST /Finance/Budget/{budgetId}/Approve
Body: {}

Expected Response: 200 OK
{
  "id": {budgetId},
  "budgetStatus": 2,
  "approvedDate": "2026-01-05"
}
```

#### Test 10.3: Activate Budget
```
POST /Finance/Budget/{budgetId}/Activate
Body: {}

Expected Response: 200 OK
{
  "id": {budgetId},
  "budgetStatus": 3
}
```

#### Test 10.4: Get Budget Variance (Basic)
```
GET /Finance/Budget/{budgetId}/Variance

Expected Response: 200 OK
{
  "data": [
    {
      "accountCode": "5101",
      "accountName": "Salaries & Wages",
      "budgetedAmount": 100000.0,
      "actualAmount": 45000.0,
      "variance": 55000.0,
      "variancePercentage": 55.0
    },
    {
      "accountCode": "5102",
      "accountName": "Medical Supplies",
      "budgetedAmount": 50000.0,
      "actualAmount": 48000.0,
      "variance": 2000.0,
      "variancePercentage": 4.0
    }
  ]
}
```

#### Test 10.5: Get Budget Variance Report (Advanced Filtering)
```
GET /Finance/Budget/{budgetId}/VarianceReport?varianceThresholdPercentage=10&sortBy=variance&sortDescending=true&pageNumber=1&pageSize=10

Expected Response: 200 OK
{
  "data": [
    {
      "accountCode": "5101",
      "accountName": "Salaries & Wages",
      "budgetedAmount": 100000.0,
      "actualAmount": 45000.0,
      "variance": 55000.0,
      "variancePercentage": 55.0
    }
  ],
  "totalCount": 1,
  "pageNumber": 1,
  "pageSize": 10
}
```

**Filtering Options to Test:**
- varianceThresholdAmount: Only show variances > certain amount
- varianceThresholdPercentage: Only show variances > certain percentage
- accountType: Filter by account type (1=Asset, 5=Expense, etc.)
- sortBy: AccountCode, AccountName, Variance, VariancePercentage
- Pagination: pageNumber & pageSize

---

## Phase 11: Bank Reconciliation

### Purpose
Reconcile bank statements with GL cash accounts.

### Business Flow
1. Import bank statement
2. Match transactions
3. Identify discrepancies
4. Post reconciliation entries
5. Close reconciliation period

### Test Scenarios

#### Test 11.1: Upload Bank Statement
```
POST /Finance/BankReconciliation/Upload
Body (multipart form data): {
  "statementFile": <CSV file>,
  "bankAccountId": {bankAccountId},
  "statementDate": "2026-01-31",
  "startingBalance": 5000000.0,
  "endingBalance": 4500000.0
}

Expected Response: 201 Created
{
  "id": {importId},
  "bankAccountId": {bankAccountId},
  "statementDate": "2026-01-31",
  "importStatus": 1,
  "linesImported": 25
}
```

#### Test 11.2: Auto-Match Bank Transactions
```
POST /Finance/BankReconciliation/{importId}/AutoMatch
Body: {
  "amountTolerance": 100.0,
  "dateTolerance": 3
}

Expected Response: 200 OK
{
  "id": {importId},
  "totalLines": 25,
  "matchedLines": 23,
  "unmatchedLines": 2,
  "matchPercentage": 92.0
}
```

**Validation Points:**
- ✓ Bank lines matched to GL entries within tolerance
- ✓ Matched lines marked as reconciled
- ✓ Unmatched lines flagged for manual review

#### Test 11.3: Manual Match Bank Transaction
```
POST /Finance/BankReconciliation/{importId}/ManualMatch
Body: {
  "bankStatementLineId": {lineId},
  "journalEntryId": {journalEntryId},
  "matchNotes": "Matched via reference number"
}

Expected Response: 200 OK
{
  "id": {matchId},
  "bankStatementLineId": {lineId},
  "journalEntryId": {journalEntryId},
  "matchStatus": 1
}
```

#### Test 11.4: Get Reconciliation Summary
```
GET /Finance/BankReconciliation/{importId}/Summary

Expected Response: 200 OK
{
  "importId": {importId},
  "bankAccountId": {bankAccountId},
  "statementDate": "2026-01-31",
  "reconciliationStatus": {
    "glBalance": 4500000.0,
    "bankBalance": 4500000.0,
    "difference": 0,
    "isBalanced": true
  },
  "matchedCount": 23,
  "unmatchedCount": 2,
  "adjustmentsNeeded": 0
}
```

#### Test 11.5: Close Reconciliation
```
POST /Finance/BankReconciliation/{importId}/Close
Body: {}

Expected Response: 200 OK
{
  "id": {importId},
  "importStatus": 4,
  "closedDate": "2026-01-31",
  "period": "January 2026"
}
```

---

## Phase 12: Financial Reporting

### Purpose
Generate financial statements and reports for analysis and compliance.

### Business Flow
1. Generate trial balance
2. Create income statement
3. Create balance sheet
4. Compare periods
5. Export reports

### Test Scenarios

#### Test 12.1: Get Trial Balance
```
GET /Finance/Reporting/TrialBalance?asOf=2026-01-31&branchId={branchId}

Expected Response: 200 OK
{
  "asOfDate": "2026-01-31",
  "accounts": [
    {
      "accountCode": "1101",
      "accountName": "Cash and Cash Equivalents",
      "debitBalance": 4500000.0,
      "creditBalance": 0
    },
    {
      "accountCode": "1102",
      "accountName": "Accounts Receivable",
      "debitBalance": 50000.0,
      "creditBalance": 0
    },
    {
      "accountCode": "2101",
      "accountName": "Accounts Payable",
      "debitBalance": 0,
      "creditBalance": 100000.0
    },
    {
      "accountCode": "3101",
      "accountName": "Capital",
      "debitBalance": 0,
      "creditBalance": 5000000.0
    },
    {
      "accountCode": "4101",
      "accountName": "Service Revenue",
      "debitBalance": 0,
      "creditBalance": 500000.0
    },
    {
      "accountCode": "5101",
      "accountName": "Salaries Expense",
      "debitBalance": 45000.0,
      "creditBalance": 0
    }
  ],
  "totals": {
    "totalDebits": 4595000.0,
    "totalCredits": 5600000.0,
    "difference": -1005000.0
  }
}
```

**Validation Points:**
- ✓ All accounts included with balances
- ✓ Total debits and credits calculated
- ✓ Debits = Credits (if GL is correct)

#### Test 12.2: Get Income Statement
```
GET /Finance/Reporting/IncomeStatement?fromDate=2026-01-01&toDate=2026-01-31&branchId={branchId}

Expected Response: 200 OK
{
  "period": "January 2026",
  "revenue": {
    "serviceRevenue": 500000.0,
    "otherRevenue": 0,
    "totalRevenue": 500000.0
  },
  "costOfGoodsSold": {
    "beginningInventory": 0,
    "purchases": 90000.0,
    "endingInventory": 0,
    "totalCOGS": 90000.0
  },
  "grossProfit": 410000.0,
  "operatingExpenses": {
    "salaries": 45000.0,
    "supplies": 48000.0,
    "utilities": 15000.0,
    "totalOperatingExpenses": 108000.0
  },
  "operatingIncome": 302000.0,
  "netIncome": 302000.0
}
```

#### Test 12.3: Compare Income Statements
```
GET /Finance/Reporting/IncomeStatement/Compare?fromDate1=2025-12-01&toDate1=2025-12-31&fromDate2=2026-01-01&toDate2=2026-01-31&branchId={branchId}

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
    "difference": 22000.0,
    "percentageChange": 7.86
  }
}
```

#### Test 12.4: Get Balance Sheet
```
GET /Finance/Reporting/BalanceSheet?asOf=2026-01-31&branchId={branchId}

Expected Response: 200 OK
{
  "asOfDate": "2026-01-31",
  "assets": {
    "currentAssets": 4550000.0,
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
    "totalAssets": 5041666.67,
    "totalLiabilities": 100000.0,
    "totalEquity": 5302000.0,
    "totalLiabilitiesAndEquity": 5402000.0,
    "difference": -360333.33
  }
}
```

**Note:** If difference exists, verify GL entries are balanced.

#### Test 12.5: Export Trial Balance
```
POST /Finance/Reporting/TrialBalance/Export
Body: {
  "format": "Excel",
  "asOfDate": "2026-01-31",
  "branchId": {branchId}
}

Expected Response: 200 OK
{
  "message": "Trial balance export initiated",
  "status": "pending",
  "format": "Excel",
  "downloadLink": "/downloads/trial_balance_2026-01-31.xlsx"
}
```

#### Test 12.6: Export Income Statement
```
POST /Finance/Reporting/IncomeStatement/Export
Body: {
  "format": "Excel",
  "fromDate": "2026-01-01",
  "toDate": "2026-01-31",
  "branchId": {branchId}
}

Expected Response: 200 OK
```

#### Test 12.7: Export Balance Sheet
```
POST /Finance/Reporting/BalanceSheet/Export
Body: {
  "format": "Excel",
  "asOfDate": "2026-01-31",
  "branchId": {branchId}
}

Expected Response: 200 OK
```

---

## Phase 13: Audit & Compliance

### Purpose
Track financial activities and maintain compliance records.

### Business Flow
1. System auto-logs financial transactions
2. Query audit trail by entity
3. Search audit logs by criteria
4. Generate audit reports
5. Export audit records

### Test Scenarios

#### Test 13.1: Get Audit Trail for Specific Entity
```
GET /Finance/AuditTrail/ByEntity?entityType=JournalEntry&entityId={journalEntryId}

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
      "beforeValues": null,
      "afterValues": {
        "entryNumber": "JE-0001",
        "status": 1,
        "totalDebit": 5000000.0
      },
      "ipAddress": "192.168.1.100",
      "userAgent": "PostmanRuntime/7.26.8"
    },
    {
      "id": {auditId2},
      "action": "Update",
      "userName": "admin@crystalclinic.local",
      "timestamp": "2026-01-05T10:35:00Z",
      "beforeValues": {
        "status": 1
      },
      "afterValues": {
        "status": 2
      }
    }
  ]
}
```

#### Test 13.2: Search Audit Trail
```
GET /Finance/AuditTrail/Search?entityType=SalesInvoice&action=Create&fromDate=2026-01-01&toDate=2026-01-31&userName=admin@crystalclinic.local

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
  "totalCount": 5,
  "pageNumber": 1,
  "pageSize": 20
}
```

#### Test 13.3: Get Audit Trail Record Details
```
GET /Finance/AuditTrail/{auditId}

Expected Response: 200 OK
{
  "id": {auditId},
  "entityType": "JournalEntry",
  "entityId": {journalEntryId},
  "action": "Create",
  "userName": "admin@crystalclinic.local",
  "timestamp": "2026-01-05T10:30:00Z",
  "beforeValues": null,
  "afterValues": {
    "entryNumber": "JE-0001",
    "entryDate": "2026-01-05",
    "description": "Opening balance",
    "status": 1
  },
  "correlationId": "corr-12345",
  "ipAddress": "192.168.1.100",
  "userAgent": "PostmanRuntime/7.26.8",
  "relatedEntityType": "Shareholder",
  "relatedEntityId": {shareholderId}
}
```

#### Test 13.4: Export Audit Trail
```
POST /Finance/AuditTrail/Export
Body: {
  "entityType": "SalesInvoice",
  "fromDate": "2026-01-01",
  "toDate": "2026-01-31",
  "format": "Excel"
}

Expected Response: 200 OK
{
  "message": "Audit trail export initiated",
  "status": "pending",
  "recordCount": 25
}
```

---

## Test Data Cleanup

After completing all test phases, clean up test data:

```
DELETE /Finance/ChartOfAccounts/{testAccountId}   [System accounts cannot be deleted]
DELETE /Finance/SalesInvoice/{testInvoiceId}
DELETE /Finance/VendorBill/{testBillId}
DELETE /Finance/Expense/{testExpenseId}
DELETE /Finance/FixedAsset/{testAssetId}
DELETE /Finance/Budget/{testBudgetId}
DELETE /Equity/Shareholders/{testShareholderId}
```

---

## Regression Test Checklist

After each deployment, run these critical paths:

- [ ] Create Journal Entry → Post → Verify GL
- [ ] Create Sales Invoice → Post → Create Receipt → Verify AR/Cash
- [ ] Create Vendor Bill → Post → Payment → Verify AP/Cash
- [ ] Create Expense → Submit → Approve → Verify GL
- [ ] Generate Trial Balance → Verify balances
- [ ] Generate Income Statement → Verify totals
- [ ] Generate Balance Sheet → Verify equation (A=L+E)
- [ ] Bank Reconciliation → Auto-match → Close
- [ ] Budget Variance Report with filters

---

## Known Issues & Workarounds

*Document any issues found during testing*

| Issue | Severity | Workaround | Status |
|-------|----------|-----------|--------|
| [To be filled] | [High/Medium/Low] | [Describe] | [Open/Resolved] |

---

## API Performance Benchmarks

Target response times for endpoints:

| Endpoint Category | Expected Response | Notes |
|------------------|-------------------|-------|
| Create (POST) | < 500ms | Includes GL entry creation |
| Get by ID (GET) | < 100ms | Single record retrieval |
| List with filter (GET) | < 1000ms | Up to 1000 records |
| Report generation | < 2000ms | Complex calculations |
| Bulk operations | < 5000ms | Multiple GL entries |

---

## Security & Compliance Testing

- [ ] Verify all endpoints require authentication
- [ ] Verify user can only see own branch data
- [ ] Verify deleted records are soft-deleted (IsDeleted = true)
- [ ] Verify audit trail captures all modifications
- [ ] Verify sensitive data (amounts) not logged
- [ ] Verify IP address and user agent captured in audit
- [ ] Verify correlation IDs track request chains

---

## End of Financial API Testing Guide

**Document Version:** 1.0  
**Last Updated:** January 5, 2026  
**Next Review:** April 5, 2026

For questions or updates, contact the Financial System Development Team.
