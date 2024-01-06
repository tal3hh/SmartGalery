using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.ProductDetail;
using ServiceLayer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class ProductDetailService : IProductDetailService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProductDetailService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<ProductDetailDto>> GetAllAsync()
        {
            List<ProductDetail> list = await _context.ProductDetails.AsNoTracking().ToListAsync();

            return _mapper.Map<List<ProductDetailDto>>(list);
        }


        public async Task<ProductDetailDto> GetByIdAsync(int id)
        {
            ProductDetail? entity = await _context.ProductDetails.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<ProductDetailDto>(entity);
        }


        public async Task CreateAsync(ProductDetailCreateDto dto)
        {
            ProductDetail entity = _mapper.Map<ProductDetail>(dto);

            await _context.ProductDetails.AddAsync(entity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(ProductDetailUpdateDto dto)
        {
            ProductDetail? unchanged = await _context.ProductDetails.AsNoTracking().SingleOrDefaultAsync(x => x.Id == dto.Id);

            if (unchanged != null)
            {
                ProductDetail entity = _mapper.Map<ProductDetail>(dto);
                _context.Entry(unchanged).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }


        public async Task RemoveAsync(int id)
        {
            ProductDetail? entity = await _context.ProductDetails.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _context.ProductDetails.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
