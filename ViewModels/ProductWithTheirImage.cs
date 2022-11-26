using ECommerceProject.Models;

namespace ECommerceProject.ViewModels
{
    public class ProductWithTheirImage
    {
        public Product product { set; get; }
        public IEnumerable<ProductImage> productImages { set; get; }
    }
}
