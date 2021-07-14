using System.Threading.Tasks;
using Router.ConfigurationStorage;
using Router.Contracts;
using Router.Handlers.Abstraction;
using Router.Model;
using Router.Model.Configuration;

namespace Router.Handlers
{
    public class SetRoutingConfigurationRequestHandler 
        : RequestHandler<SetRoutingConfigurationRequest, SetRoutingConfigurationResponse, SetRoutingConfigurationResponse.ReturnCode>
    {
        protected override SetRoutingConfigurationResponse.ReturnCode Ok => SetRoutingConfigurationResponse.ReturnCode.Ok;
        protected override SetRoutingConfigurationResponse.ReturnCode BadRequest => SetRoutingConfigurationResponse.ReturnCode.BadRequest;
        protected override SetRoutingConfigurationResponse.ReturnCode InternalServerError => SetRoutingConfigurationResponse.ReturnCode.InternalServerError;
        
        protected override SetRoutingConfigurationResponse.ReturnCode Validate(SetRoutingConfigurationRequest request)
        {
            var entries = request.Configuration?.Entries;
            if (entries == null || entries.Length == 0)
                return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;

            foreach (var entry in entries)
            {
                if (string.IsNullOrEmpty(entry.Server))
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
                
                if (string.IsNullOrEmpty(entry.Platform))
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
                
                if (string.IsNullOrEmpty(entry.RouteTarget.Trim()))
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
                
                if (!ClientBuildVersion.TryParse(entry.ClientVersion, out _))
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
                
                if (entry.UpdateMode == UpdateMode.None)
                    return SetRoutingConfigurationResponse.ReturnCode.BadConfiguration;
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