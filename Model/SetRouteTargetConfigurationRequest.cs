using Router.Model.Configuration;

namespace Router.Model
{
    public class SetRouteTargetConfigurationRequest
    {
        public RouteTargetConfiguration Configuration { get; }
        
        public SetRouteTargetConfigurationRequest(RouteTargetConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}