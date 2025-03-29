using System;
using System.Linq;
using System.Collections.Generic;
using QLBH.Models;

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
    }

    public void LoadDefaultProducts()
    {
        var defaultProducts = new List<Product>
        {
            new Product { Name = "iPhone 15", Description = "Điện thoại cao cấp của Apple", Price = 25000000, Images = new List<string> { "/images/default/iphone15.jpg" } },
            new Product { Name = "Samsung Galaxy S23", Description = "Flagship của Samsung", Price = 23000000, Images = new List<string> { "/images/default/galaxys23.jpg" } },
            new Product { Name = "MacBook Pro", Description = "Laptop mạnh mẽ của Apple", Price = 45000000, Images = new List<string> { "/images/default/macbookpro.jpg" } },
            new Product { Name = "Dell XPS 15", Description = "Laptop cao cấp của Dell", Price = 40000000, Images = new List<string> { "/images/default/dellxps.jpg" } }
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
}
