using System;
using QLBH.Models;

namespace QLBH.Services;

public class ProductService
{
    List<Product> products = new List<Product>();

    public void LoadProducts()
    {
        products = new List<Product>()
        {
            new Product { Id = 1, Name = "Product 1", Description = "Product 1 description", Price = 400 },
            new Product { Id = 2, Name = "Product 2", Description = "Product 2 description", Price = 200 },
            new Product { Id = 3, Name = "Product 3", Description = "Product 3 description", Price = 300 }
        };
    }

    public List<Product> GetProducts()
    {
        return products;
    }

    public Product GetProductById(int id)
    {
        return products.FirstOrDefault(p => p.Id == id) ?? new Product();
    }

    public int GetNextId()
    {
        return products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
    }

    public void AddProduct(Product product)
    {
        product.Id = GetNextId();
        products.Add(product);
    }

    public void UpdateProduct(int id, string name, string description, int price)
    {
        var existing = products.FirstOrDefault(p => p.Id == id);
        if (existing != null)
        {
            existing.Name = name;
            existing.Description = description;
            existing.Price = price;
        }
    }

    public void DeleteProduct(Product product)
    {
        products.RemoveAll(p => p.Id == product.Id);
    }
}
