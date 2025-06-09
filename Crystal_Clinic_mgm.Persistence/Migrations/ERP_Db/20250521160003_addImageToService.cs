using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class addImageToService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCleaningJob_ItemUnit_unitId",
                table: "ItemCleaningJob");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                schema: "Services",
                table: "Service",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "unitId",
                table: "ItemCleaningJob",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCleaningJob_ItemUnit_unitId",
                table: "ItemCleaningJob",
                column: "unitId",
                principalSchema: "Inventory",
                principalTable: "ItemUnit",
                principalColumn: "unitId",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemCleaningJob_ItemUnit_unitId",
                table: "ItemCleaningJob");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                schema: "Services",
                table: "Service");

            migrationBuilder.AlterColumn<int>(
                name: "unitId",
                table: "ItemCleaningJob",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemCleaningJob_ItemUnit_unitId",
                table: "ItemCleaningJob",
                column: "unitId",
                principalSchema: "Inventory",
                principalTable: "ItemUnit",
                principalColumn: "unitId");
        }
    }
}
