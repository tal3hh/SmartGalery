using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Dtos.Product;
using ServiceLayer.Services.Interfaces;
using ServiceLayer.ViewModels;

namespace Api.Controllers
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

        [HttpPost("OrderDateFilter")]
        public async Task<IActionResult> OrderDateFilter()
        {
            
            return null;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _ProductService.GetAllAsync());
        }

        [HttpPost("DashProductSearch")]
        public async Task<IActionResult> DashProductSearch(DashProductSearchVM vm)
        {
            return Ok(await _ProductService.DashProductSearch(vm));
        }

        [HttpPost("HomeFilter")]
        public async Task<IActionResult> HomeFilterList(HomeProductFilterVM vm)
        {
            return Ok(await _ProductService.HomeProductFilter(vm));
        }

        [HttpPost("HomeBrandFilter")]
        public async Task<IActionResult> HomeBrandFilterList(HomeProBrandFilter vm)
        {
            return Ok(await _ProductService.HomeProductBrandFilter(vm));
        }

        [HttpGet("HomeNewProducts")]
        public async Task<IActionResult> HomeNewProducts()
        {
            return Ok(await _ProductService.NewProductList());
        }

        [HttpPost("HomeProductDetail/{productId}")]
        public async Task<IActionResult> HomeProductDetail(int productId)
        {
            return Ok(await _ProductService.ProductDetailPage(productId));
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
