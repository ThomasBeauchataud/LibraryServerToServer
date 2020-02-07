using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace LibraryServerToServer
{
    public class WebSocketListener
    {
        protected bool run;
        protected IExecutor executor;
        protected IAdapter adapter;

        public WebSocketListener(IExecutor executor, IAdapter adapter)
        {
            this.executor = executor;
            this.adapter = adapter;
        }

        public void StartServer(int port)
        {
            Console.WriteLine("Server has started");
            run = true;
            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            TcpListener serverSocket = new TcpListener(ipAddress, port);
            serverSocket.Start();
            while (run)
            {
                TcpClient clientSocket = serverSocket.AcceptTcpClient();
                Console.WriteLine("New client connected");
                ClientHandler clientHandler = new ClientHandler(clientSocket, executor, adapter);
                clientHandler.Handle();
            }
            serverSocket.Stop();
            Console.WriteLine("Server has stopped");
        }

        public void StopServer()
        {
            run = false;
        }
    }

    class ClientHandler
    {
        protected TcpClient clientSocket;
        protected IExecutor executor;
        protected IAdapter adapter;

        public ClientHandler(TcpClient clientSocket, IExecutor executor, IAdapter adapter)
        {
            this.clientSocket = clientSocket;
            this.executor = executor;
            this.adapter = adapter;
        }

        public void Handle()
        {
            Thread threadHandler = new Thread(Handler);
            threadHandler.Start();
        }

        private void Handler()
        {
            Console.WriteLine("Client thread has started");
            byte[] bytesFrom = new byte[clientSocket.ReceiveBufferSize];
            NetworkStream networkStream = clientSocket.GetStream();
            networkStream.Read(bytesFrom, 0, clientSocket.ReceiveBufferSize);
            Flux flux = Flux.DeSerialize(bytesFrom);
            Console.WriteLine("Recieved flux has been deserialized");
            object result = executor.Execute(adapter.GenerateInstance(flux.ReadData()));
            Console.WriteLine("Command has been executed");
            flux.WriteData(ObjectToBytes(result));
            byte[] sendBytes = Flux.Serialize(flux);
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            Console.WriteLine("Response has been sent");
            networkStream.Flush();
            clientSocket.Close();
            Console.WriteLine("Socket has been closed");
        }

        private byte[] ObjectToBytes(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
