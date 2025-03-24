namespace Web_Server.Models
{
    public class Follow
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int? PostId { get; set; }  // Nullable vì có thể theo dõi công ty
        public int? CompanyId { get; set; }  // Nullable vì có thể theo dõi công việc
        public DateTime CreatedAt { get; set; }
        public User User { get; set; }
        public Recruitment? Post { get; set; }
        public Company? Company { get; set; }
    }
}
