using Dapper;
using HtmlAgilityPack;
using MI.Dal.IDbContext;
using MI.Entity.Enums;
using MI.Entity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;

//using MimeKit;
using Nest;
using NLog.Web.LayoutRenderers;
using Org.BouncyCastle.Asn1.Ocsp;
using PlatformWEBAPI.ExecuteCommand;
using PlatformWEBAPI.Services.BannerAds.Repository;
using PlatformWEBAPI.Services.Order.ViewModal;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Utils;
using Microsoft.EntityFrameworkCore;
using PlatformWEBAPI.PushNotification;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using FirebaseAdmin.Messaging;

namespace PlatformWEBAPI.Services.Order.Repository
{
    public interface IOrderRepository
    {
        int CreateOrderInWebsite(OrderViewModel orders);
        int GetOrderCode();

        OrderVersionMinifyResponse CreateOrderVersionMinify(OrderVersionMinifyRequest request);

        CouPonDetail GetCouponChildByCode(string Code);
        List<ViewModal.OrderDetail> GetOrderByCode(string orderCode);

        Task<CustomerAuth> DoLogin(CustomerAuthViewModel request);
        int ChangePassword(CustomerAuthViewModel request);
        int ForgotPassword(CustomerForGotPassViewModel request);
        int DoSignUp(CustomerAuthViewModel request);

        List<OrderDetailViewModel> GetListOrderDetailByOrderId(int orderId, string lang_code);
        BlankSerialNumberViewModel CheckSerialNumber(string serialNumber);
        bool RemoveOrder(string orderCode);
        OrderVersionMinifyResponse CreateOrderMultipleVersionMinify(OrderVersionMinifyRequest request);


        bool UpdateOrderIccid(int orderId, string currentIccid, string newIccid, string joyTelOrderTid);
        bool UpdateTopupStatus(string orderCode, int productId, string joyTelOrderTid);
        bool UpdateOrderInfomationToEsim(string joytelOrderTid, string joytelOrderCode, string snPin, string snSerial, int orderDetailId);
        OrderGeneratedQRResponse UpdateImageQrCode(string coupon, string imageUrl);

        Task<ResponseCreateMultipleItemOrder> CreateMultipleItemOrderAsync(RequestCreateMultipleItemOrder request);
        Task<List<ResponseGetOrdersByCustomerId>> GetOrdersByCustomerId(RequestGetOrdersByCustomerId request);

        Task<ResponseGetOrderItemFullDetail> GetOrderItemFullDetail(RequestGetOrderItemFullDetail request);

        List<ResponseGetOrderItemFullDetailForSupplier> GetOrderItemFullDetailPedningForSupplier();

        Task<bool> CancelOrdersOrderDetail(RequestCancelOrdersOrderDetail request);

        Task<bool> SendNewUserEmail(RequestSendNewUserEmail request);
        Task<bool> SendNewSubscriberEmail(SubscribersRequest request);
        Task<bool> SendNewOrderEmailToCustomer(RequestSendNewOrderEmail request);
        Task<bool> SendNewOrderEmailToHelpDesk(RequestSendNewOrderEmail request);
        bool SendRequestOrderToSupplierInBackground();
        List<ResponseGetCouponByProductId> GetCouponByProductId(RequestGetCouponByProductId request);
        ResponseGetCouponByProductId CheckCouponCode(RequestCheckCouponCode request);
        string GenerateOrderCode();
        int EmailSubscriptionRegistration(string email);
        Task<string> UpdateAvatarCustomer(string base64Image, string emailCustomer);
        Task<OrderDetailFeedback> UpdateOrderDetailCommentWithRating(RequestUpdateOrderDetailCommentWithRating request);
        Task<ResponseOrderDetailCommentWithRating> GetOrderDetailCommentWithRating(int orderDetailId);
        List<ResponseGetLastChatDetailBySessionForCustomer> ResponseGetLastChatDetailBySessionForCustomer();
        List<ResponseGetLastChatDetailBySessionForCustomer> CheckChatSessionByCustomerEmail(RequestCheckChatSessionByCustomerEmail request);
        List<ResponseOrderNotificationFeedback> GetOrderNotificationFeedback();
        ResponseCheckOrderDetailByEmail CheckOrderDetailByEmail(RequestCheckOrderDetailByEmail request);

    }
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecuters _executers;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IBannerAdsRepository _bannerAdsRepository;


        public OrderRepository(IConfiguration configuration, IExecuters executers, IHostingEnvironment hostingEnvironment, IBannerAdsRepository bannerAdsRepository)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _hostingEnvironment = hostingEnvironment;
            _bannerAdsRepository = bannerAdsRepository;
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

        public async Task<CustomerAuth> DoLogin(CustomerAuthViewModel request)
        {
            try
            {
                using (IDbContext context = new IDbContext())
                {
                    var customer = context.Customer.Where(r => r.Pcname.Equals(request.password) && r.Email.Equals(request.email)).FirstOrDefault();
                    if (customer != null)
                    {
                        var response = new CustomerAuth();
                        response.id = customer.Id;
                        response.email = customer.Email;
                        response.country = customer.Country;
                        response.phoneNumber = customer.PhoneNumber;
                        response.avatar = customer.Avatar;
                        response.firstName = customer.Fullname?.Split(" ").FirstOrDefault();
                        response.lastName = customer.Fullname?.Split(" ").LastOrDefault();
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

            return null;
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

        public int ForgotPassword(CustomerForGotPassViewModel request)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_ForgotPassword";
            p.Add("@email", request.email);
            p.Add("@newCode", request.password);
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


        public async Task<ResponseCreateMultipleItemOrder> CreateMultipleItemOrderAsync(RequestCreateMultipleItemOrder request)
        {
            using (IDbContext context = new IDbContext())
            {
                using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable))
                {
                    try
                    {
                        //Tra ve
                        var cultureCode = "";
                        switch (request.i18Code)
                        {
                            case "en":
                                cultureCode = "en-US";
                                break;
                            case "vi":
                                cultureCode = "vi-VN";
                                break;
                        }

                        bool isNewUser = false;
                        var pcName = "";
                        var customer = context.Customer.FirstOrDefault(r => r.Email == request.auth.email);

                        if (customer == null)
                        {
                            customer = new Customer
                            {
                                Email = request.auth.email,
                                Fullname = request.auth.firstName + " " + request.auth.lastName,
                                PhoneNumber = request.auth.phoneNumber,
                                Name = RandomCode(12),
                                Pcname = RandomCode(12),
                                Type = 1,
                                Source = 1,
                                Country = request.auth.country
                            };
                            pcName = customer.Pcname;
                            context.Customer.Add(customer);
                            await context.SaveChangesAsync();
                            isNewUser = true;
                        }
                        else
                        {
                            customer.Fullname = request.auth.firstName + " " + request.auth.lastName;
                            customer.PhoneNumber = request.auth.phoneNumber;
                            context.Customer.Update(customer);
                            await context.SaveChangesAsync();
                        }

                        var existingOrder = context.Orders
                                            .FromSql("SELECT * FROM Orders WITH (UPDLOCK, HOLDLOCK) WHERE OnepayRef = {0}", request.orderCode)
                                            .FirstOrDefault();
                        if (existingOrder != null)
                        {
                            return new ResponseCreateMultipleItemOrder
                            {
                                auth = new CustomerAuth
                                {
                                    isNewUser = false,
                                    firstName = request.auth.firstName,
                                    lastName = request.auth.lastName,
                                    phoneNumber = customer.PhoneNumber,
                                    email = customer.Email,
                                    id = customer.Id,
                                    pcname = customer.Pcname,
                                    country = customer.Country
                                },
                                orderCode = existingOrder.OrderCode
                            };
                        }

                        var order = new Orders
                        {
                            CustomerId = customer.Id,
                            OrderCode = CreateOrderCode(context.Orders.Count() + 1),
                            OnepayRef = request.orderCode,
                            CreatedDate = DateTime.Now,
                            CreatedBy = customer.Email,
                            Status = "TAO_MOI",
                            MetaData = Newtonsoft.Json.JsonConvert.SerializeObject(request)
                        };

                        context.Orders.Add(order);
                        await context.SaveChangesAsync();

                        var insertOrderDetail = new List<MI.Entity.Models.OrderDetail>();

                        foreach (var orderItem in request.pays)
                        {
                            var pickingDateByCustomer = DateTime.MinValue;
                            var pickingDateSplited = orderItem.choosenDate.Split("/");
                            if (pickingDateSplited.Length == 3)
                            {
                                pickingDateByCustomer = DateTime.Parse($"{pickingDateSplited[2]}-{pickingDateSplited[1]}-{pickingDateSplited[0]}");
                            }

                            var orderDetail = new MI.Entity.Models.OrderDetail
                            {
                                OrderId = order.Id,
                                ProductId = orderItem.combination?.productId ?? 0,
                                CombinationZoneList = orderItem.combination?.zoneList,
                                PriceEach = orderItem.combination?.priceEachNguoiLon ?? 0,
                                PriceEachChildren = orderItem.combination?.priceEachTreEm ?? 0,
                                LogPrice = orderItem.totalPrice,
                                Quantity = orderItem.numberOfAldut,
                                QuantityChildren = orderItem.numberOfChildrend,
                                CreatedDate = DateTime.Now,
                                ProductParentId = orderItem.productId,
                                ActiveStatus = "TAO_MOI",
                                SupplierStatus = "PENDING",
                                PaymentMethod = request.paymentMethod,
                                OnepayRef = request.orderCode,
                                PickingDate = pickingDateByCustomer,
                                DefaultLanguage = cultureCode,
                                noteSpecial = request.orderNotes.noteSpecial,
                                useAppContact = request.orderNotes.useAppContact,
                                useDiffrenceNumber = request.orderNotes.useDiffrenceNumber,
                                useAppContactValue = request.orderNotes.useAppContactValue
                            };

                            if (orderItem.discountSelected != null)
                            {
                                orderDetail.Voucher = orderItem.discountSelected.couponCode;
                                if (orderItem.discountSelected.couponPrice > 0)
                                {
                                    orderDetail.LogPrice -= orderItem.discountSelected.couponPrice;
                                }
                            }

                            var optionItem = context.ProductPriceInZoneListByDate.FirstOrDefault(r =>
                                r.ProductId == orderItem.combination.productId &&
                                r.Date.Date == pickingDateByCustomer.Date &&
                                r.ZoneList == orderItem.combination.zoneList);

                            if (optionItem != null)
                            {
                                orderDetail.PriceGross = optionItem.NetEachNguoiLon;
                                orderDetail.PriceGrossTreEm = optionItem.NetEachTreEm;
                                orderDetail.LogPriceGross = (optionItem.NetEachNguoiLon * orderItem.numberOfAldut) +
                                                            (optionItem.NetEachTreEm * orderItem.numberOfChildrend);
                            }

                            foreach (var noteGroup in orderItem.productBookingNoteGroups)
                            {
                                foreach (var noteItem in noteGroup.NoteList)
                                {
                                    if (noteItem.bookingNoteType.StartsWith("date") &&
                                        DateTime.TryParse(noteItem.noteValue, out DateTime _d))
                                    {
                                        noteItem.noteValue = _d.AddHours(7).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss");
                                    }
                                }
                            }

                            orderDetail.MetaData = Newtonsoft.Json.JsonConvert.SerializeObject(orderItem);
                            insertOrderDetail.Add(orderDetail);
                        }

                        context.AddRange(insertOrderDetail);
                        await context.SaveChangesAsync();

                        
                        


                        transaction.Commit();

                        return new ResponseCreateMultipleItemOrder
                        {
                            auth = new CustomerAuth
                            {
                                isNewUser = isNewUser,
                                firstName = request.auth.firstName,
                                lastName = request.auth.lastName,
                                phoneNumber = customer.PhoneNumber,
                                email = customer.Email,
                                id = customer.Id,
                                pcname = pcName,
                                country = request.auth.country
                            },
                            orderCode = order.OrderCode
                        };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        // log ex here if needed
                        return null;
                    }
                }
            }
        }


        private string RandomCode(int size)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            var random = new Random();
            var result = new StringBuilder(size);

            for (int i = 0; i < size; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }

        public async Task<List<ResponseGetOrdersByCustomerId>> GetOrdersByCustomerId(RequestGetOrdersByCustomerId request)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetAllOrdersByCustomerId";
            p.Add("@customerId", request.customerId);
            p.Add("@lang_code", request.cultureCode);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ResponseGetOrdersByCustomerId>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public async Task<ResponseGetOrderItemFullDetail> GetOrderItemFullDetail(RequestGetOrderItemFullDetail request)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetOrderDetailFullInfomation";
            /*
             @orderCode nvarchar(max),
	@orderDetailId int,
             */
            p.Add("@customerId", request.customerId);
            p.Add("@orderCode", request.orderCode);
            p.Add("@orderDetailId", request.orderDetailId);
            p.Add("@lang_code", request.cultureCode);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirst<ResponseGetOrderItemFullDetail>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }

        public async Task<bool> CancelOrdersOrderDetail(RequestCancelOrdersOrderDetail request)
        {
            using (IDbContext context = new IDbContext())
            {
                var customer = context.Customer.Where(r => r.Id == request.customerId).FirstOrDefault();
                if (customer != null)
                {
                    var order = context.Orders.Where(r => r.OrderCode.Equals(request.orderCode)).FirstOrDefault();
                    if (order != null)
                    {
                        var orderDetail = context.OrderDetail.Where(r => r.Id == request.orderDetailId && r.OrderId == order.Id).FirstOrDefault();
                        if (orderDetail != null)
                        {
                            orderDetail.ActiveStatus = "YEU_CAU_HUY";
                            orderDetail.rollbackOption = request.rollbackOption;
                            orderDetail.rollbackValue = request.rollbackValue;
                            orderDetail.RollbackRequestDate = DateTime.Now;

                            context.OrderDetail.Update(orderDetail);
                            await context.SaveChangesAsync();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async Task<bool> SendNewUserEmail(RequestSendNewUserEmail request)
        {
            try
            {
                var wwwrootPath = _hostingEnvironment.WebRootPath;
                var templatePath = Path.Combine(wwwrootPath, "mail-templates", "mail-dang-ky-user-moi.html");
                if (File.Exists(templatePath))
                {

                    var templateString = ReadTemplateFromFile(templatePath);
                    if (!string.IsNullOrEmpty(templateString))
                    {
                        var mailInfo = _bannerAdsRepository.GetBannerAds_By_Code(request.culture_code, "MAIL_CULTURE_NEW_REGISTER");
                        if (mailInfo != null)
                        {
                            var banners = WebHelper.ConvertSlide(mailInfo);
                            if (banners != null)
                            {
                                Dictionary<string, string> mailHooks = new Dictionary<string, string>();
                                foreach (var item in banners)
                                {
                                    if (!string.IsNullOrEmpty(item.Title))
                                    {
                                        mailHooks.Add($"[{item.Title}]", WebHelper.GetCultureText(banners, item.Title));
                                    }
                                }
                                mailHooks.Add("[CUSTOMER_FULLNAME]", request.customerName);
                                mailHooks.Add("[DATA_FULL_NAME]", request.customerName);
                                mailHooks.Add("[DATA_USERNAME]", request.username);
                                mailHooks.Add("[DATA_PASSWORD]", request.password);

                                var outputHtml = ReplacePlaceholders(templateString, mailHooks);
                                if (!string.IsNullOrEmpty(outputHtml))
                                {
                                    var smtpServer = _configuration["EmailSender:Host"];
                                    int smtpPort = int.Parse(_configuration["EmailSender:Port"]);
                                    var smtpUser = _configuration["EmailSender:CustomerService:Email"]; // cs@joytime.vn
                                    var smtpPass = _configuration["EmailSender:CustomerService:Password"]; //D2A9HnvGMJYW3BeKQw5f4F
                                    var title = mailHooks.GetValueOrDefault("[MAIL_TITLE]");
                                    var subject = "";
                                    if (!string.IsNullOrEmpty(title))
                                    {
                                        subject = ConvertToCorrectEncoding(title);
                                    }

                                    var body = outputHtml;

                                    var toEmail = request.customerEmail;


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
                                            await client.ConnectAsync(smtpServer, smtpPort, true);
                                            await client.AuthenticateAsync(smtpUser, smtpPass);
                                            await client.SendAsync(message);
                                            await client.DisconnectAsync(true);
                                        }
                                        catch (Exception ex)
                                        {
                                            // Handle exception (e.g., log the error)
                                            Console.WriteLine($"ERROR: {ex.Message}");
                                            return false;
                                        }
                                    }

                                    //using (var client = new SmtpClient(smtpServer, smtpPort))
                                    //{
                                    //    client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                                    //    client.EnableSsl = true;

                                    //    var mailMessage = new MailMessage
                                    //    {
                                    //        From = new MailAddress(smtpUser),
                                    //        Subject = subject,
                                    //        Body = body,
                                    //        IsBodyHtml = true
                                    //    };

                                    //    mailMessage.To.Add(toEmail);

                                    //    await client.SendMailAsync(mailMessage);
                                    //    return true;
                                    //}
                                }
                            }
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return false;

        }

        public async Task<bool> SendNewSubscriberEmail(SubscribersRequest request)
        {
            try
            {
                var wwwrootPath = _hostingEnvironment.WebRootPath;
                var templatePath = Path.Combine(wwwrootPath, "mail-templates", "mail-subscriber-success.html");
                if (File.Exists(templatePath))
                {

                    var templateString = ReadTemplateFromFile(templatePath);
                    if (!string.IsNullOrEmpty(templateString))
                    {
                        var mailInfo = _bannerAdsRepository.GetBannerAds_By_Code(request.culture_code, "MAIL_CULTURE_NEW_REGISTER");
                        if (mailInfo != null)
                        {
                            var banners = WebHelper.ConvertSlide(mailInfo);
                            if (banners != null)
                            {
                                Dictionary<string, string> mailHooks = new Dictionary<string, string>();
                                foreach (var item in banners)
                                {
                                    if (!string.IsNullOrEmpty(item.Title))
                                    {
                                        mailHooks.Add($"[{item.Title}]", WebHelper.GetCultureText(banners, item.Title));
                                    }
                                }

                                var outputHtml = ReplacePlaceholders(templateString, mailHooks);
                                if (!string.IsNullOrEmpty(outputHtml))
                                {
                                    var smtpServer = _configuration["EmailSender:Host"];
                                    int smtpPort = int.Parse(_configuration["EmailSender:Port"]);
                                    var smtpUser = _configuration["EmailSender:CustomerService:Email"]; // cs@joytime.vn
                                    var smtpPass = _configuration["EmailSender:CustomerService:Password"]; //D2A9HnvGMJYW3BeKQw5f4F
                                    var title = mailHooks.GetValueOrDefault("[MAIL_TITLE]");
                                    var subject = "";
                                    if (!string.IsNullOrEmpty(title))
                                    {
                                        subject = ConvertToCorrectEncoding(title);
                                    }

                                    var body = outputHtml;

                                    var toEmail = request.Email;


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
                                            await client.ConnectAsync(smtpServer, smtpPort, true);
                                            await client.AuthenticateAsync(smtpUser, smtpPass);
                                            await client.SendAsync(message);
                                            await client.DisconnectAsync(true);
                                        }
                                        catch (Exception ex)
                                        {
                                            // Handle exception (e.g., log the error)
                                            Console.WriteLine($"ERROR: {ex.Message}");
                                            return false;
                                        }
                                    }

                                    //using (var client = new SmtpClient(smtpServer, smtpPort))
                                    //{
                                    //    client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                                    //    client.EnableSsl = true;

                                    //    var mailMessage = new MailMessage
                                    //    {
                                    //        From = new MailAddress(smtpUser),
                                    //        Subject = subject,
                                    //        Body = body,
                                    //        IsBodyHtml = true
                                    //    };

                                    //    mailMessage.To.Add(toEmail);

                                    //    await client.SendMailAsync(mailMessage);
                                    //    return true;
                                    //}
                                }
                            }
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return false;
        }

        private string ReadTemplateFromFile(string filePath)
        {
            if (!File.Exists(filePath))
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

        public async Task<bool> SendNewOrderEmailToCustomer(RequestSendNewOrderEmail request)
        {

            var wwwrootPath = _hostingEnvironment.WebRootPath;
            var tempFile = "";
            if (request.activeStatus.Equals("TAO_MOI"))
            {
                tempFile = "mail-thanh-toan-don-hang.html";
            }
            if (request.activeStatus.Equals("CHAP_NHAN_DICH_VU"))
            {
                tempFile = "mail-e-ticket.html";
            }
            var templatePath = Path.Combine(wwwrootPath, "mail-templates", tempFile);
            if (File.Exists(templatePath))
            {
                var templateString = ReadTemplateFromFile(templatePath);
                var orderDetailsId = new List<int>();
                using (IDbContext context = new IDbContext())
                {
                    var order = await context.Orders.Where(r => r.OrderCode.Equals(request.orderCode)).LastOrDefaultAsync(); // Tai sao lại bị trùng đơn hàng được nhỉ

                    if (order != null)
                    {
                        orderDetailsId = context.OrderDetail.Where(r => r.OrderId == order.Id).Select(r => r.Id).ToList();
                    }
                }
                //Lay cultureCode
                //Dictionary<string, string> mailHooks = new Dictionary<string, string>();
                var mailInfo = _bannerAdsRepository.GetBannerAds_By_Code(request.culture_code, "MAIL_CULTURE_NEW_ORDER");
                
                foreach (var item in orderDetailsId)
                {
                    var banners = WebHelper.ConvertSlide(mailInfo);
                    var requestGetOrderItemFullDetail = new RequestGetOrderItemFullDetail();
                    requestGetOrderItemFullDetail.customerId = request.customerId;
                    requestGetOrderItemFullDetail.orderCode = request.orderCode;
                    requestGetOrderItemFullDetail.orderDetailId = item.ToString();
                    requestGetOrderItemFullDetail.cultureCode = request.culture_code;



                    //Goi db
                    var detail = await this.GetOrderItemFullDetail(requestGetOrderItemFullDetail);
                    if (detail != null)
                    {
                        Dictionary<string, string> mailHooks = new Dictionary<string, string>();

                        if (mailInfo != null)
                        {
                            //var banners = WebHelper.ConvertSlide(mailInfo);
                            if (banners != null)
                            {

                                foreach (var b in banners.Where(r => !r.Title.Equals("MAIL_NOI_DUNG_NOTE")))
                                {
                                    if (!string.IsNullOrEmpty(b.Title))
                                    {
                                        mailHooks.Add($"[{b.Title}]", WebHelper.GetCultureText(banners, b.Title));
                                    }
                                }

                            }

                        }
                        mailHooks.Add("[CUSTOMER_FULLNAME]", request.customerName);
                        mailHooks.Add("[DATA_FULL_NAME]", request.customerName);
                        mailHooks.Add("[DATA_MA_DON_HANG]", detail.OrderCode);
                        mailHooks.Add("[DATA_NGAY_DAT_DICH_VU]", detail.CreatedDate.ToString("dd/MM/yyyy HH:mm"));
                        //DATA_TEN_SAN_PHAM
                        mailHooks.Add("[DATA_TEN_SAN_PHAM]", detail.ProductParentTitle);
                        mailHooks.Add("[DATA_TEN_PACKAGE]", detail.ProductChildTitle);
                        mailHooks.Add("[DATA_TEN_FULL_OPTION]", detail.ZoneTitles);
                        mailHooks.Add("[MAIL_NOI_DUNG_SAN_PHAM_AVATAR]", UIHelper.StoreFilePath(detail.ProductParentAvatar));
                        var metadata = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderDetailMetaData>(detail.MetaData);
                        mailHooks.Add("[DATA_NGAY_SU_DUNG]", metadata.choosenDate);
                        //DATA_NGAY_DAT
                        mailHooks.Add("[DATA_NGAY_DAT]", metadata.choosenDate);
                        var MAIL_NOI_DUNG_SO_LUONG = new List<string>();
                        var MAIL_NOI_DUNG_SO_LUONG_NGUOI_LON = mailHooks.GetValueOrDefault("[MAIL_NOI_DUNG_SO_LUONG_NGUOI_LON]");
                        var MAIL_NOI_DUNG_SO_LUONG_TRE_EM = mailHooks.GetValueOrDefault("[MAIL_NOI_DUNG_SO_LUONG_TRE_EM]");
                        
                        
                        if(detail.quantityAldut > 0 && detail.QuantityChildren <= 0)
                        {
                            if (!string.IsNullOrEmpty(detail.unit))
                            {
                                MAIL_NOI_DUNG_SO_LUONG_NGUOI_LON = detail.unit;
                            }
                        }
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
                        


                        var outputHtml = ReplacePlaceholders(templateString, mailHooks);
                        if (!string.IsNullOrEmpty(outputHtml))
                        {
                            var htmlDoc = new HtmlDocument();
                            htmlDoc.LoadHtml(outputHtml);

                            // Find the div with class 'booking-note-wrapper'
                            var bookingNoteDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'booking-note-wrapper')]");
                            if (bookingNoteDiv != null)
                            {
                                //Lấy ra tất cả options của note ở đay
                                foreach(var noteGroup in metadata.productBookingNoteGroups)
                                {
                                    foreach(var note in noteGroup.NoteList)
                                    {
                                        if (note.bookingNoteSendWithMail)
                                        {
                                            var noteLabel = note.ZoneName;
                                            noteLabel = Regex.Replace(noteLabel, @"\s*\(.*?\)", "").Trim();
                                            var noteValue = note.noteValue;
                                            var _tryParseDateTimeValue = DateTime.Now;
                                            var noteOutString = note.noteValue;
                                            // Bat 1 so truong hop o day
                                            if(DateTime.TryParse(noteValue, out _tryParseDateTimeValue))
                                            {
                                                noteValue = _tryParseDateTimeValue.ToString("dd/MM/yyyy HH:mm");
                                            }
                                            if (!string.IsNullOrEmpty(noteValue))
                                            {
                                                string htmlContent = $@"
                                                    <table class=""paragraph_block block-3"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;"">
                                                      <tr>
                                                        <td class=""pad"" style=""padding-bottom:10px;padding-left:25px;padding-right:10px;"">
                                                          <div style=""color:#122f50;font-family:'Montserrat', 'Trebuchet MS', 'Lucida Grande', 'Lucida Sans Unicode', 'Lucida Sans', Tahoma, sans-serif;font-size:14px;line-height:120%;text-align:left;mso-line-height-alt:16.8px;"">
                                                            <p style=""margin: 0; word-break: break-word;"">{noteLabel}: {noteValue}</p>
                                                          </div>
                                                        </td>
                                                      </tr>
                                                    </table>";

                                                //Sau đó append cái này vào bookingNoteDiv ở trên
                                                var newNode = HtmlNode.CreateNode(htmlContent);
                                                bookingNoteDiv.AppendChild(newNode);
                                            }
                                        }
                                        
                                    }
                                }
                                
                            }
                            //note-spectial-wrapper
                            var noteSpectialDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'note-spectial-wrapper')]");
                            if(noteSpectialDiv != null)
                            {
                                if (request.orderNotes != null)
                                {
                                    var hookNoteTitleValue = banners.FirstOrDefault(r => r.Title.Equals("MAIL_NOI_DUNG_NOTE"));
                                    if (hookNoteTitleValue != null)
                                    {
                                        
                                        if (!string.IsNullOrEmpty(request.orderNotes.noteSpecial))
                                        {
                                            var _str = WebHelper.GetCultureText(banners, hookNoteTitleValue.Title);
                                            mailHooks.Add("[MAIL_NOI_DUNG_NOTE]", $"{_str}");
                                            mailHooks.Add("[DATA_GIA_NOTE]", $"{request.orderNotes.noteSpecial}");
                                            string htmlContent = $@"
                                                    <table class=""paragraph_block block-4"" width=""100%"" border=""0""
														   cellpadding=""0"" cellspacing=""0"" role=""presentation""
														   style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;"">
														<tr>
															<td class=""pad""
																style=""padding-bottom:10px;padding-left:25px;padding-right:10px;"">
																<div style=""color:#122f50;font-family: 'Reddit Sans', sans-serif;font-size:14px;line-height:120%;text-align:left;mso-line-height-alt:16.8px;"">
																	<p style=""margin: 0; word-break: break-word;"">
																		{_str} : {request.orderNotes.noteSpecial}
																	</p>
																</div>
															</td>
														</tr>
													</table>
                                            ";
                                            var newNode = HtmlNode.CreateNode(htmlContent);
                                            noteSpectialDiv.AppendChild(newNode);

                                        }
                                    }

                                }
                            }
                            //Gửi mail ở đây
                            outputHtml = htmlDoc.DocumentNode.OuterHtml;
                            
                            var smtpServer = _configuration["EmailSender:Host"];
                            int smtpPort = int.Parse(_configuration["EmailSender:Port"]);
                            var smtpUser = _configuration["EmailSender:BookingService:Email"]; // cs@joytime.vn
                            var smtpPass = _configuration["EmailSender:BookingService:Password"]; //D2A9HnvGMJYW3BeKQw5f4F
                            var title = mailHooks.GetValueOrDefault("[MAIL_TITLE]");
                            var subject = "";
                            if (!string.IsNullOrEmpty(title))
                            {
                                subject = ConvertToCorrectEncoding(title);
                            }

                            var body = outputHtml;

                            var toEmail = request.customerEmail;

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
                                    await client.ConnectAsync(smtpServer, smtpPort, true);
                                    await client.AuthenticateAsync(smtpUser, smtpPass);
                                    await client.SendAsync(message);
                                    await client.DisconnectAsync(true);
                                }
                                catch (Exception ex)
                                {
                                    // Handle exception (e.g., log the error)
                                    Console.WriteLine($"ERROR: {ex.Message}");
                                    return false;
                                }
                            }

                            try
                            {
                                // Tạo ra luôn PushNotification ở đây
                                using (IDbContext context = new IDbContext())
                                {
                                    var bannerCode = "NOTI_APP_NEW_ORDER";
                                    var notiCulture = _bannerAdsRepository.GetBannerAds_By_Code(request.culture_code, "NOTI_APP_NEW_ORDER");
                                    var notiTitle = "";
                                    var notiDescription = "";
                                    if (notiCulture != null)
                                    {
                                        var _b = WebHelper.ConvertSlide(notiCulture);
                                        var _t = banners.Where(r => r.Title.Equals("[TITLE]"));
                                        if (_t != null)
                                        {
                                            notiTitle = WebHelper.GetCultureNotiText(_b, "[TITLE]");
                                            notiTitle = notiTitle.Replace("[DON_HANG]", detail.OrderCode);
                                        }
                                        var _d = banners.Where(r => r.Title.Equals("[DESCRIPTION]"));
                                        if (_d != null)
                                        {
                                            notiDescription = WebHelper.GetCultureNotiText(_b, "[DESCRIPTION]");
                                            notiDescription = notiDescription.Replace("[DON_HANG]", detail.OrderCode);
                                            notiDescription = notiDescription.Replace("[PRODUCT_NAME]", detail.ProductParentTitle);
                                        }
                                    }



                                    var notification = new S_PushNotification();
                                    notification.CustomerId = detail.CustomerId;
                                    notification.EmailReceiver = detail.Email;
                                    notification.NotificationBannerCode = bannerCode;
                                    notification.NotificationTitle = notiTitle;
                                    notification.NotificationDescription = notiDescription;
                                    notification.OrderCode = detail.OrderCode;
                                    notification.OrderDetailId = detail.OrderDetailId;
                                    notification.IsPushToClient = true;
                                    notification.CreationTime = DateTime.Now;
                                    notification.PushingTime = DateTime.Now;
                                    notification.IsReaded = false;


                                    await context.S_PushNotification.AddAsync(notification);
                                    await context.SaveChangesAsync();

                                    // 🔔 Push notification
                                    var tokenRecord = await context.CustomerFcmToken.FirstOrDefaultAsync(x => x.Email == notification.EmailReceiver);

                                    if (tokenRecord != null)
                                    {
                                        var _message = new Message()
                                        {
                                            Token = tokenRecord.FcmToken,
                                            Notification = new Notification
                                            {
                                                Title = notiTitle,
                                                Body = notiDescription
                                            },
                                            Data = new Dictionary<string, string>
                                            {
                                                { "route", "/myorder?v=1" }
                                            }
                                        };

                                        await FirebaseMessaging.DefaultInstance.SendAsync(_message);


                                    }
                                }
                                    
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                //throw;
                            }

                        }


                    }


                }
                return true;
            }
            return false;

        }
        private string ConvertToCorrectEncoding(string input)
        {
            // Giải mã các ký tự HTML entities thành chuỗi Unicode
            string decodedString = HttpUtility.HtmlDecode(input);

            return decodedString;

        }

        private string CreateOrderCode(int count)
        {
            // Tăng giá trị của count lên 1
            int orderNumber = count + 1;

            // Tạo chuỗi định dạng với 5 chữ số, có thêm các số 0 ở đầu nếu cần
            string orderCode = $"JT_{orderNumber:D5}";

            return orderCode;
        }

        public bool SendRequestOrderToSupplierInBackground()
        {

            var result = this.GetOrderItemFullDetailPedningForSupplier();
            foreach (var detail in result)
            {
                var wwwrootPath = _hostingEnvironment.WebRootPath;
                var templatePath = Path.Combine(wwwrootPath, "mail-templates", "mail-thong-bao-doi-tac.html");
                if (File.Exists(templatePath))
                {
                    var templateString = ReadTemplateFromFile(templatePath);


                    //Lay cultureCode
                    Dictionary<string, string> mailHooks = new Dictionary<string, string>();
                    var mailInfo = _bannerAdsRepository.GetBannerAds_By_Code("vi-VN", "MAIL_CULTURE_NEW_ORDER_FOR_SUPPLIER");
                    if (mailInfo != null)
                    {
                        var banners = WebHelper.ConvertSlide(mailInfo);
                        if (banners != null)
                        {

                            foreach (var b in banners)
                            {
                                if (!string.IsNullOrEmpty(b.Title))
                                {
                                    mailHooks.Add($"[{b.Title}]", WebHelper.GetCultureText(banners, b.Title));
                                }
                            }

                        }

                    }
                    mailHooks.Add("[DATA_FULL_NAME]", detail.FullName);
                    mailHooks.Add("[DATA_MA_DON_HANG]", detail.OrderCode);
                    mailHooks.Add("[DATA_NGAY_DAT]", detail.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss"));
                    var metadata = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderDetailMetaData>(detail.MetaData);
                    mailHooks.Add("[DATA_NGAY_SU_DUNG]", metadata.choosenDate);
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

                    var outputHtml = ReplacePlaceholders(templateString, mailHooks);
                    if (!string.IsNullOrEmpty(outputHtml))
                    {
                        try
                        {
                            var smtpServer = _configuration["EmailSender:Host"];
                            int smtpPort = int.Parse(_configuration["EmailSender:Port"]);
                            var smtpUser = _configuration["EmailSender:SupplierService:Email"]; // cs@joytime.vn
                            var smtpPass = _configuration["EmailSender:SupplierService:Password"]; //D2A9HnvGMJYW3BeKQw5f4F
                            var title = mailHooks.GetValueOrDefault("[MAIL_TITLE]");
                            var subject = "";
                            if (!string.IsNullOrEmpty(title))
                            {
                                subject = ConvertToCorrectEncoding(title);
                            }

                            var body = outputHtml;

                            var toEmail = detail.EmailSupplier;
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
                                    // cap nhat lai don hang nay
                                    using (IDbContext context = new IDbContext())
                                    {
                                        var orderDetail = context.OrderDetail.Where(r => r.Id == detail.OrderDetailId).FirstOrDefault();
                                        if (orderDetail != null)
                                        {
                                            orderDetail.SupplierStatus = "DA_GUI_EMAIL_YEU_CAU";
                                            context.OrderDetail.Update(orderDetail);
                                            context.SaveChanges();
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    // Handle exception (e.g., log the error)
                                    Console.WriteLine($"ERROR: {ex.Message}");
                                    return false;
                                }
                            }

                            //using (var client = new SmtpClient(smtpServer, smtpPort))
                            //{
                            //    client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                            //    client.EnableSsl = false;

                            //    var mailMessage = new MailMessage
                            //    {
                            //        From = new MailAddress(smtpUser),
                            //        Subject = subject,
                            //        Body = body,
                            //        IsBodyHtml = true
                            //    };

                            //    mailMessage.To.Add(toEmail);

                            //    client.Send(mailMessage);

                            //    // cap nhat lai don hang nay
                            //    using (IDbContext context = new IDbContext())
                            //    {
                            //        var orderDetail = context.OrderDetail.Where(r => r.Id == detail.OrderDetailId).FirstOrDefault();
                            //        if(orderDetail != null)
                            //        {
                            //            orderDetail.SupplierStatus = "DA_GUI_EMAIL_YEU_CAU";
                            //            context.OrderDetail.Update(orderDetail);
                            //            context.SaveChanges();
                            //        }
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                        }

                    }

                }

            }
            return true;

        }

        public List<ResponseGetOrderItemFullDetailForSupplier> GetOrderItemFullDetailPedningForSupplier()
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetOrderDetailFullInfomationForSupplier";

            p.Add("@lang_code", "vi-VN");
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ResponseGetOrderItemFullDetailForSupplier>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public List<ResponseGetCouponByProductId> GetCouponByProductId(RequestGetCouponByProductId request)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetCouponCodeByProductId";

            p.Add("@productId", request.productId);
            p.Add("@culture_code", request.culture_code);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ResponseGetCouponByProductId>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            return result;
        }

        public string GenerateOrderCode()
        {
            using (IDbContext context = new IDbContext())
            {
                var countOrder = context.Orders.Count();
                var orderCode = CreateOrderCode(countOrder++);
                return orderCode;
            }
        }

        public ResponseGetCouponByProductId CheckCouponCode(RequestCheckCouponCode request)
        {

            var p = new DynamicParameters();
            var commandText = "usp_Web_GetCouponCodeByProductIdAndCouponCode_version_by_sku_with_remail";

            p.Add("@productId", request.productId);
            p.Add("@culture_code", request.culture_code);
            p.Add("@couponCode", request.couponCode);
            p.Add("@customerEmail", request.customerEmail);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.QueryFirst<ResponseGetCouponByProductId>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            return result;
        }

        public async Task<bool> SendNewOrderEmailToHelpDesk(RequestSendNewOrderEmail request)
        {
            var wwwrootPath = _hostingEnvironment.WebRootPath;
            var tempFile = "";
            if (request.activeStatus.Equals("TAO_MOI"))
            {
                tempFile = "mail-thanh-toan-don-hang.html";
            }
            if (request.activeStatus.Equals("CHAP_NHAN_DICH_VU"))
            {
                tempFile = "mail-e-ticket.html";
            }
            var templatePath = Path.Combine(wwwrootPath, "mail-templates", tempFile);
            if (File.Exists(templatePath))
            {
                var templateString = ReadTemplateFromFile(templatePath);
                var orderDetailsId = new List<int>();
                using (IDbContext context = new IDbContext())
                {
                    var order = await context.Orders.Where(r => r.OrderCode.Equals(request.orderCode)).LastOrDefaultAsync(); // Tại sao lại có trường hợp trùng đơn hàng nhỉ
                    if (order != null)
                    {
                        orderDetailsId = context.OrderDetail.Where(r => r.OrderId == order.Id).Select(r => r.Id).ToList();
                    }
                }
                //Lay cultureCode
                
                var mailInfo = _bannerAdsRepository.GetBannerAds_By_Code(request.culture_code, "MAIL_CULTURE_NEW_ORDER");
                
                foreach (var item in orderDetailsId)
                {

                    var requestGetOrderItemFullDetail = new RequestGetOrderItemFullDetail();
                    requestGetOrderItemFullDetail.customerId = request.customerId;
                    requestGetOrderItemFullDetail.orderCode = request.orderCode;
                    requestGetOrderItemFullDetail.orderDetailId = item.ToString();
                    requestGetOrderItemFullDetail.cultureCode = request.culture_code;



                    //Goi db
                    var detail = await this.GetOrderItemFullDetail(requestGetOrderItemFullDetail);
                    if (detail != null)
                    {
                        Dictionary<string, string> mailHooks = new Dictionary<string, string>();
                        if (mailInfo != null)
                        {
                            var banners = WebHelper.ConvertSlide(mailInfo);
                            if (banners != null)
                            {

                                foreach (var b in banners)
                                {
                                    if (!string.IsNullOrEmpty(b.Title))
                                    {
                                        mailHooks.Add($"[{b.Title}]", WebHelper.GetCultureText(banners, b.Title));
                                    }
                                }

                            }

                        }
                        var metadata = Newtonsoft.Json.JsonConvert.DeserializeObject<OrderDetailMetaData>(detail.MetaData);
                        mailHooks.Add("[CUSTOMER_FULLNAME]", request.customerName);
                        mailHooks.Add("[DATA_FULL_NAME]", request.customerName);
                        mailHooks.Add("[DATA_MA_DON_HANG]", detail.OrderCode);
                        mailHooks.Add("[DATA_TEN_PACKAGE]", detail.ProductChildTitle);
                        mailHooks.Add("[DATA_TEN_FULL_OPTION]", detail.ZoneTitles);
                        mailHooks.Add("[DATA_TEN_SAN_PHAM]", detail.ProductParentTitle);
                        mailHooks.Add("[MAIL_NOI_DUNG_SAN_PHAM_AVATAR]", UIHelper.StoreFilePath(detail.ProductParentAvatar));
                        mailHooks.Add("[DATA_NGAY_DAT_DICH_VU]", detail.CreatedDate.ToString("dd/MM/yyyy HH:mm:ss"));
                        mailHooks.Add("[DATA_NGAY_SU_DUNG]", metadata.choosenDate);


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
                        if (request.orderNotes != null)
                        {
                            if (!string.IsNullOrEmpty(request.orderNotes.noteSpecial))
                            {
                                mailHooks.Add("[DATA_GIA_NOTE]", $"{request.orderNotes.noteSpecial}");
                            }
                        }
                        var outputHtml = ReplacePlaceholders(templateString, mailHooks);
                        if (!string.IsNullOrEmpty(outputHtml))
                        {

                            //Doc HTML o day de tim den the div co class la `booking-note-wrapper`

                            //Sau do add them the h1 HELLO WORD vao
                            // Load the HTML string into an HtmlDocument
                            var htmlDoc = new HtmlDocument();
                            htmlDoc.LoadHtml(outputHtml);

                            // Find the div with class 'booking-note-wrapper'
                            var bookingNoteDiv = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'booking-note-wrapper')]");
                            if (bookingNoteDiv != null)
                            {
                                //Lấy ra tất cả options của note ở đay
                                foreach (var noteGroup in metadata.productBookingNoteGroups)
                                {
                                    foreach (var note in noteGroup.NoteList)
                                    {
                                        if (note.bookingNoteSendWithMail)
                                        {
                                            var noteLabel = note.ZoneName;
                                            var noteValue = note.noteValue;
                                            if (!string.IsNullOrEmpty(noteValue))
                                            {
                                                string htmlContent = $@"
                                                    <table class=""paragraph_block block-3"" width=""100%"" border=""0"" cellpadding=""0"" cellspacing=""0"" role=""presentation"" style=""mso-table-lspace: 0pt; mso-table-rspace: 0pt; word-break: break-word;"">
                                                      <tr>
                                                        <td class=""pad"" style=""padding-bottom:10px;padding-left:25px;padding-right:10px;"">
                                                          <div style=""color:#122f50;font-family:'Montserrat', 'Trebuchet MS', 'Lucida Grande', 'Lucida Sans Unicode', 'Lucida Sans', Tahoma, sans-serif;font-size:14px;line-height:120%;text-align:left;mso-line-height-alt:16.8px;"">
                                                            <p style=""margin: 0; word-break: break-word;"">{noteLabel}: {noteValue}</p>
                                                          </div>
                                                        </td>
                                                      </tr>
                                                    </table>";

                                                //Sau đó append cái này vào bookingNoteDiv ở trên
                                                var newNode = HtmlNode.CreateNode(htmlContent);
                                                bookingNoteDiv.AppendChild(newNode);
                                            }
                                        }

                                    }
                                }
                                outputHtml = htmlDoc.DocumentNode.OuterHtml;
                            }


                            var smtpServer = _configuration["EmailSender:Host"];
                            int smtpPort = int.Parse(_configuration["EmailSender:Port"]);
                            var smtpUser = _configuration["EmailSender:BookingService:Email"]; // cs@joytime.vn
                            var smtpPass = _configuration["EmailSender:BookingService:Password"]; //D2A9HnvGMJYW3BeKQw5f4F
                            var title = mailHooks.GetValueOrDefault("[MAIL_TITLE]");
                            var subject = "";
                            if (!string.IsNullOrEmpty(title))
                            {
                                subject = ConvertToCorrectEncoding(title);
                            }

                            var body = outputHtml;

                            var toEmail = "helpdesk@joytime.vn";
                            var helpDeskGmail = "joytime234@gmail.com";

                            var message = new MimeMessage();
                            message.From.Add(new MailboxAddress(smtpUser, smtpUser));
                            message.To.Add(new MailboxAddress(toEmail, toEmail));
                            message.To.Add(new MailboxAddress(helpDeskGmail, helpDeskGmail));
                            message.Subject = subject;

                            var bodyBuilder = new BodyBuilder { HtmlBody = body };
                            message.Body = bodyBuilder.ToMessageBody();

                            using (var client = new MailKit.Net.Smtp.SmtpClient())
                            {
                                try
                                {
                                    await client.ConnectAsync(smtpServer, smtpPort, true);
                                    await client.AuthenticateAsync(smtpUser, smtpPass);
                                    await client.SendAsync(message);
                                    await client.DisconnectAsync(true);
                                }
                                catch (Exception ex)
                                {
                                    // Handle exception (e.g., log the error)
                                    Console.WriteLine($"ERROR: {ex.Message}");
                                    return false;
                                }
                            }
                            //using (var client = new SmtpClient(smtpServer, smtpPort))
                            //{
                            //    client.Credentials = new NetworkCredential(smtpUser, smtpPass);
                            //    client.EnableSsl = false;

                            //    var mailMessage = new MailMessage
                            //    {
                            //        From = new MailAddress(smtpUser),
                            //        Subject = subject,
                            //        Body = body,
                            //        IsBodyHtml = true
                            //    };

                            //    mailMessage.To.Add(toEmail);

                            //    await client.SendMailAsync(mailMessage);
                            //    return true;
                            //}
                        }


                    }


                }
                return true;
            }
            return false;
        }

        public async Task<OrderDetailFeedback> UpdateOrderDetailCommentWithRating(RequestUpdateOrderDetailCommentWithRating request)
        {
            using (IDbContext context = new IDbContext())
            {
                if (request != null)
                {
                    if (request.OrderDetailId > 0)
                    {
                        var orderDetail = await context.OrderDetail.FindAsync(request.OrderDetailId);
                        if (orderDetail != null)
                        {
                            var feedBack = new OrderDetailFeedback();
                            if (request.Id > 0)
                            {
                                //Tien hanh Update
                                feedBack = await context.OrderDetailFeedback.FindAsync(request.Id);
                                if (feedBack != null)
                                {
                                    feedBack.TitleComment = request.TitleComment;
                                    feedBack.ContentComment = request.ContentComment;
                                    feedBack.CreatedDate = DateTime.Now;
                                    feedBack.OrderDetailId = orderDetail.Id;
                                    feedBack.Rating = request.Rating;

                                    var fileUpload = new List<string>();
                                    foreach (var item in request.OldFileUpload)
                                    {
                                        fileUpload.Add(item);
                                    }
                                    foreach (var item in request.NewFileUpload)
                                    {
                                        var filePath = SaveFeedbackImage(item, orderDetail.Id);
                                        fileUpload.Add(filePath);
                                    }

                                    feedBack.FileUpload = string.Join(",", fileUpload);

                                    context.OrderDetailFeedback.Update(feedBack);
                                    await context.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                //Tien hanh them moi

                                feedBack.TitleComment = request.TitleComment;
                                feedBack.ContentComment = request.ContentComment;
                                feedBack.CreatedDate = DateTime.Now;
                                feedBack.OrderDetailId = orderDetail.Id;
                                feedBack.Rating = request.Rating;
                                var fileUpload = new List<string>();
                                foreach (var item in request.OldFileUpload)
                                {
                                    fileUpload.Add(item);
                                }
                                foreach (var item in request.NewFileUpload)
                                {
                                    var filePath = SaveFeedbackImage(item, orderDetail.Id);
                                    fileUpload.Add(filePath);
                                }

                                feedBack.FileUpload = string.Join(",", fileUpload);

                                await context.OrderDetailFeedback.AddAsync(feedBack);
                                await context.SaveChangesAsync();

                            }
                            return feedBack;
                        }
                    }
                }
            }
            return null;
        }

        private string SaveFeedbackImage(string base64Image, int orderDetailId)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
            {
                throw new ArgumentException("Base64 image string is null or empty.", nameof(base64Image));
            }

            // Remove data:image/jpeg;base64, prefix if it exists
            var base64Data = base64Image.Contains(",") ? base64Image.Split(',')[1] : base64Image;

            byte[] imageBytes;
            try
            {
                imageBytes = Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid Base64 string format.", nameof(base64Image));
            }

            string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "feedback", $"ODT-{orderDetailId}");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath); // Ensure the directory exists
            }
            string fileName = $"{DateTime.Now.Ticks}.jpg";
            string filePath = Path.Combine(directoryPath, fileName);

            File.WriteAllBytes(filePath, imageBytes);

            // Return the relative path that can be used to access the image from the web
            return $"/feedback/ODT-{orderDetailId}/{fileName}";
        }

        public async Task<string> UpdateAvatarCustomer(string base64Image, string emailCustomer)
        {
            using (IDbContext context = new IDbContext())
            {
                var customer = await context.Customer.FirstOrDefaultAsync(c => c.Email == emailCustomer);
                if (customer == null)
                {
                    return "";
                }
                var avatarPath = ConvertBase64ToAvatar(base64Image);

                customer.Avatar = avatarPath;

                context.Customer.Update(customer);
                await context.SaveChangesAsync();

                return avatarPath;
            }
        }


        private string ConvertBase64ToAvatar(string base64Image)
        {
            if (string.IsNullOrWhiteSpace(base64Image))
            {
                throw new ArgumentNullException(nameof(base64Image));
            }

            // Remove data:image/jpeg;base64, prefix if it exists
            var base64Data = base64Image.Contains(",") ? base64Image.Split(',')[1] : base64Image;

            byte[] imageBytes;
            try
            {
                imageBytes = Convert.FromBase64String(base64Data);
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid Base64 string format.", nameof(base64Image));
            }

            string directoryPath = Path.Combine(_hostingEnvironment.WebRootPath, "avatar");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            string fileName = $"{DateTime.Now.Ticks}.jpg";
            string filePath = Path.Combine(directoryPath, fileName);

            File.WriteAllBytes(filePath, imageBytes);

            // Return the relative path that can be used to access the image from the web
            return $"/avatar/{fileName}";
        }

    public async Task<ResponseOrderDetailCommentWithRating> GetOrderDetailCommentWithRating(int orderDetailId)
        {
            var response = new ResponseOrderDetailCommentWithRating();
            if (orderDetailId > 0)
            {
                using (IDbContext context = new IDbContext())
                {
                    var orderDetailFeedback = await context.OrderDetailFeedback.Where(r => r.OrderDetailId == orderDetailId).FirstOrDefaultAsync();
                    if (orderDetailFeedback != null)
                    {
                        response.Id = orderDetailFeedback.Id;
                        response.TitleComment = orderDetailFeedback.TitleComment;
                        response.ContentComment = orderDetailFeedback.ContentComment;
                        response.Rating = orderDetailFeedback.Rating;
                        response.OldFileUpload = orderDetailFeedback.FileUpload.Split(",").ToList();
                        response.OrderDetailId = orderDetailFeedback.OrderDetailId;
                    }
                }
            }
            return response;

        }

        public int EmailSubscriptionRegistration(string email)
        {
            using (IDbContext context = new IDbContext())
            {
                var p = new DynamicParameters();
                var commandText = "usp_web_InsertEmailSubcriber";
                p.Add("@email", email);
                var result = _executers.ExecuteCommand(_connStr, conn => conn.Execute(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
                return result;
            }
        }


        public List<ResponseGetLastChatDetailBySessionForCustomer> ResponseGetLastChatDetailBySessionForCustomer()
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_GetLastChatDetailBySession";

            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ResponseGetLastChatDetailBySessionForCustomer>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));

            if (result != null)
            {
                var wwwrootPath = _hostingEnvironment.WebRootPath;
                var tempFile = "mail-noti-new-message-customer.html";
                var templatePath = Path.Combine(wwwrootPath, "mail-templates", tempFile);
                if (File.Exists(templatePath))
                {
                    var listDone = new List<int>();
                    foreach (var item in result)
                    {
                        //Lay cultureCode
                        var templateString = ReadTemplateFromFile(templatePath);
                        Dictionary<string, string> mailHooks = new Dictionary<string, string>();
                        var mailInfo = _bannerAdsRepository.GetBannerAds_By_Code(item.DefaultLanguage, "MAIL_CULTURE_NEW_MESSAGE");
                        if (mailInfo != null)
                        {
                            var banners = WebHelper.ConvertSlide(mailInfo);
                            if (banners != null)
                            {

                                foreach (var b in banners)
                                {

                                    if (!string.IsNullOrEmpty(b.Title))
                                    {
                                        mailHooks.Add($"[{b.Title}]", WebHelper.GetCultureText(banners, b.Title));


                                    }
                                }
                                mailHooks.Add("[DATA_MA_DON_HANG]", item.OrderCode);
                                mailHooks.Add("[DATA_LOI_NHAN]", item.Content);
                                mailHooks.Add("[MAIL_NOI_DUNG_SAN_PHAM_AVATAR]", $"{UIHelper.StoreFilePath(item.Avatar)}");
                                mailHooks.Add("[DATA_FULL_NAME]", item.Fullname);
                                mailHooks.Add("[CUSTOMER_FULLNAME]", item.Fullname);
                                var outputHtml = ReplacePlaceholders(templateString, mailHooks);
                                if (!string.IsNullOrEmpty(outputHtml))
                                {
                                    var smtpServer = _configuration["EmailSender:Host"];
                                    int smtpPort = int.Parse(_configuration["EmailSender:Port"]);
                                    var smtpUser = _configuration["EmailSender:BookingService:Email"]; // cs@joytime.vn
                                    var smtpPass = _configuration["EmailSender:BookingService:Password"]; //D2A9HnvGMJYW3BeKQw5f4F
                                    var title = mailHooks.GetValueOrDefault("[MAIL_TITLE]");
                                    var subject = "";
                                    if (!string.IsNullOrEmpty(title))
                                    {
                                        subject = ConvertToCorrectEncoding(title);
                                    }

                                    var body = outputHtml;

                                    var toEmail = item.CustomerEmail;

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
                                            listDone.Add(item.OrderChatSessionDetailId);
                                            //Cap nhat lai OrderSessionDetailId
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
                    using (IDbContext context = new IDbContext())
                    {
                        var affected = context.OrderChatSessionDetail.Where(r => listDone.Contains(r.Id));
                        foreach (var item in affected)
                        {
                            item.IsNotiCustomer = true;

                        }
                        context.OrderChatSessionDetail.UpdateRange(affected);
                        context.SaveChanges();
                    }
                    return result.ToList();
                }

            }
            return null;
        }

        public List<ResponseGetLastChatDetailBySessionForCustomer> CheckChatSessionByCustomerEmail(RequestCheckChatSessionByCustomerEmail request)
        {
            var p = new DynamicParameters();
            var commandText = "usp_Web_CheckChatSessionByCustomerEmail";
            p.Add("@customerEmail", request.customerEmail);

            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ResponseGetLastChatDetailBySessionForCustomer>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
            return result.ToList();
        }

        public List<ResponseOrderNotificationFeedback> GetOrderNotificationFeedback()
        {
            if (false)
            {
                var p = new DynamicParameters();
                var commandText = "usp_Web_GetOrderNotiFeedback";

                var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ResponseOrderNotificationFeedback>(commandText, p, commandType: System.Data.CommandType.StoredProcedure));
                if (result != null)
                {
                    var wwwrootPath = _hostingEnvironment.WebRootPath;
                    var tempFile = "mail-noti-feedback.html";
                    var templatePath = Path.Combine(wwwrootPath, "mail-templates", tempFile);
                    if (File.Exists(templatePath))
                    {
                        var listDone = new List<int>();
                        foreach (var item in result)
                        {
                            //Lay cultureCode
                            var templateString = ReadTemplateFromFile(templatePath);
                            Dictionary<string, string> mailHooks = new Dictionary<string, string>();
                            var mailInfo = _bannerAdsRepository.GetBannerAds_By_Code(item.DefaultLanguage, "MAIL_CULTURE_NOTI_FEEDBACK");
                            if (mailInfo != null)
                            {
                                var banners = WebHelper.ConvertSlide(mailInfo);
                                if (banners != null)
                                {

                                    foreach (var b in banners)
                                    {

                                        if (!string.IsNullOrEmpty(b.Title))
                                        {
                                            mailHooks.Add($"[{b.Title}]", WebHelper.GetCultureText(banners, b.Title));


                                        }
                                    }
                                    mailHooks.Add("[DATA_MA_DON_HANG]", item.OrderCode);
                                    mailHooks.Add("[DATA_URL_LICH_SU_DON_HANG]", $"{_configuration["MainUrl"]}/users/order/{item.OrderCode}/{item.OrderDetailId}");
                                    //DATA_TEN_SAN_PHAM
                                    mailHooks.Add("[DATA_TEN_SAN_PHAM]", item.Title);
                                    mailHooks.Add("[DATA_NGAY_SU_DUNG]", item.PickingDate.ToString("dd/MM/yyyy"));
                                    mailHooks.Add("[MAIL_NOI_DUNG_SAN_PHAM_AVATAR]", $"{UIHelper.StoreFilePath(item.Avatar)}");
                                    mailHooks.Add("[DATA_FULL_NAME]", item.Fullname);
                                    mailHooks.Add("[CUSTOMER_FULLNAME]", item.Fullname);
                                    var outputHtml = ReplacePlaceholders(templateString, mailHooks);
                                    if (!string.IsNullOrEmpty(outputHtml))
                                    {
                                        var smtpServer = _configuration["EmailSender:Host"];
                                        int smtpPort = int.Parse(_configuration["EmailSender:Port"]);
                                        var smtpUser = _configuration["EmailSender:BookingService:Email"]; // cs@joytime.vn
                                        var smtpPass = _configuration["EmailSender:BookingService:Password"]; //D2A9HnvGMJYW3BeKQw5f4F
                                        var title = mailHooks.GetValueOrDefault("[MAIL_TITLE]");
                                        var subject = "";
                                        if (!string.IsNullOrEmpty(title))
                                        {
                                            subject = ConvertToCorrectEncoding(title);
                                        }

                                        var body = outputHtml;

                                        var toEmail = item.Email;

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
                                                listDone.Add(item.OrderDetailId);
                                                //Cap nhat lai OrderSessionDetailId
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
                        //using (IDbContext context = new IDbContext())
                        //{
                        //    var affected = context.OrderChatSessionDetail.Where(r => listDone.Contains(r.Id));
                        //    foreach (var item in affected)
                        //    {
                        //        item.IsNotiCustomer = true;

                        //    }
                        //    context.OrderChatSessionDetail.UpdateRange(affected);
                        //    context.SaveChanges();
                        //}
                        return result.ToList();
                    }

                }
            }
            
            return null;
        }

        public ResponseCheckOrderDetailByEmail CheckOrderDetailByEmail(RequestCheckOrderDetailByEmail request)
        {
            //usp_Web_CheckOrderDetailByEmail
            var p = new DynamicParameters();
            var commandText = "usp_Web_CheckOrderDetailByEmail";

            p.Add("@customerEmail", request.customerEmail);
            p.Add("@orderDetailId", request.orderDetailId);
            var result = _executers.ExecuteCommand(_connStr, conn => conn.Query<ResponseGetCouponByProductId>(commandText, p, commandType: System.Data.CommandType.StoredProcedure)).ToList();
            var response = new ResponseCheckOrderDetailByEmail();
            if (result.Count > 0)
            {
                response.isAuthendicated = true;
            }
            else
            {
                response.isAuthendicated = false;
            }

            return response;
        }
    }


    public class CouPonDetail
    {
        public string Name { get; set; }
        public int DiscountOption { get; set; }
        public string ValueDiscount { get; set; }
    }
}
