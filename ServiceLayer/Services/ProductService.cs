using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Product;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<ProductDto>> GetAllAsync()
        {
            var list = await _context.Set<Product>().AsNoTracking().ToListAsync();

            return _mapper.Map<List<ProductDto>>(list);
        }


        public async Task<ProductDto> GetByIdAsync(int id)
        {
            Product? entity = await _context.Products.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<ProductDto>(entity);
        }


        public async Task CreateAsync(ProductCreateDto dto)
        {
            Product entity = _mapper.Map<Product>(dto);

            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(ProductUpdateDto dto)
        {
            Product? DBentity = await _context.Products.AsNoTracking().SingleOrDefaultAsync(x => x.Id == dto.Id);
            if (DBentity != null)
            {
                Product entity = _mapper.Map<Product>(dto);
                _context.Products.Update(entity);
                await _context.SaveChangesAsync();
            }
        }


        public async Task RemoveAsync(int id)
        {
            Product? entity = await _context.Products.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _context.Products.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
