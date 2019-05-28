using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Animals.Models
{

    public class MyDbContext : IdentityDbContext<MyUsers, IdentityRole<int>, int>
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MyUsers>().ToTable("MyUsers");
        }

        public DbSet<Animals> Animals { get; set; }
        public DbSet<Notify> Notify { get; set; }
        public DbSet<AnimalPictures> AnimalPictures { get; set; }
        public DbSet<AnimalType> AnimalType { get; set; }
    }
}
