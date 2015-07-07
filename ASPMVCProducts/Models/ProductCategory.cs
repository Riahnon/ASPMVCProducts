﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASPMVCProducts.Models
{
    public class ProductCategory
    {
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [StringLength(256)] 
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}