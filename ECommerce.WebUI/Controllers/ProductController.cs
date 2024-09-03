using ECommerce.Business.Abstract;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.WebUI.Controllers
{
	public class ProductController : Controller
	{
		private readonly IProductService _productService;

		public ProductController(IProductService productService)
		{ 
			_productService = productService;
		}

		// GET: ProductController
		public async Task<ActionResult> Index(int page = 1, int category = 0, string filterName = "", string filterPrice = "")
		{
			var items = await _productService.GetAllByCategoryAsync(category);
			int pageSize = 10;

			bool alphabeticFilter = filterName == "a-z" ? true : false;
			bool priceFilter = filterPrice == "lower" ? true : false;

			var model = new ProductListViewModel
			{
				Products = items.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
				PageSize = pageSize,
				CurrentCategory = category,
				CurrentPage = page,
				PageCount = (int)Math.Ceiling(items.Count / (double)pageSize),
			};

			if (!filterName.IsNullOrEmpty()) 
			{ 
				model.FilterByPrice = ""; 
				model.Products = (alphabeticFilter == true 
					? model.Products.OrderBy(p => p.ProductName).ToList() 
					: model.Products.OrderByDescending(p => p.ProductName).ToList()); 
			}

			if (!filterPrice.IsNullOrEmpty()) 
			{ 
				model.FilterByName = ""; 
				model.Products = (priceFilter == true 
					? model.Products.OrderBy(p => p.UnitPrice).ToList() 
					: model.Products.OrderByDescending(p => p.UnitPrice).ToList()); }

			return View(model);
		}


		[HttpGet]
		public async Task<IActionResult> SearchProducts(string query, int category = 0)
		{
			var items = await _productService.SearchProductsAsync(query, category);
			return PartialView("_ProductListPartial", items);
		}


		// GET: ProductController/Details/5
		public ActionResult Details(int id)
		{
			return View();
		}

		// GET: ProductController/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: ProductController/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: ProductController/Edit/5
		public ActionResult Edit(int id)
		{
			return View();
		}

		// POST: ProductController/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		// GET: ProductController/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: ProductController/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}
	}
}
