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
        public DataSet hospital_getDeptInfo(string hospitalId, string deptId, string deptType)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(hospitalId))
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码不能为空"));
                return ds;
            }
            else
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            DataTable dt2 = new DataTable();
            string strSql = @"SELECT A.DEPT_CODE DEPTID,
                                   A.DEPT_NAME DEPTNAME,
                                   '' DEPTTYPE,
                                   (CASE
                                     WHEN A.PARENT_CODE IS NULL THEN
                                      '-1'
                                     ELSE
                                      A.PARENT_CODE
                                   END) PARENTID,
                                   A.DEPT_DESC DESCRIPTION
                               FROM DEPT_DICT A WHERE 1=1";
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
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败" + ex.Message));
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
        public DataSet hospital_getDoctorInfo(string hospitalId, string deptId, string doctorid)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(hospitalId))
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码不能为空"));
                return ds;
            }
            else
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
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
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
            }
            return ds;
        }

        /// <summary>
        /// 获取门诊患者基本信息
        /// </summary>
        /// <param name="idCardNo">身份证号码</param>
        /// <param name="healthCardNo">健康卡号码</param>
        /// <param name="patientId">患者唯一ID</param>
        /// <param name="patientName">姓名</param>
        /// <param name="guardianName">监护人姓名</param>
        /// <param name="guardianIdCardNo">监护人身份证</param>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        public DataSet baseinfo_GetOutpatientInfo(string idCardNo, string healthCardNo, string patientId, string patientName,
            string guardianName, string guardianIdCardNo, string phone)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(idCardNo) && string.IsNullOrEmpty(healthCardNo) && string.IsNullOrEmpty(patientId))
            {
                PubConn.writeFileLog("身份证号、健康卡号、病人ID号不能同事为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,idCardNo、healthCardNo、patientId不能同时为空。"));
                return ds;
            }
            DataTable dt2 = new DataTable();
            string strSql = @"SELECT A.PATIENT_ID PATIENTID,
                                   A.NAME PATIENTNAME,
                                   (CASE
                                     WHEN A.SEX = '男' THEN
                                      'M'
                                     ELSE
                                      '女'
                                   END) AS GENDER,
                                   A.PATIENT_ID HEALTHCARDNO,
                                   A.ID_NO IDCARDNO,
                                   TO_CHAR(A.DATE_OF_BIRTH, 'YYYY-MM-DD'),
                                   A.PATIENT_ID OPPATNO,
                                   A.PHONE_NUMBER_HOME PHONE,
                                   B.INSURANCE_NO MEDICARECARDNO
                              FROM PAT_MASTER_INDEX A
                              LEFT JOIN INSURANCE_ACCOUNTS B
                                ON A.PATIENT_ID = B.PATIENT_ID WHERE 1=1";
            if (!string.IsNullOrEmpty(idCardNo))
            {
                strSql += " AND A.ID_NO='" + idCardNo + "' ";
            }
            if (!string.IsNullOrEmpty(healthCardNo))
            {
                strSql += " AND A.PATIENT_ID='" + healthCardNo + "' ";
            }
            if (!string.IsNullOrEmpty(patientId))
            {
                strSql += " AND A.PATIENT_ID='" + patientId + "' ";
            }
            if (!string.IsNullOrEmpty(patientName))
            {
                strSql += " AND A.NAME='" + patientName + "' ";
            }
            if (!string.IsNullOrEmpty(guardianName))
            {
                strSql += " AND A.NEXT_OF_KIN='" + guardianName + "' ";
            }
            if (!string.IsNullOrEmpty(guardianIdCardNo))
            {
                //strSql += " AND A.PATIENT_ID='" + guardianIdCardNo + "' ";
            }
            if (!string.IsNullOrEmpty(phone))
            {
                strSql += " AND A.PHONE_NUMBER_HOME='" + phone + "' ";
            }

            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
            }
            return ds;
        }

        /// <summary>
        /// 获取住院患者信息
        /// </summary>
        /// <param name="idCardNo">身份证号码</param>
        /// <param name="healthCardNo">健康卡号码</param>
        /// <param name="patientId">患者唯一ID</param>
        /// <param name="inpatientId">住院号</param>
        /// <returns></returns>
        public DataSet baseinfo_GetInpatientInfo(string idCardNo, string healthCardNo, string patientId, string inpatientId)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(healthCardNo))
            {
                PubConn.writeFileLog("健康卡号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,健康卡号不能为空"));
                return ds;
            }
            DataTable dt2 = new DataTable();
            string strSql = @"SELECT B.PATIENT_ID PATIENTID,
                                   '42520068101' HOSPITALID,
                                   B.NAME PATIENTNAME,
                                   A.ADMISSION_DATE_TIME INTIME,
                                   A.DISCHARGE_DATE_TIME OUTTIME,
                                   CEIL((CASE
                                          WHEN A.DISCHARGE_DATE_TIME IS NULL THEN
                                           SYSDATE
                                          ELSE
                                           A.DISCHARGE_DATE_TIME
                                        END) - A.ADMISSION_DATE_TIME) INDAYS,
                                   (CASE
                                     WHEN C.WARD_CODE IS NULL THEN
                                      '已出院'
                                     ELSE
                                      '在院'
                                   END) PATIENTFLAG,
                                   C.TOTAL_COSTS TOTALAMOUT,
                                   C.PREPAYMENTS PREPAYAMOUT,
                                   C.PREPAYMENTS BALANCE,
                                   '0' SETTLED,
                                   B.INP_NO INPATIENTID,
                                   B.ID_NO IDCARDNO,
                                   C.WARD_CODE DEPTID,
                                   (SELECT DEPT_NAME FROM DEPT_DICT WHERE DEPT_CODE = C.WARD_CODE) DEPTNAME,
                                   C.BED_NO BEDNO,
                                   (SELECT USER_ID
                                      FROM USERS
                                     WHERE USER_NAME = A.ATTENDING_DOCTOR
                                       AND ROWNUM = 1) CHARGEDOCTORID,
                                   A.ATTENDING_DOCTOR CHARGEDOCTORNAME,
                                   (SELECT USER_ID
                                      FROM USERS
                                     WHERE USER_NAME = D.RESPONSE_NURSE
                                       AND ROWNUM = 1) CHARGENURSEID,
                                   D.RESPONSE_NURSE CHARGENURSENAME,
                                   B.SEX GENDER,
                                   TO_CHAR(B.DATE_OF_BIRTH, 'YYYY-MM-DD') BIRTHDAY,
                                   B.MAILING_ADDRESS ADDRESS,
                                   B.PHONE_NUMBER_HOME CONNECTPHONE,
                                   B.NEXT_OF_KIN RELATEPERSON,
                                   E.RELATIONSHIP_NAME RELATION,
                                   B.NEXT_OF_KIN_PHONE RELATEPHONE,
                                   '' REMARK

                              FROM PAT_VISIT A
                              LEFT JOIN PAT_MASTER_INDEX B
                                ON A.PATIENT_ID = B.PATIENT_ID
                              LEFT JOIN PATS_IN_HOSPITAL C
                                ON A.PATIENT_ID = C.PATIENT_ID
                               AND A.VISIT_ID = C.VISIT_ID
                              LEFT JOIN PAT_VISIT_ZH D
                                ON A.PATIENT_ID = D.PATIENT_ID
                               AND A.VISIT_ID = D.VISIT_ID
                              LEFT JOIN RELATIONSHIP_DICT E
                                ON B.RELATIONSHIP = E.RELATIONSHIP_CODE WHERE 1=1";
            if (!string.IsNullOrEmpty(idCardNo))
            {
                strSql += " AND B.ID_NO='" + idCardNo + "' ";
            }
            if (!string.IsNullOrEmpty(healthCardNo))
            {
                strSql += " AND B.PATIENT_ID='" + healthCardNo + "' ";
            }
            if (!string.IsNullOrEmpty(patientId))
            {
                strSql += " AND B.PATIENT_ID='" + patientId + "' ";
            }
            if (!string.IsNullOrEmpty(inpatientId))
            {
                strSql += " AND B.INP_NO='" + inpatientId + "' ";
            }

            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
            }
            return ds;
        }

        /// <summary>
        /// 获取门诊患者就诊信息
        /// </summary>
        /// <param name="hospitalId">医院代码</param>
        /// <param name="healthCardNo">用户健康卡号码</param>
        /// <param name="patientId">患者唯一ID</param>
        /// <param name="doctorId">医生工号</param>
        /// <param name="startDate">查询开始时间（就诊时间）格式：YYYY-MM-DD HH24:MI:SS</param>
        /// <param name="endDate">查询结束时间（就诊时间）格式：YYYY-MM-DD HH24:MI:SS</param>
        /// <param name="diseaseLabel">疾病标签(汉字)，多个标签用半角逗号分隔</param>
        /// <param name="ICD">ICD码，多个标签用半角逗号分隔</param>
        /// <returns></returns>
        public DataSet baseinfo_GetOutpatientVisitPatient(string hospitalId, string healthCardNo, string patientId, string doctorId,
            string startDate, string endDate, string diseaseLabel, string ICD)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(doctorId) && string.IsNullOrEmpty(healthCardNo) && string.IsNullOrEmpty(patientId))
            {
                PubConn.writeFileLog("医生工号、健康卡号、病人ID号不能同时为空");
                ds.Tables.Add(GetStatus("-1", "查询失败，医生工号、健康卡号、病人ID号不能同时为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                PubConn.writeFileLog("开始日期不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,开始日期不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(endDate))
            {
                PubConn.writeFileLog("结束日期不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,结束日期不能为空"));
                return ds;
            }
            DataTable dt2 = new DataTable();
            string strSql = @"SELECT A.VISIT_NO CLINICSEQ,
                                   CASE
                                     WHEN A.SEX = '男' THEN
                                      'M'
                                     ELSE
                                      'F'
                                   END GENDER,
                                   A.PATIENT_ID HEALTHCARDNO,
                                   B.ID_NO IDCARDNO,
                                   TO_CHAR(B.DATE_OF_BIRTH, 'YYYY-MM-DD') BIRTHDAY,
                                   A.PATIENT_ID PATIENTID,
                                   A.NAME PATIENTNAME,
                                   A.DOCTOR DOCTORNAME,
                                   A.REGISTERING_DATE VISITDATE,
                                   A.VISIT_DEPT DEPTID,
                                   (SELECT DEPT_NAME FROM DEPT_DICT WHERE DEPT_CODE = A.VISIT_DEPT) DEPTNAME,
                                   '' DIAG,
                                   '' ICD
                              FROM CLINIC_MASTER A
                              LEFT JOIN PAT_MASTER_INDEX B
                                ON A.PATIENT_ID = B.PATIENT_ID WHERE 1=1";
            if (!string.IsNullOrEmpty(hospitalId))
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败"));
                    return ds;
                }
            }
            if (!string.IsNullOrEmpty(healthCardNo))
            {
                strSql += " AND A.PATIENT_ID='" + healthCardNo + "' ";
            }
            if (!string.IsNullOrEmpty(patientId))
            {
                strSql += " AND A.PATIENT_ID='" + patientId + "' ";
            }
            if (!string.IsNullOrEmpty(doctorId))
            {
                strSql += " AND (SELECT USER_ID FROM USERS WHERE USER_NAME=A.DOCTOR AND ROWNUM=1) ='" + doctorId + "' ";
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                strSql += " AND A.REGISTERING_DATE >=TO_DATE('" + startDate + "','YYYY-MM-DD HH24:MI:SS') ";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                strSql += " AND A.REGISTERING_DATE <=TO_DATE('" + endDate + "','YYYY-MM-DD HH24:MI:SS') ";
            }
            if (!string.IsNullOrEmpty(diseaseLabel))
            {
                strSql += " AND A.PATIENT_ID='" + diseaseLabel + "' ";
            }
            if (!string.IsNullOrEmpty(ICD))
            {
                strSql += " AND A.PATIENT_ID='" + ICD + "' ";
            }

            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];

                #region 诊断处理
                //if (dt2 != null && dt2.Rows.Count > 0)
                //{
                //    string diagnosis = null;
                //    string icd = null;
                //    for (int i = 0; i < dt2.Rows.Count; i++)
                //    {
                //        DataRow dr = dt2.Rows[i];
                //        strSql = @"";
                //        DataTable dtDiagnosis = PubConn.Query(strSql, strHISConn).Tables[0];
                //        if (dtDiagnosis != null && dtDiagnosis.Rows.Count > 0)
                //        {

                //            for (int j = 0; j < dtDiagnosis.Rows.Count; j++)
                //            {
                //                DataRow drdiag = dtDiagnosis.Rows[j];
                //                diagnosis += drdiag["DIAGNOSIS_DESC"].ToString() + ";";
                //                icd += drdiag["DIAGNOSIS_CODE"].ToString() + ";";
                //            }
                //        }
                //        if (!string.IsNullOrEmpty(diagnosis))
                //        {
                //            dr["DIAG"] = diagnosis.Substring(0, diagnosis.Length - 1);
                //        }
                //        if (!string.IsNullOrEmpty(icd))
                //        {
                //            dr["ICD"] = icd.Substring(0, diagnosis.Length - 1);
                //        }
                //    }
                //} 
                #endregion

                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败，" + ex.Message));
            }
            return ds;
        }

        /// <summary>
        /// 获取门诊科室列表
        /// </summary>
        /// <param name="hospitalId">医疗机构代码</param>
        /// <param name="deptId">科室编码</param>
        /// <param name="deptType">科室类型   暂时没有专家、专科分类（1专科2 专家）</param>
        /// <returns></returns>
        public DataSet appointment_GetDeptInfo(string hospitalId, string deptId, string deptType)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(hospitalId))
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码不能为空"));
                return ds;
            }
            else
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            DataTable dt2 = new DataTable();
            string strSql = @"SELECT A.DEPT_CODE DEPTID,
                                   A.DEPT_NAME DEPTNAME,
                                   '' DEPTTYPE,
                                   (CASE
                                     WHEN A.PARENT_CODE IS NULL THEN
                                      '-1'
                                     ELSE
                                      A.PARENT_CODE
                                   END) PARENTID,
                                   A.DEPT_DESC DESCRIPTION
                               FROM DEPT_DICT A  WHERE A.OUTP_OR_INP IN('0','2')";
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
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }

        /// <summary>
        /// 医生出诊信息查询
        /// </summary>
        /// <param name="hospitalId">医院代码(必填)</param>
        /// <param name="deptId">科室代码</param>
        /// <param name="deptType">科室分类：例如：1专科2 专家</param>
        /// <param name="doctorId">医生代码</param>
        /// <param name="searchCode">搜索关键字---医生姓名</param>
        /// <param name="startDate">号源开始日期,格式：YYYY-MM-DD(必填)</param>
        /// <param name="endDate">号源结束日期,格式：YYYY-MM-DD(必填)</param>
        /// <returns></returns>
        public DataSet appointment_GetScheduleInfo(string hospitalId, string deptId, string deptType, string doctorId,
            string searchCode, string startDate, string endDate)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(hospitalId))
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码不能为空"));
                return ds;
            }
            else
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            if (string.IsNullOrEmpty(startDate))
            {
                PubConn.writeFileLog("开始日期不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,开始日期不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(endDate))
            {
                PubConn.writeFileLog("结束日期不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,结束日期不能为空"));
                return ds;
            }
            DataTable dt2 = new DataTable();
            string strSql = @"SELECT DISTINCT *
                              FROM (SELECT (SELECT USER_ID
                                              FROM USERS
                                             WHERE USER_NAME = B.DOCTOR
                                               AND ROWNUM = 1) DOCTORID,
                                           B.DOCTOR DOCTORNAME,
                                           (SELECT TITLE_CODE
                                              FROM TITLE_DICT
                                             WHERE TITLE_NAME = E.TITLE
                                               AND ROWNUM = 1) DOCTORLEVELCODE,
                                           E.TITLE DOCTORLEVEL,
                                           E.DOCTOR_DESC DESCRIPTION,
                                           F.CLINIC_DEPT DEPTID,
                                           (SELECT DEPT_NAME
                                              FROM DEPT_DICT
                                             WHERE DEPT_CODE = F.CLINIC_DEPT) DEPTNAME,
                                           F.CLINIC_LABEL CLINICUNITNAME,
                                           F.SERIAL_NO CLINICUNITID,
                                           A.CLINIC_DATE REGDATE,
                                           (SELECT TIME_INTERVAL_CODE
                                              FROM TIME_INTERVAL_DICT
                                             WHERE TIME_INTERVAL_NAME = A.TIME_DESC
                                               AND ROWNUM = 1) SHIFTCODE,
                                           A.TIME_DESC SHIFTNAME,
                                           '' STARTTIME,
                                           '' ENDTIME,
                                           '0' SCHEDULEID,
                                           '1' REGSTATUS,
                                           A.REGISTRATION_LIMITS REGTOTALCOUNT,
                                           NVL((NVL(A.APPOINTMENT_LIMITS, 0) - NVL(A.CURRENT_NO, 0)), 0) REGLEAVECOUNT,
                                           B.REGIST_FEE REGFEE,
                                           B.CLINIC_FEE TREATFEE,
                                           '1' ISTIMEREG
                                      FROM CLINIC_FOR_REGIST A
                                      LEFT JOIN CLINIC_MASTER B
                                        ON A.CLINIC_DATE = B.VISIT_DATE
                                       AND A.CLINIC_LABEL = B.CLINIC_LABEL
                                       AND A.TIME_DESC = B.VISIT_TIME_DESC
                                      LEFT JOIN (SELECT DISTINCT C.*
                                                  FROM STAFF_DICT C
                                                 RIGHT JOIN CLINIC_MASTER D
                                                    ON C.NAME = D.DOCTOR) E
                                        ON B.DOCTOR = E.NAME
                                      LEFT JOIN CLINIC_INDEX F
                                        ON A.CLINIC_LABEL = F.CLINIC_LABEL) A WHERE 1=1";
            if (!string.IsNullOrEmpty(deptId))
            {
                strSql += " AND A.DEPTID='" + deptId + "' ";
            }
            if (!string.IsNullOrEmpty(deptType))
            {
                //strSql += " AND A.USER_DEPT='" + deptId + "' ";
            }
            if (!string.IsNullOrEmpty(doctorId))
            {
                strSql += " AND A.DOCTORID='" + doctorId + "' ";
            }
            if (!string.IsNullOrEmpty(searchCode))
            {
                strSql += " AND A.DOCTORNAME LIKE '%" + searchCode + "%' ";
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                strSql += " AND TO_CHAR(A.REGDATE,'yyyy-MM-dd') >= '" + startDate + "' ";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                strSql += " AND TO_CHAR(A.REGDATE,'yyyy-MM-dd') <= '" + endDate + "' ";
            }
            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }

        /// <summary>
        ///  医生号源分时信息查询
        /// </summary>
        /// <param name="hospitalId">医院代码</param>
        /// <param name="scheduleId">排班号</param>
        /// <param name="deptId">科室代码</param>
        /// <param name="clinicUnitId">诊疗单元代码</param>
        /// <param name="doctorId">医生代码</param>
        /// <param name="regDate">出诊日期，格式：YYYY-MM-DD</param>
        /// <param name="shiftCode">班别代码</param>
        /// <returns></returns>
        public DataSet appointment_GetTimeInfo(string hospitalId, string scheduleId, string deptId, string clinicUnitId,
            string doctorId, string regDate, string shiftCode)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(hospitalId))
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码不能为空"));
                return ds;
            }
            else
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            DataTable dt2 = new DataTable();
            string strSql = @" SELECT DISTINCT '' STARTTIME,
                                    '' ENDTIME,
                                    C.APPOINTMENT_LIMITS REGTOTALCOUNT,
                                    NVL((NVL(C.APPOINTMENT_LIMITS, 0) - NVL(C.CURRENT_NO, 0)),
                                        0) REGLEAVECOUNT,
                                    '1' PERIODID 
                              FROM CLINIC_FOR_REGIST C, CLINIC_INDEX B, STAFF_DICT S, DEPT_DICT D
                              WHERE B.CLINIC_LABEL = C.CLINIC_LABEL
                               AND B.DOCTOR = S.NAME
                               AND B.CLINIC_DEPT = D.DEPT_CODE ";
            if (!string.IsNullOrEmpty(scheduleId))
            {
                //strSql += " AND A.DEPTID='" + deptId + "' ";
            }
            if (!string.IsNullOrEmpty(deptId))
            {
                strSql += " AND B.CLINIC_DEPT='" + deptId + "' ";
            }
            if (!string.IsNullOrEmpty(clinicUnitId))
            {
                strSql += "AND B.SERIAL_NO ='" + clinicUnitId + "' ";
            }
            if (!string.IsNullOrEmpty(doctorId))
            {
                strSql += " AND S.STAFF_ID='" + doctorId + "' ";
            }
            if (!string.IsNullOrEmpty(regDate))
            {
                strSql += "  AND TO_CHAR(C.CLINIC_DATE,'yyyy-MM-dd')='" + regDate + "' ";
            }
            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                if (dt2 != null && dt2.Rows.Count > 0)
                {
                    //DataRow dr = new DataRow();
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        DataRow dr = dt2.Rows[i];
                        if (shiftCode == "1")
                        {
                            dr["STARTTIME"] = "08:00";
                            dr["ENDTIME"] = "12:00";
                        }
                        else if (shiftCode == "2")
                        {
                            dr["STARTTIME"] = "14:30";
                            dr["ENDTIME"] = "17:00";
                        }
                        else if (shiftCode == "3")
                        {
                            dr["STARTTIME"] = "08:00";
                            dr["ENDTIME"] = "17:00";
                        }
                        else
                        {
                            dr["STARTTIME"] = "00:00";
                            dr["ENDTIME"] = "23:59";
                        }
                    }
                }
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }


        /// <summary>
        /// 预约
        /// </summary>
        /// <param name="orderId">移动服务的订单号</param>
        /// <param name="hospitalId">医院代码</param>
        /// <param name="deptId">科室代码</param>
        /// <param name="clinicUnitId">诊疗单元代码</param>
        /// <param name="doctorId">医生代码</param>
        /// <param name="doctorLevelCode">医生职称代码</param>
        /// <param name="regDate">预约日期</param>
        /// <param name="scheduleId">排班号</param>
        /// <param name="periodId">分时号</param>
        /// <param name="shiftCode">班别代码</param>
        /// <param name="startTime">分时开始时间，格式：HH:MI</param>
        /// <param name="endTime">分时结束时间，格式：HH:MI</param>
        /// <param name="healthCardNo">患者健康卡号码</param>
        /// <param name="patientId">患者唯一ID</param>
        /// <param name="patientName">患者姓名</param>
        /// <param name="idCardNo">患者身份证号码</param>
        /// <param name="phone">联系电话</param>
        /// <param name="orderType">预约方式 11 ----支付宝,10 ----微信</param>
        /// <param name="orderTime">下订单时间，格式：YYYY-MM-DD HH24:MI:SS</param>
        /// <param name="svObjectId">服务对象id，默认为普通患者</param>
        /// <param name="fee">挂号费，单位分</param>
        /// <param name="treatfee">诊疗费，单位分</param>
        /// <param name="remark">备注</param>
        /// <returns></returns>
        public DataSet appointment_AddOrder(string orderId, string hospitalId, string deptId, string clinicUnitId, string doctorId,
            string doctorLevelCode, string regDate, string scheduleId, string periodId, string shiftCode, string startTime, string endTime,
            string healthCardNo, string patientId, string patientName, string idCardNo, string phone, string orderType, string orderTime,
            string svObjectId, string fee, string treatfee, string remark)
        {
            DataSet ds = new DataSet();

            #region 判断
            if (string.IsNullOrEmpty(hospitalId))
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码不能为空"));
                return ds;
            }
            else
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            if (string.IsNullOrEmpty(orderId))
            {
                PubConn.writeFileLog("移动服务的订单号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,移动服务的订单号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(deptId))
            {
                PubConn.writeFileLog("科室代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,科室代码不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(doctorId))
            {
                PubConn.writeFileLog("医生代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医生代码不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(regDate))
            {
                PubConn.writeFileLog("预约日期不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,预约日期不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(shiftCode))
            {
                PubConn.writeFileLog("班别代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,班别代码不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(startTime))
            {
                PubConn.writeFileLog("分时开始时间不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,分时开始时间不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(endTime))
            {
                PubConn.writeFileLog("分时结束时间不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,分时结束时间不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(healthCardNo))
            {
                PubConn.writeFileLog("健康卡号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,健康卡号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(patientId))
            {
                PubConn.writeFileLog("患者唯一ID不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,患者唯一ID不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(patientName))
            {
                PubConn.writeFileLog("姓名不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,姓名不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(orderType))
            {
                PubConn.writeFileLog("支付方式不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,支付方式不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(orderTime))
            {
                PubConn.writeFileLog("下订单时间不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,下订单时间不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(fee))
            {
                PubConn.writeFileLog("挂号费不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,挂号费不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(treatfee))
            {
                PubConn.writeFileLog("诊疗费不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,诊疗费不能为空"));
                return ds;
            }
            #endregion

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("ROOMADDRESS");//诊室位置
            dt2.Columns.Add("OPPATNO");//病历号
            dt2.Columns.Add("QUEUENO");//排队号
            dt2.Columns.Add("CLINICSEQ");//就诊流水号
            dt2.Columns.Add("VALIDTIME");//锁号有效截至时间，格式：YYYY-MM-DD HH24:MI:SS
            string strSql = null;
            int count = 0;
            try
            {
                //获取患者基本信息
                strSql = @"SELECT * FROM PAT_MASTER_INDEX A WHERE A.PATIENT_ID='" + patientId + "'";
                DataTable dtPatInfo = PubConn.Query(strSql, strHISConn).Tables[0];
                if (dtPatInfo == null || dtPatInfo.Rows.Count == 0)
                {
                    PubConn.writeFileLog("不存在该患者信息！");
                    ds.Tables.Add(GetStatus("-1", "查询失败,不存在该患者信息！"));
                    return ds;
                }
                // --挂号方式
                string operID = null;
                string oper = null;
                if (orderType == "11")
                {
                    //支付宝
                    oper = "支付宝";
                    operID = "8001";
                }
                else if (orderType == "10")
                {
                    //微信
                    oper = "微信";
                    operID = "8000";
                }
                else
                {
                    //移动
                    oper = "移动";
                    operID = "8002";
                }
                //获取挂号单元信息（CLINIC_LABEL）、号类信息（CLINIC_TYPR）
                strSql = @"SELECT * FROM CLINIC_INDEX A WHERE A.CLINIC_DEPT='" + clinicUnitId + "'";
                DataTable dtClinicIndex = PubConn.Query(strSql, strHISConn).Tables[0];
                if (dtClinicIndex == null || dtClinicIndex.Rows.Count == 0)
                {
                    PubConn.writeFileLog("不存在该挂号单元！");
                    ds.Tables.Add(GetStatus("-1", "查询失败,不存在该挂号单元！"));
                    return ds;
                }
                //获取医生姓名
                string doctorName = PubConn.GetSingle(@" SELECT A.USER_NAME FROM USERS A WHERE A.USER_ID='" + doctorId + "'",
                    strHISConn).ToString();
                //患者当天的就诊序号，使用his的序列
                int newVisitNo = Convert.ToInt32(
                    PubConn.GetSingle(@" SELECT VISIT_NO.NEXTVAL FROM DUAL",
                    strHISConn).ToString());
                //time_desc
                string timeDesc = PubConn.GetSingle(@"
                    SELECT A.TIME_INTERVAL_NAME FROM TIME_INTERVAL_DICT A WHERE A.TIME_INTERVAL_CODE='" + shiftCode + "'",
                    strHISConn).ToString();
                //获取预约排队号
                string newSerialNo = PubConn.GetSingle(@" SELECT * FROM CLINIC_MASTER_APPOINT A 
                    WHERE TO_CHAR(A.VISIT_DATE,'YYYY-MM-DD')=TO_CHAR('" + regDate
                    + "','YYYY-MM-DD') AND A.CLINIC_LABEL='" + dtClinicIndex.Rows[0]["CLINIC_LABEL"].ToString() +
                    "' AND A.VISIT_TIME_DESC='" + timeDesc + "'",
                    strHISConn).ToString();
                //插入clinic_master_appoint数据
                strSql = @" INSERT INTO CLINIC_MASTER_APPOINT
                                  (VISIT_DATE,
                                   VISIT_NO,
                                   CLINIC_LABEL,
                                   VISIT_TIME_DESC,
                                   PATIENT_ID,
                                   NAME,
                                   NAME_PHONETIC,
                                   SEX,
                                   AGE,
                                   IDENTITY,
                                   CHARGE_TYPE,
                                   CLINIC_TYPE,
                                   FIRST_VISIT_INDICATOR,
                                   VISIT_DEPT,
                                   DOCTOR,
                                   MR_PROVIDED_INDICATOR,
                                   REGISTERING_DATE,
                                   REGIST_FEE,
                                   CLINIC_FEE,
                                   OTHER_FEE,
                                   CLINIC_CHARGE,
                                   OPERATOR,
                                   PAY_WAY,
                                   RCPT_NO --收据号（将此作为微信端的订单号）--SERIAL_NO 
                                   )
                                VALUES
                                  (TO_DATE('" + regDate + "', 'YYYY-MM-DD'),'" +
                                             newVisitNo + "','" +
                                             dtClinicIndex.Rows[0]["CLINIC_LABEL"].ToString() + "','" +
                                             timeDesc + "','" +
                                             healthCardNo + "','" +
                                             patientName + "','" +
                                             "" + "','" +
                                             dtPatInfo.Rows[0]["SEX"].ToString() + "','" +
                                             (DateTime.Now.Year - Convert.ToDateTime(dtPatInfo.Rows[0]["DATE_OF_BIRTH"]).Year) + "','" +
                                             "地方" + "','" + //IDENTITY
                                             dtPatInfo.Rows[0]["CHARGE_TYPE"].ToString() + "','" +
                                             //dtClinicIndex.Rows[0]["CLINIC_TYPE"].ToString() + "','" + 
                                            "1" + "','" +
                                             0 + "','" +
                                             clinicUnitId + "','" +
                                             doctorName + "','" +
                                             0 + "'," +
                                             "TO_DATE('" + DateTime.Now.ToShortDateString() + "', 'YYYY-MM-DD')" + ",'" +
                                             fee + "','" +
                                             treatfee + "','" +
                                             0 + "','" +
                                             (Convert.ToInt32(fee) + Convert.ToInt32(treatfee)) + "','" +
                                             operID + "','" +
                                             oper + "','" +
                                             orderId + "')";
                //int result = PubConn.ExecuteSql(strSql, strHISConn);
                if (PubConn.ExecuteSql(strSql, strHISConn) != 1)
                {
                    PubConn.writeFileLog("预约失败！");
                    ds.Tables.Add(GetStatus("-1", "查询失败,预约失败！"));
                    return ds;
                }
                //更新号源
                strSql = @"UPDATE CLINIC_FOR_REGIST C
                                   SET C.CURRENT_NO = C.CURRENT_NO + 1
                                 WHERE C.CLINIC_DATE = TO_DATE('" + regDate +
                                  "', 'YYYY-MM-DD') AND C.CLINIC_LABEL IN (SELECT CLINIC_LABEL FROM CLINIC_INDEX WHERE CLINIC_DEPT = '"
                                  + clinicUnitId + "')";
                if (PubConn.ExecuteSql(strSql, strHISConn) != 1)
                {
                    PubConn.writeFileLog("更新号源失败！");
                    ds.Tables.Add(GetStatus("-1", "查询失败,更新号源失败！"));
                    return ds;
                }
                DataRow drNew = dt2.NewRow();
                drNew[0] = orderId;
                drNew[1] = patientId;
                drNew[2] = timeDesc;
                drNew[3] = newVisitNo;
                drNew[4] = "";


                dt2.Rows.Add(drNew);
                ds.Tables.Add(GetStatus("0", "预约成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "预约失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }


        /// <summary>
        /// 预约支付
        /// </summary>
        /// <param name="hospitalId">医院代码</param>
        /// <param name="orderId">移动订单号</param>
        /// <param name="tradeNo">第三方支付的交易流水号</param>
        /// <param name="healthCardNo">用户健康卡号码</param>
        /// <param name="patientId">患者唯一ID</param>
        /// <param name="bookingNo">HIS系统生成的预约订单号</param>
        /// <param name="svObjectId">服务对象id，默认为普通患者</param>
        /// <param name="medicareSettleLogId">医保预结算参数(在线医保为json格式，格式如样例，目前仅支持线上医保结算)</param>
        /// <param name="operatorId">操作员工号</param>
        /// <param name="machineId">设备代码（针对自助设备）</param>
        /// <param name="payAmout">支付金额(单位“分”)</param>
        /// <param name="recPayAmout">统筹金额：分</param>
        /// <param name="totalPayAmout">总金额：分</param>
        /// <param name="payMode">支付方式具体编码定义见2.7支付方式</param>
        /// <param name="payTime">交易时间，格式：YYYY-MM-DD HI24:MI:SS</param>
        /// <returns></returns>
        public DataSet appointment_Pay(string hospitalId, string orderId, string tradeNo, string healthCardNo, string patientId,
            string bookingNo, string svObjectId, string medicareSettleLogId, string operatorId, string machineId, string payAmout,
            string recPayAmout, string totalPayAmout, string payMode, string payTime)
        {
            DataSet ds = new DataSet();

            #region 验证
            if (string.IsNullOrEmpty(hospitalId))
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码不能为空"));
                return ds;
            }
            else
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            if (string.IsNullOrEmpty(orderId))
            {
                PubConn.writeFileLog("移动订单号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,移动订单号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(tradeNo))
            {
                PubConn.writeFileLog("第三方支付的交易流水号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,第三方支付的交易流水号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(healthCardNo))
            {
                PubConn.writeFileLog("用户健康卡号码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,用户健康卡号码不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(patientId))
            {
                PubConn.writeFileLog("患者唯一ID不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,患者唯一ID不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(bookingNo))
            {
                PubConn.writeFileLog("HIS预约订单号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,HIS预约订单号不能为空"));
                return ds;
            }

            if (string.IsNullOrEmpty(operatorId))
            {
                PubConn.writeFileLog("操作员工号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,操作员工号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(payAmout))
            {
                PubConn.writeFileLog("支付金额不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,支付金额不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(payMode))
            {
                PubConn.writeFileLog("支付方式不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,支付方式不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(payTime))
            {
                PubConn.writeFileLog("支付方式不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,支付方式不能为空"));
                return ds;
            }
            #endregion

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("ROOMADDRESS");
            dt2.Columns.Add("OPPATNO");
            dt2.Columns.Add("QUEUENO");
            dt2.Columns.Add("CLINICSEQ");
            string strSql = null;

            try
            {
                //获取预约记录
                DataTable dtAppoint = PubConn.Query(@"SELECT A.*
                                                          FROM CLINIC_MASTER_APPOINT A
                                                         WHERE A.RCPT_NO ='" + orderId + "'", strHISConn).Tables[0];
                if (dtAppoint == null || dtAppoint.Rows.Count == 0)
                {
                    PubConn.writeFileLog("预约信息不存在");
                    ds.Tables.Add(GetStatus("-1", "查询失败,预约信息不存在"));
                    return ds;
                }
                //string visitDate = dtAppoint.Rows[0]["VISIT_DATE"].ToString();
                //string clinicLable = dtAppoint.Rows[0]["CLINIC_LABEL"].ToString();

                //写入移动交易记录表   BASEINFO.TRADE_RECORD没有这张表，改为TRADE_RECORD 插入失败
                strSql = @"INSERT INTO OUTPADM.TRADE_RECORD
                            (TRADE_NO, TRADE_DATE, RCPT_NO, ORDER_ID, PAY_MODE, COSTS)
                          VALUES
                            ('" + tradeNo +
                               "', TO_DATE('" + payTime + "', 'YYYY-MM-DD HH24:MI:SS'),'" +
                               orderId + "','" +
                             orderId + "','" +
                             payMode + "'," +
                             "TO_NUMBER('" + payAmout + "') / 100)";
                if (PubConn.ExecuteSql(strSql, strHISConn) != 1)
                {
                    PubConn.writeFileLog("写入移动交易记录表失败");
                    ds.Tables.Add(GetStatus("-1", "查询失败,写入移动交易记录表失败"));
                    return ds;
                }
                //获取一个号别下的最大序号
                int maxSerialNo = Convert.ToInt32(PubConn.GetSingle(@"SELECT NVL(MAX(SERIAL_NO), 0)
                                                            FROM CLINIC_MASTER
                                                           WHERE CLINIC_LABEL = '" + dtAppoint.Rows[0]["CLINIC_LABEL"].ToString() +
                                                             "' AND TO_CHAR(VISIT_DATE,'YYYY-MM-DD') = TO_CHAR('" + dtAppoint.Rows[0]["VISIT_DATE"].ToString() +
                                                             "','YYYY-MM-DD')", strHISConn)) + 1;
                //插入就诊表clinic_master
                strSql = @"INSERT INTO CLINIC_MASTER
                                      (VISIT_DATE,
                                       VISIT_NO,
                                       CLINIC_LABEL,
                                       VISIT_TIME_DESC,
                                       SERIAL_NO,
                                       PATIENT_ID,
                                       NAME,
                                       NAME_PHONETIC,
                                       SEX,
                                       AGE,
                                       IDENTITY,
                                       CHARGE_TYPE,
                                       CLINIC_TYPE,
                                       FIRST_VISIT_INDICATOR,
                                       VISIT_DEPT,
                                       DOCTOR,
                                       MR_PROVIDED_INDICATOR,
                                       REGISTRATION_STATUS,
                                       REGISTERING_DATE,
                                       REGIST_FEE,
                                       CLINIC_FEE,
                                       OTHER_FEE,
                                       CLINIC_CHARGE,
                                       OPERATOR,
                                      -- PAY_WAY,
                                       RCPT_NO)
                                      SELECT A.VISIT_DATE,
                                             A.VISIT_NO,
                                             A.CLINIC_LABEL,
                                             A.VISIT_TIME_DESC,
                                             '" + maxSerialNo +
                                               @"'SERIAL_NO,
                                             A.PATIENT_ID,
                                             A.NAME,
                                             A.NAME_PHONETIC,
                                             A.SEX,
                                             A.AGE,
                                             A.IDENTITY,
                                             A.CHARGE_TYPE,
                                             A.CLINIC_TYPE,
                                             A.FIRST_VISIT_INDICATOR,
                                             A.VISIT_DEPT,
                                             A.DOCTOR,
                                             A.MR_PROVIDED_INDICATOR,
                                             '1',
                                             A.REGISTERING_DATE,
                                             A.REGIST_FEE,
                                             A.CLINIC_FEE,
                                             A.OTHER_FEE,
                                             A.CLINIC_CHARGE,
                                             A.OPERATOR,
                                             --A.PAY_WAY,
                                             '" + orderId +
                                                @"' ORDERID
                                        FROM CLINIC_MASTER_APPOINT A
                                       WHERE A.RCPT_NO = '" + orderId + "'";
                if (PubConn.ExecuteSql(strSql, strHISConn) != 1)
                {
                    PubConn.writeFileLog("预约支付失败！");
                    ds.Tables.Add(GetStatus("-1", "预约支付失败!"));
                    ds.Tables.Add(dt2);
                }
                //就诊流水号
                string clinicSeq = dtAppoint.Rows[0]["VISIT_DATE"].ToString() + dtAppoint.Rows[0]["VISIT_NO"].ToString() + maxSerialNo;
                // -- 记录交易流水号(预约挂号表中的trans_no)、就诊流水号clinicSeq、费用信息和付费状态
                strSql = @"UPDATE CLINIC_MASTER_APPOINT A
                                   SET A.TRANS_NO            = '" + tradeNo +
                                       "', A.CLINIC_SEQ          = '" + clinicSeq +
                                       "', A.INSUR_FEE           = '" + payAmout +
                                       "', A.REGISTRATION_STATUS = '1' WHERE A.RCPT_NO = '" + orderId + "'";
                if (PubConn.ExecuteSql(strSql, strHISConn) != 1)
                {
                    PubConn.writeFileLog("记录交易流水号等信息失败！");
                    ds.Tables.Add(GetStatus("-1", "记录交易流水号等信息失败！"));
                    ds.Tables.Add(dt2);
                }

                DataRow drNew = dt2.NewRow();
                drNew[0] = "";
                drNew[1] = patientId;
                drNew[2] = maxSerialNo;
                drNew[3] = clinicSeq;
                dt2.Rows.Add(drNew);

                ds.Tables.Add(GetStatus("0", "取消预约成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "取消预约失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }


        /// <summary>
        /// 取消预约
        /// </summary>
        /// <param name="orderId">移动订单号</param>
        /// <param name="healthCardNo">用户健康卡号码</param>
        /// <param name="patientId">患者唯一ID</param>
        /// <param name="scheduleId">排班号</param>
        /// <param name="periodId">分时号</param>
        /// <param name="bookingNo">HIS预约订单号</param>
        /// <param name="cancelTime">取消时间，格式：YYYY-MM-DD HH24:MI:SS</param>
        /// <param name="cancelReason">取消原因</param>
        /// <returns></returns>
        public DataSet appointment_CancelOrder(string orderId, string healthCardNo, string patientId, string scheduleId,
            string periodId, string bookingNo, string cancelTime, string cancelReason)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(orderId))
            {
                PubConn.writeFileLog("移动订单号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,移动订单号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(healthCardNo))
            {
                PubConn.writeFileLog("用户健康卡号码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,用户健康卡号码不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(patientId))
            {
                PubConn.writeFileLog("患者唯一ID不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,患者唯一ID不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(bookingNo))
            {
                PubConn.writeFileLog("HIS预约订单号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,HIS预约订单号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(cancelTime))
            {
                PubConn.writeFileLog("取消时间不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,取消时间不能为空"));
                return ds;
            }

            DataTable dt2 = new DataTable();
            string strSql = @"";

            try
            {
                // 获取订单信息 
                strSql = @"SELECT A.CLINIC_LABEL, A.VISIT_TIME_DESC, A.VISIT_DATE
                              FROM CLINIC_MASTER_APPOINT A
                             WHERE A.RCPT_NO = '" + orderId + "'";
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                if (dt2 == null || dt2.Rows.Count == 0)
                {
                    PubConn.writeFileLog("没有订单:" + orderId + "的信息");
                    ds.Tables.Add(GetStatus("-1", "查询失败,没有订单:" + orderId + "的信息"));
                    return ds;
                }
                //更新号源
                strSql = @" UPDATE OUTPADM.CLINIC_FOR_REGIST T
                                   SET T.CURRENT_NO = T.CURRENT_NO - 1
                                 WHERE TO_CHAR(T.CLINIC_DATE,'YYYY-MM-DD') = TO_CHAR('" + dt2.Rows[0]["VISIT_DATE"].ToString() +
                                   "','YYYY-MM-DD') AND T.CLINIC_LABEL ='" + dt2.Rows[0]["CLINIC_LABEL"].ToString() +
                                   "' AND T.TIME_DESC = '" + dt2.Rows[0]["VISIT_TIME_DESC"].ToString() + "'";
                if (PubConn.ExecuteSql(strSql, strHISConn) != 1)
                {
                    PubConn.writeFileLog("更新号源失败！");
                    ds.Tables.Add(GetStatus("-1", "查询失败,更新号源失败！"));
                    return ds;
                }
                //处理完后删除未支付的订单数据
                strSql = @"DELETE FROM CLINIC_MASTER_APPOINT A
                                     WHERE TO_CHAR(A.VISIT_DATE,'YYYY-MM-DD') = TO_CHAR('" + dt2.Rows[0]["VISIT_DATE"].ToString() +
                                       "','YYYY-MM-DD') AND A.PATIENT_ID = '" + patientId +
                                       "' AND A.RCPT_NO =  '" + orderId + "'";
                if (PubConn.ExecuteSql(strSql, strHISConn) != 1)
                {
                    PubConn.writeFileLog("删除未支付的订单失败！");
                    ds.Tables.Add(GetStatus("-1", "查询失败,删除未支付的订单失败！"));
                    return ds;
                }



                ds.Tables.Add(GetStatus("0", "取消预约成功"));
                //ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "取消预约失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }

        /// <summary>
        /// 挂号
        /// </summary>
        /// <param name="lockId">号源锁定ID</param>
        /// <param name="infoSeq">HIS锁号ID</param>
        /// <param name="orderId">移动订单号</param>
        /// <param name="hospitalId">医院代码</param>
        /// <param name="healthCardNo">患者健康卡号码</param>
        /// <param name="patientId">患者唯一ID</param>
        /// <param name="orderType">挂号方式 11 ----支付宝，10 ----微信</param>
        /// <param name="orderTime">下订单时间，格式：YYYY-MM-DD HH24:MI:SS</param>
        /// <param name="svObjectId">服务对象id，默认为普通患者</param>
        /// <param name="medicareSettleLogId">医保预结算参数(在线医保为json格式，格式如样例，目前仅支持线上医保结算)</param>
        /// <param name="operatorId">操作员工号</param>
        /// <param name="machineId">设备代码（针对自助设备）</param>
        /// <param name="payAmout">支付金额(单位“分”)</param>
        /// <param name="recPayAmout">统筹金额：分</param>
        /// <param name="totalPayAmout">总金额：分</param>
        /// <param name="payMode">支付方式：具体编码定义见2.7支付方式</param>
        /// <param name="tradeNo">交易流水号</param>
        /// <returns></returns>
        public DataSet register_Pay(string lockId, string infoSeq, string orderId, string hospitalId, string healthCardNo, string patientId,
            string orderType, string orderTime, string svObjectId, string medicareSettleLogId, string operatorId, string machineId,
            string payAmout, string recPayAmout, string totalPayAmout, string payMode, string tradeNo)
        {
            DataSet ds = new DataSet();

            #region 判断
            if (string.IsNullOrEmpty(lockId))
            {
                PubConn.writeFileLog("号源锁定ID不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,号源锁定ID不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(hospitalId))
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码不能为空"));
                return ds;
            }
            else
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            if (string.IsNullOrEmpty(orderId))
            {
                PubConn.writeFileLog("移动服务的订单号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,移动服务的订单号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(healthCardNo))
            {
                PubConn.writeFileLog("健康卡号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,健康卡号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(patientId))
            {
                PubConn.writeFileLog("患者唯一ID不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,患者唯一ID不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(orderType))
            {
                PubConn.writeFileLog("支付方式不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,支付方式不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(orderTime))
            {
                PubConn.writeFileLog("下订单时间不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,下订单时间不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(operatorId))
            {
                PubConn.writeFileLog("操作员工号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,操作员工号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(payAmout))
            {
                PubConn.writeFileLog("支付金额不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,支付金额不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(totalPayAmout))
            {
                PubConn.writeFileLog("总金额不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,总金额不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(payMode))
            {
                PubConn.writeFileLog("支付方式不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,支付方式不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(tradeNo))
            {
                PubConn.writeFileLog("交易流水号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,交易流水号不能为空"));
                return ds;
            }
            #endregion

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("ROOMADDRESS");//诊室位置
            dt2.Columns.Add("OPPATNO");//病历号
            dt2.Columns.Add("QUEUENO");//排队号
            dt2.Columns.Add("CLINICSEQ");//就诊流水号
            dt2.Columns.Add("CLINICTIME");//预计就诊时间，格式：YYYY-MM-DD HH24:MI:SS

            string strSql = null;
            int count = 0;
            try
            {
                // --挂号方式
                string operID = null;
                string oper = null;
                if (orderType == "11")
                {
                    //支付宝
                    oper = "支付宝";
                    operID = "8001";
                }
                else if (orderType == "10")
                {
                    //微信
                    oper = "微信";
                    operID = "8000";
                }
                else
                {
                    //移动
                    oper = "移动";
                    operID = "8002";
                }
                //获取预约记录
                DataTable dtAppoint = PubConn.Query(@"SELECT A.*
                                                          FROM CLINIC_MASTER_APPOINT A
                                                         WHERE A.RCPT_NO ='" + orderId + "'", strHISConn).Tables[0];
                if (dtAppoint == null || dtAppoint.Rows.Count == 0)
                {
                    PubConn.writeFileLog("预约信息不存在");
                    ds.Tables.Add(GetStatus("-1", "查询失败,预约信息不存在"));
                    return ds;
                }
                //患者当天的就诊序号，使用his的序列
                int maxVisitNo = Convert.ToInt32(
                    PubConn.GetSingle(@"  SELECT MAX(VISIT_NO)
                                              FROM CLINIC_MASTER
                                             WHERE TO_CHAR(VISIT_DATE,'YYYY-MM-DD') =TO_CHAR('" +
                                              dtAppoint.Rows[0]["VISIT_DATE"].ToString() + "''YYYY-MM-DD')",
                    strHISConn).ToString());
                //获取排队号
                string maxSerialNo = PubConn.GetSingle(@" SELECT * FROM CLINIC_MASTER A 
                                            WHERE TO_CHAR(A.VISIT_DATE,'YYYY-MM-DD')=TO_CHAR('" + dtAppoint.Rows[0]["VISIT_DATE"].ToString()
                                            + "','YYYY-MM-DD') AND A.CLINIC_LABEL='" + dtAppoint.Rows[0]["CLINIC_LABEL"].ToString() +
                                            "' AND A.VISIT_TIME_DESC='" + dtAppoint.Rows[0]["VISIT_TIME_DESC"].ToString() + "'",
                                            strHISConn).ToString();
                //插入clinic_master_appoint数据
                strSql = @" INSERT INTO CLINIC_MASTER
                                      (VISIT_DATE,
                                       VISIT_NO,
                                       CLINIC_LABEL,
                                       VISIT_TIME_DESC,
                                       SERIAL_NO,
                                       PATIENT_ID,
                                       NAME,
                                       NAME_PHONETIC,
                                       SEX,
                                       AGE,
                                       IDENTITY,
                                       CHARGE_TYPE,
                                       CLINIC_TYPE,
                                       FIRST_VISIT_INDICATOR,
                                       VISIT_DEPT,
                                       DOCTOR,
                                       MR_PROVIDED_INDICATOR,
                                       REGISTRATION_STATUS,
                                       REGISTERING_DATE,
                                       REGIST_FEE,
                                       CLINIC_FEE,
                                       OTHER_FEE,
                                       CLINIC_CHARGE,
                                       OPERATOR,
                                       --PAY_WAY,
                                       RCPT_NO)
                                      SELECT A.VISIT_DATE,
                                             A.VISIT_NO,
                                             A.CLINIC_LABEL,
                                             A.VISIT_TIME_DESC,
                                             '" + maxSerialNo +
                                               @"' SERIAL_NO,
                                             A.PATIENT_ID,
                                             A.NAME,
                                             A.NAME_PHONETIC,
                                             A.SEX,
                                             A.AGE,
                                             A.IDENTITY,
                                             A.CHARGE_TYPE,
                                             A.CLINIC_TYPE,
                                             A.FIRST_VISIT_INDICATOR,
                                             A.VISIT_DEPT,
                                             A.DOCTOR,
                                             A.MR_PROVIDED_INDICATOR,
                                             '1',
                                             A.REGISTERING_DATE,
                                             A.REGIST_FEE,
                                             A.CLINIC_FEE,
                                             A.OTHER_FEE,
                                             A.CLINIC_CHARGE,
                                             '" + oper +
                                               @"'OPERNAME,
                                              
                                              '" + orderId +
                                               @"'ORDERID
                                        FROM CLINIC_MASTER_APPOINT A
                                       WHERE A.RCPT_NO = '" + orderId + "'";
                //int result = PubConn.ExecuteSql(strSql, strHISConn);
                if (PubConn.ExecuteSql(strSql, strHISConn) != 1)
                {
                    PubConn.writeFileLog("挂号失败！");
                    ds.Tables.Add(GetStatus("-1", "查询失败,挂号失败！"));
                    return ds;
                }
                //就诊流水号
                string clinicSeq = dtAppoint.Rows[0]["VISIT_DATE"].ToString() + dtAppoint.Rows[0]["VISIT_NO"].ToString() + maxSerialNo;
                //记录就诊流水号 A.CLINIC_SEQ = '" + clinicSeq +"',
                strSql = @" UPDATE CLINIC_MASTER_APPOINT A
                                   SET A.OPERATOR= '" + operID +
                                     "', A.PAY_WAY= '" + oper +
                                     "', A.REGISTRATION_STATUS = '1' WHERE A.RCPT_NO = '" + orderId + "';";
                if (PubConn.ExecuteSql(strSql, strHISConn) != 1)
                {
                    PubConn.writeFileLog("记录就诊流水号失败！");
                    ds.Tables.Add(GetStatus("-1", "查询失败,记录就诊流水号失败！"));
                    return ds;
                }
                DataRow drNew = dt2.NewRow();
                drNew[0] = orderId;
                drNew[1] = patientId;
                drNew[2] = maxSerialNo;
                drNew[3] = clinicSeq;
                drNew[4] = dtAppoint.Rows[0]["VISIT_DATE"].ToString();


                dt2.Rows.Add(drNew);
                ds.Tables.Add(GetStatus("0", "挂号成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "挂号失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }


        /// <summary>
        /// 退号
        /// </summary>
        /// <param name="healthCardNo">用户健康卡号码</param>
        /// <param name="patientId">患者唯一ID</param>
        /// <param name="orderId">移动订单号</param>
        /// <param name="scheduleId">排班号</param>
        /// <param name="periodId">分时号</param>
        /// <param name="clinicSeq">就诊流水号</param>
        /// <param name="tradeNo">第三方支付平台交易流水号</param>
        /// <param name="medicareSettleLogId">医保预结算参数</param>
        /// <param name="operatorId">操作员工号</param>
        /// <param name="machineId">设备代码（针对自助设备）</param>
        /// <param name="refundFee">退费金额(单位“分”)</param>
        /// <param name="refundTime">退费时间，格式：YYYY-MM-DD HI24:MI:SS</param>
        /// <param name="refundReason">退费原因</param>
        /// <returns></returns>
        public DataSet appointment_ReturnPay(string healthCardNo, string patientId, string orderId, string scheduleId, string periodId,
            string clinicSeq, string tradeNo, string medicareSettleLogId, string operatorId, string machineId, string refundFee,
            string refundTime, string refundReason)
        {
            DataSet ds = new DataSet();

            #region 判断
            if (string.IsNullOrEmpty(orderId))
            {
                PubConn.writeFileLog("移动服务的订单号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,移动服务的订单号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(healthCardNo))
            {
                PubConn.writeFileLog("健康卡号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,健康卡号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(patientId))
            {
                PubConn.writeFileLog("患者唯一ID不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,患者唯一ID不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(clinicSeq))
            {
                PubConn.writeFileLog("就诊流水号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,就诊流水号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(tradeNo))
            {
                PubConn.writeFileLog("交易流水号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,交易流水号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(operatorId))
            {
                PubConn.writeFileLog("操作员工号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,操作员工号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(refundFee))
            {
                PubConn.writeFileLog("退费金额不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,退费金额不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(refundTime))
            {
                PubConn.writeFileLog("退费时间不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,退费时间不能为空"));
                return ds;
            }
            #endregion

            DataTable dt2 = new DataTable();

            string strSql = null;
            try
            {
                //获取支付方式
                string payMode = PubConn.GetSingle(@" SELECT PAY_MODE
                                                              FROM TRADE_RECORD
                                                             WHERE ORDER_ID = '" + orderId + "'", strHISConn).ToString();
                //存入交易记录表
                //首先判断是否已经退号
                int count = PubConn.GetCount(@" SELECT COUNT(*)
                                                  FROM CLINIC_MASTER_APPOINT A
                                                 WHERE A.RCPT_NO = '" + orderId +
                                                   "' AND A.PATIENT_ID = '" + patientId + "' AND A.RETURNED_DATE IS NOT NULL;",
                                                   strHISConn);
                if (count > 0)
                {
                    PubConn.writeFileLog("该号已退号");
                    ds.Tables.Add(GetStatus("-1", "查询失败,该号已退号"));
                    return ds;
                }
                strSql = @" INSERT INTO BASEINFO.TRADE_RECORD
                                      (TRADE_NO, TRADE_DATE, RCPT_NO, ORDER_ID, PAY_MODE, COSTS)
                                    VALUES
                                      ('" + tradeNo +
                                       "',TO_DATE('" + refundTime +
                                       "', 'YYYY-MM-DD HH24:MI:SS'),'" + orderId +
                                       "','" + orderId +
                                       "','" + payMode +
                                        "',-TO_NUMBER('" + refundFee + "') / 100)";
                if (PubConn.ExecuteSql(strSql, strHISConn) != 1)
                {
                    PubConn.writeFileLog("插入交易记录失败！");
                    ds.Tables.Add(GetStatus("-1", "查询失败,插入交易记录失败！"));
                    return ds;
                }
                //获取预约记录
                DataTable dtAppoint = PubConn.Query(@"SELECT A.*
                                                          FROM CLINIC_MASTER_APPOINT A
                                                         WHERE A.RCPT_NO ='" + orderId + "'", strHISConn).Tables[0];
                if (dtAppoint == null || dtAppoint.Rows.Count == 0)
                {
                    PubConn.writeFileLog("预约信息不存在");
                    ds.Tables.Add(GetStatus("-1", "查询失败,预约信息不存在"));
                    return ds;
                }
                //回收号源
                strSql = @" UPDATE OUTPADM.CLINIC_FOR_REGIST T
                                     SET T.CURRENT_NO = T.CURRENT_NO - 1 
                                   WHERE TO_CHAR(T.CLINIC_DATE,'YYYY-MM-DD') = TO_CHAR('" + dtAppoint.Rows[0]["VISIT_DATE"].ToString() +
                                     "','YYYY-MM-DD') AND T.CLINIC_LABEL = '" + dtAppoint.Rows[0]["CLINIC_LABEL"].ToString() +
                                     "' AND T.TIME_DESC = '" + dtAppoint.Rows[0]["VISIT_TIME_DESC"].ToString() + "'";
                if (PubConn.ExecuteSql(strSql, strHISConn) != 1)
                {
                    PubConn.writeFileLog("回收号源失败！");
                    ds.Tables.Add(GetStatus("-1", "查询失败,回收号源失败！"));
                    return ds;
                }

                //插入clinic_master_appoint数据
                strSql = @" UPDATE CLINIC_MASTER T
                                     SET T.RETURNED_DATE = TO_DATE('" + refundTime +
                                         @"','YYYY-MM-DD HH24:MI:SS'),T.REGISTRATION_STATUS = '3',
                                         T.RETURNED_OPERATOR   = T.PAY_WAY
                                   WHERE T.PATIENT_ID = '" + patientId +
                                     "' AND T.RCPT_NO = '" + orderId +
                                     "' AND T.RETURNED_DATE IS NULL;";
                //int result = PubConn.ExecuteSql(strSql, strHISConn);
                if (PubConn.ExecuteSql(strSql, strHISConn) != 1)
                {
                    PubConn.writeFileLog("退挂号失败！");
                    ds.Tables.Add(GetStatus("-1", "退挂号失败！"));
                    return ds;
                }
                ds.Tables.Add(GetStatus("0", "退挂号成功"));
                //ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "退挂号失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }



        /// <summary>
        /// 获取当天的门诊出诊科室列表信息。
        /// </summary>
        /// <param name="hospitalId">医疗机构代码</param>
        /// <param name="deptId">科室编码</param>
        /// <param name="deptType">科室类型   暂时没有专家、专科分类（1专科2 专家）</param>
        /// <returns></returns>
        public DataSet register_GetDeptInfo(string hospitalId, string deptId, string deptType)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(hospitalId))
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码不能为空"));
                return ds;
            }
            else
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            DataTable dt2 = new DataTable();
            string strSql = @"SELECT DISTINCT *
                              FROM (
        
                                    SELECT C.CLINIC_DEPT DEPTID,
                                            A.CLINIC_LABEL DEPTNAME,
                                            (CASE
                                              WHEN B.PARENT_CODE IS NULL THEN
                                               '-1'
                                              ELSE
                                               B.PARENT_CODE
                                            END) PARENTID,
                                            B.DEPT_DESC DESCRIPTION
                                      FROM CLINIC_FOR_REGIST A, CLINIC_INDEX C, DEPT_DICT B
                                     WHERE A.CLINIC_LABEL = C.CLINIC_LABEL
                                       AND C.CLINIC_DEPT = B.DEPT_CODE
                                       AND A.CLINIC_DATE >= TRUNC(SYSDATE)
                                       AND A.CLINIC_DATE < TRUNC(SYSDATE) + 1
                                     ORDER BY A.CLINIC_LABEL ASC) D WHERE 1=1 ";
            if (!string.IsNullOrEmpty(deptId))
            {
                strSql += " AND D.DEPTID='" + deptId + "' ";
            }
            if (!string.IsNullOrEmpty(deptType))
            {
                //strSql += " AND A.USER_DEPT='" + deptType + "' ";
            }

            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }

        /// <summary>
        /// 当天医生出诊信息查询
        /// </summary>
        /// <param name="hospitalId">医院代码</param>
        /// <param name="deptId">科室代码</param>
        /// <param name="deptType">科室分类：例如：1专科2 专家</param>
        /// <param name="doctorId">医生代码</param>
        /// <param name="searchCode">搜索关键字，如医生姓名，拼音码等</param>
        /// <returns></returns>
        public DataSet register_GetScheduleInfo(string hospitalId, string deptId, string deptType, string doctorId, string searchCode)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(hospitalId))
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码不能为空"));
                return ds;
            }
            else
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            DataTable dt2 = new DataTable();
            string strSql = @"SELECT DISTINCT *
                              FROM (SELECT DISTINCT D.STAFF_ID DOCTORID,
                                                    C.DEPT_NAME DOCTORNAME,
                                                    E.TITLE_CODE DOCTORLEVELCODE,
                                                    D.TITLE DOCTORLEVEL,
                                                    D.DOCTOR_DESC DESCRIPTION,
                                                    C.DEPT_CODE DEPTID,
                                                    C.DEPT_NAME DEPTNAME,
                                                    B.CLINIC_LABEL CLINICUNITNAME,
                                                    B.CLINIC_DEPT CLINICUNITID,
                                                    A.CLINIC_DATE REGDATE,
                                                    F.TIME_INTERVAL_CODE SHIFTCODE,
                                                    A.TIME_DESC SHIFTNAME,
                                                    '' STARTTIME,
                                                    '' ENDTIME,
                                                    '0' SCHEDULEID,
                                                    '1' REGSTATUS,
                                                    A.APPOINTMENT_LIMITS REGTOTALCOUNT,
                                                    NVL((NVL(A.APPOINTMENT_LIMITS, 0) -
                                                        NVL(A.CURRENT_NO, 0)),
                                                        0) REGLEAVECOUNT,
                                                    A.REGIST_PRICE REGFEE,
                                                    '0' TREATFEE,
                                                    '1' ISTIMEREG
                                      FROM CLINIC_FOR_REGIST  A,
                                           CLINIC_INDEX       B,
                                           DEPT_DICT          C,
                                           STAFF_DICT         D,
                                           TIME_INTERVAL_DICT F,
                                           TITLE_DICT         E
                                     WHERE A.CLINIC_LABEL = B.CLINIC_LABEL
                                       AND B.CLINIC_DEPT = C.DEPT_CODE
                                       AND A.TIME_DESC = F.TIME_INTERVAL_NAME
                                       AND D.TITLE = E.TITLE_NAME
       
                                    ) G WHERE 1=1  ";
            if (!string.IsNullOrEmpty(deptId))
            {
                strSql += " AND G.DEPTID='" + deptId + "' ";
            }
            if (!string.IsNullOrEmpty(deptType))
            {
                //strSql += " AND G.USER_DEPT='" + deptType + "' ";
            }
            if (!string.IsNullOrEmpty(doctorId))
            {
                strSql += " AND G.DOCTORID='" + doctorId + "' ";
            }
            if (!string.IsNullOrEmpty(searchCode))
            {
                strSql += " AND G.DOCTORNAME LIKE '%" + doctorId + "%' ";
            }

            if (!string.IsNullOrEmpty(strSql))
            {
                strSql += " ORDER BY REGDATE ";
            }
            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }

        /// <summary>
        /// 待缴费记录查询
        /// </summary>
        /// <param name="hospitalId">医院代码</param>
        /// <param name="healthCardNo">患者健康卡号码</param>
        /// <param name="patientId">患者唯一ID</param>
        /// <param name="startDate">查询开始日期，格式：YYYY-MM-DD</param>
        /// <param name="endDate">查询结束日期格式：YYYY-MM-DD</param>
        /// <returns></returns>
        public DataSet outpatient_getPayInfo(string hospitalId, string healthCardNo, string patientId, string startDate, string endDate)
        {
            DataSet ds = new DataSet();

            if (string.IsNullOrEmpty(healthCardNo))
            {
                PubConn.writeFileLog("健康卡号码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,健康卡号码不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(patientId))
            {
                PubConn.writeFileLog("患者唯一ID不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,患者唯一ID不能为空"));
                return ds;
            }
            DataTable dt2 = new DataTable();
            string strSql = @"
                                SELECT DISTINCT CLINICTIME,
                                                CLINICSEQ,
                                                HOSPITALID,
                                                DEPTID,
                                                DEPTNAME,
                                                DOCTORID,
                                                DOCTORNAME,
                                                SUM(PAYAMOUT) * 100 AS PAYAMOUT,
                                                SETTLECODE,
                                                SETTLETYPE
                                  FROM (SELECT DISTINCT M.VISIT_DATE CLINICTIME, --就诊日期
                                                        B.VISIT_NO CLINICSEQ, --就诊流水号(日期+单号） 
                                                        '42520068101' HOSPITALID, --医院代码
                                                        D.CLINIC_DEPT DEPTID, --接诊科室代码
                                                        D.CLINIC_LABEL DEPTNAME, --接诊科室名称
                                                        (SELECT USER_ID
                                                           FROM USERS
                                                          WHERE USER_NAME = M.DOCTOR
                                                            AND ROWNUM = 1) DOCTORID, --接诊医生代码
                                                        M.DOCTOR DOCTORNAME, --接诊医生姓名
                                                        B.CHARGES PAYAMOUT, --未缴费金总额
                                                        '0' SETTLECODE, --结算类型代码
                                                        '自费' SETTLETYPE --结算类型名称
                                          FROM OUTP_BILL_ITEMS  B,
                                               PAT_MASTER_INDEX C,
                                               CLINIC_INDEX     D,
                                               CLINIC_MASTER    M
                                         WHERE M.PATIENT_ID = C.PATIENT_ID
                                           AND (M.VISIT_DATE = B.VISIT_DATE AND M.VISIT_NO = B.VISIT_NO)
                                           AND D.CLINIC_LABEL = M.CLINIC_LABEL
                                           AND D.CLINIC_DEPT = B.PERFORMED_BY";
            if (!string.IsNullOrEmpty(hospitalId))
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            if (!string.IsNullOrEmpty(healthCardNo))
            {
                strSql += " AND C.PATIENT_ID='" + healthCardNo + "' ";
            }
            if (!string.IsNullOrEmpty(patientId))
            {
                strSql += " AND C.PATIENT_ID='" + patientId + "' ";
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                strSql += " AND TO_CHAR(B.VISIT_DATE,'yyyy-MM-dd') >='" + startDate + "' ";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                strSql += " AND TO_CHAR(B.VISIT_DATE,'yyyy-MM-dd') <='" + endDate + "' ";
            }
            if (!string.IsNullOrEmpty(strSql))
            {
                //strSql += @"TO_DATE(TO_CHAR(B.VISIT_DATE, 'YYYY-MM-DD'), 'YYYY-MM-DD')) W
                strSql += @") W
                              GROUP BY CLINICTIME,
                              CLINICSEQ,
                              HOSPITALID,
                              DEPTID,
                              DEPTNAME,
                              DOCTORID,
                              DOCTORNAME,
                              SETTLECODE,
                              SETTLETYPE ";
            }
            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }

        /// <summary>
        ///获取待缴费用信息
        /// </summary>
        /// <param name="hospitalId">医院代码</param>
        /// <param name="healthCardNo">患者健康卡号码</param>
        /// <param name="patientId">患者唯一ID</param>
        /// <param name="clinicSeq">就诊流水号</param>
        /// <param name="doctorId">接诊医生工号</param>
        /// <param name="settleCode">医保参保类型代码</param>
        /// <param name="prescriptionIds">处方号，多个处方号用半角逗号隔开，如果为空则表示查询所有处方的费用信息</param>
        /// <returns></returns>
        public DataSet outpatient_GetPaybillfee(string hospitalId, string healthCardNo, string patientId, string clinicSeq,
            string doctorId, string settleCode, string prescriptionIds)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(hospitalId))
            {
                PubConn.writeFileLog("医院代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医院代码不能为空"));
                return ds;
            }
            else
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            if (string.IsNullOrEmpty(healthCardNo))
            {
                PubConn.writeFileLog("健康卡号码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,健康卡号码不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(patientId))
            {
                PubConn.writeFileLog("患者唯一ID不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,患者唯一ID不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(patientId))
            {
                PubConn.writeFileLog("患者唯一ID不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,患者唯一ID不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(clinicSeq))
            {
                PubConn.writeFileLog("就诊流水号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,就诊流水号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(doctorId))
            {
                PubConn.writeFileLog("接诊医生工号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,接诊医生工号不能为空"));
                return ds;
            }
            DataTable dt2 = new DataTable();
            string strSql = @"  SELECT DISTINCT D.PRESC_NO PRESCRIPTIONIDS,
                                                B.CHARGES PAYAMOUT,
                                                B.CHARGES TOTALPAYAMOUT,
                                                '' RECPAYAMOUT,
                                                '' REMARK,
                                                '' MEDICARESETTLELOGID,
                                                B.CLASS_ON_RCPT TYPECODE,
                                                C.FEE_CLASS_NAME TYPENAME,
                                                B.CHARGES TYPEAMOUT
                                  FROM OUTP_BILL_ITEMS B
                                  LEFT JOIN CLINIC_MASTER A
                                    ON A.VISIT_DATE = B.VISIT_DATE
                                   AND A.VISIT_NO = B.VISIT_NO
                                  LEFT JOIN OUTP_RCPT_FEE_DICT C
                                    ON B.CLASS_ON_RCPT = C.FEE_CLASS_CODE
                                  LEFT JOIN OUTP_PRESC_MASTER D
                                    ON B.RCPT_NO = D.RCPT_NO
                                  LEFT JOIN OUTP_ORDER_DESC E
                                    ON A.RCPT_NO = E.RCPT_NO ";
            if (!string.IsNullOrEmpty(healthCardNo))
            {
                strSql += " AND A.PATIENT_ID='" + healthCardNo + "' ";
            }
            if (!string.IsNullOrEmpty(patientId))
            {
                strSql += " AND A.PATIENT_ID='" + patientId + "' ";
            }
            if (!string.IsNullOrEmpty(clinicSeq))
            {
                strSql += " AND A.VISIT_NO='" + clinicSeq + "' ";
            }
            if (!string.IsNullOrEmpty(doctorId))
            {
                strSql += " AND (SELECT USER_ID FROM USERS WHERE USER_NAME=E.ORDERED_BY_DOCTOR AND ROWNUM=1 ) ='" + doctorId + "'WHERE ROWNUM<=10 ";
            }
            if (!string.IsNullOrEmpty(settleCode))
            {
                strSql += " AND B.INSUR_RCPT_CLASS='" + settleCode + "' ";
            }
            if (!string.IsNullOrEmpty(prescriptionIds))
            {
                strSql += " AND D.PRESC_NO in (" + prescriptionIds + ") ";
            }

            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }

        /// <summary>
        /// 已缴费记录查询
        /// </summary>
        /// <param name="hospitalId">医院代码</param>
        /// <param name="healthCardNo">用户健康卡号码</param>
        /// <param name="patientId">患者唯一ID</param>
        /// <param name="startDate">查询开始日期，格式：YYYY-MM-DD</param>
        /// <param name="endDate">查询结束日期格式：YYYY-MM-DD</param>
        /// <returns></returns>
        public DataSet outpatient_GetCompletedPayInfo(string hospitalId, string healthCardNo, string patientId,
            string startDate, string endDate)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(healthCardNo))
            {
                PubConn.writeFileLog("健康卡号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,健康卡号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(patientId))
            {
                PubConn.writeFileLog("患者唯一ID不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,患者唯一ID不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(startDate))
            {
                PubConn.writeFileLog("开始日期不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,开始日期不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(endDate))
            {
                PubConn.writeFileLog("结束日期不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,结束日期不能为空"));
                return ds;
            }
            DataTable dt2 = new DataTable();
            string strSql = @" SELECT A.RCPT_NO CLINICSEQ, --就诊流水号
                                       TO_CHAR(A.VISIT_DATE, 'YYYY-MM-DD HH24:MI') CLINICTIME, --就诊日期
                                       A.ORDERED_BY_DEPT DEPTID, --接诊科室代码
                                       DEPT_DICT.DEPT_NAME DEPTNAME, --接诊科室名称
                                       (SELECT USER_ID FROM USERS WHERE USER_NAME=A.ORDERED_BY_DOCTOR AND ROWNUM=1) DOCTORID, --接诊医生代码
                                       A.ORDERED_BY_DOCTOR DOCTORNAME, --接诊医生姓名
                                       SUM(B.COSTS) * 100 PAYAMOUT, --已缴费总金额，单位：分
                                       '' RECPAYAMOUT,
                                       SUM(B.COSTS) * 100 TOTALPAYAMOUT,
                                       A.RCPT_NO RECEIPTID, --收据ID
                                       TO_CHAR(A.VISIT_DATE, 'YYYY-MM-DD HH24:MI') CHARGEDATE ,--收费日期
                                       '0' ALLOWREFUND,
                                       '' REMARK,
                                       C.ORDER_ID ORDERID --移动平台订单号
                              FROM OUTP_ORDER_DESC A, OUTP_BILL_ITEMS B, DEPT_DICT,OUTPADM.TRADE_RECORD C
                             WHERE A.VISIT_DATE = B.VISIT_DATE
                               AND A.VISIT_NO = B.VISIT_NO
                               AND A.ORDERED_BY_DEPT = DEPT_DICT.DEPT_CODE(+)
                               AND C.RCPT_NO=A.RCPT_NO";
            if (!string.IsNullOrEmpty(hospitalId))
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            if (!string.IsNullOrEmpty(healthCardNo))
            {
                strSql += " AND A.PATIENT_ID='" + healthCardNo + "' ";
            }
            if (!string.IsNullOrEmpty(patientId))
            {
                strSql += " AND A.PATIENT_ID='" + patientId + "' ";
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                strSql += " AND TO_CHAR(A.VISIT_DATE,'yyyy-MM-dd') >='" + startDate + "' ";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                strSql += " AND TO_CHAR(A.VISIT_DATE,'yyyy-MM-dd') <='" + endDate + "' ";
            }
            strSql += @"GROUP BY A.RCPT_NO,
                              A.VISIT_DATE,
                              A.ORDERED_BY_DEPT,
                              A.ORDERED_BY_DOCTOR,
                              DEPT_DICT.DEPT_NAME,
                              C.ORDER_ID";
            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }

        /// <summary>
        /// 待缴费记录支付
        /// </summary>
        /// <param name="hospitalId">医院代码</param>
        /// <param name="healthCardNo">用户健康卡号码</param>
        /// <param name="patientId">患者唯一ID</param>
        /// <param name="clinicSeq">就诊流水号</param>
        /// <param name="orderId">支付平台订单号</param>
        /// <param name="tradeNo">第三方支付交易流水号</param>
        /// <param name="operatorId">操作员工号</param>
        /// <param name="machineId">设备代码（针对自助设备）</param>
        /// <param name="payAmout">自费金额(单位：分)</param>
        /// <param name="recPayAmout">统筹金额(单位：分)</param>
        /// <param name="totalPayAmout">总金额(单位：分)</param>
        /// <param name="payMode">支付方式：具体编码定义见2.7支付方式</param>
        /// <param name="payTime">交易时间，格式：YYYY-MM-DD HI24:MI:SS</param>
        /// <param name="prescriptionIds">处方号，多个处方号用半角逗号分隔</param>
        /// <param name="medicareSettleLogId">医保预结算参数</param>
        /// <returns></returns>
        public DataSet outpatient_Pay(string hospitalId, string healthCardNo, string patientId, string clinicSeq, string orderId,
            string tradeNo, string operatorId, string machineId, string payAmout, string recPayAmout, string totalPayAmout,
            string payMode, string payTime, string prescriptionIds, string medicareSettleLogId)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(hospitalId))
            {
                PubConn.writeFileLog("医疗机构代码不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码不能为空"));
                return ds;
            }
            else
            {
                if (hospitalId != "42520068101")
                {
                    PubConn.writeFileLog("医疗机构代码错误");
                    ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                    return ds;
                }
            }
            if (string.IsNullOrEmpty(healthCardNo))
            {
                PubConn.writeFileLog("健康卡号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,健康卡号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(patientId))
            {
                PubConn.writeFileLog("患者唯一ID不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,患者唯一ID不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(clinicSeq))
            {
                PubConn.writeFileLog("就诊流水号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,就诊流水号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(orderId))
            {
                PubConn.writeFileLog("支付平台订单号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,支付平台订单号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(tradeNo))
            {
                PubConn.writeFileLog("第三方交易流水号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,第三方交易流水号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(operatorId))
            {
                PubConn.writeFileLog("操作员工号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,操作员工号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(payAmout))
            {
                PubConn.writeFileLog("自费金额不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,自费金额不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(payMode))
            {
                PubConn.writeFileLog("支付方式不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,支付方式不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(payTime))
            {
                PubConn.writeFileLog("交易日期不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,交易日期不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(prescriptionIds))
            {
                PubConn.writeFileLog("处方号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,处方号不能为空"));
                return ds;
            }
            DataTable dt2 = new DataTable();
            string strSql = @" SELECT DISTINCT '' REMARK, '' GUIDEINFO, A.RCPT_NO RECEIPTID
                                      FROM OUTP_BILL_ITEMS A
                                      LEFT JOIN CLINIC_MASTER B
                                        ON A.RCPT_NO = B.RCPT_NO
                                      LEFT JOIN OUTPADM.TRADE_RECORD C
                                        ON A.RCPT_NO = C.RCPT_NO
                                      LEFT JOIN OUTP_PRESC_MASTER D
                                        ON A.RCPT_NO = D.RCPT_NO
                                     WHERE B.PATIENT_ID = '" + patientId +
                                       "' AND B.PATIENT_ID = '" + healthCardNo +
                                       "' AND B.VISIT_NO = '" + clinicSeq +
                                       "' AND C.ORDER_ID = '" + orderId +
                                       "' AND C.TRADE_NO = '" + tradeNo +
                                       "' AND (SELECT OPERATOR_NO FROM OUTP_ACCT_MASTER " +
                                            "WHERE MIN_RCPT_NO <= B.RCPT_NO AND MAX_RCPT_NO >= B.RCPT_NO) = '" + operatorId +
                                       "' AND A.COSTS = '" + payAmout +
                                       "' AND C.PAY_MODE = '" + payMode +
                                       "' AND TO_CHAR(C.TRADE_DATE,'YYYY-MM-DD HI24:MI:SS') = '" + payTime +
                                       "' AND D.PRESC_NO IN (" + prescriptionIds + ")";
            if (!string.IsNullOrEmpty(machineId))
            {
                //strSql += " AND A.PATIENT_ID='" + healthCardNo + "' ";
            }
            if (!string.IsNullOrEmpty(recPayAmout))
            {
                //strSql += " AND A.PATIENT_ID='" + patientId + "' ";
            }
            if (!string.IsNullOrEmpty(totalPayAmout))
            {
                strSql += " AND A.COSTS = '" + totalPayAmout + "'";
            }
            if (!string.IsNullOrEmpty(medicareSettleLogId))
            {
                //strSql += " AND TO_CHAR(A.VISIT_DATE,'yyyy-MM-dd') <='" + endDate + "' ";
            }
            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }


        /// <summary>
        ///  已缴费记录明细查询
        /// </summary>
        /// <param name="clinicSeq">就诊流水号</param>
        /// <param name="receiptId">收据ID</param>
        /// <returns></returns>
        public DataSet outpatient_GetCompletedPayDetailInfo(string clinicSeq, string receiptId)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(clinicSeq))
            {
                PubConn.writeFileLog("就诊流水号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,就诊流水号不能为空"));
                return ds;
            }
            DataTable dt2 = new DataTable();
            string strSql = @"  SELECT B.FEE_CLASS_NAME DETAILFEE, --缴费费别
                                       TO_CHAR(A.ITEM_NO) DETAILID, --缴费细目流水号，要求唯一
                                       A.ITEM_NAME DETAILNAME, --缴费细目名称
                                       TO_CHAR(A.AMOUNT) DETAILCOUNT, --缴费细目数量
                                       A.UNITS DETAILUNIT, --缴费细目单位
                                       A.COSTS * 100 DETAILAMOUT, --缴费细目金额，单位：分
                                       A.ITEM_SPEC DETAILSPEC, --缴费规格
                                       TO_CHAR(A.COSTS / A.AMOUNT) * 100 DETAILPRICE, --缴费细目单价，单位：分
                                       '' USAGE,
                                       '' USAGEADVICE,
                                       '0' ALLOWREFUND,
                                       '' REMARK
                                  FROM OUTP_BILL_ITEMS A, OUTP_RCPT_FEE_DICT B
                                 WHERE A.CLASS_ON_RCPT = B.FEE_CLASS_CODE ";
            if (!string.IsNullOrEmpty(clinicSeq))
            {
                strSql += " AND A.RCPT_NO='" + clinicSeq + "' ";
            }
            if (!string.IsNullOrEmpty(receiptId))
            {
                strSql += " AND A.RCPT_NO='" + receiptId + "' ";
            }
            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败," + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }

        /// <summary>
        /// 门诊处方查询
        /// </summary>
        /// <param name="clinicSeq">就诊流水号</param>
        /// <param name="doctorId">医生工号</param>
        /// <returns></returns>
        public DataSet outpatient_GetPrescriptionInfo(string clinicSeq, string doctorId)
        {
            DataSet ds = new DataSet();

            if (string.IsNullOrEmpty(clinicSeq))
            {
                PubConn.writeFileLog("就诊流水号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,就诊流水号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(doctorId))
            {
                PubConn.writeFileLog("医生工号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,医生工号不能为空"));
                return ds;
            }
            DataTable dt2 = new DataTable();
            string strSql = @" SELECT A.PRESC_NO PRESCRIPTIONID,
                                       A.PRESC_TYPE PRESCRIPTIONNAME,
                                       A.ORDERED_BY DEPTID,
                                       (SELECT DEPT_NAME FROM DEPT_DICT WHERE DEPT_CODE = A.ORDERED_BY) DEPTNAME,
                                       (SELECT USER_ID
                                          FROM USERS
                                         WHERE USER_NAME = B.ORDERED_BY_DOCTOR
                                           AND ROWNUM = 1) DOCTORID,
                                       B.ORDERED_BY_DOCTOR DOCTORNAME,
                                       (SELECT SUM(COSTS) * 100
                                          FROM OUTP_PRESC_DETAIL
                                         WHERE PRESC_NO = A.PRESC_NO
                                         GROUP BY PRESC_NO) PAYAMOUT,
                                       (CASE
                                         WHEN A.RCPT_NO IS NULL THEN
                                          '0'
                                         ELSE
                                          '1'
                                       END) PAYFLAG,
                                       '' PAYABLE
                                  FROM OUTP_PRESC_MASTER A
                                  LEFT JOIN OUTP_ORDER_DESC B
                                    ON A.RCPT_NO = B.RCPT_NO WHERE 1=1";
            if (!string.IsNullOrEmpty(clinicSeq))
            {
                strSql += " AND A.RCPT_NO='" + clinicSeq + "' ";
            }
            if (!string.IsNullOrEmpty(doctorId))
            {
                strSql += @" AND (SELECT USER_ID
                                          FROM USERS
                                         WHERE USER_NAME = B.ORDERED_BY_DOCTOR
                                           AND ROWNUM = 1)='" + doctorId + "' ";
            }
            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败" + ex.Message));
                ds.Tables.Add(dt2);
            }
            return ds;
        }


        /// <summary>
        /// 门诊处方明细查询
        /// </summary>
        /// <param name="clinicSeq">就诊流水号</param>
        /// <param name="doctorId">医生工号</param>
        /// <param name="prescriptionId">处方ID</param>
        /// <returns></returns>
        public DataSet outpatient_GetPrescriptionDetailInfo(string clinicSeq, string doctorId, string prescriptionId)
        {
            DataSet ds = new DataSet();
            if (string.IsNullOrEmpty(clinicSeq))
            {
                PubConn.writeFileLog("就诊流水号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,就诊流水号不能为空"));
                return ds;
            }
            if (string.IsNullOrEmpty(prescriptionId))
            {
                PubConn.writeFileLog("处方号不能为空");
                ds.Tables.Add(GetStatus("-1", "查询失败,处方号不能为空"));
                return ds;
            }
            DataTable dt2 = new DataTable();
            string strSql = @" SELECT B.PHAM_CODE DETAILID,
                                       B.PHAM_NAME DETAILNAME,
                                       B.NUMBER_OF_PACKAGES DETAILCOUNT,
                                       B.PACKAGE_UNITS DETAILUNIT,
                                       B.COSTS * 100 DETAILAMOUT,
                                       B.PHAM_SPEC DETAILSPEC,
                                       B.USAGE USAGE,
                                       '' USAGEADVICE
                                  FROM OUTP_PRESC_MASTER A
                                  LEFT JOIN OUTP_PRESC_DETAIL B
                                    ON A.PRESC_NO = B.PRESC_NO
                                  LEFT JOIN OUTP_ORDER_DESC C
                                    ON A.RCPT_NO = C.RCPT_NO
                                  WHERE 1=1";
            if (!string.IsNullOrEmpty(clinicSeq))
            {
                strSql += " AND A.RCPT_NO='" + clinicSeq + "' ";
            }
            if (!string.IsNullOrEmpty(doctorId))
            {
                strSql += @" AND (SELECT USER_ID
                                          FROM USERS
                                         WHERE USER_NAME = C.ORDERED_BY_DOCTOR
                                           AND ROWNUM = 1)='" + doctorId + "' ";
            }
            if (!string.IsNullOrEmpty(prescriptionId))
            {
                strSql += " AND A.PRESC_NO='" + prescriptionId + "' ";
            }

            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败" + ex.Message));
                ds.Tables.Add(dt2);
            }
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