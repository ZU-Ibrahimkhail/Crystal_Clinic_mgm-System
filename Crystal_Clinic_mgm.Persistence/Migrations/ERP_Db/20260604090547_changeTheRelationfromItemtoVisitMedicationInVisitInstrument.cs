using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class changeTheRelationfromItemtoVisitMedicationInVisitInstrument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitInstrument_Item_ItemId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.DropIndex(
                name: "UQ_VisitInstrument_VisitId_ItemId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                newName: "VisitMedicationId");

            migrationBuilder.RenameIndex(
                name: "IX_VisitInstrument_ItemId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                newName: "IX_VisitInstrument_VisitMedicationId");

            migrationBuilder.CreateIndex(
                name: "UQ_VisitInstrument_VisitId_VisitMedicationId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                columns: new[] { "VisitId", "VisitMedicationId" },
                unique: true,
                filter: "[VisitMedicationId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitInstrument_VisitMedication_VisitMedicationId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                column: "VisitMedicationId",
                principalSchema: "CrystalClinic",
                principalTable: "VisitMedication",
                principalColumn: "medicationId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitInstrument_VisitMedication_VisitMedicationId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.DropIndex(
                name: "UQ_VisitInstrument_VisitId_VisitMedicationId",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.RenameColumn(
                name: "VisitMedicationId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_VisitInstrument_VisitMedicationId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                newName: "IX_VisitInstrument_ItemId");

            migrationBuilder.CreateIndex(
                name: "UQ_VisitInstrument_VisitId_ItemId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                columns: new[] { "VisitId", "ItemId" },
                unique: true,
                filter: "[ItemId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitInstrument_Item_ItemId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                column: "ItemId",
                principalSchema: "Stock",
                principalTable: "Item",
                principalColumn: "ItemId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
