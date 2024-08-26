using ECommerce.Entities.Concrete;
using ECommerce.Entities.Models;

namespace ECommerce.Business.Abstract;

public interface ICartService
{
    void AddToCart(Cart cart, Product product);
    List<CartLine> List(Cart cart);
    void RemoveFromCart(Cart cart, int productId);
}
