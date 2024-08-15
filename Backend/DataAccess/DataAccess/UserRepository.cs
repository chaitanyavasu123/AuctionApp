using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private readonly AuctionDbContext _context;

        public UserRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> GetByUserId(int id)
        {
            return await _context.Users.SingleOrDefaultAsync(u=> u.Id == id);
        }

        public async Task<User> GetUserWithBoughtProducts(int userId)
        {
            return await _context.Users
                .Include(u => u.BoughtProducts)
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User> GetUserWithSoldProducts(int userId)
        {
            return await _context.Users
                .Include(u => u.SoldProducts)
                .SingleOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<List<Product>> GetUnsoldProductsByUserId(int userId)
        {
            var unsoldProducts = await _context.Products
                .Where(p => p.SellerId == userId && !p.IsSold && p.AuctionId == null)
                .ToListAsync();

            return unsoldProducts;
        }



    }
}
