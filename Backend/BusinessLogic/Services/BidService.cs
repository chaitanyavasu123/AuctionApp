namespace BusinessLogic.Services
{
    using DataAccess.DataAccess;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly IProductRepository _productRepository;

        public BidService(IBidRepository bidRepository, IProductRepository productRepository)
        {
            _bidRepository = bidRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Bid>> GetBidsByProductId(int productId)
        {
            return await _bidRepository.GetBidsByProductId(productId);
        }

        public async Task<IEnumerable<Bid>> GetBidsByUserId(int userId)
        {
            return await _bidRepository.GetBidsByUserId(userId);
        }

        public async Task<Bid> AddBid(Bid bid)
        {
            var product= await _productRepository.GetProductById(bid.ProductId);
            if (product != null)
            {
               
                product.StartingPrice = bid.Amount;
                await _productRepository.UpdateProduct(bid.ProductId, product);
            }
            
            return await _bidRepository.AddBid(bid);
        }

        public async Task<bool> DeleteBid(int id)
        {
            return await _bidRepository.DeleteBid(id);
        }

        public async Task<Bid> UpdateBid(int id, Bid bid)
        {
            return await _bidRepository.UpdateBid(id, bid);
        }

        public async Task<IEnumerable<Bid>> GetAllBids()
        {
            return await _bidRepository.GetAllBids();
        }
    }

}
