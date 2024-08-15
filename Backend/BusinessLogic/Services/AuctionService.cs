using DataAccess.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Services
{

    public class AuctionService : IAuctionService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly AuctionDbContext _context;

        public AuctionService(IAuctionRepository auctionRepository, AuctionDbContext context)
        {
            _auctionRepository = auctionRepository;
            _context = context;
        }

        public async Task<Auction> CreateAuction(Auction auction)
        {
            return await _auctionRepository.CreateAuction(auction);
        }

        public async Task<bool> DeleteAuction(int id)
        {
            return await _auctionRepository.DeleteAuction(id);
        }

        public async Task<Auction> UpdateAuction(int id, Auction auction)
        {
            return await _auctionRepository.UpdateAuction(id, auction);
        }

        public async Task<IEnumerable<Auction>> GetAllAuctions()
        {
            return await _auctionRepository.GetAllAuctions();
        }

        public async Task<IEnumerable<Auction>> GetAllAuctionsByUserId(int userId)
        {
            return await _auctionRepository.GetAllAuctionsByUserId(userId);
        }
        public async Task<IEnumerable<Auction>> GetOngoingAuctions()
        {
            return await _auctionRepository.GetOngoingAuctions();
        }
       public async Task<Auction> GetAuctionById(int auctionId)
        {
            return await _auctionRepository.GetAuctionById(auctionId);
        }

        public async Task EndAuction(int auctionId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var auction = await _auctionRepository.GetAuctionById(auctionId);

                    if (auction != null && auction.EndTime <= DateTime.Now)
                    {
                        foreach (var product in auction.Products)
                        {
                            var highestBid = await _context.Bids
                                .Where(b => b.ProductId == product.Id)
                                .OrderByDescending(b => b.Amount)
                                .FirstOrDefaultAsync();

                            if (highestBid != null)
                            {
                                // Explicitly attach the product entity
                                _context.Products.Attach(product);

                                product.IsSold = true;
                                product.BuyerId = highestBid.UserId;
                                Console.WriteLine($"Product {product.Name} is marked as sold.");

                                var user = await _context.Users
                                    .Include(u => u.BoughtProducts)
                                    .SingleOrDefaultAsync(u => u.Id == highestBid.UserId);

                                if (user != null)
                                {
                                    // Explicitly attach the user entity
                                    _context.Users.Attach(user);

                                    user.BoughtProducts.Add(product);
                                    Console.WriteLine($"Product {product.Name} added to user {user.Email}'s BoughtProducts.");
                                }
                                else
                                {
                                    Console.WriteLine($"User with ID {highestBid.UserId} not found.");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"No bids found for product {product.Name}.");
                            }
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        Console.WriteLine("Auction processing completed successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Auction with ID {auctionId} either doesn't exist or has not ended yet.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during auction processing: " + ex.Message);
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }



        public async Task ProcessAuctions()
            {
            var ongoingAuctions = await _auctionRepository.GetAllAuctions();

                foreach (var auction in ongoingAuctions)
                {
                    await EndAuction(auction.Id);
                }
            }
        

    }

}
