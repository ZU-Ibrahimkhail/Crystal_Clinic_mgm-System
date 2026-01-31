using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Accounting");

            migrationBuilder.EnsureSchema(
                name: "AssetMS");

            migrationBuilder.EnsureSchema(
                name: "HR");

            migrationBuilder.EnsureSchema(
                name: "Look");

            migrationBuilder.EnsureSchema(
                name: "General");

            migrationBuilder.EnsureSchema(
                name: "CrystalClinic");

            migrationBuilder.EnsureSchema(
                name: "HRLooks");

            migrationBuilder.EnsureSchema(
                name: "BranchStock");

            migrationBuilder.EnsureSchema(
                name: "Stock");

            migrationBuilder.CreateTable(
                name: "AdjustmentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AffectsFinancials = table.Column<bool>(type: "bit", nullable: false),
                    RequiresApproval = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ApprovalLevel = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdjustmentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AssetType",
                schema: "Look",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                schema: "General",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AttachmentType = table.Column<int>(type: "int", nullable: false),
                    AttachmentDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AuditDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BeforeValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AfterValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrelationId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RelatedEntityId = table.Column<int>(type: "int", nullable: true),
                    RelatedEntityType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankStatementImports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BankAccountId = table.Column<int>(type: "int", nullable: false),
                    StatementPeriodStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatementPeriodEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OpeningBalance = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ClosingBalance = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalTransactions = table.Column<int>(type: "int", nullable: false),
                    FileFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankStatementImports", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                schema: "Look",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Branch_Branch_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManufacturerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountryOfOrigin = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChartOfAccounts",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    AccountCategory = table.Column<int>(type: "int", nullable: false),
                    NormalBalance = table.Column<int>(type: "int", nullable: false),
                    IsSystemAccount = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentAccountId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartOfAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChartOfAccounts_ChartOfAccounts_ParentAccountId",
                        column: x => x.ParentAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContractType",
                schema: "HRLooks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyType",
                schema: "Look",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Doctor",
                schema: "CrystalClinic",
                columns: table => new
                {
                    doctorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    services = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    employeeId = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctor", x => x.doctorId);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseType",
                schema: "Look",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ForecastSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Scenario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SnapshotDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HorizonMonths = table.Column<int>(type: "int", nullable: false),
                    ForecastStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ForecastEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProjectedCashBalance = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProjectedAccountsReceivable = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProjectedAccountsPayable = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ProjectedNetIncome = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForecastSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryKits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KitName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFreeForPatient = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryKits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    IdempotencyToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryReservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategory",
                schema: "Stock",
                columns: table => new
                {
                    categoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategory", x => x.categoryId);
                });

            migrationBuilder.CreateTable(
                name: "LabTestTemplate",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NormalRangeMin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NormalRangeMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ExpectedResult = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabTestTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoanType",
                schema: "Look",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "News",
                schema: "General",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "DateTime", nullable: true),
                    EndTime = table.Column<DateTime>(type: "DateTime", nullable: true),
                    NewsDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Speaker = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ShowNotification = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
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
                    table.PrimaryKey("PK_News", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameInEnglish = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameInPashto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Patient",
                schema: "CrystalClinic",
                columns: table => new
                {
                    patientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    age = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(20)", nullable: true, defaultValue: "Unknown"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patient", x => x.patientId);
                });

            migrationBuilder.CreateTable(
                name: "PayType",
                schema: "Look",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RecurringJournalTemplate",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TemplateName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Frequency = table.Column<int>(type: "int", nullable: false),
                    NextRunDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringJournalTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceInventoryLink",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    QuantityRequired = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    table.PrimaryKey("PK_ServiceInventoryLink", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shareholder",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    OwnershipPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TotalInvestment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDrawings = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ContactInfo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
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
                    table.PrimaryKey("PK_Shareholder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                schema: "BranchStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingVideos",
                schema: "General",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DariTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PashtoTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Application = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Poster = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DariVideoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PashtoVideoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingVideos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankStatementLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankStatementImportId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RunningBalance = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsMatched = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankStatementLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankStatementLines_BankStatementImports_BankStatementImportId",
                        column: x => x.BankStatementImportId,
                        principalTable: "BankStatementImports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BranchDetails",
                schema: "Look",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeaderNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FooterNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchDetails_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Budget",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FiscalYear = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ApprovedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Budget_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "FixedAsset",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssetCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    PurchaseValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ResidualValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UsefulLifeMonths = table.Column<int>(type: "int", nullable: false),
                    AcquisitionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    AccumulatedDepreciation = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedAsset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedAsset_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "InventorySites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventorySites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventorySites_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntry",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntryNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    ApprovedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferenceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntry_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PositionTitle",
                schema: "HRLooks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    JobDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EnglishName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionTitle", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PositionTitle_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CompanyProfile",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    WhatsappNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BaseCurrencyId = table.Column<int>(type: "int", nullable: true),
                    CashAccountId = table.Column<int>(type: "int", nullable: false),
                    BankAccountId = table.Column<int>(type: "int", nullable: false),
                    AccountsReceivableAccountId = table.Column<int>(type: "int", nullable: false),
                    AccountsPayableAccountId = table.Column<int>(type: "int", nullable: false),
                    SalesRevenueAccountId = table.Column<int>(type: "int", nullable: false),
                    InventoryAccountId = table.Column<int>(type: "int", nullable: false),
                    PurchaseExpenseAccountId = table.Column<int>(type: "int", nullable: false),
                    IsInitialized = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_AccountsPayableAccountId",
                        column: x => x.AccountsPayableAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_AccountsReceivableAccountId",
                        column: x => x.AccountsReceivableAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_CashAccountId",
                        column: x => x.CashAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_InventoryAccountId",
                        column: x => x.InventoryAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_PurchaseExpenseAccountId",
                        column: x => x.PurchaseExpenseAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_SalesRevenueAccountId",
                        column: x => x.SalesRevenueAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompanyProfile_CurrencyType_BaseCurrencyId",
                        column: x => x.BaseCurrencyId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "CurrencyExchangeRates",
                columns: table => new
                {
                    CurrencyExchangeRateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromCurrencyId = table.Column<int>(type: "int", nullable: false),
                    ToCurrencyId = table.Column<int>(type: "int", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyExchangeRates", x => x.CurrencyExchangeRateId);
                    table.CheckConstraint("CK_CurrencyExchangeRate_DifferentCurrencies", "[FromCurrencyId] != [ToCurrencyId]");
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeRates_CurrencyType_FromCurrencyId",
                        column: x => x.FromCurrencyId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CurrencyExchangeRates_CurrencyType_ToCurrencyId",
                        column: x => x.ToCurrencyId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeProfile",
                schema: "HR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnglishFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PashtoFirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EnglishSurName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PashtoSurName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EnglishFatherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PashtoFatherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EnglishGrandFatherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PashtoGrandFatherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TazkiraTypeId = table.Column<int>(type: "int", nullable: false),
                    TazkiraNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JoldNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PageNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    RegNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "DateTime", nullable: false),
                    TemporaryAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PermenantAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    JoinDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    LeaveDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    LeaveRemark = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PersonalEmail = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmergencyPhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    HasAccount = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeProfile", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EmployeeProfile_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_EmployeeProfile_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "MainAccount",
                schema: "AssetMS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    DepositDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    OwnerUserId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    TotalDebitAmount = table.Column<float>(type: "real", nullable: false),
                    TotalCreditAmount = table.Column<float>(type: "real", nullable: false),
                    BalanceAmount = table.Column<float>(type: "real", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MainAccount", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MainAccount_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MainAccount_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MainAccount_MainAccount_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "AssetMS",
                        principalTable: "MainAccount",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                schema: "Stock",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionRate = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false, defaultValue: 0m),
                    ImagePath = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK_Service_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ForecastLines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForecastSnapshotId = table.Column<int>(type: "int", nullable: false),
                    ForecastMonth = table.Column<int>(type: "int", nullable: false),
                    ForecastDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MetricType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MetricName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProjectedValue = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    VariancePercentage = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForecastLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForecastLines_ForecastSnapshots_ForecastSnapshotId",
                        column: x => x.ForecastSnapshotId,
                        principalTable: "ForecastSnapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                schema: "Stock",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseUnit = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CurrentStock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UseableStock = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReorderLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    ItemCode = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    RequiresExpiration = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsInventoryItem = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ValuationMethod = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CostComponents = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastNRVAssessment = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NRVAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    WriteDownAmount = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    BrandId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Item_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Item_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Item_ItemCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Stock",
                        principalTable: "ItemCategory",
                        principalColumn: "categoryId");
                });

            migrationBuilder.CreateTable(
                name: "NewsNotification",
                schema: "General",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsNotification", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NewsNotification_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_NewsNotification_News_NewsId",
                        column: x => x.NewsId,
                        principalSchema: "General",
                        principalTable: "News",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Expense",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExpenseDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    IsReimbursable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ChartOfAccountId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubmittedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubmittedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expense", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Expense_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Expense_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Expense_Patient_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Patient",
                        principalColumn: "patientId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SalesInvoice",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    NetAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    SalesArea = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesInvoice_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SalesInvoice_Patient_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Patient",
                        principalColumn: "patientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecurringJournalLine",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecurringJournalTemplateId = table.Column<int>(type: "int", nullable: false),
                    ChartOfAccountId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CreditAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringJournalLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringJournalLine_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecurringJournalLine_RecurringJournalTemplate_RecurringJournalTemplateId",
                        column: x => x.RecurringJournalTemplateId,
                        principalSchema: "Accounting",
                        principalTable: "RecurringJournalTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EquityTransaction",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShareholderId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquityTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EquityTransaction_Shareholder_ShareholderId",
                        column: x => x.ShareholderId,
                        principalSchema: "Accounting",
                        principalTable: "Shareholder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountsPayable",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ChartOfAccountId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsPayable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountsPayable_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AccountsPayable_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AccountsPayable_CurrencyType_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AccountsPayable_Supplier_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "BranchStock",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrder",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PONumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ExpectedDeliveryDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PurchaseOrder_Supplier_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "BranchStock",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierDue",
                schema: "BranchStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    DueAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierDue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierDue_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierDue_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "BranchStock",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Visit",
                schema: "CrystalClinic",
                columns: table => new
                {
                    visitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    BranchDetailsId = table.Column<int>(type: "int", nullable: true),
                    patientId = table.Column<int>(type: "int", nullable: false),
                    doctorId = table.Column<int>(type: "int", nullable: true),
                    VisitDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visit", x => x.visitId);
                    table.ForeignKey(
                        name: "FK_Visit_BranchDetails_BranchDetailsId",
                        column: x => x.BranchDetailsId,
                        principalSchema: "Look",
                        principalTable: "BranchDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Visit_Doctor_doctorId",
                        column: x => x.doctorId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Doctor",
                        principalColumn: "doctorId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Visit_Patient_patientId",
                        column: x => x.patientId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Patient",
                        principalColumn: "patientId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BudgetLine",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BudgetId = table.Column<int>(type: "int", nullable: false),
                    ChartOfAccountId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    PeriodId = table.Column<int>(type: "int", nullable: false),
                    BudgetedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Variance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetLine_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BudgetLine_Budget_BudgetId",
                        column: x => x.BudgetId,
                        principalSchema: "Accounting",
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BudgetLine_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GeneralLedger",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChartOfAccountId = table.Column<int>(type: "int", nullable: false),
                    JournalEntryId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CreditAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralLedger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralLedger_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GeneralLedger_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GeneralLedger_JournalEntry_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "Accounting",
                        principalTable: "JournalEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JournalEntryLine",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JournalEntryId = table.Column<int>(type: "int", nullable: false),
                    ChartOfAccountId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DebitAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CreditAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 1m),
                    AmountInBaseCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    ChartOfAccountsId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JournalEntryLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalEntryLine_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JournalEntryLine_ChartOfAccounts_ChartOfAccountsId",
                        column: x => x.ChartOfAccountsId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JournalEntryLine_CurrencyType_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_JournalEntryLine_JournalEntry_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "Accounting",
                        principalTable: "JournalEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CallList",
                schema: "CrystalClinic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CallingReason = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    ToBeCalledDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ActualCalledDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    CallResponse = table.Column<int>(type: "int", nullable: false),
                    ResponseReasult = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedEmployeeId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CallList_EmployeeProfile_AssignedEmployeeId",
                        column: x => x.AssignedEmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ContractDetails",
                schema: "HR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeProfileId = table.Column<int>(type: "int", nullable: false),
                    ContractTypeId = table.Column<int>(type: "int", nullable: false),
                    PositionTitleId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    SalaryAmount = table.Column<float>(type: "real", nullable: false),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_ContractDetails", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ContractDetails_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ContractDetails_ContractType_ContractTypeId",
                        column: x => x.ContractTypeId,
                        principalSchema: "HRLooks",
                        principalTable: "ContractType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ContractDetails_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ContractDetails_EmployeeProfile_EmployeeProfileId",
                        column: x => x.EmployeeProfileId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ContractDetails_PositionTitle_PositionTitleId",
                        column: x => x.PositionTitleId,
                        principalSchema: "HRLooks",
                        principalTable: "PositionTitle",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AccountTracking",
                schema: "AssetMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    DebitAmount = table.Column<float>(type: "real", nullable: false),
                    CreditAmount = table.Column<float>(type: "real", nullable: false),
                    BalanceAmount = table.Column<float>(type: "real", nullable: false),
                    MainAccountId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    trackType = table.Column<int>(type: "int", nullable: false),
                    transactionStatus = table.Column<int>(type: "int", nullable: false),
                    approvedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    toUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    fromUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTracking", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AccountTracking_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AccountTracking_MainAccount_MainAccountId",
                        column: x => x.MainAccountId,
                        principalSchema: "AssetMS",
                        principalTable: "MainAccount",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AdvancePayment",
                schema: "HR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    PayTypeId = table.Column<int>(type: "int", nullable: false),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    MainAccountId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    AdvanceDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    AdvanceAmount = table.Column<float>(type: "real", nullable: false),
                    RemainingBalance = table.Column<float>(type: "real", nullable: false),
                    EachInstallmentAmount = table.Column<float>(type: "real", nullable: false),
                    PayedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancePayment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AdvancePayment_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AdvancePayment_EmployeeProfile_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AdvancePayment_MainAccount_MainAccountId",
                        column: x => x.MainAccountId,
                        principalSchema: "AssetMS",
                        principalTable: "MainAccount",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AdvancePayment_PayType_PayTypeId",
                        column: x => x.PayTypeId,
                        principalSchema: "Look",
                        principalTable: "PayType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ExpenseTracking",
                schema: "AssetMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    ExpenseTypeId = table.Column<int>(type: "int", nullable: false),
                    MainAccountId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AttachmentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTracking", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ExpenseTracking_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ExpenseTracking_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ExpenseTracking_ExpenseType_ExpenseTypeId",
                        column: x => x.ExpenseTypeId,
                        principalSchema: "Look",
                        principalTable: "ExpenseType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ExpenseTracking_MainAccount_MainAccountId",
                        column: x => x.MainAccountId,
                        principalSchema: "AssetMS",
                        principalTable: "MainAccount",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "TradeTracking",
                schema: "AssetMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    MainAccountId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    TradeAmount = table.Column<float>(type: "real", nullable: false),
                    ProfitAmount = table.Column<float>(type: "real", nullable: false),
                    LossAmount = table.Column<float>(type: "real", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeTracking", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TradeTracking_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TradeTracking_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TradeTracking_MainAccount_MainAccountId",
                        column: x => x.MainAccountId,
                        principalSchema: "AssetMS",
                        principalTable: "MainAccount",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "WithdrawalTracking",
                schema: "AssetMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    MainAccountId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    WithdrawalAmount = table.Column<float>(type: "real", nullable: false),
                    DepositAmount = table.Column<float>(type: "real", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawalTracking", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WithdrawalTracking_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_WithdrawalTracking_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_WithdrawalTracking_MainAccount_MainAccountId",
                        column: x => x.MainAccountId,
                        principalSchema: "AssetMS",
                        principalTable: "MainAccount",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "InventoryKitLine",
                schema: "BranchStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KitId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryKitLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryKitLine_InventoryKits_KitId",
                        column: x => x.KitId,
                        principalTable: "InventoryKits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryKitLine_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Stock",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemUnit",
                schema: "Stock",
                columns: table => new
                {
                    unitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ConversionFactor = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    DailyRentalPrice = table.Column<int>(type: "int", nullable: true),
                    SellingPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemUnit", x => x.unitId);
                    table.ForeignKey(
                        name: "FK_ItemUnit_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Stock",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                schema: "Stock",
                columns: table => new
                {
                    stockId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    BarCode = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LotNumber = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ManufactureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsExpired = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    QuantityRemaining = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SiteId = table.Column<int>(type: "int", nullable: true),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: true),
                    InvoiceId = table.Column<int>(type: "int", nullable: true),
                    FreightCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    InsuranceCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    ImportDuty = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    OtherLandingCosts = table.Column<decimal>(type: "decimal(18,4)", nullable: false, defaultValue: 0m),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.stockId);
                    table.ForeignKey(
                        name: "FK_Stock_InventorySites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "InventorySites",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stock_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Stock",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Stock_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "BranchStock",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SalesEstimate",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EstimateNumber = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    EstimateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConvertedToInvoiceId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesEstimate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesEstimate_Patient_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Patient",
                        principalColumn: "patientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesEstimate_SalesInvoice_ConvertedToInvoiceId",
                        column: x => x.ConvertedToInvoiceId,
                        principalSchema: "Accounting",
                        principalTable: "SalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "SalesInvoiceLine",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesInvoiceId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    InventoryItemId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesInvoiceLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesInvoiceLine_SalesInvoice_SalesInvoiceId",
                        column: x => x.SalesInvoiceId,
                        principalSchema: "Accounting",
                        principalTable: "SalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesReceipt",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesInvoiceId = table.Column<int>(type: "int", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    AmountReceived = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesReceipt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesReceipt_Patient_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Patient",
                        principalColumn: "patientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesReceipt_SalesInvoice_SalesInvoiceId",
                        column: x => x.SalesInvoiceId,
                        principalSchema: "Accounting",
                        principalTable: "SalesInvoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountsPayableId = table.Column<int>(type: "int", nullable: false),
                    PaymentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 1m),
                    AmountInBaseCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_AccountsPayable_AccountsPayableId",
                        column: x => x.AccountsPayableId,
                        principalSchema: "Accounting",
                        principalTable: "AccountsPayable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payment_CurrencyType_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "POLine",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    ItemDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LineTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReceivedQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_POLine_PurchaseOrder_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "Accounting",
                        principalTable: "PurchaseOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VendorBill",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BillNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PurchaseOrderId = table.Column<int>(type: "int", nullable: true),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    BillDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorBill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VendorBill_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VendorBill_PurchaseOrder_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalSchema: "Accounting",
                        principalTable: "PurchaseOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VendorBill_Supplier_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "BranchStock",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DuePayment",
                schema: "BranchStock",
                columns: table => new
                {
                    DuePaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierDueId = table.Column<int>(type: "int", nullable: false),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: true),
                    ExchangeRateToDueCurrency = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountInDueCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
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
                    table.PrimaryKey("PK_DuePayment", x => x.DuePaymentId);
                    table.ForeignKey(
                        name: "FK_DuePayment_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DuePayment_SupplierDue_SupplierDueId",
                        column: x => x.SupplierDueId,
                        principalSchema: "BranchStock",
                        principalTable: "SupplierDue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountsReceivable",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    InvoiceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    BalanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ChartOfAccountId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    VisitId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsReceivable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountsReceivable_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AccountsReceivable_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AccountsReceivable_CurrencyType_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_AccountsReceivable_Patient_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Patient",
                        principalColumn: "patientId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountsReceivable_Visit_VisitId",
                        column: x => x.VisitId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Visit",
                        principalColumn: "visitId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "LabOrderLine",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitId = table.Column<int>(type: "int", nullable: false),
                    TemplateId = table.Column<int>(type: "int", nullable: false),
                    ActualValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualResult = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsAbnormal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ResultDate = table.Column<DateTime>(type: "datetime", nullable: true),
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
                    table.PrimaryKey("PK_LabOrderLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabOrderLine_LabTestTemplate_TemplateId",
                        column: x => x.TemplateId,
                        principalSchema: "Accounting",
                        principalTable: "LabTestTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LabOrderLine_Visit_VisitId",
                        column: x => x.VisitId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Visit",
                        principalColumn: "visitId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProcedureLog",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitId = table.Column<int>(type: "int", nullable: false),
                    FixedAssetId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    DurationMinutes = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_ProcedureLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcedureLog_FixedAsset_FixedAssetId",
                        column: x => x.FixedAssetId,
                        principalSchema: "Accounting",
                        principalTable: "FixedAsset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProcedureLog_Visit_VisitId",
                        column: x => x.VisitId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Visit",
                        principalColumn: "visitId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VisitPayment",
                schema: "CrystalClinic",
                columns: table => new
                {
                    VisitPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: true),
                    ExchangeRateToAFN = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    paymentType = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    AmountInAFN = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    RefundAmountInAFN = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitPayment", x => x.VisitPaymentId);
                    table.ForeignKey(
                        name: "FK_VisitPayment_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VisitPayment_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalSchema: "Stock",
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VisitPayment_Visit_VisitId",
                        column: x => x.VisitId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Visit",
                        principalColumn: "visitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitServices",
                schema: "CrystalClinic",
                columns: table => new
                {
                    visitServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    visitId = table.Column<int>(type: "int", nullable: false),
                    serviceId = table.Column<int>(type: "int", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    totalSessions = table.Column<int>(type: "int", nullable: false),
                    completedSessions = table.Column<int>(type: "int", nullable: false),
                    PaidSessions = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    pricePerSession = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    totalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    nextSessionDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    paymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sessionStatus = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false, defaultValue: 2)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitServices", x => x.visitServiceId);
                    table.ForeignKey(
                        name: "FK_VisitServices_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_VisitServices_Service_serviceId",
                        column: x => x.serviceId,
                        principalSchema: "Stock",
                        principalTable: "Service",
                        principalColumn: "ServiceId");
                    table.ForeignKey(
                        name: "FK_VisitServices_Visit_visitId",
                        column: x => x.visitId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Visit",
                        principalColumn: "visitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankMatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankStatementImportId = table.Column<int>(type: "int", nullable: false),
                    BankStatementLineId = table.Column<int>(type: "int", nullable: false),
                    GeneralLedgerId = table.Column<int>(type: "int", nullable: true),
                    JournalEntryId = table.Column<int>(type: "int", nullable: true),
                    MatchedAmount = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    MatchDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReviewedBy = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankMatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankMatches_BankStatementImports_BankStatementImportId",
                        column: x => x.BankStatementImportId,
                        principalTable: "BankStatementImports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BankMatches_BankStatementLines_BankStatementLineId",
                        column: x => x.BankStatementLineId,
                        principalTable: "BankStatementLines",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BankMatches_GeneralLedger_GeneralLedgerId",
                        column: x => x.GeneralLedgerId,
                        principalSchema: "Accounting",
                        principalTable: "GeneralLedger",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BankMatches_JournalEntry_JournalEntryId",
                        column: x => x.JournalEntryId,
                        principalSchema: "Accounting",
                        principalTable: "JournalEntry",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PayrollTracking",
                schema: "HR",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    ContractDetailsId = table.Column<int>(type: "int", nullable: false),
                    PayTypeId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    BaseSalary = table.Column<float>(type: "real", nullable: false),
                    AdvanceDeduction = table.Column<float>(type: "real", nullable: false),
                    NetSalary = table.Column<float>(type: "real", nullable: false),
                    PayedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    IsPayed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollTracking", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PayrollTracking_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PayrollTracking_ContractDetails_ContractDetailsId",
                        column: x => x.ContractDetailsId,
                        principalSchema: "HR",
                        principalTable: "ContractDetails",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PayrollTracking_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PayrollTracking_EmployeeProfile_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_PayrollTracking_PayType_PayTypeId",
                        column: x => x.PayTypeId,
                        principalSchema: "Look",
                        principalTable: "PayType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ReservedItem",
                schema: "BranchStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    ReservedQuantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservedItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservedItem_InventoryReservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "InventoryReservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservedItem_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Stock",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReservedItem_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: "Stock",
                        principalTable: "Stock",
                        principalColumn: "stockId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockMovement",
                schema: "Stock",
                columns: table => new
                {
                    StockMovementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovementType = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    SourceBranchId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: true),
                    ProcessedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    UnitCost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AdjustmentCategoryId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovement", x => x.StockMovementId);
                    table.ForeignKey(
                        name: "FK_StockMovement_AdjustmentCategories_AdjustmentCategoryId",
                        column: x => x.AdjustmentCategoryId,
                        principalTable: "AdjustmentCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockMovement_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Stock",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovement_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: "Stock",
                        principalTable: "Stock",
                        principalColumn: "stockId");
                });

            migrationBuilder.CreateTable(
                name: "VisitMedication",
                schema: "CrystalClinic",
                columns: table => new
                {
                    medicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Dosage = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    visitId = table.Column<int>(type: "int", nullable: false),
                    stockId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitMedication", x => x.medicationId);
                    table.ForeignKey(
                        name: "FK_VisitMedication_Stock_stockId",
                        column: x => x.stockId,
                        principalSchema: "Stock",
                        principalTable: "Stock",
                        principalColumn: "stockId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitMedication_Visit_visitId",
                        column: x => x.visitId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Visit",
                        principalColumn: "visitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SalesEstimateLine",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesEstimateId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesEstimateLine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesEstimateLine_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Stock",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SalesEstimateLine_SalesEstimate_SalesEstimateId",
                        column: x => x.SalesEstimateId,
                        principalSchema: "Accounting",
                        principalTable: "SalesEstimate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalesEstimateLine_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalSchema: "Stock",
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Receipt",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountsReceivableId = table.Column<int>(type: "int", nullable: false),
                    ReceiptNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceiptDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    Reference = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false, defaultValue: 1m),
                    AmountInBaseCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OriginalReceiptId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receipt_AccountsReceivable_AccountsReceivableId",
                        column: x => x.AccountsReceivableId,
                        principalSchema: "Accounting",
                        principalTable: "AccountsReceivable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Receipt_CurrencyType_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Receipt_Receipt_OriginalReceiptId",
                        column: x => x.OriginalReceiptId,
                        principalSchema: "Accounting",
                        principalTable: "Receipt",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceSessions",
                schema: "CrystalClinic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    VisitServiceId = table.Column<int>(type: "int", nullable: false),
                    VisitId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    PatientName = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    ContactInfo = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    PriceInAFN = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    IsImplemented = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ImplementationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImplementorEmployeeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceSessions_EmployeeProfile_ImplementorEmployeeId",
                        column: x => x.ImplementorEmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ServiceSessions_VisitServices_VisitServiceId",
                        column: x => x.VisitServiceId,
                        principalSchema: "CrystalClinic",
                        principalTable: "VisitServices",
                        principalColumn: "visitServiceId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountsPayable_BranchId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsPayable_ChartOfAccountId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsPayable_CurrencyId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsPayable_InvoiceNumber",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountsPayable_VendorId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsReceivable_BranchId",
                schema: "Accounting",
                table: "AccountsReceivable",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsReceivable_ChartOfAccountId",
                schema: "Accounting",
                table: "AccountsReceivable",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsReceivable_CurrencyId",
                schema: "Accounting",
                table: "AccountsReceivable",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsReceivable_CustomerId",
                schema: "Accounting",
                table: "AccountsReceivable",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsReceivable_InvoiceNumber",
                schema: "Accounting",
                table: "AccountsReceivable",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountsReceivable_VisitId",
                schema: "Accounting",
                table: "AccountsReceivable",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTracking_CurrencyTypeId",
                schema: "AssetMS",
                table: "AccountTracking",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountTracking_MainAccountId",
                schema: "AssetMS",
                table: "AccountTracking",
                column: "MainAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvancePayment_CurrencyTypeId",
                schema: "HR",
                table: "AdvancePayment",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvancePayment_EmployeeId",
                schema: "HR",
                table: "AdvancePayment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvancePayment_MainAccountId",
                schema: "HR",
                table: "AdvancePayment",
                column: "MainAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvancePayment_PayTypeId",
                schema: "HR",
                table: "AdvancePayment",
                column: "PayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_Action",
                table: "AuditTrails",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_AuditDate",
                table: "AuditTrails",
                column: "AuditDate");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_CorrelationId",
                table: "AuditTrails",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_EntityId",
                table: "AuditTrails",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_EntityType",
                table: "AuditTrails",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_EntityType_EntityId",
                table: "AuditTrails",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditTrails_UserId",
                table: "AuditTrails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BankMatches_BankStatementImportId",
                table: "BankMatches",
                column: "BankStatementImportId");

            migrationBuilder.CreateIndex(
                name: "IX_BankMatches_BankStatementLineId",
                table: "BankMatches",
                column: "BankStatementLineId");

            migrationBuilder.CreateIndex(
                name: "IX_BankMatches_GeneralLedgerId",
                table: "BankMatches",
                column: "GeneralLedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_BankMatches_JournalEntryId",
                table: "BankMatches",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_BankMatches_MatchDate",
                table: "BankMatches",
                column: "MatchDate");

            migrationBuilder.CreateIndex(
                name: "IX_BankMatches_Status",
                table: "BankMatches",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BankStatementImports_BankAccountId",
                table: "BankStatementImports",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_BankStatementImports_ImportDate",
                table: "BankStatementImports",
                column: "ImportDate");

            migrationBuilder.CreateIndex(
                name: "IX_BankStatementImports_Status",
                table: "BankStatementImports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BankStatementLines_BankStatementImportId",
                table: "BankStatementLines",
                column: "BankStatementImportId");

            migrationBuilder.CreateIndex(
                name: "IX_BankStatementLines_IsMatched",
                table: "BankStatementLines",
                column: "IsMatched");

            migrationBuilder.CreateIndex(
                name: "IX_BankStatementLines_TransactionDate",
                table: "BankStatementLines",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_ParentId",
                schema: "Look",
                table: "Branch",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_BranchDetails_BranchId",
                schema: "Look",
                table: "BranchDetails",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Budget_BranchId",
                schema: "Accounting",
                table: "Budget",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLine_BranchId",
                schema: "Accounting",
                table: "BudgetLine",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLine_BudgetId",
                schema: "Accounting",
                table: "BudgetLine",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetLine_ChartOfAccountId",
                schema: "Accounting",
                table: "BudgetLine",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CallList_AssignedEmployeeId",
                schema: "CrystalClinic",
                table: "CallList",
                column: "AssignedEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_AccountCode",
                schema: "Accounting",
                table: "ChartOfAccounts",
                column: "AccountCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_ParentAccountId",
                schema: "Accounting",
                table: "ChartOfAccounts",
                column: "ParentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_AccountsPayableAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "AccountsPayableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_AccountsReceivableAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "AccountsReceivableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_BankAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_BaseCurrencyId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "BaseCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_CashAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "CashAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_InventoryAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "InventoryAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_PurchaseExpenseAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "PurchaseExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_SalesRevenueAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "SalesRevenueAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDetails_BranchId",
                schema: "HR",
                table: "ContractDetails",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDetails_ContractTypeId",
                schema: "HR",
                table: "ContractDetails",
                column: "ContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDetails_CurrencyTypeId",
                schema: "HR",
                table: "ContractDetails",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDetails_EmployeeProfileId",
                schema: "HR",
                table: "ContractDetails",
                column: "EmployeeProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ContractDetails_PositionTitleId",
                schema: "HR",
                table: "ContractDetails",
                column: "PositionTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_FromCurrencyId",
                table: "CurrencyExchangeRates",
                column: "FromCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_ToCurrencyId",
                table: "CurrencyExchangeRates",
                column: "ToCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_DuePayment_CurrencyTypeId",
                schema: "BranchStock",
                table: "DuePayment",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DuePayment_SupplierDueId",
                schema: "BranchStock",
                table: "DuePayment",
                column: "SupplierDueId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfile_BranchId",
                schema: "HR",
                table: "EmployeeProfile",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProfile_CurrencyTypeId",
                schema: "HR",
                table: "EmployeeProfile",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_EquityTransaction_ShareholderId",
                schema: "Accounting",
                table: "EquityTransaction",
                column: "ShareholderId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_BranchId",
                schema: "Accounting",
                table: "Expense",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_ChartOfAccountId",
                schema: "Accounting",
                table: "Expense",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expense_CustomerId",
                schema: "Accounting",
                table: "Expense",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTracking_BranchId",
                schema: "AssetMS",
                table: "ExpenseTracking",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTracking_CurrencyTypeId",
                schema: "AssetMS",
                table: "ExpenseTracking",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTracking_ExpenseTypeId",
                schema: "AssetMS",
                table: "ExpenseTracking",
                column: "ExpenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTracking_MainAccountId",
                schema: "AssetMS",
                table: "ExpenseTracking",
                column: "MainAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAsset_AssetCode",
                schema: "Accounting",
                table: "FixedAsset",
                column: "AssetCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FixedAsset_BranchId",
                schema: "Accounting",
                table: "FixedAsset",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ForecastLines_ForecastMonth",
                table: "ForecastLines",
                column: "ForecastMonth");

            migrationBuilder.CreateIndex(
                name: "IX_ForecastLines_ForecastSnapshotId",
                table: "ForecastLines",
                column: "ForecastSnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_ForecastLines_MetricType",
                table: "ForecastLines",
                column: "MetricType");

            migrationBuilder.CreateIndex(
                name: "IX_ForecastSnapshots_Scenario",
                table: "ForecastSnapshots",
                column: "Scenario");

            migrationBuilder.CreateIndex(
                name: "IX_ForecastSnapshots_SnapshotDate",
                table: "ForecastSnapshots",
                column: "SnapshotDate");

            migrationBuilder.CreateIndex(
                name: "IX_ForecastSnapshots_Status",
                table: "ForecastSnapshots",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralLedger_BranchId",
                schema: "Accounting",
                table: "GeneralLedger",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralLedger_ChartOfAccountId_TransactionDate",
                schema: "Accounting",
                table: "GeneralLedger",
                columns: new[] { "ChartOfAccountId", "TransactionDate" });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralLedger_JournalEntryId",
                schema: "Accounting",
                table: "GeneralLedger",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryKitLine_ItemId",
                schema: "BranchStock",
                table: "InventoryKitLine",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryKitLine_KitId",
                schema: "BranchStock",
                table: "InventoryKitLine",
                column: "KitId");

            migrationBuilder.CreateIndex(
                name: "IX_InventorySites_BranchId",
                table: "InventorySites",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_BranchId",
                schema: "Stock",
                table: "Item",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_BrandId",
                schema: "Stock",
                table: "Item",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_CategoryId",
                schema: "Stock",
                table: "Item",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Name",
                schema: "Stock",
                table: "Item",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnit_ItemId",
                schema: "Stock",
                table: "ItemUnit",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntry_BranchId",
                schema: "Accounting",
                table: "JournalEntry",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntry_EntryNumber",
                schema: "Accounting",
                table: "JournalEntry",
                column: "EntryNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLine_ChartOfAccountId",
                schema: "Accounting",
                table: "JournalEntryLine",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLine_ChartOfAccountsId",
                schema: "Accounting",
                table: "JournalEntryLine",
                column: "ChartOfAccountsId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLine_CurrencyId",
                schema: "Accounting",
                table: "JournalEntryLine",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalEntryLine_JournalEntryId",
                schema: "Accounting",
                table: "JournalEntryLine",
                column: "JournalEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrderLine_TemplateId",
                schema: "Accounting",
                table: "LabOrderLine",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_LabOrderLine_VisitId",
                schema: "Accounting",
                table: "LabOrderLine",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_MainAccount_BranchId",
                schema: "AssetMS",
                table: "MainAccount",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_MainAccount_CurrencyTypeId",
                schema: "AssetMS",
                table: "MainAccount",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MainAccount_ParentId",
                schema: "AssetMS",
                table: "MainAccount",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsNotification_BranchId",
                schema: "General",
                table: "NewsNotification",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsNotification_NewsId",
                schema: "General",
                table: "NewsNotification",
                column: "NewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_AccountsPayableId",
                schema: "Accounting",
                table: "Payment",
                column: "AccountsPayableId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_CurrencyId",
                schema: "Accounting",
                table: "Payment",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTracking_BranchId",
                schema: "HR",
                table: "PayrollTracking",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTracking_ContractDetailsId",
                schema: "HR",
                table: "PayrollTracking",
                column: "ContractDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTracking_CurrencyTypeId",
                schema: "HR",
                table: "PayrollTracking",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTracking_EmployeeId",
                schema: "HR",
                table: "PayrollTracking",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollTracking_PayTypeId",
                schema: "HR",
                table: "PayrollTracking",
                column: "PayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_POLine_PurchaseOrderId",
                schema: "Accounting",
                table: "POLine",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionTitle_BranchId",
                schema: "HRLooks",
                table: "PositionTitle",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureLog_FixedAssetId",
                schema: "Accounting",
                table: "ProcedureLog",
                column: "FixedAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcedureLog_VisitId",
                schema: "Accounting",
                table: "ProcedureLog",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_BranchId",
                schema: "Accounting",
                table: "PurchaseOrder",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_PONumber",
                schema: "Accounting",
                table: "PurchaseOrder",
                column: "PONumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrder_VendorId",
                schema: "Accounting",
                table: "PurchaseOrder",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_AccountsReceivableId",
                schema: "Accounting",
                table: "Receipt",
                column: "AccountsReceivableId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_CurrencyId",
                schema: "Accounting",
                table: "Receipt",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_OriginalReceiptId",
                schema: "Accounting",
                table: "Receipt",
                column: "OriginalReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringJournalLine_ChartOfAccountId",
                schema: "Accounting",
                table: "RecurringJournalLine",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringJournalLine_RecurringJournalTemplateId",
                schema: "Accounting",
                table: "RecurringJournalLine",
                column: "RecurringJournalTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedItem_ItemId",
                schema: "BranchStock",
                table: "ReservedItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedItem_ReservationId",
                schema: "BranchStock",
                table: "ReservedItem",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservedItem_StockId",
                schema: "BranchStock",
                table: "ReservedItem",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesEstimate_ConvertedToInvoiceId",
                schema: "Accounting",
                table: "SalesEstimate",
                column: "ConvertedToInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesEstimate_PatientId",
                schema: "Accounting",
                table: "SalesEstimate",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesEstimateLine_ItemId",
                schema: "Accounting",
                table: "SalesEstimateLine",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesEstimateLine_SalesEstimateId",
                schema: "Accounting",
                table: "SalesEstimateLine",
                column: "SalesEstimateId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesEstimateLine_ServiceId",
                schema: "Accounting",
                table: "SalesEstimateLine",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_BranchId",
                schema: "Accounting",
                table: "SalesInvoice",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_CustomerId",
                schema: "Accounting",
                table: "SalesInvoice",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_InvoiceNumber",
                schema: "Accounting",
                table: "SalesInvoice",
                column: "InvoiceNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoiceLine_SalesInvoiceId",
                schema: "Accounting",
                table: "SalesInvoiceLine",
                column: "SalesInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReceipt_CustomerId",
                schema: "Accounting",
                table: "SalesReceipt",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesReceipt_SalesInvoiceId",
                schema: "Accounting",
                table: "SalesReceipt",
                column: "SalesInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CurrencyTypeId",
                schema: "Stock",
                table: "Service",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_Name",
                schema: "Stock",
                table: "Service",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceInventoryLink_ServiceId_InventoryItemId",
                schema: "Accounting",
                table: "ServiceInventoryLink",
                columns: new[] { "ServiceId", "InventoryItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSessions_ImplementorEmployeeId",
                schema: "CrystalClinic",
                table: "ServiceSessions",
                column: "ImplementorEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSessions_VisitServiceId",
                schema: "CrystalClinic",
                table: "ServiceSessions",
                column: "VisitServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ItemId",
                schema: "Stock",
                table: "Stock",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_SiteId",
                schema: "Stock",
                table: "Stock",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_SupplierId",
                schema: "Stock",
                table: "Stock",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_AdjustmentCategoryId",
                schema: "Stock",
                table: "StockMovement",
                column: "AdjustmentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_ItemId",
                schema: "Stock",
                table: "StockMovement",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_StockId",
                schema: "Stock",
                table: "StockMovement",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierDue_CurrencyTypeId",
                schema: "BranchStock",
                table: "SupplierDue",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierDue_SupplierId",
                schema: "BranchStock",
                table: "SupplierDue",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeTracking_BranchId",
                schema: "AssetMS",
                table: "TradeTracking",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeTracking_CurrencyTypeId",
                schema: "AssetMS",
                table: "TradeTracking",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeTracking_MainAccountId",
                schema: "AssetMS",
                table: "TradeTracking",
                column: "MainAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorBill_BillNumber",
                schema: "Accounting",
                table: "VendorBill",
                column: "BillNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorBill_BranchId",
                schema: "Accounting",
                table: "VendorBill",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorBill_PurchaseOrderId",
                schema: "Accounting",
                table: "VendorBill",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_VendorBill_VendorId",
                schema: "Accounting",
                table: "VendorBill",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_BranchDetailsId",
                schema: "CrystalClinic",
                table: "Visit",
                column: "BranchDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_doctorId",
                schema: "CrystalClinic",
                table: "Visit",
                column: "doctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Visit_patientId",
                schema: "CrystalClinic",
                table: "Visit",
                column: "patientId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitMedication_stockId",
                schema: "CrystalClinic",
                table: "VisitMedication",
                column: "stockId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitMedication_visitId",
                schema: "CrystalClinic",
                table: "VisitMedication",
                column: "visitId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitPayment_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitPayment",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitPayment_ServiceId",
                schema: "CrystalClinic",
                table: "VisitPayment",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitPayment_VisitId",
                schema: "CrystalClinic",
                table: "VisitPayment",
                column: "VisitId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitServices_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitServices_serviceId",
                schema: "CrystalClinic",
                table: "VisitServices",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitServices_visitId",
                schema: "CrystalClinic",
                table: "VisitServices",
                column: "visitId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalTracking_BranchId",
                schema: "AssetMS",
                table: "WithdrawalTracking",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalTracking_CurrencyTypeId",
                schema: "AssetMS",
                table: "WithdrawalTracking",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalTracking_MainAccountId",
                schema: "AssetMS",
                table: "WithdrawalTracking",
                column: "MainAccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTracking",
                schema: "AssetMS");

            migrationBuilder.DropTable(
                name: "AdvancePayment",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "AssetType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "Attachments",
                schema: "General");

            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "BankMatches");

            migrationBuilder.DropTable(
                name: "BudgetLine",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "CallList",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "CompanyProfile",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "CurrencyExchangeRates");

            migrationBuilder.DropTable(
                name: "DuePayment",
                schema: "BranchStock");

            migrationBuilder.DropTable(
                name: "EquityTransaction",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "Expense",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "ExpenseTracking",
                schema: "AssetMS");

            migrationBuilder.DropTable(
                name: "ForecastLines");

            migrationBuilder.DropTable(
                name: "InventoryKitLine",
                schema: "BranchStock");

            migrationBuilder.DropTable(
                name: "ItemUnit",
                schema: "Stock");

            migrationBuilder.DropTable(
                name: "JournalEntryLine",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "LabOrderLine",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "LoanType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "NewsNotification",
                schema: "General");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "Payment",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "PayrollTracking",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "POLine",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "ProcedureLog",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "Receipt",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "RecurringJournalLine",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "ReservedItem",
                schema: "BranchStock");

            migrationBuilder.DropTable(
                name: "SalesEstimateLine",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "SalesInvoiceLine",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "SalesReceipt",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "ServiceInventoryLink",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "ServiceSessions",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "StockMovement",
                schema: "Stock");

            migrationBuilder.DropTable(
                name: "TradeTracking",
                schema: "AssetMS");

            migrationBuilder.DropTable(
                name: "TrainingVideos",
                schema: "General");

            migrationBuilder.DropTable(
                name: "VendorBill",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "VisitMedication",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "VisitPayment",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "WithdrawalTracking",
                schema: "AssetMS");

            migrationBuilder.DropTable(
                name: "BankStatementLines");

            migrationBuilder.DropTable(
                name: "GeneralLedger",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "Budget",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "SupplierDue",
                schema: "BranchStock");

            migrationBuilder.DropTable(
                name: "Shareholder",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "ExpenseType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "ForecastSnapshots");

            migrationBuilder.DropTable(
                name: "InventoryKits");

            migrationBuilder.DropTable(
                name: "LabTestTemplate",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "News",
                schema: "General");

            migrationBuilder.DropTable(
                name: "AccountsPayable",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "ContractDetails",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "PayType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "FixedAsset",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "AccountsReceivable",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "RecurringJournalTemplate",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "InventoryReservations");

            migrationBuilder.DropTable(
                name: "SalesEstimate",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "VisitServices",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "AdjustmentCategories");

            migrationBuilder.DropTable(
                name: "PurchaseOrder",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "Stock",
                schema: "Stock");

            migrationBuilder.DropTable(
                name: "MainAccount",
                schema: "AssetMS");

            migrationBuilder.DropTable(
                name: "BankStatementImports");

            migrationBuilder.DropTable(
                name: "JournalEntry",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "ContractType",
                schema: "HRLooks");

            migrationBuilder.DropTable(
                name: "EmployeeProfile",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "PositionTitle",
                schema: "HRLooks");

            migrationBuilder.DropTable(
                name: "ChartOfAccounts",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "SalesInvoice",
                schema: "Accounting");

            migrationBuilder.DropTable(
                name: "Service",
                schema: "Stock");

            migrationBuilder.DropTable(
                name: "Visit",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "InventorySites");

            migrationBuilder.DropTable(
                name: "Item",
                schema: "Stock");

            migrationBuilder.DropTable(
                name: "Supplier",
                schema: "BranchStock");

            migrationBuilder.DropTable(
                name: "CurrencyType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "BranchDetails",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "Doctor",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "Patient",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "ItemCategory",
                schema: "Stock");

            migrationBuilder.DropTable(
                name: "Branch",
                schema: "Look");
        }
    }
}
