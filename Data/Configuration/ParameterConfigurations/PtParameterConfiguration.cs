using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PtParameterConfiguration : BaseParameterConfiguration<PtParameter>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PtParameter> builder)
        {
            builder.ToTable("RII_PT_PARAMETER");
        }
    }
}

