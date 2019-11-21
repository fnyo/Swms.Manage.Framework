using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Manage.MOut
{

    /// <summary>
    /// 无计划出库  拣选出库  人工出库   整出
    /// </summary>
    public class NpFullStockOut:ManageOut
    {
        protected override void HandleStock(int manageId)
        {


            base.HandleStock(manageId);
            //扣除库存
            BStockService.Delete();
            //

        }
    }
}
