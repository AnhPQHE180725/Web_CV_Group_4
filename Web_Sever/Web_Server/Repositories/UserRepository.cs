﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web_Server.Data;
using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> FindEmailExists(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<User> CheckLoginAsync(LoginVm loginVm)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == loginVm.Email && u.Password == loginVm.Password);
        }

        public async Task<User> TakeRoleAsync(User user)
        {
            return await _context.Users
                                    .Include(u => u.Role)
                                    .FirstOrDefaultAsync(u => u.Id == user.Id);
        }

        public async Task<bool> RegisterAysnc(RegisterVm registerVm)
        {
            var user = new User
            {
                FullName = registerVm.FullName,
                Email = registerVm.Email,
                Password = registerVm.Password,
                RoleId = _context.Roles
                 .Where(r => r.Name == registerVm.RoleName)
                 .Select(r => r.Id)
                 .FirstOrDefault(),
                Status = 1
            };

            await _context.Users.AddAsync(user);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<List<CandidateVm>> GetCandidateByPostId(int id)
        {
            return await _context.ApplyPosts
        .Where(a => a.RecruitmentId == id)
        .Select(a => new CandidateVm
        {
            postId = a.Id,
            id = a.User.Id,
            Address = a.User.Address,
            FullName = a.User.FullName,
            Email = a.User.Email,
            PhoneNumber = a.User.PhoneNumber,
            Image = a.User.Image,
            Description = a.User.Description,

            CVStatus = a.Status
        })
        .ToListAsync();
        }

        public async Task<CV> GetCVByUserId(int id)
        {
            return await _context.CVs.FirstOrDefaultAsync(c => c.UserId == id);
        }

        public async Task UpdatePasswordAsync(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                var passwordHasher = new PasswordHasher<User>();
                user.Password = passwordHasher.HashPassword(user, newPassword);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<ApplyPost> ApplyCV(int id)
        {
            var applyPost = await _context.ApplyPosts
         .Include(a => a.User)
         .Include(a => a.Recruitment)

                 .ThenInclude(u => u.Company)
         .FirstOrDefaultAsync(a => a.Id == id);

            if (applyPost == null)
            {
                throw new Exception("ApplyPost not found.");
            }


            applyPost.Status = 2;


            await _context.SaveChangesAsync();

            return applyPost;
        }

        public async Task<ApplyPost> RejectCV(int id)
        {
            var applyPost = await _context.ApplyPosts
                
        .Include(a => a.User)
        .Include(a => a.Recruitment)

                .ThenInclude(u => u.Company)
        .FirstOrDefaultAsync(a => a.Id == id);

            if (applyPost == null)
            {
                throw new Exception("ApplyPost not found.");
            }


            applyPost.Status = 1;


            await _context.SaveChangesAsync();

            return applyPost;
        }


        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.CV)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> UpdateProfileAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateEmailAsync(int userId, string newEmail)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Email = newEmail;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsTakenEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
