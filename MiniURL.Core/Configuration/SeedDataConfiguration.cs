using Microsoft.EntityFrameworkCore;
using MiniURL.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniURL.Core.Configuration
{
    public static class SeedDataConfiguration
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLevel>()
                .HasData(UserLevel.Seed("Guest", "The default user level", 1));
            modelBuilder.Entity<UserLevel>()
                .HasData(UserLevel.Seed("Regular", "The user level for someone who has signed up", 5));
            modelBuilder.Entity<UserLevel>()
                .HasData(UserLevel.Seed("Premium", "The user level for a paying customer", null));
        }
    }
}
