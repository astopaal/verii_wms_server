using Microsoft.EntityFrameworkCore;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data
{
    public class ErpDbContext : DbContext
    {
        public ErpDbContext(DbContextOptions<ErpDbContext> options) : base(options) { }

        // ERP DbSet'leri
        public DbSet<RII_FN_ONHANDQUANTITY> OnHandQuantities { get; set; }
        public DbSet<RII_VW_CARI> Caris { get; set; }
        public DbSet<RII_VW_STOK> Stoks { get; set; }
        public DbSet<RII_VW_DEPO> Depos { get; set; }
        public DbSet<RII_VW_PROJE> Projeler { get; set; }
        public DbSet<RII_FN_OPENGOODSFORORDERBYCUSTOMER> CustomerOrders { get; set; }
        public DbSet<RII_FN_OPENGOODSFORORDERDETAILBYORDERNUMBERS> OrderDetails { get; set; }
        public DbSet<RII_FN_STOKBARCODE> StokBarcodes { get; set; }
        public DbSet<RII_FN_BRANCHES> Branches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ERP entity konfigürasyonları
            // Bu modeller view ve function olduğu için key tanımlamaları gerekli
            modelBuilder.Entity<RII_FN_ONHANDQUANTITY>().HasNoKey().ToView("RII_FN_ONHANDQUANTITY");
            modelBuilder.Entity<RII_VW_CARI>().HasNoKey().ToView("RII_VW_CARI");
            modelBuilder.Entity<RII_VW_STOK>().HasNoKey();
            modelBuilder.Entity<RII_VW_DEPO>().HasNoKey();
            modelBuilder.Entity<RII_VW_PROJE>().HasNoKey().ToView("RII_VW_PROJE");
            modelBuilder.Entity<RII_FN_OPENGOODSFORORDERBYCUSTOMER>().HasNoKey();
            modelBuilder.Entity<RII_FN_OPENGOODSFORORDERDETAILBYORDERNUMBERS>().HasNoKey();
            modelBuilder.Entity<RII_FN_STOKBARCODE>().HasNoKey();
            modelBuilder.Entity<RII_FN_BRANCHES>().HasNoKey();

        }
    }
}
