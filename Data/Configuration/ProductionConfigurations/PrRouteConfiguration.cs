using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PrRouteConfiguration : BaseRouteEntityConfiguration<PrRoute>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PrRoute> builder)
        {
            builder.ToTable("RII_PR_ROUTE");
            builder.Property(x => x.ImportLineId)
                .IsRequired();

            builder.HasIndex(x => x.ImportLineId)
                .HasDatabaseName("IX_PrRoute_ImportLineId");

            builder.HasIndex(x => x.SerialNo)
                .HasDatabaseName("IX_PrRoute_SerialNo");

            builder.HasIndex(x => x.SourceWarehouse)
                .HasDatabaseName("IX_PrRoute_SourceWarehouse");

            builder.HasIndex(x => x.TargetWarehouse)
                .HasDatabaseName("IX_PrRoute_TargetWarehouse");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_PrRoute_IsDeleted");

            builder.HasOne(x => x.ImportLine)
                .WithMany()
                .HasForeignKey(x => x.ImportLineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
