using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Dtos.ProductImage;
using ServiceLayer.Services.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImageController : ControllerBase
    {
        readonly IProductImageService _ProductImageService;

        public ProductImageController(IProductImageService ProductImageService)
        {
            _ProductImageService = ProductImageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _ProductImageService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductImageCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                             .Where(x => x.Value.Errors.Any())
                             .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());
                return BadRequest(errors);
            }

            await _ProductImageService.CreateAsync(dto);

            return Ok(dto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductImageUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                             .Where(x => x.Value.Errors.Any())
                             .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());
                return BadRequest(errors);
            }

            await _ProductImageService.UpdateAsync(dto);

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ProductImageService.RemoveAsync(id);

            return Ok();
        }
    }
}
