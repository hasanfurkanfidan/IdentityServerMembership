using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerMembership.AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> opts) : base(opts)
        {

        }
        public DbSet<CustomUser> CustomUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser
                {
                    City = "İstanbul",
                    Password = "Password",
                    Email = "furkanfidan.job@gmail.com",
                    Id = 1,
                    UserName = "furkanfidan.job@gmail.com"
                },
                 new CustomUser
                 {
                     City = "İstanbul",
                     Password = "Password",
                     Email = "ahmet.job@gmail.com",
                     Id = 2,
                     UserName = "ahmet.job@gmail.com"
                 },
                  new CustomUser
                  {
                      City = "Konya",
                      Password = "Password",
                      Email = "furkanfidan.job@gmail.com",
                      Id = 3,
                      UserName = "mehmet.job@gmail.com"
                  }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
