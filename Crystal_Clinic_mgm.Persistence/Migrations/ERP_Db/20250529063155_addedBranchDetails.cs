using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class addedBranchDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchDetailId",
                schema: "Sales",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BranchDetails",
                schema: "Look",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeaderNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FooterNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BranchDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BranchDetails_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BranchDetails_BranchId",
                schema: "Look",
                table: "BranchDetails",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BranchDetails",
                schema: "Look");

            migrationBuilder.DropColumn(
                name: "BranchDetailId",
                schema: "Sales",
                table: "Orders");
        }
    }
}
