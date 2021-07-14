using System.Threading.Tasks;
using Router.ConfigurationStorage;
using Router.Handlers.Abstraction;
using Router.Model;

namespace Router.Handlers
{
    public class SetRouteTargetConfigurationRequestHandler
        : RequestHandler<SetRouteTargetConfigurationRequest, SetRouteTargetConfigurationResponse, SetRouteTargetConfigurationResponse.ReturnCode>
    {
        protected override SetRouteTargetConfigurationResponse.ReturnCode Ok => SetRouteTargetConfigurationResponse.ReturnCode.Ok;
        protected override SetRouteTargetConfigurationResponse.ReturnCode BadRequest => SetRouteTargetConfigurationResponse.ReturnCode.BadRequest;
        protected override SetRouteTargetConfigurationResponse.ReturnCode InternalServerError => SetRouteTargetConfigurationResponse.ReturnCode.InternalServerError;
        
        protected override SetRouteTargetConfigurationResponse.ReturnCode Validate(SetRouteTargetConfigurationRequest request)
        {
            var entries = request.Configuration?.Entries;

            if (entries == null || entries.Length == 0)
                return SetRouteTargetConfigurationResponse.ReturnCode.BadConfiguration;

            foreach (var e in entries)
            {
                if (string.IsNullOrEmpty(e.Target.Trim()))
                    return SetRouteTargetConfigurationResponse.ReturnCode.BadConfiguration;

                if (string.IsNullOrEmpty(e.Address.Trim()))
                    return SetRouteTargetConfigurationResponse.ReturnCode.BadConfiguration;
            }

            return SetRouteTargetConfigurationResponse.ReturnCode.Ok;
        }

        protected override SetRouteTargetConfigurationResponse RespondWithError(SetRouteTargetConfigurationResponse.ReturnCode errorCode)
        {
            return new SetRouteTargetConfigurationResponse(errorCode, null);
        }

        protected override async Task<SetRouteTargetConfigurationResponse> ProcessValidRequest(
            SetRouteTargetConfigurationRequest request, 
            IRoutingConfigurationStorage configurationStorage
        )
        {
            await configurationStorage.SetRouteTargetConfiguration(request.Configuration);
            
            var updated = await configurationStorage.GetRouteTargetConfiguration();
            
            return new SetRouteTargetConfigurationResponse(SetRouteTargetConfigurationResponse.ReturnCode.Ok, updated);
        }
    }
}