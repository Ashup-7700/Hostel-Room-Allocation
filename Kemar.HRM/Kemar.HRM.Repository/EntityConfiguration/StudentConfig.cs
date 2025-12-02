using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.EntityConfiguration.Configurations.BaseConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.HRM.Repository.EntityConfiguration
{
    internal class StudentConfig : BaseEntityConfig<Student>
    {
        public override void Configure(EntityTypeBuilder<Student> builder)
        {
            base.Configure(builder);

            builder.Property(s => s.StudentId).ValueGeneratedOnAdd();
            builder.Property(s => s.Name).IsRequired().HasMaxLength(25);
            builder.Property(s => s.Gender).IsRequired().HasMaxLength(8);
            builder.Property(s => s.Phone).IsRequired().HasMaxLength(15);
            builder.Property(s => s.Email).IsRequired().HasMaxLength(25);
            builder.Property(s => s.Address).IsRequired().HasMaxLength(200);
            builder.Property(s => s.DateOfAdmission).IsRequired();

            builder.HasMany(s => s.RoomAllocations)
                   .WithOne(ra => ra.Student)
                   .HasForeignKey(ra => ra.StudentId);

            builder.HasMany(s => s.Payments)
                   .WithOne(p => p.Student)
                   .HasForeignKey(p => p.StudentId);

        }
    }
}
