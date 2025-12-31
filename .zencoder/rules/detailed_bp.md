---
description: Detailed Business Processes and Code Behaviors
alwaysApply: true
---

# Detailed Business Processes and Code Behaviors

This document provides in-depth explanations of key business processes in the Crystal Clinic Service Management System, including step-by-step workflows and corresponding code implementations.

## 1. User Authentication Process

### Business Process Steps
1. **User Login Request**: User submits email/password with optional remember me
2. **Credential Validation**: System validates credentials against UMS database
3. **Account Status Check**: Verifies user is active and not deleted
4. **Multi-Factor Authentication**: If enabled, prompts for 2FA code
5. **Token Generation**: Creates JWT access token and refresh token
6. **Audit Logging**: Records login attempt with IP, browser, success/failure
7. **Session Management**: Updates last login date and success count

### Code Behavior
- **Command**: `SignInCommand` with Email, Password, IpAddress, RememberMe
- **Handler**: `SignInCommandHandler` calls `IUserService.Authenticate()`
- **Service Layer**: Validates credentials using ASP.NET Identity, checks user status
- **Token Generation**: Uses JWT with claims (user ID, roles, branch)
- **Audit**: Logs to UserAudit table with browser detection
- **Error Handling**: Returns localized error messages for invalid credentials

## 2. Visit Management Process

### Business Process Steps
1. **Visit Creation**:
   - Patient selection or new patient registration
   - Doctor assignment (optional, with availability check)
   - Date/time scheduling with conflict validation
   - Initial fee setting

2. **Visit Updates**:
   - Status progression: Scheduled → Pending → In Process → Completed
   - Service/medication additions
   - Fee adjustments

3. **Visit Completion**:
   - Requires full payment (remainingAmount = 0)
   - Generates final totals
   - Updates patient history

### Code Behavior
- **CreateVisitCommand**: Validates patient (creates if new), doctor availability, scheduling conflicts
- **Handler Logic**:
  ```csharp
  // Patient creation if needed
  if (!request.PatientId.HasValue) {
      var newPatient = new Patient { ... };
      context.Patient.Add(newPatient);
      await context.SaveChangesAsync();
      patientId = newPatient.patientId;
  }

  // Doctor conflict check
  var conflictingVisit = await context.Visit
      .AnyAsync(v => v.doctorId == request.DoctorId && 
                    v.visitDate == request.VisitDate && 
                    v.status != VisitStatus.COMPLETED);

  // Visit creation with initial amounts
  var visit = new Visit {
      totalAmount = request.FeeAmount,
      paidAmount = 0,
      remainingAmount = request.FeeAmount
  };
  ```
- **Validation**: FluentValidation ensures future dates, valid enums, required fields
- **Branch Assignment**: Automatically assigns to logged-in user's branch

## 3. Payment Processing Process

### Business Process Steps
1. **Payment Initiation**: Select visit and payment amount
2. **Currency Handling**: Support multiple currencies with exchange rates
3. **Payment Recording**: Create VisitPayment record
4. **Visit Update**: Update paidAmount, remainingAmount
5. **Completion Check**: Mark visit complete if fully paid
6. **Financial Tracking**: Update branch financial records

### Code Behavior
- **RecordVisitPaymentCommand**: Validates visit exists, amount > 0, currency valid
- **Handler Logic**:
  ```csharp
  // Load visit with related data
  var visit = await context.Visit
      .Include(v => v.Patient)
      .Include(v => v.Payments)
      .FirstOrDefaultAsync(v => v.visitId == request.VisitId);

  // Currency conversion if needed
  decimal amountInAFN = request.AmountPaid;
  if (request.CurrencyTypeId != 1) { // Assuming 1 is AFN
      var exchangeRate = await context.GetExchangeRate(request.CurrencyTypeId, 1);
      amountInAFN = request.AmountPaid * exchangeRate;
  }

  // Create payment record
  var payment = new VisitPayment {
      VisitId = request.VisitId,
      Amount = request.AmountPaid,
      CurrencyTypeId = request.CurrencyTypeId,
      AmountInAFN = amountInAFN
  };

  // Update visit totals
  visit.paidAmount += amountInAFN;
  visit.remainingAmount = visit.totalAmount - visit.paidAmount;
  ```
- **Business Rules**: Payments can exceed total (overpayment), completion requires remainingAmount <= 0

## 4. Inventory/Stock Management Process

### Business Process Steps
1. **Stock Addition**:
   - Item selection from catalog
   - Quantity, pricing, supplier details
   - Batch/lot tracking with expiry dates
   - Barcode assignment

2. **Stock Updates**:
   - Quantity adjustments
   - Price changes
   - Supplier updates

3. **Stock Consumption**:
   - Deduction during service usage
   - Automatic inventory level updates

4. **Stock Monitoring**:
   - Low stock alerts
   - Expiry tracking
   - Batch traceability

### Code Behavior
- **CreateStockCommand**: Validates item exists, creates stock record
- **Handler Logic**:
  ```csharp
  // Validate item
  var item = context.Items.FirstOrDefault(x => x.ItemId == request.ItemId);
  if (item == null || item.IsDeleted) {
      throw new KeyNotFoundException($"Item with Id {request.ItemId} not found!");
  }

  // Create stock
  var stock = new Stock {
      itemId = request.ItemId,
      quantity = request.Quantity,
      BranchId = item.BranchId, // Inherit from item
      // ... other fields
  };

  // Update item stock levels
  item.CurrentStock += request.Quantity;
  item.UseableStock += request.Quantity;

  await context.SaveChangesAsync();
  ```
- **Update Logic**: Adjusts item stock levels when quantity changes
- **Branch Isolation**: Stock operations scoped to user's branch

## 5. Human Resources Management Process

### Business Process Steps
1. **Employee Onboarding**:
   - Profile creation with personal details
   - Contract assignment
   - Role/permission setup

2. **Payroll Processing**:
   - Salary calculation based on contracts
   - Advance payment tracking
   - Payroll period management

3. **Contract Management**:
   - Contract details and terms
   - Renewal and updates
   - Termination handling

### Code Behavior
- **Employee Creation**: Validates required fields, assigns branch
- **Contract Logic**: Links employees to contract types, positions, salary details
- **Payroll Calculation**: Based on contract terms, tracks payments and advances
- **Audit Trail**: All HR changes logged with user and timestamp

## 6. Asset Management Process

### Business Process Steps
1. **Asset Registration**:
   - Asset details, value, depreciation
   - Category classification
   - Location tracking

2. **Expense Tracking**:
   - Operational expenses logging
   - Category-based classification
   - Approval workflows

3. **Asset Transactions**:
   - Transfers, maintenance, disposal
   - Value adjustments
   - Financial impact tracking

### Code Behavior
- **Asset CRUD**: Standard create/update with validation
- **Expense Recording**: Links to categories, validates amounts
- **Transaction Handling**: Updates asset status and values
- **Reporting**: Aggregates data for dashboards and financial reports

## 7. Patient Management Process

### Business Process Steps
1. **Patient Registration**:
   - New patient creation during visit or standalone
   - Basic info: name, contact, email, age, gender
   - Automatic branch assignment

2. **Patient Updates**:
   - Contact information changes
   - Profile updates
   - Medical history additions

3. **Patient Search & Retrieval**:
   - Search by name, contact, email
   - Paginated results
   - Visit history access

### Code Behavior
- **UpdatePatientCommand**: Updates patient details with audit trail
- **DeletePatientCommand**: Soft delete (IsDeleted = true)
- **GetPatientByIdQuery**: Direct lookup by ID
- **GetAllPatientsQuery**: Paginated search with filters
- **Handler Logic**:
  ```csharp
  // Update with audit
  patient.name = request.Name;
  patient.ModifiedOn = DateTime.UtcNow;
  patient.ModifiedBy = loggedInUser.Id;

  // Search with multiple fields
  data = data.Where(x => 
      x.name.Contains(request.searchby) || 
      x.contactInfo.Contains(request.searchby) || 
      x.email.Contains(request.searchby));
  ```

## 8. Doctor Management Process

### Business Process Steps
1. **Doctor Creation**:
   - Link to existing employee profile
   - Specialty assignment
   - Service offerings selection
   - Availability status

2. **Doctor Updates**:
   - Specialty changes
   - Service updates
   - Availability toggling

3. **Doctor Scheduling**:
   - Visit conflict checking
   - Availability validation
   - Automatic unavailability during visits

### Code Behavior
- **CreateDoctorCommand**: Validates employee exists, creates doctor from employee data
- **UpdateDoctorCommand**: Updates specialty, services, availability
- **Handler Logic**:
  ```csharp
  // Create from employee
  var employee = await context.EmployeeProfiles
      .FirstOrDefaultAsync(e => e.ID == request.EmployeeId);
  var doctor = new Doctor {
      firstName = employee.EnglishFirstName,
      lastName = employee.EnglishSurName,
      specialty = request.Specialty,
      services = string.Join(",", request.services),
      isAvailable = true
  };
  ```

## 9. Item and Category Management Process

### Business Process Steps
1. **Category Creation**:
   - Category name and description
   - Hierarchical organization
   - Branch-specific categories

2. **Item Registration**:
   - Item details: name, description, unit
   - Stock levels: current, usable, reorder
   - Category assignment
   - Branch isolation

3. **Stock Level Management**:
   - Automatic updates on stock operations
   - Reorder level monitoring
   - Low stock alerts

### Code Behavior
- **CreateItemCategoryCommand**: Simple category creation
- **CreateItemCommand**: Validates category exists, sets branch
- **Handler Logic**:
  ```csharp
  // Item creation with stock
  var item = new Item {
      Name = request.Name,
      CurrentStock = request.CurrentStock,
      UseableStock = request.UseableStock ?? request.CurrentStock,
      ReorderLevel = request.ReorderLevel,
      CategoryId = request.CategoryId,
      BranchId = request.BranchId
  };
  ```

## 10. Employee Profile Management Process

### Business Process Steps
1. **Employee Onboarding**:
   - Personal details entry
   - Contact information
   - Branch assignment
   - Role setup

2. **Profile Updates**:
   - Information changes
   - Status updates
   - Audit trail maintenance

3. **Employee Queries**:
   - Search and filtering
   - Profile retrieval
   - Reporting data

### Code Behavior
- **Employee CRUD**: Standard create/update/delete operations
- **Validation**: Required fields, data integrity
- **Audit**: All changes tracked with user and timestamp
- **Branch Security**: Employees scoped to branches

## 11. Main Account Management Process

### Business Process Steps
1. **Account Creation**:
   - Currency type selection
   - Initial balance deposit
   - Owner assignment
   - Branch association

2. **Account Operations**:
   - Deposits and withdrawals
   - Balance tracking
   - Transaction history

3. **Account Transfers**:
   - Inter-account transfers
   - Currency conversion
   - Balance validation

### Code Behavior
- **CreateMainAccountCommand**: Validates currency, branch, owner
- **Deposit/Transfer Commands**: Balance calculations, transaction logging
- **Handler Logic**:
  ```csharp
  // Account creation
  var account = new MainAccount {
      CurrencyTypeId = request.CurrencyTypeId,
      BalanceAmount = request.BalanceAmount,
      OwnerUserId = request.OwnerUserId,
      BranchId = request.BranchId
  };
  ```

## 12. Service Session Management Process

### Business Process Steps
1. **Session Creation**:
   - Link to visit and services
   - Doctor assignment
   - Duration and notes

2. **Session Tracking**:
   - Status updates
   - Time logging
   - Outcome recording

3. **Session Reporting**:
   - Performance metrics
   - Service utilization
   - Doctor productivity

### Code Behavior
- **ServiceSession CRUD**: Create, update, query operations
- **Validation**: Links to valid visits and doctors
- **Reporting**: Aggregated data for dashboards

## 13. Call List Management Process

### Business Process Steps
1. **Call Logging**:
   - Patient contact attempts
   - Call outcomes
   - Follow-up scheduling

2. **Call Tracking**:
   - Status updates
   - Notes and comments
   - Escalation handling

3. **Call Analytics**:
   - Success rates
   - Response times
   - Conversion tracking

### Code Behavior
- **CallList CRUD**: Standard operations with audit
- **Status Tracking**: Enum-based status management
- **Query Operations**: Filtered by date, status, patient

## 14. Supplier and Due Management Process

### Business Process Steps
1. **Supplier Registration**:
   - Contact details
   - Payment terms
   - Product catalog

2. **Due Tracking**:
   - Outstanding payments
   - Due dates
   - Payment history

3. **Supplier Relations**:
   - Performance monitoring
   - Order history
   - Payment compliance

### Code Behavior
- **Supplier CRUD**: Create/update supplier profiles
- **DuePayment Tracking**: Links to suppliers, tracks amounts
- **Handler Logic**: Automatic due calculations, alerts

## 15. News and Notification Management Process

### Business Process Steps
1. **News Creation**:
   - Content authoring
   - Publication scheduling
   - Target audience

2. **Notification Distribution**:
   - User targeting
   - Delivery tracking
   - Read receipts

3. **Content Management**:
   - Updates and archiving
   - Access control
   - Analytics

### Code Behavior
- **News CRUD**: Content management with rich text
- **Notification System**: Real-time via SignalR, database logging
- **User Targeting**: Role-based and branch-based distribution

## 16. Training Video Management Process

### Business Process Steps
1. **Video Upload**:
   - File storage
   - Metadata tagging
   - Access permissions

2. **Video Organization**:
   - Categorization
   - Search and filtering
   - User assignments

3. **Progress Tracking**:
   - View counts
   - Completion rates
   - User engagement

### Code Behavior
- **TrainingVideo CRUD**: File upload with storage service
- **Access Control**: Permission-based viewing
- **Tracking**: View logs and progress metrics

## 17. User and Role Management Process

### Business Process Steps
1. **User Creation**:
   - Account setup
   - Role assignment
   - Branch permissions

2. **Role Management**:
   - Permission assignment
   - Hierarchical roles
   - Access control

3. **Permission System**:
   - Granular permissions
   - Dynamic assignment
   - Audit logging

### Code Behavior
- **CreateUserCommand**: Links to employee, assigns roles
- **Role/Permission CRUD**: Many-to-many relationships
- **Handler Logic**:
  ```csharp
  // User creation
  var user = new ApplicationUser {
      UserName = request.UserName,
      Email = request.Email,
      EmployeeId = request.EmployeeId
  };
  // Role assignment logic
  ```

## 18. Currency and Exchange Rate Management Process

### Business Process Steps
1. **Currency Setup**:
   - Supported currencies
   - Base currency configuration
   - Exchange rate sources

2. **Rate Updates**:
   - Manual rate entry
   - Automated updates
   - Historical tracking

3. **Conversion Logic**:
   - Real-time conversions
   - Transaction applications
   - Reporting in multiple currencies

### Code Behavior
- **CurrencyExchangeRate CRUD**: Rate management with validation
- **Conversion Service**: Centralized exchange calculations
- **Handler Logic**: Rate lookup with fallback logic

## Common Code Patterns

### Validation Pattern
```csharp
public class CommandValidator : AbstractValidator<Command>
{
    public CommandValidator()
    {
        RuleFor(x => x.Field)
            .NotEmpty()
            .WithMessage("Error message");
    }
}
```

### Handler Pattern
```csharp
public class CommandHandler : IRequestHandler<Command, Result>
{
    public async Task<Result> Handle(Command request, CancellationToken ct)
    {
        // Validation
        var errors = validator.Validate(request).Errors;
        if (errors.Any()) return ErrorResponse(errors);

        // Business logic
        // Database operations
        // Return result
    }
}
```

### Repository Pattern
- `IRepository<T>` interface for CRUD operations
- Generic implementations with EF Core
- Unit of Work for transaction management

### Error Handling
- Validation errors return localized messages
- Business rule violations throw exceptions
- Database errors logged and handled gracefully
- User-friendly error responses via `IMessage` service

### Security & Audit
- All operations check user permissions
- Audit logging for sensitive operations
- Soft deletes preserve data integrity
- Branch-level data isolation