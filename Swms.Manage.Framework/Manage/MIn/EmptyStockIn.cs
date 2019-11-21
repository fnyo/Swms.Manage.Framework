using Swms.Manage.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Manage.MIn
{


    /// <summary>
    /// 空托盘入库
    /// </summary>
    public class EmptyStockIn : ManageIn
    {
        protected override bool BeforeCreateCheck(int manageId, out string sResult)
        {
            return base.BeforeCreateCheck(manageId, out sResult);
        }

        protected override void AllocateCell(int manageId)
        {
            Console.WriteLine("分配到空托盘区域!");
        }

        //下达指令给wcs
        protected override bool InstructCommand(int manageId)
        {
            //我想提高空托盘入库的优先级

            Console.WriteLine("提高空托盘入库类型的优先级");

            return true;
        }
        protected override void HandleStock(int manageId)
        {
            //
            Console.WriteLine("增加空托盘入库");
        }

    }
}
