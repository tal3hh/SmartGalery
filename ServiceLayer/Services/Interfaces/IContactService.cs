using ServiceLayer.Dtos.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface IContactService
    {
        Task<List<ContactDto>> GetAllAsync();
        Task<ContactDto> GetByIdAsync(int id);
        Task CreateAsync(ContactCreateDto dto);
        Task RemoveAsync(int id);
    }
}
