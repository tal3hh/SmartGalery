using ServiceLayer.Dtos.Product;
using ServiceLayer.Dtos.Product.Dash;
using ServiceLayer.Dtos.Product.Home;
using ServiceLayer.Utilities;
using ServiceLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IProductService
    {
        //Dash
        Task<Paginate<DashProDto>> DashProductSearch(DashProductSearchVM vm);
        Task<Paginate<DashProDto>> DashCategoryProduct(DashCategoryProductVM vm);
        Task<DashProDetailDto?> DashProductDetail(int id);

        //Home
        Task<List<HomeProductDto>> HomeProductFilter(HomeProductFilterVM vm);
        Task<List<HomeProductDto>> HomeProductBrandFilter(HomeProBrandFilter vm);
        Task<List<HomeProductDto>> NewProductList();
        Task<HomeProDetailDto> ProductDetailPage(int productId);

        //Crud
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task CreateAsync(ProductCreateDto dto);
        Task UpdateAsync(ProductUpdateDto dto);
        Task RemoveAsync(int id);
    }
}
