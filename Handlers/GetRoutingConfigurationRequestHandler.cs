using System.Threading.Tasks;
using Router.ConfigurationStorage;
using Router.Handlers.Abstraction;
using Router.Model;

namespace Router.Handlers
{
    public class GetRoutingConfigurationRequestHandler 
        : RequestHandler<GetRoutingConfigurationRequest, GetRoutingConfigurationResponse, GetRoutingConfigurationResponse.ReturnCode>
    {
        protected override GetRoutingConfigurationResponse.ReturnCode Ok => GetRoutingConfigurationResponse.ReturnCode.Ok;
        protected override GetRoutingConfigurationResponse.ReturnCode BadRequest => GetRoutingConfigurationResponse.ReturnCode.BadRequest;
        protected override GetRoutingConfigurationResponse.ReturnCode InternalServerError => GetRoutingConfigurationResponse.ReturnCode.InternalServerError;
        
        protected override Task<GetRoutingConfigurationResponse.ReturnCode> Validate(
            GetRoutingConfigurationRequest request, IRoutingConfigurationReadOnlyStorage storage)
        {
            return Task.FromResult(Ok);
        }

        protected override GetRoutingConfigurationResponse RespondWithError(GetRoutingConfigurationResponse.ReturnCode errorCode)
        {
            return new GetRoutingConfigurationResponse(errorCode, null);
        }

        protected override async Task<GetRoutingConfigurationResponse> ProcessValidRequest(
            GetRoutingConfigurationRequest request,
            IRoutingConfigurationStorage configurationStorage
        )
        {
            var configuration = await configurationStorage.GetRoutingConfiguration();
            return new GetRoutingConfigurationResponse(Ok, configuration);
        }
    }
}