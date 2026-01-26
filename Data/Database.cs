using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Inventory_and_Supplier_Chain_System.Models;
using System.Linq;

namespace Inventory_and_Supplier_Chain_System.Data
{
    public class Database : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Purchase> Purchases  { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                //        "Server=localhost;Database=SMSdB;User=Sudikshya; Password=;",
                //        ServerVersion.AutoDetect("Server=localhost;Database=SMSdB;" User=Sudikshya; Password =; ")
                //

                @"Server=ASUS; Database=Inventory-and-Suppliers;Trusted_Connection=True;TrustServerCertificate=True;");
        }

    }
}
