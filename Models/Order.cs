using ECommerceProject.Models.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public int total { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int DeliveryStaus { get; set; } = 0;

        public IEnumerable<ApplicationUser> User { get; set; }
    }
}
