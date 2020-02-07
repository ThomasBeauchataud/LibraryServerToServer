namespace LibraryServerToServer
{
    public class DataSourceDecorator : IDataSource
    {
        private IDataSource dataSource;

        public DataSourceDecorator(IDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        public byte[] ReadData()
        {
            return dataSource.ReadData();
        }

        public void WriteData(byte[] data)
        {
            dataSource.WriteData(data);
        }
    }
}
