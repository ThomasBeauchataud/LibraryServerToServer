using LibraryServerToServer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientTestLibraryServerToServer
{
    class Program
    {
        static void Main(string[] args)
        {
            List<object> commandParameters = new List<object>();
            commandParameters.Add(1);
            commandParameters.Add(2);
            Command command = new Command(commandParameters);
            Flux flux = new Flux();
            CompressionDecorator compressedFlux = new CompressionDecorator(flux);
            EncryptionDecorator encryptedCompressedFlux = new EncryptionDecorator(compressedFlux);
            encryptedCompressedFlux.WriteData(Command.Serialize(command));
            List<object> parameters = new List<object>();
            parameters.Add("localhost");
            parameters.Add(8888);
            flux = new WebSocketSender().Send(flux, parameters);
            Console.WriteLine("Command result : " + BitConverter.ToInt32(flux.ReadData(), 0));
            Console.Write("Type any key to end the program");
            Console.ReadLine();
        }
    }
}
