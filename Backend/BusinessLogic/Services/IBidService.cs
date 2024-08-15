namespace BusinessLogic.Services
{

    public interface IBidService
    {
        Task<IEnumerable<Bid>> GetBidsByProductId(int productId);
        Task<IEnumerable<Bid>> GetBidsByUserId(int userId);
        Task<Bid> AddBid(Bid bid);
        Task<bool> DeleteBid(int id);
        Task<Bid> UpdateBid(int id, Bid bid);
        Task<IEnumerable<Bid>> GetAllBids();
      
    }

}
