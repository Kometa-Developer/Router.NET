namespace Router.Model.Configuration
{
    public class RoutingConfiguration
    {
        public RoutingConfigurationEntry[] Entries { get; }
        
        public RoutingConfiguration(RoutingConfigurationEntry[] entries)
        {
            Entries = entries;
        }
    }
}