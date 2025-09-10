using Fretefy.Test.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fretefy.Test.Infra.EntityFramework.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly TestDbContext _ctx;
        protected readonly DbSet<T> _set;

        public Repository(TestDbContext ctx)
        {
            _ctx = ctx;
            _set = _ctx.Set<T>();
        }

        public virtual T Get(Guid id) => _set.Find(id);

        public virtual IEnumerable<T> List() =>
            _set.AsNoTracking().ToList();

        public virtual T Add(T entity)
        {
            _set.Add(entity);
            _ctx.SaveChanges();
            return entity;
        }

        public virtual T Update(T entity)
        {
            _set.Update(entity);
            _ctx.SaveChanges();
            return entity;
        }

        public virtual void Delete(Guid id)
        {
            var existing = _set.Find(id);
            if (existing == null) return;
            _set.Remove(existing);
            _ctx.SaveChanges();
        }
    }
}
