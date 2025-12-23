using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel_20251223_135243 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_GR_ROUTE_RII_GR_IMPORT_LINE_ImportLineId",
                table: "RII_GR_ROUTE");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "RII_WT_ROUTE");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "RII_WO_ROUTE");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "RII_WI_ROUTE");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "RII_SRT_ROUTE");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "RII_SIT_ROUTE");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "RII_SH_ROUTE");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "RII_PT_ROUTE");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "RII_PR_ROUTE");

            migrationBuilder.DropColumn(
                name: "LineId",
                table: "RII_GR_ROUTE");

            migrationBuilder.RenameIndex(
                name: "IX_RII_GR_ROUTE_ImportLineId",
                table: "RII_GR_ROUTE",
                newName: "IX_GrRoute_ImportLineId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RII_GR_ROUTE",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "RII_GR_ROUTE",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "RII_GR_TERMINAL_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    TerminalUserId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_RII_GR_TERMINAL_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_GR_TERMINAL_LINE_RII_GR_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_GR_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_GR_TERMINAL_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_GR_TERMINAL_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_GR_TERMINAL_LINE_RII_USERS_TerminalUserId",
                        column: x => x.TerminalUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_GR_TERMINAL_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PR_HEADER_SERIAL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
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
                    table.PrimaryKey("PK_RII_PR_HEADER_SERIAL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PR_HEADER_SERIAL_RII_PR_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_PR_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_PR_HEADER_SERIAL_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_HEADER_SERIAL_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_HEADER_SERIAL_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrRoute_IsDeleted",
                table: "RII_GR_ROUTE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_GrRoute_SerialNo",
                table: "RII_GR_ROUTE",
                column: "SerialNo");

            migrationBuilder.CreateIndex(
                name: "IX_GrRoute_SourceWarehouse",
                table: "RII_GR_ROUTE",
                column: "SourceWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_GrRoute_TargetWarehouse",
                table: "RII_GR_ROUTE",
                column: "TargetWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_GrTerminalLine_HeaderId",
                table: "RII_GR_TERMINAL_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_GrTerminalLine_IsDeleted",
                table: "RII_GR_TERMINAL_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_GrTerminalLine_TerminalUserId",
                table: "RII_GR_TERMINAL_LINE",
                column: "TerminalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_TERMINAL_LINE_CreatedBy",
                table: "RII_GR_TERMINAL_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_TERMINAL_LINE_DeletedBy",
                table: "RII_GR_TERMINAL_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_TERMINAL_LINE_UpdatedBy",
                table: "RII_GR_TERMINAL_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeaderSerial_HeaderId",
                table: "RII_PR_HEADER_SERIAL",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeaderSerial_IsDeleted",
                table: "RII_PR_HEADER_SERIAL",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_HEADER_SERIAL_CreatedBy",
                table: "RII_PR_HEADER_SERIAL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_HEADER_SERIAL_DeletedBy",
                table: "RII_PR_HEADER_SERIAL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_HEADER_SERIAL_UpdatedBy",
                table: "RII_PR_HEADER_SERIAL",
                column: "UpdatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_ROUTE_RII_GR_IMPORT_LINE_ImportLineId",
                table: "RII_GR_ROUTE",
                column: "ImportLineId",
                principalTable: "RII_GR_IMPORT_LINE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_GR_ROUTE_RII_GR_IMPORT_LINE_ImportLineId",
                table: "RII_GR_ROUTE");

            migrationBuilder.DropTable(
                name: "RII_GR_TERMINAL_LINE");

            migrationBuilder.DropTable(
                name: "RII_PR_HEADER_SERIAL");

            migrationBuilder.DropIndex(
                name: "IX_GrRoute_IsDeleted",
                table: "RII_GR_ROUTE");

            migrationBuilder.DropIndex(
                name: "IX_GrRoute_SerialNo",
                table: "RII_GR_ROUTE");

            migrationBuilder.DropIndex(
                name: "IX_GrRoute_SourceWarehouse",
                table: "RII_GR_ROUTE");

            migrationBuilder.DropIndex(
                name: "IX_GrRoute_TargetWarehouse",
                table: "RII_GR_ROUTE");

            migrationBuilder.RenameIndex(
                name: "IX_GrRoute_ImportLineId",
                table: "RII_GR_ROUTE",
                newName: "IX_RII_GR_ROUTE_ImportLineId");

            migrationBuilder.AddColumn<long>(
                name: "LineId",
                table: "RII_WT_ROUTE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LineId",
                table: "RII_WO_ROUTE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LineId",
                table: "RII_WI_ROUTE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LineId",
                table: "RII_SRT_ROUTE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LineId",
                table: "RII_SIT_ROUTE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LineId",
                table: "RII_SH_ROUTE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LineId",
                table: "RII_PT_ROUTE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LineId",
                table: "RII_PR_ROUTE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RII_GR_ROUTE",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "RII_GR_ROUTE",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<long>(
                name: "LineId",
                table: "RII_GR_ROUTE",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_ROUTE_RII_GR_IMPORT_LINE_ImportLineId",
                table: "RII_GR_ROUTE",
                column: "ImportLineId",
                principalTable: "RII_GR_IMPORT_LINE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
