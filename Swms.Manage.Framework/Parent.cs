using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework
{
    public abstract class Parent
    {
        public virtual void Print()
        {
            this.Detail();
            Console.WriteLine("hello");
        }


        public virtual void Detail()
        {
            Console.WriteLine("BaseDetail");
        }

        public abstract void Abstract1();
    }
}
