using HtmlAgilityPack;
using MI.Dal.IDbContext;
using MI.Entity.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using PlatformWEBAPI.Services.BannerAds.Repository;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.Order.Repository;
using PlatformWEBAPI.Services.Order.ViewModal;
using PlatformWEBAPI.Services.Product.ViewModel;
using PlatformWEBAPI.Utility;
using System;
using System.Collections.Generic;
//using MimeKit;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using Utils;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IExtraRepository _extraRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IBannerAdsRepository _bannerAdsRepository;

        public AuthController(IOrderRepository orderRepository, IExtraRepository extraRepository, IBannerAdsRepository bannerAdsRepository, IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _orderRepository = orderRepository;
            _extraRepository = extraRepository;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
            _bannerAdsRepository = bannerAdsRepository;
        }
        [HttpPost]
        [Route("DoLogin")]
        public async Task<IActionResult> DoLogin(CustomerAuthViewModel request)
        {
            var result = await _orderRepository.DoLogin(request);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(CustomerAuthViewModel request)
        {
            using (IDbContext context = new IDbContext())
            {
                var user = await context.Customer.FirstOrDefaultAsync(r => r.Email == request.email);
                if (user != null)
                {
                    if(user.Pcname != request.oldPassword){
                        return BadRequest("Password is not corressponding !!");
                    }
                }
                   
            }
            var result = _orderRepository.ChangePassword(request);
            if (result > 0)
            {
                 return Ok(result);
            }
            else
            {
                 return BadRequest();
            }
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(CustomerForGotPassViewModel request)
        {
            if (request == null || string.IsNullOrEmpty(request.email))
            {
                return BadRequest(new ApiResponse
                {
                    Status = "error",
                    Message = "Account not found. Please provide a valid email.",
                    Data = null
                });
            }

            var culture_code =request.culture_code;

            using (IDbContext context = new IDbContext())
            {
                var user = await context.Customer.FirstOrDefaultAsync(r => r.Email == request.email);
                if (user == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = "fail",
                        Message = "Account does not exist. Please check the email again.",
                        Data = null
                    });
                }

                var newPassword = GenerateRandomPassword(8);

                request.password = newPassword;
                var result = _orderRepository.ForgotPassword(request);

                if (result != null)
                {
                    var wwwrootPath = _hostingEnvironment.WebRootPath;
                    var tempFile = "mail-quen-mat-khau.html";

                    var templatePath = Path.Combine(wwwrootPath, "mail-templates", tempFile);
                    if (System.IO.File.Exists(templatePath))
                    {
                        var templateString = ReadTemplateFromFile(templatePath);


                        // Tạo mailHooks và gán giá trị cho các placeholder
                        Dictionary<string, string> mailHooks = new Dictionary<string, string>();

                        var mailInfo = _bannerAdsRepository.GetBannerAds_By_Code(culture_code, "MAIL_CULTURE_FORGOT_PASSWORD");

                        var banners = WebHelper.ConvertSlide(mailInfo);
                        if(mailInfo != null)
                        {
                            if(banners != null)
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

                        mailHooks.Add("[MAIL_TEMP_FORGOT_PASSWORD]", newPassword);
                        mailHooks.Add("[MAIL_USERNAME]", user.Fullname);
                        var outputHtml = ReplacePlaceholders(templateString, mailHooks);
                        if (!string.IsNullOrEmpty(outputHtml))
                        {
                            var smtpServer = _configuration["EmailSender:Host"];
                            int smtpPort = int.Parse(_configuration["EmailSender:Port"]);
                            var smtpUser = _configuration["EmailSender:BookingService:Email"];
                            var smtpPass = _configuration["EmailSender:BookingService:Password"];
                            var title = mailHooks.GetValueOrDefault("[MAIL_SUBJECT]");
                            var subject = "";
                            if (!string.IsNullOrEmpty(title))
                            {
                                subject = ConvertToCorrectEncoding(title);
                            }

                            var body = outputHtml;

                            var toEmail = user.Email;

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
                                    Console.WriteLine($"ERROR: {ex.Message}");
                                    return StatusCode(500, new ApiResponse
                                    {
                                        Status = "error",
                                        Message = "An unexpected error occurred while sending the email.",
                                        Data = null
                                    });
                                }
                            }
                        }


                    }

                }
            }
            return Ok(new ApiResponse
            {
                Status = "success",
                Message = "Password reset successful. Please check your email for the new password.",
                Data = null
            });
        }

        [HttpPost]
        [Route("UpdateForgotPassword")]
        public async Task<IActionResult> UpdateForgotPassword(CustomerForGotPassViewModel request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            using (IDbContext context = new IDbContext())  // Ensure IDbContext implements IAsyncDisposable if using C# 8+
            {
                var user = await context.Customer.FirstOrDefaultAsync(r => r.Email == request.email);
                if (user == null)
                {
                    return BadRequest("User not found !!");
                }

                var result = _orderRepository.ForgotPassword(request);  // Assuming ForgotPassword is an async method
                return Ok(result);
            }

        }

        [HttpPost]
        [Route("DoSignUp")]
        public async Task<IActionResult> DoSignUp(CustomerAuthViewModel request)
        {

            using (IDbContext context = new IDbContext())
            {
                var user = await context.Customer.FirstOrDefaultAsync(r => r.Email == request.email);
                if (user != null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = "fail",
                        Message = "Account is exist. Please check the email again.",
                        Data = null
                    });
                }
            }

            var result = _orderRepository.DoSignUp(request);
            if (result > 0)
            {
                var response = new RequestSendNewUserEmail();
                response.customerEmail = request.email;
                response.username = request.email;
                response.password = request.password;
                var sendMailStatus = _orderRepository.SendNewUserEmail(response);

                return Ok(result);
            }
            else
            {

                return BadRequest(new ApiResponse
                {
                    Status = "error",
                    Message = "Failed to add account",
                    Data = null
                });
            }

        }

        [HttpPost]
        [Route("Subscribers")]
        public async Task<IActionResult> SubscribersToPromotions(SubscribersRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request cannot be null");
            }

            using (IDbContext context = new IDbContext())
            {
                //    var customerExisting = await context.Subscribers.FirstOrDefaultAsync(r => r.Email == request.Email);
                //    if (customerExisting != null)
                //    {
                //        return BadRequest("Subcriber with this mail already exist !!");
                //    }

                var subscribers = new Subscribers
                {
                    Email = request.Email,
                    IsActive = request.IsActive,
                };

                await context.Subscribers.AddAsync(subscribers);
                await context.SaveChangesAsync();

                var response = await _orderRepository.SendNewSubscriberEmail(request);
                if (response)
                {
                    return Ok("Subscriber with this email successfully");
                }
            }
            return Ok("Subscriber with this email successfully");
        }


        [HttpPost]
        [Route("UpdateAvatarImage")]
        public async Task<IActionResult> UpdateAvatarImage(AvatarCustomerUpdate request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            var result = await _orderRepository.UpdateAvatarCustomer(request.AvatarBase64, request.CustomerEmail);
            if (!string.IsNullOrEmpty(result))
            {
                return Ok(result);
            }
            return BadRequest();

        }

        [HttpPost]
        [Route("GetOrdersByCustomerId")]
        public async Task<IActionResult> GetOrdersByCustomerId(RequestGetOrdersByCustomerId request)
        {

            if (request != null)
            {
                var response = await _orderRepository.GetOrdersByCustomerId(request);
                return Ok(response);
            }
            return BadRequest();

        }

        [HttpPost]
        [Route("GetOrderItemFullDetail")]
        public async Task<IActionResult> GetOrderItemFullDetail(RequestGetOrderItemFullDetail request)
        {
            var response = new ResponseGetOrderItemFullDetail();
            if (request != null)
            {
                response = await _orderRepository.GetOrderItemFullDetail(request);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("GetEmailOrderItemFullDetailETicket")]
        public async Task<IActionResult> GetEmailOrderItemFullDetailETicket(RequestGetOrderItemFullDetail request)
        {
            var response = new ResponseGetOrderItemFullDetail();
            if (request != null)
            {
                response = await _orderRepository.GetOrderItemFullDetail(request);
                if (response != null)
                {
                    var requestEmailNewOrder = new RequestSendNewOrderEmail();
                    requestEmailNewOrder.customerId = response.CustomerId;
                    requestEmailNewOrder.customerName = response.FullName;
                    requestEmailNewOrder.customerEmail = response.Email;
                    requestEmailNewOrder.culture_code = request.cultureCode;
                    requestEmailNewOrder.orderCode = response.OrderCode;
                    requestEmailNewOrder.activeStatus = response.ActiveStatus;

                    var emailResponse = _orderRepository.SendNewOrderEmailToCustomer(requestEmailNewOrder);


                }
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("GetCancelPolicyWhenOrderCancel")]
        public async Task<IActionResult> GetCancelPolicyWhenOrderCancel(RequestCancelOrdersOrderDetail request)
        {
            using (IDbContext context = new IDbContext())
            {
                if (request != null)
                {
                    var orderDetailAffected = await context.OrderDetail.FindAsync(request.orderDetailId);
                    if (orderDetailAffected != null)
                    {
                        var productParentId = orderDetailAffected.ProductParentId;
                        if (productParentId > 0)
                        {
                            var cancelPolicies = context.ProductCancelPolicy.Where(r => r.ProductId == productParentId).ToList();
                            var flag = false;
                            var isConfirmCancel = false;
                            var rollbackValue = 0;
                            var rollbackValueOption = 0;
                            if (cancelPolicies.Any())
                            {
                                var pickingDate = orderDetailAffected.PickingDate;
                                var dateNow = DateTime.Now;
                                var dateDiff = pickingDate - dateNow;
                                var numberOfDateDiff = dateDiff.Value.Hours;
                                cancelPolicies = cancelPolicies.OrderByDescending(r => r.BeforeDate).ToList();
                                foreach (var item in cancelPolicies)
                                {
                                    if (!flag)
                                    {
                                        if (numberOfDateDiff >= item.BeforeDate)
                                        {
                                            isConfirmCancel = true;
                                            rollbackValue = item.RollbackValue;
                                            rollbackValueOption = item.RollbackValueOption;
                                            flag = true;
                                        }
                                    }
                                }
                            }
                            dynamic response = new ExpandoObject();
                            response.isConfirmCancel = isConfirmCancel;
                            response.rollbackValue = rollbackValue;
                            response.rollbackValueOption = rollbackValueOption;
                            return Ok(response);
                        }
                    }
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("CancelOrdersOrderDetail")]
        public async Task<IActionResult> CancelOrdersOrderDetail(RequestCancelOrdersOrderDetail request)
        {
            if (request != null)
            {
                var result = await _orderRepository.CancelOrdersOrderDetail(request);
                dynamic response = new ExpandoObject();
                response.isSuccess = result;
                return Ok(response);
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("UpdateOrderDetailCommentWithRating")]
        public async Task<IActionResult> UpdateOrderDetailCommentWithRating(RequestUpdateOrderDetailCommentWithRating request)
        {
            var response = await _orderRepository.UpdateOrderDetailCommentWithRating(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetOrderDetailCommentWithRating/{orderDetailId}")]
        public async Task<IActionResult> GetOrderDetailCommentWithRating(int orderDetailId)
        {
            var response = await _orderRepository.GetOrderDetailCommentWithRating(orderDetailId);
            return Ok(response);
        }

        [HttpPost]
        [Route("CheckChatSessionByCustomerEmail")]
        public async Task<IActionResult> CheckChatSessionByCustomerEmail(RequestCheckChatSessionByCustomerEmail request)
        {
            var response = _orderRepository.CheckChatSessionByCustomerEmail(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("CheckOrderDetailByEmail")]
        public async Task<IActionResult> CheckOrderDetailByEmail(RequestCheckOrderDetailByEmail request)
        {
            var response = _orderRepository.CheckOrderDetailByEmail(request);
            return Ok(response);
        }



        [HttpPost]
        [Route("EmailSubscriptionRegistration")]
        public async Task<IActionResult> EmailSubscriptionRegistration(string email)
        {
            var response = _orderRepository.EmailSubscriptionRegistration(email);
            return Ok(response);
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

        private string ConvertToCorrectEncoding(string input)
        {
            // Giải mã các ký tự HTML entities thành chuỗi Unicode
            string decodedString = HttpUtility.HtmlDecode(input);

            return decodedString;

        }

        private string GenerateRandomPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var password = new char[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                var byteBuffer = new byte[1];
                for (int i = 0; i < length; i++)
                {
                    rng.GetBytes(byteBuffer);
                    var randomIndex = byteBuffer[0] % chars.Length;
                    password[i] = chars[randomIndex];
                }
            }
            return new string(password);
        }
    }
}
