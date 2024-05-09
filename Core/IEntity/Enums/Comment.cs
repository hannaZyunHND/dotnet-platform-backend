using System;
using System.Collections.Generic;
using System.Text;

namespace MI.Entity.Enums
{
    public enum CommentType : byte
    {
        [EnumDescription("Tất cả")]
        All = 0,
        [EnumDescription("Sản phẩm")]
        Product = 1,
        [EnumDescription("Bài viết")]
        Article = 2,
        [EnumDescription("Danh mục bài viết")]
        ArticleZone = 3,
        [EnumDescription("Danh mục sản phẩm")]
        ProductZone = 4,
        [EnumDescription("Liên hệ")]
        Contact = 5,
        [EnumDescription("Thu cũ đổi mới")]
        ProductOldRenewal = 6,
        [EnumDescription("Sửa chữa")]
        ProductComponent = 7
    }
    public enum StatusComment : byte
    {
        [EnumDescription("Tất cả")]
        All = 0,
        [EnumDescription("Mới")]
        New = 1,
        [EnumDescription("Xuất bản")]
        Public = 2,
        [EnumDescription("Loại")]
        Unpublic = 3,
    }
    public enum ObjectIdComment : byte
    {
        [EnumDescription("Thu cũ đổi mới")]
        ThuCu = 1,

        [EnumDescription("Sửa chữa")]
        SuaChua = 2
    }
}
