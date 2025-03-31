using QLBH.Models;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace QLBH.Services;

public class ProductService
{
    private readonly QLBHContext _context;

    public ProductService(QLBHContext context)
    {
        _context = context;
    }

    public List<Product> GetProducts()
    {
        return _context.Products.ToList();
    }

    public Product GetProductById(int id)
    {
        return _context.Products.FirstOrDefault(p => p.Id == id) ?? new Product();
    }

    public void RemoveAll()
    {
        _context.Products.RemoveRange(_context.Products);
        _context.SaveChanges();

        // Reset ID của bảng Product về 0
        using (var connection = _context.Database.GetDbConnection())
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "DBCC CHECKIDENT ('Product', RESEED, 0)";
                command.ExecuteNonQuery();
            }
        }
    }

    public void LoadDefaultProducts()
    {
        var defaultProducts = new List<Product>
        {
            // Điện thoại
            new Product { Name = "iPhone 15", Description = "Điện thoại cao cấp của Apple", Price = 25000000, Images = new List<string> { "/images/default/iphone15.jpg" }, CategoryId = 1 },
            new Product { Name = "Samsung Galaxy S23", Description = "Flagship của Samsung", Price = 23000000, Images = new List<string> { "/images/default/galaxys23.jpg" }, CategoryId = 1 },
            new Product { Name = "Xiaomi 13 Pro", Description = "Điện thoại cao cấp của Xiaomi", Price = 20000000, Images = new List<string> { "/images/default/xiaomi13pro.jpg" }, CategoryId = 1 },
            new Product { Name = "Google Pixel 7", Description = "Smartphone của Google với camera AI mạnh mẽ", Price = 21000000, Images = new List<string> { "/images/default/pixel7.jpg" }, CategoryId = 1 },

            // Laptop
            new Product { Name = "MacBook Pro 16", Description = "Laptop mạnh mẽ của Apple", Price = 60000000, Images = new List<string> { "/images/default/macbookpro16.jpg" }, CategoryId = 2 },
            new Product { Name = "Dell XPS 15", Description = "Laptop cao cấp của Dell", Price = 40000000, Images = new List<string> { "/images/default/dellxps.jpg" }, CategoryId = 2 },
            new Product { Name = "Asus ROG Zephyrus G14", Description = "Laptop gaming mỏng nhẹ của Asus", Price = 45000000, Images = new List<string> { "/images/default/rogzephyrus.jpg" }, CategoryId = 2 },

            // Phụ kiện
            new Product { Name = "AirPods Pro 2", Description = "Tai nghe không dây cao cấp của Apple", Price = 6500000, Images = new List<string> { "/images/default/airpodspro2.jpg" }, CategoryId = 3 },
            new Product { Name = "Samsung Galaxy Buds 2 Pro", Description = "Tai nghe không dây của Samsung", Price = 5500000, Images = new List<string> { "/images/default/galaxybuds2pro.jpg" }, CategoryId = 3 },
            new Product { Name = "Sạc nhanh 65W Anker", Description = "Bộ sạc nhanh công suất cao", Price = 1200000, Images = new List<string> { "/images/default/anker65w.jpg" }, CategoryId = 3 },

            // Đồng hồ thông minh
            new Product { Name = "Apple Watch Series 9", Description = "Đồng hồ thông minh của Apple", Price = 12000000, Images = new List<string> { "/images/default/applewatch9.jpg" }, CategoryId = 4 },
            new Product { Name = "Samsung Galaxy Watch 6", Description = "Đồng hồ thông minh của Samsung", Price = 9000000, Images = new List<string> { "/images/default/galaxywatch6.jpg" }, CategoryId = 4 },
            new Product { Name = "Garmin Fenix 7X", Description = "Đồng hồ thể thao chuyên dụng", Price = 25000000, Images = new List<string> { "/images/default/garminfenix7x.jpg" }, CategoryId = 4 },

            // Máy tính bảng
            new Product { Name = "iPad Pro 12.9 M2", Description = "Máy tính bảng mạnh mẽ của Apple", Price = 32000000, Images = new List<string> { "/images/default/ipadpro12.jpg" }, CategoryId = 5 },
            new Product { Name = "Samsung Galaxy Tab S9", Description = "Tablet cao cấp của Samsung", Price = 28000000, Images = new List<string> { "/images/default/galaxytabs9.jpg" }, CategoryId = 5 }
        };

        _context.Products.AddRange(defaultProducts);
        _context.SaveChanges();
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void UpdateProduct(Product updatedProduct)
    {
        var existingProduct = _context.Products.FirstOrDefault(p => p.Id == updatedProduct.Id);
        if (existingProduct != null)
        {
            existingProduct.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            existingProduct.Images = updatedProduct.Images;
            _context.SaveChanges();
        }
    }

    public void DeleteProduct(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product != null)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }

    public List<Product> SearchProducts(string searchName)
    {
        return _context.Products
            .Where(p => p.Name.ToLower().Contains(searchName.ToLower()))
            .ToList();
    }

    public List<Category> GetCategories()
    {
        return _context.Categories.ToList();
    }

    public List<Product> GetProductsByCategory(int categoryId)
    {
        return _context.Products.Where(p => p.CategoryId == categoryId).ToList();
    }
}
