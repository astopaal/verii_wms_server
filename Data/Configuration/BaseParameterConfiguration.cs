using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WMS_WEBAPI.Models;

namespace WMS_WEBAPI.Data.Configuration
{
    public abstract class BaseParameterConfiguration<T> : BaseEntityConfiguration<T> where T : BaseParameter
    {
        protected override void ConfigureEntity(EntityTypeBuilder<T> builder)
        {
            builder.Property(p => p.AllowLessQuantityBasedOnOrder)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("AllowLessQuantityBasedOnOrder");

            builder.Property(p => p.AllowMoreQuantityBasedOnOrder)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("AllowMoreQuantityBasedOnOrder");

            builder.Property(p => p.RequireApprovalBeforeErp)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("RequireApprovalBeforeErp");

            builder.Property(p => p.RequireAllOrderItemsCollected)
                .IsRequired()
                .HasDefaultValue(false)
                .HasColumnName("RequireAllOrderItemsCollected");
        }
    }
}

