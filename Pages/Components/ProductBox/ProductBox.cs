using System;
using QLBH.Services;
using QLBH.Models;
using Microsoft.AspNetCore.Mvc;

namespace QLBH.Pages.Components.ProductBox;

public class ProductBox : ViewComponent
{
    private readonly ProductService _productService;

    public ProductBox(ProductService productService)
    {
        _productService = productService;
    }

    public async Task<IViewComponentResult> InvokeAsync(bool sapxeptang = true, int? categoryId = 0, List<Product>? product_list = null)
    {
        List<Product> products = await Task.Run(() =>
        {
            var result = product_list ?? _productService.GetProducts();

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                result = result.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            result = sapxeptang
                ? result.OrderBy(p => p.Price).ToList()
                : result.OrderByDescending(p => p.Price).ToList();

            return result;
        });

        return View<List<Product>>(products);
    }
}
