using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class FeeServicesEmplyeeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FeeAmount",
                schema: "CrystalClinic",
                table: "Visit",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "employeeId",
                schema: "CrystalClinic",
                table: "Doctor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "services",
                schema: "CrystalClinic",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FeeAmount",
                schema: "CrystalClinic",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "employeeId",
                schema: "CrystalClinic",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "services",
                schema: "CrystalClinic",
                table: "Doctor");
        }
    }
}
