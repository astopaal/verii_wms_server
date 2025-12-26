using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PHeaderConfiguration : BaseEntityConfiguration<PHeader>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PHeader> builder)
        {
            builder.ToTable("RII_P_HEADER");

            // Id için GUID kullanımı (isteğe bağlı - şu an BaseEntity'deki long Id kullanılıyor)
            // Eğer GUID gerekiyorsa, aşağıdaki satırları uncomment edin ve migration oluşturun:
            // builder.Property(x => x.Id)
            //     .HasColumnName("Id")
            //     .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.WarehouseCode)
                .HasMaxLength(20)
                .HasColumnName("WarehouseCode");

            builder.Property(x => x.PackingNo)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("PackingNo");

            builder.Property(x => x.PackingDate)
                .HasColumnName("PackingDate");

            builder.Property(x => x.SourceType)
                .HasMaxLength(30)
                .HasColumnName("SourceType");

            builder.Property(x => x.SourceHeaderId)
                .HasColumnName("SourceHeaderId");

            builder.Property(x => x.CustomerCode)
                .HasMaxLength(50)
                .HasColumnName("CustomerCode");

            builder.Property(x => x.CustomerAddress)
                .HasMaxLength(255)
                .HasColumnName("CustomerAddress");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue(PHeaderStatus.Draft)
                .HasColumnName("Status");

            builder.Property(x => x.TotalPackageCount)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("TotalPackageCount");

            builder.Property(x => x.TotalQuantity)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("TotalQuantity");

            builder.Property(x => x.TotalNetWeight)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("TotalNetWeight");

            builder.Property(x => x.TotalGrossWeight)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("TotalGrossWeight");

            builder.Property(x => x.TotalVolume)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("TotalVolume");

            builder.Property(x => x.CarrierId)
                .HasColumnName("CarrierId");

            builder.Property(x => x.CarrierServiceType)
                .HasMaxLength(20)
                .HasColumnName("CarrierServiceType");

            builder.Property(x => x.TrackingNo)
                .HasMaxLength(100)
                .HasColumnName("TrackingNo");

            builder.HasIndex(x => x.PackingNo)
                .IsUnique()
                .HasDatabaseName("IX_PHeader_PackingNo");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_PHeader_Status");

            builder.HasIndex(x => x.WarehouseCode)
                .HasDatabaseName("IX_PHeader_WarehouseCode");

            builder.HasIndex(x => x.SourceHeaderId)
                .HasDatabaseName("IX_PHeader_SourceHeaderId");

            builder.HasIndex(x => x.CustomerCode)
                .HasDatabaseName("IX_PHeader_CustomerCode");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PHeader_IsDeleted");
        }
    }
}