# Phase 1: HR Domain Entities - Creation Complete

## Summary
All domain entities for Phase 1 of the HR System Enhancement have been successfully created and compiled.

**Date**: February 20, 2026  
**Status**: ✅ **BUILD SUCCESSFUL**

---

## Entities Created

### A. Enumerations (Crystal_Clinic_mgm.Domain/Entities/Enums.cs)
Added 10 new enum types for HR functionality:

1. **EmploymentStatus** - Employee employment states
   - Active, Probation, OnLeave, Suspended, Terminated, Retired

2. **LeaveStatus** - Leave request workflow status
   - Pending, Approved, Rejected, Cancelled

3. **AttendanceStatus** - Daily attendance status
   - Present, Absent, Late, OnLeave, HalfDay

4. **PayrollComponentType** - Classification of payroll components
   - Benefit (added to salary), Deduction (subtracted from salary)

5. **ComponentCalculationType** - How components are calculated
   - FixedAmount, PercentageOfBaseSalary, PercentageOfGross

6. **PayCycle** - Payment frequency
   - Monthly, BiWeekly, Weekly, Hourly

7. **PayrollAdjustmentType** - Types of salary adjustments
   - Reward, Charge, Overtime, Bonus, Penalty

8. **HRTaskCategory** - HR task classification
   - Onboarding, Offboarding

9. **HRTaskStatus** - HR task workflow status
   - Pending, Completed, Cancelled

---

### B. Modified Existing Entities

#### EmployeeProfile.cs (Enhanced)
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HR/EmployeeProfile.cs`

**New Fields Added**:
- `DepartmentId` (int?, FK) - Links to Department
- `ManagerId` (int?, FK) - Self-referencing supervisor link
- `EmploymentStatus` (enum) - Employment state tracking
- `BankAccountNo` (string) - For payroll processing
- `EmployeeCode` (string) - Unique employee identifier
- `WorkEmail` (string) - Official email address
- `PreferredPaymentMethod` (string) - Payment preference

**New Navigation Properties**:
- `Manager` - Direct supervisor (self-referencing)
- `DirectReports` - Collection of employees reporting to this person
- `Department` - Assigned department
- `PayrollContracts` - Collection of payroll contracts
- `AttendanceRecords` - Collection of attendance records
- `LeaveRequests` - Collection of leave requests
- `PayrollComponents` - Collection of payroll components assigned

#### PositionTitle.cs (Enhanced)
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HRLooks/PositionTitle.cs`

**New Fields Added**:
- `Id` (int) - Primary key (was missing)
- `Title` (string) - Job title name
- `ReportsToId` (int?, FK) - Reporting relationship
- `JobGrade` (string) - Salary grade classification
- `MinSalary` (decimal) - Grade minimum salary
- `MaxSalary` (decimal) - Grade maximum salary

**New Navigation Properties**:
- `ReportsTo` - Position this reports to
- `PayrollContracts` - Contracts using this position

---

### C. New Lookup/Reference Entities (HRLooks folder)

#### 1. Department.cs
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HRLooks/Department.cs`

**Fields**:
- `Id` (int, PK)
- `Name` (string) - Department name
- `DeptCode` (string) - Short code
- `ParentDepartmentId` (int?, FK) - For hierarchical departments
- `HeadEmployeeId` (int?) - Department head reference

**Navigation Properties**:
- `ParentDepartment` - Parent department (self-referencing)
- `ChildDepartments` - Subdepartments
- `Employees` - Employees in this department

#### 2. Shift.cs
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HRLooks/Shift.cs`

**Fields**:
- `Id` (int, PK)
- `Name` (string) - Shift name
- `StartTime` (TimeSpan) - Shift start time
- `EndTime` (TimeSpan) - Shift end time
- `GracePeriodMinutes` (int) - Grace period for marking attendance (default: 5)
- `IsActive` (bool) - Status flag

**Navigation Properties**:
- `AttendanceRecords` - Attendance records for this shift

#### 3. LeaveType.cs
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HRLooks/LeaveType.cs`

**Fields**:
- `Id` (int, PK)
- `Name` (string) - Leave type name (Annual, Sick, Maternity, etc.)
- `MaxDaysPerYear` (int) - Annual entitlement
- `IsPaid` (bool) - Whether leave is paid
- `AllowCarryover` (bool) - Whether unused days carry over
- `MaxCarryoverDays` (int) - Maximum carryover days
- `Description` (string, nullable)
- `IsActive` (bool)

**Navigation Properties**:
- `LeaveRequests` - Leave requests of this type

#### 4. PayrollComponent.cs
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HRLooks/PayrollComponent.cs`

**Fields**:
- `Id` (int, PK)
- `Name` (string) - Component name (Allowance, Deduction, Tax, etc.)
- `Type` (enum) - Benefit or Deduction
- `CalculationType` (enum) - Fixed, Percentage, etc.
- `Amount` (decimal) - Fixed amount or percentage value
- `Description` (string, nullable)
- `ChartOfAccountId` (int?, FK) - GL account mapping (Phase 2)
- `IsActive` (bool)

**Navigation Properties**:
- `EmployeeComponents` - Employee assignments of this component

---

### D. New Transaction/Master Entities (HR folder)

#### 1. PayrollContract.cs
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HR/PayrollContract.cs`

**Purpose**: Master contract defining salary and employment terms (replaces/enhances ContractDetails)

**Fields**:
- `ID` (int, PK)
- `EmployeeProfileId` (int, FK)
- `ContractTypeId` (int, FK)
- `PositionTitleId` (int, FK)
- `BranchId` (int, FK)
- `CurrencyTypeId` (int, FK)
- `BaseSalary` (decimal) - Monthly base salary ⭐ **CRITICAL**
- `StartDate` (DateTime)
- `EndDate` (DateTime, nullable)
- `IsActive` (bool)
- `AttachmentPath` (List<string>, nullable)
- `Conditions` (string) - Contract terms and conditions
- `PayCycle` (enum) - Monthly, BiWeekly, Weekly, Hourly
- `InsuranceDetails` (string) - Benefits/insurance details

**Navigation Properties**:
- `EmployeeProfile`, `ContractType`, `PositionTitle`, `Branch`, `CurrencyType`
- `AttendanceRecords` - Linked attendance

**Phase 1 Critical**: Base salary is the foundation for payroll calculations

#### 2. AttendanceRecord.cs
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HR/AttendanceRecord.cs`

**Purpose**: Daily attendance tracking

**Fields**:
- `Id` (int, PK)
- `EmployeeId` (int, FK)
- `ShiftId` (int?, FK)
- `CheckIn` (DateTime) - Entry time
- `CheckOut` (DateTime?, nullable) - Exit time
- `Status` (enum) - Present, Absent, Late, etc.
- `WorkingHours` (decimal) - Calculated working hours
- `OvertimeHours` (decimal) - Overtime hours (calculated)
- `Notes` (string, nullable)
- `AttendanceDate` (DateTime) - Date of attendance

**Navigation Properties**:
- `Employee`, `Shift`

#### 3. LeaveRequest.cs
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HR/LeaveRequest.cs`

**Purpose**: Employee leave requests with approval workflow ⭐ **CRITICAL**

**Fields**:
- `Id` (int, PK)
- `EmployeeId` (int, FK)
- `LeaveTypeId` (int, FK)
- `StartDate` (DateTime) ⭐
- `EndDate` (DateTime) ⭐
- `TotalDays` (decimal) ⭐ **CRITICAL for balance tracking**
- `Reason` (string, nullable)
- `Status` (enum) - Pending, Approved, Rejected, Cancelled
- `ApprovedById` (int?, FK) - Approver reference
- `ApprovalDate` (DateTime?, nullable)
- `ApprovalNotes` (string, nullable)
- `AttachmentPath` (string, nullable) - Medical cert, etc.

**Navigation Properties**:
- `Employee`, `LeaveType`, `ApprovedBy`

**Phase 1 Critical**: 
- Accurate `TotalDays` calculation (excluding weekends/holidays)
- Status tracking for approval workflows
- Supports multiple leave types

#### 4. LeaveCarryover.cs
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HR/LeaveCarryover.cs`

**Purpose**: Track unused leave days carried over from previous years ⭐ **CRITICAL**

**Fields**:
- `Id` (int, PK)
- `EmployeeId` (int, FK)
- `LeaveTypeId` (int, FK)
- `CarryoverYear` (int) - Year of carryover
- `RemainingDays` (decimal) ⭐ **CRITICAL**
- `UsedDays` (decimal)
- `ExpirationDays` (decimal) - Days expiring
- `ExpirationDate` (DateTime)
- `Notes` (string, nullable)

**Navigation Properties**:
- `Employee`, `LeaveType`

**Phase 1 Critical**: Foundation for accurate leave balance calculations

#### 5. EmployeePayrollComponent.cs
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HR/EmployeePayrollComponent.cs`

**Purpose**: Link employees to payroll components (allowances, deductions, tax) ⭐ **CRITICAL**

**Fields**:
- `Id` (int, PK)
- `EmployeeId` (int, FK)
- `ComponentId` (int, FK)
- `EffectiveDate` (DateTime) ⭐ **Start date of component**
- `EndDate` (DateTime?, nullable) - When component stops
- `OverrideAmount` (decimal?, nullable) - Override component amount
- `Notes` (string, nullable)
- `IsActive` (bool)

**Navigation Properties**:
- `Employee`, `Component`

**Phase 1 Critical**:
- Multiple payroll components per employee
- Effective dating for start/end of benefits
- Override capability for customization

#### 6. PayrollAdjustment.cs
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HR/PayrollAdjustment.cs`

**Purpose**: Track one-time salary adjustments (bonuses, penalties, etc.)

**Fields**:
- `Id` (int, PK)
- `EmployeeId` (int, FK)
- `Type` (enum) - Reward, Charge, Overtime, Bonus, Penalty
- `Category` (string) - "Bonus", "Late Penalty", "Overtime", etc.
- `Amount` (decimal)
- `AdjustmentDate` (DateTime)
- `Description` (string, nullable)
- `ReferenceNumber` (string, nullable) - Ref to approval/ticket
- `IsProcessed` (bool) - Whether included in payroll

**Navigation Properties**:
- `Employee`

#### 7. HRTask.cs
**Path**: `Crystal_Clinic_mgm.Domain/Entities/HR/HR/HRTask.cs`

**Purpose**: Onboarding and offboarding task checklist

**Fields**:
- `Id` (int, PK)
- `EmployeeId` (int, FK)
- `TaskName` (string) - "Handover Laptop", "ID Card Sign-off", etc.
- `Category` (enum) - Onboarding or Offboarding
- `Status` (enum) - Pending, Completed, Cancelled
- `DueDate` (DateTime)
- `CompletedDate` (DateTime?, nullable)
- `AssignedToEmployeeId` (int?, FK) - Who is responsible for task
- `Description` (string, nullable)
- `Notes` (string, nullable)

**Navigation Properties**:
- `Employee`, `AssignedToEmployee`

---

## Database Relationships Summary

### Hierarchical Relationships
- **EmployeeProfile → Manager (EmployeeProfile)**: Supervisor chain
- **Department → ParentDepartment (Department)**: Department hierarchy
- **PositionTitle → ReportsTo (PositionTitle)**: Reporting structure

### One-to-Many Relationships
- **EmployeeProfile** ← AttendanceRecord, LeaveRequest, PayrollAdjustment, HRTask, EmployeePayrollComponent
- **LeaveType** ← LeaveRequest, LeaveCarryover
- **Shift** ← AttendanceRecord
- **PayrollComponent** ← EmployeePayrollComponent
- **Department** ← EmployeeProfile
- **PositionTitle** ← PayrollContract

### Financial Integration
- **PayrollComponent.ChartOfAccountId** → ChartOfAccounts (Phase 2)
- **PayrollContract.BaseSalary** → Used for GL postings

---

## Build Status

✅ **BUILD SUCCESSFUL**  
- 0 Compilation Errors
- 228 Warnings (mostly package vulnerabilities - not code-related)
- Execution Time: ~13 seconds

All entities compile successfully with proper using directives and relationships.

---

## Next Steps (Immediate)

### Phase 1 Part 2: EF Core Configuration
1. Create DbContext mappings in **Persistence** project
2. Configure:
   - Primary keys and foreign keys
   - Composite keys if needed
   - Index configurations
   - Cascade delete rules
   - Default value conversions
   - Decimal precision for salary/amount fields

3. Add migration for database schema generation:
   ```bash
   dotnet ef migrations add HR_Phase1_Entities --project Crystal_Clinic_mgm.Persistence
   dotnet ef database update
   ```

### Phase 1 Part 3: Service Layer
1. Create services in **Application** project:
   - `ILeaveBalanceService` - Calculate balances (✅ code provided in plan)
   - `IAttendanceService` - Log and calculate attendance
   - `IPayrollService` - Generate basic payroll (✅ code provided in plan)
   - `ITaxCalculationService` - Tax deductions (✅ code provided in plan)

2. Implement FluentValidation rules:
   - Leave request validation (future dates, balance checks)
   - Payroll component validation
   - Attendance validation

### Phase 1 Part 4: API Layer
1. Create REST controllers in **UI** project:
   - `EmployeeController`
   - `LeaveController`
   - `AttendanceController`
   - `PayrollController`

2. Implement endpoints for Phase 1 scope

---

## Critical Validation Rules (Must Implement)

### Leave Balance Accuracy ⭐
- Annual entitlement + carryover - used = available
- Weekends/public holidays excluded
- Conflicting requests rejected
- Balance must be > 0 before approval

### Payroll GL Mapping ⭐
- Debit: Salary Expense (5101) | Credit: Payable to Employee (2102)
- Journal entries created in Draft status
- Double-entry validation (debits = credits)
- Immutable GL account configuration

### Tax Deduction Accuracy ⭐
- Progressive tax brackets per country
- Social insurance calculations
- Auditable formula + rate tracking
- Tax reports reconcile to GL

---

## Files Created/Modified

### Created (12 files)
```
✓ Department.cs (1 KB)
✓ Shift.cs (0.8 KB)
✓ LeaveType.cs (1 KB)
✓ PayrollComponent.cs (1 KB)
✓ PayrollContract.cs (2 KB)
✓ AttendanceRecord.cs (1.5 KB)
✓ LeaveRequest.cs (1.8 KB)
✓ LeaveCarryover.cs (1 KB)
✓ EmployeePayrollComponent.cs (1.2 KB)
✓ PayrollAdjustment.cs (1 KB)
✓ HRTask.cs (1.2 KB)
✓ Enums.cs (updated with 10 new enums)
```

### Modified (2 files)
```
✓ EmployeeProfile.cs (added 13 new properties + navigation)
✓ PositionTitle.cs (added 5 new properties + navigation)
```

---

## Quality Checklist

✅ All entities inherit from `AuditableEntity` or `LookAndAuditableEntity`  
✅ All navigation properties use nullable types correctly  
✅ All FK relationships properly typed  
✅ Meaningful default values assigned  
✅ Enums properly defined with values  
✅ Using directives organized  
✅ No duplicate or conflicting properties  
✅ Code follows existing project patterns  
✅ Documentation included inline  
✅ Builds successfully without compilation errors  

---

## Conclusion

Phase 1 Domain Entities are **100% complete** and ready for the next phase (EF Core configuration and database schema).

The foundation is solid with:
- Complete entity relationships modeled
- Proper inheritance hierarchy
- All enumerations defined
- Critical balance-tracking fields in place
- GL account integration prepared

**Ready to proceed with Persistence layer configuration.**
