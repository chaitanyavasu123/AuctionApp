using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataAccess
{

    public class BidRepository : IBidRepository
    {
        private readonly AuctionDbContext _context;

        public BidRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bid>> GetBidsByProductId(int productId)
        {
            return await _context.Bids.Where(b => b.ProductId == productId).ToListAsync();
        }

        public async Task<IEnumerable<Bid>> GetBidsByUserId(int userId)
        {
            return await _context.Bids
        .Where(b => b.UserId == userId)
        .Include(b => b.Product) // Include the related Product entity
        .ToListAsync();
        }

        public async Task<Bid> AddBid(Bid bid)
        { 
            var product= await _context.Products.FindAsync(bid.ProductId);
            if (product != null)
            {
                bid.Product=product;
            }
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
            return bid;
        }

        public async Task<bool> DeleteBid(int id)
        {
            var bid = await _context.Bids.FindAsync(id);
            if (bid == null)
                return false;

            _context.Bids.Remove(bid);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Bid> UpdateBid(int id, Bid bid)
        {
            var existingBid = await _context.Bids.FindAsync(id);
            if (existingBid == null)
                return null;

            existingBid.Amount = bid.Amount;
            existingBid.ProductId = bid.ProductId;
            existingBid.UserId = bid.UserId;

            await _context.SaveChangesAsync();
            return existingBid;
        }

        public async Task<IEnumerable<Bid>> GetAllBids()
        {
            return await _context.Bids.ToListAsync();
        }
    }

}
