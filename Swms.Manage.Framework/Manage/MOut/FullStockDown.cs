using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Manage.MOut
{


    /// <summary>
    /// 实托盘下架
    /// </summary>
    public class FullStockDown:ManageOut
    {
        protected override void HandleStock(int manageId)
        {
            base.HandleStock(manageId);
            //移库
            BStockService.Move();
        }
    }
}
