using Inventory_and_Supplier_Chain_System.Data;
using Inventory_and_Supplier_Chain_System.Exceptions;

using Inventory_Supplier_Chain_System.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory_and_Supplier_Chain_System.Services
{
    public class ProductService
    {
        private readonly InventoryContext _context;

        public ProductService(InventoryContext context)
        {
            _context = context;
        }

        public void AddProduct(string name, string description, decimal price,
            int quantity, int supplierId)
        {
            try
            {

                // Input validation
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new InventoryException("Product name cannot be empty.");
                }

                if (name.Length > 100)
                {
                    throw new InventoryException("Product name cannot exceed 100 characters.");
                }

                if (string.IsNullOrWhiteSpace(description))
                {
                    throw new InventoryException("Product description cannot be empty.");
                }

                if (description.Length > 500)
                {
                    throw new InventoryException("Product description cannot exceed 500 characters.");
                }

                if (price <= 0)
                {
                    throw new InventoryException("Product price must be greater than zero.");
                }

                if (quantity < 0)
                {
                    throw new InventoryException("Product quantity cannot be negative.");
                }


                // validating supplier exists
                var supplier = _context.Suppliers.Find(supplierId);
                if (supplier == null)
                {
                    throw new InventoryException($"Supplier with ID {supplierId} not found");
                }

                var product = new Product
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    StockQuantity = quantity,
                    SupplierId = supplierId
                };

                _context.Products.Add(product);
                _context.SaveChanges();

                Console.WriteLine($"Product '{name}' added successfully!");
            }
            catch (Exception ex)
            {
                throw new InventoryException("Error adding product", ex);
            }
        }

        public void DisplayProducts()
        {
            // LINQ query 
            var products = _context.Products
                .Include(p => p.Supplier)
                .OrderBy(p => p.Name)
                .ToList();

            if (!products.Any())
            {
                Console.WriteLine("No products found.");
                return;
            }

            Console.WriteLine("\n========== INVENTORY ==========");

            Console.WriteLine($"{"ID",-5} {"Product Name",-20} {"Price",-10} {"Stock",-8} {"Supplier",-15}");
            Console.WriteLine(new string('-', 70));

            foreach (var product in products)
            {
                Console.WriteLine($"{product.ProductId,-5} {product.Name,-20} " +
                    $"RS{product.Price,-9:F2} {product.StockQuantity,-8} " +
                    $"{product.Supplier.Name,-15}");
            }
        }

        public void UpdateProduct(int productId, decimal? newPrice = null,
            int? newQuantity = null)
        {
            try
            {
                var product = _context.Products.Find(productId);
                if (product == null)
                {
                    throw new InventoryException($"Product with ID {productId} not found");
                }

                // validating and updating price
                if (newPrice.HasValue)
                {
                    if (newPrice.Value <= 0)
                    {
                        throw new InventoryException("Product price must be greater than zero.");
                    }
                    product.Price = newPrice.Value;
                }

                // validating and updating quantity
                if (newQuantity.HasValue)
                {
                    if (newQuantity.Value < 0)
                    {
                        throw new InventoryException("Product quantity cannot be negative.");
                    }
                    product.StockQuantity = newQuantity.Value;
                }

                _context.SaveChanges();
                Console.WriteLine($" Product '{product.Name}' updated successfully!");
            }
            catch (Exception ex)
            {
                throw new InventoryException("Error updating product", ex);
            }
        }

        public void DeleteProduct(int productId)
        {
            try
            {
                var product = _context.Products.Find(productId);
                if (product == null)
                {
                    throw new InventoryException($"Product with ID {productId} not found");
                }

                _context.Products.Remove(product);
                _context.SaveChanges();
                Console.WriteLine($" Product deleted successfully!");
            }
            catch (Exception ex)
            {
                throw new InventoryException("Error deleting product", ex);
            }
        }

        public void UpdateStock(int productId, int quantityPurchased)
        {
            var product = _context.Products.Find(productId);
            if (product == null)
            {
                throw new InventoryException($"Product with ID {productId} not found");
            }

            // auto-updating stock
            product.StockQuantity -= quantityPurchased;


            // preventing negative inventory
            if (product.StockQuantity < 0)
            {
                throw new NegativeInventoryException(product.Name);
            }

            _context.SaveChanges();
        }

        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }


        public void ShowLowStockProducts(int threshold = 10)
        {
            var lowStock = _context.Products
                .Include(p => p.Supplier)
                .Where(p => p.StockQuantity < threshold)
                .OrderBy(p => p.StockQuantity)
                .ToList();

            if (!lowStock.Any())
            {
                Console.WriteLine("No low stock products.");
                return;
            }

            Console.WriteLine($"\n========== LOW STOCK ALERT (< {threshold}) ==========");
            foreach (var product in lowStock)
            {
                Console.WriteLine($" {product.Name}: {product.StockQuantity} units " +
                    $"(Supplier: {product.Supplier.Name})");
            }
        }

        public List<Product> GetProductsBySupplier(int supplierId)
        {
            return _context.Products
                .Where(p => p.SupplierId == supplierId)
                .ToList();
        }
    }
}

