using DomainLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Contexts;
using ServiceLayer.Dtos.Wish;
using ServiceLayer.ViewModels;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public WishController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost("AddMyWish")]
        public async Task<IActionResult> AddMyWish(MyWishVM vm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(vm.Username);
            if (user == null) return NotFound(vm.Username);

            Product? product = await _context.Products.FindAsync(vm.ProductId);
            if (product == null) return NotFound(vm.ProductId);

            Wish? wish = await _context.Wishes.Where(x => x.ProductId == vm.ProductId).SingleOrDefaultAsync();
            if (wish is null)
            {
                Wish newWish = new Wish
                {
                    AppUserId = user.Id,
                    ProductId = vm.ProductId
                };

                await _context.Wishes.AddAsync(newWish);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Wishes.Remove(wish);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }

        [HttpPost("MyWishList")]
        public async Task<IActionResult> MyWishList(string? username)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest(nameof(username));

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound(nameof(username));

            var wishesExist = await _context.Wishes.AnyAsync(x => x.AppUserId == user.Id);
            if (!wishesExist)
                return Ok();

            var wishDtos = new List<HomeWishDto>();

            wishDtos = await _context.Wishes
                                  .Where(x => x.AppUserId == user.Id)
                                  .Join(
                                      _context.Products.Include(p => p.ProductImages),
                                      wish => wish.ProductId,
                                      product => product.Id,
                                      (wish, product) => new HomeWishDto
                                      {
                                          ProductId = product.Id,
                                          Name = product.Name,
                                          ProductPath = product.ProductImages.SingleOrDefault().Path,
                                          OldPrice = product.OldPrice,
                                          Price = product.Price,
                                          IsStock = product.IsStock,
                                          About = product.About
                                      })
                                  .ToListAsync();

            return Ok(wishDtos);
        }

        [HttpDelete("RemoveAllWish")]
        public async Task<IActionResult> RemoveAllWish(string? username)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest(nameof(username));

            AppUser? user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound("İstifadəçi tapılmadı.");

            List<Wish> list = await _context.Wishes.Where(x=> x.AppUserId == user.Id).ToListAsync();

            foreach (var item in list)
            {
                _context.Wishes.Remove(item);
            }
            await _context.SaveChangesAsync();

            return Ok("Datalar silindi");
        }
    }
}
