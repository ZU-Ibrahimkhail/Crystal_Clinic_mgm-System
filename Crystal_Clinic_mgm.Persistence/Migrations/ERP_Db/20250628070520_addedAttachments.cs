using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class addedAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobDescription",
                schema: "HRLooks",
                table: "PositionTitle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                schema: "AssetMS",
                table: "ExpenseTracking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                schema: "AssetMS",
                table: "ExpenseTracking",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AttachmentPath",
                schema: "HR",
                table: "ContractDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobDescription",
                schema: "HRLooks",
                table: "PositionTitle");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                schema: "AssetMS",
                table: "ExpenseTracking");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                schema: "AssetMS",
                table: "ExpenseTracking");

            migrationBuilder.DropColumn(
                name: "AttachmentPath",
                schema: "HR",
                table: "ContractDetails");
        }
    }
}
