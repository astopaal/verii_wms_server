using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WoParameterConfiguration : BaseParameterConfiguration<WoParameter>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WoParameter> builder)
        {
            builder.ToTable("RII_WO_PARAMETER");
        }
    }
}

