using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class IcParameterConfiguration : BaseParameterConfiguration<IcParameter>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<IcParameter> builder)
        {
            builder.ToTable("RII_IC_PARAMETER");
        }
    }
}

