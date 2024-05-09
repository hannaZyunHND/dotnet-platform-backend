using System;
using System.Collections.Generic;
using System.Text;

namespace MI.Entity.Models
{
    public partial class ProductBranch
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? LocationID { get; set; }
    }
}
