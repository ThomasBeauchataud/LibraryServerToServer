namespace LibraryServerToServer
{
    public interface IAdapter
    {
        object GenerateInstance(byte[] data);
    }
}
