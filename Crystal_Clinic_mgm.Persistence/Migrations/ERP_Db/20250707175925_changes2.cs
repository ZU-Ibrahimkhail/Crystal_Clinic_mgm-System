using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class changes2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contactInfo",
                table: "ServiceSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "patientName",
                table: "ServiceSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "serviceId",
                table: "ServiceSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "serviceName",
                table: "ServiceSessions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "visitId",
                table: "ServiceSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contactInfo",
                table: "ServiceSessions");

            migrationBuilder.DropColumn(
                name: "patientName",
                table: "ServiceSessions");

            migrationBuilder.DropColumn(
                name: "serviceId",
                table: "ServiceSessions");

            migrationBuilder.DropColumn(
                name: "serviceName",
                table: "ServiceSessions");

            migrationBuilder.DropColumn(
                name: "visitId",
                table: "ServiceSessions");
        }
    }
}
