using Web_Server.Interfaces;
using Web_Server.Models;
using Web_Server.ViewModels;

namespace Web_Server.Services
{
    public class UserService : IUserService
    {
        public IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> CheckLoginAsync(LoginVm loginVm)
        {
            var User = await FindEmailExists(loginVm.Email);
            if (User == null)
            {
                return null;
            }
            return await _repository.CheckLoginAsync(loginVm);
        }

        public async Task<User> FindEmailExists(string email)
        {
            return await _repository.FindEmailExists(email);
        }

        public async Task<List<CandidateVm>> GetCandidateByPostId(int id)
        {
            return await _repository.GetCandidateByPostId(id);
        }

        public Task<CV> GetCVByUserId(int id)
        {
            return _repository.GetCVByUserId(id);
        }

        public async Task<bool> RegisterAysnc(RegisterVm registerVm)
        {
            var User = await FindEmailExists(registerVm.Email);
            if(User != null)
            {
                return false;
            }
            var result = await _repository.RegisterAysnc(registerVm);
            return result;

        }

        public async Task<User> TakeRoleAsync(User user)
        {
            return await _repository.TakeRoleAsync(user);
        }
    }
}
