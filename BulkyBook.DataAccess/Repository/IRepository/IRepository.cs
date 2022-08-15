using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class // T va a ser una clase cualquiera
    {
        // add, find, update, remove
        // para update la logica seria distinta para c/clase,xlo q no se poneen los repository
        //var categoryFromDbFirst = _db.Categories.FirstOrDefault(c => c.Id == id);
        // string? includeProperties = null, es para q conecte las foreign keys
        // con la contraparte y salga el valor en lugar del id
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        IEnumerable<T> GetAll(string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
