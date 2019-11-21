using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Dao
{
    public class BaseDao<T> where T:new()
    {



        public SessionFactory _sessionFactory { get; set; } = new SessionFactory();

        public int Insert(T entity)
        {
            return 1;
        }

        public int Delete(int id)
        {
            return 1;
        }

        public int Update(T entity)
        {
            return 1;
        }

        public int Select(Func<T, bool> func)
        {
            return 1;
        }

        public T Get(int id)
        {
            return new T();
        }
    }
}
