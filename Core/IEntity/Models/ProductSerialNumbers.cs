using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class ProductSerialNumbers
    {
        [Key]
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public int ProductId { get; set; }
    }
}
