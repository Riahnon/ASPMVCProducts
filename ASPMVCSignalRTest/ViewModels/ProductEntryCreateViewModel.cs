using ASPMVCSignalRTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPMVCSignalRTest.ViewModels
{
    public class ProductEntryCreateViewModel
    {
        public int ListId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public string Comments { get; set; }
    }
}