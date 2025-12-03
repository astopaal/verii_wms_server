using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Data.Configuration;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data
{
    public class WmsDbContext : DbContext
    {
        public WmsDbContext(DbContextOptions<WmsDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSession> UserSessions { get; set; }
        public DbSet<GrHeader> GrHeaders { get; set; }
        public DbSet<GrLine> GrLines { get; set; }
        public DbSet<GrImportDocument> GrImportDocuments { get; set; }
        public DbSet<GrImportLine> GrImportLines { get; set; }
        public DbSet<GrLineSerial> GrLineSerials { get; set; }
        public DbSet<UserAuthority> UserAuthorities { get; set; }
        
        // MobileSidebar DbSets
        public DbSet<MobilePageGroup> MobilePageGroups { get; set; }
        public DbSet<MobileUserGroupMatch> MobileUserGroupMatches { get; set; }
        public DbSet<MobilemenuHeader> MobilemenuHeaders { get; set; }
        public DbSet<MobilemenuLine> MobilemenuLines { get; set; }
        
        // PlatformSidebar DbSets
        public DbSet<PlatformPageGroup> PlatformPageGroups { get; set; }
        public DbSet<PlatformUserGroupMatch> PlatformUserGroupMatches { get; set; }
        public DbSet<SidebarmenuHeader> SidebarmenuHeaders { get; set; }
        public DbSet<SidebarmenuLine> SidebarmenuLines { get; set; }
        
        // WarehouseTransfer DbSets
        public DbSet<WtHeader> WtHeaders { get; set; }
        public DbSet<WtImportLine> WtImportLines { get; set; }
        public DbSet<WtLine> WtLines { get; set; }
        public DbSet<WtRoute> WtRoutes { get; set; }
        public DbSet<WtTerminalLine> WtTerminalLines { get; set; }
        public DbSet<WtLineSerial> WtLineSerials { get; set; }

        //ProductTransfer DbSets
        public DbSet<PtHeader> PtHeaders { get; set; }
        public DbSet<PtLine> PtLines { get; set; }
        public DbSet<PtImportLine> PtImportLines { get; set; }
        public DbSet<PtRoute> PtRoutes { get; set; }
        public DbSet<PtTerminalLine> PtTerminalLines { get; set; }

        // SubcontractingIssueTransfer DbSets
        public DbSet<SitHeader> SitHeaders { get; set; }
        public DbSet<SitLine> SitLines { get; set; }
        public DbSet<SitImportLine> SitImportLines { get; set; }
        public DbSet<SitRoute> SitRoutes { get; set; }
        public DbSet<SitTerminalLine> SitTerminalLines { get; set; }

        // SubcontractingReceiptTransfer DbSets
        public DbSet<SrtHeader> SrtHeaders { get; set; }
        public DbSet<SrtLine> SrtLines { get; set; }
        public DbSet<SrtImportLine> SrtImportLines { get; set; }
        public DbSet<SrtRoute> SrtRoutes { get; set; }
        public DbSet<SrtTerminalLine> SrtTerminalLines { get; set; }

        // WarehouseOutbound DbSets
        public DbSet<WoHeader> WoHeaders { get; set; }
        public DbSet<WoLine> WoLines { get; set; }
        public DbSet<WoImportLine> WoImportLines { get; set; }
        public DbSet<WoRoute> WoRoutes { get; set; }
        public DbSet<WoTerminalLine> WoTerminalLines { get; set; }

        // WarehouseInbound DbSets
        public DbSet<WiHeader> WiHeaders { get; set; }
        public DbSet<WiLine> WiLines { get; set; }
        public DbSet<WiImportLine> WiImportLines { get; set; }
        public DbSet<WiRoute> WiRoutes { get; set; }
        public DbSet<WiTerminalLine> WiTerminalLines { get; set; }

        // InventoryCount DbSets
        public DbSet<IcHeader> ICHeaders { get; set; }
        public DbSet<IcImportLine> IcImportLines { get; set; }
        public DbSet<IcRoute> IcRoutes { get; set; }
        public DbSet<IcTerminalLine> IcTerminalLines { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserSessionConfiguration());
            modelBuilder.ApplyConfiguration(new GrHeaderConfiguration());
            modelBuilder.ApplyConfiguration(new GrLineConfiguration());
            modelBuilder.ApplyConfiguration(new GrImportDocumentConfiguration());
            modelBuilder.ApplyConfiguration(new GrImportLineConfiguration());
            modelBuilder.ApplyConfiguration(new GrLineSerialConfiguration());
            modelBuilder.ApplyConfiguration(new UserAuthorityConfiguration());

            // Apply MobileSidebar configurations
            modelBuilder.ApplyConfiguration(new MobilePageGroupConfiguration());
            modelBuilder.ApplyConfiguration(new MobileUserGroupMatchConfiguration());
            modelBuilder.ApplyConfiguration(new MobilemenuHeaderConfiguration());
            modelBuilder.ApplyConfiguration(new MobilemenuLineConfiguration());
            
            // Apply PlatformSidebar configurations
            modelBuilder.ApplyConfiguration(new PlatformPageGroupConfiguration());
            modelBuilder.ApplyConfiguration(new PlatformUserGroupMatchConfiguration());
            modelBuilder.ApplyConfiguration(new SidebarmenuHeaderConfiguration());
            modelBuilder.ApplyConfiguration(new SidebarmenuLineConfiguration());
            
            // WarehouseTransfer configurations temporarily disabled

            modelBuilder.ApplyConfiguration(new PtHeaderConfiguration());
            modelBuilder.ApplyConfiguration(new PtLineConfiguration());
            modelBuilder.ApplyConfiguration(new PtImportLineConfiguration());
            modelBuilder.ApplyConfiguration(new PtRouteConfiguration());
            modelBuilder.ApplyConfiguration(new PtTerminalLineConfiguration());

            // Apply SubcontractingIssueTransfer configurations
            modelBuilder.ApplyConfiguration(new SitHeaderConfiguration());
            modelBuilder.ApplyConfiguration(new SitLineConfiguration());
            modelBuilder.ApplyConfiguration(new SitImportLineConfiguration());
            modelBuilder.ApplyConfiguration(new SitRouteConfiguration());
            modelBuilder.ApplyConfiguration(new SitTerminalLineConfiguration());

            // Apply SubcontractingReceiptTransfer configurations
            modelBuilder.ApplyConfiguration(new SrtHeaderConfiguration());
            modelBuilder.ApplyConfiguration(new SrtLineConfiguration());
            modelBuilder.ApplyConfiguration(new SrtImportLineConfiguration());
            modelBuilder.ApplyConfiguration(new SrtRouteConfiguration());
            modelBuilder.ApplyConfiguration(new SrtTerminalLineConfiguration());

            // WarehouseOutbound configurations
            modelBuilder.ApplyConfiguration(new WoHeaderConfiguration());
            modelBuilder.ApplyConfiguration(new WoLineConfiguration());
            modelBuilder.ApplyConfiguration(new WoImportLineConfiguration());
            modelBuilder.ApplyConfiguration(new WoRouteConfiguration());
            modelBuilder.ApplyConfiguration(new WoTerminalLineConfiguration());

            // WarehouseInbound configurations
            modelBuilder.ApplyConfiguration(new WiHeaderConfiguration());
            modelBuilder.ApplyConfiguration(new WiLineConfiguration());
            modelBuilder.ApplyConfiguration(new WiImportLineConfiguration());
            modelBuilder.ApplyConfiguration(new WiRouteConfiguration());
            modelBuilder.ApplyConfiguration(new WiTerminalLineConfiguration());

            modelBuilder.ApplyConfiguration(new NotificationConfiguration());

            // InventoryCount configurations temporarily disabled
                        
            // GoodReciptFunctions - Function olduğu için HasNoKey kullanıyoruz
            modelBuilder.Entity<FN_GoodsOpenOrders_Header>().HasNoKey().ToFunction("RII_FN_GR_OPENORDERS_HEADER");
            modelBuilder.Entity<FN_GoodsOpenOrders_Line>().HasNoKey().ToFunction("RII_FN_GR_OPENORDERS_LINE");

            // WtFunctions temporarily disabled
            modelBuilder.Entity<FN_TransferOpenOrder_Header>(entity =>
            {
                entity.HasNoKey();
            });

        }
    }
}
