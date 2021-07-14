using System.Threading.Tasks;
using Router.ConfigurationStorage;
using Router.Handlers.Abstraction;
using Router.Model;

namespace Router.Handlers
{
    public class GetRouteTargetConfigurationRequestHandler
        : RequestHandler<GetRouteTargetConfigurationRequest, GetRouteTargetConfigurationResponse, GetRouteTargetConfigurationResponse.ReturnCode>
    {
        protected override GetRouteTargetConfigurationResponse.ReturnCode Ok => GetRouteTargetConfigurationResponse.ReturnCode.Ok;
        protected override GetRouteTargetConfigurationResponse.ReturnCode BadRequest => GetRouteTargetConfigurationResponse.ReturnCode.BadRequest;
        protected override GetRouteTargetConfigurationResponse.ReturnCode InternalServerError => GetRouteTargetConfigurationResponse.ReturnCode.InternalServerError;
        
        protected override GetRouteTargetConfigurationResponse.ReturnCode Validate(GetRouteTargetConfigurationRequest request)
        {
            return GetRouteTargetConfigurationResponse.ReturnCode.Ok;
        }

        protected override GetRouteTargetConfigurationResponse RespondWithError(GetRouteTargetConfigurationResponse.ReturnCode errorCode)
        {
            return new GetRouteTargetConfigurationResponse(errorCode, null);
        }

        protected override async Task<GetRouteTargetConfigurationResponse> ProcessValidRequest(
            GetRouteTargetConfigurationRequest request, 
            IRoutingConfigurationStorage configurationStorage
        )
        {
            var configuration = await configurationStorage.GetRouteTargetConfiguration();
            return new GetRouteTargetConfigurationResponse(GetRouteTargetConfigurationResponse.ReturnCode.Ok, configuration);
        }
    }
}