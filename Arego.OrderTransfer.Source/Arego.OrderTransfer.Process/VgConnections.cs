using RG_SRVLib.Interop;

namespace Arego.OrderTransfer.Process
{
    public class VgConnections
    {
        public static GlobalServerComponent SourceConnection { get; private set; }
        public static GlobalServerComponent DestinationConnection { get; private set; }

        static VgConnections()
        {
            SourceConnection = new GlobalServerComponent();
            DestinationConnection = new GlobalServerComponent();
        }

	    ~VgConnections()
	    {
			SourceConnection = null;
			DestinationConnection = null;
	    }

	    public static int ConnectToSourceClient(string clientName, string bapiKey)
        {
			var loginCode = SourceConnection.bcLogon(clientName, string.Empty, string.Empty, bapiKey);
			HandleLoginResult(loginCode, clientName, bapiKey);
			return loginCode;
        }

        public static int ConnectToDestinationClient(string clientName, string bapiKey)
        {
            var loginCode = DestinationConnection.bcLogon(clientName, string.Empty, string.Empty, bapiKey);
            HandleLoginResult(loginCode, clientName, bapiKey);
            return loginCode;
        }

        public static string GetNameOfSourceClient()
        {
            return GetNameOfClient(SourceConnection);
        }

        public static string GetNameOfDestinationClient()
        {
            return GetNameOfClient(DestinationConnection);
        }

	    private static string GetNameOfClient(GlobalServerComponent connection)
        {
            string name; int connectionNo;
            connection.bcGetDataConnectionInfo(out connectionNo, out name);
            return name;
        }

        private static void HandleLoginResult(int loginCode, string clientName, string bapiKey)
        {
            if (loginCode == 0) return;

            LogFileWriter.WriteLine("ERROR: Failed to connect to Visma Global. Client=" + clientName + " Bapi=" + bapiKey);
            LogFileWriter.WriteLine(string.Format("Code {0} - {1}", loginCode, TranslateLoginCodeToMessage(loginCode)));
        }

        public static string TranslateLoginCodeToMessage(int loginCode)
        {
            switch (loginCode)
            {
                case 0:
                    return "OK";
                case 11:
                    return "Invalid username or password.";
                case 12:
                    return "Invalid BAPI key.";
                case 16:
                    return "Failed to connect to database. Check client name.";
                case 21:
                    return "Too many concurrent users at the moment.";
                case 28:
                case 27:
                    return "The Visma Global license has expired.";
                default:
                    return "Unexpected failure.";
            }
        }
    }
}