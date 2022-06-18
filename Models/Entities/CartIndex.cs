using System.ComponentModel.DataAnnotations;

namespace Entities;

public class CartIndex
{
    [Key]
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    public int CartId { get; set; }
}