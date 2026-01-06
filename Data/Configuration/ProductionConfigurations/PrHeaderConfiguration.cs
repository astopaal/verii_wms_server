using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PrHeaderConfiguration : BaseHeaderEntityConfiguration<PrHeader>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PrHeader> builder)
        {
            builder.ToTable("RII_PR_HEADER");

            builder.Property(x => x.BranchCode)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.ProjectCode)
                .HasMaxLength(20);

            builder.Property(x => x.DocumentType)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.CustomerCode)
                .HasMaxLength(20);

            builder.Property(x => x.StockCode)
                .HasMaxLength(20);

            builder.Property(x => x.YapKod)
                .HasMaxLength(20);

            builder.Property(x => x.Quantity)
                .HasColumnType("decimal(18,6)");

            builder.Property(x => x.SourceWarehouse)
                .HasMaxLength(20);

            builder.Property(x => x.TargetWarehouse)
                .HasMaxLength(20);

            builder.Property(x => x.YearCode)
                .HasMaxLength(4)
                .IsRequired();

            builder.Property(x => x.PriorityLevel);

            builder.HasIndex(x => x.BranchCode)
                .HasDatabaseName("IX_PrHeader_BranchCode");

            builder.HasIndex(x => x.ProjectCode)
                .HasDatabaseName("IX_PrHeader_ProjectCode");

            builder.HasIndex(x => x.PlannedDate)
                .HasDatabaseName("IX_PrHeader_PlannedDate");

            builder.HasIndex(x => x.CustomerCode)
                .HasDatabaseName("IX_PrHeader_CustomerCode");

            builder.HasIndex(x => x.StockCode)
                .HasDatabaseName("IX_PrHeader_StockCode");

            builder.HasIndex(x => x.YapKod)
                .HasDatabaseName("IX_PrHeader_YapKod");

            builder.HasIndex(x => x.YearCode)
                .HasDatabaseName("IX_PrHeader_YearCode");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PrHeader_IsDeleted");

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
