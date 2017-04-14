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
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医疗机构代码错误");
                ds.Tables.Add(GetStatus("-1", "查询失败"));
                return ds;
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
        public DataSet hospital_getDoctorInfo(string hospitalId, string deptId, string doctorid)
        {
            DataSet ds = new DataSet();
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医疗机构代码错误");
                ds.Tables.Add(GetStatus("-1", "查询失败"));
                return ds;
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
                PubConn.writeFileLog("医生工号、健康卡号、病人ID号不能同事为空");
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医疗机构代码错误");
                ds.Tables.Add(GetStatus("-1", "查询失败"));
                return ds;
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
                return ds;
            }
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医疗机构代码错误");
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
                strSql += " AND TO_CHAR(A.REGDATE'yyyy-MM-dd') >= '" + startDate + "' ";
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                strSql += " AND TO_CHAR(A.REGDATE'yyyy-MM-dd') <= '" + endDate + "' ";
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医疗机构代码错误");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                return ds;
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
                               AND B.CLINIC_DEPT = D.DEPT_CODE WHERE 1=1";
            if (!string.IsNullOrEmpty(scheduleId))
            {
                //strSql += " AND A.DEPTID='" + deptId + "' ";
            }
            else
            {
                strSql += " B.CLINIC_DEPT='" + deptId + "' AND　B.SERIAL_NO ='" + clinicUnitId +
                    "' AND S.STAFF_ID='" + doctorId + "' AND TO_CHAR(C.CLINIC_DATE,'yyyy-MM-dd')='" + regDate + "' ";
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
                        else if (shiftCode == "1")
                        {
                            dr["STARTTIME"] = "14:30";
                            dr["ENDTIME"] = "17:00";
                        }
                        else if (shiftCode == "1")
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医疗机构代码错误");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                return ds;
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医疗机构代码错误");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
                return ds;
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医疗机构代码错误");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
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
                strSql += @"  TO_DATE(TO_CHAR(B.VISIT_DATE, 'YYYY-MM-DD'), 'YYYY-MM-DD')) W
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医疗机构代码错误");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
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
                strSql += " AND (SELECT USER_ID FROM USERS WHERE USER_NAME=E.ORDERED_BY_DOCTOR AND ROWNUM=1 ) ='" + doctorId + "' ";
            }
            if (!string.IsNullOrEmpty(settleCode))
            {
                strSql += " AND B.INSUR_RCPT_CLASS='" + settleCode + "' ";
            }
            if (!string.IsNullOrEmpty(prescriptionIds))
            {
                strSql += " AND D.PRESC_NO in (" + settleCode + ") ";
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
            if (hospitalId != "42520068101")
            {
                PubConn.writeFileLog("医疗机构代码错误");
                ds.Tables.Add(GetStatus("-1", "查询失败,医疗机构代码错误"));
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
                                       '' ORDERID --移动平台订单号
                              FROM OUTP_ORDER_DESC A, OUTP_BILL_ITEMS B, DEPT_DICT
                             WHERE A.VISIT_DATE = B.VISIT_DATE
                               AND A.VISIT_NO = B.VISIT_NO
                               AND A.ORDERED_BY_DEPT = DEPT_DICT.DEPT_CODE(+) ";
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
                              DEPT_DICT.DEPT_NAME";
            try
            {
                dt2 = PubConn.Query(strSql, strHISConn).Tables[0];
                ds.Tables.Add(GetStatus("0", "查询成功"));
                ds.Tables.Add(dt2.Copy());
            }
            catch (Exception ex)
            {
                PubConn.writeFileLog(ex.Message);
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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
                ds.Tables.Add(GetStatus("-1", "查询失败"));
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