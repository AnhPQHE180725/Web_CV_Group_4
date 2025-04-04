using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web_Server.Data;
using Web_Server.Models;
using Web_Server.Repositories;
using Web_Server.Services;
using Web_Server.ViewModels;

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
        [Test]
        public async Task AddRecruitment_ShouldReturnTrue_WhenValidDataProvided()
        {
            var recruitmentVm = new RecruitmentVm
            {
                Title = "New Job",
                Description = "Job Description",
                Salary = 5000,
                Status = 1,
                Type = "Full-Time",
                Experience = "2 years",
                CompanyId = 1,
                CategoryId = 1,
                Quantity = 2,
                Deadline = DateTime.UtcNow.AddDays(10),
                Address = "123 Street",
                Rank = "Senior"
            };

            var result = await _recruitmentService.AddRecruitmentAsync(recruitmentVm);

            Assert.IsTrue(result);
        }
        [Test]
        public void AddRecruitment_ShouldThrowException_WhenInvalidDataProvided()
        {
            var invalidRecruitmentVm = new RecruitmentVm
            {
                Title = "", // Trống
                Description = "Thiếu title",
                Salary = -1000, // Lương âm
                Status = 1,
                Type = "Full-Time",
                Experience = "1 year",
                CompanyId = 0, // Sai ID
                CategoryId = 0, // Sai ID
                Quantity = 0, // Không hợp lệ
                Deadline = DateTime.UtcNow.AddDays(-2), // Quá hạn
                Address = "Some address",
                Rank = "Junior"
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _recruitmentService.AddRecruitmentAsync(invalidRecruitmentVm));
            Assert.AreEqual("Invalid recruitment data", ex.Message);
        }

        [Test]
        public async Task EditRecruitment_ShouldReturnTrue_WhenRecruitmentUpdated()
        {
            // Arrange - Thêm dữ liệu trước khi test
            var updatedRecruitment = new RecruitmentVm
            {
                Title = "New Title",
                Description = "New Description",
                Salary = 2000,
                Status = 1,
                Type = "Part-time",
                Experience = "5 years",
                CompanyId = 1,
                CategoryId = 2,
                Quantity = 10,
                Deadline = DateTime.UtcNow.AddDays(30),
                Address = "New Address",
                Rank = "Senior"
            };

            // Act
            var result = await _recruitmentService.EditRecruitmentAsync(2, updatedRecruitment);

            // Assert
            Assert.IsTrue(result, "Recruitment should be updated successfully.");
        }
        [Test]
        public async Task EditRecruitment_ShouldThrowException_WhenDataIsInvalid()
        {
            var updatedRecruitment = new RecruitmentVm
            {
                Title = "",  // Rỗng
                Description = "",
                Salary = 0,  // Không hợp lệ
                Status = 1,
                Type = "Full-Time",
                Experience = "3 years",
                CompanyId = 1,
                CategoryId = 1,
                Quantity = 1,
                Deadline = DateTime.UtcNow.AddDays(10),
                Address = "HCM",
                Rank = "Junior"
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _recruitmentService.EditRecruitmentAsync(1, updatedRecruitment));

            Assert.AreEqual("Invalid recruitment data", ex.Message);
        }
        [Test]
        public async Task EditRecruitment_ShouldThrowException_WhenRecruitmentDoesNotExist()
        {
            var recruitmentVm = new RecruitmentVm
            {
                Title = "Fake Job",
                Description = "No such recruitment",
                Salary = 1000,
                Status = 1,
                Type = "Full-Time",
                Experience = "1 year",
                CompanyId = 1,
                CategoryId = 1,
                Quantity = 1,
                Deadline = DateTime.UtcNow.AddDays(10),
                Address = "Unknown",
                Rank = "Junior"
            };

            var ex = Assert.ThrowsAsync<ArgumentException>(async () =>
                await _recruitmentService.EditRecruitmentAsync(9999, recruitmentVm));

            Assert.AreEqual($"Recruitment with id={9999} not found", ex.Message);
        }



        [Test]
        public async Task DeleteRecruitment_ShouldReturnTrue_WhenRecruitmentDeleted()
        {
            var result = await _recruitmentService.DeleteRecruitmentAsync(4);

            Assert.IsTrue(result);
        }
        [Test]
        public async Task DeleteRecruitment_ShouldThrowException_WhenRecruitmentDoesNotExist()
        {
            int nonExistentRecruitmentId = 9999;

            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _recruitmentService.DeleteRecruitmentAsync(nonExistentRecruitmentId));

            Assert.AreEqual($"Recruitment with ID {nonExistentRecruitmentId} not found", ex.Message);
        }
        [Test]
        public async Task GetTotalViews_ShouldReturnCorrectTotal()
        {
            var result = await _recruitmentService.GetTotalViews();
            Assert.IsTrue(result > 0, "should return total views");
        }

        [TearDown]
        public void TearDown()
        {

            _context.Dispose();
        }
    }
}
