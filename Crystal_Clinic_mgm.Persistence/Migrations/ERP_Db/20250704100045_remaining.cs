using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class remaining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitServices_CurrencyType_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices",
                type: "int",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitServices_CurrencyType_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices",
                column: "CurrencyTypeId",
                principalSchema: "Look",
                principalTable: "CurrencyType",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitServices_CurrencyType_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 2);

            migrationBuilder.AddForeignKey(
                name: "FK_VisitServices_CurrencyType_CurrencyTypeId",
                schema: "CrystalClinic",
                table: "VisitServices",
                column: "CurrencyTypeId",
                principalSchema: "Look",
                principalTable: "CurrencyType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
