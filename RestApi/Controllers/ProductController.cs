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
    }
}
