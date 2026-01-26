using Inventory_and_Supplier_Chain_System.Models;
using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Text;
using Inventory_and_Supplier_Chain_System.Data;

namespace Inventory_and_Supplier_Chain_System.Services
{
    public class SupplierService
    {
        public void CreateSupplier()
        {
            // Implementation for creating a supplier
            using (var db = new Data.Database())
            {
                var supplier = new Models.Supplier
                {
                    Name = "New Supplier",
                    ContactPerson = "John Doe",
                    Email = "john@gmail.com",
                    Phone = "123-456-7890",
                    Address = "123 Main"
                };
            }

        }
        //update supplier
        public bool UpdateSupplier(Supplier supplier)
        {
            try
            {
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    string query = @"
                    UPDATE Suppliers 
                    SET Name = @Name, 
                        ContactPerson = @ContactPerson, 
                        Email = @Email, 
                        Phone = @Phone, 
                        Address = @Address, 
                        IsActive = @IsActive
                    WHERE Id = @Id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", supplier.id);
                        command.Parameters.AddWithValue("@Name", supplier.Name);
                        command.Parameters.AddWithValue("@ContactPerson", supplier.ContactPerson);
                        command.Parameters.AddWithValue("@Email", supplier.Email);
                        command.Parameters.AddWithValue("@Phone", supplier.Phone);
                        command.Parameters.AddWithValue("@Address", supplier.Address);
                        command.Parameters.AddWithValue("@IsActive", supplier.IsActive);

                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating supplier: {ex.Message}");
                return false;
            }
        }

        //delete supplier
        public bool DeleteSupplier(int id)
        {
            try
            {
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM Suppliers WHERE Id = @Id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting supplier: {ex.Message}");
                return false;
            }
        }

        // Get supplier-wise stock value
        public Dictionary<string, decimal> GetSupplierWiseStockValue()
        {
            var result = new Dictionary<string, decimal>();

            try
            {
                using (var connection = _database.GetConnection())
                {
                    connection.Open();
                    string query = @"
                    SELECT s.Name, COALESCE(SUM(p.Price * p.Quantity), 0) as TotalValue
                    FROM Suppliers s
                    LEFT JOIN Products p ON s.Id = p.SupplierId
                    WHERE s.IsActive = 1
                    GROUP BY s.Id, s.Name
                    ORDER BY TotalValue DESC";

                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(
                                reader["Name"].ToString(),
                                Convert.ToDecimal(reader["TotalValue"])
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting supplier-wise stock value: {ex.Message}");
            }

            return result;
        }
    }
}

