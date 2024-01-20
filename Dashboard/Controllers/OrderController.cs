using DomainLayer.Entities;
using MailKit.Search;
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

        [HttpPost("OrderCreate")]
        public async Task<IActionResult> OrderCreate(string username)
        {
            if (username is null) return BadRequest(username);

            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(username);

            List<Basket>? baskets = await _context.Baskets.Include(x=> x.Product)
                                                  .Where(x => x.AppUserId == user.Id).ToListAsync();

            if (baskets.Count() == 0) return BadRequest("Sebet bosdur.");

            Order newOrder = new Order
            {
                AppUserId = user.Id,
                TotalAmount = baskets.Sum(basket => basket.Quantity * (basket.Product?.Price ?? 0)),
                OrderItems = baskets.Select(basket => new OrderItem
                {
                    ProductName = basket.Product?.Name,
                    Quantity = basket.Quantity,
                    UnitPrice = basket.Product.Price * basket.Quantity
                }).ToList()
            };

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();


            _context.Baskets.RemoveRange(baskets);
            await _context.SaveChangesAsync();

            return Ok();
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

        [HttpPost("OrdersFilter")]
        public async Task<IActionResult> OrdersFilter(DashOrderDateVM vm)
        {
            if (!ModelState.IsValid) return BadRequest(vm);

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
                .AsQueryable();

            int totalCount = await query.CountAsync();
            int take = vm.Take > 0 ? vm.Take : 20;
            int totalPages = (int)Math.Ceiling(totalCount / (double)take);

            int currentPage = vm.Page > 0 ? vm.Page : 1;

            List<DashOrderItemDto> orderItemDtos = await query
                .OrderByDescending(p => p.CreateDate)
                .Select(x => new DashOrderItemDto
                {
                    ProductName = x.ProductName,
                    Quantity = x.Quantity,
                    UnitePrice = x.UnitPrice
                })
                .Skip((currentPage - 1) * take)
                .Take(take)
                .ToListAsync();

            return Ok(new Paginate<DashOrderItemDto>(orderItemDtos, currentPage, totalPages));
        }

        [HttpPost("OrderItemsFilter")]
        public async Task<IActionResult> OrderItemsFilter(DashOrderDateVM vm)
        {
            if (!ModelState.IsValid) return BadRequest(vm);

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