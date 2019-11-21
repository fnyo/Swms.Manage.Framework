using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework
{
    public class Children:Parent
    {
        public override void Detail()
        {
            Console.WriteLine("children's detail");
        }

        public override void Abstract1()
        {
            Console.WriteLine("Children's Abstract1");
        }
    }
}
