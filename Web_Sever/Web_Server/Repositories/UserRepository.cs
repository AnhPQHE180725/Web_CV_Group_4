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
            return  await _context.Users
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
            return await _context.CVs.FirstOrDefaultAsync(c=>c.UserId == id);    
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Role)
                .Include(u => u.CV)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
