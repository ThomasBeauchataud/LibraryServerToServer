using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibraryServerToServer
{
    [Serializable]
    public class Command
    {
        protected List<object> data;

        public Command(List<object> data)
        {
            this.data = data;
        }

        public List<object> GetData()
        {
            return data;
        }

        public static byte[] Serialize(Command command)
        {
            Console.WriteLine("Trying to serialize a command");
            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();
            bin.Serialize(mem, command);
            Console.WriteLine("Command has been serialized");
            return mem.GetBuffer();
        }

        public static Command DeSerialize(byte[] bytes)
        {
            Console.WriteLine("Trying to deserialize a command");
            List<byte> TransmissionBuffer = new List<byte>(bytes);
            byte[] dataBuffer = TransmissionBuffer.ToArray();
            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();
            mem.Write(dataBuffer, 0, dataBuffer.Length);
            mem.Seek(0, 0);
            Command command = (Command)bin.Deserialize(mem);
            Console.WriteLine("Command has been deserialize");
            return command;
        }
    }
}
