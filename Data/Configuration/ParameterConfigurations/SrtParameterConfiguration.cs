using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class SrtParameterConfiguration : BaseParameterConfiguration<SrtParameter>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<SrtParameter> builder)
        {
            builder.ToTable("RII_SRT_PARAMETER");
        }
    }
}

