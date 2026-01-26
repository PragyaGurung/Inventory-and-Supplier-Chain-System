using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Inventory_Supplier_Chain_System.Models
{
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

        // one-to-many relationship
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
