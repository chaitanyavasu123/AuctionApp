namespace DataAccess.DataAccess
{


    public interface IUserRepository
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByUserId(int id);
        Task<User> GetUserWithBoughtProducts(int userId);
        Task<User> GetUserWithSoldProducts(int userId);
        Task<List<Product>> GetUnsoldProductsByUserId(int userId);
    }

}
