using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayerApp.Core.DTOs;
using NLayerApp.Core.Entities;
using NLayerApp.Web.Filters;
using NLayerApp.Web.Services;

namespace NLayerApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductApiService _productApiService;
        private readonly CategoryApiService _categoryApiService;

        public ProductsController(ProductApiService productApiService, CategoryApiService categoryApiService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductWithCategoryDto>? products = await _productApiService.GetProductsWithCategoryAsync();

            return View(products);
        }

        public async Task<IActionResult> Save()
        {
            List<CategoryDto>? categories = await _categoryApiService.GetAllAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productApiService.SaveAsync(productDto);

                return RedirectToAction("Index");
            }

            List<CategoryDto>? categories = await _categoryApiService.GetAllAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name");

            return View();
        }

        [ServiceFilter(typeof(NotFoundFilter<Product, ProductDto>))]
        public async Task<IActionResult> Update(int id)
        {
            ProductDto? product = await _productApiService.GetByIdAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            List<CategoryDto>? categories = await _categoryApiService.GetAllAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                await _productApiService.UpdateAsync(productDto);

                return RedirectToAction("Index");
            }

            List<CategoryDto>? categories = await _categoryApiService.GetAllAsync();
            ViewBag.categories = new SelectList(categories, "Id", "Name", productDto.CategoryId);

            return View(productDto);
        }


    }
}
