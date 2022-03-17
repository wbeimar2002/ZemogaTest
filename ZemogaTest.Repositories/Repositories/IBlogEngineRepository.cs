using System.Collections.Generic;
using ZemogaTest.Utilities.Entities;

namespace ZemogaTest.Repositories.Repositories
{
    public interface IBlogEngineRepository<T> where T : BaseEntity
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        bool Add(T entity);
        public bool Update(T entity);
        public T AddWithReturn(T entity);

        public IEnumerable<T> GetAllWithInclude(string table);
        public IEnumerable<T> GetAllWithInclude(string table, string table2);
    }
}