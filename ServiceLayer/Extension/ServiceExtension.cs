using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Dtos.About;
using ServiceLayer.Dtos.Category;
using ServiceLayer.Dtos.Comment;
using ServiceLayer.Dtos.Contact;
using ServiceLayer.Dtos.Product;
using ServiceLayer.Dtos.ProductDetail;
using ServiceLayer.Dtos.ProductImage;
using ServiceLayer.Dtos.Rating;
using ServiceLayer.Dtos.Subscribe;
using ServiceLayer.Services.Interfaces;
using ServiceLayer.Services;
using ServiceLayer.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Extension
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAboutService, AboutService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductDetailService, ProductDetailService>();
            services.AddScoped<IProductImageService, ProductImageService>();
            services.AddScoped<ISubscribeService, SubscribeService>();
        }
    }
}
