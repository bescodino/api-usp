using AutoMapper;
using LabSid.Api.Models;
using LabSid.Models;
using LabSid.Models.Interfaces;
using LabSid.Services.DTO;
using LabSid.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LabSid.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;


        public UserController(ILogger<UserController> logger, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize]
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

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] UserCreateModel model)
        {
            try
            {
                var mapModel = this._mapper.Map<User>(model);
                var createdUser = await this._userService.SaveAsync(mapModel).ConfigureAwait(false);
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateModel model)
        {
            try
            {
                var mapModel = this._mapper.Map<User>(model);
                var updateUser = await this._userService.SaveAsync(mapModel).ConfigureAwait(false);
                return Ok(updateUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] string email, [FromQuery] string password)
        {
            try
            {
                var login = await this._userService.Login(email, password).ConfigureAwait(false);
                return Ok(login);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}