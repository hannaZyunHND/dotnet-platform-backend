using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class OrderChatSessionDetail
    {
        [Key]
        public int Id { get; set; }
        public int OrderChatSessionId { get; set; }
        public string Sender { get; set; }
        public int SenderType { get; set; } // 1: Supplier, 2: Customer, 3: Administrator
        public string Content { get; set; }
        public int ContentType { get; set; } // 1: Text, 2: Image
        public DateTime CreatedDate { get; set; }
        public bool? isSeen { get; set; } = false;
        public bool? isSupplierSeen { get; set; } = false;
        public bool? isAdministrationSeen { get; set; } = false;
        public bool? IsNotiCustomer { get; set; } = false;
        public bool? IsNotiSupplier { get; set; } = false;

    }
}
