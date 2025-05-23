﻿using Dapper;
using MI.Entity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Nest;
using PlatformWEB.ExecuteCommand;
using PlatformWEB.Services.Order.ViewModal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformWEB.Services.Order.Repository
{
    public interface IOrderRepository
    {
        int CreateOrderInWebsite(OrderViewModel orders);
        int GetOrderCode();

        OrderVersionMinifyResponse CreateOrderVersionMinify(OrderVersionMinifyRequest request);

        CouPonDetail GetCouponChildByCode(string Code);
        List<ViewModal.OrderDetail> GetOrderByCode(string orderCode);

        CustomerViewModel DoLogin(CustomerAuthViewModel request);
        int ChangePassword(CustomerAuthViewModel request);
        int DoSignUp(CustomerAuthViewModel request);

        List<OrderDetailViewModel> GetListOrderDetailByOrderId(int orderId, string lang_code);
        BlankSerialNumberViewModel CheckSerialNumber(string serialNumber);
        bool RemoveOrder(string orderCode);
        OrderVersionMinifyResponse CreateOrderMultipleVersionMinify(OrderVersionMinifyRequest request);


        bool UpdateOrderIccid(int orderId, string currentIccid, string newIccid, string joyTelOrderTid);
        bool UpdateTopupStatus(string orderCode, int productId, string joyTelOrderTid);
        bool UpdateOrderInfomationToEsim(string joytelOrderTid, string joytelOrderCode, string snPin, string snSerial, int orderDetailId);
        OrderGeneratedQRResponse UpdateImageQrCode(string coupon, string imageUrl);

    }
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecuters _executers;
        private readonly IHostingEnvironment _hostingEnvironment;

        public OrderRepository(IConfiguration configuration, IExecuters executers, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _hostingEnvironment = hostingEnvironment;
        }

        public int CreateOrderInWebsite(OrderViewModel orders)
        {
            //Xay dung cac thu truoc
            DataTable or_Detail = new DataTable();
            or_Detail.Columns.Add("ProductId", typeof(int));
            or_Detail.Columns.Add("Quantity", typeof(decimal));
            or_Detail.Columns.Add("LogPrice", typeof(decimal));
            or_Detail.Columns.Add("OrderSourceType", typeof(int));
            or_Detail.Columns.Add("OrderSourceId", typeof(int));
            or_Detail.Columns.Add("Voucher", typeof(string));
            or_Detail.Columns.Add("VoucherType", typeof(int));
            or_Detail.Columns.Add("VoucherPrice", typeof(float));

            DataTable or_Promotions = new DataTable();
            or_Promotions.Columns.Add("ProductId", typeof(int));
            or_Promotions.Columns.Add("LogName", typeof(string));
            or_Promotions.Columns.Add("LogValue", typeof(decimal));
            or_Promotions.Columns.Add("LogType", typeof(string));
            foreach (var item in orders.Products)
            {
                or_Detail.Rows.Add(item.ProductId, item.Quantity, item.LogPrice, item.OrderSourceType, item.OrderSourceId, item.Voucher, item.VoucherType, item.VoucherPrice);
                if (item.Promotions != null)
                {
                    foreach (var b in item.Promotions)
                        or_Promotions.Rows.Add(item.ProductId, b.LogName, b.LogValue, b.LogType);
                }
            }

            var p = new DynamicParameters();
            var commandText = "usp_Web_CreateOrder";
            p.Add("@cus_Name", orders.Customer.Name);
            p.Add("@cus_Gender", orders.Customer.Gender);
            p.Add("@cus_PhoneNumber", orders.Customer.PhoneNumber);
            p.Add("@cus_Address", orders.Customer.Address);
            p.Add("@or_Code", orders.OrderCode);
            p.Add("@or_MetaData", Newtonsoft.Json.JsonConvert.SerializeObject(orders.Extras));
            p.Add("@or_InstallmentData", Newtonsoft.Json.JsonConvert.SerializeObject(orders.installmentDetail));
            p.Add("@or_Note", orders.Customer.Note);
            p.Add("@or_Detail", or_Detail.AsTableValuedParameter("type_OrderProduct_v4"));
            p.Add("@or_Promotions", or_Promotions.AsTableValuedParameter("type_OrderPromotionInProduct_v1"));
            p.Add("@out_order_id", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);


            var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            result = p.Get<int>("@out_order_id");
            return result;
        }

        public int GetOrderCode()
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetOrderCode";
            p.Add("@code", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            result = p.Get<int>("@code");
            return result;
        }


        public List<ViewModal.OrderDetail> GetOrderByCode(string orderCode)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetOrderByCode";
            p.Add("@orderCode", orderCode);

            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ViewModal.OrderDetail>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }


        public CouPonDetail GetCouponChildByCode(string Code)
        {
            try
            {
                var p = new DynamicParameters();
                var commantext = "usp_Web_GetVoucherByCode";
                p.Add("@Code", Code.Trim());
                var result = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<CouPonDetail>(commantext, p, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
            catch
            {
                return null;
            }
        }

        public OrderVersionMinifyResponse CreateOrderVersionMinify(OrderVersionMinifyRequest request)
        {
            //usp_Order_InsertOrderVersionMinify
            var p = new DynamicParameters();
            var commandText = "usp_Order_InsertOrderVersionMinify";
            p.Add("@customerRandomId", request.customerRandomId);
            p.Add("@email", request.email);
            p.Add("@totalPrice", request.totalPrice);
            p.Add("@productId", request.productId);
            p.Add("@orderCode", request.orderCode);
            p.Add("@serialNumber", request.serialNumber);
            p.Add("@orderNote", request.orderNote);
            p.Add("@customerId", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            p.Add("@productName", dbType: System.Data.DbType.String, size: 255, direction: System.Data.ParameterDirection.Output);
            p.Add("@iccid", dbType: System.Data.DbType.String, size: 255, direction: System.Data.ParameterDirection.Output);
            p.Add("@isCreatedAccount", dbType: System.Data.DbType.String, size: 255, direction: System.Data.ParameterDirection.Output);


            var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            var kq = new OrderVersionMinifyResponse();
            kq.customerId = p.Get<int>("@customerId");
            kq.productName = p.Get<string>("@productName");
            kq.iccid = p.Get<string>("@iccid");
            //kq.isCreatedAccount = p.Get<int>("@isCreatedAccount");
            kq.isCreatedAccount = 0;
            return kq;
        }

        public OrderVersionMinifyResponse CreateOrderMultipleVersionMinify(OrderVersionMinifyRequest request)
        {
            /*
            @customerRandomId nvarchar(max),
	        @email nvarchar(max),
	        @totalPrice decimal(18,2),
	        @productId int,
	        @orderCode nvarchar(max),
	        @serialNumber nvarchar(max),
	        @orderNote nvarchar(max),
	        @orderInfo type_OrderInfos readonly,
	        @customerId int out,
	        @productName nvarchar(max) out,
	        --@iccid nvarchar(max) out,
	        @isCreatedAccount int out
             */
            var p = new DynamicParameters();
            var commandText = "usp_Order_InsertOrderMultipleVersionMinify_v2";
            p.Add("@customerRandomId", request.customerRandomId);
            p.Add("@email", request.email);
            p.Add("@totalPrice", request.totalPrice);
            p.Add("@productId", request.productId);
            p.Add("@orderCode", request.orderCode);
            p.Add("@serialNumber", request.serialNumber);
            p.Add("@orderNote", request.orderNote);

            DataTable dt = new DataTable();
            dt.Columns.Add("productId", typeof(int));
            dt.Columns.Add("quantity", typeof(int));
            dt.Columns.Add("serialNumber", typeof(string));
            dt.Columns.Add("totalPrice", typeof(decimal));
            dt.Columns.Add("type", typeof(string));

            foreach (var item in request.productsInfo)
            {
                dt.Rows.Add(item.productId, item.quantity, item.serialNumber, item.totalPrice, item.type);
            }
            p.Add("@orderInfo", dt.AsTableValuedParameter("type_OrderInfos_v2"));
            p.Add("@couponCode", request.discountCode);
            p.Add("@customerId", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
            p.Add("@productName", dbType: System.Data.DbType.String, size: 255, direction: System.Data.ParameterDirection.Output);
            //p.Add("@iccid", dbType: System.Data.DbType.String, size: 255, direction: System.Data.ParameterDirection.Output);
            p.Add("@isCreatedAccount", dbType: System.Data.DbType.Int32, size: 255, direction: System.Data.ParameterDirection.Output);
            p.Add("@orderCodeOut", dbType: System.Data.DbType.String, size: 255, direction: System.Data.ParameterDirection.Output);



            var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            var kq = new OrderVersionMinifyResponse();
            kq.customerId = p.Get<int>("@customerId");
            kq.productName = p.Get<string>("@productName");
            kq.orderCode = p.Get<string>("@orderCodeOut");

            //kq.iccid = p.Get<string>("@iccid");
            kq.isCreatedAccount = p.Get<int>("@isCreatedAccount");

            return kq;
        }

        public CustomerViewModel DoLogin(CustomerAuthViewModel request)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetCustomer_ByEmailAndCode";
            p.Add("@email", request.email);
            p.Add("@code", request.password);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<CustomerViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));

            return result;
        }
        public int ChangePassword(CustomerAuthViewModel request)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_ChangeCodeCustomer";
            p.Add("@email", request.email);
            p.Add("@newCode", request.password);
            p.Add("@oldCode", request.oldPassword);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }
        public int DoSignUp(CustomerAuthViewModel request)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_CreateCustomer";
            p.Add("@email", request.email);
            p.Add("@code", request.password);
            p.Add("@outId", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

            var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            result = p.Get<int>("@outId");
            return result;
        }

        public OrderGeneratedQRResponse UpdateImageQrCode(string coupon, string imageUrl)
        {
            try
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_UpdateImageQrCode";
                p.Add("@coupon", coupon);
                p.Add("@imageUrl", imageUrl);
                p.Add("@orderCode", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 50);
                p.Add("@orderDetailId", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
                var orderCode = p.Get<string>("@orderCode");
                var orderDetailId = p.Get<int>("@orderDetailId");
                var _r = new OrderGeneratedQRResponse() { orderCode = orderCode, orderDetailId = orderDetailId };
                return _r;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Ghi log
                string logFilePath = Path.Combine(_hostingEnvironment.WebRootPath, "log.txt");
                string logContent = ex.Message;
                System.IO.File.AppendAllText(logFilePath, logContent + System.Environment.NewLine);
                return null;
            }

        }

        public List<OrderDetailViewModel> GetListOrderDetailByOrderId(int orderId, string lang_code)
        {
            //usp_Web_GetOrderDetailHistory

            var p = new DynamicParameters();
            var commandText = "usp_Web_GetOrderDetailHistory";
            p.Add("@orderId", orderId);
            p.Add("@langCode", lang_code);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<OrderDetailViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();

            return result;
        }

        public BlankSerialNumberViewModel CheckSerialNumber(string serialNumber)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_CheckSimSerial";
            p.Add("@serial", serialNumber);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirstOrDefault<BlankSerialNumberViewModel>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));

            return result;
        }

        public bool RemoveOrder(string orderCode)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_RemoveOrder";
            p.Add("@orderCode", orderCode);

            var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));

            return true;
        }

        public bool UpdateOrderIccid(int orderId, string currentIccid, string newIccid, string joyTelOrderTid)
        {

            try
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_UpdateOrderIccid";
                p.Add("@orderId", orderId);
                p.Add("@currentIccid", currentIccid);
                p.Add("@newIccid", newIccid);
                p.Add("@joyTelOrderTid", joyTelOrderTid);
                var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public bool UpdateTopupStatus(string orderCode, int productId, string joyTelOrderTid)
        {
            try
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_UpdateTopupStatus";
                p.Add("@orderCode", orderCode);
                p.Add("@productId", productId);
                p.Add("@joyTelOrderTid", joyTelOrderTid);
                var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));

                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public bool UpdateOrderInfomationToEsim(string joytelOrderTid, string joytelOrderCode, string snPin, string snSerial, int orderDetailId)
        {
            try
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_UpdateOrderInfomationToEsim";
                p.Add("@joytelOrderTid", joytelOrderTid);
                p.Add("@joytelOrderCode", joytelOrderCode);
                p.Add("@snPin", snPin);
                p.Add("@snSerial", snSerial);
                p.Add("@orderDetailId", orderDetailId);
                var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return false;
            }
        }


    }
    public class CouPonDetail
    {
        public string Name { get; set; }
        public int DiscountOption { get; set; }
        public string ValueDiscount { get; set; }
    }
}
