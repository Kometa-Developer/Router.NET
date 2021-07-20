using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Router.ConfigurationStorage;
using Router.Contracts;
using Router.Handlers.Abstraction;
using Router.Model;
using Router.Model.Configuration;
using Version = Router.Contracts.Version;

namespace Router.Handlers
{
    public class SetRoutingConfigurationRequestHandler 
        : RequestHandler<SetRoutingConfigurationRequest, SetRoutingConfigurationResponse, SetRoutingConfigurationResponse.ReturnCode>
    {
        protected override SetRoutingConfigurationResponse.ReturnCode Ok => SetRoutingConfigurationResponse.ReturnCode.Ok;
        protected override SetRoutingConfigurationResponse.ReturnCode BadRequest => SetRoutingConfigurationResponse.ReturnCode.BadRequest;
        protected override SetRoutingConfigurationResponse.ReturnCode InternalServerError => SetRoutingConfigurationResponse.ReturnCode.InternalServerError;
        
        protected override async Task<SetRoutingConfigurationResponse.ReturnCode> Validate(
            SetRoutingConfigurationRequest request, IRoutingConfigurationReadOnlyStorage storage)
        {
            var entries = request.Configuration?.Entries;
            
            if (entries == null || entries.Length == 0)
            {
                return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
            }

            var routeTargets = await storage.GetRouteTargetConfiguration();

            var map = new Dictionary<string, List<VersionRange>>();
            
            foreach (var entry in entries)
            {
                if (string.IsNullOrEmpty(entry.Server))
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
                
                if (string.IsNullOrEmpty(entry.Platform))
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
                
                if (string.IsNullOrEmpty(entry.RouteTarget))
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;

                if (routeTargets.Entries.All(x => x.Target != entry.Server))
                {
                    Console.WriteLine($"Unknown {entry.Server} server received");
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
                }
                
                if (routeTargets.Entries.All(x => x.Target != entry.RouteTarget))
                {
                    Console.WriteLine($"Unknown {entry.RouteTarget} server received");
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
                }
                
                if (!Version.TryParse(entry.FromVersion, out var fromVersion))
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
                
                if (!Version.TryParse(entry.ToVersion, out var toVersion))
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
                
                if (entry.RouteMode == RouteMode.None)
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;

                var range = new VersionRange(fromVersion, toVersion);
                var key = entry.Server + entry.Platform;
                
                if (map.ContainsKey(key))
                {
                    var list = map[key];

                    if (!list.Any(x => x.IntersectsWith(range)))
                    {
                        map[key].Add(range);
                    }
                    else
                    {
                        Console.WriteLine($"{entry.Server} version ranges intersect");
                        return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
                    }
                }
                else
                {
                    map[key] = new List<VersionRange> { range };
                }
            }

            return Ok;
        }

        protected override SetRoutingConfigurationResponse RespondWithError(SetRoutingConfigurationResponse.ReturnCode errorCode)
        {
            return new SetRoutingConfigurationResponse(errorCode, null);
        }

        protected override async Task<SetRoutingConfigurationResponse> ProcessValidRequest(
            SetRoutingConfigurationRequest request,
            IRoutingConfigurationStorage configurationStorage
        )
        {
            await configurationStorage.SetRoutingConfiguration(request.Configuration);

            var updated = await configurationStorage.GetRoutingConfiguration();
            
            return new SetRoutingConfigurationResponse(SetRoutingConfigurationResponse.ReturnCode.Ok, updated);
        }
    }
}