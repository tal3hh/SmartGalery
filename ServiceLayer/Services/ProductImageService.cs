using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.ProductImage;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class ProductImageService : IProductImageService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductImageService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<ProductImageDto>> GetAllAsync()
        {
            List<ProductImage> list = await _context.ProductImages.ToListAsync();

            return _mapper.Map<List<ProductImageDto>>(list);
        }


        public async Task<ProductImageDto> GetByIdAsync(int id)
        {
            ProductImage entity = await _context.ProductImages.FindAsync(id);

            return _mapper.Map<ProductImageDto>(entity);
        }


        public async Task CreateAsync(ProductImageCreateDto dto)
        {
            ProductImage entity = _mapper.Map<ProductImage>(dto);

            await _context.ProductImages.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(ProductImageUpdateDto dto)
        {
            ProductImage unchanged = await _context.ProductImages.FindAsync(dto.Id);

            if (unchanged != null)
            {
                ProductImage entity = _mapper.Map<ProductImage>(dto);
                _context.Entry(unchanged).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }


        public async Task RemoveAsync(int id)
        {
            ProductImage entity = await _context.ProductImages.FindAsync(id);

            if (entity != null)
            {
                _context.ProductImages.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
