using System.Threading.Tasks;
using Router.Model.Configuration;

namespace Router.ConfigurationStorage
{
    public interface IRoutingConfigurationStorage : IRoutingConfigurationReadOnlyStorage
    {
        Task SetRoutingConfiguration(RoutingConfiguration configuration);
        Task SetRouteTargetConfiguration(RouteTargetConfiguration configuration);
        Task BeginServerMaintenance(string target);
        Task FinishServerMaintenance(string target);
    }

    public interface IRoutingConfigurationReadOnlyStorage
    {
        Task<RoutingConfiguration> GetRoutingConfiguration();
        Task<RouteTargetConfiguration> GetRouteTargetConfiguration();
        Task<RouteTargetConfigurationEntry> GetRouteTarget(string target);
        Task<RoutingConfigurationEntry[]> GetRouting(string serverType);
    }
}