using ServiceLayer.Dtos.Product;
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
        Task<Paginate<ProductDto>> DashProductSearch(ProductFilterVM filter);
        Task<List<ProductDto>> HomeProductFilter(ProductFilterVM filter);
        Task<List<ProductDto>> NewProductList();
        Task<ProductDto> ProductDetailPage(int id);
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task CreateAsync(ProductCreateDto dto);
        Task UpdateAsync(ProductUpdateDto dto);
        Task RemoveAsync(int id);
    }
}
