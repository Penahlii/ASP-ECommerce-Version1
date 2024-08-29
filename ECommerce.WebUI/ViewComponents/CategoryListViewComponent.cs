using ECommerce.Business.Abstract;
using ECommerce.Entities.Models;
using ECommerce.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace ECommerce.WebUI.ViewComponents
{
    public class CategoryListViewComponent:ViewComponent
    {
        private ICategoryService _categoryService;

        public CategoryListViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public ViewViewComponentResult Invoke(string currentController, bool showDeleteButtons = false)
        {
            var allCategory = new Category()
            {
                CategoryId = 0,
                CategoryName = "All Categories"
            };
            var categories=_categoryService.GetAllAsync().Result;
            categories.Insert(0,allCategory);
            var param=HttpContext.Request.Query["category"];
            var category=int.TryParse(param, out var categoryId);
            var model = new CategoryListViewModel
            {
                Categories = categories,
                CurrentCategory = category ? categoryId : allCategory.CategoryId,
                ShowDeleteButtons = showDeleteButtons,
                CurrentController = currentController
            };
            return View(model);
        }
    }
}
