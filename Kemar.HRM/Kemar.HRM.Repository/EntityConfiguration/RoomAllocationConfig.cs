//using Kemar.HRM.Repository.Entity;
//using Kemar.HRM.Repository.EntityConfiguration.Configurations.BaseConfigurations;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//namespace Kemar.HRM.Repository.EntityConfiguration
//{
//    internal class RoomAllocationConfig : BaseEntityConfig<RoomAllocation>
//    {
//        public override void Configure(EntityTypeBuilder<RoomAllocation> builder)
//        {
//            base.Configure(builder);

//            builder.Property(x => x.RoomAllocationId).ValueGeneratedOnAdd();
//            builder.Property(r => r.StudentId).IsRequired();
//            builder.Property(r => r.RoomId).IsRequired();
//            builder.Property(r => r.AllocationDate).IsRequired();
//            builder.Property(r => r.CheckoutDate);

//            builder.HasOne(r => r.Student)
//                   .WithMany(s => s.RoomAllocations)
//                   .HasForeignKey(r => r.StudentId);

//            builder.HasOne(r => r.Room)
//                   .WithMany(rm => rm.RoomAllocations)
//                   .HasForeignKey(r => r.RoomId);

//        }
//    }
//}
