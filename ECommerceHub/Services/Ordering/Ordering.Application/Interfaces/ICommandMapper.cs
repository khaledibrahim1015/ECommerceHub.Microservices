namespace Ordering.Application.Interfaces;

public  interface ICommandMapper <TCommand , TEntity> :IMapper
    where TCommand : class
    where TEntity : class
{
    TEntity MapToEntity(TCommand command);
}
