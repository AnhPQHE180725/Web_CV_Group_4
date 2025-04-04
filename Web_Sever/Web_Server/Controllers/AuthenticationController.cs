﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private static Dictionary<string, string> _otpEmailMap = new();
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;


        public AuthenticationController(IUserService userService, IConfiguration configuration, IEmailService emailService, IMemoryCache cache)
        {
            _userService = userService;
            _configuration = configuration;
            _emailService = emailService;
            _cache = cache;
        }



        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Email)) // Kiểm tra email
                return BadRequest("Email không được để trống.");

            var result = await _userService.ForgotPasswordAsync(request.Email); // Gọi phương thức ForgotPasswordAsync
            if (!result) return BadRequest("Email không tồn tại.");

            return Ok(new { message = "Email đặt lại mật khẩu đã được gửi thành công." });
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

            return Ok(new { message = "Mật khẩu đã được cập nhật thành công." });
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
            var user = await _userService.CheckLoginAsync(loginVm);
            if (user == null)
            {
                return NotFound("Thông tin đăng nhập không chính xác.");
            }

            // Nếu user có role là "Candidate", bỏ qua xác minh OTP
            if (user.RoleId == 1)
            {
                var jwtToken = await GenerateJwtToken(user);
                return Ok(new { token = jwtToken });
            }

            // Tạo mã OTP ngẫu nhiên
            var otpCode = new Random().Next(100000, 999999).ToString();

            // Lưu OTP vào cache với thời gian hết hạn 5 phút (OTP làm key, email làm value)
            _cache.Set(otpCode, user.Email, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            // Gửi mã OTP qua email
            await _emailService.SendOtpEmailAsync(user.Email, otpCode);

            return Ok(new { message = "Vui lòng kiểm tra email để nhập mã xác minh.", email = user.Email });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            if (string.IsNullOrEmpty(request.Otp) || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Thông tin không đầy đủ.");
            }

            if (!_cache.TryGetValue(request.Otp, out string? cachedEmail) || cachedEmail != request.Email)
            {
                return BadRequest("Mã xác minh không tồn tại hoặc đã hết hạn.");
            }

            var user = await _userService.FindEmailExists(request.Email);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }

            var jwtToken = await GenerateJwtToken(user);

            _cache.Remove(request.Otp);  // Xóa OTP sau khi xác minh thành công

            return Ok(new { token = jwtToken });
        }

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpVm resendOtpVm)
        {
            var user = await _userService.FindEmailExists(resendOtpVm.Email);
            if (user == null)
            {
                return NotFound("Không tìm thấy người dùng.");
            }

            // Tạo mã OTP mới
            var otpCode = new Random().Next(100000, 999999).ToString();

            // Lưu OTP vào cache với thời gian hết hạn 5 phút
            _cache.Set(otpCode, user.Email, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            // Gửi mã OTP qua email
            await _emailService.SendOtpEmailAsync(user.Email, otpCode);

            return Ok(new { message = "Mã OTP mới đã được gửi đến email của bạn." });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVm registerVm)
        {
            try
            {
                if (await _userService.FindEmailExists(registerVm.Email) != null)
                {
                    return BadRequest("Email đã tồn tại trong hệ thống.");
                }
                if (string.IsNullOrWhiteSpace(registerVm.FullName) ||
                    string.IsNullOrWhiteSpace(registerVm.Password) ||
                    string.IsNullOrWhiteSpace(registerVm.ConfirmPassword) ||
                    string.IsNullOrWhiteSpace(registerVm.RoleName))
                {
                    return BadRequest("Vui lòng nhập đầy đủ thông tin.");
                }

                // Tạo mã OTP ngẫu nhiên
                var otpCode = new Random().Next(100000, 999999).ToString();

                // Lưu thông tin đăng ký vào cache (OTP làm key, dữ liệu người dùng làm value)
                _cache.Set(otpCode, registerVm, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
                // Lưu mapping giữa email và OTP
                _otpEmailMap[registerVm.Email] = otpCode;

                // Gửi mã OTP qua email
                await _emailService.SendOtpEmailAsync(registerVm.Email, otpCode);

                return Ok(new { message = "Vui lòng kiểm tra email để nhập mã xác minh." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi trong quá trình xử lý.", error = ex.Message, stackTrace = ex.StackTrace });

            }
        }

        [HttpPost("verify-signup")]
        public async Task<IActionResult> VerifySignup([FromBody] VerifyRegisterVm verifyRegisterVm)
        {
            if (string.IsNullOrEmpty(verifyRegisterVm.Email) || string.IsNullOrEmpty(verifyRegisterVm.Otp))
            {
                return BadRequest("Thông tin không đầy đủ.");
            }


            if (!_cache.TryGetValue(verifyRegisterVm.Otp, out RegisterVm? cachedRegisterData))
            {
                return BadRequest("Mã xác minh không tồn tại hoặc đã hết hạn.");
            }
            // Kiểm tra role
            if (string.IsNullOrEmpty(cachedRegisterData.RoleName))
            {
                return BadRequest("Vai trò không hợp lệ.");
            }

            // Thêm người dùng vào cơ sở dữ liệu
            var result = await _userService.RegisterAysnc(cachedRegisterData);
            if (!result)
            {
                return BadRequest("Đăng ký thất bại. Vui lòng thử lại.");
            }

            // Xóa OTP khỏi cache sau khi thành công
            _cache.Remove(verifyRegisterVm.Otp);

            return Ok(new { message = "Đăng ký thành công!" });
        }

        [HttpPost("resend-otp-register")]
        public async Task<IActionResult> ResendOtpRegister([FromBody] ResendOtpVm resendOtpVm)
        {
            if (string.IsNullOrEmpty(resendOtpVm.Email))
            {
                return BadRequest("Email không được để trống.");
            }

            // Kiểm tra email có trong Dictionary không
            if (!_otpEmailMap.TryGetValue(resendOtpVm.Email, out string? oldOtp))
            {
                return BadRequest("Không tìm thấy dữ liệu đăng ký. Vui lòng đăng ký lại.");
            }

            // Lấy dữ liệu đăng ký từ cache
            if (!_cache.TryGetValue(oldOtp, out RegisterVm? cachedRegisterData))
            {
                return BadRequest("Dữ liệu đăng ký không hợp lệ hoặc đã hết hạn.");
            }

            // Xóa OTP cũ
            _cache.Remove(oldOtp);

            // Tạo OTP mới
            var newOtp = new Random().Next(100000, 999999).ToString();

            // Lưu vào cache
            _cache.Set(newOtp, cachedRegisterData, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            // Cập nhật dictionary với OTP mới
            _otpEmailMap[resendOtpVm.Email] = newOtp;

            // Gửi OTP mới qua email
            await _emailService.SendOtpEmailAsync(resendOtpVm.Email, newOtp);

            return Ok(new { message = "Mã OTP mới đã được gửi đến email của bạn." });
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

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var user = await _userService.GetUserByIdAsync(int.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Description = user.Description,
                Image = user.Image,
                Role = user.Role?.Name
            });
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserVm model)
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var success = await _userService.UpdateProfileAsync(int.Parse(userId), model);
            if (!success) return BadRequest("Update failed");

            return Ok("Update successful");
        }



    }
}


