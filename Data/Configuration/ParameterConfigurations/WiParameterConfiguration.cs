using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WiParameterConfiguration : BaseParameterConfiguration<WiParameter>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WiParameter> builder)
        {
            builder.ToTable("RII_WI_PARAMETER");
        }
    }
}

