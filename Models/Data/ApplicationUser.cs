using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProject.Models.Data
{
    public class ApplicationUser : IdentityUser
    {



        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ProfileImageUrl { get; set; }

        [DefaultValue(100)]
        public int UserWallet { get; set; }
        public ICollection<Product> Product { get; set; }
        public UserCard UserCard { get; set; }
        public Order Orders { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
