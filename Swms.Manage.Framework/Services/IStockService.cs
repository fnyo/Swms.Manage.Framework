using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Services
{
    public interface IStockService
    {

        void Create();

        void Supply();

        void Remove();

        void Delete();

        void Move();
    }
}
