using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ECommerceProject.Models;
using ECommerceProject.Models.Data;
using ECommerceProject.Interfaces;
using ECommerceProject.ViewModels;

namespace ECommerceProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _host;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepository _repository;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment host , IHttpContextAccessor httpContextAccessor , IProductRepository repository)
        {
            _context = context;
            _host = host;
            _httpContextAccessor = httpContextAccessor;
            _repository = repository;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _repository.GetAllProduct();
            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            var images = await _context.productImages.Where(m => m.ProductId == product.Id).ToListAsync();

            if (product == null)
            {
                return NotFound();
            }

            var model = Tuple.Create<Product,IEnumerable<ProductImage>>(product, images);


            return View(model);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            var curUserId = _httpContextAccessor.HttpContext?.User.GetUserId();
            var create = new Product { ApplicationUserId = curUserId };
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View(create);
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryId,Name,Image,Price,Qty,description,FileImage,ApplicationUserId")] Product product,
           List<IFormFile> images)
        {
            if (ModelState.IsValid)
            {
                if (product.FileImage != null)
                {
                     await _repository.InsertImageUrl(product.Image, product);
                }
                _context.Add(product);
                await _context.SaveChangesAsync();

                if (images != null)
                {
                    await _repository.InsertMultipleImageUrl(images , product);
                await _context.SaveChangesAsync();

                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string oldImage, [Bind("Id,CategoryId,Name,Image,Price,Qty,description,FileImage")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (product.FileImage != null)
                    {
                        var image = await _repository.InsertImageUrl(product.Image, product);
                    }
                    else
                    {
                        product.Image = oldImage;
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.Id == id);
        }
    }
}
