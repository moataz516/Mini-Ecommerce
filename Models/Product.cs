using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using ECommerceProject.Models.Data;

namespace ECommerceProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public int Qty { get; set; }
        public string description { get; set; }
        [NotMapped]
        public IFormFile FileImage { get; set; }
        public Category Category { get; set; }
        public Order Order { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
