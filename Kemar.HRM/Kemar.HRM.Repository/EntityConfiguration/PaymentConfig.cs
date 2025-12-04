using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.EntityConfiguration.BaseConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.HRM.Repository.EntityConfiguration
{
    internal class PaymentConfig : BaseEntityConfig<Payment>, IEntityTypeConfiguration<Payment>
    {
        public override void Configure(EntityTypeBuilder<Payment> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.PaymentId).ValueGeneratedOnAdd();
            builder.Property(p => p.Amount).IsRequired();
            builder.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(50);
            builder.Property(p => p.PaymentType).IsRequired().HasMaxLength(50);
            builder.Property(p => p.PaymentDate).IsRequired();

            builder.HasOne(p => p.Student).WithMany(s => s.Payments).HasForeignKey(p => p.StudentId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
