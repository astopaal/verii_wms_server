using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PrHeaderSerialConfiguration : BaseEntityConfiguration<PrHeaderSerial>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PrHeaderSerial> builder)
        {
            builder.ToTable("RII_PR_HEADER_SERIAL");

            builder.Property(x => x.HeaderId).IsRequired();
            builder.Property(x => x.SerialNo).HasMaxLength(50);
            builder.Property(x => x.SerialNo2).HasMaxLength(50);
            builder.Property(x => x.SerialNo3).HasMaxLength(50);
            builder.Property(x => x.SerialNo4).HasMaxLength(50);

            builder.HasIndex(x => x.HeaderId).HasDatabaseName("IX_PrHeaderSerial_HeaderId");
            builder.HasIndex(x => x.IsDeleted).HasDatabaseName("IX_PrHeaderSerial_IsDeleted");

            builder.HasOne(x => x.Header)
                .WithMany(x => x.HeaderSerials)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
