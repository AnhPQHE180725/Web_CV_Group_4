using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web_Server.Data;
using Web_Server.Repositories;
using Web_Server.Services;

namespace TopCVWeb_Test
{
    public class CategoryServiceTest
    {
        private AppDbContext _context;
        private CategoryRepository _categoryRepository;
        private CategoryService _categoryService;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString) // Connect to the real SQL Server DB
                .Options;

            _context = new AppDbContext(options);



            _categoryRepository = new CategoryRepository(_context);
            _categoryService = new CategoryService(_categoryRepository);
        }


        [Test]
        public async Task Get5TopCategories_ReturnsTop5Categories()
        {

            var result = await _categoryService.Get5TopCategories();


            Assert.That(result, Has.Count.EqualTo(5));
            Assert.That(result.First().Name, Is.Not.Null);
        }

        [Test]
        public async Task GetAllCategories_ReturnsAllCategories()
        {

            var result = await _categoryService.GetAllCategories();


            Assert.That(result, Has.Count.EqualTo(20));
        }

        [TearDown]
        public void TearDown()
        {

            _context.Dispose();
        }

    }
}
