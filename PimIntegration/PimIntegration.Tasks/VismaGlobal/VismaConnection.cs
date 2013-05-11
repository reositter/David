using System;
using RG_SRVLib.Interop;

namespace PimIntegration.Tasks.VismaGlobal
{
	public interface IVismaConnection {
		GlobalServerComponent Open();
	}

	public class VismaConnection : IVismaConnection
	{
		private GlobalServerComponent _connection;
		private readonly string _clientName;
		private readonly string _user;
		private readonly string _password;
		private readonly string _bapiKey;

		public VismaConnection(string clientName, string user, string password, string bapiKey)
		{
			_connection = new GlobalServerComponent();

			_clientName = clientName;
			_user = user;
			_password = EncryptPassword(password);
			_bapiKey = bapiKey;
		}

		~VismaConnection()
	    {
			if (_connection != null)
				System.Runtime.InteropServices.Marshal.ReleaseComObject(_connection);
	    }

		public GlobalServerComponent Open()
		{
			if (IsConnected())
				return _connection;

			var loginCode = _connection.bcLogon(_clientName, _user, _password, _bapiKey);

			if (loginCode != 0)
			{
				Log.ForCurrent.ErrorFormat("Failed to open connection to Visma Global. ErrorCode: {0}", loginCode);
				throw new ApplicationException("Failed to open connection to Visma Global");
			}

			return _connection;
		}

		private bool IsConnected()
		{
			int userNo;
			string user;
			_connection.bcGetUserInfo(out userNo, out user);

			return userNo > 0;
		}

		private string EncryptPassword(string password)
		{
			if (string.IsNullOrEmpty(password)) return string.Empty;

			var strEncryptionKey = _connection.bcGetEncryptionKey();
			var encryptedPassword = Visma.Core.Security.CryptoServices.CredentialEncryption.Encrypt(password, strEncryptionKey);

			return "6C783FE0-B7D2-11DD-8B01-CB9755D89593" + encryptedPassword;
		}
	}
}
