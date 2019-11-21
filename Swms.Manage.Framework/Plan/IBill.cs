using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Plan
{


    /// <summary>
    /// 计划接口
    /// </summary>
    public interface IBill
    {
        void Create();

        void Finish();

        void Cancle();

        void Begin();
    }
}
