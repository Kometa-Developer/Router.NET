namespace Router.Model.Configuration
{
    public class RouteTargetConfigurationEntry
    {
        public string Target { get; }
        
        public string Address { get; }
        
        public bool Maintenance { get; }
        
        public RouteTargetConfigurationEntry(string target, string address, bool maintenance)
        {
            Target = target;
            Address = address;
            Maintenance = maintenance;
        }
    }
}