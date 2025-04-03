using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;
using System.Text;
using Web_Server.Data;
using Web_Server.Repositories;
using Web_Server.Services;

namespace TopCVWeb_Test
{
    public class CVServiceTest
    {
        /*
        database đã có:
        User ID = 1
        CV gắn với user ID = 1
        Tạo thư mục wwwroot_test/CVs/ nếu chưa có
        -> có thể thay đổi userId nếu test với user khác
         */
        private AppDbContext _context;
        private CVRepository _cvRepository;
        private CVService _cvService;
        private IWebHostEnvironment _environment;
        private IHttpContextAccessor _httpContextAccessor;
        private string _wwwrootPath;

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
            _cvRepository = new CVRepository(_context);

            // Tạo fake HttpContext với user ID = 1
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("id", "11")  //user id phải có trong db  - tự thay vào
            }, "mock"));

            _httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Khởi tạo môi trường giả định
            _environment = new FakeWebHostEnvironment(); // ở cuối

            _cvService = new CVService(_cvRepository, _environment, _httpContextAccessor);
        }

        [Test]
        public async Task UploadCVAsync_ShouldUploadAndReturnCV()
        {
            var content = Encoding.UTF8.GetBytes("Fake PDF content");
            var file = new FormFile(new MemoryStream(content), 0, content.Length, "Data", "cv_test.pdf")
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };

            var result = await _cvService.UploadCVAsync(file);

            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(11)); //= user trong db  - tự thay vào
            Assert.That(result.Name, Does.Contain(".pdf"));
        }

        [Test]
        public async Task GetCVByIdAsync_ShouldReturnCV()
        {
            var cv = _context.CVs.FirstOrDefault(c => c.UserId == 11); //user id phải có trong db - tự thay vào
            Assert.IsNotNull(cv, "Không tìm thấy CV của userId = 1");

            var result = await _cvService.GetCVByIdAsync(cv.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(cv.Name));
        }

        [Test]
        public async Task GetCVByUserIdAsync_ShouldReturnCV()
        {
            var result = await _cvService.GetCVByUserIdAsync(11);  //user id phải có trong db  - tự thay vào

            Assert.That(result, Is.Not.Null);
            Assert.That(result.UserId, Is.EqualTo(11));  // = user 
        }

        private class FakeWebHostEnvironment : IWebHostEnvironment
        {
            public string WebRootPath { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot_test");

            // Unused members
            public string EnvironmentName { get; set; } = "Development";
            public string ApplicationName { get; set; }
            public string ContentRootPath { get; set; }
            public IFileProvider WebRootFileProvider { get; set; }
            public IFileProvider ContentRootFileProvider { get; set; }
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();

            if (Directory.Exists(_wwwrootPath))
            {
                Directory.Delete(_wwwrootPath, true);
            }
        }

    }
}
