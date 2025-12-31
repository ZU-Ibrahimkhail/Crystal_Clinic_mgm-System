---
description: Detailed HR System Analysis
alwaysApply: true
---

# Detailed HR System Analysis

This document provides an in-depth analysis of the Human Resources (HR) system in the Crystal Clinic Service Management System, covering database structure, API endpoints, code behavior, and system weaknesses.

## Database Structure

### EmployeeProfile Table
**Primary Key**: ID (int)

**Columns**:
- ID (int) - Primary key
- EnglishFirstName (string) - Employee's first name in English
- PashtoFirstName (string) - Employee's first name in Pashto
- EnglishSurName (string) - Employee's surname in English
- PashtoSurName (string) - Employee's surname in Pashto
- EnglishFatherName (string) - Father's name in English
- PashtoFatherName (string) - Father's name in Pashto
- EnglishGrandFatherName (string) - Grandfather's name in English
- PashtoGrandFatherName (string) - Grandfather's name in Pashto
- Gender (string) - Employee gender
- TazkiraTypeId (int) - Type of national ID
- TazkiraNo (string) - National ID number
- JoldNo (string, nullable) - ID book number
- PageNo (string, nullable) - ID page number
- RegNo (string, nullable) - Registration number
- DateOfBirth (DateTime) - Employee's date of birth
- TemporaryAddress (string) - Temporary residence address
- PermenantAddress (string) - Permanent address
- BranchId (int) - Associated branch
- CurrencyTypeId (int, nullable) - Preferred currency
- BloodGroup (string, nullable) - Blood group
- JoinDate (DateTime) - Employment start date
- LeaveDate (DateTime, nullable) - Employment end date
- LeaveRemark (string, nullable) - Reason for leaving
- PersonalEmail (string) - Personal email address
- PhoneNumber (string) - Primary phone number
- EmergencyPhoneNumber (string) - Emergency contact number
- PhotoPath (string) - Path to employee photo
- IsActive (bool) - Active employment status
- HasAccount (bool) - Whether user account exists (default: false)

**Relationships**:
- Branch (many-to-one)
- CurrencyType (many-to-one)

**Audit Fields** (inherited from AuditableEntity):
- CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted

### ContractDetails Table
**Primary Key**: ID (int)

**Columns**:
- ID (int) - Primary key
- EmployeeProfileId (int) - Reference to employee
- ContractTypeId (int) - Type of contract
- PositionTitleId (int) - Job position
- BranchId (int) - Branch assignment
- CurrencyTypeId (int) - Salary currency
- SalaryAmount (double) - Monthly salary
- StartDate (DateTime) - Contract start date
- EndDate (DateTime, nullable) - Contract end date
- IsActive (bool) - Contract active status
- AttachmentPath (string) - Path to contract document

**Relationships**:
- EmployeeProfile (many-to-one)
- ContractType (many-to-one)
- PositionTitle (many-to-one)
- Branch (many-to-one)
- CurrencyType (many-to-one)

**Audit Fields**: CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted

### PayrollTracking Table
**Primary Key**: ID (int)

**Columns**:
- ID (int) - Primary key
- EmployeeId (int) - Reference to employee
- ContractDetailsId (int) - Reference to contract
- PayTypeId (int, nullable) - Payment method
- BranchId (int, nullable) - Branch context
- CurrencyTypeId (int, nullable) - Payment currency
- Date (DateTime) - Payroll period date
- BaseSalary (double) - Base salary amount
- AdvanceDeduction (double) - Deductions from advances
- NetSalary (double) - Final salary after deductions
- PayedBy (Guid, nullable) - User who processed payment
- IsPayed (bool) - Payment status (default: false)

**Relationships**:
- Employee (many-to-one)
- ContractDetails (many-to-one)
- PayType (many-to-one)
- Branch (many-to-one)
- CurrencyType (many-to-one)

**Audit Fields**: CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted

### AdvancePayment Table
**Primary Key**: ID (int)

**Columns**:
- ID (int) - Primary key
- EmployeeId (int) - Reference to employee
- PayTypeId (int) - Payment method for advance
- CurrencyTypeId (int) - Advance currency
- MainAccountId (Guid) - Account used for payment
- AdvanceDate (DateTime) - Date of advance
- AdvanceAmount (double) - Total advance amount
- RemainingBalance (double) - Outstanding balance
- EachInstallmentAmount (double) - Monthly deduction amount
- PayedBy (Guid) - User who approved advance

**Relationships**:
- Employee (many-to-one)
- PayType (many-to-one)
- CurrencyType (many-to-one)
- MainAccount (many-to-one)

**Audit Fields**: CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted

### Lookup Tables

#### ContractType Table
- Inherits from LookAndAuditableEntity
- Standard lookup fields: Name, Description, IsActive

#### PositionTitle Table
- Inherits from LookAndAuditableEntity
- Additional fields: BranchId, JobDescription

#### Partners Table
- Inherits from LookAndAuditableEntity
- Standard lookup fields

## API Endpoints

### EmployeeProfileController
**Base Route**: `/api/EmployeeProfile`

**Endpoints**:
- `POST /` - Create employee profile (form data with photo)
- `PUT /{Id:int}` - Update employee profile (form data with photo)
- `DELETE /{Id:int}` - Delete employee profile
- `GET /GetEmployeeProfileList` - Get paginated employee list
- `GET /GetEmployeeProfileDetails/{Id:int}` - Get employee details
- `GET /GetEmployeeProfileForEdit/{Id:int}` - Get employee for editing
- `GET /GetEmployeeProfileDDL` - Get employee dropdown list
- `GET /GetEmployeeProfileDDLByDepartmentId/{departmentId:int}` - Get employees by department
- `GET /GetActiveEmplyeeDDLByDepartment` - Get active employees by department
- `GET /GetAuthoritiesDDL` - Get authorities dropdown
- `PUT /UpdateProfileImage` - Update employee photo

### ContractDetailsController
**Base Route**: `/api/ContractDetails`

**Endpoints**:
- `POST /` - Create contract (form data with attachment)
- `PUT /{Id:int}` - Update contract (form data with attachment)
- `DELETE /{Id:int}` - Delete contract
- `GET /GetList` - Get contract list with filters
- `GET /GetDetial/{Id:int}` - Get contract details

### PayrollTrackingController
**Base Route**: `/api/PayrollTracking`

**Endpoints**:
- `GET /GeneratePayrollList` - Generate payroll for current period
- `PUT /PaySalary` - Process salary payment
- `POST /GetList` - Get payroll list with search
- `GET /GetDetail/{Id:int}` - Get payroll details

### AdvancePaymentController
**Base Route**: `/api/AdvancePayment`

**Endpoints**:
- `POST /` - Create advance payment request
- `PUT /{Id:int}` - Update advance payment
- `DELETE /{Id:int}` - Delete advance payment
- `POST /GetList` - Get advance payment list

### HRLooks Controllers

#### ContractTypeController
**Base Route**: `/api/ContractType`
- `POST /` - Create contract type
- `PUT /{Id:int}` - Update contract type
- `DELETE /{Id:int}` - Delete contract type
- `GET /GetList` - Get contract types list

#### PositionTitleController
**Base Route**: `/api/PositionTitle`
- `POST /` - Create position title
- `PUT /{Id:int}` - Update position title
- `DELETE /{Id:int}` - Delete position title
- `GET /GetList` - Get position titles list

#### PartnersController
**Base Route**: `/api/Partners`
- `POST /` - Create partner
- `PUT /{Id:int}` - Update partner
- `DELETE /{Id:int}` - Delete partner
- `GET /GetList` - Get partners list

### HRDashboardController
**Base Route**: `/api/HRDashboard`
- `GET /ActiveEmployeePieChart` - Employee distribution chart
- `GET /ActiveUsersSpiderChart` - User activity chart

## Code Behavior

### Employee Profile Management

#### CreateEmployeeProfileCommand
```csharp
public class CreateEmployeeProfileCommand : IRequest<JsonResult>
{
    public string EnglishFirstName { get; set; }
    public string EnglishSurName { get; set; }
    // ... other personal details
    public int BranchId { get; set; }
    public DateTime JoinDate { get; set; }
    public IFormFile? Photo { get; set; }
}
```

**Handler Logic**:
```csharp
public async Task<JsonResult> Handle(CreateEmployeeProfileCommand request, CancellationToken ct)
{
    // Validate branch access
    var branchAccess = await _userService.CheckBranchAccess(loggedInUser.Id, request.BranchId);
    if (!branchAccess) throw new UnauthorizedAccessException();

    // Handle photo upload
    string photoPath = "";
    if (request.Photo != null) {
        photoPath = await _storageService.UploadFile(request.Photo, "employee-photos");
    }

    // Create employee profile
    var employee = new EmployeeProfile {
        EnglishFirstName = request.EnglishFirstName,
        EnglishSurName = request.EnglishSurName,
        // ... map other fields
        PhotoPath = photoPath,
        BranchId = request.BranchId,
        IsActive = true,
        CreatedBy = loggedInUser.Id
    };

    context.EmployeeProfiles.Add(employee);
    await context.SaveChangesAsync(ct);

    return new JsonResult(employee);
}
```

#### Update Logic
- Validates user permissions for branch access
- Handles photo replacement (delete old, upload new)
- Updates audit fields
- Maintains data integrity

#### Query Behaviors
- **GetEmployeeProfileList**: Paginated with search filters
- **DDL Queries**: Optimized for dropdowns with active employees only
- **Department Filtering**: Joins with position titles for department grouping

### Contract Management

#### CreateContractDetailsCommand
```csharp
public class CreateContractDetailsCommand : IRequest<JsonResult>
{
    public int EmployeeProfileId { get; set; }
    public int ContractTypeId { get; set; }
    public int PositionTitleId { get; set; }
    public double SalaryAmount { get; set; }
    public DateTime StartDate { get; set; }
    public IFormFile? Attachment { get; set; }
}
```

**Handler Logic**:
```csharp
public async Task<JsonResult> Handle(CreateContractDetailsCommand request, CancellationToken ct)
{
    // Validate employee exists and is active
    var employee = await context.EmployeeProfiles
        .FirstOrDefaultAsync(e => e.ID == request.EmployeeProfileId && e.IsActive);
    if (employee == null) throw new KeyNotFoundException();

    // Handle attachment upload
    string attachmentPath = "";
    if (request.Attachment != null) {
        attachmentPath = await _storageService.UploadFile(request.Attachment, "contracts");
    }

    // Create contract
    var contract = new ContractDetails {
        EmployeeProfileId = request.EmployeeProfileId,
        ContractTypeId = request.ContractTypeId,
        PositionTitleId = request.PositionTitleId,
        SalaryAmount = request.SalaryAmount,
        StartDate = request.StartDate,
        IsActive = true,
        AttachmentPath = attachmentPath,
        CreatedBy = loggedInUser.Id
    };

    context.ContractDetails.Add(contract);
    await context.SaveChangesAsync(ct);

    return new JsonResult(contract);
}
```

### Payroll Processing

#### GeneratePayrollListCommand
**Handler Logic**:
```csharp
public async Task<List<PayrollTracking>> Handle(GeneratePayrollListCommand request, CancellationToken ct)
{
    var currentMonth = DateTime.Now.Month;
    var currentYear = DateTime.Now.Year;

    // Get active contracts
    var activeContracts = await context.ContractDetails
        .Where(c => c.IsActive && !c.IsDeleted)
        .Include(c => c.EmployeeProfile)
        .Include(c => c.PositionTitle)
        .ToListAsync(ct);

    var payrollList = new List<PayrollTracking>();

    foreach (var contract in activeContracts) {
        // Check if payroll already exists for this month
        var existingPayroll = await context.PayrollTracking
            .AnyAsync(p => p.EmployeeId == contract.EmployeeProfileId && 
                          p.Date.Month == currentMonth && 
                          p.Date.Year == currentYear);

        if (!existingPayroll) {
            // Calculate advance deductions
            var advanceDeductions = await context.AdvancePayments
                .Where(a => a.EmployeeId == contract.EmployeeProfileId && a.RemainingBalance > 0)
                .SumAsync(a => a.EachInstallmentAmount, ct);

            var payroll = new PayrollTracking {
                EmployeeId = contract.EmployeeProfileId,
                ContractDetailsId = contract.ID,
                Date = new DateTime(currentYear, currentMonth, 1),
                BaseSalary = contract.SalaryAmount,
                AdvanceDeduction = advanceDeductions,
                NetSalary = contract.SalaryAmount - advanceDeductions,
                IsPayed = false
            };

            payrollList.Add(payroll);
            context.PayrollTracking.Add(payroll);
        }
    }

    await context.SaveChangesAsync(ct);
    return payrollList;
}
```

#### PaySalaryCommand
```csharp
public class PaySalaryCommand : IRequest<JsonResult>
{
    public int PayrollId { get; set; }
    public Guid PayedBy { get; set; }
}
```

**Handler Logic**:
```csharp
public async Task<JsonResult> Handle(PaySalaryCommand request, CancellationToken ct)
{
    var payroll = await context.PayrollTracking
        .FirstOrDefaultAsync(p => p.ID == request.PayrollId);

    if (payroll == null || payroll.IsPayed) {
        return new JsonResult("Payroll not found or already paid");
    }

    payroll.IsPayed = true;
    payroll.PayedBy = request.PayedBy;
    payroll.ModifiedBy = loggedInUser.Id;
    payroll.ModifiedOn = DateTime.Now;

    // Update advance balances
    var advances = await context.AdvancePayments
        .Where(a => a.EmployeeId == payroll.EmployeeId && a.RemainingBalance > 0)
        .ToListAsync(ct);

    foreach (var advance in advances) {
        advance.RemainingBalance -= advance.EachInstallmentAmount;
        if (advance.RemainingBalance < 0) advance.RemainingBalance = 0;
    }

    await context.SaveChangesAsync(ct);
    return new JsonResult("Salary paid successfully");
}
```

### Advance Payment Management

#### CreateAdvancePaymentCommand
```csharp
public class CreateAdvancePaymentCommand : IRequest<JsonResult>
{
    public int EmployeeId { get; set; }
    public int PayTypeId { get; set; }
    public int CurrencyTypeId { get; set; }
    public Guid MainAccountId { get; set; }
    public DateTime AdvanceDate { get; set; }
    public double AdvanceAmount { get; set; }
    public double EachInstallmentAmount { get; set; }
}
```

**Handler Logic**:
```csharp
public async Task<JsonResult> Handle(CreateAdvancePaymentCommand request, CancellationToken ct)
{
    // Validate employee and account
    var employee = await context.EmployeeProfiles
        .FirstOrDefaultAsync(e => e.ID == request.EmployeeId && e.IsActive);
    var account = await context.MainAccounts
        .FirstOrDefaultAsync(a => a.Id == request.MainAccountId);

    if (employee == null || account == null) {
        throw new ValidationException("Invalid employee or account");
    }

    // Check account balance
    if (account.BalanceAmount < request.AdvanceAmount) {
        throw new ValidationException("Insufficient account balance");
    }

    var advance = new AdvancePayment {
        EmployeeId = request.EmployeeId,
        PayTypeId = request.PayTypeId,
        CurrencyTypeId = request.CurrencyTypeId,
        MainAccountId = request.MainAccountId,
        AdvanceDate = request.AdvanceDate,
        AdvanceAmount = request.AdvanceAmount,
        RemainingBalance = request.AdvanceAmount,
        EachInstallmentAmount = request.EachInstallmentAmount,
        PayedBy = loggedInUser.Id
    };

    // Update account balance
    account.BalanceAmount -= request.AdvanceAmount;

    context.AdvancePayments.Add(advance);
    await context.SaveChangesAsync(ct);

    return new JsonResult(advance);
}
```

## Weaknesses of the HR System

### 1. Limited Employee Lifecycle Management
- **Missing Features**: No performance review system, no training tracking, no career progression planning
- **Impact**: Cannot track employee development or identify high performers
- **Recommendation**: Add performance appraisal and training modules

### 2. Incomplete Leave Management
- **Missing Features**: No leave request/approval workflow, no leave balance tracking, no different leave types (annual, sick, maternity)
- **Impact**: Manual leave tracking leads to errors and disputes
- **Recommendation**: Implement comprehensive leave management system

### 3. Weak Document Management
- **Current State**: Basic file upload for contracts and photos
- **Issues**: No document versioning, no secure storage, no document expiry tracking
- **Impact**: Important documents can be lost or tampered with
- **Recommendation**: Implement document management with versioning and access controls

### 4. Insufficient Reporting and Analytics
- **Current State**: Basic dashboard charts only
- **Missing**: Advanced HR analytics, turnover analysis, headcount planning, cost analysis
- **Impact**: Limited insights for HR decision making
- **Recommendation**: Add comprehensive reporting and business intelligence features

### 5. Security and Access Control Issues
- **Current State**: Basic RBAC with branch-level access
- **Issues**: No field-level security, no data encryption, no audit trail for sensitive operations
- **Impact**: Potential data breaches, unauthorized access to sensitive information
- **Recommendation**: Implement comprehensive security measures including encryption and detailed audit logging

### 6. Payroll System Limitations
- **Current State**: Basic salary calculation with advance deductions
- **Issues**: No tax calculations, no benefits management, no overtime tracking, no variable pay components
- **Impact**: Inaccurate payroll calculations, compliance issues
- **Recommendation**: Integrate with professional payroll system or add comprehensive payroll features

### 7. Onboarding and Offboarding Gaps
- **Missing Features**: No structured onboarding process, no offboarding checklist, no asset return tracking
- **Impact**: Inconsistent employee experience, potential security risks during offboarding
- **Recommendation**: Implement workflow-based onboarding and offboarding processes

### 8. Data Validation and Integrity Issues
- **Current State**: Basic validation in commands
- **Issues**: No comprehensive business rule validation, potential data inconsistencies
- **Impact**: Data quality issues, reporting inaccuracies
- **Recommendation**: Add extensive validation rules and data integrity checks

### 9. Integration Limitations
- **Current State**: Standalone HR system
- **Issues**: No integration with accounting, no API for external systems, no mobile access
- **Impact**: Manual data entry, inefficient processes
- **Recommendation**: Add APIs and integration capabilities

### 10. User Experience Issues
- **Current State**: Basic web interface
- **Issues**: No workflow automation, limited self-service features, no mobile app
- **Impact**: Administrative burden, employee dissatisfaction
- **Recommendation**: Add employee self-service portal and workflow automation