using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Dtos.About;
using ServiceLayer.Services.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutController : ControllerBase
    {
        readonly IAboutService _AboutService;

        public AboutController(IAboutService AboutService)
        {
            _AboutService = AboutService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _AboutService.HomeGetAllAsync());
        }
    }
}
