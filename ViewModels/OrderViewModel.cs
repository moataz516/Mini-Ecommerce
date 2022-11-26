using ECommerceProject.Models;
using ECommerceProject.Models.Data;

namespace ECommerceProject.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public int OrderId { get; set; }
        public Product Product { get; set; }
        public Order Order { get; set; }
    }
}
