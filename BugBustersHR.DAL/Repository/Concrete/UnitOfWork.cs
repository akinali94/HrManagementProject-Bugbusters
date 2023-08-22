using BugBustersHR.DAL.Context;
using BugBustersHR.DAL.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugBustersHR.DAL.Repository.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HrDb _hrDb;

        public UnitOfWork(HrDb hrDb)
        {
            _hrDb = hrDb;
        }

        public void Save()
        {
            _hrDb.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _hrDb.SaveChangesAsync();
        }
    }
}
