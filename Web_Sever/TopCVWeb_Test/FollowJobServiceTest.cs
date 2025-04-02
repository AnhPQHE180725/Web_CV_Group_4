using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Web_Server.Data;
using Web_Server.Models;
using Web_Server.Repositories;
using Web_Server.Services;

namespace TopCVWeb_Test
{
    public class FollowJobServiceTest
    {
        /*
        RecruitmentId như 2, 3, 4 phải tồn tại sẵn trong cơ sở dữ liệu.

        User có ID = 1 phải tồn tại và không bị trùng lặp logic.
         */
        private AppDbContext _context;
        private FollowJobRepository _followJobRepo;
        private RecruitmentRepository _recruitmentRepo;
        private UserRepository _userRepo;
        private FollowJobService _service;
        private IHttpContextAccessor _httpContextAccessor;

        [SetUp]
        public void Setup()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(config.GetConnectionString("DefaultConnection"))
                .Options;

            _context = new AppDbContext(options);
            _followJobRepo = new FollowJobRepository(_context);
            _recruitmentRepo = new RecruitmentRepository(_context);
            _userRepo = new UserRepository(_context);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("id", "1") // giả lập người dùng có ID = 1
            }, "mock"));

            _httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            _service = new FollowJobService(_followJobRepo, _recruitmentRepo, _userRepo, _httpContextAccessor);
        }

        [Test]
        public async Task ToggleFollowJobAsync_ShouldFollow_WhenNotFollowing()
        {
            var result = await _service.ToggleFollowJobAsync(12); // ID tuyển dụng = thực tế theo db phải tồn tại
            Assert.IsTrue(result.success);
            Assert.That(result.message, Is.EqualTo("Lưu theo dõi thành công"));
        }

        [Test]
        public async Task GetUserFollowJobsAsync_ShouldReturnFollowList()
        {
            var result = await _service.GetUserFollowJobsAsync();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.All(f => f.UserId == 1));
        }

        [Test]
        public async Task IsFollowingJobAsync_ShouldReturnFalse_IfNotExists()
        {
            var result = await _service.IsFollowingJobAsync(9999); // ID không tồn tại
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetFollowedJobIdsAsync_ShouldReturnRecruitmentIds()
        {
            await _service.ToggleFollowJobAsync(4); // theo dõi trước
            var result = await _service.GetFollowedJobIdsAsync();
            Assert.IsTrue(result.Contains(4));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}