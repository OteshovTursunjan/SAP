using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using SAP.Application.Service;
using SAP.Application.Model.User;

namespace SAP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SapController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseUrl = "https://su15-04.sb1.cloud/ServiceLayer/b1s/v2/";
        private string _sessionId = string.Empty;
        private readonly IUserService _userService;
        public SapController(IHttpClientFactory httpClientFactory,IUserService userService)
        {
            _httpClientFactory = httpClientFactory;
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUser([FromBody]LoginModel login)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var res = await _userService.Login(login);
            return Ok(res);
        }

     
    }
}
