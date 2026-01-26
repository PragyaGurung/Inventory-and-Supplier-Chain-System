using System;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Supplier_Chain_System.Models
{
    /// <summary>
    /// Represents a purchase transaction
    /// </summary>
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        // Navigation property
        public Product Product { get; set; }
    }
}