using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory_and_Supplier_Chain_System.Models
{
    public class Supplier
    {
        public int id {  get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public string phone { get; set; }

        public string address { get; set; }


        public Supplier() { }

    }
}
