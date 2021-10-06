using Microsoft.EntityFrameworkCore;
using MiniURL.Core.Configuration;
using MiniURL.Core.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniURL.Core
{
    public class MiniURLContext : DbContext
    {
        public MiniURLContext(DbContextOptions<MiniURLContext> options) : base(options) { }

        protected MiniURLContext()
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Models.MiniURL> MiniUrls { get; set; }
        public DbSet<UserLevel> UserLevels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddUserConfiguration();
            modelBuilder.AddMiniUrlConfiguration();
            modelBuilder.Seed();


            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            if (ChangeTracker.Entries<EntityBase>() != null)
                UpdateTimestamps();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            if (ChangeTracker.Entries<EntityBase>() != null)
                UpdateTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (ChangeTracker.Entries<EntityBase>() != null)
                UpdateTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (ChangeTracker.Entries<EntityBase>() != null)
                UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            foreach (var entry in ChangeTracker.Entries<EntityBase>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                if (entry.State is EntityState.Modified or EntityState.Added)
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
            }
        }
    }
}
