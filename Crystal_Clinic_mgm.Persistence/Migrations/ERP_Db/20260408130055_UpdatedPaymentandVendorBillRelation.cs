using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class UpdatedPaymentandVendorBillRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorBillId",
                schema: "Accounting",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_VendorBillId",
                schema: "Accounting",
                table: "Payment",
                column: "VendorBillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_VendorBill_VendorBillId",
                schema: "Accounting",
                table: "Payment",
                column: "VendorBillId",
                principalSchema: "Accounting",
                principalTable: "VendorBill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_VendorBill_VendorBillId",
                schema: "Accounting",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_VendorBillId",
                schema: "Accounting",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "VendorBillId",
                schema: "Accounting",
                table: "Payment");
        }
    }
}
