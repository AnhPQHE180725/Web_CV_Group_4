﻿using Microsoft.AspNetCore.Http;
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
            if (string.IsNullOrEmpty(request.Email))
                return BadRequest("Email không được để trống.");

            var result = await _userService.ForgotPasswordAsync(request.Email);
            if (!result) return BadRequest("Email không tồn tại.");

            return Ok("Email đặt lại mật khẩu đã được gửi thành công.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.NewPassword))
                return BadRequest("Token và mật khẩu không được để trống.");

            var result = await _userService.ResetPasswordAsync(request.Token, request.NewPassword);
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
            var user = await _userService.CheckLoginAsync(loginVm);
            if (user == null)
            {
                return NotFound();
            }

            var token = await GenerateJwtToken(user);
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
            new Claim("email", user.Email),
            new Claim("id", user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userWithRole = await _userService.TakeRoleAsync(user);
            if (userWithRole != null && userWithRole.Role != null)
            {
                authClaims.Add(new Claim("role", userWithRole.Role.Name));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
    public class ResetPasswordRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}



