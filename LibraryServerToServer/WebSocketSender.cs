using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace LibraryServerToServer
{
    public class WebSocketSender : ISender
    {
        public Flux Send(Flux flux, List<object> parameters)
        {
            byte[] bytes = new byte[100000];
            IPHostEntry ipHostInfo = Dns.GetHostEntry((string)parameters[0]);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, (int)parameters[1]);
            Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                client.Connect(remoteEP);
                int bytesSent = client.Send(Flux.Serialize(flux));
                int bytesRec = client.Receive(bytes);
                flux = Flux.DeSerialize(bytes);
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                return flux;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                return null;
            }
        }
    }
}
