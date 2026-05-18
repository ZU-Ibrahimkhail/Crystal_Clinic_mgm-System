using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class visitKitsServiceSessionIdnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VisitKits_VisitId_ServiceSessionId_KitId",
                schema: "VisitKits",
                table: "VisitKits");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceSessionId",
                schema: "VisitKits",
                table: "VisitKits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_VisitKits_VisitId_ServiceSessionId_KitId",
                schema: "VisitKits",
                table: "VisitKits",
                columns: new[] { "VisitId", "ServiceSessionId", "KitId" },
                unique: true,
                filter: "[ServiceSessionId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VisitKits_VisitId_ServiceSessionId_KitId",
                schema: "VisitKits",
                table: "VisitKits");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceSessionId",
                schema: "VisitKits",
                table: "VisitKits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitKits_VisitId_ServiceSessionId_KitId",
                schema: "VisitKits",
                table: "VisitKits",
                columns: new[] { "VisitId", "ServiceSessionId", "KitId" },
                unique: true);
        }
    }
}
