using ASPMVCProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPMVCProducts.ViewModels
{
    public class ProductEntryEditViewModel
    {
        public int ListId { get; set; }
        public int EntryId { get; set; }
        public bool Checked { get; set; }
        public int Ammount { get; set; }
        public string Comments { get; set; }
    }
}