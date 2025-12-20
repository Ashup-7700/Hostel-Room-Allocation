using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.EntityConfiguration.BaseConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.HRM.Repository.EntityConfiguration
{
    internal class PaymentConfig
        : BaseEntityConfig<Payment>, IEntityTypeConfiguration<Payment>
    {
        public override void Configure(EntityTypeBuilder<Payment> builder)
        {
            base.Configure(builder);

            // 🔑 Primary Key
            builder.HasKey(p => p.PaymentId);

            builder.Property(p => p.PaymentId)
                   .ValueGeneratedOnAdd();

            // 💰 Amounts
            builder.Property(p => p.TotalAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaidAmount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            // 💳 Payment Mode
            builder.Property(p => p.PaymentMode)
                   .HasMaxLength(50);

            // 📌 Payment Status (stored OR optional)
            //builder.Property(p => p.PaymentStatus)
            //       .IsRequired()
            //       .HasMaxLength(50);

            // 📅 Payment Date
            builder.Property(p => p.PaymentDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            // 👤 Created By
            builder.Property(p => p.CreatedByUserId)
                   .IsRequired(false);

            // 🔗 Relationship
            builder.HasOne(p => p.Student)
                   .WithMany(s => s.Payments)
                   .HasForeignKey(p => p.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
