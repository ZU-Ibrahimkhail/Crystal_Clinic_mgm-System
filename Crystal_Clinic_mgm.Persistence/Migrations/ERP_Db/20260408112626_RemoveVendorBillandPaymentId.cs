using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class RemoveVendorBillandPaymentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorBill_Payment_paymentId",
                schema: "Accounting",
                table: "VendorBill");

            migrationBuilder.DropIndex(
                name: "IX_VendorBill_paymentId",
                schema: "Accounting",
                table: "VendorBill");

            migrationBuilder.DropColumn(
                name: "paymentId",
                schema: "Accounting",
                table: "VendorBill");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "paymentId",
                schema: "Accounting",
                table: "VendorBill",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorBill_paymentId",
                schema: "Accounting",
                table: "VendorBill",
                column: "paymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorBill_Payment_paymentId",
                schema: "Accounting",
                table: "VendorBill",
                column: "paymentId",
                principalSchema: "Accounting",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
