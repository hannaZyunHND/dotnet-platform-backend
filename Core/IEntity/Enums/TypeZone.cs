 using System;
using System.Collections.Generic;
using System.Text;

namespace MI.Entity.Enums
{
    public enum TypeZone : byte
    {
        [EnumDescription("Tất cả")]
        All = 0,
        [EnumDescription("Sản phẩm")]
        Product = 1,
        [EnumDescription("Bài viết")]
        Article = 2,
        [EnumDescription("Option sản phẩm")]
        ProductOptions = 3,
        //[EnumDescription("Tạp chí in")]
        [EnumDescription("Tag")]
        Tag = 4,
        //[EnumDescription("Điểm đến")]
        [EnumDescription("Điểm đến")]
        DiemDen = 5,
        //[EnumDescription("Tự viết")]
        //Visa = 6,
        [EnumDescription("Vùng hiển thị")]
        Region = 7,
        
        //[EnumDescription("Type Combo")] // Tat cac cac type ngoai tru san pham va vung hien thi
        //Combo = 8,
        //[EnumDescription("Bất động sản")] // Tat cac cac type ngoai tru san pham va vung hien thi
        //DuAn = 18,
        //[EnumDescription("Giải Pháp")] // Tat cac cac type ngoai tru san pham va vung hien thi
        //GiaiPhap = 19,
        //[EnumDescription("Dịch Vụ")] // Tat cac cac type ngoai tru san pham va vung hien thi
        //DichVu = 20,
        //[EnumDescription("Giới Thiệu")] // Tat cac cac type ngoai tru san pham va vung hien thi
        //GioiThieu = 21,
        //[EnumDescription("Sửa chữa")] // Tat cac cac type ngoai tru san pham va vung hien thi
        //SuaChua = 22,


    }
    public enum StatusZone : byte
    {
        [EnumDescription("Tất cả")]
        All = 0,
        [EnumDescription("Bình thường")]
        Normal = 1,
        [EnumDescription("Ẩn trên web")]
        HiddenWeb = 2,
        [EnumDescription("Trạng thái xóa")]
        Delete = 3,
        [EnumDescription("Trạng thái ẩn")]
        Invisible = 4,
    }

    public enum ZoneSearchType
    {
        [EnumDescription("Không tìm kiếm")]
        All = 0,
        [EnumDescription("Theo thương hiệu")]
        Manufacture = 1,
        [EnumDescription("Theo giá")]
        Price = 2
    }
}
