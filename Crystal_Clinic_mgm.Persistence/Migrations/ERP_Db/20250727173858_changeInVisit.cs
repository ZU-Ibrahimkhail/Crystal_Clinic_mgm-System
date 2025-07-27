using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class changeInVisit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchDetailsId",
                schema: "CrystalClinic",
                table: "Visit",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "CrystalClinic",
                table: "Visit",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Visit_BranchDetailsId",
                schema: "CrystalClinic",
                table: "Visit",
                column: "BranchDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Visit_BranchDetails_BranchDetailsId",
                schema: "CrystalClinic",
                table: "Visit",
                column: "BranchDetailsId",
                principalSchema: "Look",
                principalTable: "BranchDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Visit_BranchDetails_BranchDetailsId",
                schema: "CrystalClinic",
                table: "Visit");

            migrationBuilder.DropIndex(
                name: "IX_Visit_BranchDetailsId",
                schema: "CrystalClinic",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "BranchDetailsId",
                schema: "CrystalClinic",
                table: "Visit");

            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "CrystalClinic",
                table: "Visit");
        }
    }
}
