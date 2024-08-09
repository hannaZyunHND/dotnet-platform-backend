using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MI.Entity.Models
{
    public class OrderDetailFeedback
    {
        [Key]
        public int Id { get; set; }
        public int OrderDetailId { get; set; }
        public string TitleComment { get; set; }
        public string ContentComment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string FileUpload { get; set; }
        public bool? IsConfirm { get; set; }
    }
}
