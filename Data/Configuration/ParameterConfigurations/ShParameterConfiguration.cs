using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class ShParameterConfiguration : BaseParameterConfiguration<ShParameter>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<ShParameter> builder)
        {
            builder.ToTable("RII_SH_PARAMETER");
        }
    }
}

