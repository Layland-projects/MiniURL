using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Core.Configuration
{
    public static class MiniUrlConfiguration
    {
        public static void AddMiniUrlConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.MiniURL>()
                .Property(x => x.UserId)
                .IsRequired();
            modelBuilder.Entity<Models.MiniURL>()
                .HasOne(x => x.User)
                .WithMany(x => x.MiniUrls)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<Models.MiniURL>()
                .OwnsOne(x => x.Url);
            modelBuilder.Entity<Models.MiniURL>()
                .HasIndex(x => x.Reference)
                .IsUnique();
        }
    }
}
