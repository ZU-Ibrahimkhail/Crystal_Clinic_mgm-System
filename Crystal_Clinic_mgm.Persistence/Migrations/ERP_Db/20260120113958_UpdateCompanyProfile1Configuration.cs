using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class UpdateCompanyProfile1Configuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_AccountsPayableAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_AccountsReceivableAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_BankAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_CashAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_InventoryAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_PurchaseExpenseAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_SalesRevenueAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_CurrencyType_CurrencyTypeID",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropIndex(
                name: "IX_CompanyProfile_CurrencyTypeID",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropColumn(
                name: "CurrencyTypeID",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "DateTime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "UNIQUEIDENTIFIER",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsInitialized",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "DateTime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "UNIQUEIDENTIFIER",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_BaseCurrencyId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "BaseCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_AccountsPayableAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "AccountsPayableAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_AccountsReceivableAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "AccountsReceivableAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_BankAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "BankAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_CashAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "CashAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_InventoryAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "InventoryAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_PurchaseExpenseAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "PurchaseExpenseAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_SalesRevenueAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "SalesRevenueAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_CurrencyType_BaseCurrencyId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "BaseCurrencyId",
                principalSchema: "Look",
                principalTable: "CurrencyType",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_AccountsPayableAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_AccountsReceivableAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_BankAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_CashAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_InventoryAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_PurchaseExpenseAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_SalesRevenueAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_CurrencyType_BaseCurrencyId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropIndex(
                name: "IX_CompanyProfile_BaseCurrencyId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedOn",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DateTime",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ModifiedBy",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "UNIQUEIDENTIFIER",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsInitialized",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DateTime");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "UNIQUEIDENTIFIER",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyTypeID",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_CurrencyTypeID",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "CurrencyTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_AccountsPayableAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "AccountsPayableAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_AccountsReceivableAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "AccountsReceivableAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_BankAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "BankAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_CashAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "CashAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_InventoryAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "InventoryAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_PurchaseExpenseAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "PurchaseExpenseAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_SalesRevenueAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "SalesRevenueAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_CurrencyType_CurrencyTypeID",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "CurrencyTypeID",
                principalSchema: "Look",
                principalTable: "CurrencyType",
                principalColumn: "ID");
        }
    }
}
