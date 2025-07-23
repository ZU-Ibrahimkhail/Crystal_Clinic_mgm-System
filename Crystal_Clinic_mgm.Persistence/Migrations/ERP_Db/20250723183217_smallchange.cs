    using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class smallchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DuePayment_SupplierDue_SupplierDueId",
                schema: "BranchStock",
                table: "DuePayment");

            migrationBuilder.RenameColumn(
                name: "AmmountPaid",
                schema: "BranchStock",
                table: "DuePayment",
                newName: "AmountPaid");

            migrationBuilder.AddColumn<int>(
                name: "SupplierDueId1",
                schema: "BranchStock",
                table: "DuePayment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DuePayment_SupplierDueId1",
                schema: "BranchStock",
                table: "DuePayment",
                column: "SupplierDueId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DuePayment_SupplierDue_SupplierDueId",
                schema: "BranchStock",
                table: "DuePayment",
                column: "SupplierDueId",
                principalSchema: "BranchStock",
                principalTable: "SupplierDue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DuePayment_SupplierDue_SupplierDueId1",
                schema: "BranchStock",
                table: "DuePayment",
                column: "SupplierDueId1",
                principalSchema: "BranchStock",
                principalTable: "SupplierDue",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DuePayment_SupplierDue_SupplierDueId",
                schema: "BranchStock",
                table: "DuePayment");

            migrationBuilder.DropForeignKey(
                name: "FK_DuePayment_SupplierDue_SupplierDueId1",
                schema: "BranchStock",
                table: "DuePayment");

            migrationBuilder.DropIndex(
                name: "IX_DuePayment_SupplierDueId1",
                schema: "BranchStock",
                table: "DuePayment");

            migrationBuilder.DropColumn(
                name: "SupplierDueId1",
                schema: "BranchStock",
                table: "DuePayment");

            migrationBuilder.RenameColumn(
                name: "AmountPaid",
                schema: "BranchStock",
                table: "DuePayment",
                newName: "AmmountPaid");

            migrationBuilder.AddForeignKey(
                name: "FK_DuePayment_SupplierDue_SupplierDueId",
                schema: "BranchStock",
                table: "DuePayment",
                column: "SupplierDueId",
                principalSchema: "BranchStock",
                principalTable: "SupplierDue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
