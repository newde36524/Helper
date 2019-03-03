using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    /**
         * 解决Socket监听一次性问题，改为事件触发机制（一有连接就处理）
         * **/
    public class ServerSocket : IDisposable
    {
        private Socket _socket;

        public ServerSocket(Socket socket)
        {
            _socket = socket;
        }

        public void Dispose()
        {
            this._socket.Dispose();
        }

        public Task<Socket> AcceptAsyncEx()
        {
            return Task.Factory.FromAsync(_socket.BeginAccept, _socket.EndAccept, null);
        }

        public Task<int> ReceiveAsyncEx(IList<ArraySegment<byte>> a, SocketFlags b)
        {
            return Task.Factory.FromAsync(_socket.BeginReceive, _socket.EndReceive, a, b, null);
        }
    }
}
