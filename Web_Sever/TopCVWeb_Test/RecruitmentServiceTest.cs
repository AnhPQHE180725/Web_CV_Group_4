using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web_Server.Data;
using Web_Server.Repositories;
using Web_Server.Services;

namespace TopCVWeb_Test
{
    public class RecruitmentServiceTest
    {
        private AppDbContext _context;
        private RecruitmentRepository _recruitmentRepository;
        private RecruitmentService _recruitmentService;
        private IHttpContextAccessor _httpContextAccessor;
        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            _context = new AppDbContext(options);



            _recruitmentRepository = new RecruitmentRepository(_context);
            _recruitmentService = new RecruitmentService(_recruitmentRepository, _httpContextAccessor);
        }
        [Test]
        public async Task GetRecruitmentsByCategory_ShouldReturnRecruitments_WhenCalledWithValidId()
        {

            var result = await _recruitmentService.GetRecruitmentsByCategory(1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
        [Test]
        public async Task GetRecruitmentsByCategory_ShouldThrowException_WhenCalledWithInValidId()
        {

            int id = 10000;
            var ex = Assert.ThrowsAsync<ArgumentException>( async() => await _recruitmentService.GetRecruitmentsByCategory(id));


            Assert.AreEqual($"Not found Recruitment with category id={id}", ex.Message);

        }

        [Test]
        public async Task GetRecruitmentsByCompany_ShouldThrowException_WhenCalledWithInValidId()
        {
            int id = 10000;
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _recruitmentService.GetRecruitmentsByCompany(id));
            Assert.AreEqual($"Not found Recruitment with company id={id}", ex.Message);

        }
        [Test]
        public async Task GetRecruitmentsByCompany_ShouldReturnRecruitments_WhenCalledWithValidId()
        {

            var result = await _recruitmentService.GetRecruitmentsByCompany(1);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }
        [Test]
        public async Task GetTop2Recruitments_ShouldReturnTwoRecruitments()
        {

            var result = await _recruitmentService.GetTop2Recruitments();


            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count, "Should return exactly 2 recruitments");

            foreach (var recruitment in result)
            {
                Assert.IsNotNull(recruitment.Id);
                Assert.IsNotNull(recruitment.Title);
                Assert.IsNotNull(recruitment.CompanyName);
            }
        }

        [Test]
        public async Task GetRecruitmentsById_ShouldReturnRecruitments_WhenCalledWithValidId()
        {

            var result = await _recruitmentService.GetRecruitmentsByid(1);


            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0, "Should return recruitments for a valid ID");
        }
        [Test]
        public void GetRecruitmentsById_ShouldThrowException_WhenCalledWithInvalidId()
        {
            int id = 10000;
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _recruitmentService.GetRecruitmentsByid(id));
            Assert.AreEqual($"Not found Recruitment with id={id}", ex.Message);
        }
        [Test]

        public async Task GetAllRecruitments_ShouldReturnRecruitments_WhenRecruitmentsExist()
        {
            
            var result = await _recruitmentService.GetAllRecruitments();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0, "Should return a list of recruitments");
        }

        [Test]
        public void GetAllRecruitments_ShouldThrowException_WhenNoRecruitmentsExist()
        {
            
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _recruitmentService.GetAllRecruitments());
            Assert.AreEqual("Recruitment list is null", ex.Message);
        }
        [Test]
        public async Task GetRecruitmentsByCompanyName_ShouldReturnMatchingRecruitments()
        {
            var result = await _recruitmentService.GetRecruitmentsByCompanyName("Vin");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(r => r.CompanyName.Contains("Vin")));
        }
        [Test]
        public async Task GetRecruitmentsByTitle_ShouldReturnMatchingRecruitments()
        {
            var result = await _recruitmentService.GetRecruitmentsByTitle("Tuyển");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(r => r.Title.Contains("Tuyển")));
        }
        [Test]
        public async Task GetRecruitmentsByLocation_ShouldReturnMatchingRecruitments()
        {
            var result = await _recruitmentService.GetRecruitmentsByLocation("Hà Nội");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any(r => r.Address.Contains("Hà Nội")));

        }

        [TearDown]
        public void TearDown()
        {

            _context.Dispose();
        }
    }
}
