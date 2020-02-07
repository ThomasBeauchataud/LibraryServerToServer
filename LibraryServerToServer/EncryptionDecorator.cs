namespace LibraryServerToServer
{
    public class EncryptionDecorator : DataSourceDecorator
    {
        public EncryptionDecorator(IDataSource dataSource) : base(dataSource)
        {
        }

        public new void WriteData(byte[] data)
        {
            base.WriteData(data);
        }

        public new byte[] ReadData()
        {
            return base.ReadData();
        }
    }
}
