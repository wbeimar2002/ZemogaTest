using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZemogaTest.Utilities.Dtos;
using ZemogaTest.Utilities.Entities;

namespace ZemogaTest.Repositories.Repositories
{
    public class BlogEngineRepository<T>: IBlogEngineRepository<T> where T: BaseEntity
    {
        private readonly ZemogaTestContext  _dbContext;
        
        public BlogEngineRepository()
        {
            _dbContext = new ZemogaTestContext();
        }

        public bool Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            var changes = _dbContext.SaveChanges();

            return changes > 0;
        }

        public bool Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            var changes = _dbContext.SaveChanges();

            return changes > 0;
        }
        public T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbContext.Set<T>().AsEnumerable();
        }

        public IEnumerable<T> GetAllWithInclude(string table)
        {
            return _dbContext.Set<T>().Include(table).AsEnumerable();
        }
        public IEnumerable<T> GetAllWithInclude(string table, string table2)
        {
            return _dbContext.Set<T>().Include(table).Include(table2).AsEnumerable();
        }
        public T AddWithReturn(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }
    }
}
