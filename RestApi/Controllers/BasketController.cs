using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Basket;
using ServiceLayer.ViewModels;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //CARD sehifesine getdikde cixan basketler
        [HttpPost("Baskets")]
        public async Task<IActionResult> Baskets(string username)
        {
            if (username is null) return BadRequest(username);

            AppUser? user = await _userManager.FindByNameAsync(username);
            if (user == null) return NotFound(username);

            List<Basket>? baskets = await _context.Baskets.Where(x => x.AppUserId == user.Id).ToListAsync();

            if (baskets.Any())
            {
                List<int> productIds = baskets.Select(basket => basket.ProductId).ToList();

                var products = await _context.Products
                    .Where(product => productIds.Contains(product.Id))
                    .Include(product => product.ProductImages)
                    .ToListAsync();

                var basketDtos = baskets
                    .Join(products,
                        basket => basket.ProductId,
                        product => product.Id,
                        (basket, product) => new HomeBasketDto
                        {
                            ProductPath = product.ProductImages?.FirstOrDefault()?.Path,
                            About = product.About,
                            Price = product.Price,
                            Quantity = basket.Quantity,
                            UnitPrice = basket.Quantity * product.Price
                        })
                    .ToList();

                return Ok(basketDtos);
            }

            return Ok("Səbət boşdur.");
        }

        //Basketden product sayini artirmaq
        [HttpPost("ManyBasketAdd")]
        public async Task<IActionResult> ManyBasketAdd(ManyBasketAddVM vm)
        {
            if (!ModelState.IsValid) return Unauthorized(vm);

            AppUser? user = await _userManager.FindByNameAsync(vm.Username);
            if (user == null) return NotFound("İstifadəçi tapılmadı.");

            Product? product = await _context.Products.SingleOrDefaultAsync(x => x.Id == vm.ProductId && x.IsStock);
            if (product == null) return NotFound("Məhsul tapılmadı.");

            Basket? basket = await _context.Baskets.Where(x => x.AppUserId == user.Id && x.ProductId == vm.ProductId)
                                                   .FirstOrDefaultAsync();

            if (basket == null)
            {
                var newBasket = new Basket
                {
                    AppUserId = user.Id,
                    ProductId = product.Id,
                    Quantity = 1
                };

                await _context.Baskets.AddAsync(newBasket);
                await _context.SaveChangesAsync();

                return Ok("Məhsul səbətə əlavə edildi.");
            }
            else
            {
                basket.Quantity = vm.Quantity;

                _context.Baskets.Update(basket);
                await _context.SaveChangesAsync();

                return Ok("Səbətdə məhsulun sayı dəyiŞildi.");
            }
        }

        //Bu action sadece productun uzerine vurduqda isliyecek (+1)
        [HttpPost("OneBasketAdd")]
        public async Task<IActionResult> OneBasketAdd(OneBasketAddVM vm)
        {
            if (!ModelState.IsValid) return BadRequest(vm);

            AppUser? user = await _userManager.FindByNameAsync(vm.Username);
            if (user == null) return NotFound("İstifadəçi tapılmadı.");

            Product? product = await _context.Products.SingleOrDefaultAsync(x => x.Id == vm.ProductId && x.IsStock);
            if (product == null) return NotFound("Məhsul tapılmadı.");

            Basket? basket = await _context.Baskets.Where(x => x.AppUserId == user.Id && x.ProductId == vm.ProductId)
                                                   .FirstOrDefaultAsync();

            if (basket == null)
            {
                var newBasket = new Basket
                {
                    AppUserId = user.Id,
                    ProductId = product.Id,
                    Quantity = 1
                };

                await _context.Baskets.AddAsync(newBasket);
                await _context.SaveChangesAsync();

                return Ok("Məhsul səbətə əlavə edildi.");
            }
            else
            {
                basket.Quantity += 1;

                _context.Baskets.Update(basket);
                await _context.SaveChangesAsync();

                return Ok("Sebetde mehsulun sayi artirildi.");
            }
        }
    }
}
