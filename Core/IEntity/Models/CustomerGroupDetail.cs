using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class CustomerGroupDetail
    {
        [Key]
        public int Id { get; set; }

        public int CustomerGroupId { get; set; }

        public int CustomerId { get; set; }
    }
}
