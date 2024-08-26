using System.Numerics;

namespace ECommerce.Entities.Concrete;

public class Cart
{
    public List<CartLine>? CartLines { get; set; }

    public Cart()
    {
        CartLines = new List<CartLine>();
    }

    public decimal? Total 
    { 
        get 
        {
            return CartLines.Sum(c => c.Product.UnitPrice * c.Quantity);
        } 
    }
}
