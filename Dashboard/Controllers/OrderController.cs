using DomainLayer.Entities;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.X509;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Order;
using ServiceLayer.Dtos.Product.Dash;
using ServiceLayer.Utilities;
using ServiceLayer.ViewModels;

namespace Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly private AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public OrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("AllOrders")]
        public async Task<IActionResult> AllOrders(DashPagineVM vm)
        {
            var query = _context.Orders
                .Include(p => p.AppUser)
                .AsQueryable();

            int totalCount = await query.CountAsync();
            int take = vm.Take > 0 ? vm.Take : 20;
            int totalPages = (int)Math.Ceiling(totalCount / (double)take);

            int currentPage = vm.Page > 0 ? vm.Page : 1;

            List<DashOrderDto> OrderDtos = await query
                .OrderByDescending(p => p.CreateDate)
                .Select(order => new DashOrderDto
                {
                    Username = order.AppUser.UserName,
                    TotalAmount = order.TotalAmount,
                    CreateDate = order.CreateDate
                })
                .Skip((currentPage - 1) * take)
                .Take(take)
                .ToListAsync();

            return Ok(new Paginate<DashOrderDto>(OrderDtos, currentPage, totalPages));
        }

        [HttpPost("OrderFilter")]
        public async Task<IActionResult> OrderFilter(DashOrderDateVM vm)
        {
            if (vm.StartDate > vm.EndDate)
                (vm.StartDate, vm.EndDate) = (vm.EndDate, vm.StartDate);

            var query = _context.Orders
                .Include(p => p.AppUser)
                .Where(x => x.CreateDate >= vm.StartDate && x.CreateDate <= vm.EndDate)
                .AsQueryable();

            int totalCount = await query.CountAsync();
            int take = vm.Take > 0 ? vm.Take : 20;
            int totalPages = (int)Math.Ceiling(totalCount / (double)take);

            int currentPage = vm.Page > 0 ? vm.Page : 1;

            List<DashOrderDto> OrderDtos = await query
                .OrderByDescending(p => p.CreateDate)
                .Select(order => new DashOrderDto
                {
                    Username = order.AppUser.UserName,
                    TotalAmount = order.TotalAmount,
                    CreateDate = order.CreateDate
                })
                .Skip((currentPage - 1) * take)
                .Take(take)
                .ToListAsync();

            return Ok(new Paginate<DashOrderDto>(OrderDtos, currentPage, totalPages));
        }

        [HttpPost("AllOrderItems")]
        public async Task<IActionResult> AllOrderItems(DashPagineVM vm)
        {
            var query = _context.OrderItems
                .Include(x=> x.Order)
                .AsQueryable();

            int totalCount = await query.CountAsync();
            int take = vm.Take > 0 ? vm.Take : 20;
            int totalPages = (int)Math.Ceiling(totalCount / (double)take);

            int currentPage = vm.Page > 0 ? vm.Page : 1;

            List<DashOrderItemDto> orderItemDtos = await query
                .OrderByDescending(p => p.CreateDate)
                .Select(x => new DashOrderItemDto
                {
                    ByUsername = x.ByUsername,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    UnitePrice = x.UnitPrice
                })
                .Skip((currentPage - 1) * take)
                .Take(take)
                .ToListAsync();

            return Ok(new Paginate<DashOrderItemDto>(orderItemDtos, currentPage, totalPages));
        }

        [HttpPost("OrderItemFilter")]
        public async Task<IActionResult> OrderItemFilter(DashOrderDateVM vm)
        {
            if (vm.StartDate > vm.EndDate)
                (vm.StartDate, vm.EndDate) = (vm.EndDate, vm.StartDate);

            var query = _context.OrderItems
                .Where(x => x.CreateDate >= vm.StartDate && x.CreateDate <= vm.EndDate)
                .AsQueryable();

            int totalCount = await query.CountAsync();
            int take = vm.Take > 0 ? vm.Take : 20;
            int totalPages = (int)Math.Ceiling(totalCount / (double)take);

            int currentPage = vm.Page > 0 ? vm.Page : 1;

            List<DashOrderItemDto> orderItemDtos = await query
                .OrderByDescending(p => p.CreateDate)
                .Select(x => new DashOrderItemDto
                {
                    ByUsername = x.ByUsername,
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    UnitePrice = x.UnitPrice
                })
                .Skip((currentPage - 1) * take)
                .Take(take)
                .ToListAsync();

            return Ok(new Paginate<DashOrderItemDto>(orderItemDtos, currentPage, totalPages));
        }
    }
}