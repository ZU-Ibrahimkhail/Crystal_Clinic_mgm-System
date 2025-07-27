using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class stockSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                schema: "Stock",
                table: "Stock",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stock_SupplierId",
                schema: "Stock",
                table: "Stock",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_Supplier_SupplierId",
                schema: "Stock",
                table: "Stock",
                column: "SupplierId",
                principalSchema: "BranchStock",
                principalTable: "Supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stock_Supplier_SupplierId",
                schema: "Stock",
                table: "Stock");

            migrationBuilder.DropIndex(
                name: "IX_Stock_SupplierId",
                schema: "Stock",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                schema: "Stock",
                table: "Stock");
        }
    }
}
