using MI.Dal.IDbContext;
using MI.Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlatformCMS.DTOModels;
using PlatformCMS.Extensions;
using System;
using System.Linq;

namespace PlatformCMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class G_NotificationsController : ControllerBase
    {
        [HttpGet]
        [Route("GetNotifications")]
        public IActionResult GetNotifications(int pageIndex = 1, int pageSize = 10, string filter = "")
        {
            using (var context = new IDbContext())
            {
                var query = context.G_PushNotification.AsQueryable();
                if (!string.IsNullOrWhiteSpace(filter))
                    query = query.Where(x => x.NotificationName.Contains(filter));

                var total = query.Count();
                var data = query.OrderByDescending(x => x.Id)
                                .Skip((pageIndex - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

                return Ok(new { totalItems = total, items = data });
            }
        }
        [HttpGet]
        [Route("GetNotificationById")]
        public IActionResult GetById(int id)
        {
            using (var context = new IDbContext())
            {
                var notification = context.G_PushNotification
                    .FirstOrDefault(x => x.Id == id);

                if (notification == null)
                    return NotFound();

                var details = context.G_PushNotificationDetail
                    .Where(d => d.G_PushNotificationId == id)
                    .ToList();

                return Ok(new
                {
                    notification.Id,
                    notification.NotificationName,
                    notification.NotificationIcon,
                    notification.NotificationDescription,
                    notification.NotificationPushLink,
                    notification.NotificationDetail,
                    Details = details
                });
            }
        }

        [HttpPost]
        [Route("PostNotification")]
        public IActionResult PostNotification([FromBody] GNotificationCreateModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.NotificationName))
                return BadRequest("Invalid model");

            var currentUsername = User.GetUserName();
            if (string.IsNullOrEmpty(currentUsername))
                return Unauthorized();

            using (var context = new IDbContext())
            {
                var notification = new G_PushNotification
                {
                    NotificationName = model.NotificationName,
                    NotificationIcon = model.NotificationIcon,
                    NotificationDescription = model.NotificationDescription,
                    NotificationPushLink = model.NotificationPushLink,
                    NotificationDetail = model.NotificationDetail,
                    CreatedByUsername = currentUsername,
                    CreatedDate = DateTime.UtcNow
                };

                context.G_PushNotification.Add(notification);
                context.SaveChanges();

                if (model.Details != null && model.Details.Any())
                {
                    var details = model.Details.Select(d => new G_PushNotificationDetail
                    {
                        G_PushNotificationId = notification.Id,
                        CustomerGroupDetailId = d.CustomerGroupDetailId,
                        PusingSceduleTime = d.PusingSceduleTime,
                        IsPushed = false
                    }).ToList();

                    context.G_PushNotificationDetail.AddRange(details);
                    context.SaveChanges();
                }

                return Ok(notification);
            }
        }

        [HttpPut]
        [Route("PutNotification/{id}")]
        public IActionResult PutNotification(int id, [FromBody] GNotificationCreateModel model)
        {
            if (model == null)
                return BadRequest("Invalid model");

            using (var context = new IDbContext())
            {
                var notification = context.G_PushNotification.FirstOrDefault(x => x.Id == id);
                if (notification == null)
                    return NotFound();

                notification.NotificationName = model.NotificationName;
                notification.NotificationIcon = model.NotificationIcon;
                notification.NotificationDescription = model.NotificationDescription;
                notification.NotificationPushLink = model.NotificationPushLink;
                notification.NotificationDetail = model.NotificationDetail;

                context.SaveChanges();

                var oldDetails = context.G_PushNotificationDetail.Where(x => x.G_PushNotificationId == id).ToList();
                context.G_PushNotificationDetail.RemoveRange(oldDetails);
                context.SaveChanges();

                if (model.Details != null)
                {
                    var newDetails = model.Details.Select(d => new G_PushNotificationDetail
                    {
                        G_PushNotificationId = id,
                        CustomerGroupDetailId = d.CustomerGroupDetailId,
                        PusingSceduleTime = d.PusingSceduleTime,
                        IsPushed = false
                    }).ToList();

                    context.G_PushNotificationDetail.AddRange(newDetails);
                    context.SaveChanges();
                }

                return Ok(notification);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            using (var context = new IDbContext())
            {
                var notification = context.G_PushNotification.FirstOrDefault(x => x.Id == id);
                if (notification == null)
                    return NotFound();

                var details = context.G_PushNotificationDetail.Where(x => x.G_PushNotificationId == id).ToList();

                context.G_PushNotificationDetail.RemoveRange(details);
                context.G_PushNotification.Remove(notification);
                context.SaveChanges();

                return Ok(new { message = "Deleted successfully" });
            }
        }
    }
}
