using ServiceLayer.Dtos.About;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IAboutService
    {
        Task<List<AboutDto>> GetAllAsync();
        Task<AboutDto> GetByIdAsync(int id);
        Task CreateAsync(AboutCreateDto dto);
        Task UpdateAsync(AboutUpdateDto dto);
        Task RemoveAsync(int id);
    }
}
