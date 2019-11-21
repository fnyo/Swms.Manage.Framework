using Swms.Manage.Framework.Dao;
using Swms.Manage.Framework.Model;
using Swms.Manage.Framework.Model.Enum;
using Swms.Manage.Framework.Plan;
using Swms.Manage.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swms.Manage.Framework.Manage
{


    /// <summary>
    /// 最底层的任务基类
    /// </summary>
    public abstract class ManageBase
    {
        //---设计模式之桥接模式  两重维度的变化  计划(单据)和任务   计划可能存在多种计划 任务存在多种任务 任务依赖计划 
        protected IBill _bill;

        protected FStockService _stockService;

        protected FStockService BStockService
        {
            get
            {
                if (_stockService == null)
                {
                    return _stockService;
                }
                return _stockService;
            }
        }

        public ManageBase(IBill bill)
        {
            this._bill = bill;
           
        }


        //任务主表 DataAccessObject
        protected MgeMainDao<MgeMain> mgeMainDao;
        //任务子表 DataAccessObject
        protected MgeListDao<MgeList> mgeListDao;
        //wcs,wms控制
        protected IoControlDao<IoControl> ioControlDao;

        protected CellDao<Cell> cellDao;

        public ManageBase()
        {
            mgeMainDao = new MgeMainDao<MgeMain>();
            mgeListDao = new MgeListDao<MgeList>();
        }
        //创建任务
        public virtual bool ManageCreate(MgeMain mgeMain,
                                            List<MgeList> mgeLists,
                                            bool bTrans,
                                            bool autoDownload,
                                            bool autoComplete,
                                            out string sResult)
        {



            bool bresult = true;
            sResult = string.Empty;
            try
            {
                //开始事务
                mgeMainDao._sessionFactory.BeginTransaction(bTrans);



                //1.数据检查
                bresult = this.BeforeCreateCheck(mgeMain.MgeId,out sResult);
                if (!bresult)
                {
                    return bresult;
                }


                /*可变化  任务创建  部分*/
                this.ManageCreatePart();



                //2.库位锁定
                //创建任务通用流程  上移至基类中
                //库位的锁定  判断起始位置和终止位置是否都不为空 不为空则锁定
                bresult = this.LockCell(out sResult);   //可以考虑放入 s_cellservice
                if (!bresult)
                {
                    return bresult;
                }


                //3.插入任务记录
                //插入任务记录
                bresult = this.CreateManageRecord(mgeMain, mgeLists, out sResult); //该方法可以考虑放入  s_manageservice 单一职责  以服务形式提供给该类
                if (!bresult)
                {
                    return bresult;
                }

                //4.更新单据
                //更新单据
                this.UpdateBillCreate(mgeMain.MgeId, "create");  //planService




                //5.判断是否自动下达，以及自动弯沉
                //自动下达
                if (autoDownload)
                {
                    bresult = this.ManageDownload(mgeMain.MgeId, bTrans, out sResult);

                    if (!bresult)
                    {
                        return bresult;
                    }
                }

                //自动完成
                if (autoComplete)
                {
                    bresult = this.ManageComplete(mgeMain.MgeId, bTrans, out sResult);

                    if (!bresult)
                    {
                        return bresult;
                    }
                }

                return bresult;
                        
            }
            catch(Exception ex)
            {
                bresult = false;
                return bresult;
            }
            finally
            {
                if (bresult)
                {
                    mgeMainDao._sessionFactory.CommitTransaction(bTrans);
                }
                else
                {
                    mgeMainDao._sessionFactory.RollBackTransaction(bTrans) ;
                }
            }
        }
        //任务下达  流程固定 但是对于添加某种特殊任务的流程环节 需要在子类中重写该方法。   
        public virtual bool ManageDownload(int manageId, bool bTrans, out string sResult)
        {
            bool bresult = true;
            sResult = string.Empty;
            try
            {
                //事务管理
                mgeMainDao._sessionFactory.BeginTransaction(bTrans);

                //1.分配库位   if(manageTypeCode=="空托盘入库"){ 分配空托盘区域库位 }
                this.AllocateCell(manageId);

                //2.更新任务状态
                this.UpdateManageStatus(manageId);

                //3.更新计划 执行中数量，计划状态，执行时间，执行人等
                this.UpdateBillCreate(manageId, "download");

                //4.下达指令给wcs
                this.InstructCommand(manageId);


            }

            catch (Exception ex)
            {
                bresult = false;
                return bresult;
            }
            finally
            {
                if (bresult) mgeMainDao._sessionFactory.CommitTransaction(bTrans);
                else mgeMainDao._sessionFactory.RollBackTransaction(bTrans);
            }
            return bresult;
        }
        //任务完成
        public virtual bool ManageComplete(int manageId, bool bTrans, out string sResult)
        {
            bool bresult = true;
            sResult = string.Empty;
            try
            {
                //事务管理
                mgeMainDao._sessionFactory.BeginTransaction(bTrans);

                //判断有无该任务
                if (HaveManage(manageId))
                {
                    return false;
                }
                //1.解锁库位
                this.UnLockCell();
                //2.处理库存
                this.HandleStock(manageId);
                //3.更新计划
                this.UpdateBillComplete(manageId, "");
                //4.更新状态
                this.UpdateManageStatus(manageId);
                //5.生成记录,并且删除对应任务
                this.CreateManageHis(manageId);

                //6.反馈给ERP
                this.FeedBack(manageId);
            }
            catch (Exception ex)
            {
                bresult = false;
                return bresult;
            }
            finally
            {
                if (bresult) mgeMainDao._sessionFactory.CommitTransaction(bTrans);
                else mgeMainDao._sessionFactory.RollBackTransaction(bTrans);
            }

            return bresult;
        }
        //任务取消
        public virtual bool ManageCancle(int manageId, bool bTrans, out string sResult)
        {
            

            bool bresult = true;
            sResult = string.Empty;
            try
            {
                //事务管理
                mgeMainDao._sessionFactory.BeginTransaction(bTrans);


                //1.判断任务是否可以取消  首先删除在wcs客户端删除wcs指令
                if (ValidateEnum.ExistIoContorl)
                {
                    bresult = false;
                    return bresult;
                }


                //2.更新起止位置库位状态为可运行  锁定-》解锁
                this.UnLockCell();



                //3.取消计划数量
                _bill.Cancle();


                //4.删除任务相关记录

                this.DeleteManageAbout(manageId);

            }
            catch (Exception ex)
            {
                bresult = false;
                return bresult;
            }
            finally
            {
                if (bresult) mgeMainDao._sessionFactory.CommitTransaction(bTrans);
                else mgeMainDao._sessionFactory.RollBackTransaction(bTrans);
            }

            return bresult;
        }
        //任务错误
        public virtual bool ManageError(int manageId, bool bTrans, out string sResult)
        {

            bool bresult = true;
            sResult = string.Empty;
            try
            {
                //事务管理
                mgeMainDao._sessionFactory.BeginTransaction(bTrans);

                //1.获取到wcs，wms中间表错误字段值
                IoControl ioContorl = ioControlDao.Get(manageId);

                //2.更新任务状态以及异常信息
                MgeMain mgeMain = mgeMainDao.Get(manageId);
                mgeMain.MgeStatus = Status.error.ToString();
                mgeMain.MgeErrorMessage = ioContorl.ErrorMessage;
                mgeMainDao.Update(mgeMain);

            }
            catch (Exception ex)
            {
                bresult = false;
                return bresult;
            }
            finally
            {
                if (bresult) mgeMainDao._sessionFactory.CommitTransaction(bTrans);
                else mgeMainDao._sessionFactory.RollBackTransaction(bTrans);
            }

            return bresult;
        }


        /// <summary>
        /// 流程固化  创建 下达  
        /// </summary>


            //下达指令给wcs  
            //必须重写  
            //考虑到可能不同的单据对应产生的入库或者出库任务,传给wcs的拓展参数不同 
            //例如配送计划  需要传递给wcs配送时间,以及配送紧急程度
        protected abstract bool InstructCommand(int manageId);
        //库位分配
        //必须重写
        //入库任务与出库任务 的分配库位规则不同  
        //拓展 如果之后存在某种任务类型对应不同的入库规则，例如空托盘要求分配到某个区域  可在空托盘入库子类中重写该方法
        //如果所有入库任务都遵循同一种入库规则 在ManageIn中重写该方法即可 
        protected abstract void AllocateCell(int manageId);
        //任务创建细节  拓展环节  
        //任务创建流程环节  
        /*
         * （1）.数据检查 （2）.库位锁定   （3）.插入任务记录  （4）.更新单据 （5）.判断是否自动下达，以及自动完成
         *  在（2）（3）之间加入拓展环节   设计模式：模板方法
         */
        //更新库存
        protected abstract void HandleStock(int manageId);


        protected virtual void ManageCreatePart()
        {

        }
        //更新单据  任务创建时更新计划的相关信息  例如创建入库任务时,更新计划中的已经组盘数量以及计划状态。
        protected virtual void UpdateBillCreate(int manageId, string updateType)
        {
            //空方法体  默认为无计划
        }
        //更新单据  任务完成时更新计划完成数量以及计划状态
        protected virtual void UpdateBillComplete(int manageId, string updateType)
        {
            //空方法体  默认为无计划
        }
        //更新单据  任务执行时更新计划执行中数量以及计划状态等
        protected virtual void UpdateBillExecute(int manageId, string updateType)
        {
            //空方法体  默认为无计划
        }
        //数据校验  创建之前的校验
        protected virtual bool BeforeCreateCheck(int manageId, out string sResult)
        {
            sResult = string.Empty;

            //check one. 判断根据manageId获取的任务是否为空
            if (ValidateEnum.EmptyManage)
            {
                return false;
            }

            //check two. 判断是否存在任务
            if (ValidateEnum.ExistManage)
            {
                return false;
            }
            return true;

        }



        //反馈给ERP
        protected virtual void FeedBack(int manageId) { }

       


        #region 代码重构 搬移函数  -搬移至对应服务类 CreateManageRecord 搬移至  FManageService LockCell,UnLockCell 搬移至FCellService   UpdateManageStatus,HaveManage,CreateManageHis 搬移至FManageService

        //-- 保持类的职能单一 ，可维护性更高 ，且易读性强   增加代码重用

        //提练函数  创建任务记录
        protected bool CreateManageRecord(MgeMain mgeMain, List<MgeList> mgeLists, out string sResult)
        {
            sResult = string.Empty;
            int res1 = 0;
            int res2 = 0;
            res1=mgeMainDao.Insert(mgeMain);
            foreach (var mgeList in mgeLists)
            {
                mgeList.MgeId = mgeMain.MgeId;
                res2=mgeListDao.Insert(mgeList);
            }
            return res1 > 0 && res2 > 0;
        }
        //锁定库位
        protected bool LockCell(out string sResult)
        {
          
            sResult = string.Empty;
            return true;



        }
        //解锁库位
        protected bool UnLockCell() { return true; }
        //更新任务状态
        protected void UpdateManageStatus(int manageId) { }
        //判断是否存在任务
        protected bool HaveManage(int manageId)
        {
            return true;
        }
        //生成记录,并且删除任务表任务
        protected void CreateManageHis(int manageId) { }

        //删除任务相关记录 IO_CONTORL MANAGE_MAIN MANAGE_LIST MANAGE_DETAIL
        protected void DeleteManageAbout(int manageId)
        {

        }
        #endregion
    }
}
