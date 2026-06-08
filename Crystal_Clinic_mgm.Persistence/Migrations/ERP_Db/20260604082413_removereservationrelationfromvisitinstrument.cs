using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class removereservationrelationfromvisitinstrument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitInstrument_InventoryReservations_InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.DropIndex(
                name: "IX_VisitInstrument_InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.DropIndex(
                name: "UQ_VisitInstrument_VisitId_InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.DropColumn(
                name: "InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitInstrument_InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                column: "InventoryReservationId");

            migrationBuilder.CreateIndex(
                name: "UQ_VisitInstrument_VisitId_InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                columns: new[] { "VisitId", "InventoryReservationId" },
                unique: true,
                filter: "[InventoryReservationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitInstrument_InventoryReservations_InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                column: "InventoryReservationId",
                principalTable: "InventoryReservations",
                principalColumn: "Id");
        }
    }
}
