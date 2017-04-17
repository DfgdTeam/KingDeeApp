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
        Message mess = new Message();
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
        [WebMethod(Description = "检查状态提醒")]
        public DataSet pacsStatusChanged()
        {
            return mess.pacsStatusChanged();
        }
        [WebMethod(Description = "爽约通知")]
        public DataSet registerMissed()
        {
            return mess.registerMissed();
        }

        [WebMethod(Description = "获取诊间支付二维码接口")]

        public DataSet outpatientPayScan(string doctorId, string doctorName, string deptId, string deptName, string clinicSeq, string clinicTime, string patientId, string patientName, string healthCardNo, string phone, string hospitalId, string settleCode, string settleType)
        {
            return mess.outpatientPayScan(doctorId, doctorName, deptId, deptName, clinicSeq, clinicTime, patientId, patientName, healthCardNo, phone, hospitalId, settleCode, settleType);
        }

        //[WebMethod(Description = "诊间预约通知")]
        //public DataSet clinicBooking() { 

        //}
        [WebMethod(Description = "自定义消息")]
        public DataSet customMessage()
        {
            return mess.customMessage();
        }
        [WebMethod(Description = "门诊医生支付通知")]

        public DataSet outpatientPay()
        {
            return mess.outpatientPay();
        }
        //[WebMethod(Description = "医生停诊通知")]
        //public DataSet scheduleCancel() { 

        //}
        #endregion


        #region 住院
        [WebMethod(Description = "住院预交金缴纳")]
        public DataSet inpatient_doPrepay(string orderId, string hospitalId, string idCardNo, string healthCardNo, string patientId, string inpatientId, string orderTime, string tradeNo, string operatorId, string machineId, string payAmout, string payMode)
        {
            return mess.inpatient_doPrepay(orderId, hospitalId, idCardNo, healthCardNo, patientId, inpatientId, orderTime, tradeNo, operatorId, machineId, payAmout, payMode);
        }
        [WebMethod(Description = "预交金查询")]
        public DataSet inpatient_getPrepayRecord(string inpatientId)
        {
            return mess.inpatient_getPrepayRecord(inpatientId);
        }
        [WebMethod(Description = "住院费用每日清单查询")]
        public DataSet inpatient_getDailyBill(string inpatientId, string billDate)
        {
            return mess.inpatient_getDailyBill(inpatientId, billDate);
        }
        [WebMethod(Description = "住院费用汇总查询")]
        public DataSet inpatient_getTotalCost(string inpatientId)
        {
            return mess.inpatient_getTotalCost(inpatientId);
        }
        [WebMethod(Description = "住院费用分类明细查询")]
        public DataSet inpatient_getDetailCost(string inpatientId, string typeCode, string billDate, string startMxseq, string endMxseq)
        {
            return mess.inpatient_getDetailCost(inpatientId, typeCode, billDate, startMxseq, endMxseq);
        }
        [WebMethod(Description = "患者住院业务功能检测")]
        public DataSet inpatient_operationCheck(string inpatienId, string operType)
        {
            return mess.inpatient_operationCheck(inpatienId, operType);
        }

        #endregion

        #region 检验
        [WebMethod(Description = "检验报告列表查询")]
        public DataSet lis_getReport(string healthCardNo, string patientId, string clinicSeq, string beginDate, string endDate)
        {
            return mess.lis_getReport(healthCardNo, patientId, clinicSeq, beginDate, endDate);
        }
        [WebMethod(Description = "检验报告明细内容查询")]
        public DataSet lis_getReportItem(string inspectionId)
        {
            return mess.lis_getReportItem(inspectionId);
        }
        #endregion

        #region 检查
        [WebMethod(Description = " 检查报告列表查询接口")]
        public DataSet pacs_getReport(string healthCardNo, string patientId, string inpatientId, string clinicSeq, string beginDate, string endDate)
        {
            return mess.pacs_getReport(healthCardNo, patientId, inpatientId, clinicSeq, beginDate, endDate);
        }
        [WebMethod(Description = "检查报告明细内容查询接口")]
        public DataSet pacs_getReportDetail(string reportId)
        {
            return mess.pacs_getReportDetail(reportId);
        }
        #endregion


        [WebMethod(Description = "挂号记录查询")]
        public DataSet support_getRegisterInfo(string healthCardNo, string clinicSeq, string patientId, string orderId, string orderDate, string visitDate, string orderStatus)
        {
            return mess.support_getRegisterInfo(healthCardNo, clinicSeq, patientId, orderId, orderDate, visitDate, orderStatus);
        }
        #region 辅助功能
        [WebMethod(Description = "获取指引单")]
        public DataSet support_getGuideList(string clinicSeq, string receiptId)
        {
            return support_getGuideList(clinicSeq, receiptId);
        }
        [WebMethod(Description = "卡信息查询")]
        public DataSet user_getCardInfo(string idCardNo, string patientName, string phone)
        {
            return mess.user_getCardInfo(idCardNo, patientName, phone);
        }
        //[WebMethod(Description = " 获取用户可用服务对象列表")]
        //public DataSet support_getSvObject(string hospitalId, string healthCardNo)
        //{

        //}
        //[WebMethod(Description = "停诊医生信息查询")]
        //public DataSet support_getStopDoctorInfo(string hospitalId, string startDate, string endDate )
        //{

        //}
        [WebMethod(Description = "分页查询交易信息")]
        public DataSet support_pageQueryOrder(string orderId, string tradeDate, string productType, string payMode, string pageSize, string pageNo)
        { 
        return mess.support_pageQueryOrder(orderId,tradeDate,productType,payMode,pageSize,pageNo)
        }
        #endregion
    }
}