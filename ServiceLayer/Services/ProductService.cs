using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Product;
using ServiceLayer.Services.Interfaces;
using ServiceLayer.Utilities;
using ServiceLayer.ViewModels;
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

        public async Task<Paginate<ProductDto>> ProductPagineList(PaginationVM vm)
        {
            var entities = from b in _context.Products?
                           .Where(x => x.CategoryId == vm.CategoryId)
                           .Include(x => x.Category)
                           select b;
            //Sort Filter
            //switch (sort)
            //{
            //    case "price_desc":
            //        entities = entities.OrderByDescending(x => x.Price);
            //        break;
            //    case "price_asc":
            //        entities = entities.OrderBy(x => x.Price);
            //        break;
            //    case "name_desc":
            //        entities = entities.OrderByDescending(x => x.Name);
            //        break;
            //    default:
            //        entities = entities.OrderBy(x => x.Name);
            //        break;
            //}

            //Paginate
            var allCount = await entities.CountAsync();
            var Totalpage = (int)Math.Ceiling((decimal)allCount / vm.Take);

            var entities2 = await entities.Skip((vm.Page - 1) * vm.Take).Take(vm.Take).ToListAsync();

            List<ProductDto> dto = _mapper.Map<List<ProductDto>>(entities2);

            var result = new Paginate<ProductDto>(dto, vm.Take, Totalpage);

            return result;
        }

        private async Task<List<ProductDto>> GetProducts(ProductFilterVM filter)
        {
            List<Product> entity = await _context.Products.Where(p =>
                    (p.CategoryId == filter.CategoryId) &&
                    (p.Price >= filter.PriceMIN && p.Price <= filter.PriceMAX) &&
                    (p.Color == filter.Color)
                ).ToListAsync();

            return _mapper.Map<List<ProductDto>>(entity);
        }

        public async Task<Paginate<ProductDto>> GetProductsAsync(ProductFilterVM filter)
        {
            List<ProductDto> filteredProducts = await GetProducts(filter);

            // Pagination 
            int pageSize = filter.Take;
            int totalProducts = filteredProducts.Count;
            int totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

            int currentPage = filter.Page;
            List<ProductDto> pagedProducts = filteredProducts
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            
            Paginate<ProductDto> paginateResult = new Paginate<ProductDto>(pagedProducts, currentPage, totalPages);
            return await Task.FromResult(paginateResult);
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
