using Inventory_and_Supplier_Chain_System.Data;
using Inventory_and_Supplier_Chain_System.Services;
using Inventory_Supplier_Chain_System.Models;
using Inventory_and_Supplier_Chain_System.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory_and_Supplier_Chain_System.Services
{
    public class PurchaseService
    {
        private readonly InventoryContext _context;
        private readonly ProductService _productService;

        public PurchaseService(InventoryContext context, ProductService productService)
        {
            _context = context;
            _productService = productService;
        }

        public void ProcessPurchase(int productId, int quantity)
        {
            try
            {
                var product = _productService.GetProductById(productId);
                if (product == null)
                {
                    throw new InventoryException($"Product with ID {productId} not found");
                }

                // validate quantity
                    if (quantity <0)
                    {
                        throw new InventoryException("Product purchase cannot be negative.");
                    }
            
                // updating stock using ProductService
                _productService.UpdateStock(productId, quantity);

                // recording purchase
                var purchase = new Purchase
                {
                    ProductId = productId,
                    Quantity = quantity,
                    PurchaseDate = DateTime.Now,
                    TotalAmount = product.Price * quantity
                };

                _context.Purchases.Add(purchase);
                _context.SaveChanges();
             
                var updatedProduct = _productService.GetProductById(productId);
                Console.WriteLine($" Purchase processed! New stock: {updatedProduct.StockQuantity}");
            }
            catch (NegativeInventoryException ne)
            {
                Console.WriteLine(ne.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DisplayPurchases()
        {
            var purchases = _context.Purchases
                .Include(p => p.Product)
                .OrderByDescending(p => p.PurchaseDate)
                .ToList();

            if (!purchases.Any())
            {
                Console.WriteLine("No purchases found.");
                return;
            }

            Console.WriteLine("\n========== PURCHASE HISTORY ==========");
            Console.WriteLine($"{"ID",-5} {"Product",-20} {"Quantity",-10} {"Amount",-12} {"Date",-20}");
            Console.WriteLine(new string('-', 70));

            foreach (var purchase in purchases)
            {
                Console.WriteLine($"{purchase.PurchaseId,-5} {purchase.Product.Name,-20} " +
                    $"{purchase.Quantity,-10} ${purchase.TotalAmount,-11:F2} " +
                    $"{purchase.PurchaseDate,-20:yyyy-MM-dd HH:mm}");
            }
        }


        public List<Purchase> GetPurchasesByProduct(int productId)
        {
            return _context.Purchases
                .Where(p => p.ProductId == productId)
                .OrderByDescending(p => p.PurchaseDate)
                .ToList();
        }


        public void ShowPurchaseStatistics()
        {
            var stats = _context.Purchases
                .Include(p => p.Product)
                .GroupBy(p => p.Product.Name)
                .Select(g => new
                {
                    ProductName = g.Key,
                    TotalPurchases = g.Count(),
                    TotalQuantity = g.Sum(p => p.Quantity),
                    TotalAmount = g.Sum(p => p.TotalAmount)
                })
                .OrderByDescending(s => s.TotalAmount)
                .ToList();

            if (!stats.Any())
            {
                Console.WriteLine("No purchase statistics available.");
                return;
            }

            Console.WriteLine("\n========== PURCHASE STATISTICS ==========");
            Console.WriteLine($"{"Product",-20} {"Purchases",-12} {"Quantity",-10} {"Total Amount",-15}");
            Console.WriteLine(new string('-', 60));

            foreach (var stat in stats)
            {
                Console.WriteLine($"{stat.ProductName,-20} {stat.TotalPurchases,-12} " +
                    $"{stat.TotalQuantity,-10} ${stat.TotalAmount,-14:F2}");
            }
        }
    }
}