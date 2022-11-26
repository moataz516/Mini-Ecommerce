using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.Models.Data
{
    public class Register
    {

        [Display(Name="First name")]
        public string FirstName { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Email address")]
        [Required]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage ="Password field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password field is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Password do not match")]
        public string ConfirmPassword { get; set; }

    }
}
