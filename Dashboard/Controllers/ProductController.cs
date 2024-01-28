using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Dtos.Product;
using ServiceLayer.Services.Interfaces;
using ServiceLayer.ViewModels;

namespace Dashboard.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        readonly IProductService _ProductService;

        public ProductController(IProductService ProductService)
        {
            _ProductService = ProductService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _ProductService.GetAllAsync());
        }

        [HttpPost("SearchDashProduct")]
        public async Task<IActionResult> SearchDashProduct(DashProductSearchVM vm)
        {
            return Ok(await _ProductService.DashProductSearch(vm));
        }

        [HttpPost("DashCategoryFilter")]
        public async Task<IActionResult> DashCategoryProduct(DashCategoryProductVM vm)
        {
            return Ok(await _ProductService.DashCategoryProduct(vm));
        }

        [HttpPost("DashBrandFilter")]
        public async Task<IActionResult> DashBrandProduct(DashBrandProductVM vm)
        {
            return Ok(await _ProductService.DashBrandProduct(vm));
        }

        [HttpPost("DashProductDetail")]
        public async Task<IActionResult> DashProductDetail(int id)
        {
            return Ok(await _ProductService.DashProductDetail(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                             .Where(x => x.Value.Errors.Any())
                             .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());
                return BadRequest(errors);
            }

            await _ProductService.CreateAsync(dto);

            return Ok(dto);
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                             .Where(x => x.Value.Errors.Any())
                             .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());
                return BadRequest(errors);
            }

            await _ProductService.UpdateAsync(dto);

            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ProductService.RemoveAsync(id);

            return Ok();
        }
    }
}
