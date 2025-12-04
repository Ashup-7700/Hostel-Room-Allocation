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

            builder.Property(ra => ra.StudentId).IsRequired();
            builder.Property(ra => ra.RoomId).IsRequired();
            builder.Property(ra => ra.AllocatedByUserId).IsRequired();
            builder.Property(ra => ra.AllocatedAt).IsRequired();
            builder.Property(ra => ra.ReleasedAt).IsRequired(false);

            builder.HasOne(ra => ra.Student).WithMany(s => s.RoomAllocations).HasForeignKey(ra => ra.StudentId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ra => ra.Room).WithMany(r => r.RoomAllocations).HasForeignKey(ra => ra.RoomId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ra => ra.AllocatedBy).WithMany().HasForeignKey(ra => ra.AllocatedByUserId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
