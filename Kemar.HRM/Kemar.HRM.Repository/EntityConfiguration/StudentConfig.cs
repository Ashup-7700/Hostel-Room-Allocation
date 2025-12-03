using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.EntityConfiguration.BaseConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.HRM.Repository.EntityConfiguration
{
    internal class StudentConfig : BaseEntityConfig<Student>, IEntityTypeConfiguration<Student>
    {
        public override void Configure(EntityTypeBuilder<Student> builder)
        {
            base.Configure(builder);

            builder.ToTable("Students");

            builder.HasKey(s => s.StudentId);

            builder.Property(s => s.StudentId)
                   .ValueGeneratedOnAdd();

            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(s => s.Gender)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(s => s.Phone)
                   .IsRequired(false)
                   .HasMaxLength(15);

            builder.Property(s => s.Email)
                   .IsRequired()
                   .HasMaxLength(120);

            builder.Property(s => s.Address)
                   .IsRequired(false)
                   .HasMaxLength(200);

            builder.Property(s => s.DateOfAdmission)
                   .IsRequired();

            builder.HasMany(s => s.RoomAllocations)
                   .WithOne(ra => ra.Student)
                   .HasForeignKey(ra => ra.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.Payments)
                   .WithOne(p => p.Student)
                   .HasForeignKey(p => p.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
