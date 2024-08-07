using MI.Dal.IDbContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using PlatformWEBAPI.Services.Extra.Repository;
using PlatformWEBAPI.Services.Order.Repository;
using PlatformWEBAPI.Services.Order.ViewModal;
using PlatformWEBAPI.Services.Product.ViewModel;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PlatformWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IExtraRepository _extraRepository;
        
        public AuthController(IOrderRepository orderRepository, IExtraRepository extraRepository)
        {
            _orderRepository = orderRepository;
            _extraRepository = extraRepository;
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
        [Route("DoSignUp")]
        public async Task<IActionResult> DoSignUp(CustomerAuthViewModel request)
        {
            var result = _orderRepository.DoSignUp(request);
            if (result > 0)
            {
                var response = new RequestSendNewUserEmail();
                response.customerEmail = request.email;
                response.username = request.email; 
                response.password = request.password;
                var sendMailStatus = _orderRepository.SendNewUserEmail(response);
                //_extraRepository.SendEmailRegister(request);
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost]
        [Route("GetOrdersByCustomerId")]
        public async Task<IActionResult> GetOrdersByCustomerId (RequestGetOrdersByCustomerId request)
        {
            
            if(request != null)
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
            if(request != null)
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
                if(response != null)
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
                    if(orderDetailAffected != null)
                    {
                        var productParentId = orderDetailAffected.ProductParentId;
                        if(productParentId > 0)
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
                                var numberOfDateDiff = dateDiff.Value.Days;
                                cancelPolicies = cancelPolicies.OrderByDescending(r => r.BeforeDate).ToList();
                                foreach(var item in cancelPolicies)
                                {
                                    if (!flag)
                                    {
                                        if(numberOfDateDiff >= item.BeforeDate)
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
            if(request != null)
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
    }
}
