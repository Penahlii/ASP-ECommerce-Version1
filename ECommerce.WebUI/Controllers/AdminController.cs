using ECommerce.Business.Abstract;
using ECommerce.Entities.Models;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<ActionResult> Index(int page = 1, int category = 0, string filterName = "", string filterPrice = "")
        {
            var items = await _productService.GetAllByCategoryAsync(category);
            int pageSize = 10;

            bool alphabeticFilter = filterName == "a-z" ? true : false;
            bool priceFilter = filterPrice == "lower" ? true : false;

            var model = new CombinedAdminViewModel
            {
                AddCategoryViewModel = new(),
                ProductListViewModel = new()
            };

            model.ProductListViewModel.Products = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
			model.ProductListViewModel.PageSize = pageSize;
			model.ProductListViewModel.CurrentCategory = category;
            model.ProductListViewModel.CurrentPage = page;
			model.ProductListViewModel.PageCount = (int)Math.Ceiling(items.Count / (double)pageSize);

			if (!filterName.IsNullOrEmpty())
            {
				model.ProductListViewModel.FilterByPrice = "";
				model.ProductListViewModel.Products = (alphabeticFilter == true
                    ? model.ProductListViewModel.Products.OrderBy(p => p.ProductName).ToList()
                    : model.ProductListViewModel.Products.OrderByDescending(p => p.ProductName).ToList());
            }

            if (!filterPrice.IsNullOrEmpty())
            {
				model.ProductListViewModel.FilterByName = "";
				model.ProductListViewModel.Products = (priceFilter == true
                    ? model.ProductListViewModel.Products.OrderBy(p => p.UnitPrice).ToList()
                    : model.ProductListViewModel.Products.OrderByDescending(p => p.UnitPrice).ToList());
            }

            return View(model);
        }

        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            await _categoryService.DeleteAsync(categoryId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CombinedAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newCategory = new Category
                {
                    CategoryName = model.AddCategoryViewModel!.CategoryName!
                };
                await _categoryService.AddAsync(newCategory);
            }
            else
                ModelState.AddModelError("", "Invalid Category Name");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProduct(int productId)
        {
            await _productService.DeleteAsync(productId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddProduct()
        {
            var categories = await _categoryService.GetAllAsync();
            var model = new AddProductViewModel
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newProduct = new Product
                {
                    ProductName = model.ProductName,
                    CategoryId = model.CategoryId,
                    UnitPrice = model.UnitPrice,
                    UnitsInStock = model.UnitsInStock,
                    Discontinued = false
                };

                await _productService.AddAsync(newProduct);
                return RedirectToAction("Index");
            }

            var categories = await _categoryService.GetAllAsync();
            model.Categories = categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.CategoryName
            }).ToList();

            return View(model);
        }
    }
}
