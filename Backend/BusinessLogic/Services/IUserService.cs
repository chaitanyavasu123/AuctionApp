namespace BusinessLogic.Services
{
    using Models.DTOs;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<User> GetUserById(int id);
        Task<User> GetUserWithBoughtProducts(int userId);
        Task<User> GetUserWithSoldProducts(int userId);
        Task<List<Product>> GetUnsoldProductsByUserId(int userId);
    }

}
