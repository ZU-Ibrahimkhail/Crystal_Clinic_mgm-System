using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class addedCleaningJob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CleaningStateQuantity",
                schema: "Inventory",
                table: "Item",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "ItemCleaningJob",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    itemId = table.Column<int>(type: "int", nullable: false),
                    unitId = table.Column<int>(type: "int", nullable: true),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    dateToBeRestocked = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCleaningJob", x => x.id);
                    table.ForeignKey(
                        name: "FK_ItemCleaningJob_ItemUnit_unitId",
                        column: x => x.unitId,
                        principalSchema: "Inventory",
                        principalTable: "ItemUnit",
                        principalColumn: "unitId");

                    table.ForeignKey(
                        name: "FK_ItemCleaningJob_Item_itemId",
                        column: x => x.itemId,
                        principalSchema: "Inventory",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemCleaningJob_itemId",
                table: "ItemCleaningJob",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCleaningJob_unitId",
                table: "ItemCleaningJob",
                column: "unitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCleaningJob");

            migrationBuilder.DropColumn(
                name: "CleaningStateQuantity",
                schema: "Inventory",
                table: "Item");
        }
    }
}
