namespace PimIntegration.Tasks.Database
{
	public class SqliteConnectionStringWrapper
	{
		private readonly string _connectionString;

		public string ConnectionString
		{
			get { return _connectionString; }
		}

		public SqliteConnectionStringWrapper(string connectionString)
		{
			_connectionString = connectionString;
		}
	}
}
