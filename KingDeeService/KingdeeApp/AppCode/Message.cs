﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMR.Loger;
using System.Data;

namespace KingdeeApp
{
    /// <summary>
    /// 通信消息类
    /// </summary>
    public class Message
    {
        //his库链接
        string strHISConn = "HisConn";

        //检验报告通知
        public DataSet lisReportCompleted()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"SELECT A.PATIENT_ID,
                       A.PATIENT_ID,
                       A.INP_NO,
                       B.VISIT_NO,
                       C.RESULTS_RPT_DATE_TIME,
                       D.RESULT_DATE_TIME,
                       D.REPORT_ITEM_CODE,
                       D.REPORT_ITEM_NAME
                  FROM PAT_MASTER_INDEX A
                  JOIN CLINIC_MASTER B ON A.PATIENT_ID = B.PATIENT_ID
                  JOIN LAB_TEST_MASTER C ON B.PATIENT_ID = C.PATIENT_ID
                  JOIN LAB_RESULT D ON C.TEST_NO = D.TEST_NO";
            try
            {
                 dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());

            }
            return ds;
        }
        //检查报告通知
        public DataSet pacsReportCompleted()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"SELECT A.PATIENT_ID,
                           A.PATIENT_ID,
                           A.INP_NO,
                           B.VISIT_NO,
                           C.EXAM_DATE_TIME,
                           C.REPORT_DATE_TIME,
                           D.EXAM_ITEM_CODE,
                           D.EXAM_ITEM
                      FROM PAT_MASTER_INDEX A
                      JOIN CLINIC_MASTER B ON A.PATIENT_ID = B.PATIENT_ID
                      JOIN EXAM_MASTER C ON B.PATIENT_ID = C.PATIENT_ID
                      JOIN EXAM_ITEMS D ON C.EXAM_NO = D.EXAM_NO";
            dt = PubConn.Query(sql, strHISConn).Tables[0];
            try
            {
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());

            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //患者基本信息
        public DataSet updatePatientInfo()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"SELECT A.PATIENT_ID,
                           A.NAME,
                           A.SEX,
                           A.PATIENT_ID,
                           A.PATIENT_ID,
                           A.ID_NO,
                           A.DATE_OF_BIRTH,
                           A.PATIENT_ID,
                           A.INP_NO,
                           B.INSURANCE_NO
                      FROM PAT_MASTER_INDEX A
                      JOIN PAT_VISIT B ON A.PATIENT_ID = B.PATIENT_ID";
            try
            {
                 dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败，"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //健康卡注销
        public DataSet discard()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"SELECT A.PATIENT_ID, A.NAME, A.PATIENT_ID FROM PAT_MASTER_INDEX A";
            try
            {
                 dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        // 就诊报到
        public DataSet visitReport()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"SELECT C.DEPT_CODE, 
                                    C.DEPT_NAME, 
                                    A.DOCTOR,
                                    B.REGISTERING_DATE, 
                                    B.VISIT_DATE
                                     FROM CLINIC_INDEX A
                                     JOIN CLINIC_MASTER B ON A.CLINIC_LABEL = B.CLINIC_LABEL
                                     JOIN DEPT_DICT C ON C.DEPT_CODE = A.CLINIC_DEPT";
            try
            {
                 dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败，"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;

        }
        // 住院每日清单通知
        public DataSet dailyBill()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"SELECT A.PATIENT_ID,
                                   A.PATIENT_ID,
                                   B.INP_NO,
                                   C.BILLING_DATE_TIME,
                                   C.TOTAL_COSTS
                              FROM PAT_VISIT A
                              JOIN PAT_MASTER_INDEX B ON A.PATIENT_ID = B.PATIENT_ID
                              JOIN PATS_IN_HOSPITAL C ON C.PATIENT_ID = B.PATIENT_ID";
            try
            {
                 dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败，"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        // 扣费成功通知
        public DataSet medicalCardPay()
        {
            DataSet ds = new DataSet();
            string sql = @"";
            try
            {
                DataTable dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", null));
                ds.Tables.Add(dt);
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
            }
            return ds;
        }
        //检查状态提醒
        public DataSet pacsStatusChanged()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"SELECT A.PATIENT_ID, A.PATIENT_ID, B.EXAM_ITEM
                                  FROM EXAM_MASTER A
                                  JOIN EXAM_ITEMS B ON A.EXAM_NO = B.EXAM_NO  ";
            try
            {
                 dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败，"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //爽约通知
        public DataSet registerMissed()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"SELECT A.PATIENT_ID, A.NAME, B.VISIT_DATE
                                  FROM PAT_MASTER_INDEX A
                                  JOIN CLINIC_MASTER B ON A.PATIENT_ID = B.PATIENT_ID";
            try
            {
                 dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败，"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;

        }
        //医生停止通知
        //public DataSet scheduleCancel()
        //{

        //}
        //获取诊间支付二维码接口
        public DataSet outpatientPayScan(string doctorId, string doctorName, string deptId, string deptName, string clinicSeq, string clinicTime, string patientId, string patientName, string healthCardNo, string phone, string hospitalId, string settleCode, string settleType)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (doctorId == "")
            {
                PubConn.writeFileLog("医生代码不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,医生代码不能为空"));
                return ds;
            }
            if (doctorName == "")
            {
                PubConn.writeFileLog("医生姓名不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,医生姓名不能为空"));
                return ds;
            }
            if (deptId == "")
            {
                PubConn.writeFileLog("开单科室代码不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,开单科室代码不能为空"));
                return ds;
            }
            if (deptName == "")
            {
                PubConn.writeFileLog("开单科室名称不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,开单科室名称不能为空"));
                return ds;
            }
            if (clinicSeq == "")
            {
                PubConn.writeFileLog("就诊流水号不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,就诊流水号不能为空"));
                return ds;
            }
            if (clinicTime == "")
            {
                PubConn.writeFileLog("就诊时间不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,就诊时间不能为空"));
                return ds;
            }
            if (patientId == "")
            {
                PubConn.writeFileLog("患者ID不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,患者ID不能为空"));
                return ds;
            }
            if (patientName == "")
            {
                PubConn.writeFileLog("患者姓名不能为空");
                return null;
            }
            if (healthCardNo == "")
            {
                PubConn.writeFileLog("健康卡号不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,健康卡号不能为空"));
                return ds;
            }
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医院代码不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,医院代码不能为空"));
                return ds;
            }
            string sql = @"SELECT A.DOCTOR_ID,
                                   A.ORDERED_BY_DOCTOR,
                                   A.ORDERED_BY_DEPT,
                                   B.DEPT_NAME,
                                   C.VISIT_DATE,
                                   C.VISIT_NO,
                                   D.PATIENT_ID,
                                   D.NAME,
                                   D.NEXT_OF_KIN_PHONE
                              FROM OUTP_ORDER_DESC A
                              JOIN DEPT_DICT B ON A.ORDERED_BY_DEPT = B.DEPT_CODE
                              JOIN CLINIC_MASTER C ON C.PATIENT_ID = A.PATIENT_ID
                              JOIN PAT_MASTER_INDEX D ON C.PATIENT_ID = D.PATIENT_ID
                             WHERE 1=1";
            if (!string.IsNullOrEmpty(doctorId))
            {
                sql += "AND A.DOCTOR_ID ='" + doctorId + "'";
            }
            if (!string.IsNullOrEmpty(doctorName))
            {
                sql += " AND A.ORDERED_BY_DOCTOR ='" + doctorName + "'";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                sql += "AND A.ORDERED_BY_DEPT ='" + deptId + "'";
            }
            if (!string.IsNullOrEmpty(deptName))
            {
                sql += " AND B.DEPT_NAME ='" + deptName + "'";
            }
            if (!string.IsNullOrEmpty(clinicSeq))
            {
                sql += "AND  C.VISIT_NO='" + clinicSeq + "' ";
            }
            if (!string.IsNullOrEmpty(clinicTime))
            {
                sql += "AND C.VISIT_DATE ='" + clinicTime + "'";
            }
            if (!string.IsNullOrEmpty(patientId))
            {
                sql += "AND D.PATIENT_ID='" + patientId + "'";
            }
            if (!string.IsNullOrEmpty(patientName))
            {
                sql += "AND D.NAME='" + patientName + "'";
            }
            if (!string.IsNullOrEmpty(healthCardNo))
            {
                sql += "AND D.PATIENT_ID='" + healthCardNo + "'";
            }
            if (!string.IsNullOrEmpty(phone))
            {
                sql += " AND D.NEXT_OF_KIN_PHONE='" + phone + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());

            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //诊间预约通知
        //public DataSet clinicBooking() {
        //    DataSet ds = new DataSet();
        //    string sql = @"";

        //}
        //自定义消息
        public DataSet customMessage()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"SELECT A.PATIENT_ID, A.PATIENT_ID FROM CLINIC_MASTER A";
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());

            }
            return ds;
        }
        //门诊医生支付
        public DataSet outpatientPay()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @" SELECT U.USER_ID, A.ORDERED_BY_DOCTOR,
                                     A.ORDERED_BY_DEPT,
                                     B.DEPT_NAME,
                                     C.VISIT_NO,
                                     C.VISIT_DATE,
                                     A.PATIENT_ID,
                                     C.NAME,
                                     A.PATIENT_ID,
                                     D.NEXT_OF_KIN_PHONE,
                                     R.PAY_WAY_CODE,
                                     R.PAY_WAY_NAME
                                FROM OUTP_ORDER_DESC A
                                JOIN DEPT_DICT B ON A.ORDERED_BY_DEPT = B.DEPT_CODE
                                JOIN CLINIC_MASTER C ON C.PATIENT_ID = A.PATIENT_ID
                                JOIN PAT_MASTER_INDEX D ON C.PATIENT_ID = D.PATIENT_ID
                                JOIN OUTP_PAYMENTS_MONEY E ON E.RCPT_NO = A.RCPT_NO
                                JOIN PAY_WAY_DICT R ON E.MONEY_TYPE = R.PAY_WAY_NAME
                                JOIN USERS U ON A.ORDERED_BY_DOCTOR = U.USER_NAME";
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //住院预交金缴纳
        public DataSet inpatient_doPrepay(string orderId, string hospitalId, string idCardNo, string healthCardNo, string patientId, string inpatientId, string orderTime, string tradeNo, string operatorId, string machineId, string payAmout, string payMode)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (orderId == "")
            {
                PubConn.writeFileLog("移动订单号不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,移动订单号不能为空"));
                return ds;
            }
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医院代码不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,医院代码不能为空"));
                return ds;
            }
            if (patientId == "")
            {
                PubConn.writeFileLog("患者ID不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,患者ID不能为空"));
                return ds;
            }
            if (orderTime == "")
            {
                PubConn.writeFileLog("下单时间不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,下单时间不能为空"));
                return ds;
            }
            if (tradeNo == "")
            {
                PubConn.writeFileLog("支付流水不能为空");
                return null;
            }
            if (operatorId == "")
            {
                PubConn.writeFileLog("操作员工号不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,操作员工号不能为空"));
                return ds;
            }
            if (payAmout == "")
            {
                PubConn.writeFileLog("支付金额不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,支付金额不能为空"));
                return ds;
            }
            if (payMode == "")
            {
                PubConn.writeFileLog("支付方式不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,支付方式不能为空"));
                return ds;
            }
            string sql = @"SELECT C.CHECK_NO
                                  FROM PAT_VISIT A
                                  JOIN PAT_MASTER_INDEX B ON A.PATIENT_ID = B.PATIENT_ID
                                  JOIN PREPAYMENT_RCPT C ON C.PATIENT_ID = B.PATIENT_ID 
                                  WHERE 1=1";
            //移动订单号
            if (!string.IsNullOrEmpty(orderId))
            {

            }
            //医院代码
            if (!string.IsNullOrEmpty(hospitalId))
            {

            }
            //患者id
            if (!string.IsNullOrEmpty(patientId))
            {
                sql += "AND  A.PATIENT_ID='" + patientId + "'";
            }
            //住院号
            if (!string.IsNullOrEmpty(inpatientId))
            {
                sql += "B.INP_NO='" + inpatientId + "'";
            }
            //支付流水号
            if (!string.IsNullOrEmpty(tradeNo))
            {
                sql += " AND C.RCPT_NO='" + tradeNo + "'";
            }
            //操作员工
            if (!string.IsNullOrEmpty(operatorId))
            {
                sql += "AND C.OPERATOR_NO='" + operatorId + "'";
            }
            //支付金额
            if (!string.IsNullOrEmpty(payAmout))
            {
                sql += "AND C.AMOUNT='" + payAmout + "'";
            }
            //支付方式
            if (!string.IsNullOrEmpty(payMode))
            {
                sql += "C.PAY_WAY='" + payMode + "'";
            }
            if (!string.IsNullOrEmpty(idCardNo))
            {
                sql += "WHERE B.ID_NO='" + idCardNo + "'";
            }
            if (!string.IsNullOrEmpty(healthCardNo))
            {
                sql += "AND  A.PATIENT_ID='" + healthCardNo + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //预交定金

        public DataSet inpatient_getPrepayRecord(string inpatientId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (inpatientId != "42520068101")
            {
                PubConn.writeFileLog("医院代码不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,医院代码不能为空"));
                return ds;
            }

            string sql = @"SELECT B.TRANSACT_DATE, B.AMOUNT, B.PAY_WAY
                                  FROM PAT_MASTER_INDEX A
                                  JOIN PREPAYMENT_RCPT B ON A.PATIENT_ID = B.PATIENT_ID
                                 WHERE 1 = 1";
            if (!string.IsNullOrEmpty(inpatientId))
            {
                sql += "AND A.INP_NO='" + inpatientId + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //住院费用每日清单
        public DataSet inpatient_getDailyBill(string inpatientId, string billDate)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (inpatientId != "42520068101")
            {
                PubConn.writeFileLog("医院代码不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,医院代码不能为空"));
                return ds;
            }
            if (billDate == "")
            {
                PubConn.writeFileLog("日清单日期不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,日清单日期不能为空"));
                return ds;
            }
            string sql = @"SELECT
                           A.TOTAL_COSTS,
                           A.PREPAYMENTS,
                           A.TOTAL_CHARGES,
                           C.ITEM_NO,
                           C.RCPT_NO
                      FROM PATS_IN_HOSPITAL A
                      JOIN PAT_MASTER_INDEX B ON A.PATIENT_ID = B.PATIENT_ID
                      JOIN INP_BILL_DETAIL C ON C.PATIENT_ID = B.PATIENT_ID
                       WHERE 1=1";
            if (!string.IsNullOrEmpty(inpatientId))
            {
                sql += "AND B.INP_NO='" + inpatientId + "'";
            }
            if (!string.IsNullOrEmpty(billDate))
            {
                sql += "AND A.BILLING_DATE_TIME='" + billDate + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //住院汇总查询
        public DataSet inpatient_getTotalCost(string inpatientId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (inpatientId != "42520068101")
            {
                PubConn.writeFileLog("医院代码不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,医院代码不能为空"));
                return ds;
            }
            string sql = @"SELECT 
                           A.TOTAL_COSTS,
                           A.PREPAYMENTS,
                           A.TOTAL_CHARGES,
                           C.ITEM_NAME,
                           C.ITEM_CODE
                      FROM PAT_VISIT A
                      JOIN PAT_MASTER_INDEX B ON A.PATIENT_ID = B.PATIENT_ID
                      JOIN INP_BILL_DETAIL C ON C.PATIENT_ID = B.PATIENT_ID
                      WHERE 1=1";
            if (!string.IsNullOrEmpty(inpatientId))
            {
                sql += "AND  B.INP_NO='" + inpatientId + "'";

            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //住院费用分类明细查询
        public DataSet inpatient_getDetailCost(string inpatientId, string typeCode, string billDate, string startMxseq, string endMxseq)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (inpatientId == "")
            {
                PubConn.writeFileLog("住院号不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,住院号不能为空"));
                return ds;
            }
            if (typeCode == "")
            {
                PubConn.writeFileLog("费用分类代码");
                ds.Tables.Add(GETReport("-1", "查询失败,费用分类代码"));
                return ds;
            }
            string sql = @"SELECT B.INP_NO,
                                   C.ITEM_CLASS,
                                   C.ITEM_NO,
                                   C.RCPT_NO,
                                   C.ITEM_NAME,
                                   C.ITEM_CODE,
                                   C.ITEM_NAME,
                                   C.ITEM_SPEC,
                                   C.AMOUNT,
                                   C.UNITS
                              FROM PAT_VISIT A
                              JOIN PAT_MASTER_INDEX B ON A.PATIENT_ID = B.PATIENT_ID
                              JOIN INP_BILL_DETAIL C ON C.PATIENT_ID = B.PATIENT_ID
                             WHERE 1 = 1";
            if (!string.IsNullOrEmpty(inpatientId))
            {
                sql += "AND B.INP_NO='" + inpatientId + "'";
            }
            if (!string.IsNullOrEmpty(typeCode))
            {
                sql += " AND C.ITEM_CLASS='" + typeCode + "'";
            }
            if (!string.IsNullOrEmpty(startMxseq))
            {
                sql += " AND  C.ITEM_NO='" + startMxseq + "'";
            }
            if (!string.IsNullOrEmpty(endMxseq))
            {
                sql += " AND  C.RCPT_NO='" + endMxseq + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //患者住院业务功能检测
        public DataSet inpatient_operationCheck(string inpatienId, string operType)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"SELECT B.INP_NO
                                      FROM PAT_VISIT A
                                      JOIN PAT_MASTER_INDEX B ON A.PATIENT_ID = B.PATIENT_ID
                                      WHERE 1=1";
            if (!string.IsNullOrEmpty(inpatienId))
            {
                sql += "AND B.INP_NO='" + inpatienId + "'";
            }
            if (!string.IsNullOrEmpty(operType))
            {
                // sql += "AND B.INP_NO='" + inpatienId + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        // 检验报告列表查询
        public DataSet lis_getReport(string healthCardNo, string patientId, string clinicSeq, string beginDate, string endDate)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (healthCardNo == "")
            {
                PubConn.writeFileLog("患者健康卡号不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,患者健康卡号不能为空"));
                return ds;
            }
            if (patientId == "")
            {
                PubConn.writeFileLog("患者ID不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,患者ID不能为空"));
                return ds;
            }
            if (beginDate == "")
            {
                PubConn.writeFileLog("开始时间不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,开始时间不能为空"));
                return ds;
            }
            if (endDate == "")
            {
                PubConn.writeFileLog("结束时间不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,结束时间不能为空"));
                return ds;
            }
            string sql = @"SELECT A.PATIENT_ID,
                                   D.INP_NO,
                                   B.VISIT_NO,
                                   A.EXECUTE_DATE,
                                   A.RESULTS_RPT_DATE_TIME,
                                   C.TEST_NO,
                                   F.REPORT_ITEM_NAME,
                                   A.REQUESTED_DATE_TIME,
                                   A.RESULT_STATUS,
                                   A.NAME,
                                   A.AGE,
                                   A.SEX,
                                   E.DEPT_NAME,
                                   A.RELEVANT_CLINIC_DIAG,
                                   U.USER_NAME,
                                   A.TEST_NO
                              FROM LAB_TEST_MASTER A
                              JOIN CLINIC_MASTER B ON A.PATIENT_ID = B.PATIENT_ID
                              JOIN LAB_TEST_ITEMS C ON C.TEST_NO = A.TEST_NO
                              JOIN LAB_RESULT F ON F.TEST_NO = A.TEST_NO
                              JOIN PAT_MASTER_INDEX D ON A.PATIENT_ID = D.PATIENT_ID
                              JOIN DEPT_DICT E ON E.DEPT_CODE = A.ORDERING_DEPT
                              JOIN USERS U ON U.USER_DEPT = A.ORDERING_DEPT
                              WHERE 1=1";
            if (!string.IsNullOrEmpty(healthCardNo))
            {
                sql += "AND A.PATIENT_ID='" + healthCardNo + "'";
            }
            if (!string.IsNullOrEmpty(patientId))
            {
                sql += "AND  D.INP_NO='" + patientId + "'";
            }
            if (!string.IsNullOrEmpty(clinicSeq))
            {
                sql += "AND B.VISIT_NO='" + clinicSeq + "'";
            }
            if (!string.IsNullOrEmpty(beginDate))
            {
                sql += "AND A.EXECUTE_DATE='" + beginDate + "'";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                sql += "AND  A.REQUESTED_DATE_TIME='" + endDate + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //检验报告明细内容查询
        public DataSet lis_getReportItem(string inspectionId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (inspectionId == "")
            {
                PubConn.writeFileLog("检验报告ID不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,检验报告ID不能为空"));
                return ds;
            }
            string sql = @"SELECT A.TEST_NO,
                                   A.REPORT_ITEM_CODE,
                                   B.PRIORITY_INDICATOR,
                                   A.UNITS,
                                   A.RESULT_DATE_TIME RESULT
                              FROM LAB_RESULT A
                              JOIN LAB_TEST_MASTER B ON A.TEST_NO = B.TEST_NO
                              WHERE 1=";
            if (!string.IsNullOrEmpty(inspectionId))
            {
                sql += "AND A.TEST_NO='" + inspectionId + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //检查报告列表查询接口
        public DataSet pacs_getReport(string healthCardNo, string patientId, string inpatientId, string clinicSeq, string beginDate, string endDate)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (healthCardNo == "")
            {
                PubConn.writeFileLog("健康卡号不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,健康卡号不能为空"));
                return ds;
            }
            if (patientId == "")
            {
                PubConn.writeFileLog("患者ID不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,患者ID不能为空"));
                return ds;
            }
            if (beginDate == "")
            {
                PubConn.writeFileLog("开始时间不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,开始时间不能为空"));
                return ds;
            }
            if (endDate == "")
            {
                PubConn.writeFileLog("结束时间不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,结束时间不能为空"));
                return ds;
            }
            string sql = @"SELECT A.PATIENT_ID,
                                   A.PATIENT_ID,
                                   B.INP_NO,
                                   C.VISIT_NO,
                                   A.EXAM_DATE_TIME,
                                   A.REPORT_DATE_TIME,
                                   A.EXAM_NO,
                                   A.PHYS_SIGN,
                                   A.REQ_DATE_TIME,
                                   A.RESULT_STATUS,
                                   A.NAME,
                                   TO_CHAR(ROUND(MONTHS_BETWEEN(SYSDATE, A.DATE_OF_BIRTH) / 12)),
                                   A.SEX,
                                   D.DEPT_NAME,
                                   A.CLIN_DIAG,
                                   C.VISIT_NO,
                                   B.INP_NO
                              FROM EXAM_MASTER A
                              JOIN PAT_MASTER_INDEX B ON A.PATIENT_ID = B.PATIENT_ID
                              JOIN CLINIC_MASTER C ON C.PATIENT_ID = B.PATIENT_ID
                              JOIN DEPT_DICT D ON A.PERFORMED_BY = D.DEPT_CODE
                             WHERE 1 = 1";
            if (!string.IsNullOrEmpty(healthCardNo))
            {
                sql += "AND A.PATIENT_ID='" + healthCardNo + "'";
            }
            if (!string.IsNullOrEmpty(patientId))
            {
                sql += "AND A.PATIENT_ID='" + patientId + "'";
            }
            if (!string.IsNullOrEmpty(clinicSeq))
            {
                sql += "AND  B.INP_NO='" + clinicSeq + "'";
            }
            if (!string.IsNullOrEmpty(beginDate))
            {
                sql += "AND  A.EXAM_DATE_TIME='" + beginDate + "'";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                sql += "AND  A.REPORT_DATE_TIME='" + endDate + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        // 检查报告明细内容查询接口
        public DataSet pacs_getReportDetail(string reportId)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (reportId == "")
            {
                PubConn.writeFileLog("检验报告ID不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,检验报告ID不能为空"));
                return ds;
            }
            string sql = @"SELECT A.EXAM_NO,
                                   B.DEPT_NAME,
                                   A.REPORTER,
                                   C.DESCRIPTION,
                                   A.RESULT_STATUS,
                                   A.REQ_PHYSICIAN,
                                   A.EXAM_DATE_TIME
                              FROM EXAM_MASTER A
                              JOIN DEPT_DICT B ON A.REQ_DEPT = B.DEPT_CODE
                              JOIN EXAM_REPORT C ON A.EXAM_NO = C.EXAM_NO
                              WHERE 1=1";
            if (!string.IsNullOrEmpty(reportId))
            {
                sql += "AND  A.EXAM_NO='" + reportId + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //挂号记录查询
        public DataSet support_getRegisterInfo(string healthCardNo, string clinicSeq, string patientId, string orderId, string orderDate, string visitDate, string orderStatus)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (healthCardNo == "" && clinicSeq == "" && patientId == "" && orderId == "" && orderDate == "" && visitDate == "" && orderStatus == "")
            {
                PubConn.writeFileLog("患者卡号,就诊流水,患者ID,移动订单号,挂号日期,就诊日期,挂号状态不能同时为空");
                ds.Tables.Add(GETReport("-1", "查询失败,患者卡号,就诊流水,患者ID,移动订单号,挂号日期,就诊日期,挂号状态不能同时为空"));

            }
            string sql = @"SELECT A.PATIENT_ID,
       A.VISIT_NO,
       A.PATIENT_ID,
       A.RCPT_NO,
       A.REGISTERING_DATE,
       A.VISIT_DATE,
       A.REGISTRATION_STATUS,
       A.VISIT_NO,
       A.VISIT_DEPT,
       B.DEPT_NAME,
       C.USER_DEPT,
       A.DOCTOR,
       A.SERIAL_NO,
       A.VISIT_DATE
  FROM CLINIC_MASTER A
  JOIN DEPT_DICT B ON A.VISIT_DEPT = B.DEPT_CODE
  JOIN USERS C ON C.USER_NAME = A.DOCTOR
  WHERE 1=1";
            if (!string.IsNullOrEmpty(healthCardNo))
            {
                sql += "AND A.PATIENT_ID='" + healthCardNo + "'";
            }
            if (!string.IsNullOrEmpty(clinicSeq))
            {
                sql += "AND  A.VISIT_NO='" + clinicSeq + "'";
            }
            if (!string.IsNullOrEmpty(patientId))
            {
                sql += "AND  A.PATIENT_ID='" + patientId + "'";
            }
            if (!string.IsNullOrEmpty(orderId))
            {
                sql += "AND   A.RCPT_NO='" + orderId + "'";
            }
            if (!string.IsNullOrEmpty(orderDate))
            {
                sql += "AND   A.REGISTERING_DATE='" + orderDate + "'";
            }
            if (!string.IsNullOrEmpty(visitDate))
            {
                sql += "AND    A.VISIT_DATE='" + visitDate + "'";
            }
            if (!string.IsNullOrEmpty(orderStatus))
            {
                sql += "AND   A.REGISTRATION_STATUS='" + visitDate + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //获取指引单
        public DataSet support_getGuideList(string clinicSeq, string receiptId)
        {

            if (clinicSeq == "")
            {
                PubConn.writeFileLog("就诊账号不能为空");
                return null;
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"SELECT A.VISIT_NO, A.RCPT_NO, A.PERFORMED_BY, B.DEPT_NAME, A.ITEM_NAME
  FROM OUTP_BILL_ITEMS A
  JOIN DEPT_DICT B ON A.PERFORMED_BY = B.DEPT_CODE WHERE 1=1";
            if (!string.IsNullOrEmpty(clinicSeq))
            {
                sql += "AND A.VISIT_NO='" + clinicSeq + "'";
            }
            if (!string.IsNullOrEmpty(receiptId))
            {
                sql += "AND A.RCPT_NO='" + receiptId + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //卡信息查询
        public DataSet user_getCardInfo(string idCardNo, string patientName, string phone)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (idCardNo == "")
            {
                PubConn.writeFileLog("身份证不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,身份证不能为空"));
                return ds;
            }
            if (patientName == "")
            {
                PubConn.writeFileLog("姓名不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,姓名不能为空"));
                return ds;
            }

            string sql = @"SELECT B.ID_NO,
                                   B.NAME,
                                   B.PHONE_NUMBER_HOME,
                                   B.VIP_INDICATOR,
                                   A.PATIENT_ID,
                                   B.CREATE_DATE
                              FROM PATS_IN_HOSPITAL A
                              JOIN PAT_MASTER_INDEX B ON A.PATIENT_ID = B.PATIENT_ID
                              WHERE 1=1";
            if (!string.IsNullOrEmpty(idCardNo))
            {
                sql += "AND B.ID_NO='" + idCardNo + "'";
            }
            if (!string.IsNullOrEmpty(patientName))
            {
                sql += "AND B.NAME='" + patientName + "'";
            }
            if (!string.IsNullOrEmpty(phone))
            {
                sql += "AND B.PHONE_NUMBER_HOME='" + phone + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        //获取用户可用服务对象列表
        public DataSet support_getSvObject(string hospitalId, string healthCardNo)
        {
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医院代码不能为空");
                return null;
            }
            if (healthCardNo == "")
            {
                PubConn.writeFileLog("健康卡号不能我空");
                return null;
            }
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sql = @"";

            return ds;
        }
        // 分页查询交易信息
        public DataSet support_pageQueryOrder(string orderId, string tradeDate, string productType, string payMode, string pageSize, string pageNo)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            if (orderId == "" && tradeDate == "")
            {
                PubConn.writeFileLog("移动订单号与交易时间不能同时为空");
                ds.Tables.Add(GETReport("-1", "查询失败,移动订单号与交易时间不能同时为空"));
                return ds;
            }
            if (payMode == "")
            {
                PubConn.writeFileLog("交易方式不能为空");
                ds.Tables.Add(GETReport("-1", "查询失败,交易方式不能为空"));
                return ds;
            }
            string sql = @"SELECT A.ORDER_ID,
                                   A.TRADE_DATE,
                                   A.PAY_MODE,
                                   COUNT(*),
                                   A.ORDER_ID,
                                   A.TRADE_DATE,
                                   A.TRADE_NO,
                                   A.ORDER_ID,
                                   A.COSTS
                              FROM TRADE_RECORD A
                             GROUP BY A.ORDER_ID,
                                      A.TRADE_DATE,
                                      A.PAY_MODE,
                                      A.TRADE_NO,
                                      A.ORDER_ID,
                                      A.COSTS
                                      WHERE 1=1";
            if (!string.IsNullOrEmpty(orderId))
            {
                sql += "AND A.ORDER_ID='" + orderId + "'";
            }
            if (!string.IsNullOrEmpty(tradeDate))
            {
                sql += "AND A.TRADE_DATE='" + tradeDate + "'";
            }
            if (!string.IsNullOrEmpty(payMode))
            {
                sql += "AND A.PAY_MODE='" + payMode + "'";
            }
            try
            {
                dt = PubConn.Query(sql, strHISConn).Tables[0];
                ds.Tables.Add(GETReport("0", "查询成功"));
                ds.Tables.Add(dt.Copy());
            }
            catch (Exception ex)
            {

                PubConn.writeFileLog(ex.Message + "\r\n" + sql + "\r\n");
                ds.Tables.Add(GETReport("-1", "查询失败,"+ex.Message));
                ds.Tables.Add(dt.Copy());
            }
            return ds;
        }
        public DataTable GETReport(string resultCode, string resultDesc)
        {
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("resultCode");
            dt1.Columns.Add("resultDesc");
            dt1.TableName = "handleStatus";
            DataRow dr = dt1.NewRow();

            dr[0] = resultCode;
            dr[1] = resultDesc;
            dt1.Rows.Add(dr);
            return dt1;
        }
    }
}