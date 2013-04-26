namespace PimIntegration.Tasks.Database
{
	public class ConnectionStringWrapper
	{
		private readonly string _connectionString;

		public string ConnectionString
		{
			get { return _connectionString; }
		}

		public ConnectionStringWrapper(string connectionString)
		{
			_connectionString = connectionString;
		}
	}
}
