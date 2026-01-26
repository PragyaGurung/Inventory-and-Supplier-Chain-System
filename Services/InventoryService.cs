<<<<<<< HEAD
﻿using Inventory_and_Supplier_Chain_System.Models;
=======
﻿using Inventory_and_Supplier_Chain_System.Data;
>>>>>>> 547e0a306327490fb41f2b408d605a0807fc016c
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_and_Supplier_Chain_System.Services
{
<<<<<<< HEAD
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

                    using (var command = new SqlServer(query, connection))
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
=======
    public class InventoryService 
    {
    
>>>>>>> 547e0a306327490fb41f2b408d605a0807fc016c

    }
   
}
