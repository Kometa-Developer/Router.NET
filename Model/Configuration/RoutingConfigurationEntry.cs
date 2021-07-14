using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Router.Model.Configuration
{
    public class RoutingConfigurationEntry
    {
        public string Server { get; }
        
        public string Platform { get; }
        
        public string ClientVersion { get; }
        
        public string RouteTarget { get; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public UpdateMode UpdateMode { get; }

        public RoutingConfigurationEntry(
            string server, 
            string platform, 
            string clientVersion, 
            string routeTarget, 
            UpdateMode updateMode
        )
        {
            Server = server;
            Platform = platform;
            ClientVersion = clientVersion;
            RouteTarget = routeTarget;
            UpdateMode = updateMode;
        }
    }
}