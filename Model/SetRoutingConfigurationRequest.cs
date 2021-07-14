using Router.Model.Configuration;

namespace Router.Model
{
    public class SetRoutingConfigurationRequest
    {
        public RoutingConfiguration Configuration { get; }
        
        public SetRoutingConfigurationRequest(RoutingConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}