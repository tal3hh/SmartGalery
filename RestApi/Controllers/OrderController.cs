using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Order;
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

            List<Basket>? baskets = await _context.Baskets.Include(x => x.Product)
                                                  .Where(x => x.AppUserId == user.Id).ToListAsync();

            if (baskets.Count() == 0) return BadRequest("Sebet bosdur.");

            Order newOrder = new Order
            {
                AppUserId = user.Id,
                TotalAmount = baskets.Sum(basket => basket.Quantity * (basket.Product?.Price ?? 0)),
                OrderItems = baskets.Select(basket => new OrderItem
                {
                    ByUsername = username,
                    ProductName = basket.Product?.Name,
                    Quantity = basket.Quantity,
                    UnitPrice = basket.Product.Price * basket.Quantity
                }).ToList()
            };

            _context.Orders.Add(newOrder);

            foreach (var item in baskets)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                    product.Count -= item.Quantity;
            }

            _context.Baskets.RemoveRange(baskets);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
