using Swms.Manage.Framework.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Manage.MOut
{

    /// <summary>
    /// 计划出库  有计划的出库  配送出库，领料出库，退货等。
    /// </summary>
    public class HpFullStockOut:ManageOut
    {

        /// <summary>
        /// 存在计划
        /// </summary>
        /// <param name="bill"></param>
        public HpFullStockOut(IBill bill)
        {
            this._bill = bill;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public HpFullStockOut()
        {
            this._bill = new OutBil();
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
    }
}
