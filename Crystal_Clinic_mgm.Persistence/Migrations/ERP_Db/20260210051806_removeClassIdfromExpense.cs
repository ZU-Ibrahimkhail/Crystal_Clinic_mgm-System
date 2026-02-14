using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class removeClassIdfromExpense : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassId",
                schema: "Accounting",
                table: "Expense");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReservations_ServiceId",
                table: "InventoryReservations",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReservations_VisitId",
                table: "InventoryReservations",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryReservations_Service_ServiceId",
                table: "InventoryReservations",
                column: "ServiceId",
                principalSchema: "Stock",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryReservations_Visit_VisitId",
                table: "InventoryReservations",
                column: "VisitId",
                principalSchema: "CrystalClinic",
                principalTable: "Visit",
                principalColumn: "visitId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryReservations_Service_ServiceId",
                table: "InventoryReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryReservations_Visit_VisitId",
                table: "InventoryReservations");

            migrationBuilder.DropIndex(
                name: "IX_InventoryReservations_ServiceId",
                table: "InventoryReservations");

            migrationBuilder.DropIndex(
                name: "IX_InventoryReservations_VisitId",
                table: "InventoryReservations");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                schema: "Accounting",
                table: "Expense",
                type: "int",
                nullable: true);
        }
    }
}
