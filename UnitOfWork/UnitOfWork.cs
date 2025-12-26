using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using WMS_WEBAPI.Data;
using WMS_WEBAPI.Models;
using WMS_WEBAPI.Repositories;

namespace WMS_WEBAPI.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WmsDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private bool _disposed = false;

        // Repository instances
        private IGenericRepository<User>? _users;
        private IGenericRepository<BaseEntity>? _baseEntities;
        private IGenericRepository<BaseHeaderEntity>? _baseHeaderEntities;
        
        // GoodReceipt repository instances
        private IGenericRepository<GrHeader>? _grHeaders;
        private IGenericRepository<GrLine>? _grLines;
        private IGenericRepository<GrImportDocument>? _grImportDocuments;
        private IGenericRepository<GrImportLine>? _grImportLines;
        private IGenericRepository<GrLineSerial>? _grLineSerials;
        private IGenericRepository<GrRoute>? _grRoutes;
        private IGenericRepository<GrTerminalLine>? _grTerminalLines;
        
        // User and Authority repository instances
        private IGenericRepository<UserAuthority>? _userAuthorities;
        
        // PlatformSidebar repository instances
        private IGenericRepository<PlatformPageGroup>? _platformPageGroups;
        private IGenericRepository<SidebarmenuHeader>? _sidebarmenuHeaders;
        private IGenericRepository<SidebarmenuLine>? _sidebarmenuLines;
        private IGenericRepository<PlatformUserGroupMatch>? _platformUserGroupMatches;
        
        // MobileSidebar repository instances
        private IGenericRepository<MobilePageGroup>? _mobilePageGroups;
        private IGenericRepository<MobileUserGroupMatch>? _mobileUserGroupMatches;
        private IGenericRepository<MobilemenuHeader>? _mobilemenuHeaders;
        private IGenericRepository<MobilemenuLine>? _mobilemenuLines;
        
        // WarehouseTransfer repository instances
        private IGenericRepository<WtLine>? _wtLines;
        private IGenericRepository<WtHeader>? _wtHeaders;
        private IGenericRepository<WtRoute>? _wtRoutes;
        private IGenericRepository<WtTerminalLine>? _wtTerminalLines;
        private IGenericRepository<WtImportLine>? _wtImportLines;
        private IGenericRepository<WtLineSerial>? _wtLineSerials;

        // ProductTransfer repository instances
        private IGenericRepository<PtHeader>? _ptHeaders;
        private IGenericRepository<PtLine>? _ptLines;
        private IGenericRepository<PtImportLine>? _ptImportLines;
        private IGenericRepository<PtRoute>? _ptRoutes;
        private IGenericRepository<PtTerminalLine>? _ptTerminalLines;
        private IGenericRepository<PtLineSerial>? _ptLineSerials;

        // Production repository instances
        private IGenericRepository<PrHeader>? _prHeaders;
        private IGenericRepository<PrLine>? _prLines;
        private IGenericRepository<PrImportLine>? _prImportLines;
        private IGenericRepository<PrRoute>? _prRoutes;
        private IGenericRepository<PrTerminalLine>? _prTerminalLines;
        private IGenericRepository<PrLineSerial>? _prLineSerials;
        private IGenericRepository<PrHeaderSerial>? _prHeaderSerials;

        // SubcontractingIssueTransfer repository instances
        private IGenericRepository<SitHeader>? _sitHeaders;
        private IGenericRepository<SitLine>? _sitLines;
        private IGenericRepository<SitImportLine>? _sitImportLines;
        private IGenericRepository<SitRoute>? _sitRoutes;
        private IGenericRepository<SitTerminalLine>? _sitTerminalLines;
        private IGenericRepository<SitLineSerial>? _sitLineSerials;

        // SubcontractingReceiptTransfer repository instances
        private IGenericRepository<SrtHeader>? _srtHeaders;
        private IGenericRepository<SrtLine>? _srtLines;
        private IGenericRepository<SrtImportLine>? _srtImportLines;
        private IGenericRepository<SrtRoute>? _srtRoutes;
        private IGenericRepository<SrtTerminalLine>? _srtTerminalLines;
        private IGenericRepository<SrtLineSerial>? _srtLineSerials;
        private IGenericRepository<WoHeader>? _woHeaders;
        private IGenericRepository<WoLine>? _woLines;
        private IGenericRepository<WoImportLine>? _woImportLines;
        private IGenericRepository<WoRoute>? _woRoutes;
        private IGenericRepository<WoTerminalLine>? _woTerminalLines;
        private IGenericRepository<WoLineSerial>? _woLineSerials;
        private IGenericRepository<WiHeader>? _wiHeaders;
        private IGenericRepository<WiLine>? _wiLines;
        private IGenericRepository<WiImportLine>? _wiImportLines;
        private IGenericRepository<WiRoute>? _wiRoutes;
        private IGenericRepository<WiTerminalLine>? _wiTerminalLines;
        private IGenericRepository<WiLineSerial>? _wiLineSerials;
        private IGenericRepository<ShHeader>? _shHeaders;
        private IGenericRepository<ShLine>? _shLines;
        private IGenericRepository<ShImportLine>? _shImportLines;
        private IGenericRepository<ShRoute>? _shRoutes;
        private IGenericRepository<ShTerminalLine>? _shTerminalLines;
        private IGenericRepository<ShLineSerial>? _shLineSerials;

        // InventoryCount repository instances
        private IGenericRepository<IcHeader>? _icHeaders;
        private IGenericRepository<IcImportLine>? _icImportLines;
        private IGenericRepository<IcRoute>? _icRoutes;
        private IGenericRepository<IcTerminalLine>? _icTerminalLines;
        private IGenericRepository<Notification>? _notifications;

        // Package repository instances
        private IGenericRepository<PHeader>? _pHeaders;
        private IGenericRepository<PPackage>? _pPackages;
        private IGenericRepository<PLine>? _pLines;

        // Parameter repository instances
        private IGenericRepository<GrParameter>? _grParameters;
        private IGenericRepository<WtParameter>? _wtParameters;
        private IGenericRepository<WoParameter>? _woParameters;
        private IGenericRepository<WiParameter>? _wiParameters;
        private IGenericRepository<ShParameter>? _shParameters;
        private IGenericRepository<SrtParameter>? _srtParameters;
        private IGenericRepository<SitParameter>? _sitParameters;
        private IGenericRepository<PtParameter>? _ptParameters;
        private IGenericRepository<PrParameter>? _prParameters;
        private IGenericRepository<IcParameter>? _icParameters;
        private IGenericRepository<PParameter>? _pParameters;

        public UnitOfWork(WmsDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        // Repository properties

        public IGenericRepository<User> Users =>
            _users ??= new GenericRepository<User>(_context, _httpContextAccessor);

        public IGenericRepository<BaseEntity> BaseEntities =>
            _baseEntities ??= new GenericRepository<BaseEntity>(_context, _httpContextAccessor);

        public IGenericRepository<BaseHeaderEntity> BaseHeaderEntities =>
            _baseHeaderEntities ??= new GenericRepository<BaseHeaderEntity>(_context, _httpContextAccessor);

        // GoodReceipt repository properties
        public IGenericRepository<GrHeader> GrHeaders =>
            _grHeaders ??= new GenericRepository<GrHeader>(_context, _httpContextAccessor);

        public IGenericRepository<GrLine> GrLines =>
            _grLines ??= new GenericRepository<GrLine>(_context, _httpContextAccessor);

        public IGenericRepository<GrImportDocument> GrImportDocuments =>
            _grImportDocuments ??= new GenericRepository<GrImportDocument>(_context, _httpContextAccessor);

        public IGenericRepository<GrImportLine> GrImportLines =>
            _grImportLines ??= new GenericRepository<GrImportLine>(_context, _httpContextAccessor);

        public IGenericRepository<GrLineSerial> GrLineSerials =>
            _grLineSerials ??= new GenericRepository<GrLineSerial>(_context, _httpContextAccessor);

        public IGenericRepository<GrRoute> GrRoutes =>
            _grRoutes ??= new GenericRepository<GrRoute>(_context, _httpContextAccessor);

        public IGenericRepository<GrTerminalLine> GrTerminalLines =>
            _grTerminalLines ??= new GenericRepository<GrTerminalLine>(_context, _httpContextAccessor);
        
        // User and Authority repository properties
        public IGenericRepository<UserAuthority> UserAuthorities =>
            _userAuthorities ??= new GenericRepository<UserAuthority>(_context, _httpContextAccessor);

        // PlatformSidebar repository properties
        public IGenericRepository<PlatformPageGroup> PlatformPageGroups =>
            _platformPageGroups ??= new GenericRepository<PlatformPageGroup>(_context, _httpContextAccessor);

        public IGenericRepository<SidebarmenuHeader> SidebarmenuHeaders =>
            _sidebarmenuHeaders ??= new GenericRepository<SidebarmenuHeader>(_context, _httpContextAccessor);

        public IGenericRepository<SidebarmenuLine> SidebarmenuLines =>
            _sidebarmenuLines ??= new GenericRepository<SidebarmenuLine>(_context, _httpContextAccessor);

        public IGenericRepository<PlatformUserGroupMatch> PlatformUserGroupMatches =>
            _platformUserGroupMatches ??= new GenericRepository<PlatformUserGroupMatch>(_context, _httpContextAccessor);

        // MobileSidebar repository properties
        public IGenericRepository<MobilePageGroup> MobilePageGroups =>
            _mobilePageGroups ??= new GenericRepository<MobilePageGroup>(_context, _httpContextAccessor);

        public IGenericRepository<MobileUserGroupMatch> MobileUserGroupMatches =>
            _mobileUserGroupMatches ??= new GenericRepository<MobileUserGroupMatch>(_context, _httpContextAccessor);

        public IGenericRepository<MobilemenuHeader> MobilemenuHeaders =>
            _mobilemenuHeaders ??= new GenericRepository<MobilemenuHeader>(_context, _httpContextAccessor);

        public IGenericRepository<MobilemenuLine> MobilemenuLines =>
            _mobilemenuLines ??= new GenericRepository<MobilemenuLine>(_context, _httpContextAccessor);

        // WarehouseTransfer repository properties
        public IGenericRepository<WtLine> WtLines =>
            _wtLines ??= new GenericRepository<WtLine>(_context, _httpContextAccessor);

        public IGenericRepository<WtHeader> WtHeaders =>
            _wtHeaders ??= new GenericRepository<WtHeader>(_context, _httpContextAccessor);

        public IGenericRepository<WtRoute> WtRoutes =>
            _wtRoutes ??= new GenericRepository<WtRoute>(_context, _httpContextAccessor);

        public IGenericRepository<WtTerminalLine> WtTerminalLines =>
            _wtTerminalLines ??= new GenericRepository<WtTerminalLine>(_context, _httpContextAccessor);

        public IGenericRepository<WtImportLine> WtImportLines =>
            _wtImportLines ??= new GenericRepository<WtImportLine>(_context, _httpContextAccessor);

        public IGenericRepository<WtLineSerial> WtLineSerials =>
            _wtLineSerials ??= new GenericRepository<WtLineSerial>(_context, _httpContextAccessor);

        // ProductTransfer repository properties
        public IGenericRepository<PtHeader> PtHeaders =>
            _ptHeaders ??= new GenericRepository<PtHeader>(_context, _httpContextAccessor);

        public IGenericRepository<PtLine> PtLines =>
            _ptLines ??= new GenericRepository<PtLine>(_context, _httpContextAccessor);

        public IGenericRepository<PtImportLine> PtImportLines =>
            _ptImportLines ??= new GenericRepository<PtImportLine>(_context, _httpContextAccessor);

        public IGenericRepository<PtRoute> PtRoutes =>
            _ptRoutes ??= new GenericRepository<PtRoute>(_context, _httpContextAccessor);

        public IGenericRepository<PtTerminalLine> PtTerminalLines =>
            _ptTerminalLines ??= new GenericRepository<PtTerminalLine>(_context, _httpContextAccessor);

        public IGenericRepository<PtLineSerial> PtLineSerials =>
            _ptLineSerials ??= new GenericRepository<PtLineSerial>(_context, _httpContextAccessor);

        // Production repository properties
        public IGenericRepository<PrHeader> PrHeaders =>
            _prHeaders ??= new GenericRepository<PrHeader>(_context, _httpContextAccessor);

        public IGenericRepository<PrLine> PrLines =>
            _prLines ??= new GenericRepository<PrLine>(_context, _httpContextAccessor);

        public IGenericRepository<PrImportLine> PrImportLines =>
            _prImportLines ??= new GenericRepository<PrImportLine>(_context, _httpContextAccessor);

        public IGenericRepository<PrRoute> PrRoutes =>
            _prRoutes ??= new GenericRepository<PrRoute>(_context, _httpContextAccessor);

        public IGenericRepository<PrTerminalLine> PrTerminalLines =>
            _prTerminalLines ??= new GenericRepository<PrTerminalLine>(_context, _httpContextAccessor);

        public IGenericRepository<PrLineSerial> PrLineSerials =>
            _prLineSerials ??= new GenericRepository<PrLineSerial>(_context, _httpContextAccessor);

        public IGenericRepository<PrHeaderSerial> PrHeaderSerials =>
            _prHeaderSerials ??= new GenericRepository<PrHeaderSerial>(_context, _httpContextAccessor);

        // SubcontractingIssueTransfer repository properties
        public IGenericRepository<SitHeader> SitHeaders =>
            _sitHeaders ??= new GenericRepository<SitHeader>(_context, _httpContextAccessor);

        public IGenericRepository<SitLine> SitLines =>
            _sitLines ??= new GenericRepository<SitLine>(_context, _httpContextAccessor);

        public IGenericRepository<SitImportLine> SitImportLines =>
            _sitImportLines ??= new GenericRepository<SitImportLine>(_context, _httpContextAccessor);

        public IGenericRepository<SitRoute> SitRoutes =>
            _sitRoutes ??= new GenericRepository<SitRoute>(_context, _httpContextAccessor);

        public IGenericRepository<SitTerminalLine> SitTerminalLines =>
            _sitTerminalLines ??= new GenericRepository<SitTerminalLine>(_context, _httpContextAccessor);

        public IGenericRepository<SitLineSerial> SitLineSerials =>
            _sitLineSerials ??= new GenericRepository<SitLineSerial>(_context, _httpContextAccessor);

        // SubcontractingReceiptTransfer repository properties
        public IGenericRepository<SrtHeader> SrtHeaders =>
            _srtHeaders ??= new GenericRepository<SrtHeader>(_context, _httpContextAccessor);

        public IGenericRepository<SrtLine> SrtLines =>
            _srtLines ??= new GenericRepository<SrtLine>(_context, _httpContextAccessor);

        public IGenericRepository<SrtImportLine> SrtImportLines =>
            _srtImportLines ??= new GenericRepository<SrtImportLine>(_context, _httpContextAccessor);

        public IGenericRepository<SrtRoute> SrtRoutes =>
            _srtRoutes ??= new GenericRepository<SrtRoute>(_context, _httpContextAccessor);

        public IGenericRepository<SrtTerminalLine> SrtTerminalLines =>
            _srtTerminalLines ??= new GenericRepository<SrtTerminalLine>(_context, _httpContextAccessor);

        public IGenericRepository<SrtLineSerial> SrtLineSerials =>
            _srtLineSerials ??= new GenericRepository<SrtLineSerial>(_context, _httpContextAccessor);

        // WarehouseOutbound repository properties
        public IGenericRepository<WoHeader> WoHeaders =>
            _woHeaders ??= new GenericRepository<WoHeader>(_context, _httpContextAccessor);

        public IGenericRepository<WoLine> WoLines =>
            _woLines ??= new GenericRepository<WoLine>(_context, _httpContextAccessor);

        public IGenericRepository<WoImportLine> WoImportLines =>
            _woImportLines ??= new GenericRepository<WoImportLine>(_context, _httpContextAccessor);

        public IGenericRepository<WoRoute> WoRoutes =>
            _woRoutes ??= new GenericRepository<WoRoute>(_context, _httpContextAccessor);

        public IGenericRepository<WoTerminalLine> WoTerminalLines =>
            _woTerminalLines ??= new GenericRepository<WoTerminalLine>(_context, _httpContextAccessor);

        public IGenericRepository<WoLineSerial> WoLineSerials =>
            _woLineSerials ??= new GenericRepository<WoLineSerial>(_context, _httpContextAccessor);

        // WarehouseInbound repository properties
        public IGenericRepository<WiHeader> WiHeaders =>
            _wiHeaders ??= new GenericRepository<WiHeader>(_context, _httpContextAccessor);

        public IGenericRepository<WiLine> WiLines =>
            _wiLines ??= new GenericRepository<WiLine>(_context, _httpContextAccessor);

        public IGenericRepository<WiImportLine> WiImportLines =>
            _wiImportLines ??= new GenericRepository<WiImportLine>(_context, _httpContextAccessor);

        public IGenericRepository<WiRoute> WiRoutes =>
            _wiRoutes ??= new GenericRepository<WiRoute>(_context, _httpContextAccessor);

        public IGenericRepository<WiTerminalLine> WiTerminalLines =>
            _wiTerminalLines ??= new GenericRepository<WiTerminalLine>(_context, _httpContextAccessor);

        public IGenericRepository<WiLineSerial> WiLineSerials =>
            _wiLineSerials ??= new GenericRepository<WiLineSerial>(_context, _httpContextAccessor);

        // Shipping repository properties
        public IGenericRepository<ShHeader> ShHeaders =>
            _shHeaders ??= new GenericRepository<ShHeader>(_context, _httpContextAccessor);

        public IGenericRepository<ShLine> ShLines =>
            _shLines ??= new GenericRepository<ShLine>(_context, _httpContextAccessor);

        public IGenericRepository<ShImportLine> ShImportLines =>
            _shImportLines ??= new GenericRepository<ShImportLine>(_context, _httpContextAccessor);

        public IGenericRepository<ShRoute> ShRoutes =>
            _shRoutes ??= new GenericRepository<ShRoute>(_context, _httpContextAccessor);

        public IGenericRepository<ShTerminalLine> ShTerminalLines =>
            _shTerminalLines ??= new GenericRepository<ShTerminalLine>(_context, _httpContextAccessor);

        public IGenericRepository<ShLineSerial> ShLineSerials =>
            _shLineSerials ??= new GenericRepository<ShLineSerial>(_context, _httpContextAccessor);

        // InventoryCount repository properties
        public IGenericRepository<IcHeader> ICHeaders =>
            _icHeaders ??= new GenericRepository<IcHeader>(_context, _httpContextAccessor);

        public IGenericRepository<IcImportLine> IcImportLines =>
            _icImportLines ??= new GenericRepository<IcImportLine>(_context, _httpContextAccessor);

        public IGenericRepository<IcRoute> IcRoutes =>
            _icRoutes ??= new GenericRepository<IcRoute>(_context, _httpContextAccessor);

        public IGenericRepository<IcTerminalLine> IcTerminalLines =>
            _icTerminalLines ??= new GenericRepository<IcTerminalLine>(_context, _httpContextAccessor);

        public IGenericRepository<Notification> Notifications =>
            _notifications ??= new GenericRepository<Notification>(_context, _httpContextAccessor);

        // Package repository properties
        public IGenericRepository<PHeader> PHeaders =>
            _pHeaders ??= new GenericRepository<PHeader>(_context, _httpContextAccessor);

        public IGenericRepository<PPackage> PPackages =>
            _pPackages ??= new GenericRepository<PPackage>(_context, _httpContextAccessor);

        public IGenericRepository<PLine> PLines =>
            _pLines ??= new GenericRepository<PLine>(_context, _httpContextAccessor);

        // Parameter repository properties
        public IGenericRepository<GrParameter> GrParameters =>
            _grParameters ??= new GenericRepository<GrParameter>(_context, _httpContextAccessor);

        public IGenericRepository<WtParameter> WtParameters =>
            _wtParameters ??= new GenericRepository<WtParameter>(_context, _httpContextAccessor);

        public IGenericRepository<WoParameter> WoParameters =>
            _woParameters ??= new GenericRepository<WoParameter>(_context, _httpContextAccessor);

        public IGenericRepository<WiParameter> WiParameters =>
            _wiParameters ??= new GenericRepository<WiParameter>(_context, _httpContextAccessor);

        public IGenericRepository<ShParameter> ShParameters =>
            _shParameters ??= new GenericRepository<ShParameter>(_context, _httpContextAccessor);

        public IGenericRepository<SrtParameter> SrtParameters =>
            _srtParameters ??= new GenericRepository<SrtParameter>(_context, _httpContextAccessor);

        public IGenericRepository<SitParameter> SitParameters =>
            _sitParameters ??= new GenericRepository<SitParameter>(_context, _httpContextAccessor);

        public IGenericRepository<PtParameter> PtParameters =>
            _ptParameters ??= new GenericRepository<PtParameter>(_context, _httpContextAccessor);

        public IGenericRepository<PrParameter> PrParameters =>
            _prParameters ??= new GenericRepository<PrParameter>(_context, _httpContextAccessor);

        public IGenericRepository<IcParameter> IcParameters =>
            _icParameters ??= new GenericRepository<IcParameter>(_context, _httpContextAccessor);

        public IGenericRepository<PParameter> PParameters =>
            _pParameters ??= new GenericRepository<PParameter>(_context, _httpContextAccessor);

        public async Task<long> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public long SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            var tx = _context.Database.CurrentTransaction;
            if (tx != null)
            {
                await tx.CommitAsync();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            var tx = _context.Database.CurrentTransaction;
            if (tx != null)
            {
                await tx.RollbackAsync();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
