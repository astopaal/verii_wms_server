using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRecipientTerminalUserIdFromNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RII_NOTIFICATION_RII_USERS_RecipientTerminalUserId",
                table: "RII_NOTIFICATION");

            migrationBuilder.DropIndex(
                name: "IX_RII_NOTIFICATION_RecipientTerminalUserId",
                table: "RII_NOTIFICATION");

            migrationBuilder.DropColumn(
                name: "RecipientTerminalUserId",
                table: "RII_NOTIFICATION");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RecipientTerminalUserId",
                table: "RII_NOTIFICATION",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RII_NOTIFICATION_RecipientTerminalUserId",
                table: "RII_NOTIFICATION",
                column: "RecipientTerminalUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RII_NOTIFICATION_RII_USERS_RecipientTerminalUserId",
                table: "RII_NOTIFICATION",
                column: "RecipientTerminalUserId",
                principalTable: "RII_USERS",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
