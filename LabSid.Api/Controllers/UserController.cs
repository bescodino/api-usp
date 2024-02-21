using LabSid.Api.DTO;
using Microsoft.AspNetCore.Mvc;

namespace LabSid.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{ano}/{mes}")]
        public async Task<ActionResult<UserDTO>> Get(long id)
        {
            try
            {
                var chuvaUgrhiMensal = await _chuvaUgrhiMensalService.GetChuvaUgrhiMensal(ano, mes).ConfigureAwait(false);
                return Ok(chuvaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}