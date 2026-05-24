using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class AddVisitInstrumentUniqueConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VisitInstrument_VisitId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.CreateIndex(
                name: "UQ_VisitInstrument_VisitId_InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                columns: new[] { "VisitId", "InventoryReservationId" },
                unique: true,
                filter: "[InventoryReservationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_VisitInstrument_VisitId_ItemId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                columns: new[] { "VisitId", "ItemId" },
                unique: true,
                filter: "[ItemId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_VisitInstrument_VisitId_KitId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                columns: new[] { "VisitId", "KitId" },
                unique: true,
                filter: "[KitId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UQ_VisitInstrument_VisitId_ServiceSessionsId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                columns: new[] { "VisitId", "ServiceSessionsId" },
                unique: true,
                filter: "[ServiceSessionsId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UQ_VisitInstrument_VisitId_InventoryReservationId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.DropIndex(
                name: "UQ_VisitInstrument_VisitId_ItemId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.DropIndex(
                name: "UQ_VisitInstrument_VisitId_KitId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.DropIndex(
                name: "UQ_VisitInstrument_VisitId_ServiceSessionsId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.CreateIndex(
                name: "IX_VisitInstrument_VisitId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                column: "VisitId");
        }
    }
}
