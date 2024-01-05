using AutoMapper;
using DomainLayer.Entities;
using ServiceLayer.Dtos.About;
using ServiceLayer.Dtos.Category;
using ServiceLayer.Dtos.Comment;
using ServiceLayer.Dtos.Contact;
using ServiceLayer.Dtos.Product;
using ServiceLayer.Dtos.ProductDetail;
using ServiceLayer.Dtos.ProductImage;
using ServiceLayer.Dtos.Rating;
using ServiceLayer.Dtos.Subscribe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Mapping
{
    public class MappingProofile : Profile
    {
        public MappingProofile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();

            CreateMap<About, AboutDto>().ReverseMap();
            CreateMap<About, AboutCreateDto>().ReverseMap();
            CreateMap<About, AboutUpdateDto>().ReverseMap();

            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CommentCreateDto>().ReverseMap();

            CreateMap<Contact, ContactDto>().ReverseMap();
            CreateMap<Contact, ContactCreateDto>().ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();

            CreateMap<ProductDetail, ProductDetailDto>().ReverseMap();
            CreateMap<ProductDetail, ProductDetailCreateDto>().ReverseMap();
            CreateMap<ProductDetail, ProductDetailUpdateDto>().ReverseMap();

            CreateMap<ProductImage, ProductImageDto>().ReverseMap();
            CreateMap<ProductImage, ProductImageCreateDto>().ReverseMap();
            CreateMap<ProductImage, ProductImageUpdateDto>().ReverseMap();

            CreateMap<Rating, RatingDto>().ReverseMap();
            CreateMap<Rating, RatingCreateDto>().ReverseMap();
            CreateMap<Rating, RatingUpdateDto>().ReverseMap();

            CreateMap<Subscribe, SubscribeDto>().ReverseMap();
            CreateMap<Subscribe, SubscribeCreateDto>().ReverseMap();
        }
    }
}
