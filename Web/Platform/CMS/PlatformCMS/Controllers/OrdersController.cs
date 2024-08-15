using Dapper;
using MI.Bo.Bussiness;
using MI.Entity.Common;
using MI.Entity.Enums;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlatformCMS.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using System.Dynamic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MI.Dal.IDbContext;
using System.IO;
using Microsoft.AspNetCore.Hosting;
//using Org.BouncyCastle.Asn1.Ocsp;
using NPOI.SS.Formula.Functions;
using PlatformWEBAPI.Utility;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Web;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using PlatformCMS.Extensions;
using Nest;
using MimeKit;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using OfficeOpenXml;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrdersController : ControllerBase
    {
        OrdersBCL ordersBCL;
        OrderDetailBCL orderDetailBCL;
        ProductBCL productBCL;
        PromotionBCL promotionBCL;
        private readonly string _connectionString;
        private readonly IHostingEnvironment _enviroment;
        private readonly IConfiguration _configuration;
        public OrdersController(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            ordersBCL = new OrdersBCL();
            orderDetailBCL = new OrderDetailBCL();
            productBCL = new ProductBCL();
            promotionBCL = new PromotionBCL();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _enviroment = environment;
        }
        [HttpPost("Get")]
        public ResponsePaging Get(Utils.FilterOrders filter)
        {
            ResponsePaging responseData = new ResponsePaging();
            try
            {
                int total = 0;
                var data = ordersBCL.Get(filter, out total);
                var lstIdP = data.SelectMany(x => x.OrderDetail.Select(d => d.ProductId)).Distinct();
                var dicProduct = productBCL.FindAll(a => lstIdP.Contains(a.Id)).ToDictionary(x => x.Id, x => x);
                responseData.ListData = data.Select(x => new
                {
                    x.Id,
                    x.OrderCode,
                    CreatedDate = x.CreatedDate == null ? "" : x.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm"),
                    x.CreatedBy,
                    x.Status,
                    Type = MI.Entity.EnumHelper.GetDescription((MI.Entity.Enums.OrderType)x.OrderSourceType),
                    CustumerName = x.Customer.Name,
                    CustumerGender = x.Customer.Gender,
                    CustumerPhone = x.Customer.PhoneNumber,
                    CustumerNote = x.Customer.Note,
                    CustumerEmail = x.Customer.Email,
                    OrderDetail = x.OrderDetail.Where(y => y.Quantity > 0).Select(d => new
                    {
                        d.Id,
                        LogPrice = Utils.Utility.ConvertCurentce(d.LogPrice == null ? "0" : d.LogPrice.Value.ToString()),
                        Code = d.ProductId != null ? dicProduct.ContainsKey(d.ProductId ?? 0) ? dicProduct[d.ProductId.Value].Code : "" : "",
                        Name = d.ProductId != null ? dicProduct.ContainsKey(d.ProductId ?? 0) ? dicProduct[d.ProductId.Value].Name : "" : "",
                        d.ProductId,
                        d.Quantity,
                        d.Voucher,
                        VoucherPrice = d.VoucherPrice ?? 0,
                        d.VoucherMeta
                    }).ToList(),
                    //VoucherDetail = x.MetaData.Select(d => new VoucherTomTat()
                    //{
                    //    VoucherCode = Newtonsoft.Json.JsonConvert.DeserializeObject<MetaDataOrderController>(x.MetaData.Replace("\\\"", "\"").Replace("\"{", "{").Replace("}\"", "}")).MaVoucher,
                    //    VoucherGiaTri = Newtonsoft.Json.JsonConvert.DeserializeObject<MetaDataOrderController>(x.MetaData.Replace("\\\"", "\"").Replace("\"{", "{").Replace("}\"", "}")).GiaTriVoucher,
                    //    TongTien = Newtonsoft.Json.JsonConvert.DeserializeObject<MetaDataOrderController>(x.MetaData.Replace("\\\"", "\"").Replace("\"{", "{").Replace("}\"", "}")).TongTien,
                    //    SoBan = Newtonsoft.Json.JsonConvert.DeserializeObject<MetaDataOrderController>(x.MetaData.Replace("\\\"", "\"").Replace("\"{", "{").Replace("}\"", "}")).SoBan,
                    //    ThoiGianGiao = !string.IsNullOrEmpty(Newtonsoft.Json.JsonConvert.DeserializeObject<MetaDataOrderController>(x.MetaData.Replace("\\\"", "\"").Replace("\"{", "{").Replace("}\"", "}")).ThoiGianGiao) ? DateTime.ParseExact(Newtonsoft.Json.JsonConvert.DeserializeObject<MetaDataOrderController>(x.MetaData.Replace("\\\"", "\"").Replace("\"{", "{").Replace("}\"", "}")).ThoiGianGiao, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy HH:mm") : ""
                    //}).ToList().Take(1).SingleOrDefault(),
                }).ToList<object>();
                responseData.Total = total;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return responseData;
        }
        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            OrderVM result = new OrderVM();
            try
            {
                var ads = ordersBCL.FindById(x => x.Id == id, x => x.OrderDetail, x => x.Customer);
                if (ads != null)
                {
                    var orderDetail = orderDetailBCL.FindAll(x => x.OrderId == ads.Id && x.Quantity > 0, x => x.OrderPromotionDetail);
                    var product = productBCL.FindAll(x => orderDetail.Select(d => d.ProductId).Contains(x.Id)).ToDictionary(x => x.Id, x => x);
                    var number = orderDetail.Select(x => x.OrderPromotionDetail.Where(a => a.LogType == "discount").Sum(d => d.LogValue ?? 0));
                    var npromotionDetail = orderDetail.SelectMany(x => x.OrderPromotionDetail.Where(a => a.LogType == "discount")).ToList().GroupBy(x => x.OrderDetailId.Value).ToDictionary(x => x.Key, x => x.Sum(a => a.LogValue));


                    result.Code = ads.OrderCode;
                    result.CreateDate = ads.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm");
                    result.Price = Utils.Utility.ConvertCurentce(((int)ads.OrderDetail.Sum(x => x.LogPrice * x.Quantity) - (int)number.Sum(x => x) - (int)orderDetail.Sum(x => x.VoucherPrice ?? 0)).ToString());

                    //if (!string.IsNullOrEmpty(ads.InstallmentData) || ads.InstallmentData != "null")
                    //{
                    //    var installmentData = Newtonsoft.Json.JsonConvert.DeserializeObject<InstallmentData>(ads.InstallmentData);
                    //    if(installmentData != null)
                    //    {
                    //        result.InstallmentDetail = new InstallmentDetail()
                    //        {
                    //            NganHang = installmentData.NganHang,
                    //            SoThangTraGop = installmentData.SoThangTraGop,
                    //            TraTruoc = UIHelper.FormatNumber(installmentData.TraTruoc),
                    //            TraGopMoiThang = UIHelper.FormatNumber(installmentData.TraGopMoiThang),
                    //            TongSoTien = UIHelper.FormatNumber(installmentData.TongSoTien),
                    //            ChenhLech = UIHelper.FormatNumber(installmentData.ChenhLech),
                    //        };
                    //    }
                    //    else
                    //    {

                    //    }

                    //}
                    result.Note = ads.Note;
                    result.OdderDetail = orderDetail.Select(x => new ViewModel.OrderDetailVM
                    {

                        Code = product.ContainsKey(x.ProductId ?? 0) ? product[x.ProductId.Value].Code : "Default",
                        Name = product.ContainsKey(x.ProductId ?? 0) ? product[x.ProductId.Value].Name : "Default",
                        Number = x.Quantity == null ? 0 : (int)x.Quantity.Value,

                        Price = x.LogPrice.Value,
                        TotalPrice = x.LogPrice.Value * x.Quantity.Value,
                        //Price = Utility.ConvertCurentce(product.ContainsKey(x.ProductId ?? 0) ? (product[x.ProductId.Value].DiscountPrice > 0 ? product[x.ProductId.Value].DiscountPrice.ToString() : product[x.ProductId.Value].Price.ToString()) : "0".ToString()) + "đ",
                        //TotalPrice = Utility.ConvertCurentce(product.ContainsKey(x.ProductId ?? 0) ? (product[x.ProductId.Value].DiscountPrice > 0 ? (product[x.ProductId.Value].DiscountPrice * (int)x.Quantity - (int)(npromotionDetail.ContainsKey(x.Id) ? npromotionDetail[x.Id] * x.Quantity : 0)).ToString() : (product[x.ProductId.Value].Price * (int)x.Quantity - (int)(npromotionDetail.ContainsKey(x.Id) ? npromotionDetail[x.Id] * x.Quantity : 0)).ToString()) : "0".ToString()) + "đ",
                        VoucherPrice = x.VoucherType == 1 ? x.VoucherPrice + "%" : Utility.ConvertCurentce((x.VoucherPrice ?? 0).ToString()) + "đ",
                        Voucher = x.Voucher,
                        VoucherMeta = x.VoucherMeta,
                        strPromotion = BuildPromotion(x.OrderPromotionDetail.ToList()),
                        strVoucher = x.VoucherMeta,
                        //OderPrice = BuildOrderPrice(orderDetail, product, npromotionDetail),
                        OderPrice = x.LogPrice.Value * x.Quantity.Value,
                        ActiveStatus = x.ActiveStatus,
                        Url = product.ContainsKey(x.ProductId ?? 0) ? (!product[x.ProductId.Value].Url.StartsWith("http") ? Utils.Settings.AppSettings.BaseDomain + "/" + product[x.ProductId.Value].Url + ".html" : product[x.ProductId.Value].Url) : "Default",
                    }).ToList();

                    result.Price = UIHelper.FormatNumber(result.OdderDetail.Sum(r => r.TotalPrice));
                    ads.Customer.Orders = null;
                    result.Customer = ads.Customer;
                    result.Address = ads.Address;
                    result.Src = MI.Entity.EnumHelper.GetDescription((MI.Entity.Enums.OrderType)ads.OrderSourceType);
                    result.Status = StatusOrders.GetStatusName(ads.Status);
                }

            }
            catch (Exception ex)
            {

            }
            return Ok(result);
        }
        private string BuildOrderPrice(List<OrderDetail> ListorderDetails, Dictionary<int, Product> product, Dictionary<int, decimal?> npromotionDetail)
        {
            var rsult = string.Empty;
            double? total = 0;
            try
            {
                foreach (var item in ListorderDetails)
                {
                    total += (product[item.ProductId.Value].DiscountPrice > 0 ? product[item.ProductId.Value].DiscountPrice * (int)item.Quantity : product[item.ProductId.Value].Price * (int)item.Quantity) - (int)(npromotionDetail.ContainsKey(item.Id) ? npromotionDetail[item.Id] * item.Quantity : 0);
                }
                if (ListorderDetails[0].VoucherType == 1)
                {
                    rsult = Utility.ConvertCurentce((total - (total / 100 * ListorderDetails[0].VoucherPrice)).ToString()) + "đ";

                }
                else
                {
                    rsult = Utility.ConvertCurentce((total - ListorderDetails[0].VoucherType).ToString()) + "đ";
                }
                return rsult;
            }

            catch
            {
            }
            return rsult;
        }
        private string BuildTotalPrice(OrderDetail orderDetail, List<OrderPromotionDetail> lstPromotion, Dictionary<int, decimal?> npromotionDetail)
        {
            string result = "";
            try
            {
                foreach (var item in lstPromotion)
                {
                    if (item.LogType.Trim() == "discount-percent")
                    {
                        int amountDiscount = (((((int)orderDetail.LogPrice * (int)item.LogValue) / 100) * (int)orderDetail.Quantity) - (int)(npromotionDetail.ContainsKey(orderDetail.Id) ? npromotionDetail[orderDetail.Id] : 0));
                        int totalAmount = (int)orderDetail.LogPrice * (int)orderDetail.Quantity;
                        result = Utility.ConvertCurentce((totalAmount - amountDiscount).ToString()) + "đ";
                    }
                    else
                    {
                        result = Utility.ConvertCurentce((((int)orderDetail.LogPrice * (int)orderDetail.Quantity) - (int)(npromotionDetail.ContainsKey(orderDetail.Id) ? npromotionDetail[orderDetail.Id] : 0)).ToString()) + "đ";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        private string BuildPromotion(List<OrderPromotionDetail> lstPromotion)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in lstPromotion)
            {
                sb.Append("<p>" + item.LogName + $": {Utility.ConvertCurentce(item.LogValue.ToString())} </p>");
            }
            return sb.ToString();
        }
        private string BuildVoucher(string voucher, double price, string meta)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(voucher))
            {
                sb.Append($"Sử dụng: {voucher}: {Utils.Utility.ConvertCurentce(price.ToString())} đ<br/>");
                sb.Append($"Chi tiết: {meta}");
            }
            return sb.ToString();
        }

        [HttpPut("UpdateOrderStatus")]
        public ResponseData UpdateOrderStatus([FromBody] Orders obj)
        {
            ResponseData responseData = new ResponseData();
            try
            {
                bool ads = ordersBCL.UpdateTrangThai(obj);
                if (ads)
                {
                    responseData.Success = true;
                    responseData.Message = Utility.SuccessMessage;
                }
                else
                {
                    responseData.Success = false;
                    responseData.Message = Utility.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                responseData.Message = Utility.SuccessMessage;
            }
            return responseData;
        }

        [HttpPost]
        [Route("GetListOrderV2")]
        public async Task<IActionResult> GetListOrderV2(RequesetGetListOrderV2 request)
        {
            var currentUserId = User.GetUserId();
            var roles = User.GetRoleName();
            var userEmail = request.emailSupplier;



            using (IDbContext context = new IDbContext())
            {
                var user = await context.AspNetUsers.FindAsync(currentUserId);
                if (user != null)
                {
                    if (roles.Contains("Supplier"))
                    {
                        userEmail = user.Email;
                    }
                }
            }
            var orders = new List<ResponseGetListOrderV2>();
            var total = 0;
            var statistics = new ResponseGetOrderDetailStatic();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@keyword", request.keyword);
                    parameters.Add("@startDate", request.startDate);
                    parameters.Add("@endDate", request.endDate);
                    parameters.Add("@activeStatus", request.activeStatus);
                    parameters.Add("@supplierStatus", request.supplierStatus);
                    parameters.Add("@lang_code", "vi-VN");
                    parameters.Add("@emailSupplier", userEmail);
                    parameters.Add("@index", request.index);
                    parameters.Add("@size", request.size);
                    parameters.Add("@total", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    orders = connection.Query<ResponseGetListOrderV2>("usp_CMS_GetOrderDetailFullInfomations", parameters, commandType: CommandType.StoredProcedure).ToList();
                    total = parameters.Get<int>("@total");


                }

                catch (Exception e)
                {
                }

            }
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@keyword", request.keyword);
                    parameters.Add("@startDate", request.startDate);
                    parameters.Add("@endDate", request.endDate);
                    parameters.Add("@activeStatus", request.activeStatus);
                    parameters.Add("@supplierStatus", request.supplierStatus);
                    parameters.Add("@lang_code", "vi-VN");
                    parameters.Add("@emailSupplier", userEmail);
                    statistics = connection.QueryFirst<ResponseGetOrderDetailStatic>("usp_CMS_GetOrderDetailStatic", parameters, commandType: CommandType.StoredProcedure);
                    total = parameters.Get<int>("@total");
                }

                catch (Exception e)
                {
                }

            }


            dynamic response = new ExpandoObject();
            response.orders = orders;
            response.total = total;
            response.statistics = statistics;
            return Ok(response);
        }

        [HttpPost]
        [Route("ExportExcelListOrderVersionOffice")]
        public async Task<IActionResult> ExportExcelListOrderVersionOffice(RequesetGetListOrderV2 request)
        {
            var currentUserId = User.GetUserId();
            var roles = User.GetRoleName();
            var userEmail = request.emailSupplier;



            using (IDbContext context = new IDbContext())
            {
                var user = await context.AspNetUsers.FindAsync(currentUserId);
                if (user != null)
                {
                    if (roles.Contains("Supplier"))
                    {
                        userEmail = user.Email;
                    }
                }
            }
            request.index = 1;
            request.size = 10000000;
            var orders = new List<ResponseGetListOrderV2>();
            var total = 0;
            var statistics = new ResponseGetOrderDetailStatic();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@keyword", request.keyword);
                    parameters.Add("@startDate", request.startDate);
                    parameters.Add("@endDate", request.endDate);
                    parameters.Add("@activeStatus", request.activeStatus);
                    parameters.Add("@supplierStatus", request.supplierStatus);
                    parameters.Add("@lang_code", "vi-VN");
                    parameters.Add("@emailSupplier", userEmail);
                    parameters.Add("@index", request.index);
                    parameters.Add("@size", request.size);
                    parameters.Add("@total", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    orders = connection.Query<ResponseGetListOrderV2>("usp_CMS_GetOrderDetailFullInfomations", parameters, commandType: CommandType.StoredProcedure).ToList();
                    total = parameters.Get<int>("@total");


                }

                catch (Exception e)
                {
                }

            }
            //Xuat ra file excel sau do gui binary ve vue de tu vue sinh ra file va tai xuong
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Finance Data");

                // Assuming response is a list of some kind of data objects
                // Add headers (example: assuming response is a list of some objects with properties Id, Name, and Amount)
                worksheet.Cells[1, 1].Value = "No";
                worksheet.Cells[1, 2].Value = "Mã đơn hàng";
                worksheet.Cells[1, 3].Value = "Tên khách hàng";
                worksheet.Cells[1, 4].Value = "Số điện thoại";
                worksheet.Cells[1, 5].Value = "Email";
                worksheet.Cells[1, 6].Value = "Tên sản phẩm";
                worksheet.Cells[1, 7].Value = "Tên package";
                worksheet.Cells[1, 8].Value = "Tên đối tác";
                worksheet.Cells[1, 9].Value = "Ngày đặt DV";
                worksheet.Cells[1, 10].Value = "Ngày sử dụng DV";
                worksheet.Cells[1, 11].Value = "Số lượng (Số người lớn)";
                worksheet.Cells[1, 12].Value = "Số trẻ em";
                worksheet.Cells[1, 13].Value = "Giá bán";
                worksheet.Cells[1, 14].Value = "Giá vốn";
                worksheet.Cells[1, 15].Value = "Trạng thái";
                worksheet.Cells[1, 16].Value = "OnepayRef";

                // Add data
                for (int i = 0; i < orders.Count; i++)
                {
                    var item = orders[i];
                    worksheet.Cells[i + 2, 1].Value = i + 1;
                    worksheet.Cells[i + 2, 2].Value = item.OrderCode;
                    worksheet.Cells[i + 2, 3].Value = item.FullName;
                    worksheet.Cells[i + 2, 4].Value = item.PhoneNumber;
                    worksheet.Cells[i + 2, 5].Value = item.Email;
                    worksheet.Cells[i + 2, 6].Value = item.ProductParentTitle;
                    worksheet.Cells[i + 2, 7].Value = item.ProductChildTitle;
                    worksheet.Cells[i + 2, 8].Value = item.SupplierFullName;
                    worksheet.Cells[i + 2, 9].Value = item.CreatedDate.ToString("dd/MM/yyyy");
                    worksheet.Cells[i + 2, 10].Value = item.pickingDate.ToString("dd/MM/yyyy");
                    worksheet.Cells[i + 2, 11].Value = item.quantityAldut;
                    worksheet.Cells[i + 2, 12].Value = item.QuantityChildren;
                    worksheet.Cells[i + 2, 13].Value = item.LogPrice;
                    worksheet.Cells[i + 2, 14].Value = item.LogPriceGross;
                    worksheet.Cells[i + 2, 15].Value = item.ActiveStatus;
                    worksheet.Cells[i + 2, 16].Value = item.OnepayRef;
                }
                // Tự động điều chỉnh độ rộng của các cột
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                // Save the package to a stream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                // Return the Excel file as a binary file
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                // Convert the stream to a base64 string
                var base64String = Convert.ToBase64String(stream.ToArray());

                // Return the base64 string
                return Ok(base64String);
                //var fileName = "FinanceData.xlsx";
                //Response.Headers["Content-Disposition"] = $"attachment; filename={fileName}";
                //return File(stream, contentType, fileName);
            }

        }

        [HttpPost]
        [Route("ExportExcelListOrderVersionSupplier")]
        public async Task<IActionResult> ExportExcelListOrderVersionSupplier(RequesetGetListOrderV2 request)
        {
            var currentUserId = User.GetUserId();
            var roles = User.GetRoleName();
            var userEmail = request.emailSupplier;



            using (IDbContext context = new IDbContext())
            {
                var user = await context.AspNetUsers.FindAsync(currentUserId);
                if (user != null)
                {
                    if (roles.Contains("Supplier"))
                    {
                        userEmail = user.Email;
                    }
                }
            }
            request.index = 1;
            request.size = 10000000;
            var orders = new List<ResponseGetListOrderV2>();
            var total = 0;
            var statistics = new ResponseGetOrderDetailStatic();
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@keyword", request.keyword);
                    parameters.Add("@startDate", request.startDate);
                    parameters.Add("@endDate", request.endDate);
                    parameters.Add("@activeStatus", request.activeStatus);
                    parameters.Add("@supplierStatus", request.supplierStatus);
                    parameters.Add("@lang_code", "vi-VN");
                    parameters.Add("@emailSupplier", userEmail);
                    parameters.Add("@index", request.index);
                    parameters.Add("@size", request.size);
                    parameters.Add("@total", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    orders = connection.Query<ResponseGetListOrderV2>("usp_CMS_GetOrderDetailFullInfomations", parameters, commandType: CommandType.StoredProcedure).ToList();
                    total = parameters.Get<int>("@total");


                }

                catch (Exception e)
                {
                }

            }
            //Xuat ra file excel sau do gui binary ve vue de tu vue sinh ra file va tai xuong
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Finance Data");

                // Assuming response is a list of some kind of data objects
                // Add headers (example: assuming response is a list of some objects with properties Id, Name, and Amount)
                worksheet.Cells[1, 1].Value = "No";
                worksheet.Cells[1, 2].Value = "Mã đơn hàng";
                worksheet.Cells[1, 3].Value = "Tên khách hàng";
                worksheet.Cells[1, 4].Value = "Số điện thoại";
                worksheet.Cells[1, 5].Value = "Tên sản phẩm";
                worksheet.Cells[1, 6].Value = "Tên package";
                worksheet.Cells[1, 7].Value = "Tên đối tác";
                worksheet.Cells[1, 8].Value = "Ngày đặt DV";
                worksheet.Cells[1, 9].Value = "Ngày sử dụng DV";
                worksheet.Cells[1, 10].Value = "Số lượng (Số người lớn)";
                worksheet.Cells[1, 11].Value = "Số trẻ em";
                worksheet.Cells[1, 12].Value = "Giá bán";
                worksheet.Cells[1, 13].Value = "Trạng thái";


                // Add data
                for (int i = 0; i < orders.Count; i++)
                {
                    var item = orders[i];
                    worksheet.Cells[i + 2, 1].Value = i + 1;
                    worksheet.Cells[i + 2, 2].Value = item.OrderCode;
                    worksheet.Cells[i + 2, 3].Value = item.FullName;
                    worksheet.Cells[i + 2, 4].Value = item.PhoneNumber;
                    worksheet.Cells[i + 2, 5].Value = item.ProductParentTitle;
                    worksheet.Cells[i + 2, 6].Value = item.ProductChildTitle;
                    worksheet.Cells[i + 2, 7].Value = item.SupplierFullName;
                    worksheet.Cells[i + 2, 8].Value = item.CreatedDate.ToString("dd/MM/yyyy");
                    worksheet.Cells[i + 2, 9].Value = item.pickingDate.ToString("dd/MM/yyyy");
                    worksheet.Cells[i + 2, 10].Value = item.quantityAldut;
                    worksheet.Cells[i + 2, 11].Value = item.QuantityChildren;
                    worksheet.Cells[i + 2, 12].Value = item.LogPriceGross;
                    worksheet.Cells[i + 2, 13].Value = item.ActiveStatus;
                }
                // Tự động điều chỉnh độ rộng của các cột
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                // Save the package to a stream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                // Return the Excel file as a binary file
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                // Convert the stream to a base64 string
                var base64String = Convert.ToBase64String(stream.ToArray());

                // Return the base64 string
                return Ok(base64String);
                //var fileName = "FinanceData.xlsx";
                //Response.Headers["Content-Disposition"] = $"attachment; filename={fileName}";
                //return File(stream, contentType, fileName);
            }

        }

        [HttpPost]
        [Route("UpdateActiveStatusForOrderDetail")]
        public async Task<IActionResult> UpdateActiveStatusForOrderDetail([FromForm] RequestUpdateActiveStatusForOrderDetail request)
        {
            if (request != null)
            {
                using (IDbContext context = new IDbContext())
                {
                    //Kiem tra xem co orderDetail khong
                    var orderDetail = context.OrderDetail.Where(r => r.Id == request.orderDetailId).FirstOrDefault();
                    if (orderDetail != null)
                    {
                        var fileUploaded = new List<string>();
                        if (request.files != null)
                        {
                            foreach (var file in request.files)
                            {
                                //Lấy ra extension file
                                var fileExtension = Path.GetExtension(file.FileName);
                                //.jpg,.jpeg,.png,.xls,.xlsx,.pdf,.doc,.docx,.zip,.rar
                                var acceptedExtension = new List<string>() { ".jpg", ".jpeg", ".png", ".xls", ".xlsx", ".pdf", ".doc", ".docx", ".zip", ".rar" };
                                if (acceptedExtension.Contains(fileExtension))
                                {
                                    // Tạo đường dẫn lưu file
                                    var fileName = $"E-TICKET-{DateTime.Now.Ticks}{fileExtension}";

                                    var wwwroot = _enviroment.WebRootPath;
                                    var folderDir = Path.Combine(wwwroot, "upload-e-ticket", orderDetail.Id.ToString());
                                    if (!Directory.Exists(folderDir))
                                    {
                                        Directory.CreateDirectory(folderDir);
                                    }
                                    var path = Path.Combine(folderDir, fileName);
                                    //Lưu file vào thư mục wwwroot/upload-e-ticket/{orderDetail.Id}/{E-TICKET-(CurrentTick).(phần extension file)}
                                    // Lưu file vào thư mục
                                    using (var stream = new FileStream(path, FileMode.Create))
                                    {
                                        await file.CopyToAsync(stream);
                                    }

                                    //Trả path file gồm `/{orderDetail.Id}/{E-TICKET-(CurrentTick).(phần extension file)` vào fileUpload
                                    var relativePath = $"{fileName}";
                                    fileUploaded.Add(relativePath);
                                }

                            }
                        }


                        //Cap nhat trang thai cua orderDetail va fileUploaded
                        orderDetail.ActiveStatus = request.activeStatus;
                        if (!string.IsNullOrEmpty(request.cancelResponse) && request.activeStatus.Equals("TU_CHOI_DICH_VU"))
                        {
                            orderDetail.CancelResponse = request.cancelResponse;
                        }
                        if (fileUploaded.Count > 0)
                        {
                            orderDetail.FileETicket = string.Join(",", fileUploaded);
                        }

                        context.OrderDetail.Update(orderDetail);
                        await context.SaveChangesAsync();

                        await this.SendEmailToCustomerWithStatus(orderDetail.Id);
                        return Ok("CAP NHAT THANH CONG");


                    }
                }

            }
            return BadRequest("ERROR!");
        }

        [HttpPost]
        [Route("SendETicket")]
        [AllowAnonymous]
        public async Task<IActionResult> SendETicket(RequestSendETicket request)
        {
            if (request != null)
            {
                if (request.orderDetailId > 0)
                {
                    await SendEmailToCustomerWithStatus(request.orderDetailId);

                    return Ok("EMAIL WAS SENT");
                }
            }
            return BadRequest("ERROR");

        }

        [HttpGet]
        [Route("GetListOrderFeedback")]
        public async Task<IActionResult> GetListOrderFeedback()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var orders = connection.Query<OrderFeedbackDetail>("usp_CMS_ReviewFeedback", commandType: CommandType.StoredProcedure).ToList();

                    return Ok(orders);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("UpdateReviewFeedback")]
        public async Task<IActionResult> UpdateReviewFeedback (UpdateFeedbackDetail request)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    if(connection.State == ConnectionState.Closed)
                    {
                        connection.Close();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@orderDetailFeedbackId", request.OrderDetailFeedbackId);
                    parameters.Add("@IsConfirm", request.IsConfirm);
                    
                    var orders = connection.Query<OrderFeedbackDetail>("usp_CMS_UpdateReviewFeedback", parameters, commandType: CommandType.StoredProcedure).ToList();
                    return Ok(orders);
                }
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task SendEmailToCustomerWithStatus(int orderDetailId)
        {
            var orderResponse = new ResponseGetListOrderV2();
            var bannerCode = "";
            var languageBanner = "";
            var templateEmailName = "";
            //usp_CMS_GetOrderDetailFullInfomationsById
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@orderDetailId", orderDetailId);

                    orderResponse = connection.QueryFirst<ResponseGetListOrderV2>("usp_CMS_GetOrderDetailFullInfomationsById", parameters, commandType: CommandType.StoredProcedure);



                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            if (orderResponse.OrderDetailId > 0)
            {
                var detail = orderResponse;
                languageBanner = orderResponse.DefaultLanguage;
                var smtpServer = _configuration["EmailSender:Host"];
                int smtpPort = int.Parse(_configuration["EmailSender:Port"]);
                var smtpUser = _configuration["EmailSender:BookingService:Email"]; // cs@joytime.vn
                var smtpPass = _configuration["EmailSender:BookingService:Password"]; //D2A9HnvGMJYW3BeKQw5f4F
                if (orderResponse.ActiveStatus.Equals("CHAP_NHAN_DICH_VU"))
                {
                    //GUI EMAIL CHAP NHAN DICH VU
                    bannerCode = "MAIL_CULTURE_SEND_E_TICKET";
                    templateEmailName = "mail-e-ticket.html";
                    var bannerCulture = GetBannerAdsByCode(languageBanner, bannerCode);

                    if (bannerCulture != null)
                    {
                        var wwwroot = _enviroment.WebRootPath;
                        var templateFullPath = Path.Combine(wwwroot, "mail-templates", templateEmailName);
                        if (System.IO.File.Exists(templateFullPath))
                        {
                            //Ve gioi ve bien gi thi ve vao day
                            Dictionary<string, string> mailHooks = new Dictionary<string, string>();
                            foreach (var b in bannerCulture)
                            {
                                if (!string.IsNullOrEmpty(b.Title))
                                {
                                    mailHooks.Add($"[{b.Title}]", CMSHelper.GetCultureText(bannerCulture, b.Title));
                                }
                            }
                            mailHooks.Add("[CUSTOMER_FULLNAME]", detail.FullName);
                            mailHooks.Add("[DATA_FULL_NAME]", detail.FullName);
                            mailHooks.Add("[DATA_MA_DON_HANG]", detail.OrderCode);
                            mailHooks.Add("[DATA_NGAY_SU_DUNG]", detail.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            var metadata = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderDetailMetaData>(detail.MetaData);
                            mailHooks.Add("[DATA_NGAY_DAT_DICH_VU]", metadata.choosenDate);
                            mailHooks.Add("[DATA_TEN_PACKAGE]", detail.ProductChildTitle);
                            mailHooks.Add("[DATA_TEN_FULL_OPTION]", detail.ZoneTitles);
                            mailHooks.Add("[MAIL_NOI_DUNG_SAN_PHAM_AVATAR]", UIHelper.StoreFilePath(detail.ProductParentAvatar));

                            var MAIL_NOI_DUNG_SO_LUONG = new List<string>();
                            var MAIL_NOI_DUNG_SO_LUONG_NGUOI_LON = mailHooks.GetValueOrDefault("[MAIL_NOI_DUNG_SO_LUONG_NGUOI_LON]");
                            var MAIL_NOI_DUNG_SO_LUONG_TRE_EM = mailHooks.GetValueOrDefault("[MAIL_NOI_DUNG_SO_LUONG_TRE_EM]");
                            if (detail.quantityAldut > 0)
                            {
                                MAIL_NOI_DUNG_SO_LUONG.Add($"{detail.quantityAldut} x {MAIL_NOI_DUNG_SO_LUONG_NGUOI_LON}");
                            }
                            if (detail.QuantityChildren > 0)
                            {
                                MAIL_NOI_DUNG_SO_LUONG.Add($"{detail.QuantityChildren} x {MAIL_NOI_DUNG_SO_LUONG_TRE_EM}");
                            }
                            mailHooks.Add("[DATA_SO_LUONG]", string.Join(" - ", MAIL_NOI_DUNG_SO_LUONG));
                            mailHooks.Add("[DATA_GIA_TIEN]", $"VND {UIHelper.FormatNumber(detail.LogPrice)}");
                            var templateString = ReadTemplateFromFile(templateFullPath);
                            var outputHtml = ReplacePlaceholders(templateString, mailHooks);
                            if (!string.IsNullOrEmpty(outputHtml))
                            {

                                var title = mailHooks.GetValueOrDefault("[MAIL_TITLE]");
                                var subject = "";
                                if (!string.IsNullOrEmpty(title))
                                {
                                    subject = ConvertToCorrectEncoding(title);
                                }

                                var body = outputHtml;
                                var toEmail = detail.Email;

                                var message = new MimeMessage();
                                message.From.Add(new MailboxAddress(smtpUser, smtpUser));
                                message.To.Add(new MailboxAddress(toEmail, toEmail));
                                message.Subject = subject;

                                var bodyBuilder = new BodyBuilder { HtmlBody = body };


                                // Đính kèm file


                                //đính kèm file ở đây
                                var attachmentFilePath = new List<string>();
                                if (!string.IsNullOrEmpty(detail.FileETicket))
                                {
                                    var filePathSplited = detail.FileETicket.Split(",");
                                    foreach (var f in filePathSplited)
                                    {
                                        var fFullPath = Path.Combine(_enviroment.WebRootPath, "upload-e-ticket", detail.OrderDetailId.ToString(), f);
                                        if (System.IO.File.Exists(fFullPath))
                                        {
                                            attachmentFilePath.Add(fFullPath);
                                        }
                                    }
                                }
                                // Đính kèm các file có địa chỉ ở attachmentFilePath
                                foreach (var filePath in attachmentFilePath)
                                {
                                    bodyBuilder.Attachments.Add(filePath);

                                }
                                message.Body = bodyBuilder.ToMessageBody();
                                using (var client = new MailKit.Net.Smtp.SmtpClient())
                                {
                                    try
                                    {
                                        client.Connect(smtpServer, smtpPort, true);
                                        client.Authenticate(smtpUser, smtpPass);
                                        client.Send(message);
                                        client.Disconnect(true);
                                    }
                                    catch (Exception ex)
                                    {
                                        // Handle exception (e.g., log the error)
                                        Console.WriteLine($"ERROR: {ex.Message}");

                                    }
                                }
                            }
                        }
                    }

                }
                if (orderResponse.ActiveStatus.Equals("TU_CHOI_DICH_VU"))
                {
                    //GUI EMAIL TU CHOI DICH VU
                    bannerCode = "MAIL_CULTURE_SEND_TU_CHOI_DON_HANG";
                    templateEmailName = "mail-tu-choi-dich-vu.html";
                    var bannerCulture = GetBannerAdsByCode(languageBanner, bannerCode);

                    if (bannerCulture != null)
                    {
                        var wwwroot = _enviroment.WebRootPath;
                        var templateFullPath = Path.Combine(wwwroot, "mail-templates", templateEmailName);
                        if (System.IO.File.Exists(templateFullPath))
                        {
                            //Ve gioi ve bien gi thi ve vao day
                            Dictionary<string, string> mailHooks = new Dictionary<string, string>();
                            foreach (var b in bannerCulture)
                            {
                                if (!string.IsNullOrEmpty(b.Title))
                                {
                                    mailHooks.Add($"[{b.Title}]", CMSHelper.GetCultureText(bannerCulture, b.Title));
                                }
                            }
                            mailHooks.Add("[CUSTOMER_FULLNAME]", detail.FullName);
                            mailHooks.Add("[DATA_FULL_NAME]", detail.FullName);
                            mailHooks.Add("[DATA_MA_DON_HANG]", detail.OrderCode);
                            mailHooks.Add("[DATA_NGAY_SU_DUNG]", detail.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            mailHooks.Add("[DATA_LY_DO_TU_CHOI]", detail.CancelResponse);
                            mailHooks.Add("[DATA_TEN_PACKAGE]", detail.ProductChildTitle);
                            mailHooks.Add("[DATA_TEN_FULL_OPTION]", detail.ZoneTitles);
                            mailHooks.Add("[MAIL_NOI_DUNG_SAN_PHAM_AVATAR]", UIHelper.StoreFilePath(detail.ProductParentAvatar));

                            var MAIL_NOI_DUNG_SO_LUONG = new List<string>();
                            var MAIL_NOI_DUNG_SO_LUONG_NGUOI_LON = mailHooks.GetValueOrDefault("[MAIL_NOI_DUNG_SO_LUONG_NGUOI_LON]");
                            var MAIL_NOI_DUNG_SO_LUONG_TRE_EM = mailHooks.GetValueOrDefault("[MAIL_NOI_DUNG_SO_LUONG_TRE_EM]");
                            if (detail.quantityAldut > 0)
                            {
                                MAIL_NOI_DUNG_SO_LUONG.Add($"{detail.quantityAldut} x {MAIL_NOI_DUNG_SO_LUONG_NGUOI_LON}");
                            }
                            if (detail.QuantityChildren > 0)
                            {
                                MAIL_NOI_DUNG_SO_LUONG.Add($"{detail.QuantityChildren} x {MAIL_NOI_DUNG_SO_LUONG_TRE_EM}");
                            }
                            mailHooks.Add("[DATA_SO_LUONG]", string.Join(" - ", MAIL_NOI_DUNG_SO_LUONG));
                            mailHooks.Add("[DATA_GIA_TIEN]", $"VND {UIHelper.FormatNumber(detail.LogPrice)}");
                            var templateString = ReadTemplateFromFile(templateFullPath);
                            var outputHtml = ReplacePlaceholders(templateString, mailHooks);
                            if (!string.IsNullOrEmpty(outputHtml))
                            {

                                var title = mailHooks.GetValueOrDefault("[MAIL_TITLE]");
                                var subject = "";
                                if (!string.IsNullOrEmpty(title))
                                {
                                    subject = ConvertToCorrectEncoding(title);
                                }

                                var body = outputHtml;
                                var toEmail = detail.Email;

                                var message = new MimeMessage();
                                message.From.Add(new MailboxAddress(smtpUser, smtpUser));
                                message.To.Add(new MailboxAddress(toEmail, toEmail));
                                message.Subject = subject;

                                var bodyBuilder = new BodyBuilder { HtmlBody = body };
                                message.Body = bodyBuilder.ToMessageBody();
                                using (var client = new MailKit.Net.Smtp.SmtpClient())
                                {
                                    try
                                    {
                                        client.Connect(smtpServer, smtpPort, true);
                                        client.Authenticate(smtpUser, smtpPass);
                                        client.Send(message);
                                        client.Disconnect(true);
                                    }
                                    catch (Exception ex)
                                    {
                                        // Handle exception (e.g., log the error)
                                        Console.WriteLine($"ERROR: {ex.Message}");

                                    }
                                }
                            }
                        }
                    }

                }
                if (orderResponse.ActiveStatus.Equals("DA_HUY"))
                {
                    // GUI EMAIL DA HUY DICH VU
                    //GUI EMAIL TU CHOI DICH VU
                    bannerCode = "MAIL_CULTURE_SEND_HUY_DICH_VU";
                    templateEmailName = "mail-huy-dich-vu.html";
                    var bannerCulture = GetBannerAdsByCode(languageBanner, bannerCode);

                    if (bannerCulture != null)
                    {
                        var wwwroot = _enviroment.WebRootPath;
                        var templateFullPath = Path.Combine(wwwroot, "mail-templates", templateEmailName);
                        if (System.IO.File.Exists(templateFullPath))
                        {
                            //Ve gioi ve bien gi thi ve vao day
                            Dictionary<string, string> mailHooks = new Dictionary<string, string>();
                            foreach (var b in bannerCulture)
                            {
                                if (!string.IsNullOrEmpty(b.Title))
                                {
                                    mailHooks.Add($"[{b.Title}]", CMSHelper.GetCultureText(bannerCulture, b.Title));
                                }
                            }
                            mailHooks.Add("[CUSTOMER_FULLNAME]", detail.FullName);
                            mailHooks.Add("[DATA_FULL_NAME]", detail.FullName);
                            mailHooks.Add("[DATA_MA_DON_HANG]", detail.OrderCode);
                            mailHooks.Add("[DATA_NGAY_SU_DUNG]", detail.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss"));
                            mailHooks.Add("[DATA_LY_DO_TU_CHOI]", detail.CancelResponse);
                            mailHooks.Add("[DATA_TEN_PACKAGE]", detail.ProductChildTitle);
                            mailHooks.Add("[DATA_TEN_FULL_OPTION]", detail.ZoneTitles);
                            mailHooks.Add("[MAIL_NOI_DUNG_SAN_PHAM_AVATAR]", UIHelper.StoreFilePath(detail.ProductParentAvatar));

                            var MAIL_NOI_DUNG_SO_LUONG = new List<string>();
                            var MAIL_NOI_DUNG_SO_LUONG_NGUOI_LON = mailHooks.GetValueOrDefault("[MAIL_NOI_DUNG_SO_LUONG_NGUOI_LON]");
                            var MAIL_NOI_DUNG_SO_LUONG_TRE_EM = mailHooks.GetValueOrDefault("[MAIL_NOI_DUNG_SO_LUONG_TRE_EM]");
                            if (detail.quantityAldut > 0)
                            {
                                MAIL_NOI_DUNG_SO_LUONG.Add($"{detail.quantityAldut} x {MAIL_NOI_DUNG_SO_LUONG_NGUOI_LON}");
                            }
                            if (detail.QuantityChildren > 0)
                            {
                                MAIL_NOI_DUNG_SO_LUONG.Add($"{detail.QuantityChildren} x {MAIL_NOI_DUNG_SO_LUONG_TRE_EM}");
                            }
                            mailHooks.Add("[DATA_SO_LUONG]", string.Join(" - ", MAIL_NOI_DUNG_SO_LUONG));
                            mailHooks.Add("[DATA_GIA_TIEN]", $"VND {UIHelper.FormatNumber(detail.LogPrice)}");
                            var templateString = ReadTemplateFromFile(templateFullPath);
                            var outputHtml = ReplacePlaceholders(templateString, mailHooks);
                            if (!string.IsNullOrEmpty(outputHtml))
                            {

                                var title = mailHooks.GetValueOrDefault("[MAIL_TITLE]");
                                var subject = "";
                                if (!string.IsNullOrEmpty(title))
                                {
                                    subject = ConvertToCorrectEncoding(title);
                                }

                                var body = outputHtml;
                                var toEmail = detail.Email;


                                var message = new MimeMessage();
                                message.From.Add(new MailboxAddress(smtpUser, smtpUser));
                                message.To.Add(new MailboxAddress(toEmail, toEmail));
                                message.Subject = subject;

                                var bodyBuilder = new BodyBuilder { HtmlBody = body };
                                message.Body = bodyBuilder.ToMessageBody();
                                using (var client = new MailKit.Net.Smtp.SmtpClient())
                                {
                                    try
                                    {
                                        client.Connect(smtpServer, smtpPort, true);
                                        client.Authenticate(smtpUser, smtpPass);
                                        client.Send(message);
                                        client.Disconnect(true);
                                    }
                                    catch (Exception ex)
                                    {
                                        // Handle exception (e.g., log the error)
                                        Console.WriteLine($"ERROR: {ex.Message}");

                                    }
                                }
                            }
                        }
                    }
                }
            }

        }
        private string ConvertToCorrectEncoding(string input)
        {
            // Giải mã các ký tự HTML entities thành chuỗi Unicode
            string decodedString = HttpUtility.HtmlDecode(input);

            return decodedString;

        }
        private string ReadTemplateFromFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                throw new FileNotFoundException("Template file not found.", filePath);
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(filePath);
            return htmlDoc.DocumentNode.OuterHtml;
        }
        private string ReplacePlaceholders(string template, Dictionary<string, string> replacements)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(template);

            foreach (var replacement in replacements)
            {
                template = template.Replace(replacement.Key, replacement.Value);
            }

            return template;
        }
        private List<DetailBanerAds> GetBannerAdsByCode(string cultureCode, string bannerCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@langCode", cultureCode);
                    parameters.Add("@code", bannerCode);

                    var result = connection.QueryFirst<BannerAdsViewModel>("usp_Web_Get_BannerAds_By_Code", parameters, commandType: CommandType.StoredProcedure);
                    if (result != null)
                    {
                        var banners = CMSHelper.ConvertSlide(result);
                        return banners;
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                }
            }
            return null;
        }
    }

    public class UpdateFeedbackDetail
    {
        public int OrderDetailFeedbackId { get; set; }
        public bool IsConfirm { get; set; }
    }

    public class OrderFeedbackDetail
    {
        public int Id { get; set; }
        public string TitleComment { get; set; }
        public string ContentComment { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsConfirm { get; set; }
        public string FileUpload { get; set; }

        public string Customer { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string Title { get; set; }
        public string Preview { get; set; }
        public string Image_Preview { get; set; }
    }
    public class RequesetGetListOrderV2
    {
        public string keyword { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string activeStatus { get; set; }
        public string supplierStatus { get; set; }
        public string emailSupplier { get; set; } = "";
        public int index { get; set; } = 1;
        public int size { get; set; } = 10;
    }
    public class ResponseGetListOrderV2
    {
        public int OrderId { get; set; } = 0;
        public string OrderCode { get; set; } = string.Empty;
        public int OrderDetailId { get; set; } = 0;
        public int CustomerId { get; set; } = 0;
        public int quantityAldut { get; set; } = 0;
        public int QuantityChildren { get; set; } = 0;
        public decimal LogPrice { get; set; } = 0;
        public DateTime CreatedDate { get; set; } = DateTime.MinValue;
        public string ActiveStatus { get; set; } = string.Empty;
        public string MetaData { get; set; } = string.Empty;
        public int ProductChildId { get; set; } = 0;
        public string ProductChildTitle { get; set; } = string.Empty;
        public int ProductParentId { get; set; } = 0;
        public string ProductParentTitle { get; set; } = string.Empty;
        public string ProductParentUrl { get; set; } = string.Empty;
        public string ProductParentAvatar { get; set; } = string.Empty;
        public string productParentLichTour { get; set; } = string.Empty;
        public string productParentThuTucVisa { get; set; } = string.Empty;
        public string productThongTinTour { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string ZoneTitles { get; set; } = string.Empty;
        public string EmailSupplier { get; set; } = string.Empty;
        public string SupplierStatus { get; set; } = string.Empty;
        public string DefaultLanguage { get; set; } = "vi-VN";
        public string FileETicket { get; set; } = "";
        public string CancelResponse { get; set; } = "";
        public decimal LogPriceGross { get; set; } = 0;
        public string SupplierFullName { get; set; } = "";
        public string SupplierUserName { get; set; } = "";
        public string Voucher { get; set; } = "";
        public DateTime pickingDate { get; set; }
        public string OnepayRef { get; set; } = "";
        public string noteSpecial { get; set; }
        public string useAppContact { get; set; }
        public string useAppContactValue { get; set; }
        public string useDiffrenceNumber { get; set; }
        public decimal? rollbackValue { get; set; }
        public int? rollbackOption { get; set; }
        public DateTime? rollbackRequestDate { get; set; }
    }

    public class ResponseGetOrderDetailStatic
    {
        public decimal TotalPrice { get; set; } = 0;
        public decimal TotalGrossPrice { get; set; } = 0;
        public int TotalOrder { get; set; } = 0;
        public int Total_TAO_MOI { get; set; } = 0;
        public int Total_TU_CHOI_DICH_VU { get; set; } = 0;
        public int Total_DA_SU_DUNG_DICH_VU { get; set; } = 0;
        public int Total_YEU_CAU_HUY { get; set; } = 0;
        public int Total_CHAP_NHAN_DICH_VU { get; set; } = 0;
        public int Total_DA_HUY { get; set; } = 0;
    }

    public class RequestUpdateActiveStatusForOrderDetail
    {
        public int orderDetailId { get; set; }
        public string activeStatus { get; set; }
        public string cancelResponse { get; set; }
        public List<IFormFile> files { get; set; } = null;
    }

    public class RequestSendETicket
    {
        public int orderDetailId { get; set; }
    }
    public class MetaDataOrderController
    {

        /*"{\"IsNgoiTaiQuan\":\"1\",\"SoBan\":\"ádasd\",\"GhiChu\":\"áda\",\"HHThanhToan\":\"\",\"ThoiGianGiao\":\"2021-04-08\",\"MaVoucher\":\"\",\"GiaTriVoucher\":0}"*/
        public string IsNgoiTaiQuan { get; set; }
        public string SoBan { get; set; }
        public string GhiChu { get; set; }
        public string HHThanhToan { get; set; }
        public string ThoiGianGiao { get; set; }
        public string MaVoucher { get; set; }
        public int GiaTriVoucher { get; set; }
        public int TongTien { get; set; }

    }
    public class VoucherTomTat
    {
        public string VoucherCode { get; set; }
        public int VoucherGiaTri { get; set; }
        public int TongTien { get; set; }
        public string SoBan { get; set; }
        public string ThoiGianGiao { get; set; }
    }

    public class OrderDetailMetaData
    {
        public string choosenDate { get; set; }
        public Combination combination { get; set; }
        public Currentpickoption[] currentPickOption { get; set; }
        public int numberOfAldut { get; set; }
        public int numberOfChildrend { get; set; }
        public int couponValue { get; set; }
        public int couponType { get; set; }
        public int totalPrice { get; set; }
        public string avatar { get; set; }
        public string bookingName { get; set; }
        public int productId { get; set; }
        public int productChildId { get; set; }
        public string bookingParentName { get; set; }
        public List<NoteOptions> noteOptions { get; set; } = new List<NoteOptions>();
    }

    public class Combination
    {
        public int id { get; set; }
        public string zoneList { get; set; }
        public int priceEachNguoiLon { get; set; }
        public int priceEachTreEm { get; set; }
        public int netEachNguoiLon { get; set; }
        public int netEachTreEm { get; set; }
        public int productId { get; set; }
        public int[] convertedZoneList { get; set; }
    }

    public class Currentpickoption
    {
        public string parentGroup { get; set; }
        public int pickItem { get; set; }
        public string pickItemName { get; set; }
    }
    public class NoteOptions
    {
        public string title { get; set; }
        public string note { get; set; }
    }
}
