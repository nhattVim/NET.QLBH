using System;
using QLBH.Services;
using QLBH.Models;
using Microsoft.AspNetCore.Mvc;

namespace QLBH.Pages.Components.ProductBox;

public class ProductBox : ViewComponent
{
    List<Product> products = new List<Product>();

    public ProductBox(ProductService productService)
    {
        products = productService.GetProducts();
    }

    public async Task<IViewComponentResult> InvokeAsync(bool sapxeptang = true)
    {
        List<Product> _products = new List<Product>();
        if (sapxeptang)
        {
            _products = await Task.Run(() => products.OrderBy(p => p.Price).ToList());
        }
        else
        {
            _products = await Task.Run(() => products.OrderByDescending(p => p.Price).ToList());
        }
        return View<List<Product>>(_products);
    }
}
