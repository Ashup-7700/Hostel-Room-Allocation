using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.EntityConfiguration.BaseConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.HRM.Repository.EntityConfiguration
{
    internal class FeeStructureConfig : BaseEntityConfig<FeeStructure>
    {
        public override void Configure(EntityTypeBuilder<FeeStructure> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.FeeStructureId).ValueGeneratedOnAdd();
            builder.Property(f => f.RoomType).IsRequired().HasMaxLength(50);
            builder.Property(f => f.MonthlyRent).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(f => f.SecurityDeposit).IsRequired().HasColumnType("decimal(18,2)");

        }
    }
}