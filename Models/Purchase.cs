using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_and_Supplier_Chain_System.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int  ProductId { get; set; }
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public Purchase() { }
    }
}
