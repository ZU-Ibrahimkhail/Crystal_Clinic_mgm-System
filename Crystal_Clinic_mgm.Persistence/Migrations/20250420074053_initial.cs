using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Application",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbrevation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DefaultRoute = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Application", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailHistory",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailHistory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                schema: "dbo",
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
                    EnglishName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PashtoName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NotificationMessage",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnglishTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    DariTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    PashtoTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    EnglishMessage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    DariMessage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    PashtoMessage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    ApplicationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    EnglishDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, defaultValue: ""),
                    DariDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, defaultValue: ""),
                    PashtoDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, defaultValue: ""),
                    ControllerLink = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: ""),
                    ActionLink = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: ""),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationMessage", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Expires = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Created = table.Column<DateTime>(type: "DateTime", nullable: false),
                    CreatedByIp = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Revoked = table.Column<DateTime>(type: "DateTime", nullable: true),
                    RevokedByIp = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsExpired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SuccessLoginCount = table.Column<int>(type: "int", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsSuperAdmin = table.Column<bool>(type: "bit", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    FailedLoginCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserAllowedDocTypesSecurityLevels",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 200, nullable: false),
                    AllowedBranchId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAllowedDocTypesSecurityLevels", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Controller = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    Action = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Method = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ActionCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    IsGlobal = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    RoleDescription = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackingId = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    TrackingNumber = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true, defaultValue: ""),
                    FromUserId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    ToUserId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    NotificationMessageId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Notification_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Notification_NotificationMessage_NotificationMessageId",
                        column: x => x.NotificationMessageId,
                        principalSchema: "dbo",
                        principalTable: "NotificationMessage",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Notification_User_FromUserId",
                        column: x => x.FromUserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Notification_User_ToUserId",
                        column: x => x.ToUserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UserAudit",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    UserId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ActionOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ActionEnd = table.Column<DateTime>(type: "DateTime", nullable: true),
                    IpAddress = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DeviceName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Result = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    BrowserName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    BrowserVersion = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Os = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DeviceType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAudit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAudit_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => new { x.PermissionId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "dbo",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "dbo",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.RoleID, x.UserID });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleID",
                        column: x => x.RoleID,
                        principalSchema: "dbo",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserID",
                        column: x => x.UserID,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackingTable",
                schema: "dbo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAuditId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<Guid>(type: "UNIQUEIDENTIFIER", nullable: false),
                    ApplicationId = table.Column<int>(type: "int", nullable: true),
                    PermissionId = table.Column<int>(type: "int", nullable: true),
                    IsAccessed = table.Column<bool>(type: "bit", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ErrorDetails = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    ActionName = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false),
                    Url = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackingTable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TrackingTable_Application_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Application",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_TrackingTable_UserAudit_UserAuditId",
                        column: x => x.UserAuditId,
                        principalSchema: "dbo",
                        principalTable: "UserAudit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrackingTable_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ApplicationId",
                schema: "dbo",
                table: "Notification",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_FromUserId",
                schema: "dbo",
                table: "Notification",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationMessageId",
                schema: "dbo",
                table: "Notification",
                column: "NotificationMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ToUserId",
                schema: "dbo",
                table: "Notification",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_ApplicationId",
                schema: "dbo",
                table: "Permission",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_ApplicationId",
                schema: "dbo",
                table: "Role",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingTable_ApplicationId",
                schema: "dbo",
                table: "TrackingTable",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingTable_UserAuditId",
                schema: "dbo",
                table: "TrackingTable",
                column: "UserAuditId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackingTable_UserId",
                schema: "dbo",
                table: "TrackingTable",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "User",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserAudit_UserId",
                schema: "dbo",
                table: "UserAudit",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserID",
                schema: "dbo",
                table: "UserRole",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EmailHistory",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Language",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "TrackingTable",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserAllowedDocTypesSecurityLevels",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "NotificationMessage",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserAudit",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "User",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Application");
        }
    }
}
