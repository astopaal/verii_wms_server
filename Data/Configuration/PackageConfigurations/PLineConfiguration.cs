using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PLineConfiguration : BaseEntityConfiguration<PLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PLine> builder)
        {
            builder.ToTable("RII_P_LINE");

            builder.Property(x => x.PackingHeaderId)
                .IsRequired()
                .HasColumnName("PackingHeaderId");

            builder.HasOne(x => x.PackingHeader)
                .WithMany()
                .HasForeignKey(x => x.PackingHeaderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PLine_PHeader");

            builder.Property(x => x.PackageId)
                .IsRequired()
                .HasColumnName("PackageId");

            builder.HasOne(x => x.Package)
                .WithMany(x => x.Lines)
                .HasForeignKey(x => x.PackageId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PLine_PPackage");

            builder.Property(x => x.Barcode)
                .HasMaxLength(50)
                .HasColumnName("Barcode");

            builder.Property(x => x.StockCode)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("StockCode");

            builder.Property(x => x.YapKod)
                .HasMaxLength(50)
                .HasColumnName("YapKod");

            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasColumnType("decimal(18,6)")
                .HasColumnName("Quantity");

            builder.Property(x => x.SerialNo)
                .HasMaxLength(50)
                .HasColumnName("SerialNo");

            builder.Property(x => x.SerialNo2)
                .HasMaxLength(50)
                .HasColumnName("SerialNo2");

            builder.Property(x => x.SerialNo3)
                .HasMaxLength(50)
                .HasColumnName("SerialNo3");

            builder.Property(x => x.SerialNo4)
                .HasMaxLength(50)
                .HasColumnName("SerialNo4");

            builder.Property(x => x.SourceRouteId)
                .HasColumnName("SourceRouteId");

            builder.HasIndex(x => x.PackingHeaderId)
                .HasDatabaseName("IX_PLine_PackingHeaderId");

            builder.HasIndex(x => x.PackageId)
                .HasDatabaseName("IX_PLine_PackageId");

            builder.HasIndex(x => x.StockCode)
                .HasDatabaseName("IX_PLine_StockCode");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PLine_IsDeleted");
        }
    }
}