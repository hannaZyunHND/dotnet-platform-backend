using FirebaseAdmin.Messaging;
using System.Threading.Tasks;
using System;

namespace PlatformCMS.PushNotification
{
    public class NotificationService
    {
        public async Task PushToFirebaseAsync(string fcmToken, string title, string body)
        {
            if (string.IsNullOrEmpty(fcmToken))
            {
                Console.WriteLine("⚠️ Token rỗng, không thể gửi");
                return;
            }

            var message = new Message()
            {
                Token = fcmToken,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
            };

            try
            {
                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine("✔ Gửi FCM thành công: " + response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Lỗi gửi FCM: " + ex.Message);
            }
        }
    }
}
