
using Inventory_and_Supplier_Chain_System.Models;
﻿using Inventory_and_Supplier_Chain_System.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace Inventory_and_Supplier_Chain_System.Services
{

    public class InventoryService
    {
        

        public bool UpdateProduct(Product product)
        {
            try
            {
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    string query = @"
                    UPDATE Products 
                    SET Name = @Name, 
                        Description = @Description, 
                        Price = @Price, 
                        Quantity = @Quantity,  
                        SupplierId = @SupplierId, 
                        LastUpdated = @LastUpdated
                    WHERE Id = @Id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", product.Id);
                        command.Parameters.AddWithValue("@Name", product.Name);
                        command.Parameters.AddWithValue("@Description", product.Description);
                        command.Parameters.AddWithValue("@Price", product.Price);
                        command.Parameters.AddWithValue("@Quantity", product.Quantity);
                        command.Parameters.AddWithValue("@SupplierId", product.supplierId);
                        command.Parameters.AddWithValue("@LastUpdated", DateTime.Now);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
                return false;
            }
        }
        // DELETE
        public bool DeleteProduct(int id)
        {
            try
            {
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM Products WHERE Id = @Id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return false;
            }
        }

        // Check stock availability
        public bool CheckStockAvailability(int productId, int requestedQuantity)
        {
            var product = GetProductById(productId);
            return product != null && product.Quantity >= requestedQuantity;
        }

        // Update stock quantity (prevent negative inventory)
        public bool UpdateStock(int productId, int quantityChange)
        {
            try
            {
                using (var connection = _database.GetConnection())
                {
                    connection.Open();

                    // First get current quantity
                    string getQuery = "SELECT Quantity FROM Products WHERE Id = @Id";
                    int currentQuantity = 0;

                    using (var getCommand = new SQLiteCommand(getQuery, connection))
                    {
                        getCommand.Parameters.AddWithValue("@Id", productId);
                        var result = getCommand.ExecuteScalar();
                        if (result != null)
                        {
                            currentQuantity = Convert.ToInt32(result);
                        }
                    }

                    // Prevent negative inventory
                    int newQuantity = currentQuantity + quantityChange;
                    if (newQuantity < 0)
                    {
                        Console.WriteLine($"Error: Cannot have negative inventory. Current: {currentQuantity}, Change: {quantityChange}");
                        return false;
                    }

                    // Update quantity
                    string updateQuery = @"
                    UPDATE Products 
                    SET Quantity = Quantity + @Change
                    WHERE Id = @Id";

                    using (var command = new SQLiteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Id", productId);
                        command.Parameters.AddWithValue("@Change", quantityChange);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating stock: {ex.Message}");
                return false;
            }
        }

    }
}
