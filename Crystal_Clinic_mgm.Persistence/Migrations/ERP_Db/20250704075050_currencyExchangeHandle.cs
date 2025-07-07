using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class currencyExchangeHandle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentToAFNExchangeRate",
                schema: "CrystalClinic",
                table: "VisitPayment",
                newName: "ExchangeRateToAFN");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountInAFN",
                schema: "CrystalClinic",
                table: "VisitPayment",
                type: "decimal(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRateToServiceCurrency",
                schema: "CrystalClinic",
                table: "VisitPayment",
                type: "decimal(18,0)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitServices_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices",
                column: "CurrencyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitServices_CurrencyType_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices",
                column: "CurrencyTypeId",
                principalSchema: "Look",
                principalTable: "CurrencyType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitServices_CurrencyType_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices");

            migrationBuilder.DropIndex(
                name: "IX_VisitServices_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices");

            migrationBuilder.DropColumn(
                name: "CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices");

            migrationBuilder.DropColumn(
                name: "AmountInAFN",
                schema: "CrystalClinic",
                table: "VisitPayment");

            migrationBuilder.DropColumn(
                name: "ExchangeRateToServiceCurrency",
                schema: "CrystalClinic",
                table: "VisitPayment");

            migrationBuilder.RenameColumn(
                name: "ExchangeRateToAFN",
                schema: "CrystalClinic",
                table: "VisitPayment",
                newName: "PaymentToAFNExchangeRate");
        }
    }
}
