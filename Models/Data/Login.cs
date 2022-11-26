using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.Models.Data
{
    public class Login
    {
        [Required(ErrorMessage = "Email address is required") , Display(Name="Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required"), DataType(DataType.Password)]
        public string Password { get; set; }
        

    }
}
