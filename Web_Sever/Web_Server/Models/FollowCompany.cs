namespace Web_Server.Models
{
    public class FollowCompany
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Company Company { get; set; }

        public int UserId {  get; set; }
        public int CompanyId {  get; set; }


    }
}
