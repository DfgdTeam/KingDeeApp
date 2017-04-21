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
        public DataSet hospital_getDeptInfo(string hospitalId, string deptId, string deptType)
        {
            return basic.hospital_getDeptInfo(hospitalId, deptId, deptType);
        }

        [WebMethod(Description = "获取医院医生信息")]
        public DataSet hospital_getDoctorInfo(string hospitalId, string deptId, string doctorid)
        {
            return basic.hospital_getDoctorInfo(hospitalId, deptId, doctorid);
        }

        [WebMethod(Description = "获取门诊患者基本信息")]
        public DataSet baseinfo_GetOutpatientInfo(string idCardNo, string healthCardNo, string patientId, string patientName,
            string guardianName, string guardianIdCardNo, string phone)
        {
            return basic.baseinfo_GetOutpatientInfo(idCardNo, healthCardNo, patientId, patientName, guardianName, guardianIdCardNo, phone);
        }

        [WebMethod(Description = "获取住院患者信息")]
        public DataSet baseinfo_GetInpatientInfo(string idCardNo, string healthCardNo, string patientId, string inpatientId)
        {
            return basic.baseinfo_GetInpatientInfo(idCardNo, healthCardNo, patientId, inpatientId);
        }

        [WebMethod(Description = "获取门诊患者就诊信息")]
        public DataSet baseinfo_GetOutpatientVisitPatient(string hospitalId, string healthCardNo, string patientId, string doctorId,
            string startDate, string endDate, string diseaseLabel, string ICD)
        {
            return basic.baseinfo_GetOutpatientVisitPatient(hospitalId, healthCardNo, patientId, doctorId, startDate, endDate, diseaseLabel, ICD);
        }

        [WebMethod(Description = "获取门诊科室列表")]
        public DataSet appointment_GetDeptInfo(string hospitalId, string deptId, string deptType)
        {
            return basic.appointment_GetDeptInfo(hospitalId, deptId, deptType);
        }


        [WebMethod(Description = "获取医生出诊信息")]
        public DataSet appointment_GetScheduleInfo(string hospitalId, string deptId, string deptType, string doctorId,
            string searchCode, string startDate, string endDate)
        {
            return basic.appointment_GetScheduleInfo(hospitalId, deptId, deptType, doctorId, searchCode, startDate, endDate);
        }

        [WebMethod(Description = "获取医生号源分时信息")]
        public DataSet appointment_GetTimeInfo(string hospitalId, string scheduleId, string deptId, string clinicUnitId,
            string doctorId, string regDate, string shiftCode)
        {
            return basic.appointment_GetTimeInfo(hospitalId, scheduleId, deptId, clinicUnitId, doctorId, regDate, shiftCode);
        }

        [WebMethod(Description = "预约")]
        public DataSet appointment_AddOrder(string orderId, string hospitalId, string deptId, string clinicUnitId, string doctorId,
            string doctorLevelCode, string regDate, string scheduleId, string periodId, string shiftCode, string startTime, string endTime,
            string healthCardNo, string patientId, string patientName, string idCardNo, string phone, string orderType, string orderTime,
            string svObjectId, string fee, string treatfee, string remark)
        {
            return basic.appointment_AddOrder(orderId, hospitalId, deptId, clinicUnitId, doctorId, doctorLevelCode, regDate, scheduleId,
                periodId, shiftCode, startTime, endTime, healthCardNo, patientId, patientName, idCardNo, phone, orderType, orderTime, svObjectId,
                fee, treatfee, remark);
        }

        [WebMethod(Description = "预约支付")]
        public DataSet appointment_Pay(string hospitalId, string orderId, string tradeNo, string healthCardNo, string patientId,
            string bookingNo, string svObjectId, string medicareSettleLogId, string operatorId, string machineId, string payAmout,
            string recPayAmout, string totalPayAmout, string payMode, string payTime)
        {
            return basic.appointment_Pay(hospitalId, orderId, tradeNo, healthCardNo, patientId, bookingNo, svObjectId,
                medicareSettleLogId, operatorId, machineId, payAmout, recPayAmout, totalPayAmout, payMode, payTime);
        }

        [WebMethod(Description = "取消预约")]
        public DataSet appointment_CancelOrder(string orderId, string healthCardNo, string patientId, string scheduleId,
           string periodId, string bookingNo, string cancelTime, string cancelReason)
        {
            return basic.appointment_CancelOrder(orderId, healthCardNo, patientId, scheduleId, periodId, bookingNo, cancelTime, cancelReason);
        }

        [WebMethod(Description = "挂号")]
        public DataSet register_Pay(string lockId, string infoSeq, string orderId, string hospitalId, string healthCardNo, string patientId,
           string orderType, string orderTime, string svObjectId, string medicareSettleLogId, string operatorId, string machineId,
           string payAmout, string recPayAmout, string totalPayAmout, string payMode, string tradeNo)
        {
            return basic.register_Pay(lockId, infoSeq, orderId, hospitalId, healthCardNo, patientId, orderType, orderTime, svObjectId,
                medicareSettleLogId, operatorId, machineId, payAmout, recPayAmout, totalPayAmout, payMode, tradeNo);
        }

         public DataSet appointment_ReturnPay(string healthCardNo, string patientId, string orderId, string scheduleId, string periodId,
            string clinicSeq, string tradeNo, string medicareSettleLogId, string operatorId, string machineId, string refundFee,
            string refundTime, string refundReason)
        {
            return basic.appointment_ReturnPay(healthCardNo, patientId, orderId, scheduleId, periodId, clinicSeq, tradeNo,
                medicareSettleLogId, operatorId, machineId, refundFee, refundTime, refundReason);
        }


        [WebMethod(Description = "获取当天的门诊出诊科室列表信息")]
        public DataSet register_GetDeptInfo(string hospitalId, string deptId, string deptType)
        {
            return basic.register_GetDeptInfo(hospitalId, deptId, deptType);
        }

        [WebMethod(Description = "获取当天医生出诊信息查询")]
        public DataSet register_GetScheduleInfo(string hospitalId, string deptId, string deptType, string doctorId, string searchCode)
        {
            return basic.register_GetScheduleInfo(hospitalId, deptId, deptType, doctorId, searchCode);
        }

        [WebMethod(Description = "待缴费记录查询")]
        public DataSet outpatient_getPayInfo(string hospitalId, string healthCardNo, string patientId, string startDate, string endDate)
        {
            return basic.outpatient_getPayInfo(hospitalId, healthCardNo, patientId, startDate, endDate);
        }

        [WebMethod(Description = "获取待缴费用信息")]
        public DataSet outpatient_GetPaybillfee(string hospitalId, string healthCardNo, string patientId, string clinicSeq,
            string doctorId, string settleCode, string prescriptionIds)
        {
            return basic.outpatient_GetPaybillfee(hospitalId, healthCardNo, patientId, clinicSeq, doctorId, settleCode, prescriptionIds);
        }


        [WebMethod(Description = "待缴费记录支付")]
        public DataSet outpatient_Pay(string hospitalId, string healthCardNo, string patientId, string clinicSeq, string orderId,
            string tradeNo, string operatorId, string machineId, string payAmout, string recPayAmout, string totalPayAmout,
            string payMode, string payTime, string prescriptionIds, string medicareSettleLogId)
        {
            return outpatient_Pay(hospitalId, healthCardNo, patientId, clinicSeq, orderId, tradeNo, operatorId, machineId, payAmout, recPayAmout,
                totalPayAmout, payMode, payTime, prescriptionIds, medicareSettleLogId);
        }

        [WebMethod(Description = "已缴费记录查询")]
        public DataSet outpatient_GetCompletedPayInfo(string hospitalId, string healthCardNo, string patientId,
           string startDate, string endDate)
        {
            return basic.outpatient_GetCompletedPayInfo(hospitalId, healthCardNo, patientId, startDate, endDate);
        }

        [WebMethod(Description = "已缴费记录查询")]
        public DataSet outpatient_GetCompletedPayDetailInfo(string clinicSeq, string receiptId)
        {
            return basic.outpatient_GetCompletedPayDetailInfo(clinicSeq, receiptId);
        }

        [WebMethod(Description = "门诊处方查询")]
        public DataSet outpatient_GetPrescriptionInfo(string clinicSeq, string doctorId)
        {
            return basic.outpatient_GetPrescriptionInfo(clinicSeq, doctorId);
        }

        [WebMethod(Description = "门诊处方明细查询")]
        public DataSet outpatient_GetPrescriptionDetailInfo(string clinicSeq, string doctorId, string prescriptionId)
        {
            return basic.outpatient_GetPrescriptionDetailInfo(clinicSeq, doctorId, prescriptionId);
        }









        #endregion

        #region 消息
        [WebMethod(Description = "检查状态提醒")]
        public DataSet pacsStatusChanged()
        {
            return mess.pacsStatusChanged();
        }
        [WebMethod(Description = " 检验报告通知")]
        public DataSet lisReportCompleted()
        {
            return mess.lisReportCompleted();
        }
        [WebMethod(Description = "检查报告通知")]
        public DataSet pacsReportCompleted()
        {
            return mess.pacsReportCompleted();
        }
        [WebMethod(Description = "患者基本信息")]
        public DataSet updatePatientInfo()
        {
            return mess.updatePatientInfo();
        }
        [WebMethod(Description = "爽约通知")]
        public DataSet registerMissed()
        {
            return mess.registerMissed();
        }
        [WebMethod(Description = "健康卡注销")]
        public DataSet discard()
        {
            return mess.discard();
        }
        [WebMethod(Description = "就诊报到")]
        public DataSet visitReport()
        {
            return mess.visitReport();
        }
        [WebMethod(Description = " 住院每日清单通知")]
        public DataSet dailyBill()
        {
            return mess.dailyBill();

        }
        //[WebMethod(Description = " 医生停诊通知")]
        //public DataSet scheduleCancel() { 

        //}
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
        //public DataSet support_getStopDoctorInfo()
        //{

        //}
        [WebMethod(Description = "分页查询交易信息")]
        public DataSet support_pageQueryOrder(string orderId, string tradeDate, string productType, string payMode, string pageSize, string pageNo)
        {
            return mess.support_pageQueryOrder(orderId, tradeDate, productType, payMode, pageSize, pageNo);
        }
        #endregion
    }
}