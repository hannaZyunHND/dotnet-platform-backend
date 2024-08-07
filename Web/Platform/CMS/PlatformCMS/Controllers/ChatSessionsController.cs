using MI.Bo.Bussiness;
using MI.Dal.IDbContext;
using MI.Entity.Enums;
using MI.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlatformCMS.Extensions;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatSessionsController : ControllerBase
    {
        OrdersBCL ordersBCL;
        OrderDetailBCL orderDetailBCL;
        ProductBCL productBCL;
        PromotionBCL promotionBCL;
        private readonly string _connectionString;
        private readonly IHostingEnvironment _enviroment;
        private readonly IConfiguration _configuration;
        public ChatSessionsController(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            ordersBCL = new OrdersBCL();
            orderDetailBCL = new OrderDetailBCL();
            productBCL = new ProductBCL();
            promotionBCL = new PromotionBCL();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _enviroment = environment;
        }
        [HttpPost]
        [Route("GetChatSessionByOrderDetailId")]
        [AllowAnonymous]
        public async Task<IActionResult> GetChatSessionByOrderDetailId(RequestGetChatSessionDetailId request)
        {
            var currentUserId = User.GetUserId();
            var seenUserType = "";
            if(currentUserId == null)
            {
                //Day la customer
                //
                seenUserType = "CUSTOMER";
            }
            else
            {
                var roles = User.GetRoleName();
                if (roles.Contains("Supplier"))
                {
                    seenUserType = "SUPPLIER";
                }
                else
                {
                    seenUserType = "ADMINISTRATOR";
                }
            }
            if (request != null)
            {
                if(request.orderDetailId > 0)
                {
                    using (IDbContext context = new IDbContext())
                    {
                        var chatSession = context.OrderChatSession.Where(r => r.OrderDetailId == request.orderDetailId).FirstOrDefault();
                        if(chatSession == null)
                        {
                            chatSession = new MI.Entity.Models.OrderChatSession();
                            chatSession.OrderDetailId = request.orderDetailId;
                            chatSession.RoomTickName = DateTime.Now.Ticks.ToString();
                            chatSession.CreatedDate = DateTime.Now;
                            await context.OrderChatSession.AddAsync(chatSession);
                            await context.SaveChangesAsync();
                            //return Ok(chatSession);
                        }
                        if(chatSession != null)
                        {
                            var sessionDetail = context.OrderChatSessionDetail.Where(r => r.OrderChatSessionId == chatSession.Id).ToList();

                            if(sessionDetail != null)
                            {
                                switch (seenUserType)
                                {
                                    case "CUSTOMER":
                                        foreach(var item in sessionDetail)
                                        {
                                            item.isSeen = true;
                                        }
                                        context.OrderChatSessionDetail.UpdateRange(sessionDetail);
                                        await context.SaveChangesAsync();
                                        break;
                                    case "SUPPLIER":
                                        foreach (var item in sessionDetail)
                                        {
                                            item.isSupplierSeen = true;
                                        }
                                        context.OrderChatSessionDetail.UpdateRange(sessionDetail);
                                        await context.SaveChangesAsync();
                                        break;
                                    case "ADMINISTRATOR":
                                        foreach (var item in sessionDetail)
                                        {
                                            item.isAdministrationSeen = true;
                                        }
                                        context.OrderChatSessionDetail.UpdateRange(sessionDetail);
                                        await context.SaveChangesAsync();
                                        break;
                                }
                            }




                            dynamic response = new ExpandoObject();
                            sessionDetail = sessionDetail.OrderBy(r => r.CreatedDate).ToList();
                            response.id = chatSession.Id;
                            response.roomTickName = chatSession.RoomTickName;
                            response.createdDate = chatSession.CreatedDate;
                            response.orderDetailId = chatSession.OrderDetailId;
                            response.sessionDetails  = sessionDetail;
                            return Ok(response);
                        }
                    }
                }
                
            }
            return BadRequest();
            
        }

        [HttpPost]
        [Route("GetChatSessionByOrderDetailIdByCustomer")]
        [AllowAnonymous]
        public async Task<IActionResult> GetChatSessionByOrderDetailIdByCustomer(RequestGetChatSessionDetailId request)
        {
            var seenUserType = "CUSTOMER";
            if (request != null)
            {
                if (request.orderDetailId > 0)
                {
                    using (IDbContext context = new IDbContext())
                    {
                        var chatSession = context.OrderChatSession.Where(r => r.OrderDetailId == request.orderDetailId).FirstOrDefault();
                        if (chatSession == null)
                        {
                            chatSession = new MI.Entity.Models.OrderChatSession();
                            chatSession.OrderDetailId = request.orderDetailId;
                            chatSession.RoomTickName = DateTime.Now.Ticks.ToString();
                            chatSession.CreatedDate = DateTime.Now;
                            await context.OrderChatSession.AddAsync(chatSession);
                            await context.SaveChangesAsync();
                            //return Ok(chatSession);
                        }
                        if (chatSession != null)
                        {
                            var sessionDetail = context.OrderChatSessionDetail.Where(r => r.OrderChatSessionId == chatSession.Id).ToList();

                            if (sessionDetail != null)
                            {
                                switch (seenUserType)
                                {
                                    case "CUSTOMER":
                                        foreach (var item in sessionDetail)
                                        {
                                            item.isSeen = true;
                                        }
                                        context.OrderChatSessionDetail.UpdateRange(sessionDetail);
                                        await context.SaveChangesAsync();
                                        break;
                                    case "SUPPLIER":
                                        foreach (var item in sessionDetail)
                                        {
                                            item.isSupplierSeen = true;
                                        }
                                        context.OrderChatSessionDetail.UpdateRange(sessionDetail);
                                        await context.SaveChangesAsync();
                                        break;
                                    case "ADMINISTRATOR":
                                        foreach (var item in sessionDetail)
                                        {
                                            item.isAdministrationSeen = true;
                                        }
                                        context.OrderChatSessionDetail.UpdateRange(sessionDetail);
                                        await context.SaveChangesAsync();
                                        break;
                                }
                            }




                            dynamic response = new ExpandoObject();
                            sessionDetail = sessionDetail.OrderBy(r => r.CreatedDate).ToList();
                            response.id = chatSession.Id;
                            response.roomTickName = chatSession.RoomTickName;
                            response.createdDate = chatSession.CreatedDate;
                            response.orderDetailId = chatSession.OrderDetailId;
                            response.sessionDetails = sessionDetail;
                            return Ok(response);
                        }
                    }
                }

            }
            return BadRequest();

        }
        [HttpPost]
        [Route("SendChatSessionMessageByCMSUser")]
        
        public async Task<IActionResult> SendChatSessionMessageByCMSUser(SendChatSessionMessageByCMSUser request)
        {
            var currentUserId = User.GetUserId();
            var roles = User.GetRoleName();
            var userEmail = "";
            var userType = 0;
            using (IDbContext context = new IDbContext())
            {
                var user = await context.AspNetUsers.FindAsync(currentUserId);
                if (user != null)
                {
                    userEmail = user.Email;
                }
                if (roles.Contains("Supplier"))
                {
                    userType = (int)OrderChatSessionSenderType.SUPPLIER;
                }
                else
                {
                    userType = (int)OrderChatSessionSenderType.ADMINSTRATOR;
                }
            }
            if(request.OrderChatSessionId > 0 && (request.Images.Count > 0 || !string.IsNullOrEmpty(request.Messages)))
            {
                using (IDbContext context = new IDbContext())
                {
                    var messages = new List<OrderChatSessionDetail>();
                    if (!string.IsNullOrEmpty(request.Messages))
                    {
                        var chatMessage = new OrderChatSessionDetail();
                        chatMessage.OrderChatSessionId = request.OrderChatSessionId;
                        chatMessage.Sender = userEmail;
                        chatMessage.CreatedDate = DateTime.Now;
                        chatMessage.Content = request.Messages;
                        chatMessage.ContentType = (int)OrderChatSessionContentType.CONTENT;
                        chatMessage.isSeen = false;
                        if(userType == (int)OrderChatSessionSenderType.SUPPLIER)
                        {
                            chatMessage.isSupplierSeen = true;
                        }
                        if (userType == (int)OrderChatSessionSenderType.ADMINSTRATOR)
                        {
                            chatMessage.isAdministrationSeen = true;
                        }
                        chatMessage.SenderType = userType;
                        
                        messages.Add(chatMessage);
                    }
                    if(request.Images.Count > 0)
                    {

                        foreach(var item in request.Images)
                        {
                            try
                            {
                                var chatMessage = new OrderChatSessionDetail();
                                chatMessage.OrderChatSessionId = request.OrderChatSessionId;
                                chatMessage.Sender = userEmail;
                                chatMessage.CreatedDate = DateTime.Now;
                                chatMessage.Content = SaveFeedbackImage(item, request.OrderChatSessionId);
                                chatMessage.ContentType = (int)OrderChatSessionContentType.IMAGE;
                                chatMessage.isSeen = false;
                                if (userType == (int)OrderChatSessionSenderType.SUPPLIER)
                                {
                                    chatMessage.isSupplierSeen = true;
                                }
                                if (userType == (int)OrderChatSessionSenderType.ADMINSTRATOR)
                                {
                                    chatMessage.isAdministrationSeen = true;
                                }
                                chatMessage.SenderType = userType;
                                messages.Add(chatMessage);
                            }
                            catch (Exception ex)
                            {

                                Console.WriteLine(ex.Message);
                            }
                            
                        }
                    }
                    await context.OrderChatSessionDetail.AddRangeAsync(messages);
                    await context.SaveChangesAsync();

                    //Gui Email nhac thang khách là mày có một tin nhắn mới

                    return Ok(messages);
                    
                    //Kiem tra neu co loi nhan thi them loi nhan
                }
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("SendChatSessionMessageByCustomerUser")]
        [AllowAnonymous]
        public async Task<IActionResult> SendChatSessionMessageByCustomerUser(SendChatSessionMessageByCustomerUser request)
        {
            
            var userEmail = "";
            var userType = 0;
            using (IDbContext context = new IDbContext())
            {
                userEmail = (from c in context.OrderChatSession
                             join o in context.OrderDetail on c.OrderDetailId equals o.Id
                             join od in context.Orders on o.OrderId equals od.Id
                             join cus in context.Customer on od.CustomerId equals cus.Id
                             select cus.Email).FirstOrDefault();
            }
            if (request.OrderChatSessionId > 0 && !string.IsNullOrEmpty(userEmail) && (request.Images.Count > 0 || !string.IsNullOrEmpty(request.Messages)))
            {
                using (IDbContext context = new IDbContext())
                {
                    var messages = new List<OrderChatSessionDetail>();
                    if (!string.IsNullOrEmpty(request.Messages))
                    {
                        var chatMessage = new OrderChatSessionDetail();
                        chatMessage.OrderChatSessionId = request.OrderChatSessionId;
                        chatMessage.Sender = userEmail;
                        chatMessage.CreatedDate = DateTime.Now;
                        chatMessage.Content = request.Messages;
                        chatMessage.ContentType = (int)OrderChatSessionContentType.CONTENT;
                        chatMessage.isSeen = true;
                        chatMessage.isSupplierSeen = false;
                        chatMessage.isAdministrationSeen = false;
                        
                        chatMessage.SenderType = (int)OrderChatSessionSenderType.CUSTOMER;

                        messages.Add(chatMessage);
                    }
                    if (request.Images.Count > 0)
                    {

                        foreach (var item in request.Images)
                        {
                            try
                            {
                                var chatMessage = new OrderChatSessionDetail();
                                chatMessage.OrderChatSessionId = request.OrderChatSessionId;
                                chatMessage.Sender = userEmail;
                                chatMessage.CreatedDate = DateTime.Now;
                                chatMessage.Content = SaveFeedbackImage(item, request.OrderChatSessionId);
                                chatMessage.ContentType = (int)OrderChatSessionContentType.IMAGE;
                                chatMessage.isSeen = true;
                                chatMessage.isSupplierSeen = false;
                                chatMessage.isAdministrationSeen = false;

                                chatMessage.SenderType = (int)OrderChatSessionSenderType.CUSTOMER;
                                messages.Add(chatMessage);
                            }
                            catch (Exception ex)
                            {

                                Console.WriteLine(ex.Message);
                            }

                        }
                    }
                    await context.OrderChatSessionDetail.AddRangeAsync(messages);
                    await context.SaveChangesAsync();

                    //Gui Email nhac thang khách là mày có một tin nhắn mới

                    return Ok(messages);

                    //Kiem tra neu co loi nhan thi them loi nhan
                }
            }
            return BadRequest();
        }
        private string SaveFeedbackImage(string base64Image, int orderChatSessionId)
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

            string directoryPath = Path.Combine(_enviroment.WebRootPath, "chat_session", $"ODT-{orderChatSessionId}");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath); // Ensure the directory exists
            }
            string fileName = $"{DateTime.Now.Ticks}.jpg";
            string filePath = Path.Combine(directoryPath, fileName);

            System.IO.File.WriteAllBytes(filePath, imageBytes);

            // Return the relative path that can be used to access the image from the web
            return $"/chat_session/ODT-{orderChatSessionId}/{fileName}";
        }

    }

    public class RequestGetChatSessionDetailId
    {
        public int orderDetailId { get; set; }
    }
    public class SendChatSessionMessageByCMSUser
    {
        public int OrderChatSessionId { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string Messages { get; set; } = "";

    }

    public class SendChatSessionMessageByCustomerUser
    {
        public int OrderChatSessionId { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string Messages { get; set; } = "";
    }
}
