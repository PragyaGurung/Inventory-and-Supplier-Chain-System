using Inventory_and_Supplier_Chain_System.Data;
using Inventory_and_Supplier_Chain_System.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Inventory_Supplier_Chain_System.Models;

namespace Inventory_and_Supplier_Chain_System.Services
{
    public class SupplierService
    {
        private readonly InventoryContext _context;

        public SupplierService(InventoryContext context)
        {
            _context = context;
        }


        public void AddSupplier(string name, string email, string phone)
        {
            try
            {

                // input validation
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new InventoryException("Supplier name cannot be empty.");
                }

                if (name.Length > 100)
                {
                    throw new InventoryException("Supplier name cannot exceed 100 characters.");
                }

                if (!IsValidEmail(email))
                {
                    throw new InventoryException("Invalid email format. Please enter a valid email address.");
                }

                if (!IsValidPhone(phone))
                {
                    throw new InventoryException("Invalid phone number. Phone must be 10-15 digits and can include '+', '-', or spaces.");
                }

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

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // basic email validation using regex pattern
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
            }
            catch
            {
                return false;
            }
        }


        private bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // removing common phone number characters
            string cleanPhone = phone.Replace("+", "").Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

            return cleanPhone.All(char.IsDigit) && cleanPhone.Length >= 10 && cleanPhone.Length <= 15;
        }


        


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

                // validating and updating name
                if (!string.IsNullOrWhiteSpace(newName))
                {
                    if (newName.Length > 100)
                    {
                        throw new InventoryException("Supplier name cannot exceed 100 characters.");
                    }
                    supplier.Name = newName;
                }

                // validating and updating email
                if (!string.IsNullOrWhiteSpace(newEmail))
                {
                    if (!IsValidEmail(newEmail))
                    {
                        throw new InventoryException("Invalid email format. Please enter a valid email address.");
                    }
                    supplier.ContactEmail = newEmail;
                }

                // validating and updating phone
                if (!string.IsNullOrWhiteSpace(newPhone))
                {
                    if (!IsValidPhone(newPhone))
                    {
                        throw new InventoryException("Invalid phone number. Phone must be 10-15 digits and can include '+', '-', or spaces.");
                    }
                    supplier.Phone = newPhone;
                }

                _context.SaveChanges();
                Console.WriteLine($"✓ Supplier '{supplier.Name}' updated successfully!");

            }
            catch (Exception ex)
            {
                throw new InventoryException("Error updating supplier", ex);
            }
        }


        

        public void ShowSupplierStockValue()
        {
           
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
                    $"RS{supplier.TotalStockValue,-14:F2}");
            }
        }


        public Supplier GetSupplierById(int supplierId)
        {
            return _context.Suppliers.Find(supplierId);
        }


        public List<Supplier> GetAllSuppliers()
        {
            return _context.Suppliers.OrderBy(s => s.Name).ToList();
        }
    }
}
