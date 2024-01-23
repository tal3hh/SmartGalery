using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Product;
using ServiceLayer.Dtos.Product.Dash;
using ServiceLayer.Dtos.Product.Home;
using ServiceLayer.Services.Interfaces;
using ServiceLayer.Utilities;
using ServiceLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        //Dahsboard Products Filter(and List)
        public async Task<Paginate<DashProDto>> DashProductSearch(DashProductSearchVM vm)
        {
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .AsQueryable();

            if (!string.IsNullOrEmpty(vm.Search))
            {
                query = query.Where(p => p.Name.Contains(vm.Search));
            }

            int totalCount = await query.CountAsync();
            int take = vm.Take > 0 ? vm.Take : 10;
            int totalPages = (int)Math.Ceiling(totalCount / (double)take);

            int currentPage = vm.Page > 0 ? vm.Page : 1;

            List<DashProDto> productDtos = await query
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new DashProDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    OldPrice = p.OldPrice,
                    Count = p.Count,
                    IsStock = p.IsStock,

                    CategoryName = p.Category.Name,
                    BrandName = p.Brand.Name,

                    ProductImages = p.ProductImages.Select(pi => pi.Path).ToList(),
                })
                .Skip((currentPage - 1) * take)
                .Take(take)
                .ToListAsync();

            return new Paginate<DashProDto>(productDtos, currentPage, totalPages);
        }

        //Dahsboard Products Details
        public async Task<DashProDetailDto?> DashProductDetail(int id)
        {
            if (!await _context.Products.AnyAsync(x => x.Id == id))
                return null;

            DashProDetailDto? dto = await _context.Products
                .Where(x => x.Id == id)
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .Include(p => p.ProductDetails)
                .Select(p => new DashProDetailDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    OldPrice = p.OldPrice,
                    Count = p.Count,
                    Color = p.Color,
                    IsStock = p.IsStock,
                    About = p.About,

                    CategoryName = p.Category.Name,
                    BrandName = p.Brand.Name,

                    commentCount = p.Comments.Count(),
                    ProductImages = p.ProductImages.Select(pi => pi.Path).ToList(),
                    ProductDetails = p.ProductDetails.Select(pi => pi.Description).ToList(),
                    Rating = p.Ratings.Any() ? p.Ratings.Average(r => r.Star) : 0
                }).SingleOrDefaultAsync();

            return dto;
        }

        //Dahsboard Category Products Filter(and List)
        public async Task<Paginate<DashProDto>> DashCategoryProduct(DashCategoryProductVM vm)
        {
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .AsQueryable();

            if (vm.CategoryId > 0)
                query = query.Where(p => p.CategoryId == vm.CategoryId);

            int totalCount = await query.CountAsync();
            int take = vm.Take > 0 ? vm.Take : 10;
            int totalPages = (int)Math.Ceiling(totalCount / (double)take);

            int currentPage = vm.Page > 0 ? vm.Page : 1;

            List<DashProDto> productDtos = await query
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new DashProDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    OldPrice = p.OldPrice,
                    Count = p.Count,
                    IsStock = p.IsStock,

                    CategoryName = p.Category.Name,
                    BrandName = p.Brand.Name,

                    ProductImages = p.ProductImages.Select(pi => pi.Path).ToList(),
                })
                .Skip((currentPage - 1) * take)
                .Take(take)
                .ToListAsync();

            return new Paginate<DashProDto>(productDtos, currentPage, totalPages);
        }

        #endregion


        #region Home

        //Home Product Filter 
        public async Task<List<HomeProductDto>> HomeProductFilter(HomeProductFilterVM vm)
        {
            var query = _context.Products
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .AsQueryable();

            if (vm.CategoryId?.Count() > 0)
            {

                query = query.Where(p => vm.CategoryId.Contains(p.CategoryId));
                //foreach (var categoryId in vm.CategoryId)
                //{
                //    query = query.Where(p => p.CategoryId == categoryId);
                //}
            }

            if (vm.Color?.Count() > 0)
            {
                query = query.Where(p => vm.Color.Contains(p.Color));
                //foreach (var color in vm.Color)
                //{
                //    if (!string.IsNullOrEmpty(color))
                //        query = query.Where(p => p.Color == color);
                //}
            }

            if (vm.PriceMIN > 0)
            {
                query = query.Where(p => p.Price >= vm.PriceMIN);
            }

            if (vm.PriceMAX > 0)
            {
                query = query.Where(p => p.Price <= vm.PriceMAX);
            }

            int totalCount = await query.CountAsync();
            int take = vm.Take > 0 ? vm.Take : 20;
            int totalPages = (int)Math.Ceiling(totalCount / (double)take);

            int currentPage = vm.Page > 0 ? vm.Page : 1;

            List<HomeProductDto> productDtos = await query
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new HomeProductDto
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

            return productDtos;
        }

        //Home Product Brand Filter 
        public async Task<List<HomeProductDto>> HomeProductBrandFilter(HomeProBrandFilter vm)
        {
            var query = _context.Products
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.ProductImages)
                .AsQueryable();

            if (vm.BrandId > 0)
            {
                query = query.Where(p => p.Brand.Id == vm.BrandId);
            }

            int totalCount = await query.CountAsync();
            int take = vm.Take > 0 ? vm.Take : 20;
            int totalPages = (int)Math.Ceiling(totalCount / (double)take);

            int currentPage = vm.Page > 0 ? vm.Page : 1;

            List<HomeProductDto> productDtos = await query
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new HomeProductDto
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

            return productDtos;
        }

        //New Product List
        public async Task<List<HomeProductDto>> NewProductList()
        {
            List<HomeProductDto> products = await _context.Products
                .Include(p => p.Ratings)
                .Include(p => p.Comments)
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new HomeProductDto
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
        public async Task<HomeProDetailDto> ProductDetailPage(int productId)
        {
            HomeProDetailDto productDto = await _context.Products
                                            .Include(p => p.ProductImages)
                                            .Include(p => p.ProductDetails)
                                            .Where(p => p.Id == productId)
                                            .Select(p => new HomeProDetailDto
                                            {
                                                Name = p.Name,
                                                Color = p.Color,
                                                About = p.About,
                                                ProductImages = p.ProductImages.Select(pi => pi.Path).ToList(),
                                                ProductDetails = p.ProductDetails.Select(pi => pi.Description).ToList()
                                            })
                                            .FirstOrDefaultAsync();

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
