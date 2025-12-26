using WMS_WEBAPI.Models;
using WMS_WEBAPI.Repositories;

using Microsoft.EntityFrameworkCore.Storage;

namespace WMS_WEBAPI.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // Repository properties
        IGenericRepository<User> Users { get; }
        IGenericRepository<BaseEntity> BaseEntities { get; }
        IGenericRepository<BaseHeaderEntity> BaseHeaderEntities { get; }
        
        // GoodReceipt repositories
        IGenericRepository<GrHeader> GrHeaders { get; }
        IGenericRepository<GrLine> GrLines { get; }
        IGenericRepository<GrImportDocument> GrImportDocuments { get; }
        IGenericRepository<GrImportLine> GrImportLines { get; }
        IGenericRepository<GrLineSerial> GrLineSerials { get; }
        IGenericRepository<GrRoute> GrRoutes { get; }
        IGenericRepository<GrTerminalLine> GrTerminalLines { get; }
        
        // User and Authority repositories
        IGenericRepository<UserAuthority> UserAuthorities { get; }
        
        // PlatformSidebar repositories
        IGenericRepository<PlatformPageGroup> PlatformPageGroups { get; }
        IGenericRepository<SidebarmenuHeader> SidebarmenuHeaders { get; }
        IGenericRepository<SidebarmenuLine> SidebarmenuLines { get; }
        IGenericRepository<PlatformUserGroupMatch> PlatformUserGroupMatches { get; }
        
        // MobileSidebar repositories
        IGenericRepository<MobilePageGroup> MobilePageGroups { get; }
        IGenericRepository<MobileUserGroupMatch> MobileUserGroupMatches { get; }
        IGenericRepository<MobilemenuHeader> MobilemenuHeaders { get; }
        IGenericRepository<MobilemenuLine> MobilemenuLines { get; }
        
        // WarehouseTransfer repositories
        IGenericRepository<WtLine> WtLines { get; }
        IGenericRepository<WtHeader> WtHeaders { get; }
        IGenericRepository<WtRoute> WtRoutes { get; }
        IGenericRepository<WtTerminalLine> WtTerminalLines { get; }
        IGenericRepository<WtImportLine> WtImportLines { get; }
        IGenericRepository<WtLineSerial> WtLineSerials { get; }

        // ProductTransfer repositories
        IGenericRepository<PtHeader> PtHeaders { get; }
        IGenericRepository<PtLine> PtLines { get; }
        IGenericRepository<PtImportLine> PtImportLines { get; }
        IGenericRepository<PtRoute> PtRoutes { get; }
        IGenericRepository<PtTerminalLine> PtTerminalLines { get; }
        IGenericRepository<PtLineSerial> PtLineSerials { get; }

        // Production repositories
        IGenericRepository<PrHeader> PrHeaders { get; }
        IGenericRepository<PrLine> PrLines { get; }
        IGenericRepository<PrImportLine> PrImportLines { get; }
        IGenericRepository<PrRoute> PrRoutes { get; }
        IGenericRepository<PrTerminalLine> PrTerminalLines { get; }
        IGenericRepository<PrLineSerial> PrLineSerials { get; }
        IGenericRepository<PrHeaderSerial> PrHeaderSerials { get; }

        // SubcontractingIssueTransfer Repositories
        IGenericRepository<SitHeader> SitHeaders { get; }
        IGenericRepository<SitLine> SitLines { get; }
        IGenericRepository<SitImportLine> SitImportLines { get; }
        IGenericRepository<SitRoute> SitRoutes { get; }
        IGenericRepository<SitTerminalLine> SitTerminalLines { get; }
        IGenericRepository<SitLineSerial> SitLineSerials { get; }

        // SubcontractingReceiptTransfer repositories
        IGenericRepository<SrtHeader> SrtHeaders { get; }
        IGenericRepository<SrtLine> SrtLines { get; }
        IGenericRepository<SrtImportLine> SrtImportLines { get; }
        IGenericRepository<SrtRoute> SrtRoutes { get; }
        IGenericRepository<SrtTerminalLine> SrtTerminalLines { get; }
        IGenericRepository<SrtLineSerial> SrtLineSerials { get; }

        // WarehouseOutbound repositories
        IGenericRepository<WoHeader> WoHeaders { get; }
        IGenericRepository<WoLine> WoLines { get; }
        IGenericRepository<WoImportLine> WoImportLines { get; }
        IGenericRepository<WoRoute> WoRoutes { get; }
        IGenericRepository<WoTerminalLine> WoTerminalLines { get; }
        IGenericRepository<WoLineSerial> WoLineSerials { get; }

        // WarehouseInbound repositories
        IGenericRepository<WiHeader> WiHeaders { get; }
        IGenericRepository<WiLine> WiLines { get; }
        IGenericRepository<WiImportLine> WiImportLines { get; }
        IGenericRepository<WiRoute> WiRoutes { get; }
        IGenericRepository<WiTerminalLine> WiTerminalLines { get; }
        IGenericRepository<WiLineSerial> WiLineSerials { get; }

        // Shipping repositories
        IGenericRepository<ShHeader> ShHeaders { get; }
        IGenericRepository<ShLine> ShLines { get; }
        IGenericRepository<ShImportLine> ShImportLines { get; }
        IGenericRepository<ShRoute> ShRoutes { get; }
        IGenericRepository<ShTerminalLine> ShTerminalLines { get; }
        IGenericRepository<ShLineSerial> ShLineSerials { get; }

        // InventoryCount repositories
        IGenericRepository<IcHeader> ICHeaders { get; }
        IGenericRepository<IcImportLine> IcImportLines { get; }
        IGenericRepository<IcRoute> IcRoutes { get; }
        IGenericRepository<IcTerminalLine> IcTerminalLines { get; }

        // Notification repositories
        IGenericRepository<Notification> Notifications { get; }

        // Package repositories
        IGenericRepository<PHeader> PHeaders { get; }
        IGenericRepository<PPackage> PPackages { get; }
        IGenericRepository<PLine> PLines { get; }

        // Parameter repositories
        IGenericRepository<GrParameter> GrParameters { get; }
        IGenericRepository<WtParameter> WtParameters { get; }
        IGenericRepository<WoParameter> WoParameters { get; }
        IGenericRepository<WiParameter> WiParameters { get; }
        IGenericRepository<ShParameter> ShParameters { get; }
        IGenericRepository<SrtParameter> SrtParameters { get; }
        IGenericRepository<SitParameter> SitParameters { get; }
        IGenericRepository<PtParameter> PtParameters { get; }
        IGenericRepository<PrParameter> PrParameters { get; }
        IGenericRepository<IcParameter> IcParameters { get; }
        IGenericRepository<PParameter> PParameters { get; }

        // Methods
        Task<long> SaveChangesAsync();
        long SaveChanges();

        // Transactions
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
