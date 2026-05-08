using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class newSmallFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BarCode",
                schema: "Accounting",
                table: "POLine",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpectedSalePrice",
                schema: "Accounting",
                table: "POLine",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ItemExpiry",
                schema: "Accounting",
                table: "POLine",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EquityTransactionId",
                schema: "Accounting",
                table: "JournalEntryLine",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpenseId",
                schema: "Accounting",
                table: "JournalEntryLine",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                schema: "Accounting",
                table: "JournalEntryLine",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceiptId",
                schema: "Accounting",
                table: "JournalEntryLine",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesReceiptId",
                schema: "Accounting",
                table: "JournalEntryLine",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EquityTransactionId",
                schema: "Accounting",
                table: "JournalEntry",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpenseId",
                schema: "Accounting",
                table: "JournalEntry",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                schema: "Accounting",
                table: "JournalEntry",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReceiptId",
                schema: "Accounting",
                table: "JournalEntry",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalesReceiptId",
                schema: "Accounting",
                table: "JournalEntry",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFixedAsset",
                schema: "Stock",
                table: "Item",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "AccumulatedDepreciationAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepreciationExpenseAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EquityAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FixedAssetAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_AccumulatedDepreciationAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "AccumulatedDepreciationAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_DepreciationExpenseAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "DepreciationExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_EquityAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "EquityAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_FixedAssetAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "FixedAssetAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_AccumulatedDepreciationAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "AccumulatedDepreciationAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_DepreciationExpenseAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "DepreciationExpenseAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_EquityAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "EquityAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_FixedAssetAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "FixedAssetAccountId",
                principalSchema: "Accounting",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_AccumulatedDepreciationAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_DepreciationExpenseAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_EquityAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyProfile_ChartOfAccounts_FixedAssetAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropIndex(
                name: "IX_CompanyProfile_AccumulatedDepreciationAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropIndex(
                name: "IX_CompanyProfile_DepreciationExpenseAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropIndex(
                name: "IX_CompanyProfile_EquityAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropIndex(
                name: "IX_CompanyProfile_FixedAssetAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropColumn(
                name: "BarCode",
                schema: "Accounting",
                table: "POLine");

            migrationBuilder.DropColumn(
                name: "ExpectedSalePrice",
                schema: "Accounting",
                table: "POLine");

            migrationBuilder.DropColumn(
                name: "ItemExpiry",
                schema: "Accounting",
                table: "POLine");

            migrationBuilder.DropColumn(
                name: "EquityTransactionId",
                schema: "Accounting",
                table: "JournalEntryLine");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                schema: "Accounting",
                table: "JournalEntryLine");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                schema: "Accounting",
                table: "JournalEntryLine");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                schema: "Accounting",
                table: "JournalEntryLine");

            migrationBuilder.DropColumn(
                name: "SalesReceiptId",
                schema: "Accounting",
                table: "JournalEntryLine");

            migrationBuilder.DropColumn(
                name: "EquityTransactionId",
                schema: "Accounting",
                table: "JournalEntry");

            migrationBuilder.DropColumn(
                name: "ExpenseId",
                schema: "Accounting",
                table: "JournalEntry");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                schema: "Accounting",
                table: "JournalEntry");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                schema: "Accounting",
                table: "JournalEntry");

            migrationBuilder.DropColumn(
                name: "SalesReceiptId",
                schema: "Accounting",
                table: "JournalEntry");

            migrationBuilder.DropColumn(
                name: "IsFixedAsset",
                schema: "Stock",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "AccumulatedDepreciationAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropColumn(
                name: "DepreciationExpenseAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropColumn(
                name: "EquityAccountId",
                schema: "Accounting",
                table: "CompanyProfile");

            migrationBuilder.DropColumn(
                name: "FixedAssetAccountId",
                schema: "Accounting",
                table: "CompanyProfile");
        }
    }
}
