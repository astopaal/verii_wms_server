using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class GrParameterConfiguration : BaseParameterConfiguration<GrParameter>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<GrParameter> builder)
        {
            builder.ToTable("RII_GR_PARAMETER");
        }
    }
}

