using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class ModifyAPEnttiyAddedRelationToVendorBill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VendorBillId",
                schema: "Accounting",
                table: "AccountsPayable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AccountsPayable_VendorBillId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "VendorBillId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsPayable_VendorBill_VendorBillId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "VendorBillId",
                principalSchema: "Accounting",
                principalTable: "VendorBill",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsPayable_VendorBill_VendorBillId",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.DropIndex(
                name: "IX_AccountsPayable_VendorBillId",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.DropColumn(
                name: "VendorBillId",
                schema: "Accounting",
                table: "AccountsPayable");
        }
    }
}
