using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class changes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeRateToServiceCurrency",
                schema: "CrystalClinic",
                table: "VisitPayment");

            migrationBuilder.AddColumn<decimal>(
                name: "RefundAmountInAFN",
                schema: "CrystalClinic",
                table: "VisitPayment",
                type: "decimal(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSessions_visitServiceId",
                table: "ServiceSessions",
                column: "visitServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceSessions_VisitServices_visitServiceId",
                table: "ServiceSessions",
                column: "visitServiceId",
                principalSchema: "CrystalClinic",
                principalTable: "VisitServices",
                principalColumn: "visitServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceSessions_VisitServices_visitServiceId",
                table: "ServiceSessions");

            migrationBuilder.DropIndex(
                name: "IX_ServiceSessions_visitServiceId",
                table: "ServiceSessions");

            migrationBuilder.DropColumn(
                name: "RefundAmountInAFN",
                schema: "CrystalClinic",
                table: "VisitPayment");

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRateToServiceCurrency",
                schema: "CrystalClinic",
                table: "VisitPayment",
                type: "decimal(18,0)",
                nullable: true);
        }
    }
}
