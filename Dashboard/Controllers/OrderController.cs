using DomainLayer.Entities;
using MailKit.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Order;
using ServiceLayer.Services.Interfaces;
using ServiceLayer.ViewModels;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly private AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOrderService _orderService;
        public OrderController(AppDbContext context, UserManager<AppUser> userManager, IOrderService orderService)
        {
            _context = context;
            _userManager = userManager;
            _orderService = orderService;
        }

        [HttpPost("DashOrderDateFilter")]
        public async Task<IActionResult> DashOrderDateFilter(DashOrderDateVM vm)
        {
            var list = await _orderService.DashProductOrderFilter(vm);

            return Ok(list);
        }

        //CARD sehifesine getdikde cixan orderler
        [HttpPost("OrderProducts")]
        public async Task<IActionResult> OrderProducts(string username)
        {
            if (username is null) return NotFound(username);

            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(username);

            Order? order = await _context.Orders.SingleOrDefaultAsync(x => x.AppUserId == user.Id && x.IsActive);
            if (order == null) return NotFound("Sebet bosdur");

            List<OrderItem> orderItems = await _context.OrderItems.Include(x => x.Product)
                .Where(x => x.OrderId == order.Id).ToListAsync();

            HomeOrderDto homeOrderDto = new HomeOrderDto
            {
                Subtotal = order.TotalAmount,
                HomeOrderItemDtos = new List<HomeOrderItemDto>()
            };

            foreach (var item in orderItems)
            {
                var productImage = await _context.ProductImages
                    .Where(pi => pi.ProductId == item.Product.Id)
                    .FirstOrDefaultAsync();

                homeOrderDto.HomeOrderItemDtos.Add(new HomeOrderItemDto
                {
                    ProductPath = productImage?.Path,
                    About = item.Product.About,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                    UnitePrice = item.UnitPrice
                });
            }

            return Ok(homeOrderDto);
        }

        //Basketden product sayini artirmaq
        [HttpPost("ManyProductAdd")]
        public async Task<IActionResult> ManyProductAdd(ManyProductAddVM vm)
        {
            if (!ModelState.IsValid) return Unauthorized(vm);

            var user = await _userManager.FindByNameAsync(vm.Username);
            if (user == null) return NotFound(vm.Username);

            Order? order = await _context.Orders.SingleOrDefaultAsync(x => x.AppUserId == user.Id && x.IsActive);
            if (order == null)
            {
                var newOrder = new Order
                {
                    AppUserId = user.Id,
                    IsActive = true
                };

                await _context.Orders.AddAsync(newOrder);
                await _context.SaveChangesAsync();

                order = await _context.Orders.SingleOrDefaultAsync(x => x.AppUserId == user.Id && x.IsActive);
            }

            Product? product = await _context.Products.Include(x => x.ProductImages)
                .SingleOrDefaultAsync(x => x.Id == vm.ProductId);

            if (product == null) return NotFound("Product tapilmadi");
            if (product.Count < vm.Quantity) return BadRequest($"Mehsuldan {product.Count} eded qalib.");

            OrderItem? orderItem = await _context.OrderItems.SingleOrDefaultAsync(x => x.ProductId == vm.ProductId);
            if (orderItem == null)
            {
                orderItem = new OrderItem
                {
                    ProductId = vm.ProductId,
                    OrderId = order.Id,
                    Quantity = 1,
                    UnitPrice = product.Price
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            else
            {
                orderItem.Quantity = vm.Quantity;
                orderItem.UnitPrice = vm.Quantity * product.Price;
            }
            await _context.SaveChangesAsync();

            order.TotalAmount = await _context.OrderItems
                                        .Where(x => x.OrderId == order.Id)
                                        .SumAsync(x => x.UnitPrice);

            await _context.SaveChangesAsync();
            return Ok();
        }

        //Bu action sadece productun uzerine vurduqda isliyecek
        [HttpPost("OneProductAdd")]
        public async Task<IActionResult> OneProductAdd(OneProductAddVM vm)
        {
            if (!ModelState.IsValid) return Unauthorized(vm);

            var user = await _userManager.FindByNameAsync(vm.Username);
            if (user == null) return NotFound(vm.Username);

            Order? order = await _context.Orders.SingleOrDefaultAsync(x => x.AppUserId == user.Id && x.IsActive);
            if (order == null)
            {
                var newOrder = new Order
                {
                    AppUserId = user.Id,
                    IsActive = true
                };

                await _context.Orders.AddAsync(newOrder);
                await _context.SaveChangesAsync();

                order = await _context.Orders.SingleOrDefaultAsync(x => x.AppUserId == user.Id && x.IsActive);
            }

            Product? product = await _context.Products.SingleOrDefaultAsync(x => x.Id == vm.ProductId && x.IsStock);
            if (product == null) return NotFound("Product tapilmadi");

            OrderItem? orderItem = await _context.OrderItems.SingleOrDefaultAsync(x => x.ProductId == vm.ProductId);
            if (orderItem == null)
            {
                orderItem = new OrderItem
                {
                    ProductId = vm.ProductId,
                    OrderId = order.Id,
                    Quantity = 1,
                    UnitPrice = product.Price
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            else
            {
                orderItem.Quantity += 1;
                orderItem.UnitPrice += product.Price;
            }

            order.TotalAmount += product.Price;

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}