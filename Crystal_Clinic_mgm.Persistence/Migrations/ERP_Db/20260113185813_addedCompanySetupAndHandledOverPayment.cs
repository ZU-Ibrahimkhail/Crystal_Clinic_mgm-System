using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class addedCompanySetupAndHandledOverPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AmountReceived",
                schema: "Accounting",
                table: "Receipt",
                newName: "Amount");

            migrationBuilder.AddColumn<int>(
                name: "OriginalReceiptId",
                schema: "Accounting",
                table: "Receipt",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                schema: "Accounting",
                table: "Receipt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CompanyProfile",
                schema: "Accounting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WhatsappNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BaseCurrencyId = table.Column<int>(type: "int", nullable: false),
                    CurrencyTypeID = table.Column<int>(type: "int", nullable: true),
                    CashAccountId = table.Column<int>(type: "int", nullable: false),
                    BankAccountId = table.Column<int>(type: "int", nullable: false),
                    AccountsReceivableAccountId = table.Column<int>(type: "int", nullable: false),
                    AccountsPayableAccountId = table.Column<int>(type: "int", nullable: false),
                    SalesRevenueAccountId = table.Column<int>(type: "int", nullable: false),
                    InventoryAccountId = table.Column<int>(type: "int", nullable: false),
                    PurchaseExpenseAccountId = table.Column<int>(type: "int", nullable: false),
                    IsInitialized = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_AccountsPayableAccountId",
                        column: x => x.AccountsPayableAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_AccountsReceivableAccountId",
                        column: x => x.AccountsReceivableAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_BankAccountId",
                        column: x => x.BankAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_CashAccountId",
                        column: x => x.CashAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_InventoryAccountId",
                        column: x => x.InventoryAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_PurchaseExpenseAccountId",
                        column: x => x.PurchaseExpenseAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyProfile_ChartOfAccounts_SalesRevenueAccountId",
                        column: x => x.SalesRevenueAccountId,
                        principalSchema: "Accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompanyProfile_CurrencyType_CurrencyTypeID",
                        column: x => x.CurrencyTypeID,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Receipt_OriginalReceiptId",
                schema: "Accounting",
                table: "Receipt",
                column: "OriginalReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_AccountsPayableAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "AccountsPayableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_AccountsReceivableAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "AccountsReceivableAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_BankAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "BankAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_CashAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "CashAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_CurrencyTypeID",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "CurrencyTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_InventoryAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "InventoryAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_PurchaseExpenseAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "PurchaseExpenseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyProfile_SalesRevenueAccountId",
                schema: "Accounting",
                table: "CompanyProfile",
                column: "SalesRevenueAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipt_Receipt_OriginalReceiptId",
                schema: "Accounting",
                table: "Receipt",
                column: "OriginalReceiptId",
                principalSchema: "Accounting",
                principalTable: "Receipt",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipt_Receipt_OriginalReceiptId",
                schema: "Accounting",
                table: "Receipt");

            migrationBuilder.DropTable(
                name: "CompanyProfile",
                schema: "Accounting");

            migrationBuilder.DropIndex(
                name: "IX_Receipt_OriginalReceiptId",
                schema: "Accounting",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "OriginalReceiptId",
                schema: "Accounting",
                table: "Receipt");

            migrationBuilder.DropColumn(
                name: "TransactionType",
                schema: "Accounting",
                table: "Receipt");

            migrationBuilder.RenameColumn(
                name: "Amount",
                schema: "Accounting",
                table: "Receipt",
                newName: "AmountReceived");
        }
    }
}
