using Microsoft.EntityFrameworkCore;

namespace DataAccess.DataAccess
{


    public class AuctionRepository : IAuctionRepository
    {
        private readonly AuctionDbContext _context;

        public AuctionRepository(AuctionDbContext context)
        {
            _context = context;
        }

        public async Task<Auction> CreateAuction(Auction auction)
        {
            // Ensure that the ProductIds collection is not null or empty
            if (auction.ProductIds == null || !auction.ProductIds.Any())
            {
                throw new ArgumentException("No valid product IDs provided.");
            }

            // Fetch the existing products from the database using the provided product IDs
            var existingProducts = await _context.Products
                                                 .Where(p => auction.ProductIds.Contains(p.Id))
                                                 .ToListAsync();

            if (existingProducts.Count == 0)
            {
                throw new ArgumentException("No valid products found for the given product IDs.");
            }

            // Calculate the auction duration in hours
            double auctionDurationInHours = (auction.EndTime - auction.StartTime).TotalHours;

            foreach (var product in existingProducts)
            {
                // Set the auction duration in hours for each product
                product.AuctionDuration = (int)auctionDurationInHours;

                // Set the auction ID for each product
                product.AuctionId = auction.Id;
            }

            // Associate the existing products with the auction
            auction.Products = existingProducts;

            // Add the auction to the context and save changes
            _context.Auctions.Add(auction);
            await _context.SaveChangesAsync();

            return auction;
        }





        public async Task<bool> DeleteAuction(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
                return false;

            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Auction> UpdateAuction(int id, Auction auction)
        {
            var existingAuction = await _context.Auctions.FindAsync(id);
            if (existingAuction == null)
                return null;

            existingAuction.Title = auction.Title;
            existingAuction.Description = auction.Description;
            existingAuction.StartTime = auction.StartTime;
            existingAuction.EndTime = auction.EndTime;

            await _context.SaveChangesAsync();
            return existingAuction;
        }

        public async Task<IEnumerable<Auction>> GetAllAuctions()
        {
            return await _context.Auctions
            .Include(a => a.Products) // Eager loading the related Product entity
            .ToListAsync();
        }

        public async Task<IEnumerable<Auction>> GetAllAuctionsByUserId(int userId)
        {
            return await _context.Auctions
            .Include(a => a.Products) // Eager loading the related Product entity
            .Where(a => a.UserId == userId)
            .ToListAsync();
        }
        public async Task<IEnumerable<Auction>> GetOngoingAuctions()
        {
            return await _context.Auctions
                .Where(a => a.EndTime > DateTime.Now)
                .Include(a => a.Products)
                .ToListAsync();
        }
        public async Task<Auction> GetAuctionById(int auctionId)
        {
            return await _context.Auctions.
                Include(a => a.Products).
                FirstOrDefaultAsync(a => a.Id == auctionId);


        }
    }

}
