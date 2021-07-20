using System;
using System.Linq;
using System.Threading.Tasks;
using Router.ConfigurationStorage;
using Router.Contracts;
using Router.Handlers.Abstraction;
using Router.Model.Configuration;
using Version = Router.Contracts.Version;

namespace Router.Handlers
{
    public class GetServerAddressRequestHandler
        : RequestHandler<GetServerAddressRequest, GetServerAddressResponse, GetServerAddressResponse.ReturnCode>
    {
        protected override GetServerAddressResponse.ReturnCode Ok => GetServerAddressResponse.ReturnCode.Ok;

        protected override GetServerAddressResponse.ReturnCode BadRequest =>
            GetServerAddressResponse.ReturnCode.IncorrectVersion;

        protected override GetServerAddressResponse.ReturnCode InternalServerError =>
            GetServerAddressResponse.ReturnCode.InternalServerError;

        protected override Task<GetServerAddressResponse.ReturnCode> Validate(
            GetServerAddressRequest addressRequest, IRoutingConfigurationReadOnlyStorage storage)
        {
            if (!Version.TryParse(addressRequest.ClientVersion, out _))
                return Task.FromResult(GetServerAddressResponse.ReturnCode.IncorrectVersion);

            if (string.IsNullOrEmpty(addressRequest.Server))
                return Task.FromResult(GetServerAddressResponse.ReturnCode.IncorrectServer);

            if (string.IsNullOrEmpty(addressRequest.ClientPlatform))
                return Task.FromResult(GetServerAddressResponse.ReturnCode.IncorrectPlatform);

            return Task.FromResult(Ok);
        }

        protected override GetServerAddressResponse RespondWithError(GetServerAddressResponse.ReturnCode errorCode)
        {
            return new GetServerAddressResponse(errorCode, null);
        }

        protected override async Task<GetServerAddressResponse> ProcessValidRequest(
            GetServerAddressRequest addressRequest,
            IRoutingConfigurationStorage configurationStorage
        )
        {
            var routesArray = await configurationStorage.GetRouting(addressRequest.Server);

            if (routesArray.Length == 0)
                return new GetServerAddressResponse(GetServerAddressResponse.ReturnCode.IncorrectServer, null);

            routesArray = routesArray
                .GroupBy(x => x.Platform)
                .First(x => x.Key == addressRequest.ClientPlatform 
                            || addressRequest.ClientPlatform == "any")
                .ToArray();

            if (routesArray.Length == 0)
                return new GetServerAddressResponse(GetServerAddressResponse.ReturnCode.IncorrectPlatform, null);

            var version = Version.Parse(addressRequest.ClientVersion);
            
            var suitable = routesArray
                .FirstOrDefault(x => version >= Version.Parse(x.FromVersion) && version <= Version.Parse(x.ToVersion));

            if (suitable == null)
                return new GetServerAddressResponse(GetServerAddressResponse.ReturnCode.UpdateRequired, null);

            var suitableRouteTarget = await configurationStorage.GetRouteTarget(suitable.RouteTarget);

            var successCode = ToReturnCode(suitable.RouteMode);
            var serverAddress = ShouldRouteToServer(successCode)
                ? suitableRouteTarget.Address
                : null;

            return suitableRouteTarget.Maintenance
                ? new GetServerAddressResponse(GetServerAddressResponse.ReturnCode.Maintenance, null)
                : new GetServerAddressResponse(successCode, serverAddress);
        }

        private bool ShouldRouteToServer(GetServerAddressResponse.ReturnCode code)
        {
            switch (code)
            {
                case GetServerAddressResponse.ReturnCode.Ok:
                    return true;
                default:
                    return false;
            }
        }

        private GetServerAddressResponse.ReturnCode ToReturnCode(RouteMode mode)
        {
            switch (mode)
            {
                case RouteMode.Allow:
                    return GetServerAddressResponse.ReturnCode.Ok;
                case RouteMode.RequireUpdate:
                    return GetServerAddressResponse.ReturnCode.UpdateRequired;
                default:
                    throw new ArgumentException($"Unknown UpdateMode - ${mode}");
            }
        }
    }
}