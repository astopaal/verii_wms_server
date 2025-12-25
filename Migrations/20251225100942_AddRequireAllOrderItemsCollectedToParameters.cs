using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddRequireAllOrderItemsCollectedToParameters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequireAllOrderItemsCollected",
                table: "RII_WT_PARAMETER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireAllOrderItemsCollected",
                table: "RII_WO_PARAMETER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireAllOrderItemsCollected",
                table: "RII_WI_PARAMETER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireAllOrderItemsCollected",
                table: "RII_SRT_PARAMETER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireAllOrderItemsCollected",
                table: "RII_SIT_PARAMETER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireAllOrderItemsCollected",
                table: "RII_SH_PARAMETER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireAllOrderItemsCollected",
                table: "RII_PT_PARAMETER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireAllOrderItemsCollected",
                table: "RII_PR_PARAMETER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireAllOrderItemsCollected",
                table: "RII_P_PARAMETER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireAllOrderItemsCollected",
                table: "RII_IC_PARAMETER",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireAllOrderItemsCollected",
                table: "RII_GR_PARAMETER",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequireAllOrderItemsCollected",
                table: "RII_WT_PARAMETER");

            migrationBuilder.DropColumn(
                name: "RequireAllOrderItemsCollected",
                table: "RII_WO_PARAMETER");

            migrationBuilder.DropColumn(
                name: "RequireAllOrderItemsCollected",
                table: "RII_WI_PARAMETER");

            migrationBuilder.DropColumn(
                name: "RequireAllOrderItemsCollected",
                table: "RII_SRT_PARAMETER");

            migrationBuilder.DropColumn(
                name: "RequireAllOrderItemsCollected",
                table: "RII_SIT_PARAMETER");

            migrationBuilder.DropColumn(
                name: "RequireAllOrderItemsCollected",
                table: "RII_SH_PARAMETER");

            migrationBuilder.DropColumn(
                name: "RequireAllOrderItemsCollected",
                table: "RII_PT_PARAMETER");

            migrationBuilder.DropColumn(
                name: "RequireAllOrderItemsCollected",
                table: "RII_PR_PARAMETER");

            migrationBuilder.DropColumn(
                name: "RequireAllOrderItemsCollected",
                table: "RII_P_PARAMETER");

            migrationBuilder.DropColumn(
                name: "RequireAllOrderItemsCollected",
                table: "RII_IC_PARAMETER");

            migrationBuilder.DropColumn(
                name: "RequireAllOrderItemsCollected",
                table: "RII_GR_PARAMETER");
        }
    }
}
