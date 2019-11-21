using Swms.Manage.Framework.Model;
using Swms.Manage.Framework.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Manage.MIn
{


    //HpFullStockIn   有计划组盘入库
    //NpFullStockIn   无计划组盘入库
    //HpSupplyStockIn 有计划补盘入库
    //NpSupplyStockIn 无计划补盘入库
    //EmptyStockIn    空托盘入库
    //FullStockBack   实托盘回库





    public abstract class ManageIn : ManageBase
    {

        public ManageIn() : base() { }
        

        //创建之前的逻辑判断    利用《重构改善既有代码的接口》 359.函数上移   
        protected override bool BeforeCreateCheck(int manageId, out string sResult)
        {
            bool bresult = true;
            //基类的判断
            bresult = base.BeforeCreateCheck(manageId, out sResult);
            if (!bresult) { return bresult; }

            //入库的判断   任务类型是指定了库位
            if (manageId == 1)
            {
                if (ValidateEnum.CellError)
                {
                    bresult = false;
                    return bresult;
                }
            }
            return bresult;
        }

        //任务的创建  直接继承任务基类 流程固定  如果有变更，直接在该方法体内重写
        public override bool ManageCreate(MgeMain mgeMain, List<MgeList> mgeLists, bool bTrans, bool autoDownload, bool autoComplete, out string sResult)
        {
            return base.ManageCreate(mgeMain, mgeLists, bTrans, autoDownload, autoComplete, out sResult);
        }

        //任务的下达  直接继承任务基类 流程固定
        public override bool ManageDownload(int manageId, bool bTrans, out string sResult)
        {
            return base.ManageDownload(manageId, bTrans, out sResult);
            
        }

        //任务的完成  直接继承任务基类 流程固定
        public override bool ManageComplete(int manageId, bool bTrans, out string sResult)
        {
            return base.ManageComplete(manageId, bTrans, out sResult);
        }

        //分配库位  如果所有的入库任务类型都遵循该种分配规则 则可将入库规则代码写入该模块  

        //
        protected override void AllocateCell(int manageId)
        {
            Console.WriteLine("分配入库的库位");
        }

        //下达指令给wcs
        protected override bool InstructCommand(int manageId)
        {
            throw new NotImplementedException();
        }


        //更新库存
        protected override void HandleStock(int manageId)
        {
            //修改cell的库位状态为Full
            Cell cell = cellDao.Get(1);
            cell.CellStatus = "Full";
            cellDao.Update(cell); 
        }



    }
}
