namespace SikshyaPaymentGatewayAPI.Models
{
    public class ConnectionStringModel
    {

        private string _serverIp = "103.90.86.220,5437";
        public string serverIp
        {
            get { return _serverIp; }
            set { _serverIp = value != string.Empty ? value : _serverIp; }
        }

        private string _database = "SHIKSHYADEMO";
        public string database
        {
            get { return _database; }
            set { _database = value != string.Empty ? value : _database; }
        }

        private string _loginId = "sa";
        public string loginId
        {
            get { return _loginId; }
            set { _loginId = value != string.Empty ? value : _loginId; }
        }

        private string _password = "S@_m$56_*300&@_2014X";
        public string password
        {
            get { return _password; }
            set { _password = value != string.Empty ? value : _password; }
        }
    }
}
