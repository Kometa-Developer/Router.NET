using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Router.Model.Configuration;

namespace Router.Model
{
    public class SetRouteTargetConfigurationResponse
    {
        public enum ReturnCode
        {
            Ok,
            BadRequest,
            BadConfiguration,
            InternalServerError
        }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public ReturnCode Code { get; }
        
        public RouteTargetConfiguration Configuration { get; }
        
        public SetRouteTargetConfigurationResponse(ReturnCode code, RouteTargetConfiguration configuration)
        {
            Code = code;
            Configuration = configuration;
        }
    }
}