using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Router.Model.Configuration
{
    public class RoutingConfigurationEntry
    {
        public string Server { get; }
        public string Platform { get; }
        public string FromVersion { get; }
        public string ToVersion { get; }
        public string RouteTarget { get; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public RouteMode RouteMode { get; }

        public RoutingConfigurationEntry(
            string server, 
            string platform, 
            string fromVersion, 
            string toVersion, 
            string routeTarget, 
            RouteMode routeMode
        )
        {
            Server = server;
            Platform = platform;
            FromVersion = fromVersion;
            ToVersion = toVersion;
            RouteTarget = routeTarget;
            RouteMode = routeMode;
        }
    }
}