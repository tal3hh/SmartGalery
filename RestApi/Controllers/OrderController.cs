using DomainLayer.Entities;
using MailKit.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.ViewModels;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        readonly private AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public OrderController(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        //Bu action sadece productun uzerine vurduqda isliyecek.
        [HttpPost("Order")]
        public async Task<IActionResult> AddOrder(HomeOrderAddVM vm)
        {
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

            Product? product = await _context.Products.FindAsync(vm.ProductId);

            if (product == null) return NotFound("Product tapilmadi");

            OrderItem? orderItem = await _context.OrderItems.SingleOrDefaultAsync(x => x.ProductId == vm.ProductId);

            if (orderItem == null)
            {
                orderItem = new OrderItem
                {
                    ProductId = vm.ProductId,
                    OrderId = order.Id,
                    Quantity = vm.Quantity,
                    UnitPrice = vm.Quantity == 0 ? product.Price : vm.Quantity * product.Price
                };
                await _context.OrderItems.AddAsync(orderItem);
            }
            else
            {
                orderItem.Quantity += vm.Quantity;
                orderItem.UnitPrice = orderItem.Quantity == 0 ? product.Price : orderItem.Quantity * product.Price;
            }

            order.TotalAmount += product.Price * vm.Quantity;

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}