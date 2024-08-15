using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Web.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;

        public BidController(IBidService bidService)
        {
            _bidService = bidService;
        }

        [HttpGet("ByProductId/{productId}")]
        [Authorize]
        public async Task<IActionResult> GetBidsByProductId(int productId)
        {
            var bids = await _bidService.GetBidsByProductId(productId);
            return Ok(bids);
        }

        [HttpGet("ByUserId/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetBidsByUserId(int userId)
        {
            try
            {
                var bids = await _bidService.GetBidsByUserId(userId);
                return Ok(bids);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("AddBid")]
        [Authorize]
        public async Task<IActionResult> AddBid([FromBody] Bid bid)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                Console.WriteLine($"Received bid amount: {bid.Amount}");
                await _bidService.AddBid(bid);
                return CreatedAtAction(nameof(GetBidsByProductId), new { productId = bid.ProductId }, bid);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBid(int id)
        {
            try
            {
                var deleted = await _bidService.DeleteBid(id);
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
        public async Task<IActionResult> UpdateBid(int id, [FromBody] Bid bid)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedBid = await _bidService.UpdateBid(id, bid);
                if (updatedBid == null)
                    return NotFound();

                return Ok(updatedBid);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllBids()
        {
            try
            {
                var bids = await _bidService.GetAllBids();
                return Ok(bids);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}
