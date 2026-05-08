using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class newSmallFieldsInSalesInvoiceAndItsLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KitId",
                schema: "Accounting",
                table: "SalesInvoiceLine",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VisitId",
                schema: "Accounting",
                table: "SalesInvoice",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalesInvoice_VisitId",
                schema: "Accounting",
                table: "SalesInvoice",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesInvoice_Visit_VisitId",
                schema: "Accounting",
                table: "SalesInvoice",
                column: "VisitId",
                principalSchema: "CrystalClinic",
                principalTable: "Visit",
                principalColumn: "visitId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesInvoice_Visit_VisitId",
                schema: "Accounting",
                table: "SalesInvoice");

            migrationBuilder.DropIndex(
                name: "IX_SalesInvoice_VisitId",
                schema: "Accounting",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "KitId",
                schema: "Accounting",
                table: "SalesInvoiceLine");

            migrationBuilder.DropColumn(
                name: "VisitId",
                schema: "Accounting",
                table: "SalesInvoice");
        }
    }
}
