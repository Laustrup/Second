using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Entities
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        private List<Product> _products {get; set;} public List<Product> Products { get{return _products;} }
        public List<Product> AddProduct(Product product)
        {
            _products.Add(product);
            return _products;            
        }
        public void RemoveProducts() { _products = new List<Product>(); }

        public string UserId { get; set; }
        public IdentityUser? User { get; set; }
    }
}