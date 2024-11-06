using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Surveys
{
    [Key]
    public int Survey_Id { get; set; } // Mã định danh duy nhất cho mỗi khảo sát

    public string Title { get; set; } // Tiêu đề của khảo sát

    public string Description { get; set; } // Mô tả thêm về khảo sát

    public DateTime Created_At { get; set; } = DateTime.Now; // Ngày giờ tạo khảo sát

    public DateTime Updated_At { get; set; } = DateTime.Now; // Ngày giờ cập nhật khảo sát gần nhất

}