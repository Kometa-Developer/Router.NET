namespace Router.Contracts
{
    public class GetServerAddressResponse
    {
        public enum ReturnCode
        {
            Ok = 0,
            UpdateRequired = 1,
            IncorrectVersion = 2,
            IncorrectServer = 3,
            IncorrectPlatform = 4,
            Maintenance = 5,
            InternalServerError = 6
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