using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MI.Entity.Models
{
    public class Responses
    {
        [Key]
        public int Response_Id { get; set; } // Mã định danh cho mỗi lần phản hồi
        public int Survey_Id { get; set; } // Liên kết câu trả lời với khảo sát
        public string Email { get; set; } // Địa chỉ email của người dùng (nếu cần)
        public DateTime Submitted_At { get; set; } = DateTime.Now; // Ngày giờ gửi câu trả lời
    }

}