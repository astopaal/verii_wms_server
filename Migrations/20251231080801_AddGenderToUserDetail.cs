using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddGenderToUserDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "RII_USER_DETAIL",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "RII_USER_DETAIL");
        }
    }
}
