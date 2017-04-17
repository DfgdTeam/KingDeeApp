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
        /// 获取医院所有的科室列表
        /// </summary>
        /// <param name="hospitalId">医疗机构代码</param>
        /// <param name="deptId">科室编码</param>
        /// <param name="deptType">科室类型   暂时没有专家、专科分类（1专科2 专家）</param>
        /// <returns></returns>
        public DataSet getHospitalDeptInfo(string hospitalId, string deptId, string deptType)
        {
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                return null;
            }
            DataSet ds = new DataSet();
            DataTable dt2 = new DataTable();
            string strSql = @"SELECT A.DEPT_CODE DEPTID,
                                   A.DEPT_NAME DEPTNAME,
                                   '' DEPTTYPE,
                                   (CASE
                                     WHEN A.PARENT_CODE IS NULL THEN
                                      '-1'
                                     ELSE
                                      A.PARENT_CODE
                                   END) PAERENTID,
                                   A.DEPT_DESC DESCRIPTION
                               FROM DEPT_DICT A";
            if (!string.IsNullOrEmpty(hospitalId))
            {
                //strSql += " AND A.USER_DEPT='" + deptId + "' ";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                strSql += " AND A.DEPT_CODE='" + deptId + "' ";
            }
            if (!string.IsNullOrEmpty(deptType))
            {
                //strSql += " AND A.USER_DEPT='" + deptId + "' ";
            }

            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch(Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败"));
                ds.Tables.Add(dt2);
            }
            return ds;
        }

        /// <summary>
        /// 获取医院指定条件的医生信息
        /// </summary>
        /// <param name="hospitalId">医疗机构代码</param>
        /// <param name="deptId">科室编码</param>
        /// <param name="doctorid">医生编号</param>
        /// <returns></returns>
        public DataSet getHospitalDoctorInfo(string hospitalId, string deptId, string doctorid)
        {
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                return null;
            }
            DataSet ds = new DataSet();
            DataTable dt2 = new DataTable();
            string strSql = @"SELECT A.USER_ID DOCTORID,
                                   A.USER_NAME DOCTORNAME,
                                   '' DOCTORLEVELCODE,
                                   B.TITLE DOCTORLEVEL,
                                   A.USER_DEPT DEPTID,
                                   C.DEPT_NAME DEPTNAME,
                                   B.DOCTOR_DESC DESCRIPTION
                              FROM USERS A
                              LEFT JOIN STAFF_DICT B
                                ON A.USER_ID = B.STAFF_ID
                              LEFT JOIN DEPT_DICT C
                                ON A.USER_DEPT = C.DEPT_CODE WHERE 1=1";
            if (!string.IsNullOrEmpty(hospitalId))
            {
                //strSql += " AND A.USER_DEPT='" + deptId + "' ";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                strSql += " AND A.USER_DEPT='" + deptId + "' ";
            }
            if (!string.IsNullOrEmpty(doctorid))
            {
                strSql += " AND A.USER_ID='" + doctorid + "' ";
            }
            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch(Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败"));
                ds.Tables.Add(dt2);
            }
            return ds;
        }

        /// <summary>
        /// 获取门诊患者基本信息
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="deptId"></param>
        /// <param name="doctorid"></param>
        /// <returns></returns>
        public DataSet getbaseOutpatientInfo(string idCardNo,string healthCardNo,string patientId,string patientName,
            string guardianName, string guardianIdCardNo, string phone)
        {
            DataSet ds = new DataSet();
            string strSql = @"SELECT A.USER_ID DOCTORID,
                                   A.USER_NAME DOCTORNAME,
                                   '' DOCTORLEVELCODE,
                                   B.TITLE DOCTORLEVEL,
                                   A.USER_DEPT DEPTID,
                                   C.DEPT_NAME DEPTNAME,
                                   B.DOCTOR_DESC DESCRIPTION
                              FROM USERS A
                              LEFT JOIN STAFF_DICT B
                                ON A.USER_ID = B.STAFF_ID
                              LEFT JOIN DEPT_DICT C
                                ON A.USER_DEPT = C.DEPT_CODE WHERE 1=1";
            //if (!string.IsNullOrEmpty(hospitalId))
            //{
            //    //strSql += " AND A.USER_DEPT='" + deptId + "' ";
            //}
            //if (!string.IsNullOrEmpty(deptId))
            //{
            //    strSql += " AND A.USER_DEPT='" + deptId + "' ";
            //}
            //if (!string.IsNullOrEmpty(doctorid))
            //{
            //    strSql += " AND A.USER_ID='" + doctorid + "' ";
            //}

            DataTable dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
            ds.Tables.Add(GetStatus("0", null));
            ds.Tables.Add(dt2.Copy());
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
            dt2.TableName = "handleStatus";
            DataRow dr = dt2.NewRow();

            dr[0] = resultCode;//0--成功
            dr[1] = resultDesc;//处理结果描述
            dt2.Rows.Add(dr);
            return dt2;
        }
        #endregion

    }
}