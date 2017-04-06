using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace KingdeeApp
{
    /// <summary>
    /// 基本信息类
    /// </summary>
    public class BasicInfo
    {
        //his库链接
        string strHISConn = "HisConn";

        /// <summary>
        /// 通过本接口获取医院所有的科室列表
        /// </summary>
        /// <returns></returns>
        public DataSet getDeptInfo(string hospitalId, string deptId, string deptType)
        {
            DataSet ds = new DataSet();
            string strSql = @"SELECT A.DEPT_CODE DEPTID ,A.DEPT_NAME DEPTNAMED FROM DEPT_DICT A";
            DataTable dt2 = PubConn.Query(strSql,strHISConn).Tables[0];
            ds.Tables.Add(GetStatus("0", null));
            ds.Tables.Add(dt2);
            return ds;
        }



        #region 私有方法
        /// <summary>
        /// 返回结果状态
        /// </summary>
        /// <param name="resultCode">处理结果代码 0--成功</param>
        /// <param name="resultDesc">处理结果描述</param>
        /// <returns></returns>
        private DataTable GetStatus(string resultCode, string resultDesc)
        {
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("resultCode");
            dt2.Columns.Add("resultDesc");
            DataRow dr = dt2.NewRow();

            dr[0] = resultCode;//0--成功
            dr[1] = resultDesc;//处理结果描述
            dt2.Rows.Add(dr);
            return dt2;
        } 
        #endregion

    }
}