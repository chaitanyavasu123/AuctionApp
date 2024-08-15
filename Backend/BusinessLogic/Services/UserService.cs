using DataAccess.DataAccess;
using Models.DTOs;
    
namespace BusinessLogic.Services
{

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtUtils _jwtUtils;

        public UserService(IUserRepository userRepository, IJwtUtils jwtUtils)
        {
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            var user = await _userRepository.GetByEmail(model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
                return null;

            var token = _jwtUtils.GenerateToken(user);
            return new AuthenticateResponse(user, token);
        }
        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetByUserId(id);
        }

        public async Task<User> GetUserWithBoughtProducts(int userId)
        {
            return await _userRepository.GetUserWithBoughtProducts(userId);
        }

        public async Task<User> GetUserWithSoldProducts(int userId)
        {
            return await _userRepository.GetUserWithSoldProducts(userId);
        }

        public async Task<List<Product>> GetUnsoldProductsByUserId(int userId)
        {
            return await _userRepository.GetUnsoldProductsByUserId(userId);
        }
    }

}
