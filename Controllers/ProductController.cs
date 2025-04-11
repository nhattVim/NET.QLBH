using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLBH.Models;

namespace QLBH.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly QlbhContext _context;

        public ProductController(QlbhContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Product/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Product
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Product/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Product/DeleteAll
        [HttpDelete("DeleteAll")]
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

            return NoContent();
        }

        // POST: api/Product/LoadAll
        [HttpPost("LoadAll")]
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

            return Ok(defaultProducts);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
