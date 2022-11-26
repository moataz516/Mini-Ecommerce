using ECommerceProject.Helpers;
using ECommerceProject.Models;
using ECommerceProject.Models.Data;
using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ECommerceProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger , ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        //[Authorize(Roles="admin")]
        public IActionResult Index()
        {

            var products = _context.Products.ToList();
            var categories = _context.Categories.ToList();
            var collection = Tuple.Create<IEnumerable<Product>, IEnumerable<Category>>(products,categories);
            var multi = (from c in categories join p in products on c.Id equals p.CategoryId select 
            new ProductBelongsToCategory { ProductId = p.Id, ProductName = p.Name , ProductDescription=p.description,ProductImage=p.Image,
            CategoryId=c.Id,CategoryName=c.Name,ProductPrice=p.Price ,
                ProductCatId = p.CategoryId
            }).ToList();

            return View(collection);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}