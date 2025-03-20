namespace Web_Server.Models
{
    public class ApplyPost
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Recruitment Recruitment { get; set; }

        public int RecruitmentId { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Text { get; set; }
        public string CVName { get; set; }

        public int Status { get; set; }
    }
}
