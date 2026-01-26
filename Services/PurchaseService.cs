using Inventory_and_Supplier_Chain_System.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using Inventory_and_Supplier_Chain_System.Data;
using Inventory_and_Supplier_Chain_System.Services;

namespace Inventory_and_Supplier_Chain_System.Services
{
    public class PurchaseService
    {
        private readonly Database _database;
        private readonly InventoryService _inventoryService;

        public PurchaseService(Database database, InventoryService inventoryService)
        {
            _database = database;
            _inventoryService = inventoryService;
        }

        // CREATE Purchase 
        public bool AddPurchase(Purchase purchase)
        {
            try
            {
                // Check if product exists
                var product = _inventoryService.GetProductById(purchase.ProductId);

                if (product == null)
                {
                    Console.WriteLine("Error: Product not found!");
                    return false;
                }

                // Prevent negative inventory
                if (!_inventoryService.CheckStockAvailability(purchase.ProductId, purchase.Quantity))
                {
                    Console.WriteLine($"Error: Insufficient stock! Available: {product.Quantity}, Requested: {purchase.Quantity}");
                    return false;
                }

                using (var connection = _database.GetConnection())
                {
                    connection.Open();

                    // Insert purchase record
                    string purchaseQuery = @"
                    INSERT INTO Purchases (ProductId, SupplierId, Quantity, UnitPrice, TotalAmount, PurchaseDate) VALUES (@ProductId, @SupplierId, @Quantity, @UnitPrice, @TotalAmount, @PurchaseDate)";

                    using (var command = new SQLiteCommand(purchaseQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", purchase.ProductId);
                        command.Parameters.AddWithValue("@SupplierId", purchase.SupplierId);
                        command.Parameters.AddWithValue("@Quantity", purchase.Quantity);
                        command.Parameters.AddWithValue("@UnitPrice", purchase.UnitPrice);
                        command.Parameters.AddWithValue("@TotalAmount", purchase.TotalAmount);
                        command.Parameters.AddWithValue("@PurchaseDate", purchase.PurchaseDate);

                        command.ExecuteNonQuery();
                    }


                    // Auto-update stock 
                    bool stockUpdated = _inventoryService.UpdateStock(purchase.ProductId, -purchase.Quantity);

                    if (!stockUpdated)
                    {
                        return false;
                    }

                    Console.WriteLine($"Purchase completed successfully! Stock updated.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing purchase: {ex.Message}");
                return false;
            }
        }


        // READ All Purchases
        public List<Purchase> GetAllPurchases()
        {
            var purchases = new List<Purchase>();

            try
            {
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    string query = @"
                    SELECT p.*, pr.Name as ProductName, s.Name as SupplierName FROM Purchases  JOIN Products pr ON p.ProductId = pr.Id  JOIN Suppliers s ON p.SupplierId = s.Id  ORDER BY p.PurchaseDate DESC";

                    using (var command = new SQLiteCommand(query, connection))

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            purchases.Add(new Purchase
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ProductId = Convert.ToInt32(reader["ProductId"]),
                                SupplierId = Convert.ToInt32(reader["SupplierId"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                                TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                PurchaseDate = Convert.ToDateTime(reader["PurchaseDate"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving purchases: {ex.Message}");
            }

            return purchases;
        }

        // READ Purchase by ID
        public Purchase GetPurchaseById(int id)
        {
            try
            {
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT * FROM Purchases WHERE Id = @Id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Purchase
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    SupplierId = Convert.ToInt32(reader["SupplierId"]),
                                    Quantity = Convert.ToInt32(reader["Quantity"]),
                                    UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                                    TotalAmount = Convert.ToDecimal(reader["TotalAmount"]),
                                    PurchaseDate = Convert.ToDateTime(reader["PurchaseDate"])
                                };
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving purchase: {ex.Message}");
            }

            return null;
        }
    }
}
