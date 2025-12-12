using Kemar.HRM.Repository.Entity;
using Kemar.HRM.Repository.Entity.BaseEntities;
using Kemar.HRM.Repository.EntityConfiguration;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kemar.HRM.Repository.Context
{
    public class HostelDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HostelDbContext(DbContextOptions<HostelDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomAllocation> RoomAllocations { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<FeeStructure> FeeStructures { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Student>().HasQueryFilter(s => s.IsActive);
            modelBuilder.Entity<User>().HasQueryFilter(u => u.IsActive);

            modelBuilder.Entity<RoomAllocation>()
                .HasOne(r => r.AllocatedByUser)
                .WithMany()
                .HasForeignKey(r => r.AllocatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.ApplyConfiguration(new StudentConfig());
            modelBuilder.ApplyConfiguration(new RoomConfig());
            modelBuilder.ApplyConfiguration(new RoomAllocationConfig());
            modelBuilder.ApplyConfiguration(new PaymentConfig());
            modelBuilder.ApplyConfiguration(new FeeStructureConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new UserTokenConfig());
        }

        public override int SaveChanges()
        {
            AddAuditInfo();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void AddAuditInfo()
        {
            string currentUserRole = "Unknown";
            int? currentUserId = null;

            try
            {
                var userClaims = _httpContextAccessor.HttpContext?.User;

                if (userClaims != null && userClaims.Identity.IsAuthenticated)
                {
                    var userIdClaim = userClaims.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

                    if (int.TryParse(userIdClaim, out int uid))
                        currentUserId = uid;

                    currentUserRole = userClaims.Claims
                        .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "Unknown";
                }
            }
            catch
            {
                currentUserRole = "Unknown";
                currentUserId = null;
            }

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                bool isUserEntity = entry.Entity is User;

                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.IsActive = true;
                    entry.Entity.CreatedBy = isUserEntity ? "Admin" : currentUserRole;

                    if (!isUserEntity)
                    {
                        var prop = entry.Entity.GetType().GetProperty("CreatedByUserId");
                        if (prop != null)
                            prop.SetValue(entry.Entity, currentUserId);
                    }

                    entry.Entity.UpdatedAt = null;
                    entry.Entity.UpdatedBy = null;
                    continue;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = isUserEntity ? "Admin" : currentUserRole;

                    
                    if (currentUserId == null)
                    {
                        throw new UnauthorizedAccessException("User must be logged in to update records.");
                    }
                    continue;
                }

                if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsActive = false;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = isUserEntity ? "Admin" : currentUserRole;
                }
            }
        }


    }
}
