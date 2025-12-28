using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakePLineYapKodNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FN_ShOpenOrder_Header");

            migrationBuilder.DropTable(
                name: "FN_ShOpenOrder_Line");

            migrationBuilder.DropTable(
                name: "FN_SitOpenOrder_Header");

            migrationBuilder.DropTable(
                name: "FN_SitOpenOrder_Line");

            migrationBuilder.DropTable(
                name: "FN_SrtOpenOrder_Header");

            migrationBuilder.DropTable(
                name: "FN_SrtOpenOrder_Line");

            migrationBuilder.DropTable(
                name: "FN_TransferOpenOrder_Header");

            migrationBuilder.DropTable(
                name: "FN_TransferOpenOrder_Line");

            migrationBuilder.DropTable(
                name: "FN_WiOpenOrder_Header");

            migrationBuilder.DropTable(
                name: "FN_WiOpenOrder_Line");

            migrationBuilder.DropTable(
                name: "FN_WoOpenOrder_Header");

            migrationBuilder.DropTable(
                name: "FN_WoOpenOrder_Line");

            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_P_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "YapKod",
                table: "RII_P_LINE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "FN_ShOpenOrder_Header",
                columns: table => new
                {
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TargetWh = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_ShOpenOrder_Line",
                columns: table => new
                {
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetWh = table.Column<int>(type: "int", nullable: true),
                    YapAcik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_SitOpenOrder_Header",
                columns: table => new
                {
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TargetWh = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_SitOpenOrder_Line",
                columns: table => new
                {
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetWh = table.Column<int>(type: "int", nullable: true),
                    YapAcik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_SrtOpenOrder_Header",
                columns: table => new
                {
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TargetWh = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_SrtOpenOrder_Line",
                columns: table => new
                {
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetWh = table.Column<int>(type: "int", nullable: true),
                    YapAcik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_TransferOpenOrder_Header",
                columns: table => new
                {
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TargetWh = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_TransferOpenOrder_Line",
                columns: table => new
                {
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetWh = table.Column<int>(type: "int", nullable: true),
                    YapAcik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_WiOpenOrder_Header",
                columns: table => new
                {
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TargetWh = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_WiOpenOrder_Line",
                columns: table => new
                {
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetWh = table.Column<int>(type: "int", nullable: true),
                    YapAcik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_WoOpenOrder_Header",
                columns: table => new
                {
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    TargetWh = table.Column<short>(type: "smallint", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_WoOpenOrder_Line",
                columns: table => new
                {
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetWh = table.Column<int>(type: "int", nullable: true),
                    YapAcik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });
        }
    }
}
