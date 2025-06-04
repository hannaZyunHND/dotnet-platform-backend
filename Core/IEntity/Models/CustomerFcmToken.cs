using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class CustomerFcmToken
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string FcmToken { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
