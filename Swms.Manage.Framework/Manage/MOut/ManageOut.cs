using Swms.Manage.Framework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Manage.MOut
{


    /// <summary>
    /// 出库
    /// </summary>
    public abstract class ManageOut:ManageBase
    {
        protected override void AllocateCell(int manageId)
        {
            throw new NotImplementedException();
        }

        protected override bool BeforeCreateCheck(int manageId, out string sResult)
        {
            return base.BeforeCreateCheck(manageId, out sResult);
        }

        protected override bool InstructCommand(int manageId)
        {
            manageId = 1;
            return true;
        }


        protected override void HandleStock(int manageId)
        {
            Cell cell = cellDao.Get(1);
            cell.CellStatus = "Nohave";
            cellDao.Update(cell);

        }
    }
}
