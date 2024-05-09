namespace PlatformWEBAPI.Services.ApiJoyTel.ViewModel
{
    public class ApiJoyTelViewModel
    {
    }

    public class OTARechargeResponse
    {
        public string message { get; set; }
        public int code { get; set; }
        public Data data { get; set; }
    }
    public class Data
    {
        public string rechargeCode { get; set; }
        public string orderTid { get; set; }
    }


    public class ESimSubmitResponse
    {
        public string message { get; set; }
        public int code { get; set; }
        public DataEsimSubmit data { get; set; }
    }

    public class DataEsimSubmit
    {
        public string orderTid { get; set; }
        public string orderCode { get; set; }
    }



    public class EsimOrderQueryResponse
    {
        public string message { get; set; }
        public int code { get; set; }
        public DataEsim data { get; set; }
    }

    public class DataEsim
    {
        public Itemlist[] itemList { get; set; }
        public string orderCode { get; set; }
        public string orderTid { get; set; }
        public string phone { get; set; }
        public string outboundCode { get; set; }
        public string receiveName { get; set; }
        public string email { get; set; }
        public int status { get; set; }
    }

    public class Itemlist
    {
        public string productCode { get; set; }
        public Snlist[] snList { get; set; }
        public int quantity { get; set; }
        public string productName { get; set; }
    }

    public class Snlist
    {
        public string snCode { get; set; }
        public string productExpireDate { get; set; }
        public string snPin { get; set; }
    }

    public class EsimRequestGetQrcodeResponse
    {
        public string code { get; set; }
        public string mesg { get; set; }
    }

    public class EsimQrCodeResponse
    {
        public string transId { get; set; }
        public string resultCode { get; set; }
        public string resultMesg { get; set; }
        public string finishTime { get; set; }
        public DataQrCode data { get; set; }
    }

    public class DataQrCode
    {
        public string coupon { get; set; }
        public int qrcodeType { get; set; }
        public string qrcode { get; set; }
        public string salePlanName { get; set; }
        public int salePlanDays { get; set; }
    }

}
