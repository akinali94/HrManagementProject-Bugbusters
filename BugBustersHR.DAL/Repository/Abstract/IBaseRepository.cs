using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Abstract
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T entity);


        void Delete(T entity);


        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);


        T GetById(string id);



        void Update(T entity);
    }
}
