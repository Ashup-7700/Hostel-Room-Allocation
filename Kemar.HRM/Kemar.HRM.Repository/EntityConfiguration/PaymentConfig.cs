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

            builder.HasKey(p => p.PaymentId);

            builder.Property(p => p.PaymentId)
                   .ValueGeneratedOnAdd();

            builder.Property(p => p.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaymentMode)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.PaymentStatus)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.PaymentDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne<Student>()
                   .WithMany()
                   .HasForeignKey(p => p.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
