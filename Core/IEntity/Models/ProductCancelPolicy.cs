using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class ProductCancelPolicy
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; } = 0;
        public int BeforeDate { get; set; } = 0;
        public int RollbackValue { get; set; } = 0;
        public int RollbackValueOption { get; set; } = 1;
    }
}
