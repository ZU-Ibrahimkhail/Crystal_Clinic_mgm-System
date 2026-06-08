using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class updatedthevisitInstrumentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitInstrument_InventoryKits_KitId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.DropIndex(
                name: "UQ_VisitInstrument_VisitId_KitId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.RenameColumn(
                name: "KitId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                newName: "VisitKitsId");

            migrationBuilder.RenameIndex(
                name: "IX_VisitInstrument_KitId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                newName: "IX_VisitInstrument_VisitKitsId");

            migrationBuilder.CreateIndex(
                name: "UQ_VisitInstrument_VisitKitsId_KitId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                columns: new[] { "VisitId", "VisitKitsId" },
                unique: true,
                filter: "[VisitKitsId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitInstrument_VisitKits_VisitKitsId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                column: "VisitKitsId",
                principalSchema: "VisitKits",
                principalTable: "VisitKits",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitInstrument_VisitKits_VisitKitsId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.DropIndex(
                name: "UQ_VisitInstrument_VisitKitsId_KitId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.RenameColumn(
                name: "VisitKitsId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                newName: "KitId");

            migrationBuilder.RenameIndex(
                name: "IX_VisitInstrument_VisitKitsId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                newName: "IX_VisitInstrument_KitId");

            migrationBuilder.CreateIndex(
                name: "UQ_VisitInstrument_VisitId_KitId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                columns: new[] { "VisitId", "KitId" },
                unique: true,
                filter: "[KitId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitInstrument_InventoryKits_KitId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                column: "KitId",
                principalTable: "InventoryKits",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
