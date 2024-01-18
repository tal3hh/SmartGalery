using AutoMapper;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Order;
using ServiceLayer.Dtos.Product;
using ServiceLayer.Dtos.Product.Dash;
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
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;
        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        //OrderItem Date Filter 
        public async Task<Paginate<HomeOrderItemDto>> DashProductOrderFilter(DashOrderDateVM vm)
        {
            if (vm.StartDate > vm.EndDate)
                (vm.StartDate, vm.EndDate) = (vm.EndDate, vm.StartDate);

            var query = _context.OrderItems
                                .Include(x => x.Order)
                                .Include(x => x.Product)
                                .Where(x => x.Order.OrderDate >= vm.StartDate && x.Order.OrderDate <= vm.EndDate
                                                                              && x.Order.IsActive == false)
                                .OrderByDescending(x => x.Id)
                                .AsQueryable();

            int totalCount = await query.CountAsync();
            int take = vm.Take > 0 ? vm.Take : 10;
            int totalPages = (int)Math.Ceiling(totalCount / (double)take);

            int currentPage = vm.Page > 0 ? vm.Page : 1;

            List<OrderItem> entities = await query
                                              .OrderByDescending(p => p.Id)
                                              .Skip((currentPage - 1) * take)
                                              .Take(take)
                                              .ToListAsync();

            List<HomeOrderItemDto> dtos = new();
            foreach (var item in entities)
            {
                var productImage = await _context.ProductImages
                                                 .Where(pi => pi.ProductId == item.Product.Id)
                                                 .FirstOrDefaultAsync();

                dtos.Add(new HomeOrderItemDto
                {
                    ProductPath = productImage?.Path,
                    About = item.Product.About,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    UnitePrice = item.UnitPrice,
                    OrderDate = item.Order.OrderDate
                });
            }

            return new Paginate<HomeOrderItemDto>(dtos, currentPage, totalPages);
        }
    }
}
