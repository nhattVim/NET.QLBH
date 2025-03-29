using QLBH.Models;
using QLBH.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace QLBH.Pages;

public class ProductPageModel : PageModel
{
    [BindProperty]
    public Product product { get; set; }

    public List<Product> products { get; set; }
    public List<Product> products_search { get; set; }
    public string Message { get; set; }
    public bool IsProductDetail { get; set; }

    private readonly ProductService _productService;

    public ProductPageModel(ProductService productService)
    {
        _productService = productService;
        product = new Product();
        Message = string.Empty;
        products_search = new List<Product>();
        products = _productService.GetProducts();
    }

    public void OnGet(int? id)
    {
        if (id != null)
        {
            ViewData["Title"] = $"Thông tin sản phẩm (ID={id.Value})";
            product = _productService.GetProductById(id.Value);
            IsProductDetail = true;
        }
        else
        {
            ViewData["Title"] = $"Danh sách sản phẩm";
        }
    }

    public IActionResult OnGetRemoveAll()
    {
        _productService.RemoveAll();
        return RedirectToPage("ProductPage");
    }

    public IActionResult OnGetLoadAll()
    {
        _productService.LoadDefaultProducts();
        return RedirectToPage("ProductPage");
    }

    public IActionResult OnPostAddProduct(List<IFormFile> UploadedImages)
    {
        if (ModelState.IsValid)
        {
            var imagePaths = new List<string>();
            var uploadPath = Path.Combine("wwwroot", "images", "products");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var file in UploadedImages)
            {
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                imagePaths.Add($"/images/products/{fileName}");
            }

            product.Images = imagePaths;

            _productService.AddProduct(product);

            return RedirectToPage("ProductPage");
        }
        return Page();
    }

    public IActionResult OnPostUpdateProduct(int id, string name, string description, decimal price, List<IFormFile> images)
    {
        var existingProduct = _productService.GetProductById(id);
        if (existingProduct == null)
        {
            ModelState.AddModelError("", "Sản phẩm không tồn tại.");
            return Page();
        }

        existingProduct.Name = name;
        existingProduct.Description = description;
        existingProduct.Price = price;

        if (images != null && images.Count > 0)
        {
            var imagePaths = new List<string>();
            var uploadPath = Path.Combine("wwwroot", "images", "products");

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            foreach (var file in images)
            {
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                imagePaths.Add($"/images/products/{fileName}");
            }

            existingProduct.Images = imagePaths;
        }

        _productService.UpdateProduct(existingProduct);
        return RedirectToPage("ProductPage");
    }

    public IActionResult OnPostDeleteProduct(int id)
    {
        _productService.DeleteProduct(id);
        return RedirectToPage("ProductPage", new { id = (int?)null });
    }

    public void OnPostSearch(string searchName)
    {
        products_search = _productService.SearchProducts(searchName);
    }
}
