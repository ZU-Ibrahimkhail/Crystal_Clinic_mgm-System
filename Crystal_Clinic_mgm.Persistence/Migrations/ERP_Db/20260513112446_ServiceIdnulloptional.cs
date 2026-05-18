using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class ServiceIdnulloptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryReservations_Service_ServiceId",
                table: "InventoryReservations");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "InventoryReservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryReservations_Service_ServiceId",
                table: "InventoryReservations",
                column: "ServiceId",
                principalSchema: "Stock",
                principalTable: "Service",
                principalColumn: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryReservations_Service_ServiceId",
                table: "InventoryReservations");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "InventoryReservations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryReservations_Service_ServiceId",
                table: "InventoryReservations",
                column: "ServiceId",
                principalSchema: "Stock",
                principalTable: "Service",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
