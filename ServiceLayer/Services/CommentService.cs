using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Comment;
using ServiceLayer.Dtos.Comment;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CommentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<CommentDto>> GetAllAsync()
        {
            List<Comment> list = await _context.Comments.AsNoTracking().ToListAsync();

            return _mapper.Map<List<CommentDto>>(list);
        }


        public async Task<CommentDto> GetByIdAsync(int id)
        {
            Comment? entity = await _context.Comments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<CommentDto>(entity);
        }


        public async Task CreateAsync(CommentCreateDto dto)
        {
            Comment entity = _mapper.Map<Comment>(dto);

            await _context.Comments.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task RemoveAsync(int id)
        {
            Comment? entity = await _context.Comments.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _context.Comments.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
