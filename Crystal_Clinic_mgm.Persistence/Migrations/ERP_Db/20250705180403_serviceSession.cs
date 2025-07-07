using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class serviceSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "paidAmount",
                schema: "CrystalClinic",
                table: "VisitServices");

            migrationBuilder.DropColumn(
                name: "remainAmount",
                schema: "CrystalClinic",
                table: "VisitServices");

            migrationBuilder.AddColumn<int>(
                name: "PaidSessions",
                schema: "CrystalClinic",
                table: "VisitServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ServiceSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    visitServiceId = table.Column<int>(type: "int", nullable: false),
                    sessionNumber = table.Column<int>(type: "int", nullable: false),
                    PriceInAFN = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsImplemented = table.Column<bool>(type: "bit", nullable: false),
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
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSessions_ImplementorEmployeeId",
                table: "ServiceSessions",
                column: "ImplementorEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceSessions");

            migrationBuilder.DropColumn(
                name: "PaidSessions",
                schema: "CrystalClinic",
                table: "VisitServices");

            migrationBuilder.AddColumn<decimal>(
                name: "paidAmount",
                schema: "CrystalClinic",
                table: "VisitServices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "remainAmount",
                schema: "CrystalClinic",
                table: "VisitServices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
