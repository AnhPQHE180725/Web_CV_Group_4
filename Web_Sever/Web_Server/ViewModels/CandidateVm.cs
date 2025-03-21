using Web_Server.Models;

namespace Web_Server.ViewModels
{
    public class CandidateVm
    {
        public string? Address { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Image { get; set; }

        public int Status { get; set; }

        public string? Description { get; set; }
        public int CVStatus { get; set; }
    }
}
