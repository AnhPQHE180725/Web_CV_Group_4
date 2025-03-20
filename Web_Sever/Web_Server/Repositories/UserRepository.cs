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

        public async Task<bool> FindEmailExists(string email)
        {
            throw new NotImplementedException();
        }


        public Task<User> CheckLoginAsync(LoginVm loginVm)
        {
            throw new NotImplementedException();
        }


    }
}
