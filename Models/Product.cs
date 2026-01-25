using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_and_Supplier_Chain_System.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int supplierId { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdatedDate { get; set; }
        public Product() {
            CreatedDate = DateTime.Now;
        }




    }
}
