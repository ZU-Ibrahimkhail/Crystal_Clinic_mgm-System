using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "paymentType",
                schema: "CrystalClinic",
                table: "VisitPayment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VisitPayment_VisitId",
                schema: "CrystalClinic",
                table: "VisitPayment",
                column: "VisitId");

            migrationBuilder.AddForeignKey(
                name: "FK_VisitPayment_Visit_VisitId",
                schema: "CrystalClinic",
                table: "VisitPayment",
                column: "VisitId",
                principalSchema: "CrystalClinic",
                principalTable: "Visit",
                principalColumn: "visitId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VisitPayment_Visit_VisitId",
                schema: "CrystalClinic",
                table: "VisitPayment");

            migrationBuilder.DropIndex(
                name: "IX_VisitPayment_VisitId",
                schema: "CrystalClinic",
                table: "VisitPayment");

            migrationBuilder.DropColumn(
                name: "paymentType",
                schema: "CrystalClinic",
                table: "VisitPayment");
        }
    }
}
