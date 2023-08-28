using BugBustersHR.BLL.Services.Abstract;
using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract;
using BugBustersHR.ENTITY.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.BLL.Services.Concrete
{
    public class Service<T> : IService<T> where T : class
    {
        protected readonly IBaseRepository<T> _repository;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly HrDb _db;

        public Service(IBaseRepository<T> repository, IUnitOfWork unitOfWork, HrDb db)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public Employee GetByIdEmployee(string id)
        {
            return _repository.GetByIdEmployee(id);
        }

        public async Task TAddAsync(T entity)
        {
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveAsync();
        }

        public void TDelete(T entity)
        {
            _repository.Delete(entity);
             _unitOfWork.Save();
        }

        public async Task<IEnumerable<T>> TGetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            return await _repository.GetAllAsync(filter);
        }

        public T TGetById(string id)
        {
            return _repository.GetById(id);
        }

        public void TUpdate(T entity)
        {
            _repository.Update(entity);
            _db.SaveChanges();
            //_unitOfWork.Save();

        }
    }
}
