namespace Web_Server.Models
{
    public class FollowJob
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Recruitment Recruitment { get; set; }

        public int RecruitmentId { get; set; }

        public string UserId { get; set; }
    }
}
