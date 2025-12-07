using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class ShHeaderConfiguration : BaseHeaderEntityConfiguration<ShHeader>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ShHeader> builder)
        {
            builder.ToTable("RII_SH_HEADER");

            builder.Property(x => x.BranchCode).HasMaxLength(10).IsRequired();
            builder.Property(x => x.ProjectCode).HasMaxLength(20);
            builder.Property(x => x.DocumentType).HasMaxLength(10).IsRequired();
            builder.Property(x => x.CustomerCode).HasMaxLength(20);
            builder.Property(x => x.SourceWarehouse).HasMaxLength(20);
            builder.Property(x => x.TargetWarehouse).HasMaxLength(20);
            builder.Property(x => x.YearCode).HasMaxLength(4).IsRequired();
            builder.Property(x => x.Description1).HasMaxLength(50);
            builder.Property(x => x.Description2).HasMaxLength(100);

            builder.HasIndex(x => x.BranchCode).HasDatabaseName("IX_ShHeader_BranchCode");
            builder.HasIndex(x => x.PlannedDate).HasDatabaseName("IX_ShHeader_PlannedDate");
            builder.HasIndex(x => x.CustomerCode).HasDatabaseName("IX_ShHeader_CustomerCode");
            builder.HasIndex(x => x.YearCode).HasDatabaseName("IX_ShHeader_YearCode");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_ShHeader_IsDeleted");

            builder.HasMany(x => x.Lines)
                .WithOne(x => x.Header)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.ImportLines)
                .WithOne(x => x.Header)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
