using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.ViewModels
{
    public class ChangePasswordViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter a password")]

        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="New Password")]
        [Required(ErrorMessage = "Enter a new password")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Required(ErrorMessage = "Enter a confirm password")]

        public string ConfirmPassword { get; set; }
    }
}
