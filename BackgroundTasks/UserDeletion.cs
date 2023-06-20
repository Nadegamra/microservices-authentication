using Authentication.IntegrationEvents.Events;
using Infrastructure.EventBus.Generic;

namespace Authentication.BackgroundTasks
{
    public class UserDeletion : BackgroundService
    {
        private readonly ILogger<UserDeletion> _logger;
        private readonly IServiceProvider _services;

        public UserDeletion(ILogger<UserDeletion> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DeleteUsers(stoppingToken);
        }

        private async Task DeleteUsers(CancellationToken stoppingToken)
        {
            int amount = 0;
            using (var scope = _services.CreateScope())
            {
                var _authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
                var eventBus = scope.ServiceProvider.GetRequiredService<Infrastructure.EventBus.Generic.IEventBus>();
                var expiredUsers = _authDbContext.Users.Where(x => x.IsDeleted && x.DeletedAt < DateTime.UtcNow.AddDays(-30)).ToList();
                amount = expiredUsers.Count;


                _authDbContext.Users.RemoveRange(expiredUsers);
                await _authDbContext.SaveChangesAsync(stoppingToken);

                foreach (var user in expiredUsers)
                {
                    var userRoles = _authDbContext.UserRoles.Where(x => x.UserId == user.Id).Select(x => x.RoleId).ToList();
                    var roleNames = _authDbContext.Roles.Where(x => userRoles.Contains(x.Id)).Select(x => x.Name).ToList();
                    if (roleNames.Contains("Creator"))
                    {
                        var creatorDeletedIntegrationEvent = new CreatorDeletedIntegrationEvent
                        {
                            UserId = user.Id
                        };
                        eventBus.Publish(creatorDeletedIntegrationEvent);
                    }
                }
            }

            _logger.Log(LogLevel.Information, $"Removed {amount} deleted users");
            await Task.Delay(new TimeSpan(24, 0, 0), stoppingToken);
        }
    }
}