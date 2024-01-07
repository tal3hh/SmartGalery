using ServiceLayer.Dtos.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task<List<CategoryDto>> GetByNameAsync(string name);
        Task CreateAsync(CategoryCreateDto dto);
        Task UpdateAsync(CategoryUpdateDto dto);
        Task RemoveAsync(int id);
    }
}
