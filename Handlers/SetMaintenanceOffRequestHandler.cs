using System.Threading.Tasks;
using Router.ConfigurationStorage;
using Router.Handlers.Abstraction;
using Router.Model;

namespace Router.Handlers
{
    public class SetMaintenanceOffRequestHandler 
        : RequestHandler<SetMaintenanceOffRequest, SetMaintenanceOffResponse, SetMaintenanceOffResponse.ReturnCode>
    {
        protected override SetMaintenanceOffResponse.ReturnCode Ok => SetMaintenanceOffResponse.ReturnCode.Ok;
        protected override SetMaintenanceOffResponse.ReturnCode BadRequest => SetMaintenanceOffResponse.ReturnCode.BadRequest;
        protected override SetMaintenanceOffResponse.ReturnCode InternalServerError => SetMaintenanceOffResponse.ReturnCode.InternalServerError;
        
        protected override SetMaintenanceOffResponse.ReturnCode Validate(SetMaintenanceOffRequest request)
        {
            return string.IsNullOrEmpty(request.RouteTarget.Trim())
                ? SetMaintenanceOffResponse.ReturnCode.BadRequest
                : SetMaintenanceOffResponse.ReturnCode.Ok;
        }

        protected override SetMaintenanceOffResponse RespondWithError(SetMaintenanceOffResponse.ReturnCode errorCode)
        {
            return new SetMaintenanceOffResponse(errorCode);
        }

        protected override async Task<SetMaintenanceOffResponse> ProcessValidRequest(
            SetMaintenanceOffRequest request,
            IRoutingConfigurationStorage configurationStorage
        )
        {
            await configurationStorage.FinishServerMaintenance(request.RouteTarget);
            return new SetMaintenanceOffResponse(SetMaintenanceOffResponse.ReturnCode.Ok);
        }
    }
}