﻿using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _ContactService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContactCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                             .Where(x => x.Value.Errors.Any())
                             .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());
                return BadRequest(errors);
            }

            await _ContactService.CreateAsync(dto);

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ContactService.RemoveAsync(id);

            return Ok();
        }
    }
}