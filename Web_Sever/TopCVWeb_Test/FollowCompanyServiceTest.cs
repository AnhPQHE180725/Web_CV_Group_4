using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.Services;

namespace TopCVWeb_Test
{
    public class FollowCompanyServiceTest
    {
        private Mock<IFollowCompanyRepository> _followCompanyRepoMock;
        private Mock<ICompanyRepository> _companyRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private FollowCompanyService _followCompanyService;

        /*
         ToggleFollowCompanyAsync – theo dõi và hủy theo dõi công ty

        GetUserFollowCompaniesAsync – lấy danh sách công ty đã theo dõi

        IsFollowingCompanyAsync – kiểm tra có đang theo dõi công ty không

        GetFollowedCompanyIdsAsync – lấy danh sách ID công ty đã theo dõi
         */
        [SetUp]
        public void Setup()
        {
            _followCompanyRepoMock = new Mock<IFollowCompanyRepository>();
            _companyRepoMock = new Mock<ICompanyRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("id", "1")
            }, "mock"));

            var httpContext = new DefaultHttpContext
            {
                User = user
            };

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            _followCompanyService = new FollowCompanyService(
                _followCompanyRepoMock.Object,
                _companyRepoMock.Object,
                _userRepoMock.Object,
                _httpContextAccessorMock.Object
            );
        }

        [Test]
        public async Task ToggleFollowCompanyAsync_ShouldFollow_WhenNotFollowing()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new User { Id = 1 });
            _companyRepoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(new Company { Id = 10 });
            _followCompanyRepoMock.Setup(r => r.GetByUserIdAndCompanyIdAsync(1, 10)).ReturnsAsync((FollowCompany)null);
            _followCompanyRepoMock.Setup(r => r.CreateAsync(It.IsAny<FollowCompany>()))
                .ReturnsAsync(new FollowCompany { UserId = 1, CompanyId = 10 });

            var (success, message) = await _followCompanyService.ToggleFollowCompanyAsync(10);

            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo("Lưu theo dõi thành công"));
        }

        [Test]
        public async Task ToggleFollowCompanyAsync_ShouldUnfollow_WhenAlreadyFollowing()
        {
            var follow = new FollowCompany { Id = 5, UserId = 1, CompanyId = 10 };
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new User { Id = 1 });
            _companyRepoMock.Setup(r => r.GetByIdAsync(10)).ReturnsAsync(new Company { Id = 10 });
            _followCompanyRepoMock.Setup(r => r.GetByUserIdAndCompanyIdAsync(1, 10)).ReturnsAsync(follow);
            _followCompanyRepoMock.Setup(r => r.DeleteAsync(5)).ReturnsAsync(true);

            var (success, message) = await _followCompanyService.ToggleFollowCompanyAsync(10);

            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo("Bỏ lưu theo dõi thành công"));
        }

        [Test]
        public async Task GetUserFollowCompaniesAsync_ShouldReturnFollowList()
        {
            var follows = new List<FollowCompany>
            {
                new FollowCompany { CompanyId = 1 },
                new FollowCompany { CompanyId = 2 }
            };

            _followCompanyRepoMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(follows);

            var result = await _followCompanyService.GetUserFollowCompaniesAsync();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task IsFollowingCompanyAsync_ShouldReturnTrue_IfExists()
        {
            _followCompanyRepoMock.Setup(r => r.GetByUserIdAndCompanyIdAsync(1, 99))
                .ReturnsAsync(new FollowCompany());

            var result = await _followCompanyService.IsFollowingCompanyAsync(99);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsFollowingCompanyAsync_ShouldReturnFalse_IfNotExists()
        {
            _followCompanyRepoMock.Setup(r => r.GetByUserIdAndCompanyIdAsync(1, 99))
                .ReturnsAsync((FollowCompany)null);

            var result = await _followCompanyService.IsFollowingCompanyAsync(99);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetFollowedCompanyIdsAsync_ShouldReturnCompanyIds()
        {
            var follows = new List<FollowCompany>
            {
                new FollowCompany { CompanyId = 1 },
                new FollowCompany { CompanyId = 2 },
                new FollowCompany { CompanyId = 3 }
            };

            _followCompanyRepoMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(follows);

            var result = await _followCompanyService.GetFollowedCompanyIdsAsync();

            Assert.That(result, Is.EquivalentTo(new List<int> { 1, 2, 3 }));
        }
    }
}
