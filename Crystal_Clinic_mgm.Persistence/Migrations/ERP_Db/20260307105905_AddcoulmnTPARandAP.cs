using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class AddcoulmnTPARandAP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attachemnt",
                schema: "Accounting",
                table: "AccountsReceivable",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyRate",
                schema: "Accounting",
                table: "AccountsReceivable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Accounting",
                table: "AccountsReceivable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Refrence",
                schema: "Accounting",
                table: "AccountsReceivable",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Attachemnt",
                schema: "Accounting",
                table: "AccountsPayable",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyRate",
                schema: "Accounting",
                table: "AccountsPayable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "Accounting",
                table: "AccountsPayable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Refrence",
                schema: "Accounting",
                table: "AccountsPayable",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attachemnt",
                schema: "Accounting",
                table: "AccountsReceivable");

            migrationBuilder.DropColumn(
                name: "CurrencyRate",
                schema: "Accounting",
                table: "AccountsReceivable");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Accounting",
                table: "AccountsReceivable");

            migrationBuilder.DropColumn(
                name: "Refrence",
                schema: "Accounting",
                table: "AccountsReceivable");

            migrationBuilder.DropColumn(
                name: "Attachemnt",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.DropColumn(
                name: "CurrencyRate",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "Accounting",
                table: "AccountsPayable");

            migrationBuilder.DropColumn(
                name: "Refrence",
                schema: "Accounting",
                table: "AccountsPayable");
        }
    }
}
