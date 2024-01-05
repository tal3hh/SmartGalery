using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.About;
using ServiceLayer.Services.Interfaces;

namespace ServiceLayer.Services
{
    public class AboutService : IAboutService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public AboutService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<AboutDto>> GetAllAsync()
        {
            List<About> list = await _context.Abouts.ToListAsync();

            return _mapper.Map<List<AboutDto>>(list);
        }


        public async Task<AboutDto> GetByIdAsync(int id)
        {
            About entity = await _context.Abouts.FindAsync(id);

            return _mapper.Map<AboutDto>(entity);
        }


        public async Task CreateAsync(AboutCreateDto dto)
        {
            About entity = _mapper.Map<About>(dto);

            await _context.Abouts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(AboutUpdateDto dto)
        {
            About unchanged = await _context.Abouts.FindAsync(dto.Id);

            if (unchanged != null)
            {
                About entity = _mapper.Map<About>(dto);
                _context.Entry(unchanged).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }


        public async Task RemoveAsync(int id)
        {
            About entity = await _context.Abouts.FindAsync(id);

            if (entity != null)
            {
                _context.Abouts.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
