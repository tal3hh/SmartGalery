using ServiceLayer.Dtos.ProductDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IProductDetailService
    {
        Task<List<ProductDetailDto>> GetAllAsync();
        Task<ProductDetailDto> GetByIdAsync(int id);
        Task CreateAsync(ProductDetailCreateDto dto);
        Task UpdateAsync(ProductDetailUpdateDto dto);
        Task RemoveAsync(int id);
    }
}
