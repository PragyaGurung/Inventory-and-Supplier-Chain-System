using Inventory_and_Supplier_Chain_System.Data;
using Inventory_and_Supplier_Chain_System.Exceptions;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory_and_Supplier_Chain_System.Services
{
    /// <summary>
    /// Service class for Supplier-related operations
    /// </summary>
    public class SupplierService
    {
        private readonly InventoryContext _context;

        public SupplierService(InventoryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add a new supplier to the system (CREATE)
        /// </summary>
        public void AddSupplier(string name, string email, string phone)
        {
            try
            {
                var supplier = new Supplier
                {
                    Name = name,
                    ContactEmail = email,
                    Phone = phone
                };

                _context.Suppliers.Add(supplier);
                _context.SaveChanges();

                Console.WriteLine($"Supplier '{name}' added successfully!");
            }
            catch (Exception ex)
            {
                throw new InventoryException("Error adding supplier", ex);
            }
        }

        /// <summary>
        /// Display all suppliers with their products (READ)
        /// </summary>
        public void DisplaySuppliers()
        {
            // LINQ query with Include for navigation properties
            var suppliers = _context.Suppliers
                .Include(s => s.Products)
                .OrderBy(s => s.Name)
                .ToList();

            if (!suppliers.Any())
            {
                Console.WriteLine("No suppliers found.");
                return;
            }

            Console.WriteLine("\n========== SUPPLIERS ==========");
            foreach (var supplier in suppliers)
            {
                Console.WriteLine($"\nSupplier ID: {supplier.SupplierId}");
                Console.WriteLine($"Name: {supplier.Name}");
                Console.WriteLine($"Email: {supplier.ContactEmail}");
                Console.WriteLine($"Phone: {supplier.Phone}");
                Console.WriteLine($"Products Count: {supplier.Products.Count}");
            }
        }

        /// <summary>
        /// Update supplier information (UPDATE)
        /// </summary>
        public void UpdateSupplier(int supplierId, string newName = null,
            string newEmail = null, string newPhone = null)
        {
            try
            {
                var supplier = _context.Suppliers.Find(supplierId);
                if (supplier == null)
                {
                    throw new InventoryException($"Supplier with ID {supplierId} not found");
                }

                if (!string.IsNullOrWhiteSpace(newName))
                    supplier.Name = newName;

                if (!string.IsNullOrWhiteSpace(newEmail))
                    supplier.ContactEmail = newEmail;

                if (!string.IsNullOrWhiteSpace(newPhone))
                    supplier.Phone = newPhone;

                _context.SaveChanges();
                Console.WriteLine($" Supplier '{supplier.Name}' updated successfully!");
            }
            catch (Exception ex)
            {
                throw new InventoryException("Error updating supplier", ex);
            }
        }

        /// <summary>
        /// Delete a supplier (DELETE)
        /// </summary>
        public void DeleteSupplier(int supplierId)
        {
            try
            {
                var supplier = _context.Suppliers
                    .Include(s => s.Products)
                    .FirstOrDefault(s => s.SupplierId == supplierId);

                if (supplier == null)
                {
                    throw new InventoryException($"Supplier with ID {supplierId} not found");
                }

                // Check if supplier has products
                if (supplier.Products.Any())
                {
                    throw new InventoryException(
                        $"Cannot delete supplier '{supplier.Name}' because they have {supplier.Products.Count} product(s). " +
                        "Please reassign or delete products first.");
                }

                _context.Suppliers.Remove(supplier);
                _context.SaveChanges();
                Console.WriteLine($" Supplier deleted successfully!");
            }
            catch (Exception ex)
            {
                throw new InventoryException("Error deleting supplier", ex);
            }
        }

        /// <summary>
        /// Show supplier-wise stock value (Feature requirement)
        /// </summary>
        public void ShowSupplierStockValue()
        {
            // Complex LINQ query with grouping and projection
            var supplierValues = _context.Products
                .Include(p => p.Supplier)
                .GroupBy(p => p.Supplier.Name)
                .Select(g => new
                {
                    SupplierName = g.Key,
                    TotalProducts = g.Count(),
                    TotalStockValue = g.Sum(p => p.Price * p.StockQuantity),
                    TotalQuantity = g.Sum(p => p.StockQuantity)
                })
                .OrderByDescending(s => s.TotalStockValue)
                .ToList();

            Console.WriteLine("\n========== SUPPLIER-WISE STOCK VALUE ==========");
            Console.WriteLine($"{"Supplier",-20} {"Products",-10} {"Quantity",-10} {"Total Value",-15}");
            Console.WriteLine(new string('-', 60));

            foreach (var supplier in supplierValues)
            {
                Console.WriteLine($"{supplier.SupplierName,-20} " +
                    $"{supplier.TotalProducts,-10} " +
                    $"{supplier.TotalQuantity,-10} " +
                    $"${supplier.TotalStockValue,-14:F2}");
            }
        }

        /// <summary>
        /// Get supplier by ID
        /// </summary>
        public Supplier GetSupplierById(int supplierId)
        {
            return _context.Suppliers.Find(supplierId);
        }

        /// <summary>
        /// Get all suppliers
        /// </summary>
        public List<Supplier> GetAllSuppliers()
        {
            return _context.Suppliers.OrderBy(s => s.Name).ToList();
        }
    }
}
