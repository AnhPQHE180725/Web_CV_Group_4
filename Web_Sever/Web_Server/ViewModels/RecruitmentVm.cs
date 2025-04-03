namespace Web_Server.ViewModels
{
    public class RecruitmentVm
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string Experience { get; set; }
        public int Quantity { get; set; }
        public string Rank { get; set; }
        public double Salary { get; set; }

        public int Status { get; set; }

        public string Title { get; set; }
        public string Type { get; set; }
        public int View { get; set; }
        public DateTime Deadline { get; set; }

        public string? CompanyName { get; set; }

        public string? CategoryName { get; set; }



        // Add these properties to support Add and Edit
        public int CompanyId { get; set; }
        public int CategoryId { get; set; }
        public string? logo { get; set; }
    }
}
