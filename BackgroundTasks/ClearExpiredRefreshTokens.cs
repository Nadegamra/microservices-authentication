namespace Authentication.BackgroundTasks
{
    public class ClearExpiredRefreshTokens : BackgroundService
    {
        private readonly ILogger<ClearExpiredRefreshTokens> _logger;
        private readonly IServiceProvider _services;

        public ClearExpiredRefreshTokens(ILogger<ClearExpiredRefreshTokens> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await DeleteExpired(stoppingToken);
        }

        private async Task DeleteExpired(CancellationToken stoppingToken)
        {
            int amount = 0;
            using(var scope = _services.CreateScope())
            {
                var _authDbContext = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
                var expiredTokens = _authDbContext.UserTokens.Where(x => x.RefreshExpiry < DateTime.UtcNow).ToList();
                amount = expiredTokens.Count;

                _authDbContext.RemoveRange(expiredTokens);
                await _authDbContext.SaveChangesAsync(stoppingToken);
            }
            _logger.Log(LogLevel.Information, $"Removed {amount} expired tokens");
            await Task.Delay(new TimeSpan(1,0,0), stoppingToken);
        }
    }
}
