using Web_Server.Interfaces;
using Web_Server.Models;

namespace Web_Server.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> Get5TopCategories()
        {
           return await _categoryRepository.Get5TopCategories();
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAllCategories();
        }


    }
}
