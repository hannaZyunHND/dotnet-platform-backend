using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MI.Entity.Models
{

    // Bảng Answers
    public class Answers
    {
        [Key]
        public int Answer_Id { get; set; } // Mã định danh cho mỗi câu trả lời
        public int Response_Id { get; set; } // Liên kết với phản hồi
        public int Question_Id { get; set; } // Liên kết với câu hỏi
        public int? Option_Id { get; set; } // Liên kết với lựa chọn (có thể null)
        public string Text_Answer { get; set; } // Câu trả lời dạng văn bản
    }

}