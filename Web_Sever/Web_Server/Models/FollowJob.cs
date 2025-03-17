namespace Web_Server.Models
{
    public class FollowJob
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RecruitmentId { get; set; }


        public User User { get; set; }
        public Recruitment Recruitment { get; set; }


    }
}
