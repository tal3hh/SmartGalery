using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Brand;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class BrandService : IBrandService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BrandService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<BrandDto>> GetAllAsync()
        {
            List<Brand>? list = await _context.Brands.AsNoTracking().ToListAsync();

            return _mapper.Map<List<BrandDto>>(list);
        }


        public async Task<BrandDto> GetByIdAsync(int id)
        {
            Brand? entity = await _context.Brands.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<BrandDto>(entity);
        }

        public async Task<List<BrandDto>> GetByNameAsync(string name)
        {
            List<Brand>? entity = await _context.Brands.AsNoTracking().Where(x => x.Name == name).ToListAsync();

            return _mapper.Map<List<BrandDto>>(entity);
        }


        public async Task CreateAsync(BrandCreateDto dto)
        {
            Brand entity = _mapper.Map<Brand>(dto);

            await _context.Brands.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(BrandUpdateDto dto)
        {
            Brand? DBentity = await _context.Brands.AsNoTracking().SingleOrDefaultAsync(x => x.Id == dto.Id);
            if (DBentity != null)
            {
                Brand entity = _mapper.Map<Brand>(dto);
                _context.Brands.Update(entity);
                await _context.SaveChangesAsync();
            }
        }


        public async Task RemoveAsync(int id)
        {
            Brand? entity = await _context.Brands.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _context.Brands.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
