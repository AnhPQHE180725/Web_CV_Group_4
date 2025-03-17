using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web_Server.Models;

namespace Web_Server.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<Recruitment> Recruitments { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<CV> CVs { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ApplyPost> ApplyPosts { get; set; }

        public DbSet<FollowCompany> FollowCompanies { get; set; }

        public DbSet<FollowJob> FollowJobs { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Company>()
               .HasOne(c => c.User)
               .WithMany()
               .HasForeignKey(c => c.UserId);

            builder.Entity<CV>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            builder.Entity<FollowJob>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);

            builder.Entity<FollowCompany>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId);

            builder.Entity<FollowCompany>()
                .HasOne(f => f.Company)
                .WithMany()
                .HasForeignKey(f => f.CompanyId);

            builder.Entity<Recruitment>()
                .HasOne(r => r.Company)
                .WithMany()
                .HasForeignKey(r => r.CompanyId);

            builder.Entity<ApplyPost>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId);
            SeedData.Seed(builder);
        }


    }
}
