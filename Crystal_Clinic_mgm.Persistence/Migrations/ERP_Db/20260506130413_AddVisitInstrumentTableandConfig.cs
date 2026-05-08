using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class AddVisitInstrumentTableandConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VisitInstrument",
                schema: "CrystalClinic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitId = table.Column<int>(type: "int", nullable: false),
                    KitId = table.Column<int>(type: "int", nullable: true),
                    ServiceSessionsId = table.Column<int>(type: "int", nullable: true),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    IsInvoiceGenerated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitInstrument", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitInstrument_InventoryKits_KitId",
                        column: x => x.KitId,
                        principalTable: "InventoryKits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VisitInstrument_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Stock",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VisitInstrument_ServiceSessions_ServiceSessionsId",
                        column: x => x.ServiceSessionsId,
                        principalSchema: "CrystalClinic",
                        principalTable: "ServiceSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_VisitInstrument_Visit_VisitId",
                        column: x => x.VisitId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Visit",
                        principalColumn: "visitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitInstrument_ItemId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitInstrument_KitId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                column: "KitId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitInstrument_ServiceSessionsId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                column: "ServiceSessionsId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitInstrument_VisitId",
                schema: "CrystalClinic",
                table: "VisitInstrument",
                column: "VisitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitInstrument",
                schema: "CrystalClinic");
        }
    }
}
