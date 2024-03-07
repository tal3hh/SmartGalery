using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Dtos.Contact;
using ServiceLayer.Services.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        readonly IContactService _ContactService;

        public ContactController(IContactService ContactService)
        {
            _ContactService = ContactService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContactCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(dto);

            await _ContactService.CreateAsync(dto);

            return Ok();
        }
    }
}
