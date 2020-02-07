namespace LibraryServerToServer
{
    public interface IDataSource
    {
        void WriteData(byte[] data);
        byte[] ReadData();
    }
}
