﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ASPMVCSignalRTest.Models
{
    public class ProductsDb : DbContext
    {

        public DbSet<Product> Products { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ProductList> ProductLists { get; set; }
    }
}