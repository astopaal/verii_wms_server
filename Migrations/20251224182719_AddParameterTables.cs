using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddParameterTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RII_GR_PARAMETER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    AllowLessQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    AllowMoreQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    RequireApprovalBeforeErp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_GR_PARAMETER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_GR_PARAMETER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_GR_PARAMETER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_GR_PARAMETER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_IC_PARAMETER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    AllowLessQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    AllowMoreQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    RequireApprovalBeforeErp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_IC_PARAMETER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_IC_PARAMETER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_IC_PARAMETER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_IC_PARAMETER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_P_PARAMETER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    AllowLessQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    AllowMoreQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    RequireApprovalBeforeErp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_P_PARAMETER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_P_PARAMETER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_P_PARAMETER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_P_PARAMETER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PR_PARAMETER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    AllowLessQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    AllowMoreQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    RequireApprovalBeforeErp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PR_PARAMETER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PR_PARAMETER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_PARAMETER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_PARAMETER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PT_PARAMETER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    AllowLessQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    AllowMoreQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    RequireApprovalBeforeErp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PT_PARAMETER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PT_PARAMETER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PT_PARAMETER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PT_PARAMETER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_SH_PARAMETER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    AllowLessQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    AllowMoreQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    RequireApprovalBeforeErp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SH_PARAMETER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SH_PARAMETER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_PARAMETER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_PARAMETER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_SIT_PARAMETER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    AllowLessQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    AllowMoreQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    RequireApprovalBeforeErp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SIT_PARAMETER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SIT_PARAMETER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SIT_PARAMETER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SIT_PARAMETER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_SRT_PARAMETER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    AllowLessQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    AllowMoreQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    RequireApprovalBeforeErp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SRT_PARAMETER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SRT_PARAMETER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SRT_PARAMETER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SRT_PARAMETER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_WI_PARAMETER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    AllowLessQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    AllowMoreQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    RequireApprovalBeforeErp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WI_PARAMETER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WI_PARAMETER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_PARAMETER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WI_PARAMETER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_WO_PARAMETER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    AllowLessQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    AllowMoreQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    RequireApprovalBeforeErp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WO_PARAMETER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WO_PARAMETER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_PARAMETER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WO_PARAMETER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_WT_PARAMETER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    AllowLessQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    AllowMoreQuantityBasedOnOrder = table.Column<bool>(type: "bit", nullable: false),
                    RequireApprovalBeforeErp = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_WT_PARAMETER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_WT_PARAMETER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_PARAMETER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_WT_PARAMETER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_PARAMETER_CreatedBy",
                table: "RII_GR_PARAMETER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_PARAMETER_DeletedBy",
                table: "RII_GR_PARAMETER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_GR_PARAMETER_UpdatedBy",
                table: "RII_GR_PARAMETER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_PARAMETER_CreatedBy",
                table: "RII_IC_PARAMETER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_PARAMETER_DeletedBy",
                table: "RII_IC_PARAMETER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_IC_PARAMETER_UpdatedBy",
                table: "RII_IC_PARAMETER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_P_PARAMETER_CreatedBy",
                table: "RII_P_PARAMETER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_P_PARAMETER_DeletedBy",
                table: "RII_P_PARAMETER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_P_PARAMETER_UpdatedBy",
                table: "RII_P_PARAMETER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_PARAMETER_CreatedBy",
                table: "RII_PR_PARAMETER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_PARAMETER_DeletedBy",
                table: "RII_PR_PARAMETER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_PARAMETER_UpdatedBy",
                table: "RII_PR_PARAMETER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_PARAMETER_CreatedBy",
                table: "RII_PT_PARAMETER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_PARAMETER_DeletedBy",
                table: "RII_PT_PARAMETER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PT_PARAMETER_UpdatedBy",
                table: "RII_PT_PARAMETER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_PARAMETER_CreatedBy",
                table: "RII_SH_PARAMETER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_PARAMETER_DeletedBy",
                table: "RII_SH_PARAMETER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_PARAMETER_UpdatedBy",
                table: "RII_SH_PARAMETER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_PARAMETER_CreatedBy",
                table: "RII_SIT_PARAMETER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_PARAMETER_DeletedBy",
                table: "RII_SIT_PARAMETER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SIT_PARAMETER_UpdatedBy",
                table: "RII_SIT_PARAMETER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_PARAMETER_CreatedBy",
                table: "RII_SRT_PARAMETER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_PARAMETER_DeletedBy",
                table: "RII_SRT_PARAMETER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SRT_PARAMETER_UpdatedBy",
                table: "RII_SRT_PARAMETER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_PARAMETER_CreatedBy",
                table: "RII_WI_PARAMETER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_PARAMETER_DeletedBy",
                table: "RII_WI_PARAMETER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WI_PARAMETER_UpdatedBy",
                table: "RII_WI_PARAMETER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_PARAMETER_CreatedBy",
                table: "RII_WO_PARAMETER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_PARAMETER_DeletedBy",
                table: "RII_WO_PARAMETER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WO_PARAMETER_UpdatedBy",
                table: "RII_WO_PARAMETER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_PARAMETER_CreatedBy",
                table: "RII_WT_PARAMETER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_PARAMETER_DeletedBy",
                table: "RII_WT_PARAMETER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_WT_PARAMETER_UpdatedBy",
                table: "RII_WT_PARAMETER",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RII_GR_PARAMETER");

            migrationBuilder.DropTable(
                name: "RII_IC_PARAMETER");

            migrationBuilder.DropTable(
                name: "RII_P_PARAMETER");

            migrationBuilder.DropTable(
                name: "RII_PR_PARAMETER");

            migrationBuilder.DropTable(
                name: "RII_PT_PARAMETER");

            migrationBuilder.DropTable(
                name: "RII_SH_PARAMETER");

            migrationBuilder.DropTable(
                name: "RII_SIT_PARAMETER");

            migrationBuilder.DropTable(
                name: "RII_SRT_PARAMETER");

            migrationBuilder.DropTable(
                name: "RII_WI_PARAMETER");

            migrationBuilder.DropTable(
                name: "RII_WO_PARAMETER");

            migrationBuilder.DropTable(
                name: "RII_WT_PARAMETER");
        }
    }
}
