using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Contexts
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasOne(x => x.Category).WithMany(x => x.Products).HasForeignKey(x => x.CategoryId);
            modelBuilder.Entity<Product>().HasOne(x => x.Brand).WithMany(x => x.Products).HasForeignKey(x => x.BrandId);
            modelBuilder.Entity<ProductImage>().HasOne(x => x.Product).WithMany(x => x.ProductImages).HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<Comment>().HasOne(x => x.Product).WithMany(x => x.Comments).HasForeignKey(x => x.ProductId);
            modelBuilder.Entity<Comment>().HasOne(x => x.AppUser).WithMany(x => x.Comments).HasForeignKey(x => x.AppUserId);

            modelBuilder.Entity<ProductDetail>().HasOne(x => x.Product).WithMany(x => x.ProductDetails).HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<Rating>().HasOne(x => x.Product).WithMany(x => x.Ratings).HasForeignKey(x => x.ProductId);
            modelBuilder.Entity<Rating>().HasOne(x => x.AppUser).WithMany(x => x.Ratings).HasForeignKey(x => x.AppUserId);

            modelBuilder.Entity<Order>().HasOne(x => x.AppUser).WithMany(x => x.Orders).HasForeignKey(x => x.AppUserId);

            modelBuilder.Entity<OrderItem>().HasOne(x => x.Order).WithMany(x => x.OrderItems).HasForeignKey(x => x.OrderId);
            modelBuilder.Entity<OrderItem>().HasOne(x => x.Product).WithMany(x => x.OrderItems).HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<ShippingAsdress>().HasOne(x => x.AppUser).WithMany(x => x.ShippingAsdresses).HasForeignKey(x => x.AppUserId);
            modelBuilder.Entity<ShippingAsdress>().HasOne(x => x.Order).WithOne(x => x.ShippingAsdress).HasForeignKey<ShippingAsdress>(x => x.OrderId);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShippingAsdress> ShippingAsdresses { get; set; }

    }
}
