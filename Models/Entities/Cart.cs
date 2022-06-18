using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public List<Product>? Products { get; set; }

        public List<Product> AddProduct(Product product)
        {
            if (Products == null) {Products = new List<Product>();}
            if (!Products.Contains(product)) { Products.Add(product); }
            return Products;
        }
        
        public void RemoveProducts() { Products = new List<Product>(); }
        public void RemoveProduct(Product product) { if (Products!=null) {Products.Remove(product);} }

        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}