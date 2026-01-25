using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_and_Supplier_Chain_System.Models
{
    public class Supplier
    {
        public int id {  get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public decimal TotalStockValue { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

        public Supplier()
        {
            CreatedDate = DateTime.Now;
            IsActive = true;
        }
    }
}
