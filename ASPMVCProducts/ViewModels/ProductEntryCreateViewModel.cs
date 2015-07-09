using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPMVCProducts.ViewModels
{
    public class ProductEntryCreateViewModel
    {
        public string Name { get; set; }
        public int Ammount { get; set; }
        public string Comments { get; set; }
    }
}