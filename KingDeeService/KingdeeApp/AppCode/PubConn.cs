using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using Devart.Data.Oracle;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Data.SqlClient;
using System.IO;


/*=================================
 * 
 * 用于数据库连接
 * 
 =================================*/

/// <summary>
///PubConn 的摘要说明
/// </summary>
public class PubConn
{
    public PubConn()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    #region ORACLE 连接方式

    /// <summary>
    /// 执行查询语句，返回DataSet
    /// </summary>
    /// <param name="SQLString">sql语句</param>
    /// <param name="strConn">数据库链接</param>
    /// <returns>数据集</returns>
    public static DataSet Query(string SQLString, string strConn)
    {
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[strConn];
        using (OracleConnection connection = new OracleConnection(connectionString.ToString()))
        {
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                OracleDataAdapter command = new OracleDataAdapter(SQLString, connection);
                command.Fill(ds, "ds");
            }
            catch (OracleException ex)
            {
                throw new Exception(ex.Message);
            }
            connection.Close();
            connection.Dispose();
            return ds;
        }
    }


    /// <summary>
    /// 执行SQL语句，返回影响的记录数
    /// </summary>
    /// <param name="SQLString">sql语句</param>
    /// <param name="strConn">数据库链接</param>
    /// <returns>影响的记录数</returns>
    public static int ExecuteSql(string SQLString, string strConn)
    {
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[strConn];
        using (OracleConnection connection = new OracleConnection(connectionString.ToString()))
        {
            using (OracleCommand cmd = new OracleCommand(SQLString, connection))
            {
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return rows;
                }
                catch (OracleException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }


    /// <summary>
    /// 执行一条计算查询结果语句，返回查询结果（整数）。
    /// </summary>
    /// <param name="SQLString">sql语句</param>
    /// <param name="strConn">数据库链接</param>
    /// <returns>查询结果</returns>
    public static int GetCount(string strSQL, string strConn)
    {
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[strConn];
        using (OracleConnection connection = new OracleConnection(connectionString.ToString()))
        {
            OracleCommand cmd = new OracleCommand(strSQL, connection);
            try
            {
                connection.Open();
                OracleDataReader result = cmd.ExecuteReader();
                int i = 0;
                while (result.Read())
                {
                    i = result.GetInt32(0);
                }
                result.Close();
                connection.Close();
                connection.Dispose();
                return i;
            }
            catch (OracleException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
    }

    /// <summary>
    /// 执行一条计算查询结果语句，返回查询结果（object）。
    /// </summary>
    /// <param name="SQLString">sql语句</param>
    /// <param name="strConn">数据库链接</param>
    /// <returns>查询结果（object）</returns>
    public static object GetSingle(string SQLString, string strConn)
    {
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[strConn];
        using (OracleConnection connection = new OracleConnection(connectionString.ToString()))
        {
            OracleCommand cmd = new OracleCommand(SQLString, connection);
            try
            {
                connection.Open();
                object obj = cmd.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    connection.Close();
                    connection.Dispose();
                    return null;
                }
                else
                {
                    connection.Close();
                    connection.Dispose();
                    return obj;
                }
            }
            catch (OracleException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
    }

    /// <summary>
    /// 记录出错信息到数据库表，方便维护人员快速定位出错点,包含代码行号和备注
    /// </summary>
    /// <param name="strExceptionDBType">异常产生时数据库类型</param>
    /// <param name="strSql">引起异常的sql语句</param>
    /// <param name="ex">捕获到的异常对象</param>
    /// <param name="strNote">备注</param>
    public static void ExceptionLoger(string strExceptionDBType, string strSql, Exception ex, string strNote)
    {
        try
        {
            //写本地日志
            string strServerPath = HttpContext.Current.Server.MapPath("~/");

            //每天生成一个日志
            string fileName = strServerPath + @"\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }

            if (ex != null)
                File.AppendAllText(fileName,
                    "【记录时间】" + DateTime.Now.ToString() + "\r\n"
                  + "【 DbType 】" + strExceptionDBType + "\r\n"
                  + "【异常信息】" + ex.Message + "\r\n"
                  + "【堆栈信息】" + ex.StackTrace + "\r\n"
                  + "【SQL语句 】" + strSql + "\r\n"
                  + "【备注信息】" + strNote);
            else
                File.AppendAllText(fileName,
                    "【记录时间】" + DateTime.Now.ToString() + "\r\n"
                  + "【 DbType 】" + strExceptionDBType + "\r\n"
                  + "【异常信息】" + "" + "\r\n"
                  + "【堆栈信息】" + "" + "\r\n"
                  + "【SQL语句 】" + strSql + "\r\n"
                  + "【备注信息】" + strNote);
            File.AppendAllText(fileName,
                "\r\n====================================================================\r\n");
        }
        catch
        {

        }

        #region 取消写入数据库日志 避免出现SQL注入的风险

        //try
        //{
        //    if (!string.IsNullOrEmpty(strSql))
        //    {
        //        strSql = strSql.Replace("'", "''");
        //    }
        //    string cmdtxt = string.Empty;
        //    int MaxLogNo = 1;
        //    cmdtxt = "SELECT MAX(LOG_SN) FROM RH.DATA_ERROR_LOG ";
        //    object objNo = GetSingle(cmdtxt, "HISConn");
        //    if (objNo != null && objNo.ToString().Trim().Length > 0)
        //    {
        //        MaxLogNo = Convert.ToInt32(objNo.ToString()) + 1;
        //    }


        //    if (ex != null && ex.StackTrace != null)
        //    {
        //        string[] st = ex.StackTrace.Split('\r');

        //        cmdtxt = "INSERT INTO RH.DATA_ERROR_LOG (LOG_SN,EVENT_ID,LOG_DATETIME,EXCEPTION_DBTYPE,EXCEPTION_STACKTRACE,CODE_ROW_NUM,EXCEPTION_SQLSTR,EXCEPTION_CONTENT,NOTE) VALUES("
        //            + "'" + MaxLogNo + "','" + Guid.NewGuid().ToString("N").ToUpper() + "',TO_DATE('" + DateTime.Now.ToString() + "','YYYY-MM-DD HH24:MI:SS'),'"
        //            + strExceptionDBType + "','" + st[st.Length - 1] + "','" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(':') + 1) + "','"
        //            + strSql + "','" + ex.Message + "','" + (string.IsNullOrEmpty(strNote) ? "" : strNote) + "')";
        //    }
        //    else
        //    {
        //        cmdtxt = @"INSERT INTO RH.DATA_ERROR_LOG (LOG_SN,EVENT_ID,LOG_DATETIME,EXCEPTION_DBTYPE,EXCEPTION_STACKTRACE,CODE_ROW_NUM,EXCEPTION_SQLSTR,EXCEPTION_CONTENT,NOTE) VALUES("
        //                + "'" + MaxLogNo + "','" + Guid.NewGuid().ToString("N").ToUpper() + "',TO_DATE('" + DateTime.Now.ToString() + "','YYYY-MM-DD HH24:MI:SS'),'"
        //                + strExceptionDBType + "','','','" + strSql + "',NULL,'" + (string.IsNullOrEmpty(strNote) ? "" : strNote) + "')";
        //    }

        //    ExecuteSql(cmdtxt, "HISConn");

        //}
        //catch
        //{

        //}

        #endregion
    }

    #region 内部方法不会使用到

    ///// <summary>
    ///// 记录与外部系统进行数据交换时发生的总体交互情况日志
    ///// 本方法用于在交互发生之前进行记录，与SynchroLogerEnd方法成对使用。
    ///// 通过两方法结合，实现对交互总耗时的记录。
    ///// </summary>
    ///// <param name="obj"></param>
    //public void SynchroLogerStart(object[] obj)
    //{
    //    string strSQL = string.Empty;
    //    try
    //    {
    //        string cmdtxt = string.Empty;
    //        int MaxLogNo = 1;
    //        cmdtxt = "SELECT MAX(LOG_SN) FROM other_sys_synchro_log ";
    //        object objNo = GetSingle(cmdtxt, "DataCenterConn");
    //        if (objNo != null && objNo.ToString().Trim().Length > 0)
    //        {
    //            MaxLogNo = Convert.ToInt32(objNo.ToString()) + 1;
    //        }

    //        strSQL = "INSERT INTO other_sys_synchro_log(LOG_SN,LOG_EVENT_ID,SYNCHRO_START_DATETIME,CLASS_NAME,METHOD_NAME,CAPTION) values(";

    //        strSQL += "'" + MaxLogNo + "',";
    //        strSQL += "'" + obj[0].ToString() + "',";
    //        strSQL += " sysdate, ";
    //        strSQL += " '" + obj[1].ToString() + "', ";
    //        strSQL += " '" + obj[2].ToString() + "', ";
    //        strSQL += " '" + obj[3].ToString() + "')";
    //        ExecuteSql(strSQL, "DataCenterConn");
    //    }
    //    catch (Exception ex)
    //    {
    //        this.ExceptionLoger("DataCenterConn", strSQL, ex, "与外部系统交互开始日志");
    //    }
    //}

    ///// <summary>
    ///// 记录与外部系统进行数据交换时发生的总体交互情况日志
    ///// 本方法用于在交互发生之后进行记录，与SynchroLogerStart方法成对使用。
    ///// 通过两方法结合，实现对交互总耗时的记录。
    ///// </summary>
    ///// <param name="obj"></param>
    //public void SynchroLogerEnd(object[] obj)
    //{
    //    string strSQL = string.Empty;
    //    try
    //    {
    //        string cmdtxt = string.Empty;
    //        cmdtxt = "SELECT COUNT(0) FROM other_sys_synchro_log WHERE ";
    //        cmdtxt += " LOG_EVENT_ID = '" + obj[0].ToString() + "'   ";
    //        int nCount = this.GetCount(cmdtxt, "DataCenterConn");
    //        if (nCount <= 0)
    //        {
    //            Exception ex = new Exception("记录与其他系统结束日志时，未发现匹配的日志开始记录");
    //            this.ExceptionLoger("DataCenterConn", strSQL, ex, "与外部系统交互结束日志");
    //        }
    //        else
    //        {
    //            strSQL = "UPDATE other_sys_synchro_log SET ";
    //            strSQL += "SYNCHRO_END_DATETIME =sysdate,";
    //            strSQL += "SUCCESS_COUNT ='" + obj[1].ToString() + "',";
    //            strSQL += "ERROR_COUNT ='" + obj[2].ToString() + "',";
    //            strSQL += "ALL_COUNT ='" + obj[3].ToString() + "' ";
    //            strSQL += " WHERE ";
    //            strSQL += " LOG_EVENT_ID = '" + obj[0].ToString() + "'   ";
    //        }

    //        ExecuteSql(strSQL, "DataCenterConn");
    //    }
    //    catch (Exception ex)
    //    {
    //        this.ExceptionLoger("DataCenterConn", strSQL, ex, "与外部系统交互结束日志");
    //    }

    //    System.GC.Collect();

    //}

    ///// <summary>
    ///// 记录与外部系统进行数据交换时发生的异常日志
    ///// </summary>
    ///// <param name="strExceptionDBType">异常产生时数据库类型</param>
    ///// <param name="strSql">引起异常的sql语句</param>
    ///// <param name="ex">捕获到的异常对象</param>
    ///// <param name="strNote">备注</param>
    //public bool ExceptionSynchroLoger(string strExceptionDBType, string strLogEventID,string strSql, Exception ex, string strNote)
    //{

    //    try
    //    {
    //        if (ex != null)
    //            System.IO.File.AppendAllText(@"E:\数据平台\WebService\eHI.txt", strExceptionDBType + ":" + ex.ToString() + ":【" + strSql + "】" + strNote + "\r\n");
    //        else
    //            System.IO.File.AppendAllText(@"E:\数据平台\WebService\eHI.txt", strExceptionDBType + ":【" + strSql + "】" + strNote + "\r\n");
    //    }
    //    catch (Exception ex1)
    //    {
    //    }


    //    bool flag = false;
    //    string cmdtxt = string.Empty;
    //    try
    //    {
    //        if (!string.IsNullOrEmpty(strSql))
    //        {
    //            strSql = strSql.Replace("'", "''");
    //        }            
    //        //string strSQL = "INSERT INTO DATA_SYNCHRO_EXCEPTION_LOG(LOG_SN,LOG_EVENT_ID,LOG_DATETIME,";
    //        //strSQL +="EXCEPTION_DBTYPE,EXCEPTION_STACKTRACE,CODE_ROW_NUM,EXCEPTION_SQLSTR,EXCEPTION_CONTENT,NOTE) values(";
    //        if (ex != null && ex.StackTrace != null)
    //        {
    //            string[] st = ex.StackTrace.Split('\r');

    //            cmdtxt = "insert into other_sys_synchro_ex_log (LOG_SN,LOG_EVENT_ID,LOG_DATETIME,EXCEPTION_DBTYPE,EXCEPTION_STACKTRACE,CODE_ROW_NUM,EXCEPTION_SQLSTR,EXCEPTION_CONTENT,NOTE) VALUES("
    //                + "'" + Guid.NewGuid().ToString("N").ToUpper() + "','" + strLogEventID + "',TO_DATE('" + DateTime.Now.ToString() + "','YYYY-MM-DD HH24:MI:SS'),'"
    //                + strExceptionDBType + "','" + st[st.Length - 1] + "','" + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(':') + 1) + "','"
    //                + strSql + "','" + ex.Message + "','" + (string.IsNullOrEmpty(strNote) ? "" : strNote) + "')";
    //        }
    //        else if (ex != null)
    //        {
    //            cmdtxt = "insert into other_sys_synchro_ex_log (LOG_SN,LOG_EVENT_ID,LOG_DATETIME,EXCEPTION_DBTYPE,EXCEPTION_STACKTRACE,CODE_ROW_NUM,EXCEPTION_SQLSTR,EXCEPTION_CONTENT,NOTE) VALUES("
    //                    + "'" + Guid.NewGuid().ToString("N").ToUpper() + "','" + strLogEventID + "',TO_DATE('" + DateTime.Now.ToString() + "','YYYY-MM-DD HH24:MI:SS'),'"
    //                    + strExceptionDBType + "','','','" + strSql + "','" + ex.Message + "','" + (string.IsNullOrEmpty(strNote) ? "" : strNote) + "')";
    //        }

    //        ExecuteSql(cmdtxt, "DataCenterConn");

    //    }
    //    catch
    //    {
    //        this.ExceptionLoger("DataCenterConn", cmdtxt, ex, "与外部系统进行数据交换时发生的异常日志");
    //    }
    //    return flag;

    //}

    #endregion

    /// <summary>
    /// 获取服务器时间
    /// </summary>
    /// <returns></returns>
    public static DateTime SeverNow()
    {
        DateTime nettime = DateTime.Now;

        //oracle
        DataSet pDataSet = new DataSet();
        string strSQL = "select sysdate as  cursysdate FROM dual";
        pDataSet = Query(strSQL, "DataCenterConn");
        if (pDataSet.Tables.Count > 0)
        {
            DataTable objTable;
            objTable = pDataSet.Tables[0];
            DataRow drCurrent;
            drCurrent = objTable.Rows[0];
            nettime = Convert.ToDateTime(drCurrent["cursysdate"]);
        }
        return nettime;
    }

    /// <summary>
    /// 记录日志到数据库
    /// </summary>
    /// <param name="obj"></param>
    public static void SucessLoger(object[] obj)
    {
        try
        {
            string strSQL = "INSERT INTO DATA_SYNCHRO_LOG(SYNCHRO_DATE,METHOD,CAPTION,SUCCEXXFUL,FAIL) values(";
            strSQL += " sysdate, ";
            strSQL += " '" + obj[1].ToString() + "', ";
            strSQL += " '" + obj[2].ToString() + "', ";
            strSQL += " '" + obj[3].ToString() + "', ";
            strSQL += " '" + obj[4].ToString() + "')";
            ExecuteSql(strSQL, "DataCenterConn");
        }
        catch (Exception ex)
        {
            ex.ToString();
        }


    }

    #endregion


    #region SQL数据库连接

    public DataSet SQLQuery(string SQLString, string strConn)
    {
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[strConn];
        using (SqlConnection connection = new SqlConnection(connectionString.ToString()))
        {
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                command.Fill(ds, "ds");
            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }
            connection.Close();
            connection.Dispose();
            return ds;
        }
    }



    /// <summary>
    /// 执行SQL语句，返回影响的记录数
    /// </summary>
    /// <param name="SQLString">SQL语句</param>
    /// <returns>影响的记录数</returns>
    public int SQLExecuteSql(string SQLString, string strConn)
    {
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[strConn];
        using (SqlConnection connection = new SqlConnection(connectionString.ToString()))
        {
            using (SqlCommand cmd = new SqlCommand(SQLString, connection))
            {
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    connection.Close();
                    connection.Dispose();
                    return rows;
                }
                catch (SqlException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                    connection.Dispose();
                }
            }
        }
    }


    /// <summary>
    /// 执行一条计算查询结果语句，返回查询结果（整数）。
    /// </summary>
    /// <param name="strSQL">计算查询语句</param>
    /// <returns>查询结果</returns>
    public int SQLGetCount(string strSQL, string strConn)
    {
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[strConn];
        using (SqlConnection connection = new SqlConnection(connectionString.ToString()))
        {
            SqlCommand cmd = new SqlCommand(strSQL, connection);
            try
            {
                connection.Open();
                SqlDataReader result = cmd.ExecuteReader();
                int i = 0;
                while (result.Read())
                {
                    i = result.GetInt32(0);
                }
                result.Close();
                connection.Close();
                connection.Dispose();
                return i;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
    }


    /// <summary>
    /// 执行一条计算查询结果语句，返回查询结果（object）。
    /// </summary>
    /// <param name="SQLString">计算查询结果语句</param>
    /// <returns>查询结果（object）</returns>
    public object sqlGetSingle(string SQLString, string strConn)
    {
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[strConn];
        using (SqlConnection connection = new SqlConnection(connectionString.ToString()))
        {
            SqlCommand cmd = new SqlCommand(SQLString, connection);
            try
            {
                connection.Open();
                object obj = cmd.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    connection.Close();
                    connection.Dispose();
                    return null;
                }
                else
                {
                    connection.Close();
                    connection.Dispose();
                    return obj;
                }
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                cmd.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
    }

    #endregion


    /// <summary>
    /// 在记事本文件中写入异常信息，日志等级默认INFO
    /// </summary>
    /// <param name="ex">异常信息</param>
    public static void writeFileLog(string messge)
    {
        string strDateTimtNow = DateTime.Now.ToString("yyyy-MM-dd");
        if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\log"))
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\log");
        }

        string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\log\" + strDateTimtNow + ".txt";
        if (!File.Exists(filePath))
        {
            try
            {
                File.Create(filePath).Close();
            }
            catch
            {

            }
        }

        File.AppendAllText(filePath,
            "===============================================================================" + 
            Environment.NewLine + messge + Environment.NewLine + DateTime.Now.ToString());

    }

}