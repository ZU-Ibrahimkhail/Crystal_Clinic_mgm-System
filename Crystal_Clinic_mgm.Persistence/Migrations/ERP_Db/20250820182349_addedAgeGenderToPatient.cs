using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class addedAgeGenderToPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "age",
                schema: "CrystalClinic",
                table: "Patient",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "gender",
                schema: "CrystalClinic",
                table: "Patient",
                type: "nvarchar(20)",
                nullable: true,
                defaultValue: "Unknown");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "age",
                schema: "CrystalClinic",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "gender",
                schema: "CrystalClinic",
                table: "Patient");
        }
    }
}
