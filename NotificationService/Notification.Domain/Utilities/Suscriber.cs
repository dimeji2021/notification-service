using Newtonsoft.Json;
using NotificationService.Notification.Domain.Entities.SendEmail;
using NotificationService.Notification.Domain.Repository;
using NotificationService.Notification.Infrastructure.RepositoryImpl;
using StackExchange.Redis;
using System.Net.Http.Headers;
using System.Text;

namespace NotificationService.Notification.Domain.Utilities
{
    public class Suscriber 
    {
        private static ConnectionMultiplexer connection = ConnectionMultiplexer.Connect("localhost:6379");

        private const string Channel = "providus-channel";
        private readonly ISendEmail _sendEmail;

        public Suscriber(ISendEmail sendEmail)
        {
            _sendEmail = sendEmail;
        }

        public void Suscribe()
        {
            var pubsub = connection.GetSubscriber();
            pubsub.SubscribeAsync(Channel, (channel, message) =>
            {
                var res = JsonConvert.DeserializeObject<SendEmailRequest>(message);
                _sendEmail.SendMail(res);
            });
        }
    }

}