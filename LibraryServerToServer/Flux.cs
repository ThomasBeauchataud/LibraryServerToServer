using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LibraryServerToServer
{
    [Serializable]
    public class Flux : IDataSource
    {
        protected byte[] data;

        public void WriteData(byte[] data)
        {
            this.data = data;
        }

        public byte[] ReadData()
        {
            return data;
        }

        public static byte[] Serialize(Flux flux)
        {
            Console.WriteLine("Trying to serialize a flux");
            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();
            bin.Serialize(mem, flux);
            Console.WriteLine("Flux has been serialized");
            return mem.GetBuffer();
        }

        public static Flux DeSerialize(byte[] bytes)
        {
            Console.WriteLine("Trying to deserialize a flux");
            List<byte> TransmissionBuffer = new List<byte>(bytes);
            byte[] dataBuffer = TransmissionBuffer.ToArray();
            BinaryFormatter bin = new BinaryFormatter();
            MemoryStream mem = new MemoryStream();
            mem.Write(dataBuffer, 0, dataBuffer.Length);
            mem.Seek(0, 0);
            Flux flux = (Flux)bin.Deserialize(mem);
            Console.WriteLine("Flux has been deserialized");
            return flux;
        }
    }
}
