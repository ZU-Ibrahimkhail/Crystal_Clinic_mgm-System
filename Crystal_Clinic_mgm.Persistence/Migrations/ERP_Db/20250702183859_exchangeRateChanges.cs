using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class exchangeRateChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitPayment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PaymentToAFNExchangeRate",
                schema: "CrystalClinic",
                table: "VisitPayment",
                type: "decimal(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "Stock",
                table: "Service",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyTypeId",
                schema: "Stock",
                table: "Service",
                type: "int",
                nullable: false,
                defaultValue: 2);

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

            migrationBuilder.CreateIndex(
                name: "IX_VisitPayment_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitPayment",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_CurrencyTypeId",
                schema: "Stock",
                table: "Service",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_FromCurrencyId",
                table: "CurrencyExchangeRates",
                column: "FromCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrencyExchangeRates_ToCurrencyId",
                table: "CurrencyExchangeRates",
                column: "ToCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Service_CurrencyType_CurrencyTypeId",
                schema: "Stock",
                table: "Service",
                column: "CurrencyTypeId",
                principalSchema: "Look",
                principalTable: "CurrencyType",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitPayment_CurrencyType_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitPayment",
                column: "CurrencyTypeId",
                principalSchema: "Look",
                principalTable: "CurrencyType",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Service_CurrencyType_CurrencyTypeId",
                schema: "Stock",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_VisitPayment_CurrencyType_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitPayment");

            migrationBuilder.DropTable(
                name: "CurrencyExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_VisitPayment_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitPayment");

            migrationBuilder.DropIndex(
                name: "IX_Service_CurrencyTypeId",
                schema: "Stock",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitPayment");

            migrationBuilder.DropColumn(
                name: "PaymentToAFNExchangeRate",
                schema: "CrystalClinic",
                table: "VisitPayment");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "Stock",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "CurrencyTypeId",
                schema: "Stock",
                table: "Service");
        }
    }
}
