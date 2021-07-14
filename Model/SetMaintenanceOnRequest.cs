namespace Router.Model
{
    public class SetMaintenanceOnRequest
    {
        public string RouteTarget { get; }
        
        public SetMaintenanceOnRequest(string routeTarget)
        {
            RouteTarget = routeTarget;
        }
    }
}