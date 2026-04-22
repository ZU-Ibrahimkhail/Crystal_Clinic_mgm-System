using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class AddedVisitKitsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "VisitKits");

            migrationBuilder.CreateTable(
                name: "VisitKits",
                schema: "VisitKits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitId = table.Column<int>(type: "int", nullable: false),
                    ServiceSessionId = table.Column<int>(type: "int", nullable: false),
                    KitId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitKits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitKits_InventoryKits_KitId",
                        column: x => x.KitId,
                        principalTable: "InventoryKits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitKits_ServiceSessions_ServiceSessionId",
                        column: x => x.ServiceSessionId,
                        principalSchema: "CrystalClinic",
                        principalTable: "ServiceSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitKits_Visit_VisitId",
                        column: x => x.VisitId,
                        principalSchema: "CrystalClinic",
                        principalTable: "Visit",
                        principalColumn: "visitId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VisitKits_KitId",
                schema: "VisitKits",
                table: "VisitKits",
                column: "KitId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitKits_ServiceSessionId",
                schema: "VisitKits",
                table: "VisitKits",
                column: "ServiceSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitKits_VisitId_ServiceSessionId_KitId",
                schema: "VisitKits",
                table: "VisitKits",
                columns: new[] { "VisitId", "ServiceSessionId", "KitId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VisitKits",
                schema: "VisitKits");
        }
    }
}
