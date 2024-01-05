using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Category;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CategoryService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<CategoryDto>> GetAllAsync()
        {
            List<Category> list = await _context.Categories.ToListAsync();

            return _mapper.Map<List<CategoryDto>>(list);
        }


        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            Category entity = await _context.Categories.FindAsync(id);

            return _mapper.Map<CategoryDto>(entity);
        }


        public async Task CreateAsync(CategoryCreateDto dto)
        {
            Category entity = _mapper.Map<Category>(dto);

            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(CategoryUpdateDto dto)
        {
            Category unchanged = await _context.Categories.FindAsync(dto.Id);

            if (unchanged != null)
            {
                Category entity = _mapper.Map<Category>(dto);
                _context.Entry(unchanged).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }


        public async Task RemoveAsync(int id)
        {
            Category entity = await _context.Categories.FindAsync(id);

            if (entity != null)
            {
                _context.Categories.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
