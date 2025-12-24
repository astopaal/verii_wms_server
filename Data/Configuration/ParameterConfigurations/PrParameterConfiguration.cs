using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class PrParameterConfiguration : BaseParameterConfiguration<PrParameter>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<PrParameter> builder)
        {
            builder.ToTable("RII_PR_PARAMETER");
        }
    }
}

