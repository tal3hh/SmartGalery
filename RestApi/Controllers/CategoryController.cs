using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Dtos.Category;
using ServiceLayer.Services.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _categoryService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateDto dto)
        {
            if(!ModelState.IsValid) return BadRequest();

            await _categoryService.CreateAsync(dto);

            return Ok(dto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _categoryService.UpdateAsync(dto);

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Update(int id)
        {
            await _categoryService.RemoveAsync(id);

            return Ok();
        }
    }
}
