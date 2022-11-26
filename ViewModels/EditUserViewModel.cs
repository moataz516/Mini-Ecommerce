using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProject.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PhoneNumber { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
