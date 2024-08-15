using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : ControllerBase
    {
        private readonly IAuctionService _auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateAuction([FromBody] Auction auction)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _auctionService.CreateAuction(auction);
                return CreatedAtAction(nameof(GetAllAuctions), new { id = auction.Id }, auction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAuction(int id)
        {
            try
            {
                var deleted = await _auctionService.DeleteAuction(id);
                if (!deleted)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAuction(int id, [FromBody] Auction auction)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedAuction = await _auctionService.UpdateAuction(id, auction);
                if (updatedAuction == null)
                    return NotFound();

                return Ok(updatedAuction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAuctions()
        {
            try
            {
                var auctions = await _auctionService.GetAllAuctions();
                return Ok(auctions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("ByUserId/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetAllAuctionsByUserId(int userId)
        {
            try
            {
                var auctions = await _auctionService.GetAllAuctionsByUserId(userId);
                return Ok(auctions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("GetAuction/{auctionId}")]
        [Authorize]
        public async Task<IActionResult>GetAuctionById(int auctionId)
        {
            try
            {
                var auction = await _auctionService.GetAuctionById(auctionId);
                return Ok(auction);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("ongoing")]
        [Authorize]
        public async Task<IActionResult> GetOngoingAuctions()
        {
            try
            {
                var ongoingAuctions = await _auctionService.GetOngoingAuctions();
                return Ok(ongoingAuctions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}
