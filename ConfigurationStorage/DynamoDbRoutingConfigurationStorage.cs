using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Router.ConfigurationStorage.DynamoDb;
using Router.Model.Configuration;

namespace Router.ConfigurationStorage
{
    public class DynamoDbRoutingConfigurationStorage : IRoutingConfigurationStorage
    {
        public const string RoutingConfigurationTable = "router-configuration";
        public const string RouteTargetsTable = "router-targets";

        private readonly AmazonDynamoDBClient _client;
        private readonly DynamoDBContext _context;

        private const string TablePrefix = "router-";
        private const string AWSAccessKeyId = "";
        private const string AWSSecret = "";
        
        private readonly RegionEndpoint _endpoint = RegionEndpoint.EUCentral1;
        
        public DynamoDbRoutingConfigurationStorage()
        {
            var config = new AmazonDynamoDBConfig
            {
                RegionEndpoint = _endpoint,
                Timeout = TimeSpan.FromSeconds(15)
            };
            _client = new AmazonDynamoDBClient(
                AWSAccessKeyId, 
                AWSSecret, 
                config
            );
            _context = new DynamoDBContext(_client, new DynamoDBContextConfig { TableNamePrefix = TablePrefix });
        }
        
        public async Task<RoutingConfiguration> GetRoutingConfiguration()
        {
            await EnsureRoutingConfigurationTableCreated(TablePrefix);

            var search = _context.ScanAsync<RoutingDocument>(RoutingDocument.SearchAll());
            var documents = await search.GetRemainingAsync();
            var routes = documents
                .Select(FromDocument)
                .OrderBy(x => x.Server)
                .ThenBy(x => int.Parse(x.ClientVersion.Substring(x.ClientVersion.LastIndexOf(".", StringComparison.Ordinal) + 1)))
                .ThenBy(x => x.Platform)
                .ToArray();
            
            return new RoutingConfiguration(routes);
        }

        public async Task SetRoutingConfiguration(RoutingConfiguration configuration)
        {
            await EnsureRoutingConfigurationTableCreated(TablePrefix);
            
            var search = _context.ScanAsync<RoutingDocument>(RoutingDocument.SearchAll());
            var documents = await search.GetRemainingAsync();

            foreach (var document in documents)
                await _context.DeleteAsync(document);

            var nextDocuments = configuration.Entries.Select(ToDocument).ToArray();

            foreach (var nextDocument in nextDocuments)
                await _context.SaveAsync(nextDocument);
        }

        public async Task<RouteTargetConfiguration> GetRouteTargetConfiguration()
        {
            await EnsureRouteTargetsTableCreated(TablePrefix);

            var search = _context.ScanAsync<RouteTargetDocument>(RouteTargetDocument.SearchAll());
            var documents = await search.GetRemainingAsync();
            var targets = documents
                .Select(FromDocument)
                .OrderBy(x => x.Target)
                .ToArray();
            
            return new RouteTargetConfiguration(targets);
        }

        public async Task SetRouteTargetConfiguration(RouteTargetConfiguration configuration)
        {
            await EnsureRouteTargetsTableCreated(TablePrefix);
            
            var search = _context.ScanAsync<RouteTargetDocument>(RouteTargetDocument.SearchAll());
            var documents = await search.GetRemainingAsync();

            foreach (var document in documents)
                await _context.DeleteAsync(document);
            
            var nextDocuments = configuration.Entries.Select(ToDocument).ToArray();

            foreach (var nextDocument in nextDocuments)
                await _context.SaveAsync(nextDocument);
        }

        public async Task BeginServerMaintenance(string target)
        {
            await EnsureRouteTargetsTableCreated(TablePrefix);

            var search = _context.ScanAsync<RouteTargetDocument>(RouteTargetDocument.SearchRouteTarget(target));
            var documents = await search.GetRemainingAsync();
            var targetDocument = documents.First();

            targetDocument.Maintenance = true;

            await _context.SaveAsync(targetDocument);
        }

        public async Task FinishServerMaintenance(string target)
        {
            await EnsureRouteTargetsTableCreated(TablePrefix);

            var search = _context.ScanAsync<RouteTargetDocument>(RouteTargetDocument.SearchRouteTarget(target));
            var documents = await search.GetRemainingAsync();
            var targetDocument = documents.First();

            targetDocument.Maintenance = false;

            await _context.SaveAsync(targetDocument);
        }

        public async Task<RouteTargetConfigurationEntry> GetRouteTarget(string target)
        {
            await EnsureRouteTargetsTableCreated(TablePrefix);

            var search = _context.ScanAsync<RouteTargetDocument>(RouteTargetDocument.SearchRouteTarget(target));
            var documents = await search.GetRemainingAsync();
            var targetDocument = documents.First();

            return FromDocument(targetDocument);
        }

        public async Task<RoutingConfigurationEntry[]> GetRouting(string serverType)
        {
            await EnsureRoutingConfigurationTableCreated(TablePrefix);
            
            var search = _context.ScanAsync<RoutingDocument>(RoutingDocument.SearchServerType(serverType));
            var documents = await search.GetRemainingAsync();

            return documents
                .Select(FromDocument)
                .ToArray();
        }

        private async Task EnsureRoutingConfigurationTableCreated(string tablePrefix)
        {
            var name = $"{TablePrefix}{RoutingConfigurationTable}";
            var tablesList = await _client.ListTablesAsync();
            if (tablesList.TableNames.Contains(name))
                return;

            await _client.CreateTableAsync(RoutingDocument.CreateRequest(tablePrefix));
        }
        
        private async Task EnsureRouteTargetsTableCreated(string tablePrefix)
        {
            var name = $"{TablePrefix}{RouteTargetsTable}";
            var tablesList = await _client.ListTablesAsync();
            if (tablesList.TableNames.Contains(name))
                return;

            await _client.CreateTableAsync(RouteTargetDocument.CreateRequest(tablePrefix));
        }

        private static RouteTargetConfigurationEntry FromDocument(RouteTargetDocument arg)
        {
            return new RouteTargetConfigurationEntry(
                arg.Target,
                arg.Address,
                arg.Maintenance
            );
        }

        private static RouteTargetDocument ToDocument(RouteTargetConfigurationEntry arg)
        {
            return new RouteTargetDocument(
                arg.Target,
                arg.Address,
                arg.Maintenance
            );
        }
        
        private static RoutingConfigurationEntry FromDocument(RoutingDocument arg)
        {
            return new RoutingConfigurationEntry(
                arg.Server,
                arg.Platform,
                arg.ClientVersion,
                arg.RouteTarget,
                arg.UpdateMode
            );
        }

        private static RoutingDocument ToDocument(RoutingConfigurationEntry arg)
        {
            return new RoutingDocument(
                arg.Server,
                arg.Platform,
                arg.ClientVersion,
                arg.RouteTarget,
                arg.UpdateMode
            );
        }
    }
}