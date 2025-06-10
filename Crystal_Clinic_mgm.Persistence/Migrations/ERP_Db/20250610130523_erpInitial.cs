using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class erpInitial : Migration
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
                name: "General");

            migrationBuilder.EnsureSchema(
                name: "HRLooks");

            migrationBuilder.EnsureSchema(
                name: "CrystalClinic");

            migrationBuilder.EnsureSchema(
                name: "Stock");

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
                name: "Patient",
                schema: "CrystalClinic",
                columns: table => new
                {
                    patientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: true),
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
                name: "Service",
                schema: "Stock",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(255)", nullable: true),
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
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ItemCategorycategoryId = table.Column<int>(type: "int", nullable: true),
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
                        name: "FK_Item_ItemCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Stock",
                        principalTable: "ItemCategory",
                        principalColumn: "categoryId");
                    table.ForeignKey(
                        name: "FK_Item_ItemCategory_ItemCategorycategoryId",
                        column: x => x.ItemCategorycategoryId,
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
                name: "Visit",
                schema: "CrystalClinic",
                columns: table => new
                {
                    visitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    patientId = table.Column<int>(type: "int", nullable: false),
                    doctorId = table.Column<int>(type: "int", nullable: true),
                    VisitDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
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
                name: "VisitPayment",
                schema: "CrystalClinic",
                columns: table => new
                {
                    VisitPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    SessionNumber = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "DateTime", nullable: false),
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
                        name: "FK_VisitPayment_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalSchema: "Stock",
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.SetNull);
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
                name: "ItemUnits",
                columns: table => new
                {
                    unitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConversionFactor = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    DailyRentalPrice = table.Column<int>(type: "int", nullable: true),
                    SellingPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemUnits", x => x.unitId);
                    table.ForeignKey(
                        name: "FK_ItemUnits_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Stock",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
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
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    BatchNumber = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    BarCode = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime", nullable: false),
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
                        name: "FK_Stock_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Stock",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockMovements",
                columns: table => new
                {
                    StockMovementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovementType = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    SourceBranchId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovements", x => x.StockMovementId);
                    table.ForeignKey(
                        name: "FK_StockMovements_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Stock",
                        principalTable: "Item",
                        principalColumn: "ItemId",
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
                    pricePerSession = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    totalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    paidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    remainAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    nextSessionDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    paymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sessionStatus = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    discount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitServices", x => x.visitServiceId);
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
                name: "ItemCleaningJob",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    unitId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    dateToBeRestocked = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCleaningJob", x => x.id);
                    table.ForeignKey(
                        name: "FK_ItemCleaningJob_ItemUnits_unitId",
                        column: x => x.unitId,
                        principalTable: "ItemUnits",
                        principalColumn: "unitId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemCleaningJob_Item_itemId",
                        column: x => x.itemId,
                        principalSchema: "Stock",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_Item_CategoryId",
                schema: "Stock",
                table: "Item",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_ItemCategorycategoryId",
                schema: "Stock",
                table: "Item",
                column: "ItemCategorycategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_Name",
                schema: "Stock",
                table: "Item",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCleaningJob_itemId",
                table: "ItemCleaningJob",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCleaningJob_unitId",
                table: "ItemCleaningJob",
                column: "unitId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnits_ItemId",
                table: "ItemUnits",
                column: "ItemId");

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
                name: "IX_Service_Name",
                schema: "Stock",
                table: "Service",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_ItemId",
                schema: "Stock",
                table: "Stock",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ItemId",
                table: "StockMovements",
                column: "ItemId");

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
                name: "IX_VisitPayment_ServiceId",
                schema: "CrystalClinic",
                table: "VisitPayment",
                column: "ServiceId");

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
                name: "BranchDetails",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "ExpenseTracking",
                schema: "AssetMS");

            migrationBuilder.DropTable(
                name: "ItemCleaningJob");

            migrationBuilder.DropTable(
                name: "LoanType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "NewsNotification",
                schema: "General");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropTable(
                name: "PayrollTracking",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "StockMovements");

            migrationBuilder.DropTable(
                name: "TradeTracking",
                schema: "AssetMS");

            migrationBuilder.DropTable(
                name: "TrainingVideos",
                schema: "General");

            migrationBuilder.DropTable(
                name: "VisitMedication",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "VisitPayment",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "VisitServices",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "WithdrawalTracking",
                schema: "AssetMS");

            migrationBuilder.DropTable(
                name: "ExpenseType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "ItemUnits");

            migrationBuilder.DropTable(
                name: "News",
                schema: "General");

            migrationBuilder.DropTable(
                name: "ContractDetails",
                schema: "HR");

            migrationBuilder.DropTable(
                name: "PayType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "Stock",
                schema: "Stock");

            migrationBuilder.DropTable(
                name: "Service",
                schema: "Stock");

            migrationBuilder.DropTable(
                name: "Visit",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "MainAccount",
                schema: "AssetMS");

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
                name: "Item",
                schema: "Stock");

            migrationBuilder.DropTable(
                name: "Doctor",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "Patient",
                schema: "CrystalClinic");

            migrationBuilder.DropTable(
                name: "CurrencyType",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "Branch",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "ItemCategory",
                schema: "Stock");
        }
    }
}
