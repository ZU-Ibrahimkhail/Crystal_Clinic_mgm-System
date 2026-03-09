using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class AddedAttachmentToFinanceEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attachemnt",
                schema: "Accounting",
                table: "Shareholder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Attachemnt",
                schema: "Accounting",
                table: "SalesInvoice",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "Accounting",
                table: "PurchaseOrder",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Attachemnt",
                schema: "Accounting",
                table: "PurchaseOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Attachemnt",
                schema: "Accounting",
                table: "JournalEntry",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Attachemnt",
                schema: "Accounting",
                table: "AccountsReceivable",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "Attachemnt",
                schema: "Accounting",
                table: "AccountsPayable",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachemnt",
                schema: "Accounting",
                table: "Shareholder");

            migrationBuilder.DropColumn(
                name: "Attachemnt",
                schema: "Accounting",
                table: "SalesInvoice");

            migrationBuilder.DropColumn(
                name: "Attachemnt",
                schema: "Accounting",
                table: "PurchaseOrder");

            migrationBuilder.DropColumn(
                name: "Attachemnt",
                schema: "Accounting",
                table: "JournalEntry");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "Accounting",
                table: "PurchaseOrder",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Attachemnt",
                schema: "Accounting",
                table: "AccountsReceivable",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Attachemnt",
                schema: "Accounting",
                table: "AccountsPayable",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
