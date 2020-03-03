namespace Core.EventStore.Registration
{
    public interface IPersistentSubscriptionClient
    {
        bool Start();
    }
}