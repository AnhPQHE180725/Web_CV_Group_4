using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Web_Server.Data;
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
        private EmailService _emailService;
        private MemoryCache _memoryCache;

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
            _emailService = new EmailService();
            _userRepository = new UserRepository(_context);
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _userService = new UserService(_userRepository, _memoryCache, _emailService);

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

        //FindEmailExists_Testing:

        [Test]
        public async Task FindEmailExists_ExistsEmail_ReturnUser()
        {
            string email = "alice@example.com";

            var result = await _userService.FindEmailExists(email);

            // Kiểm tra kết quả trả về
            Assert.IsNotNull(result); // Kết quả không được null
            Assert.AreEqual("alice@example.com", result.Email); // Đảm bảo email trả về đúng
        }



        [Test]
        public async Task FindEmailExists_NotExistsEmail_ReturnNull()
        {
            string email = "invalid@example.com";

            var result = await _userService.FindEmailExists(email);

            // Kiểm tra kết quả trả về
            Assert.IsNull(result); // Kết quả phải là null vì thông tin sai
        }


        //RegisterAsnyc_Testing:
        [Test]
        public async Task RegisterAysnc_InvalidEmailFormat_ReturnFalse()
        {
            var registerVm = new RegisterVm
            {
                Email = "invalid-email",
                Password = "123",
                ConfirmPassword = "123",
                FullName = "Invalid",
                RoleName = "Candidate"

            };

            var result = await _userService.RegisterAysnc(registerVm);

            Assert.IsFalse(result); // Phải trả về false do email không hợp lệ
        }

        [Test]
        public async Task RegisterAysnc_ExistsEmail_ReturnFalse()
        {
            var registerVm = new RegisterVm
            {
                Email = "alice@example.com",
                Password = "123",
                ConfirmPassword = "123",
                FullName = "Invalid",
                RoleName = "Candidate"

            };

            var result = await _userService.RegisterAysnc(registerVm);

            Assert.IsFalse(result); // Phải trả về false do email không hợp lệ
        }

        [Test]
        public async Task RegisterAysnc_ValidEmail_ReturnTrue()
        {
            var registerVm = new RegisterVm
            {
                Email = "abc@gmail.com",
                Password = "123",
                ConfirmPassword = "123",
                FullName = "Valid",
                RoleName = "Candidate"

            };

            var result = await _userService.RegisterAysnc(registerVm);

            Assert.IsTrue(result); // Phải trả về false do email không hợp lệ
        }

        //TakeRoleAsync_Testing:
        [Test]
        public async Task TakeRoleAsync_ValidUser_ReturnUserRole()
        {
            // Lấy user đã được thêm trong Setup()
            var user = _context.Users.FirstOrDefault(u => u.Email == "alice@example.com");

            var result = await _userService.TakeRoleAsync(user);

            Assert.IsNotNull(result); // Đảm bảo user không null
            Assert.AreEqual(1, result.RoleId); // Đảm bảo vai trò đúng
        }

        //ForgotPasswordAsync_Testing
        [Test]
        public async Task ForgotPasswordAsync_NotExistMail_ReturnFalse()
        {
            string email = "invalidemail@gmail.com";

            var result = await _userService.ForgotPasswordAsync(email);
            Assert.IsFalse(result);
        }


        //ResetPasswordAsync_Testing

        [Test]
        public async Task ResetPasswordAsync_NotValidToken_ReturnFalse()
        {
            var result = await _userService.ResetPasswordAsync("NotValidToken", "1234");
            Assert.IsFalse(result);
        }

        // Phương thức dọn dẹp sau khi kiểm thử (TearDown)
        [TearDown]
        public void TearDown()
        {
            var testEmails = new List<string>
            {
                "abc@gmail.com",       // Email test của RegisterAysnc_ValidEmail_ReturnTrue
            };

            var usersToDelete = _context.Users.Where(u => testEmails.Contains(u.Email)).ToList();

            if (usersToDelete.Any())
            {
                _context.Users.RemoveRange(usersToDelete);
                _context.SaveChanges();
            }

            _context.Dispose();
            _memoryCache.Dispose();
        }
    }
}
