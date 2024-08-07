using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class OrderChatSession
    {
        [Key]
        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public string RoomTickName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
