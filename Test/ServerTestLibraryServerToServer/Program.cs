using LibraryServerToServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTestLibraryServerToServer
{
    class Program
    {
        static void Main(string[] args)
        {
            IExecutor executor = new CommandExecutor();
            IAdapter adapter = new CommandAdapter();
            WebSocketListener webSocketListener = new WebSocketListener(executor, adapter);
            webSocketListener.StartServer(8888);
        }
    }
}
