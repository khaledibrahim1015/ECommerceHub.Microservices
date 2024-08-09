namespace Discount.Application.Interfaces;

public interface IQueryMapper<TEntity, TResult>: IMapper
    where TEntity : class
    where TResult : class  
{
    TResult MapFromEntity(TEntity entity);
}
