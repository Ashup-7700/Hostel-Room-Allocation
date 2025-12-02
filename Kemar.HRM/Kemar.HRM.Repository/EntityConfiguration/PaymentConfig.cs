using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.EntityConfiguration.Configurations.BaseConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.HRM.Repository.EntityConfiguration
{
    internal class PaymentConfig : BaseEntityConfig<Payment>
    {
        public override void Configure(EntityTypeBuilder<Payment> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.PaymentId).ValueGeneratedOnAdd();
            builder.Property(p => p.StudentId).IsRequired();
            builder.Property(p => p.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(p => p.PaymentMethod).IsRequired().HasMaxLength(50);
            builder.Property(p => p.PaymentType).IsRequired().HasMaxLength(50);
            builder.Property(p => p.PaymentDate).IsRequired();

            builder.HasOne(p => p.Student)
                   .WithMany(s => s.Payments)
                   .HasForeignKey(p => p.StudentId);

        }
    }
}
