using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using api.Interfaces;
using api.Lib;

namespace api.Services
{
    public class ServiceBusQueueConnector : IRealTimeConnector
    {
        public ServiceBusQueueConnector()
        {
            Connect();
        }

        public void Connect()
        {
            {
                string connectionString = Config.QueueConnectionString;
                
                QueueClient client =
                  QueueClient.CreateFromConnectionString(connectionString);

                // Configure the callback options.
                OnMessageOptions options = new OnMessageOptions();
                options.AutoComplete = false;
                options.AutoRenewTimeout = TimeSpan.FromSeconds(5);

                // Callback to handle received messages.
                client.OnMessage(OnMessage, options);
            }
        }

        protected void OnMessage(BrokeredMessage message)
        {
            try
            {
                // Process message from queue.
                var body = message.GetBody<string>();
                Console.WriteLine("Body: " + body);
                //Console.WriteLine("MessageID: " + message.MessageId);
                //Console.WriteLine("Test Property: " + message.Properties["Type"]);

                // Remove message from queue.
                message.Complete();
                WebSocketHandler.Broadcast(body);
            }
            catch (Exception)
            {
                // Indicates a problem, unlock message in queue.
                message.Abandon();
            }
        }
    }
}
