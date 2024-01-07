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
        Task<List<ProductDto>> GetAllAsync();
        Task<Paginate<ProductDto>> ProductPagineList(PaginationVM vm);
        Task<Paginate<ProductDto>> GetProductsAsync(ProductFilterVM filter);
        Task<ProductDto> GetByIdAsync(int id);
        Task CreateAsync(ProductCreateDto dto);
        Task UpdateAsync(ProductUpdateDto dto);
        Task RemoveAsync(int id);
    }
}
