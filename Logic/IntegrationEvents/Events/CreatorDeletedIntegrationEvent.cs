using Infrastructure.EventBus.Generic.IntegrationEvents;

namespace Authentication.IntegrationEvents.Events
{
    public class CreatorDeletedIntegrationEvent : IntegrationEvent
    {
        public required int UserId { get; set; }
    }
}