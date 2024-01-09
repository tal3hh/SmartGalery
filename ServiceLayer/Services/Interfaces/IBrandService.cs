using ServiceLayer.Dtos.Brand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetAllAsync();
        Task<BrandDto> GetByIdAsync(int id);
        Task<List<BrandDto>> GetByNameAsync(string name);
        Task CreateAsync(BrandCreateDto dto);
        Task UpdateAsync(BrandUpdateDto dto);
        Task RemoveAsync(int id);
    }
}
