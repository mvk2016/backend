using System;
using Microsoft.ServiceBus.Messaging;
using api.Interfaces;
using api.Lib;

namespace api.Services
{
    /// <summary>
    /// Forwards messages from an Azure Service Bus Queue to connected
    /// WebSocket clients using WebSocketHandler.
    /// </summary>
    /// <seealso cref="IRealTimeConnector" />
    public class ServiceBusQueueConnector : IRealTimeConnector
    {
        public ServiceBusQueueConnector()
        {
            Connect();
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        public void Connect()
        {
            var client =
                QueueClient.CreateFromConnectionString(Config.QueueConnectionString);

            // Configure the callback options.
            var options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromSeconds(5)
            };

            // Callback to handle received messages.
            client.OnMessage(OnMessage, options);
        }

        private static void OnMessage(BrokeredMessage message)
        {
            try
            {
                // Process message from queue.
                var body = message.GetBody<string>();
                Console.WriteLine("Popped from Queue: " + body);

                // Remove message from queue.
                WebSocketHandler.Broadcast(body);
                message.Complete();
            }
            catch (Exception)
            {
                // Indicates a problem, unlock message in queue.
                message.Abandon();
            }
        }
    }
}
