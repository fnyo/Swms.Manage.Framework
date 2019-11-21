using Swms.Manage.Framework.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Manage.MIn
{


    /// <summary>
    /// 计划组盘入库
    /// </summary>
    public class HpFullStockIn:ManageIn
    {

        //开放封闭原则

        public HpFullStockIn()
        {
            base._bill = new InBill();
        }



        //计划组盘入库   重写即可  对于无计划的无需重写
        protected override void UpdateBillCreate(int manageId, string updateType)
        {
            _bill.Create();
        }


        protected override void UpdateBillExecute(int manageId, string updateType)
        {
            _bill.Begin();
        }


        protected override void UpdateBillComplete(int manageId, string updateType)
        {
            _bill.Finish();
        }


        protected override void FeedBack(int manageId)
        {
            Console.WriteLine("将组盘入库的物料以及托盘信息反馈至MES");
        }


    }
}
