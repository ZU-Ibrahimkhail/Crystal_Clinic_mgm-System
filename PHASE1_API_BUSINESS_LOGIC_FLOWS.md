# Phase 1 API Business Logic Flows

## Overview
This document defines the complete business logic flows for Phase 1 implementation:
1. Employee Creation & Onboarding
2. Payroll Processing & GL Integration
3. Fixed Asset Assignment

---

## Part A: Employee Creation & Onboarding Flow

### 1. Step-by-Step Employee Onboarding Process

```
┌─────────────────────────────────────────────────────────────────┐
│ EMPLOYEE ONBOARDING WORKFLOW                                    │
└─────────────────────────────────────────────────────────────────┘

Step 1: Create Employee Profile
  ├─ Personal Information (Name, DOB, Contact, etc.)
  ├─ Identification (Tazkira, ID Number, Jold/Page/Reg)
  ├─ Assignment (Branch, Department, Manager)
  ├─ Status (EmploymentStatus = Active)
  └─ Bank Details (BankAccountNo, PreferredPaymentMethod)
       ↓
Step 2: Assign Position & Contract
  ├─ Select Position Title (with Job Grade)
  ├─ Create Payroll Contract
  │  ├─ ContractType (Full-time, Part-time, Contractor)
  │  ├─ BaseSalary (per PayCycle)
  │  ├─ PayCycle (Monthly, BiWeekly, Weekly, Hourly)
  │  └─ Conditions & InsuranceDetails
  ├─ Set Contract Period (StartDate, EndDate optional)
  └─ Upload Contract Document
       ↓
Step 3: Configure Payroll Components (Deductions & Benefits)
  ├─ Select from PayrollComponent master lookup table
  ├─ Assign Allowances (Housing, Transport, Performance)
  ├─ Assign Deductions (Pension, Health Insurance, Union Dues)
  ├─ Set Effective Dates (when benefits start for this employee)
  ├─ Override amounts if employee-specific (e.g., custom advance deduction)
  └─ Creates EmployeePayrollComponent records (one per component per employee)
       ↓
Step 4: Schedule & Assign Shift
  ├─ Select Shift (Morning, Evening, Night)
  ├─ Set Working Hours (8:00 AM - 5:00 PM)
  ├─ Grace Period (5 mins default)
  └─ Create attendance records
       ↓
Step 5: Create Onboarding Tasks
  ├─ Assign onboarding checklist
  ├─ Laptop Handover
  ├─ ID Card Issuance
  ├─ Security Badge Setup
  ├─ System Access Provisioning
  └─ Orientation Session
       ↓
Step 6: Assign Fixed Assets
  ├─ Laptop/Computer
  ├─ Office Desk & Chair
  ├─ ID Badge
  └─ Tools (if applicable)
       ↓
✓ EMPLOYEE READY FOR PAYROLL
```

### 2. API Endpoints: Employee Creation & Onboarding

#### 2.1 POST /api/HR/Employees
**Create New Employee Profile**

```http
POST /api/HR/Employees
Content-Type: application/json
Authorization: Bearer <token>

Request Body:
{
  "englishFirstName": "Ahmed",
  "englishSurName": "Khan",
  "englishFatherName": "Ali",
  "englishGrandFatherName": "Hassan",
  "pashtoFirstName": "احمد",
  "pashtoSurName": "خان",
  "pashtoFatherName": "علی",
  "pashtoGrandFatherName": "حسن",
  "gender": "Male",
  "dateOfBirth": "1990-05-15",
  "tazkiraTypeId": 1,
  "tazkiraNo": "12345678",
  "joldNo": "001",
  "pageNo": "002",
  "regNo": "REG12345",
  "temporaryAddress": "Kabul, District 4",
  "permenantAddress": "Kandahar, Panjwayi",
  "branchId": 1,
  "departmentId": 3,
  "managerId": 5,
  "employmentStatus": 1,
  "bankAccountNo": "1234567890",
  "employeeCode": "EMP-2026-001",
  "workEmail": "ahmed.khan@clinic.local",
  "phoneNumber": "+93700123456",
  "emergencyPhoneNumber": "+93700654321",
  "joinDate": "2026-02-20",
  "currencyTypeId": 1
}

Response (201 Created):
{
  "id": 42,
  "englishFirstName": "Ahmed",
  "englishSurName": "Khan",
  "employeeCode": "EMP-2026-001",
  "departmentId": 3,
  "managerId": 5,
  "employmentStatus": 1,
  "joinDate": "2026-02-20",
  "isActive": true,
  "createdOn": "2026-02-20T10:30:00Z",
  "createdBy": "user-uuid"
}

Status Codes:
✓ 201 Created - Employee created successfully
✗ 400 Bad Request - Validation failed
✗ 409 Conflict - Employee code already exists
✗ 403 Forbidden - No permission to create in this branch
```

---

#### 2.2 POST /api/HR/Contracts
**Create Payroll Contract for Employee**

```http
POST /api/HR/Contracts
Content-Type: application/json
Authorization: Bearer <token>

Request Body:
{
  "employeeProfileId": 42,
  "contractTypeId": 1,
  "positionTitleId": 8,
  "branchId": 1,
  "currencyTypeId": 1,
  "baseSalary": 15000,
  "payCycle": 1,
  "startDate": "2026-02-20",
  "endDate": null,
  "conditions": "Notice period: 30 days. Probation: 3 months. Annual leave: 20 days",
  "insuranceDetails": "Health insurance included. Pension contribution: 5%"
}

Response (201 Created):
{
  "id": 101,
  "employeeProfileId": 42,
  "positionTitleId": 8,
  "baseSalary": 15000,
  "payCycle": 1,
  "startDate": "2026-02-20",
  "isActive": true,
  "createdOn": "2026-02-20T10:35:00Z"
}

Business Logic:
✓ Verify employee exists and is active
✓ Validate contract type is active
✓ Validate position title exists
✓ Check BaseSalary is within PositionTitle.MinSalary and MaxSalary
✓ Store contract document if provided
✓ Set IsActive = true automatically
✗ Return 400 if BaseSalary is outside grade range
✗ Return 409 if employee already has active contract
```

---

#### 2.3 POST /api/HR/Payroll/Components/Assign
**Assign Payroll Components to Employee**

> **Source of Deductions/Benefits**: All deductions and benefits come from the **PayrollComponent** master lookup table (company-wide configuration). This endpoint assigns those components to a specific employee, creating **EmployeePayrollComponent** records.

```http
POST /api/HR/Payroll/Components/Assign
Content-Type: application/json
Authorization: Bearer <token>

Request Body:
{
  "employeeId": 42,
  "components": [
    {
      "componentId": 1,
      "effectiveDate": "2026-02-20",
      "overrideAmount": null
    },
    {
      "componentId": 2,
      "effectiveDate": "2026-02-20",
      "overrideAmount": null
    },
    {
      "componentId": 3,
      "effectiveDate": "2026-02-20",
      "overrideAmount": null
    },
    {
      "componentId": 4,
      "effectiveDate": "2026-02-20",
      "overrideAmount": null
    },
    {
      "componentId": 5,
      "effectiveDate": "2026-02-20",
      "overrideAmount": 1500
    }
  ]
}

Component Details (from PayrollComponent master table):
- Component 1: Housing Allowance (Type: Benefit, CalculationType: FixedAmount = 3000 AFN)
- Component 2: Health Insurance (Type: Deduction, CalculationType: FixedAmount = 500 AFN)
- Component 3: Social Insurance (Type: Deduction, CalculationType: PercentageOfGross = 8%)
- Component 4: Pension Contribution (Type: Deduction, CalculationType: PercentageOfBaseSalary = 5%)
- Component 5: Advance Deduction (Type: Deduction, CalculationType: FixedAmount = 1500 AFN - overridden per employee)

Response (201 Created):
{
  "employeeId": 42,
  "assignedComponents": 5,
  "totalBenefits": 3000,
  "totalFixedDeductions": 2000,
  "totalPercentageDeductions": "8% + 5%",
  "effectiveDate": "2026-02-20",
  "componentAssignments": [
    {
      "componentId": 1,
      "componentName": "Housing Allowance",
      "type": "Benefit",
      "calculationType": "FixedAmount",
      "amount": 3000,
      "effectiveDate": "2026-02-20"
    },
    {
      "componentId": 2,
      "componentName": "Health Insurance",
      "type": "Deduction",
      "calculationType": "FixedAmount",
      "amount": 500,
      "effectiveDate": "2026-02-20"
    },
    {
      "componentId": 3,
      "componentName": "Social Insurance",
      "type": "Deduction",
      "calculationType": "PercentageOfGross",
      "amount": "8%",
      "effectiveDate": "2026-02-20"
    },
    {
      "componentId": 4,
      "componentName": "Pension Contribution",
      "type": "Deduction",
      "calculationType": "PercentageOfBaseSalary",
      "amount": "5%",
      "effectiveDate": "2026-02-20"
    },
    {
      "componentId": 5,
      "componentName": "Advance Deduction",
      "type": "Deduction",
      "calculationType": "FixedAmount",
      "amount": 1500,
      "overrideAmount": 1500,
      "effectiveDate": "2026-02-20"
    }
  ],
  "createdOn": "2026-02-20T10:40:00Z"
}

Business Logic:
✓ Load PayrollComponent master records by componentId
✓ Verify all components are active (IsActive = true)
✓ For each component, create EmployeePayrollComponent record
✓ Store EffectiveDate (when benefit/deduction starts for this employee)
✓ Store OverrideAmount if provided (employee-specific amount, overrides component default)
   - If OverrideAmount is null: Use component's default amount
   - If OverrideAmount is provided: Use employee-specific amount (e.g., custom advance deduction)
✓ Calculate totals for reference (benefits vs deductions)
✓ Mark previous assignments as inactive if replacing with new ones
✓ Create audit trail entry for assignment
✗ Return 400 if component doesn't exist or is inactive
✗ Return 400 if EffectiveDate is in past
✗ Return 409 if component already assigned to employee (unless replacing)
```

---

**Key Difference: Position vs Employee**

| Aspect | Position | Employee Assignment |
|--------|----------|-------------------|
| **Salary Range** | Min/Max grade salary | ✅ Actual BaseSalary |
| **Deductions/Benefits** | ❌ NOT defined | ✅ Assigned from PayrollComponent |
| **Flexibility** | Rigid per grade | ✅ Flexible per employee |
| **Override Amount** | N/A | ✅ Can customize per employee |

**Example**: Two IT Managers (same position, same 15,000 base salary)
- **Manager A**: Gets standard Housing Allowance (3,000), standard deductions
- **Manager B**: Gets custom Housing Allowance (5,000 - negotiated), plus child allowance (override)

---

---

#### 2.4 POST /api/HR/Attendance/Shift/Assign
**Assign Shift to Employee**

```http
POST /api/HR/Attendance/Shift/Assign
Content-Type: application/json
Authorization: Bearer <token>

Request Body:
{
  "employeeId": 42,
  "shiftId": 2,
  "effectiveDate": "2026-02-20"
}

Shift Details (ID 2):
- Name: "Morning Shift"
- StartTime: "08:00"
- EndTime: "17:00"
- GracePeriodMinutes: 5

Response (201 Created):
{
  "employeeId": 42,
  "shiftId": 2,
  "shiftName": "Morning Shift",
  "workingHours": 9,
  "effectiveDate": "2026-02-20",
  "createdOn": "2026-02-20T10:45:00Z"
}

Business Logic:
✓ Verify employee exists
✓ Verify shift exists and is active
✓ Set effective date (when shift starts)
✓ Calculate working hours from shift times
✗ Return 400 if shift is inactive
```

---

#### 2.5 POST /api/HR/Onboarding/Tasks
**Create Onboarding Task Checklist**

```http
POST /api/HR/Onboarding/Tasks
Content-Type: application/json
Authorization: Bearer <token>

Request Body:
{
  "employeeId": 42,
  "tasks": [
    {
      "taskName": "Laptop Setup",
      "dueDate": "2026-02-22",
      "assignedToEmployeeId": 10
    },
    {
      "taskName": "ID Card Issuance",
      "dueDate": "2026-02-21",
      "assignedToEmployeeId": 15
    },
    {
      "taskName": "Security Training",
      "dueDate": "2026-02-25",
      "assignedToEmployeeId": null
    },
    {
      "taskName": "System Access Setup",
      "dueDate": "2026-02-20",
      "assignedToEmployeeId": 20
    }
  ]
}

Response (201 Created):
{
  "employeeId": 42,
  "taskCount": 4,
  "pendingTasks": 4,
  "completedTasks": 0,
  "createdOn": "2026-02-20T10:50:00Z"
}

Business Logic:
✓ Create task record for each item
✓ Set Category = Onboarding
✓ Set Status = Pending
✓ Assign to designated employee or HR team
✓ Set DueDate for tracking
✗ Return 400 if AssignedToEmployeeId doesn't exist
```

---

## Part B: Payroll Processing & GL Integration Flow

### 1. Complete Payroll Processing Workflow

```
┌──────────────────────────────────────────────────────────────┐
│ PAYROLL PROCESSING WORKFLOW                                  │
└──────────────────────────────────────────────────────────────┘

Month: February 2026

Step 1: Generate Payroll Calculation
  ├─ Load all active employees with contracts
  ├─ Get employee payroll components (benefits + deductions)
  ├─ Calculate gross salary:
  │  └─ BaseSalary + Benefits - Deductions
  ├─ Calculate taxes (progressive brackets)
  └─ Create PayrollTracking records (Status: Draft)
       ↓
Step 2: Calculate Taxes & Deductions ⭐ CRITICAL
  ├─ Income Tax (Afghanistan brackets):
  │  ├─ 0-50,000 AFN @ 0%
  │  ├─ 50,001-100,000 AFN @ 10%
  │  └─ 100,001+ AFN @ 20%
  ├─ Social Insurance (8% of gross)
  ├─ Employee Advance Deductions (from AdvancePayment)
  └─ Store Tax calculation formula + rate for audit
       ↓
Step 3: Generate Draft Journal Entries ⭐ CRITICAL
  ├─ FOR EACH EMPLOYEE:
  │  ├─ Debit: Salary Expense (5101) [GrossSalary amount]
  │  ├─ Credit: Payable to Employee (2102) [NetSalary amount]
  │  ├─ Credit: Tax Payable (2103) [TaxAmount]
  │  └─ Credit: Insurance Payable (2104) [InsuranceAmount]
  │
  └─ Status: Draft (NOT Posted yet)
       ↓
Step 4: Review & Approval (Finance Team)
  ├─ Review payroll summaries
  ├─ Verify journal entries balance (Debits = Credits)
  ├─ Check tax calculations are reasonable
  └─ Approve payroll
       ↓
Step 5: Post Journal Entries
  ├─ Move all JournalEntry.Status from Draft → Posted
  ├─ Transfer entries to GeneralLedger
  ├─ Update account balances
  └─ Create audit trail
       ↓
Step 6: Process Payments
  ├─ Generate payment instructions per employee
  ├─ Debit: Cash/Bank (1101)
  ├─ Credit: Payable to Employee (2102)
  └─ Payment Status = Completed
       ↓
✓ PAYROLL COMPLETE FOR MONTH
```

### 2. Source of Deductions & Benefits (Architecture)

**Where do deductions and benefits come from?**

```
STEP 1: Admin creates PayrollComponent (Master Lookup)
─────────────────────────────────────────────────────
PayrollComponent Table (Company-Wide Configuration)
ID | Name                  | Type      | CalcType              | Amount
---|------------------------|-----------|--------------------------|--------
1  | Housing Allowance      | Benefit   | FixedAmount          | 3,000
2  | Health Insurance       | Deduction | FixedAmount          | 500
3  | Social Insurance       | Deduction | PercentageOfGross    | 8%
4  | Pension Contribution   | Deduction | PercentageOfBaseSalary| 5%
5  | Performance Bonus      | Benefit   | PercentageOfGross    | 10%

          ↓

STEP 2: HR assigns components to Employee (Per-Employee)
─────────────────────────────────────────────────────
EmployeePayrollComponent Table (Employee-Specific)
EmployeeId | ComponentId | EffectiveDate | OverrideAmount | IsActive
-----------|-------------|---------------|----------------|----------
42         | 1           | 2026-02-20    | NULL           | true     // Housing: 3,000
42         | 2           | 2026-02-20    | NULL           | true     // Health: 500
42         | 3           | 2026-02-20    | NULL           | true     // Social: 8%
42         | 4           | 2026-02-20    | NULL           | true     // Pension: 5%
42         | 5           | 2026-02-20    | 2000           | true     // Bonus: 2,000 (override)

          ↓

STEP 3: Payroll calculates using EmployeePayrollComponent
─────────────────────────────────────────────────────
Load employee's assigned components
For each component, calculate:
  - If FixedAmount: Use amount (or OverrideAmount if set)
  - If PercentageOfBaseSalary: Calculate % of BaseSalary
  - If PercentageOfGross: Calculate % of (BaseSalary + Benefits)

RESULT:
  Housing (Fixed)        = 3,000
  Health (Fixed)         = -500
  Social (8% of 20,000)  = -1,600
  Pension (5% of 15,000) = -1,000
  Bonus (Override)       = +2,000

Net Effect on Salary = +3,000 - 500 - 1,600 - 1,000 + 2,000 = +1,900 AFN benefit
```

**Key Points:**
- ✅ Deductions/Benefits are **company-wide** (PayrollComponent master)
- ✅ Each employee gets **custom assignment** (EmployeePayrollComponent)
- ✅ Can override amount per employee (e.g., custom advance deduction)
- ✅ EffectiveDate allows benefits to start on different dates
- ✅ NOT defined at Position level - Position only defines salary range

---

### 2. Payroll Calculation Details

#### 2.1 Example Payroll Calculation for Employee Ahmed Khan

```
PAYROLL CALCULATION: February 2026
Employee: Ahmed Khan (ID: 42)
Contract: Full-time, Monthly Pay
=====================================

BASE COMPONENTS:
  Base Salary               15,000 AFN
  Housing Allowance         +3,000 AFN
  Performance Bonus         +2,000 AFN
  ──────────────────────────────────
  GROSS SALARY             20,000 AFN

DEDUCTIONS (from EmployeePayrollComponent assignments):
  Health Insurance           -500 AFN     (Component: Deduction, FixedAmount)
  Social Insurance (8%)    -1,600 AFN     (Component: Deduction, PercentageOfGross)
  Pension (5% contrib)     -1,000 AFN     (Component: Deduction, PercentageOfBaseSalary)
  Previous Advance         -1,500 AFN     (Component: Deduction, FixedAmount - override)
  ──────────────────────────────────
  TOTAL DEDUCTIONS         -4,600 AFN
  
  SOURCE: All deductions come from PayrollComponent master table and are 
          assigned to Ahmed Khan via EmployeePayrollComponent records.

TAXABLE INCOME: 20,000 AFN
  Calculation (Progressive):
  - First 5,000 AFN @ 0%              = 0 AFN
  - Tax on 5,001 - 12,500 (7,500 AFN) @ 2%   = 150 AFN
  - Tax on 12,501 - 20,000 (7,500 AFN) @ 10% = 750 AFN
  ──────────────────────────────────
  INCOME TAX               900 AFN

NET SALARY TO EMPLOYEE: 20,000 - 4,600 - 900 = 14,500 AFN
```

### 3. API Endpoints: Payroll Processing

#### 3.1 POST /api/HR/Payroll/Generate
**Generate Payroll for Period**

```http
POST /api/HR/Payroll/Generate
Content-Type: application/json
Authorization: Bearer <token>

Request Body:
{
  "month": 2,
  "year": 2026,
  "branchId": 1
}

Response (201 Created):
{
  "payrollPeriod": "2026-02",
  "employeeCount": 45,
  "totalGrossSalary": 675000,
  "totalDeductions": 125000,
  "totalTax": 42500,
  "totalNetSalary": 507500,
  "journalEntriesCreated": 45,
  "journalEntriesStatus": "Draft",
  "createdOn": "2026-02-20T14:00:00Z"
}

Business Logic - IPayrollService.GeneratePayrollAsync():
1. Get all employees with active contracts
2. For each employee:
   a) Load payroll contract (BaseSalary, PayCycle)
   b) Load assigned payroll components
   c) Calculate total benefits & deductions
   d) Call ITaxCalculationService.CalculateIncomeTax()
   e) Create PayrollTracking record
   f) Create draft JournalEntry with:
      - Debit: SalaryExpense (5101) = GrossSalary
      - Credit: PayableToEmployee (2102) = NetSalary
      - Credit: TaxPayable (2103) = TaxAmount
      - Credit: InsurancePayable (2104) = Insurance
   g) Validate journal entries balance
3. Return summary with entry count and status

Status Codes:
✓ 201 Created - Payroll generated successfully
✗ 400 Bad Request - Invalid month/year or no active employees
✗ 409 Conflict - Payroll already exists for this period
```

#### 3.2 GET /api/HR/Payroll/{payrollId}
**Get Payroll Details for Review**

```http
GET /api/HR/Payroll/2026-02?branchId=1
Authorization: Bearer <token>

Response (200 OK):
{
  "payrollId": "2026-02-001",
  "period": "2026-02",
  "branchId": 1,
  "employeeCount": 45,
  "createdDate": "2026-02-20T14:00:00Z",
  "journalEntryId": 5010,
  "journalEntryStatus": "Draft",
  "journalEntriesCount": 45,
  "summary": {
    "totalGrossSalary": 675000.00,
    "totalDeductions": 125000.00,
    "totalTax": 42500.00,
    "totalNetSalary": 507500.00,
    "totalInsurance": 22500.00
  },
  "employees": [
    {
      "employeeId": 42,
      "employeeName": "Ahmed Khan",
      "baseSalary": 15000,
      "benefits": 5000,
      "deductions": 4600,
      "grossSalary": 20000,
      "tax": 900,
      "netSalary": 14500
    },
    // ... more employees
  ],
  "journalEntries": [
    {
      "entryNumber": "PAY-2026-02-001",
      "description": "February 2026 Payroll",
      "status": "Draft",
      "totalDebits": 675000,
      "totalCredits": 675000
    }
  ]
}

Business Logic:
✓ Return payroll with all employee details
✓ Show journal entries in Draft status
✓ Show GL account mappings
✗ Return 404 if payroll doesn't exist
```

---

#### 3.3 POST /api/HR/Payroll/{payrollId}/Approve
**Approve Payroll (Finance Team)**

```http
POST /api/HR/Payroll/2026-02-001/Approve
Content-Type: application/json
Authorization: Bearer <token>
X-Requires-Permission: Finance.Payroll.Approve

Request Body:
{
  "approvedBy": "user-uuid",
  "notes": "Verified all calculations. Tax calculations confirmed."
}

Response (200 OK):
{
  "payrollId": "2026-02-001",
  "status": "Approved",
  "approvedBy": "user-uuid",
  "approvedDate": "2026-02-20T15:30:00Z",
  "journalEntriesStatus": "Unposted",
  "journalEntriesCount": 45
}

Business Logic:
✓ Verify user has Finance.Payroll.Approve permission
✓ Change JournalEntry.Status from Draft → Unposted
✓ Update PayrollTracking.Status = Approved
✓ Lock payroll from further editing
✗ Return 403 if user lacks permission
✗ Return 409 if already approved
```

---

#### 3.4 POST /api/HR/Payroll/{payrollId}/Post
**Post Journal Entries to GL (Finance Controller)**

```http
POST /api/HR/Payroll/2026-02-001/Post
Authorization: Bearer <token>
X-Requires-Permission: Finance.JournalEntry.Post

Request Body:
{}

Response (200 OK):
{
  "payrollId": "2026-02-001",
  "journalEntriesPosted": 45,
  "journalEntryIds": [5010, 5011, 5012, ...],
  "status": "Posted",
  "postedDate": "2026-02-20T16:00:00Z",
  "glImpact": {
    "salaryExpenseDebits": 675000,
    "payableCredits": 507500,
    "taxPayableCredits": 42500,
    "insurancePayableCredits": 22500
  }
}

Business Logic:
✓ Validate all journal entries are in Approved status
✓ For each journal entry:
   a) Change JournalEntry.Status = Posted
   b) Populate GeneralLedger from JournalEntryLines
   c) Update account running balances
   d) Create audit trail
✓ Emit Finance.PayrollPosted event
✗ Return 409 if entries already posted
✗ Return 400 if entries don't balance
```

---

## Part C: Fixed Asset Assignment to Employee

### 1. Fixed Asset Assignment Workflow

```
┌──────────────────────────────────────────────────────────┐
│ FIXED ASSET ASSIGNMENT WORKFLOW                          │
└──────────────────────────────────────────────────────────┘

Step 1: Identify Assets to Assign
  ├─ Laptop (FixedAsset ID: 1001)
  ├─ Office Chair (FixedAsset ID: 1002)
  ├─ Desk (FixedAsset ID: 1003)
  └─ Monitor (FixedAsset ID: 1004)
       ↓
Step 2: Create Asset Assignments
  ├─ Link each asset to employee
  ├─ Record assignment date
  ├─ Set ownership/responsibility
  └─ Generate asset tag label
       ↓
Step 3: Track Asset Lifecycle
  ├─ Current Owner: Ahmed Khan (Employee ID: 42)
  ├─ Assigned Since: 2026-02-20
  ├─ Status: In Use
  └─ Location: Building A, Office 201
       ↓
Step 4: Offboarding Process (Separation)
  ├─ Generate asset return checklist
  ├─ Record ReturnDate
  ├─ Verify condition
  └─ Re-assign or retire asset
       ↓
✓ ASSET LIFECYCLE TRACKED
```

### 2. API Endpoints: Fixed Asset Assignment

#### 2.1 POST /api/HR/Assets/Assign
**Assign Fixed Assets to Employee**

```http
POST /api/HR/Assets/Assign
Content-Type: application/json
Authorization: Bearer <token>

Request Body:
{
  "employeeId": 42,
  "assets": [
    {
      "fixedAssetId": 1001,
      "quantity": 1,
      "assignmentNotes": "Laptop (Dell, Model XPS 15)"
    },
    {
      "fixedAssetId": 1002,
      "quantity": 1,
      "assignmentNotes": "Office Chair"
    },
    {
      "fixedAssetId": 1003,
      "quantity": 1,
      "assignmentNotes": "Desk"
    },
    {
      "fixedAssetId": 1004,
      "quantity": 2,
      "assignmentNotes": "Monitors (Dual)"
    }
  ]
}

Response (201 Created):
{
  "employeeId": 42,
  "employeeName": "Ahmed Khan",
  "assetsAssigned": 4,
  "assignmentDate": "2026-02-20",
  "assetAssignments": [
    {
      "assignmentId": 5001,
      "fixedAssetId": 1001,
      "assetName": "Laptop (Dell XPS 15)",
      "assetCode": "ASSET-1001",
      "quantity": 1,
      "assignedDate": "2026-02-20",
      "returnDate": null,
      "status": "Active"
    },
    // ... more assets
  ],
  "createdOn": "2026-02-20T11:00:00Z"
}

Business Logic:
✓ Verify employee exists and is active
✓ For each asset:
   a) Verify FixedAsset exists
   b) Check asset is not already assigned to another employee
   c) Create EmployeeAssetAssignment record
   d) Set AssignedDate = today
   e) Set ReturnDate = null (active)
   f) Record assignment in audit log
✓ Emit HR.AssetAssignmentChanged event
✗ Return 400 if asset doesn't exist
✗ Return 409 if asset already assigned
✗ Return 403 if no permission to assign assets
```

---

#### 2.2 GET /api/HR/Assets/Employee/{employeeId}
**Get All Assets Assigned to Employee**

```http
GET /api/HR/Assets/Employee/42
Authorization: Bearer <token>

Response (200 OK):
{
  "employeeId": 42,
  "employeeName": "Ahmed Khan",
  "department": "IT",
  "assetCount": 4,
  "activeAssets": [
    {
      "assignmentId": 5001,
      "assetCode": "ASSET-1001",
      "assetName": "Laptop (Dell XPS 15)",
      "category": "Computer Hardware",
      "serialNumber": "SN-123456",
      "purchaseValue": 1200.00,
      "assignedDate": "2026-02-20",
      "returnDate": null,
      "condition": "Good",
      "location": "Building A, Office 201"
    },
    {
      "assignmentId": 5002,
      "assetCode": "ASSET-1002",
      "assetName": "Office Chair",
      "category": "Office Furniture",
      "serialNumber": "SN-789012",
      "purchaseValue": 250.00,
      "assignedDate": "2026-02-20",
      "returnDate": null,
      "condition": "Good",
      "location": "Building A, Office 201"
    },
    // ... more assets
  ],
  "totalAssetValue": 4500.00
}

Business Logic:
✓ Get all active assignments (ReturnDate IS NULL)
✓ Fetch asset details from FixedAsset table
✓ Include assignment metadata
✗ Return 404 if employee not found
```

---

#### 2.3 POST /api/HR/Assets/Return
**Return Asset from Employee (Offboarding)**

```http
POST /api/HR/Assets/Return
Content-Type: application/json
Authorization: Bearer <token>

Request Body:
{
  "assignmentId": 5001,
  "returnDate": "2026-12-31",
  "condition": "Good",
  "notes": "Employee separation - device returned in good condition",
  "reassignToEmployeeId": null
}

Response (200 OK):
{
  "assignmentId": 5001,
  "employeeId": 42,
  "fixedAssetId": 1001,
  "returnedDate": "2026-12-31",
  "condition": "Good",
  "assetStatus": "Available for Reassignment",
  "updatedOn": "2026-12-31T14:00:00Z"
}

Business Logic:
✓ Update EmployeeAssetAssignment.ReturnDate
✓ Record return condition (Good, Fair, Damaged, etc.)
✓ Update FixedAsset status if damaged
✓ Update depreciation calculations if needed
✓ If reassignToEmployeeId provided:
   a) Create new assignment for new employee
   b) Update old assignment.ReturnDate
✗ Return 404 if assignment not found
✗ Return 400 if asset already returned
```

---

#### 2.4 POST /api/HR/Offboarding/Assets
**Generate Asset Return Checklist (Offboarding)**

```http
POST /api/HR/Offboarding/Assets
Content-Type: application/json
Authorization: Bearer <token>

Request Body:
{
  "employeeId": 42,
  "separationDate": "2026-12-31",
  "reason": "Resignation"
}

Response (201 Created):
{
  "employeeId": 42,
  "employeeName": "Ahmed Khan",
  "separationDate": "2026-12-31",
  "assetReturnTasks": [
    {
      "taskId": 9001,
      "assetCode": "ASSET-1001",
      "assetName": "Laptop (Dell XPS 15)",
      "taskName": "Return Laptop",
      "dueDate": "2026-12-31",
      "status": "Pending",
      "createdOn": "2026-12-20T10:00:00Z"
    },
    {
      "taskId": 9002,
      "assetCode": "ASSET-1002",
      "assetName": "Office Chair",
      "taskName": "Return Office Chair",
      "dueDate": "2026-12-31",
      "status": "Pending",
      "createdOn": "2026-12-20T10:00:00Z"
    },
    // ... more assets
  ],
  "totalAssetsToReturn": 4
}

Business Logic:
✓ Get all active assignments for employee
✓ Create HRTask for each asset:
   a) Category = Offboarding
   b) Status = Pending
   c) DueDate = SeparationDate
   d) TaskName = "Return [AssetName]"
✓ Link to employee responsible for collection
✗ Return 404 if employee not found
```

---

## Part D: Onboarding Task Templates Management

### 1. Onboarding Task Template Workflow

```
┌──────────────────────────────────────────────────────────┐
│ ONBOARDING TASK TEMPLATE MANAGEMENT                      │
└──────────────────────────────────────────────────────────┘

Step 1: Create Reusable Template
  ├─ Template Name: "Standard IT Employee Onboarding"
  ├─ Category: Onboarding
  ├─ Status: Active
  └─ Description: Default tasks for new IT department hires
       ↓
Step 2: Define Template Lines (Tasks)
  ├─ Task 1: Laptop Setup (Order: 1, Required: true)
  ├─ Task 2: ID Card Issuance (Order: 2, Required: true)
  ├─ Task 3: Security Training (Order: 3, Required: true)
  ├─ Task 4: System Access Setup (Order: 4, Required: true)
  ├─ Task 5: Office Tour (Order: 5, Required: false)
  └─ Task 6: Meet Team Lead (Order: 6, Required: false)
       ↓
Step 3: Assign Template to Employee
  ├─ Select employee: Ahmed Khan
  ├─ Apply template: "Standard IT Employee Onboarding"
  ├─ Create HRTask records from template lines
  └─ Auto-populate tasks with template details
       ↓
Step 4: Track Task Completion
  ├─ Task Status: Pending → Completed
  ├─ Record completion date
  ├─ Assign responsible person
  └─ Mark template usage
       ↓
✓ ONBOARDING TASKS CREATED FROM TEMPLATE
```

### 2. API Endpoints: Onboarding Task Templates

#### 2.1 POST /api/HR/Onboarding/Templates
**Create Onboarding Task Template**

```http
POST /api/HR/Onboarding/Templates
Content-Type: application/json
Authorization: Bearer <token>

Request Body:
{
  "name": "Standard IT Employee Onboarding",
  "description": "Default onboarding tasks for new IT department employees",
  "category": 1,
  "isActive": true,
  "templateLines": [
    {
      "taskName": "Laptop Setup",
      "taskDescription": "Configure laptop with corporate software and network access",
      "taskOrder": 1,
      "isRequired": true,
      "assignedToRole": "IT Manager"
    },
    {
      "taskName": "ID Card Issuance",
      "taskDescription": "Create and issue company ID card",
      "taskOrder": 2,
      "isRequired": true,
      "assignedToDepartmentId": 2
    },
    {
      "taskName": "Security Training",
      "taskDescription": "Complete mandatory security awareness training",
      "taskOrder": 3,
      "isRequired": true,
      "assignedToRole": "HR Manager"
    },
    {
      "taskName": "System Access Setup",
      "taskDescription": "Create user accounts and configure access permissions",
      "taskOrder": 4,
      "isRequired": true,
      "assignedToRole": "IT Manager"
    },
    {
      "taskName": "Office Tour",
      "taskDescription": "Show office facilities and workspace assignment",
      "taskOrder": 5,
      "isRequired": false,
      "assignedToRole": "Team Lead"
    }
  ]
}

Response (201 Created):
{
  "id": 12,
  "name": "Standard IT Employee Onboarding",
  "category": 1,
  "isActive": true,
  "templateLineCount": 5,
  "createdOn": "2026-02-20T10:00:00Z",
  "createdBy": "user-uuid"
}

Business Logic:
✓ Validate template name is unique
✓ Create OnboardingTaskTemplate record
✓ For each template line:
   a) Validate TaskOrder is sequential
   b) Create OnboardingTaskTemplateLine
   c) Link to department/role if specified
✓ Mark template as Active
✗ Return 400 if duplicate template name
✗ Return 400 if TaskOrder is not sequential
```

---

#### 2.2 GET /api/HR/Onboarding/Templates
**Get All Onboarding Templates**

```http
GET /api/HR/Onboarding/Templates?category=1&isActive=true
Authorization: Bearer <token>

Response (200 OK):
{
  "totalCount": 3,
  "templates": [
    {
      "id": 12,
      "name": "Standard IT Employee Onboarding",
      "description": "Default onboarding tasks for new IT department employees",
      "category": 1,
      "isActive": true,
      "lineCount": 5,
      "createdOn": "2026-02-20T10:00:00Z"
    },
    {
      "id": 13,
      "name": "HR Department Onboarding",
      "description": "Onboarding tasks for HR department staff",
      "category": 1,
      "isActive": true,
      "lineCount": 6,
      "createdOn": "2026-02-19T09:00:00Z"
    },
    {
      "id": 14,
      "name": "Clinical Staff Onboarding",
      "description": "Onboarding tasks for clinic medical staff",
      "category": 1,
      "isActive": true,
      "lineCount": 8,
      "createdOn": "2026-02-18T08:00:00Z"
    }
  ]
}

Business Logic:
✓ Filter by category if provided
✓ Filter by isActive status
✓ Include line count for each template
✓ Return with pagination support
```

---

#### 2.3 GET /api/HR/Onboarding/Templates/{templateId}
**Get Template Details with Lines**

```http
GET /api/HR/Onboarding/Templates/12
Authorization: Bearer <token>

Response (200 OK):
{
  "id": 12,
  "name": "Standard IT Employee Onboarding",
  "description": "Default onboarding tasks for new IT department employees",
  "category": 1,
  "isActive": true,
  "createdOn": "2026-02-20T10:00:00Z",
  "templateLines": [
    {
      "id": 120,
      "taskName": "Laptop Setup",
      "taskDescription": "Configure laptop with corporate software and network access",
      "taskOrder": 1,
      "isRequired": true,
      "assignedToRole": "IT Manager",
      "assignedToDepartmentId": null
    },
    {
      "id": 121,
      "taskName": "ID Card Issuance",
      "taskDescription": "Create and issue company ID card",
      "taskOrder": 2,
      "isRequired": true,
      "assignedToRole": null,
      "assignedToDepartmentId": 2
    },
    // ... more lines
  ]
}

Business Logic:
✓ Load template with all lines
✓ Order lines by TaskOrder
✓ Include assignment details
✗ Return 404 if template not found
```

---

#### 2.4 POST /api/HR/Onboarding/Templates/{templateId}/Apply
**Apply Template to Employee (Auto-Create Tasks)**

```http
POST /api/HR/Onboarding/Templates/12/Apply
Content-Type: application/json
Authorization: Bearer <token>

Request Body:
{
  "employeeId": 42,
  "baseDueDate": "2026-02-27",
  "daysPerTask": 1
}

Response (201 Created):
{
  "employeeId": 42,
  "employeeName": "Ahmed Khan",
  "templateId": 12,
  "templateName": "Standard IT Employee Onboarding",
  "tasksCreated": 5,
  "pendingTasks": 5,
  "completedTasks": 0,
  "tasks": [
    {
      "id": 5001,
      "taskName": "Laptop Setup",
      "dueDate": "2026-02-21",
      "status": "Pending",
      "assignedToRole": "IT Manager",
      "templateLineId": 120
    },
    {
      "id": 5002,
      "taskName": "ID Card Issuance",
      "dueDate": "2026-02-22",
      "status": "Pending",
      "assignedToDepartmentId": 2,
      "templateLineId": 121
    },
    // ... more tasks
  ],
  "createdOn": "2026-02-20T11:00:00Z"
}

Business Logic:
✓ Verify employee exists and is active
✓ Load template with all lines
✓ For each template line:
   a) Create HRTask record
   b) Copy taskName and description
   c) Set taskOrder as dueDate offset
   d) Calculate DueDate = baseDueDate + (taskOrder * daysPerTask)
   e) Set Category = Onboarding
   f) Set Status = Pending
   g) Link TemplateLineId (for tracking template usage)
   h) Assign to specified role/department
✓ Emit HR.OnboardingTasksCreatedFromTemplate event
✗ Return 404 if employee not found
✗ Return 400 if template not found
```

---

## Part E: Tax Configuration Management

### 1. Tax Configuration Workflow

```
┌──────────────────────────────────────────────────────────┐
│ TAX CONFIGURATION & CALCULATION                          │
└──────────────────────────────────────────────────────────┘

Step 1: Create Tax Configuration
  ├─ Name: "Afghanistan Income Tax 2026"
  ├─ TaxType: "Salary"
  ├─ Description: "Progressive tax brackets for employee salaries"
  └─ IsActive: true
       ↓
Step 2: Define Tax Brackets (Progressive)
  ├─ Bracket 1: 0 - 5,000 AFN @ 0% (No tax)
  ├─ Bracket 2: 5,001 - 12,500 AFN @ 2% (On overdraft amount)
  ├─ Bracket 3: 12,501 - 100,000 AFN @ 10% (On overdraft amount)
  └─ Bracket 4: 100,001+ AFN @ 20% (On overdraft amount)
       ↓
Step 3: Configure Calculation Method
  ├─ Calculation Type: "StepByStep" (progressive)
  ├─ Apply percentage on amount ABOVE bracket minimum
  ├─ Example: Salary 20,000 AFN
  │  └─ Tax = (12,500-5,000) * 2% + (20,000-12,500) * 10%
  │  └─ Tax = 7,500 * 0.02 + 7,500 * 0.10 = 150 + 750 = 900 AFN
  └─ Store configuration in database (immutable from day 1)
       ↓
Step 4: Use in Payroll Processing
  ├─ Load tax configuration by ID
  ├─ Calculate tax for each employee salary
  ├─ Store calculation formula for audit trail
  └─ Apply withholding from net salary
       ↓
✓ TAX CALCULATION DATABASE-DRIVEN & COMPLIANT
```

### 2. API Endpoints: Tax Configuration

#### 2.1 POST /api/HR/Tax/Configurations
**Create Tax Configuration with Brackets**

```http
POST /api/HR/Tax/Configurations
Content-Type: application/json
Authorization: Bearer <token>
X-Requires-Permission: Finance.Tax.Configure

Request Body:
{
  "name": "Afghanistan Income Tax 2026",
  "taxType": "Salary",
  "description": "Progressive income tax brackets for Afghanistan",
  "isActive": true,
  "brackets": [
    {
      "minAmount": 0,
      "maxAmount": 5000,
      "percentage": 0,
      "flatAmount": 0,
      "calculationType": 1,
      "bracketOrder": 1
    },
    {
      "minAmount": 5001,
      "maxAmount": 12500,
      "percentage": 2,
      "flatAmount": 0,
      "calculationType": 1,
      "bracketOrder": 2
    },
    {
      "minAmount": 12501,
      "maxAmount": 100000,
      "percentage": 10,
      "flatAmount": 0,
      "calculationType": 1,
      "bracketOrder": 3
    },
    {
      "minAmount": 100001,
      "maxAmount": 999999999,
      "percentage": 20,
      "flatAmount": 0,
      "calculationType": 1,
      "bracketOrder": 4
    }
  ]
}

Response (201 Created):
{
  "id": 1,
  "name": "Afghanistan Income Tax 2026",
  "taxType": "Salary",
  "isActive": true,
  "bracketCount": 4,
  "createdOn": "2026-02-20T10:00:00Z",
  "createdBy": "user-uuid",
  "brackets": [
    {
      "id": 10,
      "minAmount": 0,
      "maxAmount": 5000,
      "percentage": 0,
      "flatAmount": 0,
      "calculationType": 1,
      "bracketOrder": 1
    },
    // ... more brackets
  ]
}

Business Logic:
✓ Verify user has Finance.Tax.Configure permission
✓ Create TaxConfiguration record
✓ For each bracket:
   a) Validate minAmount < maxAmount
   b) Validate bracketOrder is sequential
   c) Ensure percentage OR flatAmount is set
   d) Create TaxBracket record
   e) Set IsActive = true
✓ Mark configuration immutable (no updates after creation, only new versions)
✗ Return 400 if brackets don't cover full range
✗ Return 400 if bracketOrder is not sequential
✗ Return 403 if insufficient permissions
```

---

#### 2.2 GET /api/HR/Tax/Configurations
**Get All Tax Configurations**

```http
GET /api/HR/Tax/Configurations?taxType=Salary&isActive=true
Authorization: Bearer <token>

Response (200 OK):
{
  "totalCount": 2,
  "configurations": [
    {
      "id": 1,
      "name": "Afghanistan Income Tax 2026",
      "taxType": "Salary",
      "description": "Progressive income tax brackets for Afghanistan",
      "isActive": true,
      "bracketCount": 4,
      "createdOn": "2026-02-20T10:00:00Z"
    },
    {
      "id": 2,
      "name": "Contractor Tax Rate 2026",
      "taxType": "Contractor",
      "description": "Flat rate for contractor income",
      "isActive": true,
      "bracketCount": 1,
      "createdOn": "2026-02-15T09:00:00Z"
    }
  ]
}

Business Logic:
✓ Filter by taxType if provided
✓ Filter by isActive status
✓ Include bracket count for each configuration
✓ Return with pagination support
```

---

#### 2.3 GET /api/HR/Tax/Configurations/{configId}
**Get Tax Configuration with Brackets**

```http
GET /api/HR/Tax/Configurations/1
Authorization: Bearer <token>

Response (200 OK):
{
  "id": 1,
  "name": "Afghanistan Income Tax 2026",
  "taxType": "Salary",
  "description": "Progressive income tax brackets for Afghanistan",
  "isActive": true,
  "createdOn": "2026-02-20T10:00:00Z",
  "brackets": [
    {
      "id": 10,
      "minAmount": 0,
      "maxAmount": 5000,
      "percentage": 0,
      "flatAmount": 0,
      "calculationType": 1,
      "bracketOrder": 1
    },
    {
      "id": 11,
      "minAmount": 5001,
      "maxAmount": 12500,
      "percentage": 2,
      "flatAmount": 0,
      "calculationType": 1,
      "bracketOrder": 2
    },
    {
      "id": 12,
      "minAmount": 12501,
      "maxAmount": 100000,
      "percentage": 10,
      "flatAmount": 0,
      "calculationType": 1,
      "bracketOrder": 3
    },
    {
      "id": 13,
      "minAmount": 100001,
      "maxAmount": 999999999,
      "percentage": 20,
      "flatAmount": 0,
      "calculationType": 1,
      "bracketOrder": 4
    }
  ]
}

Business Logic:
✓ Load configuration with all brackets
✓ Order brackets by BracketOrder
✓ Include bracket details for calculation reference
✗ Return 404 if configuration not found
```

---

#### 2.4 POST /api/HR/Tax/Calculate
**Calculate Tax for Salary (Preview)**

```http
POST /api/HR/Tax/Calculate
Content-Type: application/json
Authorization: Bearer <token>

Request Body:
{
  "taxConfigurationId": 1,
  "salary": 20000
}

Response (200 OK):
{
  "salary": 20000,
  "totalTax": 900,
  "netSalary": 19100,
  "taxPercentageRate": 4.5,
  "calculationDetails": {
    "brackets": [
      {
        "bracketOrder": 1,
        "minAmount": 0,
        "maxAmount": 5000,
        "appliedAmount": 0,
        "percentage": 0,
        "calculatedTax": 0,
        "formula": "0 - 5000 AFN @ 0% = 0 AFN"
      },
      {
        "bracketOrder": 2,
        "minAmount": 5001,
        "maxAmount": 12500,
        "appliedAmount": 7500,
        "percentage": 2,
        "calculatedTax": 150,
        "formula": "(12500 - 5001) = 7499 * 2% = 150 AFN"
      },
      {
        "bracketOrder": 3,
        "minAmount": 12501,
        "maxAmount": 100000,
        "appliedAmount": 7500,
        "percentage": 10,
        "calculatedTax": 750,
        "formula": "(20000 - 12501) = 7499 * 10% = 750 AFN"
      },
      {
        "bracketOrder": 4,
        "minAmount": 100001,
        "maxAmount": 999999999,
        "appliedAmount": 0,
        "percentage": 20,
        "calculatedTax": 0,
        "formula": "Not applicable"
      }
    ],
    "totalFormula": "150 + 750 = 900 AFN"
  }
}

Business Logic - ITaxCalculationService.CalculateTaxWithDetailsAsync():
✓ Load TaxConfiguration by ID
✓ For each active bracket (ordered by BracketOrder):
   a) If CalculationType = Single:
      └─ Check if salary falls in bracket range
      └─ If yes: Calculate = (salary * percentage) OR flatAmount
      └─ Return immediately (don't process more brackets)
   b) If CalculationType = StepByStep:
      └─ Check if salary exceeds MinAmount
      └─ If yes:
         └─ AppliedAmount = MIN(salary, MaxAmount) - MinAmount
         └─ If percentage > 0: Tax += AppliedAmount * (percentage / 100)
         └─ If percentage = 0: Tax += FlatAmount
      └─ Continue to next bracket if salary > MaxAmount
✓ Return detailed calculation with formula for audit trail
✗ Return 404 if tax configuration not found
✗ Return 400 if salary < 0
```

---

#### 2.5 POST /api/HR/Payroll/Generate (Updated)
**Updated: Generate Payroll Using Tax Configuration**

```
Key Change: ITaxCalculationService Integration

POST /api/HR/Payroll/Generate
...

Business Logic Update:
✓ Load TaxConfiguration by ID or use default
✓ For each employee:
   a) Load payroll contract and components
   b) Calculate GrossSalary = BaseSalary + Benefits - Fixed Deductions
   c) Call ITaxCalculationService.CalculateTaxAsync(configId, GrossSalary)
   d) Deduct tax from salary
   e) Create PayrollTracking with tax calculation formula
   f) Create Journal Entries with GL mappings
   g) Store TaxCalculationDetail for audit trail
```

---

## Summary Tables

### GL Account Mappings (Phase 1)

| Account | Code | Type | Purpose |
|---------|------|------|---------|
| Salaries & Wages Expense | 5101 | Expense | Debit for gross salary |
| Payable to Employees | 2102 | Liability | Credit for net salary owed |
| Income Tax Payable | 2103 | Liability | Credit for withholding tax |
| Insurance Payable | 2104 | Liability | Credit for insurance deductions |
| Cash/Bank | 1101 | Asset | Debit when payments made |

### Critical Validations (Phase 1 Must-Have)

| Validation | Where | Impact |
|-----------|-------|--------|
| BaseSalary within PositionTitle range | Contract creation | Must reject invalid salaries |
| Tax calculation per Afghanistan brackets | Payroll generation | Legal/compliance requirement |
| Journal entry balancing (Debits = Credits) | Payroll posting | Financial integrity |
| Leave balance > 0 before approval | Leave approval | Prevent negative balances |
| Asset uniqueness (one employee per asset) | Asset assignment | Prevent double assignment |
| Employee status check before payroll | Payroll generation | Exclude terminated employees |

---

## Implementation Checklist

### Services to Implement (Highest Priority)

- [ ] **IEmployeeService** - Employee CRUD + onboarding workflow
- [ ] **IPayrollService** - Payroll generation with GL integration ⭐
- [ ] **ITaxCalculationService** - Tax deduction logic ⭐ (IMPLEMENTED)
- [ ] **ILeaveBalanceService** - Accurate balance tracking ⭐
- [ ] **IAssetAssignmentService** - Fixed asset lifecycle
- [ ] **IOnboardingTemplateService** - Template CRUD + apply to employees
- [ ] **ITaxConfigurationService** - Tax config CRUD + tax calculation

### Validations to Implement

- [ ] FluentValidation: Employee creation rules
- [ ] FluentValidation: Contract salary range validation
- [ ] FluentValidation: Leave request date/balance validation
- [ ] Business logic: Tax calculation per brackets
- [ ] Business logic: Journal entry balancing

### Controllers to Create

- [ ] EmployeeController (POST, PUT, GET, DELETE)
- [ ] ContractController (POST, PUT, GET)
- [ ] PayrollController (POST /Generate, /Approve, /Post)
- [ ] LeaveController (POST /Request, GET /Balance)
- [ ] AssetController (POST /Assign, GET, /Return)
- [ ] OnboardingTemplateController (POST, GET, GET/{id}, POST /{id}/Apply)
- [ ] TaxConfigurationController (POST, GET, GET/{id}, POST /Calculate)

