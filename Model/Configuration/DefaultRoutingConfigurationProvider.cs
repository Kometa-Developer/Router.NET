namespace Router.Model.Configuration
{
    public static class DefaultRoutingConfigurationProvider
    {
        public static RouteTargetConfiguration RouteTargetConfiguration => new RouteTargetConfiguration(
            new[]
            {
                new RouteTargetConfigurationEntry(
                    "development",
                    "http://development.cloud",
                    false
                ),
                new RouteTargetConfigurationEntry(
                    "review",
                    "http://review.cloud",
                    false
                ),
                new RouteTargetConfigurationEntry(
                    "production",
                    "https://production.cloud",
                    false
                )
            }
        );

        public static RoutingConfiguration RoutingConfiguration => new RoutingConfiguration(
            new[]
            {
                new RoutingConfigurationEntry(
                    "development",
                    "ios",
                    "0.0.0",
                    "development",
                    UpdateMode.UpToDate
                ),
                new RoutingConfigurationEntry(
                    "development",
                    "android",
                    "0.0.0",
                    "development",
                    UpdateMode.UpToDate
                ),

                new RoutingConfigurationEntry(
                    "review",
                    "ios",
                    "0.0.0",
                    "review",
                    UpdateMode.UpToDate
                ),
                new RoutingConfigurationEntry(
                    "review",
                    "android",
                    "0.0.0",
                    "review",
                    UpdateMode.UpToDate
                ),

                new RoutingConfigurationEntry(
                    "production",
                    "ios",
                    "0.0.0",
                    "production",
                    UpdateMode.UpToDate
                ),
                new RoutingConfigurationEntry(
                    "production",
                    "android",
                    "0.0.0",
                    "production",
                    UpdateMode.UpToDate
                )
            }
        );
    }
}