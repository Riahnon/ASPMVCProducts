using ASPMVCSignalRTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPMVCSignalRTest.ViewModels
{
    public class ProductEntryEditViewModel
    {
        public int ListId { get; set; }
        public int EntryId { get; set; }
        public bool Checked { get; set; }
        public int Amount { get; set; }
        public string Comments { get; set; }
    }
}