namespace Ordering.Application.Interfaces;

public interface IEventCommandMapper<TEvent, TCommand> : IMapper
    where TEvent : class
    where TCommand : class
{
    TCommand MapToCommand(TEvent @event);
}