using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class addedExtraColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DamagedQuantity",
                schema: "Sales",
                table: "OrderItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ReturnedQuantity",
                schema: "Sales",
                table: "OrderItem",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RealTimeAvailableStock",
                schema: "Inventory",
                table: "Item",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BranchId",
                schema: "Sales",
                table: "Orders",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Branch_BranchId",
                schema: "Sales",
                table: "Orders",
                column: "BranchId",
                principalSchema: "Look",
                principalTable: "Branch",
                principalColumn: "ID",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branch_BranchId",
                schema: "Sales",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BranchId",
                schema: "Sales",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DamagedQuantity",
                schema: "Sales",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "ReturnedQuantity",
                schema: "Sales",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "RealTimeAvailableStock",
                schema: "Inventory",
                table: "Item");
        }
    }
}
