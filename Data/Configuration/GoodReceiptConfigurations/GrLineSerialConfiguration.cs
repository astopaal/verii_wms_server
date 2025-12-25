using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class GrLineSerialConfiguration : BaseLineSerialEntityConfiguration<GrLineSerial>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<GrLineSerial> builder)
        {
            builder.ToTable("RII_GR_LINE_SERIAL");

            builder.Property(x => x.LineId)
                .HasColumnName("LineId");

            builder.HasOne(x => x.Line)
                .WithMany(x => x.SerialLines)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_GrLineSerial_GrLine");

            builder.HasIndex(x => x.LineId)
                .HasDatabaseName("IX_GrLineSerial_LineId");

            builder.HasIndex(x => x.IsDeleted)
                .HasDatabaseName("IX_GrLineSerial_IsDeleted");

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
