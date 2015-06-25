using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ASPMVCProducts.Models
{
    public class Product
    {
        public int Id { get; set; }
        public UserProfile Owner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual List<ProductCategory> Categories { get; set; }
    }
}