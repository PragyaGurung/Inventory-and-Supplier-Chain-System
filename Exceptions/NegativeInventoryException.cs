using System;

namespace InventorySupplierChainSystem.Exceptions
{
    /// <summary>
    /// Custom exception for negative inventory scenarios
    /// </summary>
    public class NegativeInventoryException : InventoryException
    {
        public NegativeInventoryException(string productName)
            : base($"Cannot have negative inventory for product: {productName}") { }
    }
}