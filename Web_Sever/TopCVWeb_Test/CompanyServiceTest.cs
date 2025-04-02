using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Web_Server.Data;
using Web_Server.Repositories;
using Web_Server.Services;
using Web_Server.ViewModels;

namespace TopCVWeb_Test
{
    public class CompanyServiceTest
    {
        /*
         Đảm bảo có dữ liệu test trong DB:
         Company với Id = 1 tồn tại
         Có user Id = 1 với công ty của họ
         */
        private AppDbContext _context;
        private CompanyRepository _companyRepo;
        private IHttpContextAccessor _httpContextAccessor;
        private IWebHostEnvironment _environment;
        private CompanyService _companyService;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(config.GetConnectionString("DefaultConnection"))
                .Options;

            _context = new AppDbContext(options);
            _companyRepo = new CompanyRepository(_context);

            _httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim("id", "1")
                    }, "mock"))
                }
            };

            _environment = new WebHostEnvironmentStub();

            _companyService = new CompanyService(_companyRepo, _httpContextAccessor, _environment);
        }

        [Test]
        public async Task GetAllCompanies_ShouldReturnList()
        {
            var result = await _companyService.GetAllCompanies();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [Test]
        public async Task GetTop4Companies_ShouldReturnUpTo4Companies()
        {
            var result = await _companyService.GetTop4Companies();

            Assert.IsNotNull(result);
            Assert.LessOrEqual(result.Count, 4);
        }

        [Test]
        public async Task GetCompanyByIdAsync_ShouldReturnCompany()
        {
            var result = await _companyService.GetCompanyByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task CreateCompanyAsync_ShouldCreateAndReturnCompany()
        {
            var model = new CompanyCreateModel
            {
                Name = "UnitTest Co",
                Description = "Test Company",
                Address = "123 Test St",
                Email = "test@company.com",
                PhoneNumber = "0123456789",
                Logo = "https://example.com/logo.png",
                Status = 1
            };

            var result = await _companyService.CreateCompanyAsync(model);

            Assert.IsNotNull(result);
            Assert.That(result.Name, Is.EqualTo("UnitTest Co"));
            Assert.That(result.UserId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUserCompaniesAsync_ShouldReturnOnlyUserCompanies()
        {
            var result = await _companyService.GetUserCompaniesAsync();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.All(c => c.UserId == 1));
        }

        [Test]
        public async Task UpdateCompanyAsync_ShouldUpdateCompany()
        {
            var existing = await _companyRepo.GetByIdAsync(1);
            if (existing == null) Assert.Fail("Company ID 1 does not exist");

            var updateModel = new CompanyUpdateModel
            {
                Id = existing.Id,
                Name = "Updated Co",
                Description = existing.Description,
                Address = existing.Address,
                Email = existing.Email,
                PhoneNumber = existing.PhoneNumber,
                Logo = "https://example.com/updatedlogo.png",
                Status = existing.Status
            };

            var result = await _companyService.UpdateCompanyAsync(updateModel);

            Assert.That(result.Name, Is.EqualTo("Updated Co"));
            Assert.That(result.Logo, Is.EqualTo("https://example.com/updatedlogo.png"));
        }

        [Test]
        public async Task GetCompaniesByName_ShouldReturnMatchingCompanies()
        {
            var result = await _companyService.GetCompaniesByName("FPT");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(c => c.Name.Contains("FPT")));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

    }
}
