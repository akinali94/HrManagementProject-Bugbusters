using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Abstract
{
    public interface IService<T> where T : class
    {
        Task TAddAsync(T entity);


        void TDelete(T entity);


        Task<IEnumerable<T>> TGetAllAsync(Expression<Func<T, bool>>? filter = null);


        T TGetById(string id);



        void TUpdate(T entity);
        Employee GetByIdEmployee(string id);
    }
}
