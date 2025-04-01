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
using Web_Server.ViewModels;

namespace TopCVWeb_Test
{
    public class ApplyPostServiceTest
    {
        private Mock<IApplyPostRepository> _applyPostRepoMock;
        private Mock<IRecruitmentRepository> _recruitmentRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<ICVService> _cvServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private ApplyPostService _applyPostService;

        [SetUp]
        public void Setup()
        {
            _applyPostRepoMock = new Mock<IApplyPostRepository>();
            _recruitmentRepoMock = new Mock<IRecruitmentRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _cvServiceMock = new Mock<ICVService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("id", "1")
            }, "mock"));

            var httpContext = new DefaultHttpContext
            {
                User = user
            };
            _httpContextAccessorMock.Setup(_ => _.HttpContext).Returns(httpContext);

            _applyPostService = new ApplyPostService(
                _applyPostRepoMock.Object,
                _recruitmentRepoMock.Object,
                _userRepoMock.Object,
                _cvServiceMock.Object,
                _httpContextAccessorMock.Object
            );
        }

        [Test]
        public async Task ApplyWithExistingCVAsync_ShouldReturnNewApplication()
        {
            var recruitment = new Recruitment { Id = 1, Status = 1 };
            var user = new User { Id = 1 };
            var cv = new CV { Id = 1, Name = "cv.pdf", UserId = 1 };

            _recruitmentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(recruitment);
            _userRepoMock.Setup(u => u.GetByIdAsync(1)).ReturnsAsync(user);
            _applyPostRepoMock.Setup(a => a.GetByUserIdAndPostIdAsync(1, 1)).ReturnsAsync((ApplyPost)null);
            _cvServiceMock.Setup(c => c.GetCVByUserIdAsync(1)).ReturnsAsync(cv);
            _applyPostRepoMock.Setup(a => a.CreateAsync(It.IsAny<ApplyPost>()))
                .ReturnsAsync((ApplyPost a) => a);

            var applyVm = new ApplyWithExistingCVVm
            {
                PostId = 1,
                Text = "I’m applying"
            };

            var result = await _applyPostService.ApplyWithExistingCVAsync(applyVm);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.CVName, Is.EqualTo("cv.pdf"));
        }

        [Test]
        public async Task GetApplicationsByUserIdAsync_ShouldReturnApplications()
        {
            var list = new List<ApplyPost>
            {
                new ApplyPost { UserId = 1, CVName = "cv1.pdf" },
                new ApplyPost { UserId = 1, CVName = "cv2.pdf" }
            };

            _applyPostRepoMock.Setup(r => r.GetByUserIdAsync(1)).ReturnsAsync(list);

            var result = await _applyPostService.GetApplicationsByUserIdAsync(1);

            Assert.That(result, Has.Count.EqualTo(2));
        }

        [Test]
        public async Task ApplyWithExistingCVAsync_ShouldThrow_WhenCVNotFound()
        {
            var recruitment = new Recruitment { Id = 1, Status = 1 };
            var user = new User { Id = 1 };

            _recruitmentRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(recruitment);
            _userRepoMock.Setup(u => u.GetByIdAsync(1)).ReturnsAsync(user);
            _applyPostRepoMock.Setup(a => a.GetByUserIdAndPostIdAsync(1, 1)).ReturnsAsync((ApplyPost)null);
            _cvServiceMock.Setup(c => c.GetCVByUserIdAsync(1)).ReturnsAsync((CV)null);

            var applyVm = new ApplyWithExistingCVVm
            {
                PostId = 1,
                Text = "missing CV"
            };

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _applyPostService.ApplyWithExistingCVAsync(applyVm));
        }
    }
}
