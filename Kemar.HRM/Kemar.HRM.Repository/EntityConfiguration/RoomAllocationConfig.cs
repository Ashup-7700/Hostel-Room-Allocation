using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.EntityConfiguration.BaseConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.HRM.Repository.EntityConfiguration
{
    internal class RoomAllocationConfig : BaseEntityConfig<RoomAllocation>, IEntityTypeConfiguration<RoomAllocation>
    {
        public override void Configure(EntityTypeBuilder<RoomAllocation> builder)
        {
            base.Configure(builder);

            builder.Property(ra => ra.RoomAllocationId).ValueGeneratedOnAdd();

            builder.HasOne(ra => ra.Student)
                   .WithMany(s => s.RoomAllocations)
                   .HasForeignKey(ra => ra.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ra => ra.Room)
                   .WithMany(r => r.RoomAllocations)
                   .HasForeignKey(ra => ra.RoomId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ra => ra.AllocatedByUser)
                   .WithMany(u => u.AllocationsHandled)
                   .HasForeignKey(ra => ra.AllocatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(ra => ra.AllocatedAt)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(ra => ra.ReleasedAt)
                   .IsRequired(false);

            builder.Property(ra => ra.IsActive)
                   .IsRequired()
                   .HasDefaultValue(true);
        }
    }
}