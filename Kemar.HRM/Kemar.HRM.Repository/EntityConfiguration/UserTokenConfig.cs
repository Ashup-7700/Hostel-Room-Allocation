using Kemar.HRM.Repository.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kemar.HRM.Repository.EntityConfiguration
{
    internal class UserTokenConfig : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder) 
        {
           
            builder.HasKey(ut => ut.UserTokenId);

            builder.Property(ut => ut.UserTokenId).ValueGeneratedOnAdd();
            builder.Property(ut => ut.Token).IsRequired().HasMaxLength(500);
            builder.Property(ut => ut.GeneratedAt).IsRequired();
            builder.Property(ut => ut.ExpiresAt).IsRequired();
            builder.Property(ut => ut.SystemIp).IsRequired(false).HasMaxLength(50);

            builder.HasOne<User>().WithMany().HasForeignKey(ut => ut.UserId).OnDelete(DeleteBehavior.Cascade);

        }

    }
}