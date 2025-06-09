using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crystal_Clinic_Mgm.Persistence.Migrations.ERP_Db
{
    /// <inheritdoc />
    public partial class MainTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Sales");

            migrationBuilder.EnsureSchema(
                name: "Returns");

            migrationBuilder.EnsureSchema(
                name: "Inventory");

            migrationBuilder.EnsureSchema(
                name: "Payments");

            migrationBuilder.EnsureSchema(
                name: "Rentals");

            migrationBuilder.EnsureSchema(
                name: "Services");

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "Sales",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsBlacklisted = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "DamageReport",
                schema: "Returns",
                columns: table => new
                {
                    DamageReportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RepairCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageReport", x => x.DamageReportId);
                });

            migrationBuilder.CreateTable(
                name: "ItemCategory",
                schema: "Inventory",
                columns: table => new
                {
                    categoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCategory", x => x.categoryId);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                schema: "Services",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    IsRentable = table.Column<bool>(type: "bit", nullable: false),
                    DailyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    HourlyRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.ServiceId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                schema: "Sales",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderType = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    OriginalTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdjustedTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    DeliveryNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Sales",
                        principalTable: "Customer",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_EmployeeProfile_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                schema: "Inventory",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSellable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsRentable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    BaseUnit = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CurrentStock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReorderLevel = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_Item_ItemCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Inventory",
                        principalTable: "ItemCategory",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderPayment",
                schema: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Method = table.Column<int>(type: "int", nullable: false),
                    TransactionReference = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAdvancePayment = table.Column<bool>(type: "bit", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPayment", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_OrderPayment_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Sales",
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderServices",
                columns: table => new
                {
                    OrderServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedEmployeeId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OriginalRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    DurationType = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderServices", x => x.OrderServiceId);
                    table.ForeignKey(
                        name: "FK_OrderServices_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Sales",
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderServices_Service_ServiceId",
                        column: x => x.ServiceId,
                        principalSchema: "Services",
                        principalTable: "Service",
                        principalColumn: "ServiceId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Return",
                schema: "Rentals",
                columns: table => new
                {
                    ReturnId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DamageFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DepositRefund = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Return", x => x.ReturnId);
                    table.ForeignKey(
                        name: "FK_Return_EmployeeProfile_EmployeeId",
                        column: x => x.EmployeeId,
                        principalSchema: "HR",
                        principalTable: "EmployeeProfile",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Return_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Sales",
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemUnit",
                schema: "Inventory",
                columns: table => new
                {
                    unitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConversionFactor = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    DailyRentalPrice = table.Column<int>(type: "int", nullable: true),
                    SellingPrice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemUnit", x => x.unitId);
                    table.ForeignKey(
                        name: "FK_ItemUnit_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockMovement",
                schema: "Inventory",
                columns: table => new
                {
                    StockMovementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MovementType = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Reason = table.Column<int>(type: "int", nullable: false),
                    SourceBranchId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovement", x => x.StockMovementId);
                    table.ForeignKey(
                        name: "FK_StockMovement_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovement_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Sales",
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "OrderItem",
                schema: "Sales",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OriginalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPriceOverridden = table.Column<bool>(type: "bit", nullable: false),
                    IsRental = table.Column<bool>(type: "bit", nullable: false),
                    RentalDays = table.Column<int>(type: "int", nullable: true),
                    RentalStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RentalEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsExternalItem = table.Column<bool>(type: "bit", nullable: false),
                    SourceBranchId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    RentalReservationReservationId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItem", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItem_Branch_SourceBranchId",
                        column: x => x.SourceBranchId,
                        principalSchema: "Look",
                        principalTable: "Branch",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_OrderItem_ItemUnit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: "Inventory",
                        principalTable: "ItemUnit",
                        principalColumn: "unitId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItem_Item_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Item",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItem_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Sales",
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItem_RentalReservation_RentalReservationReservationId",
                        column: x => x.RentalReservationReservationId,
                        principalSchema: "Rentals",
                        principalTable: "RentalReservation",
                        principalColumn: "ReservationId");
                });

            migrationBuilder.CreateTable(
                name: "OrderAdjustment",
                schema: "Sales",
                columns: table => new
                {
                    AdjustmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdjustmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdjustmentType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    OrderItemId = table.Column<int>(type: "int", nullable: true),
                    OrderServiceId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderAdjustment", x => x.AdjustmentId);
                    table.ForeignKey(
                        name: "FK_OrderAdjustment_OrderItem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalSchema: "Sales",
                        principalTable: "OrderItem",
                        principalColumn: "OrderItemId");
                    table.ForeignKey(
                        name: "FK_OrderAdjustment_OrderServices_OrderServiceId",
                        column: x => x.OrderServiceId,
                        principalTable: "OrderServices",
                        principalColumn: "OrderServiceId");
                    table.ForeignKey(
                        name: "FK_OrderAdjustment_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "Sales",
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Item_CategoryId",
                schema: "Inventory",
                table: "Item",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUnit_ItemId",
                schema: "Inventory",
                table: "ItemUnit",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAdjustment_OrderId",
                schema: "Sales",
                table: "OrderAdjustment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAdjustment_OrderItemId",
                schema: "Sales",
                table: "OrderAdjustment",
                column: "OrderItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderAdjustment_OrderServiceId",
                schema: "Sales",
                table: "OrderAdjustment",
                column: "OrderServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_ItemId",
                schema: "Sales",
                table: "OrderItem",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_OrderId",
                schema: "Sales",
                table: "OrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_RentalReservationReservationId",
                schema: "Sales",
                table: "OrderItem",
                column: "RentalReservationReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_SourceBranchId",
                schema: "Sales",
                table: "OrderItem",
                column: "SourceBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItem_UnitId",
                schema: "Sales",
                table: "OrderItem",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPayment_OrderId",
                schema: "Payments",
                table: "OrderPayment",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                schema: "Sales",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployeeId",
                schema: "Sales",
                table: "Orders",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderServices_OrderId",
                table: "OrderServices",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderServices_ServiceId",
                table: "OrderServices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalReservation_OrderId",
                schema: "Rentals",
                table: "RentalReservation",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Return_EmployeeId",
                schema: "Rentals",
                table: "Return",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Return_OrderId",
                schema: "Rentals",
                table: "Return",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_ItemId",
                schema: "Inventory",
                table: "StockMovement",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_OrderId",
                schema: "Inventory",
                table: "StockMovement",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DamageReport",
                schema: "Returns");

            migrationBuilder.DropTable(
                name: "OrderAdjustment",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "OrderPayment",
                schema: "Payments");

            migrationBuilder.DropTable(
                name: "Return",
                schema: "Rentals");

            migrationBuilder.DropTable(
                name: "StockMovement",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "OrderItem",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "OrderServices");

            migrationBuilder.DropTable(
                name: "ItemUnit",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "RentalReservation",
                schema: "Rentals");

            migrationBuilder.DropTable(
                name: "Service",
                schema: "Services");

            migrationBuilder.DropTable(
                name: "Item",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "Orders",
                schema: "Sales");

            migrationBuilder.DropTable(
                name: "ItemCategory",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "Sales");
        }
    }
}
