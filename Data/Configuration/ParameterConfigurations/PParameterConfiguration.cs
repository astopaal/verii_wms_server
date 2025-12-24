using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PParameterConfiguration : BaseParameterConfiguration<PParameter>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PParameter> builder)
        {
            builder.ToTable("RII_P_PARAMETER");
        }
    }
}

