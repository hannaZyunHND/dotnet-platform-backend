using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MI.Entity.Models
{
    public class Options
    {
        [Key]
        public int Option_Id { get; set; } // Mã định danh cho mỗi lựa chọn
        public int Question_Id { get; set; } // Liên kết lựa chọn với câu hỏi
        public string Text { get; set; } // Nội dung lựa chọn
        public int Value { get; set; } // Giá trị của lựa chọn
    }
}