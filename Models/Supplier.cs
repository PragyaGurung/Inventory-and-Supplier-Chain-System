using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_and_Supplier_Chain_System.Models
{
    /// <summary>
    /// Represents a supplier in the system
    /// </summary>
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string ContactEmail { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        // Navigation property - One-to-Many relationship
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
