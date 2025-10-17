using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class CallList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CallList",
                schema: "CrystalClinic",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CallingReason = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(255)", nullable: true),
                    ToBeCalledDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ActualCalledDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    CallResponse = table.Column<int>(type: "int", nullable: false),
                    ResponseReasult = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedEmployeeId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CallList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CallList_EmployeeProfile_AssignedEmployeeId",
                        column: x => x.AssignedEmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CallList_AssignedEmployeeId",
                schema: "CrystalClinic",
                table: "CallList",
                column: "AssignedEmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CallList",
                schema: "CrystalClinic");
        }
    }
}
