using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MI.Entity.Models
{
    public partial class Redirect
    {
        [Key]
        public int Id { get; set; }
        public string UrlOld { get; set; }
        public string UrlNew { get; set; }
        public int? UrlType { get; set; }
        public bool? Status { get; set; }
    }
}
