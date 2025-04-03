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
    public class ApplyPostServiceTest
    {
        /*
         Đảm bảo trong DB:
         User Id = 1 có CV và tồn tại.
         Recruitment Id = 1 là hợp lệ.
         */
        private AppDbContext _context;
        private ApplyPostRepository _applyPostRepo;
        private RecruitmentRepository _recruitmentRepo;
        private UserRepository _userRepo;
        private CVRepository _cvRepo;
        private CVService _cvService;
        private IHttpContextAccessor _httpContextAccessor;
        private ApplyPostService _applyPostService;

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

            _applyPostRepo = new ApplyPostRepository(_context);
            _recruitmentRepo = new RecruitmentRepository(_context);
            _userRepo = new UserRepository(_context);
            _cvRepo = new CVRepository(_context);

            _httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                    {
                        new Claim("id", "11") // user id trong db có role la candidate  - tự thay vào
                    }, "mock"))
                }
            };

            _cvService = new CVService(_cvRepo, new WebHostEnvironmentStub(), _httpContextAccessor);

            _applyPostService = new ApplyPostService(
                _applyPostRepo,
                _recruitmentRepo,
                _userRepo,
                _cvService,
                _httpContextAccessor
            );
        }

        //chạy 1 lần vì chỉ cho ứng tuyển 1 lần thôi, lần 2 lỗi , xóa trong ApplyPosst
        [Test]
        public async Task ApplyWithExistingCVAsync_ShouldCreateApplication_WhenValid()
        {
            var applyVm = new ApplyWithExistingCVVm
            {
                PostId = 3, // bài tuyển dụng đã có trong DB  - tự thay vào
                Text = "Ứng tuyển test"
            };

            var result = await _applyPostService.ApplyWithExistingCVAsync(applyVm);

            Assert.IsNotNull(result);
            Assert.That(result.UserId, Is.EqualTo(11));
            Assert.That(result.CVName, Is.Not.Null);
        }

        [Test]
        public async Task GetApplicationsByUserIdAsync_ShouldReturnCorrectList()
        {
            var result = await _applyPostService.GetApplicationsByUserIdAsync(11); //user id trong db  - tự thay vào

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        //Test lỗi khi không có CV
        [Test]
        public async Task ApplyWithExistingCVAsync_ShouldThrow_WhenUserHasNoCV()
        {
            var applyVm = new ApplyWithExistingCVVm
            {
                PostId = 3, // Bài đăng hợp lệ có trong db  - tự thay vào
                Text = "Test without CV"
            };

            // Giả lập user ID = 2 (đã tồn tại, nhưng không có CV)
            _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
             new Claim("id", "2")
    }));

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await _applyPostService.ApplyWithExistingCVAsync(applyVm));

        }


        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

    }
}
