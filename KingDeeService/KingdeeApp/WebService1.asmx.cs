using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace KingdeeApp
{
    /// <summary>
    /// WebService1 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        BasicInfo basic = new BasicInfo();
        [WebMethod]
        public string HelloWorld()
        {
            //PubConn.writeFileLog("hahh");
            return "Hello World";
        }
        #region 基础信息

        [WebMethod(Description = "获取医院科室信息")]
        public DataSet getHospitalDeptInfo(string hospitalId, string deptId, string deptType)
        {
            return basic.getHospitalDeptInfo(hospitalId, deptId, deptType);
        }

        [WebMethod(Description = "获取医院医生信息")]
        public DataSet getHospitalDoctorInfo(string hospitalId, string deptId, string doctorid)
        {
            return basic.getHospitalDoctorInfo(hospitalId, deptId, doctorid);
        } 

        #endregion

        #region 消息
        
        #endregion
    }
}
