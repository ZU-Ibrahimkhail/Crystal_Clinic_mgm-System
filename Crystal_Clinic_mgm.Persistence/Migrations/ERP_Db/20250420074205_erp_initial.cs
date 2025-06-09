using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class erp_initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "AssetMS");

            migrationBuilder.EnsureSchema(
                name: "HR");

            migrationBuilder.EnsureSchema(
                name: "Look");

            migrationBuilder.EnsureSchema(
                name: "PropertyMS");

            migrationBuilder.EnsureSchema(
                name: "General");

            migrationBuilder.EnsureSchema(
                name: "HRLooks");

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
                name: "Category",
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
                    table.PrimaryKey("PK_Category", x => x.ID);
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
                    AttachmentPath = table.Column<string>(type: "nvarchar(700)", maxLength: 700, nullable: true),
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
                name: "PropertyCondition",
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
                    table.PrimaryKey("PK_PropertyCondition", x => x.ID);
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
                name: "PositionTitle",
                schema: "HRLooks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                    AssetTypeId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_MainAccount_AssetType_AssetTypeId",
                        column: x => x.AssetTypeId,
                        principalSchema: "Look",
                        principalTable: "AssetType",
                        principalColumn: "ID");
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
                name: "Property",
                schema: "PropertyMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    PropertyConditionID = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    PaymentStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Property_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalSchema: "Look",
                        principalTable: "Category",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Property_PropertyCondition_PropertyConditionID",
                        column: x => x.PropertyConditionID,
                        principalSchema: "Look",
                        principalTable: "PropertyCondition",
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
                name: "Assignment",
                schema: "PropertyMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EndDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Assignment_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "PropertyMS",
                        principalTable: "Property",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Maintenance",
                schema: "PropertyMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayedAmountInUSD = table.Column<float>(type: "real", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Maintenance_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "PropertyMS",
                        principalTable: "Property",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                schema: "PropertyMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<float>(type: "real", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    RemainingBalance = table.Column<float>(type: "real", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Payment_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "PropertyMS",
                        principalTable: "Property",
                        principalColumn: "ID");
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
                name: "IX_Assignment_PropertyId",
                schema: "PropertyMS",
                table: "Assignment",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Branch_ParentId",
                schema: "Look",
                table: "Branch",
                column: "ParentId");

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
                name: "IX_MainAccount_AssetTypeId",
                schema: "AssetMS",
                table: "MainAccount",
                column: "AssetTypeId");

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
                name: "IX_Maintenance_PropertyId",
                schema: "PropertyMS",
                table: "Maintenance",
                column: "PropertyId");

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
                name: "IX_Payment_PropertyId",
                schema: "PropertyMS",
                table: "Payment",
                column: "PropertyId");

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
                name: "IX_PositionTitle_BranchId",
                schema: "HRLooks",
                table: "PositionTitle",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Property_CategoryID",
                schema: "PropertyMS",
                table: "Property",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Property_PropertyConditionID",
                schema: "PropertyMS",
                table: "Property",
                column: "PropertyConditionID");

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
                name: "Assignment",
                schema: "PropertyMS");

            migrationBuilder.DropTable(
                name: "Attachments",
                schema: "General");

            migrationBuilder.DropTable(
                name: "ExpenseTracking",
                schema: "AssetMS");

            migrationBuilder.DropTable(
                name: "LoanType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "Maintenance",
                schema: "PropertyMS");

            migrationBuilder.DropTable(
                name: "NewsNotification",
                schema: "General");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "Payment",
                schema: "PropertyMS");

            migrationBuilder.DropTable(
                name: "PayrollTracking",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "TradeTracking",
                schema: "AssetMS");

            migrationBuilder.DropTable(
                name: "TrainingVideos",
                schema: "General");

            migrationBuilder.DropTable(
                name: "WithdrawalTracking",
                schema: "AssetMS");

            migrationBuilder.DropTable(
                name: "ExpenseType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "News",
                schema: "General");

            migrationBuilder.DropTable(
                name: "Property",
                schema: "PropertyMS");

            migrationBuilder.DropTable(
                name: "ContractDetails",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "PayType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "MainAccount",
                schema: "AssetMS");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "PropertyCondition",
                schema: "Look");

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
                name: "AssetType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "CurrencyType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "Branch",
                schema: "Look");
        }
    }
}
