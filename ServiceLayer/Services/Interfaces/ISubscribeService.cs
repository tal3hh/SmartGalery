using ServiceLayer.Dtos.Subscribe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface ISubscribeService
    {
        Task<List<SubscribeDto>> GetAllAsync();
        Task<SubscribeDto> GetByIdAsync(int id);
        Task CreateAsync(SubscribeCreateDto dto);
        Task RemoveAsync(int id);
    }
}
