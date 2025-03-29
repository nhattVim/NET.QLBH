using QLBH.Services;
using QLBH.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QLBH.Pages
{
    public class ProductPageModel : PageModel
    {
        [BindProperty]
        public Product product { get; set; }

        [BindProperty]
        public IFormFileCollection UploadedImages { get; set; }

        public bool IsProductDetail { get; set; }
        public string Message { get; set; }

        public List<Product> products;
        public List<Product> products_search;
        private readonly ProductService _productService;

        public ProductPageModel(ProductService productService)
        {
            _productService = productService;
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

        public IActionResult OnGetLastProduct()
        {
            ViewData["Title"] = $"Sản phẩm cuối cùng";
            product = _productService.GetProducts().LastOrDefault();
            if (product != null)
            {
                return Page();
            }
            return NotFound();
        }

        public IActionResult OnGetRemoveAll()
        {
            products.Clear();
            return RedirectToPage("ProductPage");
        }

        public IActionResult OnGetLoadAll()
        {
            _productService.LoadProducts();
            return RedirectToPage("ProductPage");
        }

        public IActionResult OnPostAddProduct()
        {
            if (ModelState.IsValid)
            {
                if (UploadedImages != null && UploadedImages.Count > 0)
                {
                    foreach (var file in UploadedImages)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);
                        var savePath = Path.Combine("wwwroot/images", fileName);
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        product.Images.Add("/images/" + fileName);
                    }
                }
                _productService.AddProduct(product);
                Message = "Du lieu gui den hop le";
            }
            else
            {
                Message = "Du lieu gui den khong hop le";
            }
            return Page();
        }

        public IActionResult OnPostFindProductByName(string searchName)
        {
            products_search.Clear();
            var searchResults = products.Where(p => p.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (searchResults.Any())
            {
                products_search.AddRange(searchResults);
            }
            return Page();
        }

        public IActionResult OnPostUpdateProduct(int id, string name, string description, int price)
        {
            _productService.UpdateProduct(id, name, description, price);
            return RedirectToPage("ProductPage");
        }
    }
}
