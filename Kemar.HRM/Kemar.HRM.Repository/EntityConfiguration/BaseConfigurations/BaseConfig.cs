using Kemar.HRM.Repository.Entity.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.HRM.Repository.EntityConfiguration.BaseConfigurations
{
    internal abstract class BaseEntityConfig<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(b => b.CreatedBy).HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.UpdatedBy).HasMaxLength(50).IsRequired(false);
            builder.Property(b => b.CreatedAt).IsRequired();
            builder.Property(b => b.UpdatedAt).IsRequired(false);
            builder.Property(b => b.IsActive).HasDefaultValue(true);

        }
    }
}