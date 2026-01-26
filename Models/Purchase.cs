using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_and_Supplier_Chain_System.Models
{
    public class Purchase
    {
        public int Id { get; set; }
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
    }
}
