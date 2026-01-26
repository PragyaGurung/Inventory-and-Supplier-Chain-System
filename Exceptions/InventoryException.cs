using System;

namespace InventorySupplierChainSystem.Exceptions
{
    /// <summary>
    /// Custom exception for inventory-related errors
    /// </summary>
    public class InventoryException : Exception
    {
        public InventoryException(string message) : base(message) { }

        public InventoryException(string message, Exception inner)
            : base(message, inner) { }
    }
}
