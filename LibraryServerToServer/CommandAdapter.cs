namespace LibraryServerToServer
{
    public class CommandAdapter : IAdapter
    {
        public object GenerateInstance(byte[] data)
        {
            return Command.DeSerialize(data);
        }
    }
}
