using Web_Server.Models;

namespace Web_Server.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategories();
        Task<List<Category>> Get5TopCategories();
    }
}
