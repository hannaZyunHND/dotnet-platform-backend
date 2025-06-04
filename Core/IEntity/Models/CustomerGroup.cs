using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class CustomerGroup
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string CreatedDate { get; set; }
        public string CreateByUsername { get; set; }
    }
}
