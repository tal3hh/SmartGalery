using ServiceLayer.Dtos.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services.Interfaces
{
    public interface ICommentService
    {
        Task<List<CommentDto>> GetAllAsync();
        Task<CommentDto> GetByIdAsync(int id);
        Task CreateAsync(CommentCreateDto dto);
        Task RemoveAsync(int id);
    }
}
