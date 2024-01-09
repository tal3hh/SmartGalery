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

        #region Dashboard

        //Dahsboard Product Filter
        public async Task<Paginate<ProductDto>> DashProductSearch(ProductFilterVM filter)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(p => p.Name.Contains(filter.Search));
            }

            if (filter.CategoryId?.Count() > 0)
            {
                foreach (var categoryId in filter.CategoryId)
                {
                    query = query.Where(p => p.CategoryId == categoryId);
                }
            }

            //if (filter.CategoryId != 0)
            //{
            //    query = query.Where(p => p.CategoryId == filter.CategoryId);
            //}

            query = query.OrderByDescending(p => p.Id);

            int totalCount = await query.CountAsync();
            int take = filter.Take > 0 ? filter.Take : 10;
            int totalPages = (int)Math.Ceiling(totalCount / (double)take);

            int currentPage = filter.Page > 0 ? filter.Page : 1;

            List<Product> products = await query
                .Skip((currentPage - 1) * take)
                .Take(take)
                .ToListAsync();

            List<ProductDto> productDto = _mapper.Map<List<ProductDto>>(products);

            return new Paginate<ProductDto>(productDto, currentPage, totalPages);
        }

        #endregion


        #region Home
        //Home Product Filter 
        public async Task<List<ProductDto>> HomeProductFilter(ProductFilterVM filter)
        {
            var query = _context.Products
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .AsQueryable();

            if (filter.CategoryId?.Count() > 0)
            {
                foreach (var categoryId in filter.CategoryId)
                {
                    query = query.Where(p => p.CategoryId == categoryId);
                }
            }

            //if (filter.CategoryId != 0)
            //{
            //    query = query.Where(p => p.CategoryId == filter.CategoryId);
            //}

            if (!string.IsNullOrEmpty(filter.Color))
            {
                query = query.Where(p => p.Color == filter.Color);
            }

            if (filter.PriceMIN > 0)
            {
                query = query.Where(p => p.Price >= filter.PriceMIN);
            }

            if (filter.PriceMAX > 0)
            {
                query = query.Where(p => p.Price <= filter.PriceMAX);
            }

            int totalCount = await query.CountAsync();
            int take = filter.Take > 0 ? filter.Take : 20;
            int totalPages = (int)Math.Ceiling(totalCount / (double)take);

            int currentPage = filter.Page > 0 ? filter.Page : 1;

            List<ProductDto> productDtos = await query
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    OldPrice = p.OldPrice,
                    IsStock = p.IsStock,
                    About = p.About,
                    commentCount = p.Comments.Count(),
                    ProductImages = p.ProductImages.Select(pi => pi.Path).ToList(),
                    Rating = p.Ratings.Any() ? p.Ratings.Average(r => r.Star) : 0
                })
                .Skip((currentPage - 1) * take)
                .Take(take)
                .ToListAsync();

            List<ProductDto> productDto = _mapper.Map<List<ProductDto>>(productDtos);
            return productDtos;
        }

        //New Product List
        public async Task<List<ProductDto>> NewProductList()
        {
            var products = await _context.Products
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    OldPrice = p.OldPrice,
                    IsStock = p.IsStock,
                    About = p.About,
                    commentCount = p.Comments.Count(),
                    ProductImages = p.ProductImages.Select(pi => pi.Path).ToList(),
                    Rating = p.Ratings.Any() ? p.Ratings.Average(r => r.Star) : 0
                })
                .ToListAsync();

            return products;
        }

        // Product Detail Page
        public async Task<ProductDto> ProductDetailPage(int id)
        {
            ProductDto productDto = await _context.Products
                                            .Include(p => p.ProductImages)
                                            .Include(p => p.ProductDetails)
                                            .Select(p => new ProductDto
                                            {
                                                Name = p.Name,
                                                Color = p.Color,
                                                About = p.About,
                                                ProductImages = p.ProductImages.Select(pi => pi.Path).ToList(),
                                                ProductDetails = p.ProductDetails.Select(pi => pi.Description).ToList()
                                            })
                                            .FirstOrDefaultAsync(p => p.Id == id);

            return productDto;
        }
        #endregion


        #region Crud
        public async Task<List<ProductDto>> GetAllAsync()
        {
            var list = await _context.Products.OrderByDescending(x => x.CreateDate).AsNoTracking().ToListAsync();

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
        #endregion
    }
}
