using System;
using System.Collections.Generic;

namespace Fretefy.Test.Domain.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        T Get(Guid id);
        IEnumerable<T> List();
        T Add(T entity);
        T Update(T entity);
        void Delete(Guid id);
    }
}
