using Swms.Manage.Framework.Manage.MIn;
using Swms.Manage.Framework.Manage.MOut;
using Swms.Manage.Framework.Model;
using Swms.Manage.Framework.Plan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Client
{

    //客户端  调用利用反射取代条件表达式或者格式工厂
    public class CilentProxy
    {


        //启动调用
        public static void StartUp()
        {
            //根据传入参数调用相应类方法 无论哪个界面的任务


            MgeMain mgeMain = new MgeMain();
            List<MgeList> mgeList = new List<MgeList>();

            //EmptyStockIn 可以与窗体名匹配 windowName
            Object ret=Invoke("EmptyStockIn", "ManageCreate", new object[] { 1, 2, 3, 4 });


            HpFullStockOut hpFullStockIn = new HpFullStockOut(new OutBil());




           // EmptyStockIn stockIn = new EmptyStockIn(new InBill());



        }




        //反射方法
        public static Object Invoke(string className, string method, object[] objs)
        {
            return true;
        }
    }



    
}
