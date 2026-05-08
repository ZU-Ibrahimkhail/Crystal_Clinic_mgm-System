using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class UpdateVisitInstrumentEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalCost",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "IsInvoiceGenerated",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                newName: "IsFreeForPatient");

            migrationBuilder.AddColumn<int>(
                name: "Count",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                schema: "CrystalClinic",
                table: "VisitInstrument");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                newName: "TotalCost");

            migrationBuilder.RenameColumn(
                name: "IsFreeForPatient",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                newName: "IsInvoiceGenerated");
        }
    }
}
