using Microsoft.EntityFrameworkCore;
using MiniURL.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Core.Configuration
{
    public static class UserConfiguration
    {
        public static void AddUserConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(x => x.MiniUrls)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
            modelBuilder.Entity<User>()
                .OwnsOne(x => x.Email);
            modelBuilder.Entity<UserLevel>()
                .HasIndex(x => x.Name)
                .IsUnique();
        }
    }
}
