using ECommerce.Entities.Models;

namespace ECommerce.Entities.Concrete;

public class CartLine
{
    public  Product? Product  { get; set; }
    public int Quantity { get; set; }
}
