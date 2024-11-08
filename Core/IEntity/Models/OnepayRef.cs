using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class OnepayRef
    {
        [Key]
        public int Id { get; set; }

        public string EncryptMerchantTxtCode { get; set; }

        public string Object { get; set; }
    }
}
