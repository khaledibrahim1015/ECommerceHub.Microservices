using MediatR;

namespace Ordering.Application.Commands;

public class DeleteOrderCommand : IRequest<Unit>
{
    public DeleteOrderCommand(int id)
    {
        Id = id;
    }
    public DeleteOrderCommand()
    {

    }
    public int Id { get; set; }
}
