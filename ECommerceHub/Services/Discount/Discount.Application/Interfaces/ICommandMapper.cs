using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Interfaces
{
    public interface ICommandMapper<TCommand, TEntity> : IMapper
     where TCommand : class  
     where TEntity : class   
    {
        TEntity MapToEntity(TCommand command);
    }
}
