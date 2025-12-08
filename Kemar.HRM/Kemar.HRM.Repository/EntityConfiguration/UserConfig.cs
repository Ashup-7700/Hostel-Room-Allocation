using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.EntityConfiguration.BaseConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.HRM.Repository.EntityConfiguration
{
    internal class UserConfig : BaseEntityConfig<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.UserId).ValueGeneratedOnAdd().UseIdentityColumn();

            builder.Property(x => x.FullName).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Role).HasMaxLength(50).HasDefaultValue("Warden");

            builder.HasMany(u => u.AllocationsHandled).WithOne(ra => ra.AllocatedBy).HasForeignKey(ra => ra.AllocatedByUserId).OnDelete(DeleteBehavior.Restrict);

        }
    }
}
