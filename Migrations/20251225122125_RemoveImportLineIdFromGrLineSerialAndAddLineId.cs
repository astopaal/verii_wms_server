using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImportLineIdFromGrLineSerialAndAddLineId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // First, drop foreign key constraints to allow data migration
            migrationBuilder.DropForeignKey(
                name: "FK_GrLineSerial_GrImportLine",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropForeignKey(
                name: "FK_RII_GR_LINE_SERIAL_RII_GR_IMPORT_LINE_GrImportLineId",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropForeignKey(
                name: "FK_RII_GR_LINE_SERIAL_RII_GR_LINE_GrLineId",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropIndex(
                name: "IX_RII_GR_LINE_SERIAL_GrImportLineId",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropIndex(
                name: "IX_RII_GR_LINE_SERIAL_GrLineId",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "ClientKey",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "GrImportLineId",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.DropColumn(
                name: "GrLineId",
                table: "RII_GR_LINE_SERIAL");

            // Update ImportLineId values to LineId values from ImportLine table
            migrationBuilder.Sql(@"
                UPDATE ls
                SET ls.ImportLineId = il.LineId
                FROM RII_GR_LINE_SERIAL ls
                INNER JOIN RII_GR_IMPORT_LINE il ON ls.ImportLineId = il.Id
                WHERE ls.ImportLineId IS NOT NULL AND il.LineId IS NOT NULL
            ");

            // Set ImportLineId to NULL where ImportLine doesn't have a LineId
            migrationBuilder.Sql(@"
                UPDATE RII_GR_LINE_SERIAL
                SET ImportLineId = NULL
                WHERE ImportLineId IS NOT NULL 
                AND ImportLineId NOT IN (SELECT Id FROM RII_GR_IMPORT_LINE WHERE LineId IS NOT NULL)
            ");

            migrationBuilder.RenameColumn(
                name: "ImportLineId",
                table: "RII_GR_LINE_SERIAL",
                newName: "LineId");

            migrationBuilder.RenameIndex(
                name: "IX_GrLineSerial_ImportLineId",
                table: "RII_GR_LINE_SERIAL",
                newName: "IX_GrLineSerial_LineId");

            migrationBuilder.AddForeignKey(
                name: "FK_GrLineSerial_GrLine",
                table: "RII_GR_LINE_SERIAL",
                column: "LineId",
                principalTable: "RII_GR_LINE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GrLineSerial_GrLine",
                table: "RII_GR_LINE_SERIAL");

            migrationBuilder.RenameColumn(
                name: "LineId",
                table: "RII_GR_LINE_SERIAL",
                newName: "ImportLineId");

            migrationBuilder.RenameIndex(
                name: "IX_GrLineSerial_LineId",
                table: "RII_GR_LINE_SERIAL",
                newName: "IX_GrLineSerial_ImportLineId");

            migrationBuilder.AddColumn<string>(
                name: "ClientKey",
                table: "RII_GR_LINE_SERIAL",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GrImportLineId",
                table: "RII_GR_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GrLineId",
                table: "RII_GR_LINE_SERIAL",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_LINE_SERIAL_GrImportLineId",
                table: "RII_GR_LINE_SERIAL",
                column: "GrImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_LINE_SERIAL_GrLineId",
                table: "RII_GR_LINE_SERIAL",
                column: "GrLineId");

            migrationBuilder.AddForeignKey(
                name: "FK_GrLineSerial_GrImportLine",
                table: "RII_GR_LINE_SERIAL",
                column: "ImportLineId",
                principalTable: "RII_GR_IMPORT_LINE",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_LINE_SERIAL_RII_GR_IMPORT_LINE_GrImportLineId",
                table: "RII_GR_LINE_SERIAL",
                column: "GrImportLineId",
                principalTable: "RII_GR_IMPORT_LINE",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_GR_LINE_SERIAL_RII_GR_LINE_GrLineId",
                table: "RII_GR_LINE_SERIAL",
                column: "GrLineId",
                principalTable: "RII_GR_LINE",
                principalColumn: "Id");
        }
    }
}
