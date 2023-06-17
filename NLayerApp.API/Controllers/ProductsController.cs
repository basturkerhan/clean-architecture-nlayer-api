using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayerApp.API.Filters;
using NLayerApp.Core.CustomMessages;
using NLayerApp.Core.DTOs;
using NLayerApp.Core.DTOs.ResponseDTOs;
using NLayerApp.Core.Entities;
using NLayerApp.Core.Services;

namespace NLayerApp.API.Controllers
{
    public class ProductsController : CustomBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductsWithCategory()
        {
            return CreateActionResult(await _productService.GetProductsWithCategoryAsync());
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            return CreateActionResult(await _productService.GetAllAsync());
        }

        [ServiceFilter(typeof(NotFoundFilter<Product, ProductDto>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResult(await _productService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Save(CreateProductDto dto)
        {
            return CreateActionResult(await _productService.AddAsync(dto));
        }


        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto dto)
        {
            return CreateActionResult(await _productService.UpdateAsync(dto));
        }

        [ServiceFilter(typeof(NotFoundFilter<Product, ProductDto>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResult(await _productService.GetByIdAsync(id));
        }

    }
}
