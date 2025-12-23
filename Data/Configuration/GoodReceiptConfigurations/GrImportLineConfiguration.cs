using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class GrImportLineConfiguration : BaseImportLineEntityConfiguration<GrImportLine>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<GrImportLine> builder)
        {
            builder.ToTable("RII_GR_IMPORT_LINE");

            builder.Property(x => x.LineId)
                .HasColumnName("LineId");

            builder.Property(x => x.HeaderId)
                .IsRequired()
                .HasColumnName("HeaderId");

            


            // Relationships
            builder.HasOne(x => x.Line)
                .WithMany(x => x.ImportLines)
                .HasForeignKey(x => x.LineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Header)
                .WithMany(x => x.ImportLines)
                .HasForeignKey(x => x.HeaderId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            
        }
    }
}