using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
