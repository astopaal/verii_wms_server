using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUserDetailTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_USER_DETAIL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ProfilePictureUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Height = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
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
                    table.PrimaryKey("PK_RII_USER_DETAIL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_USER_DETAIL_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_DETAIL_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_DETAIL_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_USER_DETAIL_RII_USERS_UserId",
                        column: x => x.UserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_DETAIL_CreatedBy",
                table: "RII_USER_DETAIL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_DETAIL_DeletedBy",
                table: "RII_USER_DETAIL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_DETAIL_UpdatedBy",
                table: "RII_USER_DETAIL",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_USER_DETAIL_UserId",
                table: "RII_USER_DETAIL",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_USER_DETAIL");
        }
    }
}
