using Microsoft.AspNetCore.Hosting;
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
    public class CompanyServiceTest
    {
        private Mock<ICompanyRepository> _companyRepoMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private Mock<IWebHostEnvironment> _environmentMock;
        private CompanyService _companyService;

        [SetUp]
        public void Setup()
        {
            _companyRepoMock = new Mock<ICompanyRepository>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _environmentMock = new Mock<IWebHostEnvironment>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("id", "1")
            }, "mock"));

            var httpContext = new DefaultHttpContext
            {
                User = user
            };

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            _companyService = new CompanyService(
                _companyRepoMock.Object,
                _httpContextAccessorMock.Object,
                _environmentMock.Object
            );
        }

        [Test]
        public async Task GetAllCompanies_ShouldReturnList()
        {
            _companyRepoMock.Setup(r => r.GetAllCompanies()).ReturnsAsync(new List<Company> { new Company(), new Company() });

            var result = await _companyService.GetAllCompanies();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetTop4Companies_ShouldReturn4Companies()
        {
            _companyRepoMock.Setup(r => r.GetTop4Companies()).ReturnsAsync(new List<Company> { new Company(), new Company(), new Company(), new Company() });

            var result = await _companyService.GetTop4Companies();

            Assert.That(result.Count, Is.EqualTo(4));
        }

        [Test]
        public async Task GetCompanyByIdAsync_ShouldReturnCompany()
        {
            _companyRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Company { Id = 1 });

            var result = await _companyService.GetCompanyByIdAsync(1);

            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public async Task CreateCompanyAsync_ShouldReturnCreatedCompany()
        {
            var model = new CompanyCreateModel
            {
                Name = "NewCo",
                Description = "Desc",
                Address = "123 Street",
                Email = "test@mail.com",
                PhoneNumber = "123456789",
                Logo = "logo.png",
                Status = 1
            };

            _companyRepoMock.Setup(r => r.CreateAsync(It.IsAny<Company>()))
                .ReturnsAsync((Company c) => c);

            var result = await _companyService.CreateCompanyAsync(model);

            Assert.That(result.Name, Is.EqualTo("NewCo"));
            Assert.That(result.UserId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetUserCompaniesAsync_ShouldReturnCompanies()
        {
            var companies = new List<Company>
            {
                new Company { Id = 1, UserId = 1 },
                new Company { Id = 2, UserId = 1 }
            };

            _companyRepoMock.Setup(r => r.GetCompaniesByUserIdAsync(1)).ReturnsAsync(companies);

            var result = await _companyService.GetUserCompaniesAsync();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.All(c => c.UserId == 1), Is.True);
        }

        [Test]
        public async Task UpdateCompanyAsync_ShouldReturnUpdatedCompany()
        {
            var company = new Company { Id = 1, Name = "OldCo", Logo = "old.png" };

            var updateModel = new CompanyUpdateModel
            {
                Id = 1,
                Name = "UpdatedCo",
                Description = "New Desc",
                Address = "New Address",
                Email = "new@email.com",
                PhoneNumber = "999999999",
                Logo = "newlogo.png",
                Status = 1
            };

            _companyRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(company);
            _companyRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Company>())).ReturnsAsync((Company c) => c);

            var result = await _companyService.UpdateCompanyAsync(updateModel);

            Assert.That(result.Name, Is.EqualTo("UpdatedCo"));
            Assert.That(result.Logo, Is.EqualTo("newlogo.png"));
        }
    }
}
