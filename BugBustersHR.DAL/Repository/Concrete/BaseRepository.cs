using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract;
using BugBustersHR.ENTITY.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Concrete
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly HrDb _hrDb;

        public BaseRepository(HrDb hrDb)
        {
            _hrDb = hrDb;
        }
        public async Task AddAsync(T entity)
        {
            await _hrDb.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _hrDb.Remove(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            return await _hrDb.Set<T>().Where(filter).ToListAsync();

        }


        public T GetById(string id)
        {
            return _hrDb.Set<T>().Find(id);
        }

        public void Update(T entity)
        {
            _hrDb.Update(entity);
        }
    }
}
