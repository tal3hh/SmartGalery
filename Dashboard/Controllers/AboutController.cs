using Microsoft.AspNetCore.Authorization;
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
            return Ok(await _AboutService.GetAllAsync());
        }

        [HttpGet("{title}")]
        public async Task<IActionResult> Search(string title)
        {
            if (string.IsNullOrEmpty(title)) return BadRequest(title);

            return Ok(await _AboutService.GetByNameAsync(title));
        }

        [HttpPost]
        public async Task<IActionResult> Create(AboutCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(dto);
            
            await _AboutService.CreateAsync(dto);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(AboutUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(dto);

            await _AboutService.UpdateAsync(dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _AboutService.RemoveAsync(id);

            return Ok();
        }
    }
}
