using System;
using System.Collections.Generic;
using System.Text;

namespace MI.Entity.Enums
{
    public enum OrderDetailStatus
    {
        TAO_MOI = 1,
        CHAP_NHAN_DICH_VU = 2,
        DA_SU_DUNG_DICH_VU = 3,
        TU_CHOI_DICH_VU = 4,
        YEU_CAU_HUY = 5,
        DA_HUY = 6


    }

    public enum OrderSupplierStatus
    {
        PENDING = 1,
        DA_GUI_EMAIL_YEU_CAU = 2,
        CHAP_NHAN_DICH_VU = 3,
        XAC_NHAN_HOAN_THANH = 4,
        TU_CHOI_DICH_VU = 5
    }

    public enum OrderChatSessionSenderType
    {
        SUPPLIER = 1,
        CUSTOMER = 2,
        ADMINSTRATOR = 3
    }

    public enum OrderChatSessionContentType
    {
        CONTENT = 1,
        IMAGE = 2
    }
}
