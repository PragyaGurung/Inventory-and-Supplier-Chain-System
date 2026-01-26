using Inventory_and_Supplier_Chain_System.Data;
using Inventory_and_Supplier_Chain_System.Exceptions;
using Inventory_and_Supplier_Chain_System.Services;
using System;

namespace Inventory_and_Supplier_Chain_System
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║  INVENTORY & SUPPLIER CHAIN SYSTEM         ║");
            Console.WriteLine("║  Team Members:                             ║");
            Console.WriteLine("║  - PRAGYA GURUNG                           ║");
            Console.WriteLine("║  - SUDIKSHYA SHRESTHA                      ║");
            Console.WriteLine("║  - ALISHA PANTA                            ║");
            Console.WriteLine("║  - SHISAM KHADKA                           ║");
            Console.WriteLine("╚════════════════════════════════════════════╝\n");

            // Initialize context and services
            var context = new InventoryContext();
            context.Database.EnsureCreated();

            var productService = new ProductService(context);
            var supplierService = new SupplierService(context);
            var purchaseService = new PurchaseService(context, productService);

            // Seed initial data if needed
            SeedData(supplierService, productService);

            // Main menu loop
            while (true)
            {
                try
                {
                    DisplayMenu();
                    Console.Write("\nEnter your choice: ");
                    string choice = Console.ReadLine();

                    // Use switch statement for menu handling
                    switch (choice)
                    {
                        case "1":
                            AddSupplierMenu(supplierService);
                            break;
                        case "2":
                            supplierService.DisplaySuppliers();
                            break;
                        case "3":
                            AddProductMenu(productService);
                            break;
                        case "4":
                            productService.DisplayProducts();
                            break;
                        case "5":
                            UpdateProductMenu(productService);
                            break;
                        case "6":
                            DeleteProductMenu(productService);
                            break;
                        case "7":
                            ProcessPurchaseMenu(purchaseService);
                            break;
                        case "8":
                            purchaseService.DisplayPurchases();
                            break;
                        case "9":
                            supplierService.ShowSupplierStockValue();
                            break;
                        case "10":
                            LowStockMenu(productService);
                            break;
                        case "11":
                            purchaseService.ShowPurchaseStatistics();
                            break;
                        case "0":
                            Console.WriteLine("\nThank you for using the Inventory System!");
                            return;
                        default:
                            Console.WriteLine("Invalid choice! Please try again.");
                            break;
                    }

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
                catch (InventoryException ex)
                {
                    // Custom exception handling
                    Console.WriteLine($"\n✗ Inventory Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Generic exception handling
                    Console.WriteLine($"\n✗ Unexpected Error: {ex.Message}");
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\n========== MAIN MENU ==========");
            Console.WriteLine("SUPPLIER OPERATIONS:");
            Console.WriteLine("  1. Add Supplier");
            Console.WriteLine("  2. View All Suppliers");
            Console.WriteLine("\nPRODUCT OPERATIONS:");
            Console.WriteLine("  3. Add Product");
            Console.WriteLine("  4. View All Products");
            Console.WriteLine("  5. Update Product");
            Console.WriteLine("  6. Delete Product");
            Console.WriteLine("\nPURCHASE OPERATIONS:");
            Console.WriteLine("  7. Process Purchase");
            Console.WriteLine("  8. View Purchase History");
            Console.WriteLine("\nREPORTS:");
            Console.WriteLine("  9. Supplier-wise Stock Value");
            Console.WriteLine("  10. Low Stock Alert");
            Console.WriteLine("  11. Purchase Statistics");
            Console.WriteLine("\n  0. Exit");
        }

        // ========== MENU HANDLERS ==========

        static void AddSupplierMenu(SupplierService service)
        {
            Console.WriteLine("\n--- Add New Supplier ---");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Phone: ");
            string phone = Console.ReadLine();

            service.AddSupplier(name, email, phone);
        }

        static void AddProductMenu(ProductService service)
        {
            Console.WriteLine("\n--- Add New Product ---");
            Console.Write("Product Name: ");
            string name = Console.ReadLine();
            Console.Write("Description: ");
            string description = Console.ReadLine();
            Console.Write("Price: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Initial Quantity: ");
            int quantity = int.Parse(Console.ReadLine());
            Console.Write("Supplier ID: ");
            int supplierId = int.Parse(Console.ReadLine());

            service.AddProduct(name, description, price, quantity, supplierId);
        }

        static void UpdateProductMenu(ProductService service)
        {
            Console.WriteLine("\n--- Update Product ---");
            Console.Write("Product ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("New Price (leave blank to skip): ");
            string priceInput = Console.ReadLine();
            Console.Write("New Quantity (leave blank to skip): ");
            string qtyInput = Console.ReadLine();

            decimal? price = string.IsNullOrWhiteSpace(priceInput) ? null : decimal.Parse(priceInput);
            int? qty = string.IsNullOrWhiteSpace(qtyInput) ? null : int.Parse(qtyInput);

            service.UpdateProduct(id, price, qty);
        }

        static void DeleteProductMenu(ProductService service)
        {
            Console.WriteLine("\n--- Delete Product ---");
            Console.Write("Product ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Are you sure? (y/n): ");

            if (Console.ReadLine().ToLower() == "y")
            {
                service.DeleteProduct(id);
            }
        }

        static void ProcessPurchaseMenu(PurchaseService service)
        {
            Console.WriteLine("\n--- Process Purchase ---");
            Console.Write("Product ID: ");
            int productId = int.Parse(Console.ReadLine());
            Console.Write("Quantity (use negative for sales): ");
            int quantity = int.Parse(Console.ReadLine());

            service.ProcessPurchase(productId, quantity);
        }

        static void LowStockMenu(ProductService service)
        {
            Console.Write("\nEnter threshold (default 10): ");
            string input = Console.ReadLine();
            int threshold = string.IsNullOrWhiteSpace(input) ? 10 : int.Parse(input);

            service.ShowLowStockProducts(threshold);
        }

        static void SeedData(SupplierService supplierService, ProductService productService)
        {
            try
            {
                supplierService.AddSupplier("Tech Supplies Ltd", "contact@techsupplies.com", "555-1001");
                supplierService.AddSupplier("Office Mart", "info@officemart.com", "555-1002");

                productService.AddProduct("Laptop", "Dell Inspiron 15", 45000, 15, 1);
                productService.AddProduct("Mouse", "Wireless Optical Mouse", 500, 50, 1);
                productService.AddProduct("Notebook", "A4 Spiral Notebook", 80, 100, 2);
            }
            catch
            {
                // Data already seeded
            }
        }
    }
}

