---
description: Comprehensive HR System Enhancement Plan
alwaysApply: true
---

# Comprehensive HR System Enhancement Plan

## Executive Summary

This document outlines a comprehensive plan to enhance the Crystal Clinic HR system from basic employee and payroll tracking to a full-featured, standard HR management system.

Legacy AssetMS functionality is retired; HR references the financial module’s fixed-asset register for asset metadata and only manages assignment counts per employee.

## Current State Assessment (Dec 2025)

The current HR system is a **basic administrative database** (readiness: ~28%). It handles basic personal data and simple contract records but lacks the structural depth and automation required for enterprise resource management.

---

## Technical Implementation Details

### 1. Database Schema Changes (Entity Level)

#### Table: `EmployeeProfile` (Modified)
| Field Name | Change | Purpose |
| :--- | :--- | :--- |
| `DepartmentId` | Add (int FK) | Links employee to a specific functional unit. |
| `ManagerId` | Add (int FK) | Self-referencing link for direct reporting hierarchy (**Supervisor**). |
| `EmploymentStatus`| Add (enum) | Tracks active, probation, leave, and termination states (**Type**). |
| `BankAccountNo` | Add (string) | Required for automated payroll transfers. |

#### Table: `PositionTitle` (Modified - HRLooks)
| Field Name | Change | Purpose |
| :--- | :--- | :--- |
| `ReportsToId` | Add (int FK) | Defines the reporting line for the position itself (**Job Titles**). |
| `JobGrade` | Add (string) | Links position to a **Salary Category**. |

#### Table: `PayrollContract` (**Evolution of existing `ContractDetails`**)
*Note: This table replaces `ContractDetails`, but production data currently lives only in `EmployeeProfile`, so treat the rename as a schema swap with fresh seed data rather than a legacy migration.*
| Field Name | Change | Purpose |
| :--- | :--- | :--- |
| `SalaryAmount` | Rename to `BaseSalary` | Fixed monthly salary (**Salary Details**). |
| `Conditions` | Add (string) | **Contract Conditions** (e.g., notice period, bonuses). |
| `PayCycle` | Add (enum) | Monthly, Bi-weekly, Hourly. |
| `InsuranceDetails`| Add (string) | Deductions for health or social insurance. |

#### New Table: `AttendanceRecord` (**Attendance Sheet**)
| Field Name | Type | Purpose |
| :--- | :--- | :--- |
| `EmployeeId` | int (FK) | Link to employee. |
| `CheckIn` / `CheckOut`| DateTime | Actual times. |
| `ShiftId` | int (FK) | Link to assigned **Shift**. |
| `Status` | enum | Present, Absent, Late, **Overtime**. |

#### New Table: `PayrollAdjustment` (**Charges Category / Rewards**)
| Field Name | Type | Purpose |
| :--- | :--- | :--- |
| `Type` | enum | Reward, Charge, **Overtime**. |
| `Category` | string | **Charges Category** (e.g., Penalty) or **Employee Rewards**. |
| `Amount` | decimal | **Charges Price** or Reward Value. |

#### New Table: `EmployeeAssessment`
| Field Name | Type | Purpose |
| :--- | :--- | :--- |
| `EmployeeId` | int (FK) | Link to employee. |
| `Score` | int | Performance rating. |

#### New Table: `PayrollComponent` (**Benefits & Deductions**)
| Field Name | Type | Purpose |
| :--- | :--- | :--- |
| `Name` | string | e.g., "Housing Allowance", "Income Tax", "Pension". |
| `Type` | enum | **Benefit** (Add to salary) or **Deduction** (Subtract). |
| `CalculationType` | enum | Fixed Amount or Percentage of Base Salary. |
| `Amount` | decimal | The value or percentage. |

#### New Table: `EmployeePayrollComponent`
| Field Name | Type | Purpose |
| :--- | :--- | :--- |
| `EmployeeId` | int (FK) | Link to employee. |
| `ComponentId` | int (FK) | Link to the benefit/deduction type. |
| `EffectiveDate` | DateTime | When this starts applying. |

#### Asset Tracking Source of Truth
- No new `FixedAsset` table is created inside HR; instead, `FixedAssetId` references the financial module’s fixed-asset register.
- Consumable assignments (e.g., uniforms) continue to reference Inventory `ItemId` when needed.

#### New Table: `EmployeeAssetAssignment`
| Field Name | Type | Purpose |
| :--- | :--- | :--- |
| `EmployeeId` | int (FK) | Who has the asset. |
| `FixedAssetId` | int (FK) | Link to financial module `FixedAsset`. |
| `ItemId` | int (FK, nullable) | Optional link to Inventory `Item` for consumables. |
| `Quantity` | decimal | How many units (supports "Employee A has 1 chair"). |
| `AssignedDate` | DateTime | When it was given to the employee. |
| `ReturnDate` | DateTime | When it was returned (null if still with employee). |

#### New Table: `SalaryGrade` (**Templates**)
| Field Name | Type | Purpose |
| :--- | :--- | :--- |
| `GradeName` | string | e.g., "Senior Doctor", "Junior Nurse". |
| `MinBaseSalary` | decimal | Lower bound for the grade. |
| `MaxBaseSalary` | decimal | Upper bound for the grade. |

#### New Table: `HRTask` (**Onboarding & Offboarding**)
| Field Name | Type | Purpose |
| :--- | :--- | :--- |
| `EmployeeId` | int (FK) | Employee the task is for. |
| `TaskName` | string | e.g., "Handover Laptop", "Sign ID Card", "Exit Interview". |
| `Category` | enum | **Onboarding** or **Offboarding**. |
| `Status` | enum | Pending, Completed, Cancelled. |
| `DueDate` | DateTime | Deadline for the task. |

---

### 2. Service Layer Specifications

#### `IAttendanceService`
- **`ProcessOvertimeRequestAsync(OvertimeRequest request)`**
  - **Input**: `employeeId`, `hours`, `type` (**Overtime Type**).
  - **Purpose**: Validates and approves **Overtime Requests**.

#### `IPayrollService` (**Pay Salaries**)
- **`GeneratePayrollAsync(int month, int year)`**
  - **Input**: Month, Year.
  - **Purpose**: Calculates salary + rewards - charges (**Payroll**). Automatically generates Financial Journal Entries.

### Integration Contracts & Events
- **`HR.PayrollGenerated`** → published after `GeneratePayrollAsync`, carrying period totals, so the financial module can prepare journal entry drafts.
- **`HR.PayrollPaid`** → emitted after salary disbursement; payload contains payment references for Accounts Payable reconciliation.
- **`HR.LeaveStatusChanged`** → notifies Clinic and Scheduling modules when approval decisions change availability.
- **`HR.AssetAssignmentChanged`** → event with `EmployeeId`, `FixedAssetId`, `Quantity` so finance and inventory dashboards stay in sync.

---

### 3. API Payload & Response Definitions

#### `POST /api/HR/Leave/Request` (Submit Leave)
- **Payload**:
```json
{
  "leaveTypeId": 1,
  "startDate": "2026-01-10T08:00:00Z",
  "endDate": "2026-01-15T17:00:00Z",
  "reason": "Family vacation"
}
```

---

### 4. Checklist Coverage Confirmation

The following items from your list are now explicitly covered:
- ✅ **Employees** & **Supervisor**: Via `EmployeeProfile` hierarchy.
- ✅ **Job Titles**: Via Enhanced `PositionTitle`.
- ✅ **Attendance Sheet**, **Shift**, **Overtime**: Via `AttendanceRecord`.
- ✅ **Salary Details**, **Payroll**, **Pay Salaries**: Via `PayrollContract` and `IPayrollService`.
- ✅ **Charges Category**, **Charges Price**, **Rewards**: Via `PayrollAdjustment`.
- ✅ **Leave**, **Employee Leaves**: Via `LeaveRequest` system.
- ✅ **Assessment**: Via `EmployeeAssessment`.
- ✅ **Benefits & Deductions**: Via `PayrollComponent` and `EmployeePayrollComponent`.
- ✅ **Onboarding/Offboarding**: Via `HRTask`.
- ✅ **Asset Tracking**: Via `EmployeeAssetAssignment` and `FixedAsset`.
- ✅ **Contract Conditions**: Via `PayrollContract`.
- ✅ **Transformation**: `ContractDetails` evolved into `PayrollContract`.

---

## 5. HR System ERD (Enhanced)

```mermaid
erDiagram
    DEPARTMENT ||--o{ EMPLOYEE_PROFILE : "contains"
    DEPARTMENT ||--o{ DEPARTMENT : "sub-department"
    EMPLOYEE_PROFILE ||--o{ EMPLOYEE_PROFILE : "reports to (Supervisor)"
    POSITION_TITLE ||--o{ EMPLOYEE_PROFILE : "held by"
    POSITION_TITLE ||--o{ POSITION_TITLE : "reports to"
    EMPLOYEE_PROFILE ||--o{ PAYROLL_CONTRACT : "has"
    EMPLOYEE_PROFILE ||--o{ ATTENDANCE_RECORD : "logs"
    SHIFT ||--o{ ATTENDANCE_RECORD : "scheduled in"
    EMPLOYEE_PROFILE ||--o{ LEAVE_REQUEST : "submits"
    LEAVE_TYPE ||--o{ LEAVE_REQUEST : "categorizes"
    EMPLOYEE_PROFILE ||--o{ PAYROLL_ADJUSTMENT : "incurs"
    EMPLOYEE_PROFILE ||--o{ EMPLOYEE_ASSESSMENT : "receives"
    EMPLOYEE_PROFILE ||--o{ EMPLOYEE_DOCUMENT : "owns"
    EMPLOYEE_PROFILE ||--o{ EMPLOYEE_PAYROLL_COMPONENT : "has assigned"
    PAYROLL_COMPONENT ||--o{ EMPLOYEE_PAYROLL_COMPONENT : "defines"
    EMPLOYEE_PROFILE ||--o{ HR_TASK : "must complete"
    EMPLOYEE_PROFILE ||--o{ EMPLOYEE_ASSET_ASSIGNMENT : "responsible for"
    FIXED_ASSET ||--o{ EMPLOYEE_ASSET_ASSIGNMENT : "assigned as"
    ITEM ||--o{ EMPLOYEE_ASSET_ASSIGNMENT : "assigned as (tools)"
    
    EMPLOYEE_PROFILE {
        int ID PK
        string EnglishFirstName
        string PashtoFirstName
        string EnglishSurName
        string PashtoSurName
        string EnglishFatherName
        string PashtoFatherName
        string EnglishGrandFatherName
        string PashtoGrandFatherName
        string Gender
        int TazkiraTypeId
        string TazkiraNo
        string JoldNo
        string PageNo
        string RegNo
        DateTime DateOfBirth
        string TemporaryAddress
        string PermenantAddress
        int BranchId FK
        int DepartmentId FK
        int ManagerId FK
        string EmployeeCode
        enum EmploymentStatus
        string WorkEmail
        string BankAccountNo
        string PreferredPaymentMethod
        DateTime JoinDate
        DateTime LeaveDate
        string LeaveRemark
        bool IsActive
    }

    DEPARTMENT {
        int Id PK
        string Name
        string DeptCode
        int ParentDeptId FK
        int HeadId FK
    }

    POSITION_TITLE {
        int Id PK
        string Title
        string JobDescription
        int ReportsToId FK
        string JobGrade
        decimal MinSalary
        decimal MaxSalary
        bool IsActive
    }

    PAYROLL_CONTRACT {
        int ID PK
        int EmployeeProfileId FK
        int ContractTypeId FK
        int PositionTitleId FK
        int BranchId FK
        decimal BaseSalary
        string Conditions
        enum PayCycle
        string InsuranceDetails
        DateTime StartDate
        DateTime EndDate
        bool IsActive
    }

    ATTENDANCE_RECORD {
        int Id PK
        int EmployeeId FK
        DateTime CheckIn
        DateTime CheckOut
        int ShiftId FK
        enum Status
        decimal WorkingHours
        decimal OvertimeHours
    }

    SHIFT {
        int Id PK
        string Name
        time StartTime
        time EndTime
        int GracePeriodMinutes
    }

    LEAVE_REQUEST {
        int Id PK
        int EmployeeId FK
        int LeaveTypeId FK
        DateTime StartDate
        DateTime EndDate
        decimal TotalDays
        string Reason
        enum Status
        int ApprovedById FK
    }

    LEAVE_TYPE {
        int Id PK
        string Name
        int MaxDaysPerYear
        bool IsPaid
    }

    PAYROLL_ADJUSTMENT {
        int Id PK
        int EmployeeId FK
        enum Type
        string Category
        decimal Amount
        DateTime Date
        bool IsProcessed
    }

    EMPLOYEE_ASSESSMENT {
        int Id PK
        int EmployeeId FK
        int ReviewerId FK
        int Score
        string Comments
        DateTime ReviewDate
    }

    EMPLOYEE_DOCUMENT {
        int Id PK
        int EmployeeId FK
        string DocumentType
        string FilePath
        DateTime ExpiryDate
    }

    PAYROLL_COMPONENT {
        int Id PK
        string Name
        enum Type
        enum CalculationType
        decimal Amount
    }

    EMPLOYEE_PAYROLL_COMPONENT {
        int Id PK
        int EmployeeId FK
        int ComponentId FK
        DateTime EffectiveDate
    }

    FIXED_ASSET {
        int Id PK
        string AssetName
        string SerialNo
        string AssetCode
        DateTime PurchaseDate
    }

    EMPLOYEE_ASSET_ASSIGNMENT {
        int Id PK
        int EmployeeId FK
        int FixedAssetId FK
        int ItemId FK
        decimal Quantity
        DateTime AssignedDate
        DateTime ReturnDate
    }

    HR_TASK {
        int Id PK
        int EmployeeId FK
        string TaskName
        enum Category
        enum Status
        DateTime DueDate
    }
```

## UI/UX & Deployment Strategy
- Rebuild or simplify employee, payroll, leave, and asset assignment forms as needed; no retrofits are required because the rollout is greenfield.
- Keep shared widgets (employee pickers, benefit tables) aligned with Clinic and Finance UI guidelines.
- No data migration is needed—seed demo departments, positions, and payroll components automatically.

## Testing Strategy
### Unit Tests
- Validation for employment status transitions, payroll component calculations, and leave accrual rules
- PayrollAdjustment math (rewards, deductions, overtime) and asset assignment quantity enforcement
- Attendance and overtime calculations per shift definition

### Integration & Contract Tests
- `HR.PayrollGenerated`, `HR.PayrollPaid`, `HR.LeaveStatusChanged`, and `HR.AssetAssignmentChanged` contract suites
- Financial journal generation handshake tests for payroll events
- Clinic scheduling sync tests reacting to leave approvals/denials

### UI/UX Validation
- End-to-end UI tests for onboarding → payroll → offboarding scenarios
- Accessibility/localization coverage for HR forms
- Stakeholder usability sessions (HR ops, finance, line managers) to validate redesigned workflows
