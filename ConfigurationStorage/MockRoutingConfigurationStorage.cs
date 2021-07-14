using System.Linq;
using System.Threading.Tasks;
using Router.Model.Configuration;

namespace Router.ConfigurationStorage
{
    public class MockRoutingConfigurationStorage : IRoutingConfigurationStorage
    {
        public Task<RoutingConfiguration> GetRoutingConfiguration()
        {
            return Task.FromResult(DefaultRoutingConfigurationProvider.RoutingConfiguration);
        }

        public Task SetRoutingConfiguration(RoutingConfiguration configuration)
        {
            return Task.CompletedTask;
        }

        public Task<RouteTargetConfiguration> GetRouteTargetConfiguration()
        {
            return Task.FromResult(DefaultRoutingConfigurationProvider.RouteTargetConfiguration);
        }

        public Task SetRouteTargetConfiguration(RouteTargetConfiguration configuration)
        {
            return Task.CompletedTask;
        }

        public Task BeginServerMaintenance(string target)
        {
            return Task.CompletedTask;
        }

        public Task FinishServerMaintenance(string target)
        {
            return Task.CompletedTask;
        }

        public async Task<RouteTargetConfigurationEntry> GetRouteTarget(string target)
        {
            var routeTargets = await GetRouteTargetConfiguration();

            return routeTargets.Entries.First(x => x.Target == target);
        }

        public async Task<RoutingConfigurationEntry[]> GetRouting(string serverType)
        {
            var routes = await GetRoutingConfiguration();

            return routes.Entries
                .Where(x => x.Server == serverType)
                .ToArray();
        }
    }
}