using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLBH.Models;

namespace QLBH.Controllers
{
    public class ProductController : Controller
    {
        private readonly QlbhContext _context;

        public ProductController(QlbhContext context)
        {
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index(string searchString, int? categoryId)
        {
            var products = _context.Products.AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            var productList = await products.ToListAsync();
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name", categoryId);
            return View(productList);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, List<IFormFile> Images)
        {
            if (ModelState.IsValid)
            {
                var imagePaths = new List<string>();
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                foreach (var file in Images)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(uploadPath, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        imagePaths.Add($"/images/products/{fileName}");
                    }
                }

                product.Images = Newtonsoft.Json.JsonConvert.SerializeObject(imagePaths);

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, List<IFormFile> Images)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                var imageList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(product.Images);
                if (imageList != null && imageList.Count > 0)
                {
                    foreach (var imagePath in imageList)
                    {
                        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                }
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/DeleteAll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAll()
        {
            var allProducts = _context.Products;
            _context.Products.RemoveRange(allProducts);
            await _context.SaveChangesAsync();

            // Reset ID của bảng Product về 0
            using (var connection = _context.Database.GetDbConnection())
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DBCC CHECKIDENT ('Product', RESEED, 0)";
                    await command.ExecuteNonQueryAsync();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Product/LoadAll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoadAll()
        {
            var defaultProducts = new List<Product>
            {
                // Điện thoại
                new Product { Name = "iPhone 15", Description = "Điện thoại cao cấp của Apple", Price = 25000000, Images = "[\"/images/default/iphone15.jpg\"]", CategoryId = 1 },
                new Product { Name = "Samsung Galaxy S23", Description = "Flagship của Samsung", Price = 23000000, Images = "[\"/images/default/galaxys23.jpg\"]", CategoryId = 1 },
                new Product { Name = "Xiaomi 13 Pro", Description = "Điện thoại cao cấp của Xiaomi", Price = 20000000, Images = "[\"/images/default/xiaomi13pro.jpg\"]", CategoryId = 1 },
                new Product { Name = "Google Pixel 7", Description = "Smartphone của Google với camera AI mạnh mẽ", Price = 21000000, Images = "[\"/images/default/pixel7.jpg\"]", CategoryId = 1 },

                // Laptop
                new Product { Name = "MacBook Pro 16", Description = "Laptop mạnh mẽ của Apple", Price = 60000000, Images = "[\"/images/default/macbookpro16.jpg\"]", CategoryId = 2 },
                new Product { Name = "Dell XPS 15", Description = "Laptop cao cấp của Dell", Price = 40000000, Images = "[\"/images/default/dellxps.jpg\"]", CategoryId = 2 },
                new Product { Name = "Asus ROG Zephyrus G14", Description = "Laptop gaming mỏng nhẹ của Asus", Price = 45000000, Images = "[\"/images/default/rogzephyrus.jpg\"]", CategoryId = 2 },

                // Phụ kiện
                new Product { Name = "AirPods Pro 2", Description = "Tai nghe không dây cao cấp của Apple", Price = 6500000, Images = "[\"/images/default/airpodspro2.jpg\"]", CategoryId = 3 },
                new Product { Name = "Samsung Galaxy Buds 2 Pro", Description = "Tai nghe không dây của Samsung", Price = 5500000, Images = "[\"/images/default/galaxybuds2pro.jpg\"]", CategoryId = 3 },
                new Product { Name = "Sạc nhanh 65W Anker", Description = "Bộ sạc nhanh công suất cao", Price = 1200000, Images = "[\"/images/default/anker65w.jpg\"]", CategoryId = 3 },

                // Đồng hồ thông minh
                new Product { Name = "Apple Watch Series 9", Description = "Đồng hồ thông minh của Apple", Price = 12000000, Images = "[\"/images/default/applewatch9.jpg\"]", CategoryId = 4 },
                new Product { Name = "Samsung Galaxy Watch 6", Description = "Đồng hồ thông minh của Samsung", Price = 9000000, Images = "[\"/images/default/galaxywatch6.jpg\"]", CategoryId = 4 },
                new Product { Name = "Garmin Fenix 7X", Description = "Đồng hồ thể thao chuyên dụng", Price = 25000000, Images = "[\"/images/default/garminfenix7x.jpg\"]", CategoryId = 4 },

                // Máy tính bảng
                new Product { Name = "iPad Pro 12.9 M2", Description = "Máy tính bảng mạnh mẽ của Apple", Price = 32000000, Images = "[\"/images/default/ipadpro12.jpg\"]", CategoryId = 5 },
                new Product { Name = "Samsung Galaxy Tab S9", Description = "Tablet cao cấp của Samsung", Price = 28000000, Images = "[\"/images/default/galaxytabs9.jpg\"]", CategoryId = 5 }
            };

            _context.Products.AddRange(defaultProducts);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
