using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Dao
{
    public class SessionFactory
    {
        public void BeginTransaction()
        {

        }

        public void BeginTransaction(bool bTrans)
        {
            if (bTrans)
            {
                this.BeginTransaction();
            }
        }


        public void RollBackTransaction()
        {

        }

        public void RollBackTransaction(bool  bTrans)
        {
            if (bTrans)
            {
                this.RollBackTransaction();
            }
        }


        public void CommitTransaction()
        {

        }

        public void CommitTransaction(bool bTrans)
        {
            if (bTrans)
            {
                this.CommitTransaction();
            }
        }
    }


}
