using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Router.Model.Configuration;

namespace Router.ConfigurationStorage.Mongo
{
    public class RoutingEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        
        public string Server { get; set; }
        
        public string Platform { get; set; }
        
        public string FromVersion { get; set; }
        public string ToVersion { get; set; }
        
        public string RouteTarget { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)] 
        public RouteMode RouteMode { get; set; }
        
        public RoutingEntity()
        {
        }
        
        public RoutingEntity(
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