using ECommerceProject.Models;
using ECommerceProject.Models.Data;

namespace ECommerceProject.Interfaces
{
    public interface IProductRepository
    {

        Task<IEnumerable<Product>> GetAllProduct();
        Task<Product> InsertImageUrl(string image, Product product);
        Task<ProductImage> InsertMultipleImageUrl(List<IFormFile> images, Product product);
    }
}
