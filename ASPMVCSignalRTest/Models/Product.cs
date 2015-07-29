using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASPMVCSignalRTest.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [StringLength(256)]
        public string Name { get; set; }
    }
}