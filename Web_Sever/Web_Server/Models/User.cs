using Microsoft.AspNetCore.Identity;

namespace Web_Server.Models
{
    public class User 
    {
        public int Id { get; set; } 
        public string? Address { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Image { get; set; }

        public int Status {  get; set; }

        public string? Description { get; set; }
        public int? CVId { get; set; }
        public CV CV { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public List<Recruitment> Recruitments { get; set; }  
        
        public List<FollowJob> FollowJobs { get; set; }
        public List<FollowCompany> FollowCompany {  get; set; }
    }
}
