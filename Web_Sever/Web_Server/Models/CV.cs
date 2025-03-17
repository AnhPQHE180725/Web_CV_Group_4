namespace Web_Server.Models
{
    public class CV
    {
        public int Id {  get; set; }

        public string Name { get; set; }

        public User User { get; set; }

        public string UserId {  get; set; }
    }
}
