﻿using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.interfaces
{
    public  interface IAsyncRepository<T>where T:BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate );

        Task<T> GetByIdAsync(int id );
        Task<T> AddAsync(T  entity);
        Task UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);




    }
}
