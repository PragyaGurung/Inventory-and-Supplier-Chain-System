using System;
using System.Collections.Generic;
using System.Text;
using Inventory_and_Supplier_Chain_System.Models;
using Inventory_and_Supplier_Chain_System.Data;

namespace Inventory_and_Supplier_Chain_System.Models
{
    internal class Purchase
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int SupplierId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }

        public Purchase() 
        {
            PurchaseDate = DateTime.Now;
        }
        
        // Navigation property
        public Product Product { get; set; }
    }
}
