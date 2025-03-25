using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IUserService userService, IConfiguration configuration, IEmailService emailService)
        {
            _userService = userService;
            _configuration = configuration;
            _emailService = emailService;
        }



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email)) // Kiểm tra email
                return BadRequest("Email không được để trống.");

            var result = await _userService.ForgotPasswordAsync(request.Email); // Gọi phương thức ForgotPasswordAsync
            if (!result) return BadRequest("Email không tồn tại.");

            return Ok("Email đặt lại mật khẩu đã được gửi thành công.");
        }

        [HttpGet("reset-password")]
        public IActionResult ResetPassword([FromQuery] string token)
        {
            if (string.IsNullOrEmpty(token)) // Kiểm tra token
                return BadRequest("Token không hợp lệ.");

            // Tạo form HTML để nhập mật khẩu mới
            var htmlContent = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <title>Reset password</title>
        </head>
        <body>
            <h2>Reset password</h2>
            <form method='POST' action='/api/Authentication/reset-password'>
                <input type='hidden' name='token' value='{token}' />
                <label for='newPassword'>New password:</label>
                <input type='password' id='newPassword' name='newPassword' required />
                <button type='submit'>Submit</button>
            </form>
        </body>
        </html>";

            return Content(htmlContent, "text/html"); // Thêm Content-Type để trình duyệt hiển thị HTML
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordPost([FromForm] string token, [FromForm] string newPassword)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(newPassword)) // Kiểm tra token và mật khẩu
                return BadRequest("Token và mật khẩu không được để trống.");

            var result = await _userService.ResetPasswordAsync(token, newPassword); // Gọi phương thức ResetPasswordAsync
            if (!result) return BadRequest("Token không hợp lệ hoặc đã hết hạn.");

            return Ok("Mật khẩu đã được cập nhật thành công.");
        }



        [HttpPost("check-email-exists")]
        public async Task<IActionResult> CheckEmailExists([FromBody] string email)
        {
            var user = await _userService.FindEmailExists(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginVm loginVm)
        {
            var user = await _userService.CheckLoginAsync(loginVm);  // Gọi phương thức CheckLoginAsync
            if (user == null)
            {
                return NotFound();
            }

            var token = await GenerateJwtToken(user); // Tạo token
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVm registerVm)
        {
            if (registerVm.Password != registerVm.ConfirmPassword)
            {
                return BadRequest("Password and Confirm Password do not match");
            }
            var result = await _userService.RegisterAysnc(registerVm);

            if (!result)
            {
                return BadRequest();
            }
            return Ok();
        }


        private async Task<string> GenerateJwtToken(User user)
        {
            var authClaims = new List<Claim>
            {
            new Claim("email", user.Email), // Thêm email vào token
            new Claim("id", user.Id.ToString()), // Thêm id vào token
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Thêm Jti (JWT ID) vào token
            };

            var userWithRole = await _userService.TakeRoleAsync(user); // Lấy thông tin user với role
            if (userWithRole != null && userWithRole.Role != null)
            {
                authClaims.Add(new Claim("role", userWithRole.Role.Name)); // Thêm role vào token
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));  // Lấy Secret Key từ appsettings.json
            // Tạo token
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Trả về token
        }

    }
}



