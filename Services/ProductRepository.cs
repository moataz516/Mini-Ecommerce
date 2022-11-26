using ECommerceProject.Interfaces;
using ECommerceProject.Models;
using ECommerceProject.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerceProject.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _host;

        public ProductRepository(ApplicationDbContext context, IWebHostEnvironment host)
        {
            _context = context;
            _host = host;
        }

        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            var product = await _context.Products.Include(p => p.Category).ToListAsync();
            return product;
        }

        public async Task<Product> InsertImageUrl(string image, Product product)
        {

            var root = _host.WebRootPath;
            var fileName = Guid.NewGuid().ToString() + "_" + product.FileImage.FileName;
            var path = Path.Combine(root + "/image/product/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await product.FileImage.CopyToAsync(fileStream);
            }
            product.Image = fileName;

            return product;
        }

        public async Task<ProductImage> InsertMultipleImageUrl(List<IFormFile> images, Product product)
        {
            ProductImage res = null;
            foreach (var image in images)
            {
                var root = _host.WebRootPath;
                var fileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                var path = Path.Combine(root + "/image/product/multi/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }
                
                res = new ProductImage{
                    ProductId = product.Id,
                    Image = fileName,
                };
                _context.productImages.Add(res);
            }
            return res;

        }

    }
}
