using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Product : BaseEntity
    {
        public string? Name { get; set; }
        public decimal OldPrice { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public bool IsStock { get; set; }
        public string? PurposUse { get; set; }
        public string? Color { get; set; }
        public string? About { get; set; }

        public int CategoryId { get; set; }
        public int BrandId { get; set; }


        //Rlation Property
        public Category? Category { get; set; }
        public Brand? Brand { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public List<Rating>? Ratings { get; set; }
        public List<ProductImage>? ProductImages { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<ProductDetail>? ProductDetails { get; set; }
        public List<Wish>? Wishes { get; set; }
    }
}
