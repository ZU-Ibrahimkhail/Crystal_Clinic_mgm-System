using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class AddedRelationInventoryReservationandVisitInstrument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_VisitInstrument_InventoryReservations_InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                column: "InventoryReservationId",
                principalTable: "InventoryReservations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitInstrument_InventoryReservations_InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.DropIndex(
                name: "IX_VisitInstrument_InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.DropColumn(
                name: "InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument");
        }
    }
}
