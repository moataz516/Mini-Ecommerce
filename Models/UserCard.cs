using ECommerceProject.Models.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceProject.Models
{
    public class UserCard
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public int CardNumber { get; set; }
        public int CCB { get; set; }
        public int Wallet { get; set; }

        public ApplicationUser? User { get; set; }

    }
}
