using Infrastructure.EventBus.Generic.IntegrationEvents;

namespace Authentication.IntegrationEvents.Events
{
    public class CreatorRegisteredIntegrationEvent: IntegrationEvent
    {
        public required int UserId { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
    }
}
