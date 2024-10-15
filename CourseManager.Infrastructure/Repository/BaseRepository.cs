using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseManager.Application.Repository;
using CourseManager.Infrastructure.Context;
using CourseManager.Infrastructure.Extensions;

namespace CourseManager.Infrastructure.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public BaseRepository(ApplicationDbContext context) => _context = context;

        public virtual T Add(T item)
        {
            _context.Set<T>().Add(item);
            _context.SaveChanges();
            return item;
        }

        public virtual void Delete<TKey>(TKey id)
        {
            var dbItem = _context.Set<T>().Find(id);
            _context.Set<T>().Remove(dbItem);
            _context.SaveChanges();
        }

        public System.Collections.Generic.IEnumerable<T> Find() => _context.Set<T>();

        public T Find<TKey>(TKey id) => _context.Set<T>().Find(id);

        public virtual T Update<TKey>(TKey id, T item)
        {
            var dbItem = _context.Set<T>().Find(id);
            if (dbItem == null) throw new System.Exception("Could not find requested item");
            var props = item.GetType().GetProperties();
            foreach (var prop in props)
            {
                object propValue = prop.GetValue(item);
                if (propValue != null && propValue.GetType().IsPrimitive())
                    prop.SetValue(dbItem, propValue);
            }
            _context.SaveChanges();
            return item;
        }
    }
}
