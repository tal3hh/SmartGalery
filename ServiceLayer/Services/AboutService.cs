    using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.About;
using ServiceLayer.Dtos.Category;
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

        public async Task<List<HomeAboutDto>> HomeGetAllAsync()
        {
            List<About>? list = await _context.Abouts.AsNoTracking().ToListAsync();

            return _mapper.Map<List<HomeAboutDto>>(list);
        }

        public async Task<List<AboutDto>> GetAllAsync()
        {
            List<About>? list = await _context.Abouts.AsNoTracking().ToListAsync();

            return _mapper.Map<List<AboutDto>>(list);
        }


        public async Task<List<AboutDto>> GetByNameAsync(string name)
        {
            List<About>? entity = await _context.Abouts.AsNoTracking().Where(x => x.Title.Contains(name)).ToListAsync();

            return _mapper.Map<List<AboutDto>>(entity);
        }


        public async Task CreateAsync(AboutCreateDto dto)
        {
            About entity = _mapper.Map<About>(dto);

            await _context.Abouts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(AboutUpdateDto dto)
        {
            About? DBentity = await _context.Abouts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == dto.Id);
            if(DBentity != null)
            {
                About entity = _mapper.Map<About>(dto);
                _context.Abouts.Update(entity);
                await _context.SaveChangesAsync();
            }
        }


        public async Task RemoveAsync(int id)
        {
            About? entity = await _context.Abouts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _context.Abouts.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
