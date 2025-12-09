using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.EntityConfiguration.BaseConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.HRM.Repository.EntityConfiguration
{
    internal class RoomConfig : BaseEntityConfig<Room>, IEntityTypeConfiguration<Room>
    {
        public override void Configure(EntityTypeBuilder<Room> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.RoomId).ValueGeneratedOnAdd();
            builder.Property(r => r.RoomNumber).IsRequired().HasMaxLength(20);
            builder.Property(r => r.RoomType).IsRequired().HasMaxLength(50);
            builder.Property(r => r.Floor).IsRequired();
            builder.Property(r => r.Capacity).IsRequired();
            builder.Property(r => r.CurrentOccupancy).IsRequired();

            builder.HasMany(r => r.RoomAllocations).WithOne(ra => ra.Room).HasForeignKey(ra => ra.RoomId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
