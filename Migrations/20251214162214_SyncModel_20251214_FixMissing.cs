using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WMS_WEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel_20251214_FixMissing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FN_ShOpenOrder_Header",
                columns: table => new
                {
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    TargetWh = table.Column<short>(type: "smallint", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_ShOpenOrder_Line",
                columns: table => new
                {
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapAcik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    TargetWh = table.Column<int>(type: "int", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_SitOpenOrder_Header",
                columns: table => new
                {
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    TargetWh = table.Column<short>(type: "smallint", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_SitOpenOrder_Line",
                columns: table => new
                {
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapAcik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    TargetWh = table.Column<int>(type: "int", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_SrtOpenOrder_Header",
                columns: table => new
                {
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    TargetWh = table.Column<short>(type: "smallint", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_SrtOpenOrder_Line",
                columns: table => new
                {
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapAcik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    TargetWh = table.Column<int>(type: "int", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_TransferOpenOrder_Header",
                columns: table => new
                {
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    TargetWh = table.Column<short>(type: "smallint", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_TransferOpenOrder_Line",
                columns: table => new
                {
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapAcik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    TargetWh = table.Column<int>(type: "int", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_WiOpenOrder_Header",
                columns: table => new
                {
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    TargetWh = table.Column<short>(type: "smallint", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_WiOpenOrder_Line",
                columns: table => new
                {
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapAcik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    TargetWh = table.Column<int>(type: "int", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_WoOpenOrder_Header",
                columns: table => new
                {
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<short>(type: "smallint", nullable: true),
                    TargetWh = table.Column<short>(type: "smallint", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "FN_WoOpenOrder_Line",
                columns: table => new
                {
                    Mode = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    SiparisNo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    StockCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YapAcik = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomerCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: true),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchCode = table.Column<int>(type: "int", nullable: false),
                    TargetWh = table.Column<int>(type: "int", nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderedQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeliveredQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RemainingHamax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PlannedQtyAllocated = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingForImport = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.Sql(@"
IF OBJECT_ID('dbo.RII_PASSWORD_RESET_REQUEST', 'U') IS NULL
BEGIN
    CREATE TABLE [RII_PASSWORD_RESET_REQUEST] (
        [Id] bigint NOT NULL IDENTITY(1,1),
        [UserId] bigint NOT NULL,
        [TokenHash] nvarchar(128) NOT NULL,
        [ExpiresAt] datetime2 NOT NULL,
        [UsedAt] datetime2 NULL,
        [RequestIp] nvarchar(100) NULL,
        [UserAgent] nvarchar(500) NULL,
        [CreatedDate] datetime2 NOT NULL CONSTRAINT [DF_RPRR_CreatedDate] DEFAULT (GETUTCDATE()),
        [UpdatedDate] datetime2 NULL,
        [DeletedDate] datetime2 NULL,
        [IsDeleted] bit NOT NULL CONSTRAINT [DF_RPRR_IsDeleted] DEFAULT (0),
        [CreatedBy] bigint NULL,
        [UpdatedBy] bigint NULL,
        [DeletedBy] bigint NULL,
        CONSTRAINT [PK_RII_PASSWORD_RESET_REQUEST] PRIMARY KEY ([Id])
    );
    ALTER TABLE [RII_PASSWORD_RESET_REQUEST] WITH CHECK ADD CONSTRAINT [FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_UserId] FOREIGN KEY ([UserId]) REFERENCES [RII_USERS]([Id]) ON DELETE NO ACTION;
    ALTER TABLE [RII_PASSWORD_RESET_REQUEST] WITH CHECK ADD CONSTRAINT [FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [RII_USERS]([Id]);
    ALTER TABLE [RII_PASSWORD_RESET_REQUEST] WITH CHECK ADD CONSTRAINT [FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [RII_USERS]([Id]);
    ALTER TABLE [RII_PASSWORD_RESET_REQUEST] WITH CHECK ADD CONSTRAINT [FK_RII_PASSWORD_RESET_REQUEST_RII_USERS_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [RII_USERS]([Id]);
END

IF COL_LENGTH('RII_PASSWORD_RESET_REQUEST', 'RequestIp') IS NULL
    ALTER TABLE [RII_PASSWORD_RESET_REQUEST] ADD [RequestIp] nvarchar(100) NULL;
IF COL_LENGTH('RII_PASSWORD_RESET_REQUEST', 'UserAgent') IS NULL
    ALTER TABLE [RII_PASSWORD_RESET_REQUEST] ADD [UserAgent] nvarchar(500) NULL;
");

            migrationBuilder.CreateTable(
                name: "RII_PR_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    YapKod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SourceWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YearCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "0"),
                    OrderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPlanned = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DocumentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PriorityLevel = table.Column<byte>(type: "tinyint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERPReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPIntegrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ERPIntegrationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Description1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PR_HEADER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PR_HEADER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_HEADER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_HEADER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_SH_HEADER",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SourceWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetWarehouse = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ShipmentId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    DocumentNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YearCode = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValue: "0"),
                    OrderId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProjectCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PlannedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsPlanned = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DocumentType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    PriorityLevel = table.Column<byte>(type: "tinyint", nullable: true),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPendingApproval = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ApprovalStatus = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedByUserId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsERPIntegrated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ERPReferenceNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPIntegrationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ERPIntegrationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ERPErrorMessage = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Description1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SH_HEADER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SH_HEADER_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_HEADER_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_HEADER_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PR_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ErpOrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ErpOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PR_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PR_LINE_RII_PR_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_PR_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_PR_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PR_TERMINAL_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    TerminalUserId = table.Column<long>(type: "bigint", nullable: false),
                    PrHeaderId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_RII_PR_TERMINAL_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PR_TERMINAL_LINE_RII_PR_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_PR_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_PR_TERMINAL_LINE_RII_PR_HEADER_PrHeaderId",
                        column: x => x.PrHeaderId,
                        principalTable: "RII_PR_HEADER",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_TERMINAL_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_TERMINAL_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_TERMINAL_LINE_RII_USERS_TerminalUserId",
                        column: x => x.TerminalUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_PR_TERMINAL_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_SH_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(35)", maxLength: 35, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ErpOrderNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ErpOrderId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SH_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SH_LINE_RII_SH_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_SH_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_SH_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_SH_TERMINAL_LINE",
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
                    table.PrimaryKey("PK_RII_SH_TERMINAL_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SH_TERMINAL_LINE_RII_SH_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_SH_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_SH_TERMINAL_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_TERMINAL_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_TERMINAL_LINE_RII_USERS_TerminalUserId",
                        column: x => x.TerminalUserId,
                        principalTable: "RII_USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RII_SH_TERMINAL_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PR_IMPORT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PR_IMPORT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PR_IMPORT_LINE_RII_PR_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_PR_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_PR_IMPORT_LINE_RII_PR_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_PR_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_PR_IMPORT_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_IMPORT_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_IMPORT_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PR_LINE_SERIAL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PR_LINE_SERIAL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PR_LINE_SERIAL_RII_PR_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_PR_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_PR_LINE_SERIAL_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_LINE_SERIAL_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_LINE_SERIAL_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_SH_IMPORT_LINE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeaderId = table.Column<long>(type: "bigint", nullable: false),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    StockCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YapKod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SH_IMPORT_LINE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SH_IMPORT_LINE_RII_SH_HEADER_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "RII_SH_HEADER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_SH_IMPORT_LINE_RII_SH_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_SH_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_SH_IMPORT_LINE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_IMPORT_LINE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_IMPORT_LINE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_SH_LINE_SERIAL",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SH_LINE_SERIAL", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SH_LINE_SERIAL_RII_SH_LINE_LineId",
                        column: x => x.LineId,
                        principalTable: "RII_SH_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_SH_LINE_SERIAL_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_LINE_SERIAL_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_LINE_SERIAL_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_PR_ROUTE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportLineId = table.Column<long>(type: "bigint", nullable: false),
                    PrImportLineId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    ScannedBarcode = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceWarehouse = table.Column<int>(type: "int", nullable: true),
                    TargetWarehouse = table.Column<int>(type: "int", nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_PR_ROUTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_PR_ROUTE_RII_PR_IMPORT_LINE_ImportLineId",
                        column: x => x.ImportLineId,
                        principalTable: "RII_PR_IMPORT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_PR_ROUTE_RII_PR_IMPORT_LINE_PrImportLineId",
                        column: x => x.PrImportLineId,
                        principalTable: "RII_PR_IMPORT_LINE",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_ROUTE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_ROUTE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_PR_ROUTE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RII_SH_ROUTE",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportLineId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true),
                    DeletedBy = table.Column<long>(type: "bigint", nullable: true),
                    LineId = table.Column<long>(type: "bigint", nullable: true),
                    ScannedBarcode = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SerialNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo3 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo4 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SourceWarehouse = table.Column<int>(type: "int", nullable: true),
                    TargetWarehouse = table.Column<int>(type: "int", nullable: true),
                    SourceCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TargetCellCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RII_SH_ROUTE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RII_SH_ROUTE_RII_SH_IMPORT_LINE_ImportLineId",
                        column: x => x.ImportLineId,
                        principalTable: "RII_SH_IMPORT_LINE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RII_SH_ROUTE_RII_USERS_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_ROUTE_RII_USERS_DeletedBy",
                        column: x => x.DeletedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RII_SH_ROUTE_RII_USERS_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "RII_USERS",
                        principalColumn: "Id");
                });

            migrationBuilder.Sql(@"
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_RII_PASSWORD_RESET_REQUEST_CreatedBy' AND object_id = OBJECT_ID('RII_PASSWORD_RESET_REQUEST'))
    CREATE INDEX [IX_RII_PASSWORD_RESET_REQUEST_CreatedBy] ON [RII_PASSWORD_RESET_REQUEST]([CreatedBy]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_RII_PASSWORD_RESET_REQUEST_DeletedBy' AND object_id = OBJECT_ID('RII_PASSWORD_RESET_REQUEST'))
    CREATE INDEX [IX_RII_PASSWORD_RESET_REQUEST_DeletedBy] ON [RII_PASSWORD_RESET_REQUEST]([DeletedBy]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_RII_PASSWORD_RESET_REQUEST_UpdatedBy' AND object_id = OBJECT_ID('RII_PASSWORD_RESET_REQUEST'))
    CREATE INDEX [IX_RII_PASSWORD_RESET_REQUEST_UpdatedBy] ON [RII_PASSWORD_RESET_REQUEST]([UpdatedBy]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_RII_PASSWORD_RESET_REQUEST_ExpiresAt' AND object_id = OBJECT_ID('RII_PASSWORD_RESET_REQUEST'))
    CREATE INDEX [IX_RII_PASSWORD_RESET_REQUEST_ExpiresAt] ON [RII_PASSWORD_RESET_REQUEST]([ExpiresAt]);
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_RII_PASSWORD_RESET_REQUEST_UserId_TokenHash' AND object_id = OBJECT_ID('RII_PASSWORD_RESET_REQUEST'))
    CREATE INDEX [IX_RII_PASSWORD_RESET_REQUEST_UserId_TokenHash] ON [RII_PASSWORD_RESET_REQUEST]([UserId], [TokenHash]);
");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_BranchCode",
                table: "RII_PR_HEADER",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_CustomerCode",
                table: "RII_PR_HEADER",
                column: "CustomerCode");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_IsDeleted",
                table: "RII_PR_HEADER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_PlannedDate",
                table: "RII_PR_HEADER",
                column: "PlannedDate");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_ProjectCode",
                table: "RII_PR_HEADER",
                column: "ProjectCode");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_StockCode",
                table: "RII_PR_HEADER",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_YapKod",
                table: "RII_PR_HEADER",
                column: "YapKod");

            migrationBuilder.CreateIndex(
                name: "IX_PrHeader_YearCode",
                table: "RII_PR_HEADER",
                column: "YearCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_HEADER_CreatedBy",
                table: "RII_PR_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_HEADER_DeletedBy",
                table: "RII_PR_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_HEADER_UpdatedBy",
                table: "RII_PR_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrImportLine_HeaderId",
                table: "RII_PR_IMPORT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PrImportLine_IsDeleted",
                table: "RII_PR_IMPORT_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PrImportLine_LineId",
                table: "RII_PR_IMPORT_LINE",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_PrImportLine_StockCode",
                table: "RII_PR_IMPORT_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_IMPORT_LINE_CreatedBy",
                table: "RII_PR_IMPORT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_IMPORT_LINE_DeletedBy",
                table: "RII_PR_IMPORT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_IMPORT_LINE_UpdatedBy",
                table: "RII_PR_IMPORT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrLine_ErpOrderNo",
                table: "RII_PR_LINE",
                column: "ErpOrderNo");

            migrationBuilder.CreateIndex(
                name: "IX_PrLine_HeaderId",
                table: "RII_PR_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PrLine_IsDeleted",
                table: "RII_PR_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PrLine_StockCode",
                table: "RII_PR_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_LINE_CreatedBy",
                table: "RII_PR_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_LINE_DeletedBy",
                table: "RII_PR_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_LINE_UpdatedBy",
                table: "RII_PR_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrLineSerial_IsDeleted",
                table: "RII_PR_LINE_SERIAL",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PrLineSerial_LineId",
                table: "RII_PR_LINE_SERIAL",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_LINE_SERIAL_CreatedBy",
                table: "RII_PR_LINE_SERIAL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_LINE_SERIAL_DeletedBy",
                table: "RII_PR_LINE_SERIAL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_LINE_SERIAL_UpdatedBy",
                table: "RII_PR_LINE_SERIAL",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrRoute_ImportLineId",
                table: "RII_PR_ROUTE",
                column: "ImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_PrRoute_IsDeleted",
                table: "RII_PR_ROUTE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PrRoute_SerialNo",
                table: "RII_PR_ROUTE",
                column: "SerialNo");

            migrationBuilder.CreateIndex(
                name: "IX_PrRoute_SourceWarehouse",
                table: "RII_PR_ROUTE",
                column: "SourceWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_PrRoute_TargetWarehouse",
                table: "RII_PR_ROUTE",
                column: "TargetWarehouse");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_ROUTE_CreatedBy",
                table: "RII_PR_ROUTE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_ROUTE_DeletedBy",
                table: "RII_PR_ROUTE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_ROUTE_PrImportLineId",
                table: "RII_PR_ROUTE",
                column: "PrImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_ROUTE_UpdatedBy",
                table: "RII_PR_ROUTE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PrTerminalLine_HeaderId",
                table: "RII_PR_TERMINAL_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_PrTerminalLine_IsDeleted",
                table: "RII_PR_TERMINAL_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_PrTerminalLine_TerminalUserId",
                table: "RII_PR_TERMINAL_LINE",
                column: "TerminalUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_TERMINAL_LINE_CreatedBy",
                table: "RII_PR_TERMINAL_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_TERMINAL_LINE_DeletedBy",
                table: "RII_PR_TERMINAL_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_TERMINAL_LINE_PrHeaderId",
                table: "RII_PR_TERMINAL_LINE",
                column: "PrHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_PR_TERMINAL_LINE_UpdatedBy",
                table: "RII_PR_TERMINAL_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_HEADER_CreatedBy",
                table: "RII_SH_HEADER",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_HEADER_DeletedBy",
                table: "RII_SH_HEADER",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_HEADER_UpdatedBy",
                table: "RII_SH_HEADER",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ShHeader_BranchCode",
                table: "RII_SH_HEADER",
                column: "BranchCode");

            migrationBuilder.CreateIndex(
                name: "IX_ShHeader_CustomerCode",
                table: "RII_SH_HEADER",
                column: "CustomerCode");

            migrationBuilder.CreateIndex(
                name: "IX_ShHeader_IsDeleted",
                table: "RII_SH_HEADER",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ShHeader_PlannedDate",
                table: "RII_SH_HEADER",
                column: "PlannedDate");

            migrationBuilder.CreateIndex(
                name: "IX_ShHeader_YearCode",
                table: "RII_SH_HEADER",
                column: "YearCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_IMPORT_LINE_CreatedBy",
                table: "RII_SH_IMPORT_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_IMPORT_LINE_DeletedBy",
                table: "RII_SH_IMPORT_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_IMPORT_LINE_UpdatedBy",
                table: "RII_SH_IMPORT_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ShImportLine_HeaderId",
                table: "RII_SH_IMPORT_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShImportLine_IsDeleted",
                table: "RII_SH_IMPORT_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ShImportLine_LineId",
                table: "RII_SH_IMPORT_LINE",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_ShImportLine_StockCode",
                table: "RII_SH_IMPORT_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_LINE_CreatedBy",
                table: "RII_SH_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_LINE_DeletedBy",
                table: "RII_SH_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_LINE_UpdatedBy",
                table: "RII_SH_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ShLine_HeaderId",
                table: "RII_SH_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShLine_IsDeleted",
                table: "RII_SH_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ShLine_StockCode",
                table: "RII_SH_LINE",
                column: "StockCode");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_LINE_SERIAL_CreatedBy",
                table: "RII_SH_LINE_SERIAL",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_LINE_SERIAL_DeletedBy",
                table: "RII_SH_LINE_SERIAL",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_LINE_SERIAL_UpdatedBy",
                table: "RII_SH_LINE_SERIAL",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ShLineSerial_IsDeleted",
                table: "RII_SH_LINE_SERIAL",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ShLineSerial_LineId",
                table: "RII_SH_LINE_SERIAL",
                column: "LineId");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_ROUTE_CreatedBy",
                table: "RII_SH_ROUTE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_ROUTE_DeletedBy",
                table: "RII_SH_ROUTE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_ROUTE_UpdatedBy",
                table: "RII_SH_ROUTE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ShRoute_ImportLineId",
                table: "RII_SH_ROUTE",
                column: "ImportLineId");

            migrationBuilder.CreateIndex(
                name: "IX_ShRoute_IsDeleted",
                table: "RII_SH_ROUTE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_TERMINAL_LINE_CreatedBy",
                table: "RII_SH_TERMINAL_LINE",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_TERMINAL_LINE_DeletedBy",
                table: "RII_SH_TERMINAL_LINE",
                column: "DeletedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RII_SH_TERMINAL_LINE_UpdatedBy",
                table: "RII_SH_TERMINAL_LINE",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_ShTerminalLine_HeaderId",
                table: "RII_SH_TERMINAL_LINE",
                column: "HeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ShTerminalLine_IsDeleted",
                table: "RII_SH_TERMINAL_LINE",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ShTerminalLine_TerminalUserId",
                table: "RII_SH_TERMINAL_LINE",
                column: "TerminalUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "RII_PASSWORD_RESET_REQUEST");

            migrationBuilder.DropTable(
                name: "RII_PR_LINE_SERIAL");

            migrationBuilder.DropTable(
                name: "RII_PR_ROUTE");

            migrationBuilder.DropTable(
                name: "RII_PR_TERMINAL_LINE");

            migrationBuilder.DropTable(
                name: "RII_SH_LINE_SERIAL");

            migrationBuilder.DropTable(
                name: "RII_SH_ROUTE");

            migrationBuilder.DropTable(
                name: "RII_SH_TERMINAL_LINE");

            migrationBuilder.DropTable(
                name: "RII_PR_IMPORT_LINE");

            migrationBuilder.DropTable(
                name: "RII_SH_IMPORT_LINE");

            migrationBuilder.DropTable(
                name: "RII_PR_LINE");

            migrationBuilder.DropTable(
                name: "RII_SH_LINE");

            migrationBuilder.DropTable(
                name: "RII_PR_HEADER");

            migrationBuilder.DropTable(
                name: "RII_SH_HEADER");
        }
    }
}
