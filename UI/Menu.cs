using System;
using System.Collections.Generic;
using System.Text;
using Inventory_and_Supplier_Chain_System.Data;
using Inventory_and_Supplier_Chain_System.Services;
using Inventory_and_Supplier_Chain_System.Models;
using Inventory_and_Supplier_Chain_System.InputValidation;

namespace Inventory_and_Supplier_Chain_System.UI
{
    public class Menu
    {
        private readonly Database _database;
        private readonly InventoryService _inventoryService;
        private readonly SupplierService _supplierService;
        private readonly PurchaseService _purchaseService;

        public Menu()
        {
            _database = new Database();
            _inventoryService = new InventoryService(_database);
            _supplierService = new SupplierService(_database);
            _purchaseService = new PurchaseService(_database, _inventoryService);
        }

        public void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========= Inventory and Supplier Chain Management System =========");
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1. Product Management");
                Console.WriteLine("2. Supplier Management");
                Console.WriteLine("3. Purchase Management");
                Console.WriteLine("4. Reports and Analytics");
                Console.WriteLine("5. Exit");

                Console.WriteLine("Enter your choice(1-5): ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ShowProductMenu();
                        break;

                    case "2":
                        ShowSuppliersMenu();
                        break;

                    case "3":
                        ShowPurchasesMenu();
                        break;

                    case "4":
                        ShowReportsMenu();
                        break;

                    case "5":
                        Console.WriteLine("Thank you for using the system. Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ShowProductMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========= Product Management =========");
                Console.WriteLine("1. Add New Product");
                Console.WriteLine("2. View All Products");
                Console.WriteLine("3. View Product Details");
                Console.WriteLine("4. Update Product");
                Console.WriteLine("5. Delete Product");
                Console.WriteLine("6 Check Stock Availability");
                Console.WriteLine("7. Back to Main Menu");

                Console.WriteLine("\nEnter your choice(1-7): ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddProduct();
                        break;

                    case "2":
                        ViewAllProducts();
                        break;

                    case "3":
                        ViewProductDetails();
                        break;

                    case "4":
                        UpdateProduct();
                        break;

                    case "5":
                        DeleteProduct();
                        break;

                    case "6":
                        CheckStockAvailability();
                        break;

                    case "7":
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ShowSuppliersMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========= Supplier Management =========");
                Console.WriteLine("1. Add New Supplier");
                Console.WriteLine("2. View All Suppliers");
                Console.WriteLine("3. View Supplier Details");
                Console.WriteLine("4. Update Supplier");
                Console.WriteLine("5. Delete Supplier");
                Console.WriteLine("6. Back to Main Menu");

                Console.WriteLine("\nEnter your choice(1-6): ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddSupplier();
                        break;

                    case "2":
                        ViewAllSuppliers();
                        break;

                    case "3":
                        ViewSupplierDetails();
                        break;

                    case "4":
                        UpdateSupplier();
                        break;

                    case "5":
                        DeleteSupplier();
                        break;

                    case "6":
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ShowPurchasesMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========= Purchase Management =========");
                Console.WriteLine("1. Add New Purchase");
                Console.WriteLine("2. View All Purchases");
                Console.WriteLine("3. View Purchase Details");
                Console.WriteLine("4. Back to Main Menu");

                Console.WriteLine("\nEnter your choice(1-4): ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddPurchase();
                        break;

                    case "2":
                        ViewAllPurchases();
                        break;

                    case "3":
                        ViewPurchaseDetails();
                        break;

                    case "4":
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ShowReportsMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("========= Reports and Analytics =========");
                Console.WriteLine("\n1. Supplier-wise Stock Value");
                Console.WriteLine("2. Back to Main Menu");

                Console.WriteLine("\nEnter your choice(1-2): ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ShowSupplierWiseStockValue();
                        break;

                    case "2":
                        return;

                    case "3":
                        Console.WriteLine("Invalid choice. Please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }



    }
}
