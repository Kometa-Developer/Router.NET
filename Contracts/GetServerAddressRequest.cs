namespace Router.Contracts
{
    public class GetServerAddressRequest
    {
        public string Server { get; }

        public string ClientPlatform { get; }

        public string ClientVersion { get; }
        
        public GetServerAddressRequest(string server, string clientPlatform, string clientVersion)
        {
            Server = server;
            ClientPlatform = clientPlatform;
            ClientVersion = clientVersion;
        }
    }
}