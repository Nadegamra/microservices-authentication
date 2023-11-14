using Infrastructure.EventBus.Generic.IntegrationEvents;

namespace Authentication.IntegrationEvents.Events
{
    public class UserNameChangedIntegrationEvent : IntegrationEvent
    {
        public required int UserId { get; set; }
        public required string OldUserName { get; set; }
        public required string NewUserName { get; set; }
    }
}
