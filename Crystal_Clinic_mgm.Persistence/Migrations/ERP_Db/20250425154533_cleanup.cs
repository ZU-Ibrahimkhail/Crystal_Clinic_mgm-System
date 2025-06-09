using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class cleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItem_RentalReservation_RentalReservationReservationId",
                schema: "Sales",
                table: "OrderItem");

            migrationBuilder.DropTable(
                name: "Assignment",
                schema: "PropertyMS");

            migrationBuilder.DropTable(
                name: "Maintenance",
                schema: "PropertyMS");

            migrationBuilder.DropTable(
                name: "Payment",
                schema: "PropertyMS");

            migrationBuilder.DropTable(
                name: "RentalReservation",
                schema: "Rentals");

            migrationBuilder.DropTable(
                name: "Property",
                schema: "PropertyMS");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "Look");

            migrationBuilder.DropTable(
                name: "PropertyCondition",
                schema: "Look");

            migrationBuilder.DropIndex(
                name: "IX_OrderItem_RentalReservationReservationId",
                schema: "Sales",
                table: "OrderItem");

            migrationBuilder.DropColumn(
                name: "RentalReservationReservationId",
                schema: "Sales",
                table: "OrderItem");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                schema: "Inventory",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                schema: "Inventory",
                table: "Item");

            migrationBuilder.EnsureSchema(
                name: "PropertyMS");

            migrationBuilder.AddColumn<int>(
                name: "RentalReservationReservationId",
                schema: "Sales",
                table: "OrderItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "Look",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PashtoName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "PropertyCondition",
                schema: "Look",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    DariName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PashtoName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyCondition", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RentalReservation",
                schema: "Rentals",
                columns: table => new
                {
                    ReservationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ReservedFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReservedTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalReservation", x => x.ReservationId);
                    table.ForeignKey(
                        name: "FK_RentalReservation_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Sales",
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                schema: "PropertyMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    PropertyConditionID = table.Column<int>(type: "int", nullable: false),
                    AmountPaid = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Price = table.Column<float>(type: "real", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Property_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalSchema: "Look",
                        principalTable: "Category",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Property_PropertyCondition_PropertyConditionID",
                        column: x => x.PropertyConditionID,
                        principalSchema: "Look",
                        principalTable: "PropertyCondition",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Assignment",
                schema: "PropertyMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "DateTime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Assignment_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "PropertyMS",
                        principalTable: "Property",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Maintenance",
                schema: "PropertyMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Date = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PayedAmountInUSD = table.Column<float>(type: "real", nullable: true),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenance", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Maintenance_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "PropertyMS",
                        principalTable: "Property",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                schema: "PropertyMS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountPaid = table.Column<float>(type: "real", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "DateTime", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "UNIQUEIDENTIFIER", maxLength: 50, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    PropertyId = table.Column<int>(type: "int", nullable: false),
                    RemainingBalance = table.Column<float>(type: "real", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Payment_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: "PropertyMS",
                        principalTable: "Property",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_RentalReservationReservationId",
                schema: "Sales",
                table: "OrderItem",
                column: "RentalReservationReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignment_PropertyId",
                schema: "PropertyMS",
                table: "Assignment",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_PropertyId",
                schema: "PropertyMS",
                table: "Maintenance",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PropertyId",
                schema: "PropertyMS",
                table: "Payment",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Property_CategoryID",
                schema: "PropertyMS",
                table: "Property",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Property_PropertyConditionID",
                schema: "PropertyMS",
                table: "Property",
                column: "PropertyConditionID");

            migrationBuilder.CreateIndex(
                name: "IX_RentalReservation_OrderId",
                schema: "Rentals",
                table: "RentalReservation",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItem_RentalReservation_RentalReservationReservationId",
                schema: "Sales",
                table: "OrderItem",
                column: "RentalReservationReservationId",
                principalSchema: "Rentals",
                principalTable: "RentalReservation",
                principalColumn: "ReservationId");
        }
    }
}
