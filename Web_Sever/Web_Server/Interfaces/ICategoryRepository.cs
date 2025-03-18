﻿using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories();
    }
}
