using api.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Services
{
    public class EventHubConnector : IRealTimeConnector
    {
        public EventHubConnector()
        {
            Connect();
        }

        public void Connect()
        {
            Console.WriteLine("EventHubConnector initialized!");
            // Environment.Exit(0);
        }
    }
}
