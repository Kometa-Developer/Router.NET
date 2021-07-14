using Amazon;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Router.ConfigurationStorage;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Router
{
    public class Functions
    {
        private const string Token = "token";
        
        private readonly IRoutingConfigurationStorage _storage = new MockRoutingConfigurationStorage();
        
        public Functions()
        {
        }

        public APIGatewayProxyResponse GetServerAddress(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return RouterApi.GetServerAddress(request, _storage).Result;
        }
        
        public APIGatewayProxyResponse GetRoutingConfiguration(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return RouterApi.Authorize(Token, request.Headers) 
                   ?? RouterApi.GetRoutingConfiguration(request, _storage).Result;
        }
        
        public APIGatewayProxyResponse SetRoutingConfiguration(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return RouterApi.Authorize(Token, request.Headers) 
                   ?? RouterApi.SetRoutingConfiguration(request, _storage).Result;
        }
        
        public APIGatewayProxyResponse GetRouteTargetConfiguration(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return RouterApi.Authorize(Token, request.Headers) 
                   ?? RouterApi.GetRouteTargetConfiguration(request, _storage).Result;
        }
        
        public APIGatewayProxyResponse SetRouteTargetConfiguration(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return RouterApi.Authorize(Token, request.Headers) 
                   ?? RouterApi.SetRouteTargetConfiguration(request, _storage).Result;
        }
        
        public APIGatewayProxyResponse SetMaintenanceOn(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return RouterApi.Authorize(Token, request.Headers) 
                   ?? RouterApi.SetMaintenanceOn(request, _storage).Result;
        }
        
        public APIGatewayProxyResponse SetMaintenanceOff(APIGatewayProxyRequest request, ILambdaContext context)
        {
            return RouterApi.Authorize(Token, request.Headers) 
                   ?? RouterApi.SetMaintenanceOff(request, _storage).Result;
        }
    }
}