using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Router.Model.Configuration;

namespace Router.Model
{
    public class GetRoutingConfigurationResponse
    {
        public enum ReturnCode
        {
            Ok,
            BadRequest,
            InternalServerError
        }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public ReturnCode Code { get; }
        
        public RoutingConfiguration Configuration { get; }
        
        public GetRoutingConfigurationResponse(ReturnCode code, RoutingConfiguration configuration)
        {
            Code = code;
            Configuration = configuration;
        }
    }
}