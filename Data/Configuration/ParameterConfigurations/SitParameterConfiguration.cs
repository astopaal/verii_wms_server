using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SitParameterConfiguration : BaseParameterConfiguration<SitParameter>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SitParameter> builder)
        {
            builder.ToTable("RII_SIT_PARAMETER");
        }
    }
}

