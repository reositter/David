using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public class VismaConnection
	{
		public static GlobalServerComponent Connection { get; private set; }

		static VismaConnection()
        {
			Connection = new GlobalServerComponent();
        }

	    ~VismaConnection()
	    {
			Connection = null;
	    }

		public static int Open(string clientName, string user, string password, string bapiKey)
		{
			if (!string.IsNullOrEmpty(password))
			{
				var strEncryptionKey = Connection.bcGetEncryptionKey();
				password = Visma.Core.Security.CryptoServices.CredentialEncryption.Encrypt(password, strEncryptionKey);
				password = "6C783FE0-B7D2-11DD-8B01-CB9755D89593" + password;
			}

			return Connection.bcLogon(clientName, user, password, bapiKey);
		}
	}
}
