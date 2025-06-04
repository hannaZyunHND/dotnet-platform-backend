using Google;
using MI.Dal.IDbContext;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Internal;
using PlatformCMS.DTOModels;
using PlatformCMS.Extensions;
using System;
using System.Linq;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerGroupsController : ControllerBase
    {
        // GET api/CustomerGroup/GetAll?pageIndex=1&pageSize=10&filter=abc
        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll(int pageIndex = 1, int pageSize = 10, string filter = "")
        {
            using (IDbContext context = new IDbContext())
            {
                var query = context.CustomerGroup.AsQueryable();

                if (!string.IsNullOrEmpty(filter))
                {
                    query = query.Where(x => x.Name.Contains(filter));
                }

                var total = query.Count();

                var data = query
                    .OrderByDescending(x => x.Id)
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    TotalItems = total,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    Items = data
                });
            }
        }


        [HttpPost]
        [Route("Create")]
        public IActionResult Create([FromBody] CustomerGroupCreateModel model)
        {
            if (model == null)
                return BadRequest("Model is null.");

            if (string.IsNullOrWhiteSpace(model.Name))
                return BadRequest("Group name is required.");

            var currentUsername = User.GetUserName();
            if (string.IsNullOrEmpty(currentUsername))
                return Unauthorized();

            using (IDbContext context = new IDbContext())
            {
                var group = new CustomerGroup
                {
                    Name = model.Name,
                    CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                    CreateByUsername = currentUsername
                };

                context.CustomerGroup.Add(group);
                context.SaveChanges(); // Save to get group.Id

                if (model.CustomerIds != null && model.CustomerIds.Any())
                {
                    var details = model.CustomerIds.Select(customerId => new CustomerGroupDetail
                    {
                        CustomerGroupId = group.Id,
                        CustomerId = customerId
                    }).ToList();

                    context.CustomerGroupDetail.AddRange(details);
                    context.SaveChanges();
                }

                return Ok(group);
            }
        }


        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult Update(int id, [FromBody] CustomerGroupUpdateModel model)
        {
            if (model == null)
                return BadRequest("Invalid data");

            using (IDbContext context = new IDbContext())
            {
                var group = context.CustomerGroup.FirstOrDefault(x => x.Id == id);
                if (group == null)
                    return NotFound("CustomerGroup not found");

                group.Name = model.Name;
                context.SaveChanges();

                // Xoá chi tiết cũ
                var oldDetails = context.CustomerGroupDetail
                    .Where(x => x.CustomerGroupId == id)
                    .ToList();

                context.CustomerGroupDetail.RemoveRange(oldDetails);
                context.SaveChanges();

                // Thêm chi tiết mới
                if (model.CustomerIds != null)
                {
                    foreach (var customerId in model.CustomerIds)
                    {
                        context.CustomerGroupDetail.Add(new CustomerGroupDetail
                        {
                            CustomerGroupId = id,
                            CustomerId = customerId
                        });
                    }
                    context.SaveChanges();
                }

                return Ok(group);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            using (IDbContext context = new IDbContext())
            {
                var group = context.CustomerGroup.FirstOrDefault(x => x.Id == id);
                if (group == null)
                    return NotFound("CustomerGroup not found");

                var details = context.CustomerGroupDetail
                    .Where(x => x.CustomerGroupId == id)
                    .ToList();

                context.CustomerGroupDetail.RemoveRange(details);
                context.CustomerGroup.Remove(group);
                context.SaveChanges();

                return Ok(new { message = "Deleted successfully" });
            }
        }


        [HttpPost]
        [Route("SearchCustomer")]
        public IActionResult Search([FromBody] CustomerSearchRequest request)
        {
            if (request == null)
                return BadRequest("Invalid search request");

            using (IDbContext context = new IDbContext())
            {
                var query = context.Customer.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Email))
                    query = query.Where(x => x.Email.Contains(request.Email));

                if (!string.IsNullOrWhiteSpace(request.Country))
                    query = query.Where(x => x.Country.Contains(request.Country));

                if (!string.IsNullOrWhiteSpace(request.Fullname))
                    query = query.Where(x => x.Fullname.Contains(request.Fullname));

                var totalItems = query.Count();

                var customerList = query
                    .OrderByDescending(x => x.Id)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToList();

                var customerIds = customerList.Select(c => c.Id).ToList();

                // Đếm tổng số OrderDetail theo CustomerId
                var orderDetailCount = context.Orders
                    .Where(o => customerIds.Contains(o.CustomerId.Value))
                    .SelectMany(o => o.OrderDetail)
                    .GroupBy(od => od.Order.CustomerId)
                    .ToDictionary(g => g.Key, g => g.Count());

                var result = customerList.Select(c => new
                {
                    c.Id,
                    c.Email,
                    c.Country,
                    c.Fullname,
                    TotalOrderDetails = orderDetailCount.ContainsKey(c.Id) ? orderDetailCount[c.Id] : 0
                }).ToList();

                return Ok(new
                {
                    TotalItems = totalItems,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    Items = result
                });
            }
        }
        [HttpGet]
        [Route("GetCustomerSelectedById/{id}")]
        public IActionResult GetById(int id)
        {
            using (var context = new IDbContext())
            {
                var group = context.CustomerGroup.FirstOrDefault(x => x.Id == id);
                if (group == null) return NotFound();

                var customerIds = context.CustomerGroupDetail
                    .Where(x => x.CustomerGroupId == id)
                    .Select(x => x.CustomerId)
                    .ToList();

                return Ok(new
                {
                    group.Id,
                    group.Name,
                    CustomerIds = customerIds
                });
            }
        }

    }


}
