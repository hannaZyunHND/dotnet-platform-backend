using System;
using System.Collections.Generic;
using System.Text;

namespace MI.Entity.Models
{
    public class Subscribers
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UnsubscribeAt { get; set; } 
    }
}
