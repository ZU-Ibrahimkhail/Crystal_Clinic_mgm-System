using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class AddedPropertyForKitandReservationofVisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConsumed",
                schema: "VisitKits",
                table: "VisitKits",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCommited",
                schema: "BranchStock",
                table: "ReservedItem",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConsumed",
                schema: "VisitKits",
                table: "VisitKits");

            migrationBuilder.DropColumn(
                name: "IsCommited",
                schema: "BranchStock",
                table: "ReservedItem");
        }
    }
}
