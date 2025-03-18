namespace Web_Server.Models
{
    public class Recruitment
    {
        public int Id { get; set; }
        public string Address {  get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string Experience { get; set; }
        public int Quantity { get; set; }
         public string Rank { get; set; }
        public double Salary { get; set; }

        public int Status {  get; set; }

        public string Title { get; set; }
        public string Type {  get; set; }
        public int View {  get; set; }
        public string Deadline { get; set; }
        public Company Company { get; set; }
        public int CompanyId { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }


    }
}
