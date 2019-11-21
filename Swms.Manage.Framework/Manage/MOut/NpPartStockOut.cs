using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Manage.MOut
{

    /// <summary>
    /// 拣选出库
    /// </summary>
    public class NpPartStockOut:ManageOut
    {
        protected override void HandleStock(int manageId)
        {
            base.HandleStock(manageId);
            //扣减库存
            BStockService.Remove();
            //移库
            BStockService.Move();
        }
    }
}
