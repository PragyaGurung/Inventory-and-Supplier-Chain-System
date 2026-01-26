using System;

namespace Inventory_and_Supplier_Chain_System.Exceptions
{
    public class NegativeInventoryException : InventoryException
    {
        public NegativeInventoryException(string productName)
            : base($"Cannot have negative inventory for product: {productName}") { }
    }
}