namespace Router.Contracts
{
    public class GetServerAddressResponse
    {
        public enum ReturnCode
        {
            Ok,
            UpdateRequired,
            IncorrectVersion,
            IncorrectServer,
            IncorrectPlatform,
            Maintenance,
            InternalServerError
        }
        
        public ReturnCode Code { get; }
        
        public string ServerAddress { get; }
        
        public GetServerAddressResponse(ReturnCode code, string serverAddress)
        {
            Code = code;
            ServerAddress = serverAddress;
        }
    }
}