using Ordering.Core.Entities;

namespace Ordering.Application.Interfaces;

public interface IQueryMapper<TEntity, TResult> :IMapper
    where TEntity : class 
    where TResult : class
{
    TResult MapFromEntity(TEntity entity);

}
