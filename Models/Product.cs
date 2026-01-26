using Inventory_and_Supplier_Chain_System.Models;
using System.ComponentModel.DataAnnotations;

namespace InventorySupplierChainSystem.Models
{
    /// <summary>
    /// Represents a product in the inventory
    /// </summary>
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int StockQuantity { get; set; }

        [Required]
        public int SupplierId { get; set; }

        // Navigation property
        public Supplier Supplier { get; set; }
    }
}
