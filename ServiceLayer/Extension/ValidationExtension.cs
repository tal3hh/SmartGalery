using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ServiceLayer.Dtos.About;
using ServiceLayer.Dtos.Account;
using ServiceLayer.Dtos.Brand;
using ServiceLayer.Dtos.Category;
using ServiceLayer.Dtos.Comment;
using ServiceLayer.Dtos.Contact;
using ServiceLayer.Dtos.Order;
using ServiceLayer.Dtos.Product;
using ServiceLayer.Dtos.ProductDetail;
using ServiceLayer.Dtos.ProductImage;
using ServiceLayer.Dtos.Rating;
using ServiceLayer.Dtos.Subscribe;
using ServiceLayer.Validations;
using ServiceLayer.Validations.Account;
using ServiceLayer.Validations.Brand;
using ServiceLayer.Validations.ModelView;
using ServiceLayer.Validations.Order;
using ServiceLayer.ViewModels;

namespace ServiceLayer.Extension
{
    public static class ValidationExtension
    {
        public static void AddValidation(this IServiceCollection services)
        {
            services.AddScoped<IValidator<UserCreateDto>, UserCreateValidation>();
            services.AddScoped<IValidator<UserLoginDto>, UserLoginValidation>();
            services.AddScoped<IValidator<RoleCreateDto>, RoleCreateValidation>();

            services.AddScoped<IValidator<AboutCreateDto>, AboutCreateValidation>();
            services.AddScoped<IValidator<AboutUpdateDto>, AboutUpdateValidation>();

            services.AddScoped<IValidator<BrandCreateDto>, BrandCreateValidation>();
            services.AddScoped<IValidator<BrandUpdateDto>, BrandUpdateValidation>();

            services.AddScoped<IValidator<ProductCreateDto>, ProductCreateValidation>();
            services.AddScoped<IValidator<ProductUpdateDto>, ProductUpdateValidation>();

            services.AddScoped<IValidator<CategoryCreateDto>, CategoryCreateValidation>();
            services.AddScoped<IValidator<CategoryUpdateDto>, CategoryUpdateValidation>();

            services.AddScoped<IValidator<ProductDetailCreateDto>, ProductDetailCreateValidation>();
            services.AddScoped<IValidator<ProductDetailUpdateDto>, ProductDetailUpdateValidation>();

            services.AddScoped<IValidator<ProductImageCreateDto>, ProductImageCreateValidation>();
            services.AddScoped<IValidator<ProductImageUpdateDto>, ProductImageUpdateValidation>();

            services.AddScoped<IValidator<CommentCreateDto>, CommentCreateValidation>();

            services.AddScoped<IValidator<ContactCreateDto>, ContactCreateValidation>();

            services.AddScoped<IValidator<RatingCreateDto>, RatingCreateValidation>();
            services.AddScoped<IValidator<RatingUpdateDto>, RatingUpdateValidation>();

            services.AddScoped<IValidator<SubscribeCreateDto>, SubscribeCreateValidation>();

            services.AddScoped<IValidator<ShippingDto>, ShippingValidation>();

            //Model View
            services.AddScoped<IValidator<OneProductAddVM>, OneProductAddVMValidation>();
            services.AddScoped<IValidator<ManyProductAddVM>, ManyProductAddVMValidation>();
        }
        
    }
}
