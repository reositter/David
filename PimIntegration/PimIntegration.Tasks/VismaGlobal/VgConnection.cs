using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public class VgConnection
	{
		public static GlobalServerComponent Connection { get; private set; }

		static VgConnection()
        {
			Connection = new GlobalServerComponent();
        }

	    ~VgConnection()
	    {
			Connection = null;
	    }

		public static int Open(string clientName, string bapiKey)
		{
			var loginCode = Connection.bcLogon(clientName, string.Empty, string.Empty, bapiKey);
			return loginCode;
		}
	}
}
