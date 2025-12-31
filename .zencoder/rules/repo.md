---
description: Repository Information Overview
alwaysApply: true
---

# Repository Information Overview

## Repository Summary
Crystal Clinic Service Management System - a .NET 8.0 web application for clinic service management with clean architecture (Domain, Application, Persistence, UI layers).

## Repository Structure
The repository contains a Visual Studio solution with 5 .NET projects organized under a "Src" folder, plus configuration files and build artifacts.

### Main Repository Components
- **Crystal_Clinic_mgm.Domain**: Core domain entities and business rules
- **Crystal_Clinic_mgm.Common**: Shared utilities, constants, localization, and common services
- **Crystal_Clinic_mgm.Persistence**: Data access layer with Entity Framework Core and SQL Server
- **Crystal_Clinic_mgm.Application**: Application services, MediatR commands/queries, and business logic
- **Crystal_Clinic_mgm.UI**: ASP.NET Core Web API with Swagger, authentication, and UI controllers

## Projects

### Crystal_Clinic_mgm.Domain
**Configuration File**: Crystal_Clinic_mgm.Domain.csproj

#### Language & Runtime
**Language**: C#  
**Version**: .NET 8.0  
**Build System**: .NET SDK  
**Package Manager**: NuGet

#### Dependencies
**Main Dependencies**:
- Microsoft.SqlServer.Types (160.1000.6)
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (8.0.6)

#### Build & Installation
```bash
dotnet build
```

### Crystal_Clinic_mgm.Common
**Configuration File**: Crystal_Clinic_mgm.Common.csproj

#### Language & Runtime
**Language**: C#  
**Version**: .NET 8.0  
**Build System**: .NET SDK  
**Package Manager**: NuGet

#### Dependencies
**Main Dependencies**:
- AutoMapper (13.0.1)
- FluentValidation (11.9.2)
- Magick.NET-Q16-AnyCPU (13.8.0)
- Microsoft.AspNetCore.Authentication.JwtBearer (8.0.6)
- Microsoft.EntityFrameworkCore.Design (8.0.6)

#### Build & Installation
```bash
dotnet build
```

### Crystal_Clinic_mgm.Persistence
**Configuration File**: Crystal_Clinic_mgm.Persistence.csproj

#### Language & Runtime
**Language**: C#  
**Version**: .NET 8.0  
**Build System**: .NET SDK  
**Package Manager**: NuGet

#### Dependencies
**Main Dependencies**:
- Microsoft.EntityFrameworkCore.SqlServer (8.0.6)
- Microsoft.EntityFrameworkCore.Tools (8.0.6)
- Microsoft.SqlServer.Types (160.1000.6)

#### Build & Installation
```bash
dotnet build
```

### Crystal_Clinic_mgm.Application
**Configuration File**: Crystal_Clinic_Mgm.Application.csproj

#### Language & Runtime
**Language**: C#  
**Version**: .NET 8.0  
**Build System**: .NET SDK  
**Package Manager**: NuGet

#### Dependencies
**Main Dependencies**:
- AutoMapper (13.0.1)
- FluentValidation (11.9.2)
- MediatR (12.3.0)
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (8.0.7)
- MailKit (4.7.1)

#### Build & Installation
```bash
dotnet build
```

### Crystal_Clinic_mgm.UI
**Configuration File**: Crystal_Clinic_mgm.UI.csproj

#### Language & Runtime
**Language**: C#  
**Version**: .NET 8.0  
**Build System**: .NET SDK  
**Package Manager**: NuGet

#### Dependencies
**Main Dependencies**:
- Microsoft.AspNetCore.Authentication.JwtBearer (8.0.7)
- Microsoft.EntityFrameworkCore.SqlServer (8.0.7)
- Swashbuckle.AspNetCore (6.6.2)
- MediatR (12.3.0)
- StackExchange.Redis (2.8.0)

#### Build & Installation
```bash
dotnet build
dotnet run
```

#### Main Files & Resources
**Entry Point**: Program.cs  
**Configuration**: appsettings.json  
**Features**: Swagger API documentation, JWT authentication, localization (en, ps-AF, fa-IR), SignalR, background jobs

## Database Structure

The application uses two separate SQL Server databases managed by Entity Framework Core:

### ERP_DbContext
Main operational database containing business entities:

- **Lookups**: Branch, BranchDetails, CurrencyType, AssetType, PayType, ExpenseType, etc.
- **Human Resources**: EmployeeProfile, ContractDetails, PayrollTracking, AdvancePayment
- **General**: News, TrainingVideo
- **Asset Management**: MainAccount, ExpenseTracking, WithdrawalTracking
- **Branch Stock**: Stock, Supplier, Service, ItemCategory
- **Crystal Clinic**: Doctor, Patient, Visit, VisitMedication, VisitServices, VisitPayment, ServiceSessions, CallList
- **Financial**: CurrencyExchangeRate, DuePayment

### UMS_DbContext
User management database extending ASP.NET Core Identity:

- **Identity Tables**: AspNetUsers, AspNetRoles, AspNetUserRoles
- **Custom UMS**: Permission, RolePermission, ApplicationRole, UserAudit, TrackingTable, Language, RefreshToken, Notification, EmailHistory

Entities include audit fields (CreatedOn, ModifiedOn, CreatedBy, ModifiedBy, IsDeleted) and navigation properties for relationships.

## API Endpoints

Endpoints are organized by modules with RESTful conventions. Main groups:

### User Management System (UMS)
- **AuthController**: Signin, signup, password reset, 2FA, refresh token
- **UserController**: CRUD operations for users, profile management
- **RoleController**: Role management and permissions
- **PermissionController**: Permission CRUD
- **ApplicationController**: Application settings
- **NotificationController**: User notifications

### Crystal Clinic Services
- **VisitController**: Visit scheduling, updates, status changes
- **DoctorController**: Doctor information management
- **PatientsController**: Patient records
- **ServiceSesssionsController**: Service session tracking
- **VisitPaymentController**: Payment processing for visits
- **CallListsController**: Call list management

### Human Resources (HR)
- **EmployeeProfileController**: Employee data management
- **ContractDetailsController**: Employment contracts
- **PayrollTrackingController**: Salary tracking
- **AdvancePaymentController**: Advance payments
- **HRDashboardController**: HR analytics

### Asset Management System (AssetMS)
- **MainAssetController**: Asset CRUD
- **ExpenseTrackingController**: Expense logging
- **TransactionsController**: Asset transactions
- **WithdrawalTrackingController**: Asset withdrawals
- **DashboardAndReportController**: Asset reports

### Branch Stock
- **StockController**: Inventory management
- **ServicesController**: Service offerings
- **SuppliersController**: Supplier management
- **SupplierDuesController**: Supplier payment tracking
- **InventoryController**: Stock levels

### Lookups and General
- **BranchController**: Branch management
- **CurrencyExchangeRateController**: Exchange rates
- **NewsController**: News and announcements
- **TrainingVideoController**: Training content

All endpoints use JWT authentication, support localization, and return JSON responses.

## Business Logic Implementation

Business logic is implemented in the Application layer using MediatR for CQRS pattern:

### Command/Query Handlers
- **Commands**: Create, Update, Delete operations (e.g., CreateVisitCommand, UpdateUserCommand)
- **Queries**: Read operations (e.g., GetVisitsQuery, GetUserProfileQuery)
- Each command/query has a validator (FluentValidation) and handler class

### Key Behaviors
- **Visit Management**: Handles patient visits with status tracking (Scheduled → Pending → In Process → Completed), automatic fee calculation, medication/service assignment
- **User Authentication**: JWT-based auth with 2FA, role-based permissions, audit logging
- **Financial Operations**: Currency exchange rate handling, payment tracking, due calculations
- **Inventory Management**: Stock movement tracking, supplier dues, item categorization
- **HR Operations**: Employee contracts, payroll calculations, advance payments

### Validation and Rules
- FluentValidation for input validation with localized error messages
- Business rules enforced in command handlers (e.g., visit completion requires payments)
- AutoMapper for object mapping between DTOs and entities
- SignalR for real-time notifications on key events

### Data Access
- Repository pattern with IRepository interfaces
- Unit of Work for transaction management
- EF Core for complex queries with eager loading