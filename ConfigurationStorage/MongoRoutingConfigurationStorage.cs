using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Router.ConfigurationStorage.Mongo;
using Router.Model.Configuration;

namespace Router.ConfigurationStorage
{
    public class MongoRoutingConfigurationStorage : IRoutingConfigurationStorage
    {
        private readonly RoutingDbContext _dbContext;

        public MongoRoutingConfigurationStorage(RoutingDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<RoutingConfiguration> GetRoutingConfiguration()
        {
            var entities = await _dbContext.Routes
                .AsQueryable()
                .ToListAsync();

            var entries = entities.Select(FromEntity).ToArray();
            
            return new RoutingConfiguration(entries);
        }

        public async Task SetRoutingConfiguration(RoutingConfiguration configuration)
        {
            var entities = configuration.Entries.Select(ToEntity).ToArray();

            await _dbContext.Routes.DeleteManyAsync(x => true);
            await _dbContext.Routes.InsertManyAsync(entities);
        }

        public async Task<RouteTargetConfiguration> GetRouteTargetConfiguration()
        {
            var entities = await _dbContext.RouteTargets
                .AsQueryable()
                .ToListAsync();

            var entries = entities.Select(FromEntity).ToArray();
            
            return new RouteTargetConfiguration(entries);
        }

        public async Task SetRouteTargetConfiguration(RouteTargetConfiguration configuration)
        {
            var entities = configuration.Entries.Select(ToEntity).ToArray();

            await _dbContext.RouteTargets.DeleteManyAsync(x => true);
            await _dbContext.RouteTargets.InsertManyAsync(entities);
        }

        public async Task BeginServerMaintenance(string target)
        {
            await _dbContext.RouteTargets.UpdateManyAsync(
                Builders<RouteTargetEntity>.Filter.Eq(x => x.Target, target),
                Builders<RouteTargetEntity>.Update.Set(x => x.Maintenance, true)
            );
        }

        public async Task FinishServerMaintenance(string target)
        {
            await _dbContext.RouteTargets.UpdateManyAsync(
                Builders<RouteTargetEntity>.Filter.Eq(x => x.Target, target),
                Builders<RouteTargetEntity>.Update.Set(x => x.Maintenance, false)
            );
        }

        public async Task<RouteTargetConfigurationEntry> GetRouteTarget(string target)
        {
            var query = await _dbContext.RouteTargets
                .AsQueryable()
                .Where(x => x.Target == target)
                .ToListAsync();

            var entity = query.First();

            return FromEntity(entity);
        }

        public async Task<RoutingConfigurationEntry[]> GetRouting(string serverType)
        {
            var query = await _dbContext.Routes
                .AsQueryable()
                .Where(x => x.Server == serverType)
                .ToListAsync();

            return query.Select(FromEntity).ToArray();
        }

        private static RoutingConfigurationEntry FromEntity(RoutingEntity entity)
        {
            return new RoutingConfigurationEntry(
                entity.Server,
                entity.Platform,
                entity.FromVersion,
                entity.ToVersion,
                entity.RouteTarget,
                entity.RouteMode
            );
        }
        
        private static RoutingEntity ToEntity(RoutingConfigurationEntry entry)
        {
            return new RoutingEntity(
                entry.Server,
                entry.Platform,
                entry.FromVersion,
                entry.ToVersion,
                entry.RouteTarget,
                entry.RouteMode
            );

        }
        
        private static RouteTargetConfigurationEntry FromEntity(RouteTargetEntity entity)
        {
            return new RouteTargetConfigurationEntry(
                entity.Target,
                entity.Address,
                entity.Maintenance
            );
        }
        
        private static RouteTargetEntity ToEntity(RouteTargetConfigurationEntry entry)
        {
            return new RouteTargetEntity(
                entry.Target,
                entry.Address,
                entry.Maintenance
            );
        }
    }
}