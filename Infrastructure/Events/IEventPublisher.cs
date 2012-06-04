namespace Infrastructure.Events
{
    public interface IEventPublisher
    {
        void Publish(IEvent @event);
    }
}