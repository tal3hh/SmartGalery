using ServiceLayer.Dtos.ProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IProductImageService
    {
        Task<List<ProductImageDto>> GetAllAsync();
        Task<ProductImageDto> GetByIdAsync(int id);
        Task CreateAsync(ProductImageCreateDto dto);
        Task UpdateAsync(ProductImageUpdateDto dto);
        Task RemoveAsync(int id);
    }
}
