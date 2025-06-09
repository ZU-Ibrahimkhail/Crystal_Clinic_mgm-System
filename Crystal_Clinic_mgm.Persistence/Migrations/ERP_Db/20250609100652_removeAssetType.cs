using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class removeAssetType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MainAccount_AssetType_AssetTypeId",
                schema: "AssetMS",
                table: "MainAccount");

            migrationBuilder.DropIndex(
                name: "IX_MainAccount_AssetTypeId",
                schema: "AssetMS",
                table: "MainAccount");

            migrationBuilder.DropColumn(
                name: "AssetTypeId",
                schema: "AssetMS",
                table: "MainAccount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssetTypeId",
                schema: "AssetMS",
                table: "MainAccount",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MainAccount_AssetTypeId",
                schema: "AssetMS",
                table: "MainAccount",
                column: "AssetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MainAccount_AssetType_AssetTypeId",
                schema: "AssetMS",
                table: "MainAccount",
                column: "AssetTypeId",
                principalSchema: "Look",
                principalTable: "AssetType",
                principalColumn: "ID");
        }
    }
}
