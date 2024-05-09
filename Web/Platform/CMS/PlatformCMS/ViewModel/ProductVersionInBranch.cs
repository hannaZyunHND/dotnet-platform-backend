using MI.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformCMS.ViewModel
{
    public class ProductVersionInBranch
    {
        public int IDBranch { get; set; }
        public List<ProductVersionByBranch> ProductVersionByBranches { get; set; }
    }
}
