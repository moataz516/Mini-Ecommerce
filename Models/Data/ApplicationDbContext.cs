using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ECommerceProject.ViewModels;

namespace ECommerceProject.Models.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductImage> productImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        //public DbSet<ECommerceProject.ViewModels.ChangePasswordViewModel> ChangePasswordViewModel { get; set; }

        //public DbSet<ECommerceProject.ViewModels.UserViewModel> UserViewModel { get; set; }

        //public DbSet<ECommerceProject.ViewModels.EditUserViewModel> EditUserViewModel { get; set; }

    }
}
