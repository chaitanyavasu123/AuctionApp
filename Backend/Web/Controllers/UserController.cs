using BusinessLogic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;

namespace Web.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest model)
        {
            try
            {
                var response = await _userService.Authenticate(model);
                if (response == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{userId}/bought-products")]
        [Authorize]
        public async Task<IActionResult> GetUserWithBoughtProducts(int userId)
        {
            try
            {
                var user = await _userService.GetUserWithBoughtProducts(userId);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{userId}/sold-products")]
        [Authorize]
        public async Task<IActionResult> GetUserWithSoldProducts(int userId)
        {
            try
            {
                var user = await _userService.GetUserWithSoldProducts(userId);
                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{userId}/not-sold-products")]
        [Authorize]
        public async Task<IActionResult> GetUnsoldProductsByUserId(int userId)
        {
            try
            {
                var products = await _userService.GetUnsoldProductsByUserId(userId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

}
