using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProject.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Image { get; set; }

        public IEnumerable<Product> Product { get; set; }

        [NotMapped]
        [Display(Name ="Multiple Image")]
        public IEnumerable<IFormFile> FileImages { get; set; }
    }
}
