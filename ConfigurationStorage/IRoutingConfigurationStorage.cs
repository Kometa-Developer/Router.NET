using System.Threading.Tasks;
using Router.Model.Configuration;

namespace Router.ConfigurationStorage
{
    public interface IRoutingConfigurationStorage
    {
        Task<RoutingConfiguration> GetRoutingConfiguration();
        Task SetRoutingConfiguration(RoutingConfiguration configuration);

        Task<RouteTargetConfiguration> GetRouteTargetConfiguration();
        Task SetRouteTargetConfiguration(RouteTargetConfiguration configuration);
        
        Task BeginServerMaintenance(string target);
        Task FinishServerMaintenance(string target);

        Task<RouteTargetConfigurationEntry> GetRouteTarget(string target);
        Task<RoutingConfigurationEntry[]> GetRouting(string serverType);
    }
}