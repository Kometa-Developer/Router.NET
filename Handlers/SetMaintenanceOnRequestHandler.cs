using System.Threading.Tasks;
using Router.ConfigurationStorage;
using Router.Handlers.Abstraction;
using Router.Model;

namespace Router.Handlers
{
    public class SetMaintenanceOnRequestHandler 
        : RequestHandler<SetMaintenanceOnRequest, SetMaintenanceOnResponse, SetMaintenanceOnResponse.ReturnCode>
    {
        protected override SetMaintenanceOnResponse.ReturnCode Ok => SetMaintenanceOnResponse.ReturnCode.Ok;
        protected override SetMaintenanceOnResponse.ReturnCode BadRequest => SetMaintenanceOnResponse.ReturnCode.BadRequest;
        protected override SetMaintenanceOnResponse.ReturnCode InternalServerError => SetMaintenanceOnResponse.ReturnCode.InternalServerError;
        
        protected override SetMaintenanceOnResponse.ReturnCode Validate(SetMaintenanceOnRequest request)
        {
            return string.IsNullOrEmpty(request.RouteTarget.Trim())
                ? SetMaintenanceOnResponse.ReturnCode.BadRequest
                : SetMaintenanceOnResponse.ReturnCode.Ok;
        }

        protected override SetMaintenanceOnResponse RespondWithError(SetMaintenanceOnResponse.ReturnCode errorCode)
        {
            return new SetMaintenanceOnResponse(errorCode);
        }

        protected override async Task<SetMaintenanceOnResponse> ProcessValidRequest(
            SetMaintenanceOnRequest request,
            IRoutingConfigurationStorage configurationStorage
        )
        {
            await configurationStorage.BeginServerMaintenance(request.RouteTarget);
            return new SetMaintenanceOnResponse(SetMaintenanceOnResponse.ReturnCode.Ok);
        }
    }
}