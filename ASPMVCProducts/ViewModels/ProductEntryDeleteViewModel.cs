using ASPMVCProducts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPMVCProducts.ViewModels
{
	public class ProductEntryDeleteViewModel
	{
		public int ListId { get; set; }
		public int EntryId { get; set; }
		public string Name { get; set; }
	}
}