using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class UpdatedtheRelationAP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CutormerId",
                schema: "Accounting",
                table: "AccountsPayable",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseOrderId",
                schema: "Accounting",
                table: "AccountsPayable",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountsPayable_CutormerId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "CutormerId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsPayable_PurchaseOrderId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "PurchaseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsPayable_Patient_CutormerId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "CutormerId",
                principalSchema: "CrystalClinic",
                principalTable: "Patient",
                principalColumn: "patientId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsPayable_PurchaseOrder_PurchaseOrderId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "PurchaseOrderId",
                principalSchema: "Accounting",
                principalTable: "PurchaseOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsPayable_Patient_CutormerId",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountsPayable_PurchaseOrder_PurchaseOrderId",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.DropIndex(
                name: "IX_AccountsPayable_CutormerId",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.DropIndex(
                name: "IX_AccountsPayable_PurchaseOrderId",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.DropColumn(
                name: "CutormerId",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                schema: "Accounting",
                table: "AccountsPayable");
        }
    }
}
