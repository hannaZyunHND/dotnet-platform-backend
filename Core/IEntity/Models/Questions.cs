using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MI.Entity.Models
{
    public partial class Questions
    {
        [Key]
        public int Question_Id { get; set; }
        public int Survey_Id { get; set; }
        public string Text { get; set; } // Noi dung cau hoi
        public string Question_Type { get; set; } //  Loai cau hoi
        public int Order { get; set; } // Thu tu cau hoi

    }
}



