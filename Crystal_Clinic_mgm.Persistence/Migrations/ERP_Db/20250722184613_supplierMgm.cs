using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class supplierMgm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BranchStock");

            migrationBuilder.CreateTable(
                name: "Supplier",
                schema: "BranchStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupplierDue",
                schema: "BranchStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    DueAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierDue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierDue_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierDue_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "BranchStock",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DuePayment",
                schema: "BranchStock",
                columns: table => new
                {
                    DuePaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierDueId = table.Column<int>(type: "int", nullable: false),
                    CurrencyTypeId = table.Column<int>(type: "int", nullable: true),
                    ExchangeRateToDueCurrency = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    AmmountPaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountInDueCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuePayment", x => x.DuePaymentId);
                    table.ForeignKey(
                        name: "FK_DuePayment_CurrencyType_CurrencyTypeId",
                        column: x => x.CurrencyTypeId,
                        principalSchema: "Look",
                        principalTable: "CurrencyType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DuePayment_SupplierDue_SupplierDueId",
                        column: x => x.SupplierDueId,
                        principalSchema: "BranchStock",
                        principalTable: "SupplierDue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DuePayment_CurrencyTypeId",
                schema: "BranchStock",
                table: "DuePayment",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DuePayment_SupplierDueId",
                schema: "BranchStock",
                table: "DuePayment",
                column: "SupplierDueId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierDue_CurrencyTypeId",
                schema: "BranchStock",
                table: "SupplierDue",
                column: "CurrencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierDue_SupplierId",
                schema: "BranchStock",
                table: "SupplierDue",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuePayment",
                schema: "BranchStock");

            migrationBuilder.DropTable(
                name: "SupplierDue",
                schema: "BranchStock");

            migrationBuilder.DropTable(
                name: "Supplier",
                schema: "BranchStock");
        }
    }
}
