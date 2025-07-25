using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class supplierDuedate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DuePayment_SupplierDue_SupplierDueId",
                schema: "BranchStock",
                table: "DuePayment");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                schema: "BranchStock",
                table: "SupplierDue",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DuePayment_SupplierDue_SupplierDueId",
                schema: "BranchStock",
                table: "DuePayment",
                column: "SupplierDueId",
                principalSchema: "BranchStock",
                principalTable: "SupplierDue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DuePayment_SupplierDue_SupplierDueId",
                schema: "BranchStock",
                table: "DuePayment");

            migrationBuilder.DropColumn(
                name: "DueDate",
                schema: "BranchStock",
                table: "SupplierDue");

            migrationBuilder.AddForeignKey(
                name: "FK_DuePayment_SupplierDue_SupplierDueId",
                schema: "BranchStock",
                table: "DuePayment",
                column: "SupplierDueId",
                principalSchema: "BranchStock",
                principalTable: "SupplierDue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
