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
            builder.ToTable("RoomAllocations");

            builder.HasKey(r => r.RoomAllocationId);
            builder.Property(r => r.RoomAllocationId).ValueGeneratedOnAdd();

            builder.Property(r => r.StudentId).IsRequired();
            builder.Property(r => r.RoomId).IsRequired();
            builder.Property(r => r.AllocatedByUserId).IsRequired();
            builder.Property(r => r.AllocatedAt).IsRequired();
            builder.Property(r => r.ReleasedAt).IsRequired(false);

            builder.HasOne(r => r.Student).WithMany(s => s.RoomAllocations)
                   .HasForeignKey(r => r.StudentId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Room).WithMany(rm => rm.RoomAllocations)
                   .HasForeignKey(r => r.RoomId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.AllocatedBy).WithMany()
                   .HasForeignKey(r => r.AllocatedByUserId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
