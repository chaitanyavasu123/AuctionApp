namespace DataAccess.DataAccess
{

    public interface IAuctionRepository
    {
        Task<Auction> CreateAuction(Auction auction);
        Task<bool> DeleteAuction(int id);
        Task<Auction> UpdateAuction(int id, Auction auction);
        Task<IEnumerable<Auction>> GetAllAuctions();
        Task<IEnumerable<Auction>> GetAllAuctionsByUserId(int userId);
        Task<IEnumerable<Auction>> GetOngoingAuctions();
        Task<Auction> GetAuctionById(int auctionId);
    }

}
