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
        [WebMethod(Description = "获取医院所有的科室列表")]
        public DataSet getDeptInfo(string hospitalId, string deptId, string deptType)
        {
            return basic.getDeptInfo(hospitalId, deptId, deptType);
        }
    }
}
