﻿namespace Web_Server.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Recruitment> Recruitments { get; set; }
    }
}
