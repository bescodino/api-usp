using LabSid.Services.DTO;
using LabSid.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LabSid.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(long id)
        {
            try
            {
                var user = await this._userService.GetByIdAsync(id).ConfigureAwait(false);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}