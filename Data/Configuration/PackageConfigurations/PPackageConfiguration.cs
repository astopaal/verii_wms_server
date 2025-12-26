using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PPackageConfiguration : BaseEntityConfiguration<PPackage>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PPackage> builder)
        {
            builder.ToTable("RII_P_PACKAGE");

            // Id için GUID kullanımı (isteğe bağlı - şu an BaseEntity'deki long Id kullanılıyor)
            // Eğer GUID gerekiyorsa, aşağıdaki satırları uncomment edin ve migration oluşturun:
            // builder.Property(x => x.Id)
            //     .HasColumnName("Id")
            //     .HasDefaultValueSql("NEWID()");

            builder.Property(x => x.PackingHeaderId)
                .IsRequired()
                .HasColumnName("PackingHeaderId");

            builder.HasOne(x => x.PackingHeader)
                .WithMany(x => x.Packages)
                .HasForeignKey(x => x.PackingHeaderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PPackage_PHeader");

            builder.Property(x => x.PackageNo)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("PackageNo");

            builder.Property(x => x.PackageType)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue(PPackageType.Box)
                .HasColumnName("PackageType");

            builder.Property(x => x.Barcode)
                .HasMaxLength(100)
                .HasColumnName("Barcode");

            builder.Property(x => x.Length)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("Length");

            builder.Property(x => x.Width)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("Width");

            builder.Property(x => x.Height)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("Height");

            builder.Property(x => x.Volume)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("Volume");

            builder.Property(x => x.NetWeight)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("NetWeight");

            builder.Property(x => x.TareWeight)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("TareWeight");

            builder.Property(x => x.GrossWeight)
                .HasColumnType("decimal(18,6)")
                .HasColumnName("GrossWeight");

            builder.Property(x => x.IsMixed)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("IsMixed");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue(PPackageStatus.Open)
                .HasColumnName("Status");

            builder.HasIndex(x => x.PackingHeaderId)
                .HasDatabaseName("IX_PPackage_PackingHeaderId");

            builder.HasIndex(x => x.PackageNo)
                .HasDatabaseName("IX_PPackage_PackageNo");

            builder.HasIndex(x => x.Barcode)
                .IsUnique()
                .HasFilter("[Barcode] IS NOT NULL")
                .HasDatabaseName("IX_PPackage_Barcode");

            builder.HasIndex(x => x.Status)
                .HasDatabaseName("IX_PPackage_Status");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PPackage_IsDeleted");
        }
    }
}

