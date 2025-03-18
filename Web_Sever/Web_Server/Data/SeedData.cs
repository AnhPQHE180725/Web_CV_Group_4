using Microsoft.EntityFrameworkCore;
using Web_Server.Models;

namespace Web_Server.Data
{
    public class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
            new Role {Id=1,Name="Candidate"  },
            new Role { Id = 2, Name = "Recruiter" }
            );
            // Seed Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, FullName = "John Doe", Email = "john@example.com", Password = "hashedpassword", Address = "123 Main St", PhoneNumber = "1234567890", Status = 1, Description = "Software Developer", CVId = 1, Image = "test",RoleId=1 },
                new User { Id = 2, FullName = "Jane Smith", Email = "jane@example.com", Password = "hashedpassword", Address = "456 Elm St", PhoneNumber = "9876543210", Status = 1, Description = "Data Analyst", CVId = 2, Image = "test",RoleId=2 }
            );

            // Seed CVs
            modelBuilder.Entity<CV>().HasData(
                new CV { Id = 1, Name = "John's CV", UserId = 1 },
                new CV { Id = 2, Name = "Jane's CV", UserId = 2 }
            );

            // Seed Companies
            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 1, Name = "Tech Corp", Description = "A leading tech company", Address = "789 Market St", Email = "contact@techcorp.com", PhoneNumber = "1112223333", Status = 1, UserId = 1, Logo = "a" },
                new Company { Id = 2, Name = "Finance Inc", Description = "A finance firm", Address = "321 Wall St", Email = "info@financeinc.com", PhoneNumber = "4445556666", Status = 1, UserId = 2, Logo = "a" }
            );

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Software Engineering" },
                new Category { Id = 2, Name = "Data Science" }
            );

            // Seed Recruitments
            modelBuilder.Entity<Recruitment>().HasData(
                new Recruitment { Id = 1, Title = "Software Developer", Description = "Looking for a full-stack developer", Salary = 80000, Status = 1, Type = "Full-Time", Experience = "2 years", CompanyId = 1, CategoryId = 1, CreatedAt = new DateTime(2024, 3, 18), Quantity = 10, Deadline = ".", Address = "Hanoi", Rank = "S", View = 100 },
                new Recruitment { Id = 2, Title = "Data Analyst", Description = "Seeking an experienced data analyst", Salary = 75000, Status = 1, Type = "Full-Time", Experience = "3 years", CompanyId = 2, CategoryId = 2, CreatedAt = new DateTime(2024, 3, 18), Quantity = 10, Deadline = ".", Address = "Hanoi", Rank = "S", View = 100 }
            );

            // Seed FollowCompanies
            modelBuilder.Entity<FollowCompany>().HasData(
                new FollowCompany { Id = 1, UserId = 1, CompanyId = 2 },
                new FollowCompany { Id = 2, UserId = 2, CompanyId = 1 }
            );

            // Seed FollowJobs
            modelBuilder.Entity<FollowJob>().HasData(
                new FollowJob { Id = 1, UserId = 1, RecruitmentId = 2 },
                new FollowJob { Id = 2, UserId = 2, RecruitmentId = 1 }
            );

            // Seed ApplyPosts
            modelBuilder.Entity<ApplyPost>().HasData(
                new ApplyPost { Id = 1, RecruitmentId = 1, UserId = 2, CreatedAt = new DateTime(2024, 3, 18), Text = "Applying for Software Developer", CVName = "Jane's CV", Status = 0 },
                new ApplyPost { Id = 2, RecruitmentId = 2, UserId = 1, CreatedAt = new DateTime(2024, 3, 18), Text = "Applying for Data Analyst", CVName = "John's CV", Status = 0 }
            );
        }

    }
}
