using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePackageModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_P_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarehouseCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PackingNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PackingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SourceType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    SourceHeaderId = table.Column<long>(type: "bigint", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CustomerAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Draft"),
                    TotalPackageCount = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    TotalQuantity = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    TotalNetWeight = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    TotalGrossWeight = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    TotalVolume = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    CarrierId = table.Column<long>(type: "bigint", nullable: true),
                    CarrierServiceType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TrackingNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_P_HEADER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_P_HEADER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_P_HEADER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_P_HEADER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_P_PACKAGE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackingHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    PackageNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PackageType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Box"),
                    Barcode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Length = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    Width = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    Volume = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    NetWeight = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    TareWeight = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    GrossWeight = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    IsMixed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Open"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_P_PACKAGE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PPackage_PHeader",
                        column: x => x.PackingHeaderId,
                        principalTable: "RII_P_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_P_PACKAGE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_P_PACKAGE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_P_PACKAGE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_P_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackingHeaderId = table.Column<long>(type: "bigint", nullable: false),
                    PackageId = table.Column<long>(type: "bigint", nullable: false),
                    Barcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceRouteId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_P_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PLine_PHeader",
                        column: x => x.PackingHeaderId,
                        principalTable: "RII_P_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PLine_PPackage",
                        column: x => x.PackageId,
                        principalTable: "RII_P_PACKAGE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_P_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_P_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_P_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PHeader_CustomerCode",
                table: "RII_P_HEADER",
                column: "CustomerCode");

            migrationBuilder.CreateIndex(
                name: "IX_PHeader_IsDeleted",
                table: "RII_P_HEADER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PHeader_PackingNo",
                table: "RII_P_HEADER",
                column: "PackingNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PHeader_SourceHeaderId",
                table: "RII_P_HEADER",
                column: "SourceHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PHeader_Status",
                table: "RII_P_HEADER",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PHeader_WarehouseCode",
                table: "RII_P_HEADER",
                column: "WarehouseCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_P_HEADER_CreatedBy",
                table: "RII_P_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_P_HEADER_DeletedBy",
                table: "RII_P_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_P_HEADER_UpdatedBy",
                table: "RII_P_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PLine_IsDeleted",
                table: "RII_P_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PLine_PackageId",
                table: "RII_P_LINE",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PLine_PackingHeaderId",
                table: "RII_P_LINE",
                column: "PackingHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PLine_StockCode",
                table: "RII_P_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_P_LINE_CreatedBy",
                table: "RII_P_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_P_LINE_DeletedBy",
                table: "RII_P_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_P_LINE_UpdatedBy",
                table: "RII_P_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PPackage_Barcode",
                table: "RII_P_PACKAGE",
                column: "Barcode",
                unique: true,
                filter: "[Barcode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PPackage_IsDeleted",
                table: "RII_P_PACKAGE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PPackage_PackageNo",
                table: "RII_P_PACKAGE",
                column: "PackageNo");

            migrationBuilder.CreateIndex(
                name: "IX_PPackage_PackingHeaderId",
                table: "RII_P_PACKAGE",
                column: "PackingHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PPackage_Status",
                table: "RII_P_PACKAGE",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RII_P_PACKAGE_CreatedBy",
                table: "RII_P_PACKAGE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_P_PACKAGE_DeletedBy",
                table: "RII_P_PACKAGE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_P_PACKAGE_UpdatedBy",
                table: "RII_P_PACKAGE",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_P_LINE");

            migrationBuilder.DropTable(
                name: "RII_P_PACKAGE");

            migrationBuilder.DropTable(
                name: "RII_P_HEADER");
        }
    }
}
