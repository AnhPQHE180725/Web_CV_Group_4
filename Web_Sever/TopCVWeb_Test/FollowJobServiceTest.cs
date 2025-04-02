using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.Services;

namespace TopCVWeb_Test
{
    public class FollowJobServiceTest
    {
        private Mock<IFollowJobRepository> _followJobRepoMock;
        private Mock<IRecruitmentRepository> _recruitmentRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private FollowJobService _followJobService;

        [SetUp]
        public void Setup()
        {
            _followJobRepoMock = new Mock<IFollowJobRepository>();
            _recruitmentRepoMock = new Mock<IRecruitmentRepository>();
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

            _followJobService = new FollowJobService(
                _followJobRepoMock.Object,
                _recruitmentRepoMock.Object,
                _userRepoMock.Object,
                _httpContextAccessorMock.Object
            );
        }

        [Test]
        public async Task ToggleFollowJobAsync_ShouldFollow_WhenNotFollowing()
        {
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new User { Id = 1 });
            _recruitmentRepoMock.Setup(r => r.GetByIdAsync(100)).ReturnsAsync(new Recruitment { Id = 100 });
            _followJobRepoMock.Setup(r => r.GetByUserIdAndRecruitmentIdAsync(1, 100)).ReturnsAsync((FollowJob)null);
            _followJobRepoMock.Setup(r => r.CreateAsync(It.IsAny<FollowJob>()))
                .ReturnsAsync(new FollowJob { UserId = 1, RecruitmentId = 100 });

            var (success, message) = await _followJobService.ToggleFollowJobAsync(100);

            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo("Lưu theo dõi thành công"));
        }

        [Test]
        public async Task ToggleFollowJobAsync_ShouldUnfollow_WhenAlreadyFollowing()
        {
            var existing = new FollowJob { Id = 10, UserId = 1, RecruitmentId = 100 };
            _userRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new User { Id = 1 });
            _recruitmentRepoMock.Setup(r => r.GetByIdAsync(100)).ReturnsAsync(new Recruitment { Id = 100 });
            _followJobRepoMock.Setup(r => r.GetByUserIdAndRecruitmentIdAsync(1, 100)).ReturnsAsync(existing);
            _followJobRepoMock.Setup(r => r.DeleteAsync(10)).ReturnsAsync(true);

            var (success, message) = await _followJobService.ToggleFollowJobAsync(100);

            Assert.That(success, Is.True);
            Assert.That(message, Is.EqualTo("Bỏ lưu theo dõi thành công"));
        }

        [Test]
        public async Task GetUserFollowJobsAsync_ShouldReturnFollowList()
        {
            var follows = new List<FollowJob>
            {
                new FollowJob { RecruitmentId = 1 },
                new FollowJob { RecruitmentId = 2 }
            };

            _followJobRepoMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(follows);

            var result = await _followJobService.GetUserFollowJobsAsync();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task IsFollowingJobAsync_ShouldReturnTrue_IfExists()
        {
            _followJobRepoMock.Setup(r => r.GetByUserIdAndRecruitmentIdAsync(1, 10))
                .ReturnsAsync(new FollowJob { Id = 99 });

            var result = await _followJobService.IsFollowingJobAsync(10);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsFollowingJobAsync_ShouldReturnFalse_IfNotExists()
        {
            _followJobRepoMock.Setup(r => r.GetByUserIdAndRecruitmentIdAsync(1, 10))
                .ReturnsAsync((FollowJob)null);

            var result = await _followJobService.IsFollowingJobAsync(10);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task GetFollowedJobIdsAsync_ShouldReturnRecruitmentIds()
        {
            var follows = new List<FollowJob>
            {
                new FollowJob { RecruitmentId = 1 },
                new FollowJob { RecruitmentId = 2 },
                new FollowJob { RecruitmentId = 3 }
            };

            _followJobRepoMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(follows);

            var result = await _followJobService.GetFollowedJobIdsAsync();

            Assert.That(result, Is.EquivalentTo(new List<int> { 1, 2, 3 }));
        }
    }
}
