using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public class WtParameterConfiguration : BaseParameterConfiguration<WtParameter>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<WtParameter> builder)
        {
            builder.ToTable("RII_WT_PARAMETER");
        }
    }
}

