using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class UpdateStockEntityConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "stockId",
                schema: "Stock",
                table: "Stock",
                newName: "StockId");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                schema: "Stock",
                table: "Stock",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                schema: "Stock",
                table: "Stock",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_BranchId",
                schema: "Stock",
                table: "Stock",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_PurchaseOrderId",
                schema: "Stock",
                table: "Stock",
                column: "PurchaseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_Branch_BranchId",
                schema: "Stock",
                table: "Stock",
                column: "BranchId",
                principalSchema: "Look",
                principalTable: "Branch",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_PurchaseOrder_PurchaseOrderId",
                schema: "Stock",
                table: "Stock",
                column: "PurchaseOrderId",
                principalSchema: "Accounting",
                principalTable: "PurchaseOrder",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stock_Branch_BranchId",
                schema: "Stock",
                table: "Stock");

            migrationBuilder.DropForeignKey(
                name: "FK_Stock_PurchaseOrder_PurchaseOrderId",
                schema: "Stock",
                table: "Stock");

            migrationBuilder.DropIndex(
                name: "IX_Stock_BranchId",
                schema: "Stock",
                table: "Stock");

            migrationBuilder.DropIndex(
                name: "IX_Stock_PurchaseOrderId",
                schema: "Stock",
                table: "Stock");

            migrationBuilder.RenameColumn(
                name: "StockId",
                schema: "Stock",
                table: "Stock",
                newName: "stockId");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                schema: "Stock",
                table: "Stock",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BranchId",
                schema: "Stock",
                table: "Stock",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
