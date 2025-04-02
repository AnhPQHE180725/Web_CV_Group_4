using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Server.Data;
using Web_Server.Interfaces;
using Web_Server.Repositories;
using Web_Server.Services;
using Web_Server.ViewModels;


namespace TopCVWeb_Test
{
    public class UserServiceTest
    {
        private AppDbContext _context;
        private UserRepository _userRepository;
        private UserService _userService;

        // Thiết lập môi trường kiểm thử (Setup)
        [SetUp]
        public void Setup()
        {
            // Đọc file cấu hình appsettings.json
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Tạo DbContext với kết nối đến cơ sở dữ liệu thực tế
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString) // Kết nối tới SQL Server thực tế
                .Options;

            _context = new AppDbContext(options);

            // Khởi tạo repository và service
            _userRepository = new UserRepository(_context);
            _userService = new UserService(_userRepository, null, null);  // Sử dụng null cho IMemoryCache và IEmailService
        }

        //CheckLoginAsync

        // Kiểm tra đăng nhập với thông tin hợp lệ
        [Test]
        public async Task CheckLoginAsync_ValidCredentials_ReturnsUser()
        {
            // Chuẩn bị dữ liệu kiểm thử
            var loginVm = new LoginVm { Email = "alice@example.com", Password = "123" };

            // Thực hiện phương thức đăng nhập
            var result = await _userService.CheckLoginAsync(loginVm);

            // Kiểm tra kết quả trả về
            Assert.IsNotNull(result); // Kết quả không được null
            Assert.AreEqual("alice@example.com", result.Email); // Đảm bảo email trả về đúng
        }

        // Kiểm tra đăng nhập với thông tin không hợp lệ
        [Test]
        public async Task CheckLoginAsync_InvalidCredentials_ReturnsNull()
        {
            var loginVm = new LoginVm { Email = "invalid@example.com", Password = "wrongpassword" };

            var result = await _userService.CheckLoginAsync(loginVm);

            // Kiểm tra kết quả trả về
            Assert.IsNull(result); // Kết quả phải là null vì thông tin đăng nhập sai
        }

        //


        // Phương thức dọn dẹp sau khi kiểm thử (TearDown)
        [TearDown]
        public void TearDown()
        {
            // Đảm bảo rằng chúng ta giải phóng tài nguyên sau mỗi lần kiểm thử
            _context.Dispose();
        }
    }
}
