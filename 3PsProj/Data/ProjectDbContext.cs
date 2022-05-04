using System;
using Microsoft.EntityFrameworkCore;
using _3PsProj.Models;

namespace _3PsProj.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options) { }

        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Company>().HasKey(c => c.Id);
            base.OnModelCreating(builder);
        }
    }
}
