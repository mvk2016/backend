using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace api.Lib
{
    public class WebSocketHandler
    {
        private static readonly ConcurrentBag<WebSocket> _sockets = new ConcurrentBag<WebSocket>();

        /// <summary>
        ///     Add a websocket to the bag of websockets.
        /// </summary>
        /// <param name="webSocket"></param>
        internal static void Add(WebSocket webSocket)
        {
            _sockets.Add(webSocket);
        }

        /// <summary>
        ///     Broadcast a message to every open websocket in the bag.
        /// </summary>
        /// <param name="message"></param>
        internal static async void Broadcast(string message)
        {
            var token = CancellationToken.None;
            var type = WebSocketMessageType.Text;
            var data = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(data);

            try
            {
                await Task.WhenAll(_sockets.Where(s => s.State == WebSocketState.Open)
                    .Select(s => s.SendAsync(buffer, type, true, token)));
            }
            catch
            {
                // Socket didn't close gracefully, no clean way to handle this.
                // TODO: Do this cleanly.
            }
        }
    }
}