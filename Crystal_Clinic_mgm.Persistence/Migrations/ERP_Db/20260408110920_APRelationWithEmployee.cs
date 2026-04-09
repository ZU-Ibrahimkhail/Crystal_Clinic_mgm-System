using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class APRelationWithEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsPayable_Patient_CustomerId",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "Accounting",
                table: "AccountsPayable",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountsPayable_CustomerId",
                schema: "Accounting",
                table: "AccountsPayable",
                newName: "IX_AccountsPayable_EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountsPayable_EmployeeProfile_EmployeeId",
                schema: "Accounting",
                table: "AccountsPayable",
                column: "EmployeeId",
                principalSchema: "HR",
                principalTable: "EmployeeProfile",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountsPayable_EmployeeProfile_EmployeeId",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                schema: "Accounting",
                table: "AccountsPayable",
                newName: "CustomerId");

            migrationBuilder.RenameIndex(
                name: "IX_AccountsPayable_EmployeeId",
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
    }
}
