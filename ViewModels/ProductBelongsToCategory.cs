namespace ECommerceProject.ViewModels
{
    public class ProductBelongsToCategory
    {
        public int ProductId { get; set; }
        public int ProductCatId { get; set; }
        public int CategoryId { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public int ProductPrice { get; set; }
        public string? ProductImage { get; set; }
        public string? ProductDescription { get; set; }
    }
}
