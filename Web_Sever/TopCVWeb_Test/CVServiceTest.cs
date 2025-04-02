using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using System.Text;
using Web_Server.Data;
using Web_Server.Models;
using Web_Server.Repositories;
using Web_Server.Services;

namespace TopCVWeb_Test
{
    public class CVServiceTest
    {
        private AppDbContext _context;
        private CVRepository _cvRepository;
        private CVService _cvService;
        private Mock<IWebHostEnvironment> _envMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private string _wwwrootPath;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique DB for each test
                .Options;

            _context = new AppDbContext(options);
            _cvRepository = new CVRepository(_context);

            _envMock = new Mock<IWebHostEnvironment>();
            _wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot_test");
            _envMock.Setup(e => e.WebRootPath).Returns(_wwwrootPath);

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            // Setup fake user with ID = 1
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("id", "1")
            }, "mock"));

            var httpContext = new DefaultHttpContext
            {
                User = user
            };
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            _cvService = new CVService(_cvRepository, _envMock.Object, _httpContextAccessorMock.Object);
        }

        [Test]
        public async Task UploadCVAsync_ShouldUploadAndReturnCV()
        {
            var content = Encoding.UTF8.GetBytes("Fake PDF content");
            var file = new FormFile(new MemoryStream(content), 0, content.Length, "Data", "test.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            var result = await _cvService.UploadCVAsync(file);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserId, Is.EqualTo(1));
            Assert.That(result.Name, Does.Contain("test.pdf"));
        }

        [Test]
        public async Task GetCVByIdAsync_ShouldReturnCV()
        {
            var cv = new CV { Name = "test.pdf", UserId = 1 };
            await _context.CVs.AddAsync(cv);
            await _context.SaveChangesAsync();

            var result = await _cvService.GetCVByIdAsync(cv.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("test.pdf"));
        }

        [Test]
        public async Task GetCVByUserIdAsync_ShouldReturnCV()
        {
            var cv = new CV { Name = "usercv.pdf", UserId = 1 };
            await _context.CVs.AddAsync(cv);
            await _context.SaveChangesAsync();

            var result = await _cvService.GetCVByUserIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("usercv.pdf"));
        }

        [Test]
        public async Task GetCVFilePathByUserIdAsync_ShouldReturnFilePath_IfFileExists()
        {
            var cv = new CV { Name = "exist.pdf", UserId = 1 };
            await _context.CVs.AddAsync(cv);
            await _context.SaveChangesAsync();

            var folderPath = Path.Combine(_wwwrootPath, "CVs");
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, "exist.pdf");
            await File.WriteAllTextAsync(filePath, "test");

            var result = await _cvService.GetCVFilePathByUserIdAsync(1);

            Assert.That(result, Is.EqualTo(filePath));
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_wwwrootPath))
            {
                Directory.Delete(_wwwrootPath, true);
            }

            _context.Dispose();
        }
    }
}
