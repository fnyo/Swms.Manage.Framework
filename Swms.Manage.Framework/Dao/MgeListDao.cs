using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Dao
{
    public class MgeListDao<T> : BaseDao<T> where T:class,new()
    {
        public List<T> MgeListsByMgeId(int mgeId)
        {
            List<T> mgeLists = new List<T>();
            return mgeLists;
        }
    }
}
