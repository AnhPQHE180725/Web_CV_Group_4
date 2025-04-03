
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Web_Server.Data;
using Web_Server.Repositories;
using Web_Server.Services;

namespace TopCVWeb_Test
{
    public class FollowCompanyServiceTest
    {
        private AppDbContext _context;
        private FollowCompanyRepository _followRepo;
        private CompanyRepository _companyRepo;
        private UserRepository _userRepo;
        private FollowCompanyService _service;
        private IHttpContextAccessor _httpContextAccessor;

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
            _followRepo = new FollowCompanyRepository(_context);
            _companyRepo = new CompanyRepository(_context);
            _userRepo = new UserRepository(_context);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("id", "1")
            }, "mock"));

            var httpContext = new DefaultHttpContext
            {
                User = user
            };
            _httpContextAccessor = new HttpContextAccessor { HttpContext = httpContext };

            _service = new FollowCompanyService(_followRepo, _companyRepo, _userRepo, _httpContextAccessor);
        }


        //khi chưa theo dõi cty nào, chay lần 2 là bỏ follow cty, chỉ chạy 1 lần
        [Test]
        public async Task ToggleFollowCompanyAsync_ShouldFollow_WhenNotFollowing()
        {
            var result = await _service.ToggleFollowCompanyAsync(2); // CompanyID = 2 must exist
            Assert.IsTrue(result.success);
            Assert.AreEqual("Lưu theo dõi thành công", result.message);
        }

        [Test]
        public async Task GetUserFollowCompaniesAsync_ShouldReturnFollowedCompanies()
        {
            var list = await _service.GetUserFollowCompaniesAsync();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count >= 0);
        }

        //khi chưa theo dõi cty nào, chay lần 2 là bỏ follow cty, chỉ chạy 1 lần
        [Test]
        public async Task IsFollowingCompanyAsync_ShouldReturnTrue_WhenFollowed()
        {
            await _service.ToggleFollowCompanyAsync(3); // Assume company 3 exists
            var isFollowing = await _service.IsFollowingCompanyAsync(3);
            Assert.IsTrue(isFollowing);
        }

        [Test]
        public async Task GetFollowedCompanyIdsAsync_ShouldReturnIds()
        {
            var ids = await _service.GetFollowedCompanyIdsAsync();
            Assert.IsNotNull(ids);
            Assert.IsTrue(ids.All(id => id > 0));
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }
    }
}
