using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class ShRouteConfiguration : BaseRouteEntityConfiguration<ShRoute>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ShRoute> builder)
        {
            builder.ToTable("RII_SH_ROUTE");

            builder.Property(x => x.ImportLineId).IsRequired();

            builder.HasIndex(x => x.ImportLineId).HasDatabaseName("IX_ShRoute_ImportLineId");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_ShRoute_IsDeleted");

            builder.HasOne(x => x.ImportLine)
                .WithMany(x => x.Routes)
                .HasForeignKey(x => x.ImportLineId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
