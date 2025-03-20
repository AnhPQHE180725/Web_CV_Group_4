using Microsoft.EntityFrameworkCore;
using Web_Server.Models;

namespace Web_Server.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Recruitment> Recruitments { get; set; }
        public DbSet<Company> Companies { get; set; }

        public DbSet<CV> CVs { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ApplyPost> ApplyPosts { get; set; }

        public DbSet<FollowCompany> FollowCompanies { get; set; }

        public DbSet<FollowJob> FollowJobs { get; set; }
        public DbSet<Role> Roles { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasOne(u => u.CV)
                .WithOne(c => c.User)
                .HasForeignKey<CV>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            builder.Entity<FollowCompany>()
                .HasOne(f => f.Company)
                .WithMany()
                .HasForeignKey(f => f.CompanyId)
                .OnDelete(DeleteBehavior.NoAction);  

            builder.Entity<ApplyPost>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);  
            builder.Entity<FollowJob>()
                .HasOne(fj => fj.User)
                .WithMany(u => u.FollowJobs)  
                .HasForeignKey(fj => fj.UserId) 
                .OnDelete(DeleteBehavior.NoAction);

            SeedData.Seed(builder);
        }

    }
}
