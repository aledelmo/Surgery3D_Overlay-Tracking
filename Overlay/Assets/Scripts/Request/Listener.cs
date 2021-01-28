using NetMQ;
using System;
using NetMQ.Sockets;
using System.Threading.Tasks;

namespace Request
{
    public class Listener
    {
        private readonly string _host;
        private readonly string _port;
        private readonly Action<string> _messageCallback;
        
        public Listener(string host, string port, Action<string> messageCallback)
        {
            _host = host;
            _port = port;
            _messageCallback = messageCallback;
        }

        public void RequestMessageAsync(string messageToSend)
        {
            AsyncIO.ForceDotNet.Force();
            
            async Task ReceiveAsync()
            {
                using (var socket = new RequestSocket())
                {
                    socket.Connect($"tcp://{_host}:{_port}");
                    if (socket.TrySendFrame(messageToSend))
                    {
                        var(message, _) = await socket.ReceiveFrameStringAsync();
                        _messageCallback(message);
                    }
                }
            }
            
            using (var runtime = new NetMQRuntime())
            {
                var t = ReceiveAsync();
                runtime.Run(t);

                if (t.IsFaulted && t.Exception is AggregateException exc)
                {
                    throw exc.GetBaseException();
                }
            }
        }
        
        public void RequestMessage()
        {
            var messageReceived = false;
            var message = "";
            AsyncIO.ForceDotNet.Force();

            var timeout = new TimeSpan(0, 0, 2);
            using (var socket = new RequestSocket())
            {
                socket.Connect($"tcp://{_host}:{_port}");
                if (socket.TrySendFrame("Hello"))
                {
                    messageReceived = socket.TryReceiveFrameString(timeout, out message);
                }
            }

            NetMQConfig.Cleanup();
            if (!messageReceived)
                message = "Could not receive message from server!";
            _messageCallback(message);
        }
    }
}