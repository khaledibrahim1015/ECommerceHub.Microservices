namespace EventBus.Messages.Events;

public class BaseIntegrationEvent
{
    public string Id { get; private set; }
    public DateTime CreationDate { get; private set; }
    public BaseIntegrationEvent()
    {
        Id = Guid.NewGuid().ToString();
        CreationDate = DateTime.UtcNow;
    }

    public BaseIntegrationEvent(string id, DateTime creationDate)
    {
        Id = id;
        CreationDate = creationDate;
    }
}
