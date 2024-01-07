using ServiceLayer.Dtos.Product;
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
        Task<ProductDto> GetByIdAsync(int id);
        Task CreateAsync(ProductCreateDto dto);
        Task UpdateAsync(ProductUpdateDto dto);
        Task RemoveAsync(int id);
    }
}
