using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class accountTransactionTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "approvedBy",
                schema: "AssetMS",
                table: "AccountTracking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "fromUserId",
                schema: "AssetMS",
                table: "AccountTracking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "toUserId",
                schema: "AssetMS",
                table: "AccountTracking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "transactionStatus",
                schema: "AssetMS",
                table: "AccountTracking",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "approvedBy",
                schema: "AssetMS",
                table: "AccountTracking");

            migrationBuilder.DropColumn(
                name: "fromUserId",
                schema: "AssetMS",
                table: "AccountTracking");

            migrationBuilder.DropColumn(
                name: "toUserId",
                schema: "AssetMS",
                table: "AccountTracking");

            migrationBuilder.DropColumn(
                name: "transactionStatus",
                schema: "AssetMS",
                table: "AccountTracking");
        }
    }
}
