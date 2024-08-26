using ECommerce.Business.Abstract;
using ECommerce.Entities.Concrete;
using ECommerce.WebUI.Models;
using ECommerce.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly IProductService _productService;
    private readonly ICartSessionService _cartSessionService;

    public CartController(ICartService cartService, IProductService productService, ICartSessionService cartSessionService)
    {
        _cartService = cartService;
        _productService = productService;
        _cartSessionService = cartSessionService;
    }

    public async Task<IActionResult> AddToCart(int productId, int page, int category)
    {
        var productToBeAdded = await _productService.GetByIdAsync(productId);
        var cart = _cartSessionService.GetCart();
        _cartService.AddToCart(cart, productToBeAdded);
        _cartSessionService.SetCart(cart);

        TempData.Add("message", String.Format("Your product, {0} was added successfully to cart", productToBeAdded.ProductName));
        return RedirectToAction("Index", "Product", new { page = page, category = category });
    }

    public IActionResult List()
    {
        var cart = _cartSessionService.GetCart();
        var model = new CartListViewModel
        {
            Cart = cart
        };
        return View(model);
    }

    public IActionResult Remove(int productId)
    {
        var cart = _cartSessionService.GetCart();
        _cartService.RemoveFromCart(cart, productId);
        _cartSessionService.SetCart(cart);
        TempData.Add("message", "Your Product Removed From Cart");
        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Complete()
    {
        var model = new ShippingDetailViewModel
        {
            ShippingDetails = new ShippingDetails()
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Complete(ShippingDetailViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        TempData.Add("message", String.Format("Thank you {0}, your order is in Progress", model.ShippingDetails.FirstName));
        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Decrease(int productId)
    {
        var cart = _cartSessionService.GetCart();
        var cartLine = cart.CartLines.FirstOrDefault(c => c.Product.ProductId == productId);
        if (cartLine.Quantity > 1)
        {
            cartLine.Quantity--;
            _cartSessionService.SetCart(cart);
            return RedirectToAction("List");
        }
        else
        {
            return RedirectToAction("List");
        }
    }

    [HttpGet]
    public IActionResult Increase(int productId)
    {
        var cart = _cartSessionService.GetCart();
        var cartLine = cart.CartLines.FirstOrDefault(c => c.Product.ProductId == productId);
        if (cartLine.Quantity < cartLine.Product.UnitsInStock)
        {
            cartLine.Quantity++;
            _cartSessionService.SetCart(cart);
            return RedirectToAction("List");
        }
        else
        {
            return RedirectToAction("List");
        }
    }
}
