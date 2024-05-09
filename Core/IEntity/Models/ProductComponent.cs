using System;
using System.Collections.Generic;

namespace MI.Entity.Models
{
    public partial class ProductComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsShow { get; set; }
        public bool? IsDelete { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedById { get; set; }
    }
}
