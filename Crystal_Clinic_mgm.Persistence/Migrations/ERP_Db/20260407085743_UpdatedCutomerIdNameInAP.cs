using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class UpdatedCutomerIdNameInAP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsPayable_Patient_CutormerId",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.RenameColumn(
                name: "CutormerId",
                schema: "Accounting",
                table: "AccountsPayable",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountsPayable_CutormerId",
                schema: "Accounting",
                table: "AccountsPayable",
                newName: "IX_AccountsPayable_CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsPayable_Patient_CustomerId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "CustomerId",
                principalSchema: "CrystalClinic",
                principalTable: "Patient",
                principalColumn: "patientId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsPayable_Patient_CustomerId",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "Accounting",
                table: "AccountsPayable",
                newName: "CutormerId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountsPayable_CustomerId",
                schema: "Accounting",
                table: "AccountsPayable",
                newName: "IX_AccountsPayable_CutormerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsPayable_Patient_CutormerId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "CutormerId",
                principalSchema: "CrystalClinic",
                principalTable: "Patient",
                principalColumn: "patientId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
