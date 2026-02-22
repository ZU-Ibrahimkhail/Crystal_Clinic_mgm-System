using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class FixEnumDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                schema: "HRLooks",
                table: "PositionTitle",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "PashtoName",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "EnglishName",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "DariName",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "JobGrade",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxSalary",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MinSalary",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ReportsToId",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BankAccountNo",
                schema: "HR",
                table: "EmployeeProfile",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                schema: "HR",
                table: "EmployeeProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCode",
                schema: "HR",
                table: "EmployeeProfile",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmploymentStatus",
                schema: "HR",
                table: "EmployeeProfile",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "ManagerId",
                schema: "HR",
                table: "EmployeeProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreferredPaymentMethod",
                schema: "HR",
                table: "EmployeeProfile",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkEmail",
                schema: "HR",
                table: "EmployeeProfile",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Department",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeptCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ParentDepartmentId = table.Column<int>(type: "int", nullable: true),
                    HeadEmployeeId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Department_Department_ParentDepartmentId",
                        column: x => x.ParentDepartmentId,
                        principalSchema: "HR",
                        principalTable: "Department",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LeaveType",
                schema: "HR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaxDaysPerYear = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AllowCarryover = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    MaxCarryoverDays = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingTaskTemplate",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingTaskTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayrollAdjustment",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdjustmentDate = table.Column<DateTime>(type: "date", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollAdjustment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollAdjustment_EmployeeProfile_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "PayrollComponent",
                schema: "HR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CalculationType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ChartOfAccountId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollComponent", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PayrollContract",
                schema: "HR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeProfileId = table.Column<int>(type: "int", nullable: false),
                    ContractTypeId = table.Column<int>(type: "int", nullable: false),
                    PositionTitleId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    BranchID = table.Column<int>(type: "int", nullable: true),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    BaseSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayCycle = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    InsuranceDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollContract", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PayrollContract_Branch_BranchID",
                        column: x => x.BranchID,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PayrollContract_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PayrollContract_ContractType_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalSchema: "HRLooks",
                        principalTable: "ContractType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PayrollContract_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PayrollContract_EmployeeProfile_EmployeeProfileId",
                        column: x => x.EmployeeProfileId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PayrollContract_PositionTitle_PositionTitleId",
                        column: x => x.PositionTitleId,
                        principalSchema: "HRLooks",
                        principalTable: "PositionTitle",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Shift",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    GracePeriodMinutes = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxConfiguration",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaxType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveCarryover",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false),
                    CarryoverYear = table.Column<int>(type: "int", nullable: false),
                    RemainingDays = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    UsedDays = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    ExpirationDays = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    ExpirationDate = table.Column<DateTime>(type: "date", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveCarryover", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveCarryover_EmployeeProfile_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LeaveCarryover_LeaveType_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalSchema: "HR",
                        principalTable: "LeaveType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "LeaveRequest",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    LeaveTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    TotalDays = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ApprovedById = table.Column<int>(type: "int", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ApprovalNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaveRequest_EmployeeProfile_ApprovedById",
                        column: x => x.ApprovedById,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LeaveRequest_EmployeeProfile_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LeaveRequest_LeaveType_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalSchema: "HR",
                        principalTable: "LeaveType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "OnboardingTaskTemplateLine",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    TaskName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskOrder = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AssignedToDepartmentId = table.Column<int>(type: "int", nullable: true),
                    AssignedToRole = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingTaskTemplateLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnboardingTaskTemplateLine_OnboardingTaskTemplate_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "HR",
                        principalTable: "OnboardingTaskTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeePayrollComponent",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ComponentId = table.Column<int>(type: "int", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    OverrideAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeePayrollComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeePayrollComponent_EmployeeProfile_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_EmployeePayrollComponent_PayrollComponent_ComponentId",
                        column: x => x.ComponentId,
                        principalSchema: "HR",
                        principalTable: "PayrollComponent",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AttendanceRecord",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: true),
                    CheckIn = table.Column<DateTime>(type: "datetime", nullable: false),
                    CheckOut = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    WorkingHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    OvertimeHours = table.Column<decimal>(type: "decimal(5,2)", nullable: false, defaultValue: 0m),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttendanceDate = table.Column<DateTime>(type: "date", nullable: false),
                    PayrollContractID = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceRecord_EmployeeProfile_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AttendanceRecord_PayrollContract_PayrollContractID",
                        column: x => x.PayrollContractID,
                        principalSchema: "HR",
                        principalTable: "PayrollContract",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AttendanceRecord_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: "HR",
                        principalTable: "Shift",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaxBracket",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxConfigurationId = table.Column<int>(type: "int", nullable: false),
                    MinAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    FlatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CalculationType = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    BracketOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxBracket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxBracket_TaxConfiguration_TaxConfigurationId",
                        column: x => x.TaxConfigurationId,
                        principalSchema: "HR",
                        principalTable: "TaxConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HRTask",
                schema: "HR",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    TemplateLineId = table.Column<int>(type: "int", nullable: true),
                    TaskName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    AssignedToEmployeeId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRTask", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HRTask_EmployeeProfile_AssignedToEmployeeId",
                        column: x => x.AssignedToEmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HRTask_EmployeeProfile_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_HRTask_OnboardingTaskTemplateLine_TemplateLineId",
                        column: x => x.TemplateLineId,
                        principalSchema: "HR",
                        principalTable: "OnboardingTaskTemplateLine",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PositionTitle_ReportsToId",
                schema: "HRLooks",
                table: "PositionTitle",
                column: "ReportsToId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfile_DepartmentId",
                schema: "HR",
                table: "EmployeeProfile",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfile_ManagerId",
                schema: "HR",
                table: "EmployeeProfile",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecord_EmployeeId_AttendanceDate",
                schema: "HR",
                table: "AttendanceRecord",
                columns: new[] { "EmployeeId", "AttendanceDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecord_PayrollContractID",
                schema: "HR",
                table: "AttendanceRecord",
                column: "PayrollContractID");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceRecord_ShiftId",
                schema: "HR",
                table: "AttendanceRecord",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Department_ParentDepartmentId",
                schema: "HR",
                table: "Department",
                column: "ParentDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayrollComponent_ComponentId",
                schema: "HR",
                table: "EmployeePayrollComponent",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayrollComponent_EmployeeId",
                schema: "HR",
                table: "EmployeePayrollComponent",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeePayrollComponent_EmployeeId_ComponentId",
                schema: "HR",
                table: "EmployeePayrollComponent",
                columns: new[] { "EmployeeId", "ComponentId" });

            migrationBuilder.CreateIndex(
                name: "IX_HRTask_AssignedToEmployeeId",
                schema: "HR",
                table: "HRTask",
                column: "AssignedToEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HRTask_EmployeeId_Category",
                schema: "HR",
                table: "HRTask",
                columns: new[] { "EmployeeId", "Category" });

            migrationBuilder.CreateIndex(
                name: "IX_HRTask_Status",
                schema: "HR",
                table: "HRTask",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_HRTask_TemplateLineId",
                schema: "HR",
                table: "HRTask",
                column: "TemplateLineId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveCarryover_EmployeeId_LeaveTypeId_CarryoverYear",
                schema: "HR",
                table: "LeaveCarryover",
                columns: new[] { "EmployeeId", "LeaveTypeId", "CarryoverYear" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaveCarryover_LeaveTypeId",
                schema: "HR",
                table: "LeaveCarryover",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_ApprovedById",
                schema: "HR",
                table: "LeaveRequest",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_EmployeeId",
                schema: "HR",
                table: "LeaveRequest",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_LeaveTypeId",
                schema: "HR",
                table: "LeaveRequest",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaveRequest_Status",
                schema: "HR",
                table: "LeaveRequest",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingTaskTemplateLine_TemplateId_TaskOrder",
                schema: "HR",
                table: "OnboardingTaskTemplateLine",
                columns: new[] { "TemplateId", "TaskOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollAdjustment_EmployeeId_AdjustmentDate",
                schema: "HR",
                table: "PayrollAdjustment",
                columns: new[] { "EmployeeId", "AdjustmentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_PayrollAdjustment_IsProcessed",
                schema: "HR",
                table: "PayrollAdjustment",
                column: "IsProcessed");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollContract_BranchId",
                schema: "HR",
                table: "PayrollContract",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollContract_BranchID",
                schema: "HR",
                table: "PayrollContract",
                column: "BranchID");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollContract_ContractTypeId",
                schema: "HR",
                table: "PayrollContract",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollContract_CurrencyTypeId",
                schema: "HR",
                table: "PayrollContract",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollContract_EmployeeProfileId",
                schema: "HR",
                table: "PayrollContract",
                column: "EmployeeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollContract_PositionTitleId",
                schema: "HR",
                table: "PayrollContract",
                column: "PositionTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxBracket_TaxConfigurationId_BracketOrder",
                schema: "HR",
                table: "TaxBracket",
                columns: new[] { "TaxConfigurationId", "BracketOrder" });

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProfile_Department_DepartmentId",
                schema: "HR",
                table: "EmployeeProfile",
                column: "DepartmentId",
                principalSchema: "HR",
                principalTable: "Department",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProfile_EmployeeProfile_ManagerId",
                schema: "HR",
                table: "EmployeeProfile",
                column: "ManagerId",
                principalSchema: "HR",
                principalTable: "EmployeeProfile",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_PositionTitle_PositionTitle_ReportsToId",
                schema: "HRLooks",
                table: "PositionTitle",
                column: "ReportsToId",
                principalSchema: "HRLooks",
                principalTable: "PositionTitle",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProfile_Department_DepartmentId",
                schema: "HR",
                table: "EmployeeProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProfile_EmployeeProfile_ManagerId",
                schema: "HR",
                table: "EmployeeProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionTitle_PositionTitle_ReportsToId",
                schema: "HRLooks",
                table: "PositionTitle");

            migrationBuilder.DropTable(
                name: "AttendanceRecord",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Department",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "EmployeePayrollComponent",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "HRTask",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "LeaveCarryover",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "LeaveRequest",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "PayrollAdjustment",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "TaxBracket",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "PayrollContract",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "Shift",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "PayrollComponent",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "OnboardingTaskTemplateLine",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "LeaveType",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "TaxConfiguration",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "OnboardingTaskTemplate",
                schema: "HR");

            migrationBuilder.DropIndex(
                name: "IX_PositionTitle_ReportsToId",
                schema: "HRLooks",
                table: "PositionTitle");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeProfile_DepartmentId",
                schema: "HR",
                table: "EmployeeProfile");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeProfile_ManagerId",
                schema: "HR",
                table: "EmployeeProfile");

            migrationBuilder.DropColumn(
                name: "JobGrade",
                schema: "HRLooks",
                table: "PositionTitle");

            migrationBuilder.DropColumn(
                name: "MaxSalary",
                schema: "HRLooks",
                table: "PositionTitle");

            migrationBuilder.DropColumn(
                name: "MinSalary",
                schema: "HRLooks",
                table: "PositionTitle");

            migrationBuilder.DropColumn(
                name: "ReportsToId",
                schema: "HRLooks",
                table: "PositionTitle");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "HRLooks",
                table: "PositionTitle");

            migrationBuilder.DropColumn(
                name: "BankAccountNo",
                schema: "HR",
                table: "EmployeeProfile");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                schema: "HR",
                table: "EmployeeProfile");

            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                schema: "HR",
                table: "EmployeeProfile");

            migrationBuilder.DropColumn(
                name: "EmploymentStatus",
                schema: "HR",
                table: "EmployeeProfile");

            migrationBuilder.DropColumn(
                name: "ManagerId",
                schema: "HR",
                table: "EmployeeProfile");

            migrationBuilder.DropColumn(
                name: "PreferredPaymentMethod",
                schema: "HR",
                table: "EmployeeProfile");

            migrationBuilder.DropColumn(
                name: "WorkEmail",
                schema: "HR",
                table: "EmployeeProfile");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "HRLooks",
                table: "PositionTitle",
                newName: "ID");

            migrationBuilder.AlterColumn<string>(
                name: "PashtoName",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EnglishName",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DariName",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
