using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ASPMVCProducts.Models
{
    public class ProductsDb : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
    }
}