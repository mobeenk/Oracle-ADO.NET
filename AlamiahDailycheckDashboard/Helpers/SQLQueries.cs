using System.Configuration;

namespace AlamiahDailycheckDashboard.Helpers
{
    public static class SQLQueries
    {

        // BuildMyString.com generated code. Please enjoy your string responsibly.
        public static string trx_query { get; } = "SELECT COUNT(*),MT_TE_CODE,MT_STATUS,MT_VALID , MT_ELM_ERROR  " +
                "FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_MUTAMER_TRANSACTIONS " +
                "WHERE MT_VALID NOT IN(-4,44,201001,299001,-201001,202009,-20,-204002,-202006, -10202 , -10204 , -10210  , 130007  ,149,212,218,219 ,204005 ,5 ,10406 ,-1 ,404)  " +
                "   AND MT_STATUS NOT IN(40) GROUP BY MT_TE_CODE,MT_STATUS,MT_VALID ,MT_ELM_ERROR ORDER BY MT_TE_CODE";
        public static string max_query { get; } =
         "SELECT A.* , B.CNT AS GROUPS_PENDING_PAYMENT  , B.GR PENDING_MUTAMERS" +
                " FROM" +
                " (" +
                " SELECT (TRUNC((SYSDATE - (MAX(GR_MOFA_DATE))) * 24) || ' : ') || (ROUND(MOD((SYSDATE - (MAX(GR_MOFA_DATE))) * 24 * 60, 60)) || ' : ') || (ROUND(MOD((SYSDATE - (MAX(GR_MOFA_DATE))) * 24 * 60 * 60, 60)) || '  ')LAST_MOFA" +
                "                , (TRUNC((SYSDATE - (MAX(GR_PAYMENT_DATE))) * 24) || ' : ') || (ROUND(MOD((SYSDATE - (MAX(GR_PAYMENT_DATE))) * 24 * 60, 60)) || ' : ') || (ROUND(MOD((SYSDATE - (MAX(GR_PAYMENT_DATE))) * 24 * 60 * 60, 60)) || '  ')LAST_PAYMENT" +
                "               , (TRUNC((SYSDATE - (MAX(YVI_TIMESTAMP_LA))) * 24) || ' : ') || (ROUND(MOD((SYSDATE - (MAX(YVI_TIMESTAMP_LA))) * 24 * 60, 60)) || ' : ') || (ROUND(MOD((SYSDATE - (MAX(YVI_TIMESTAMP_LA))) * 24 * 60 * 60, 60)) || '  ')LAST_VOUCHER" +
                "                                     FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS ," + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_VOUCHER_INFO " +
                "                      WHERE GR_CODE = YVI_GROUP_ID" +
                "    )   a" +
                "    ,(     SELECT COUNT(*) CNT,SUM(GR_COUNT) gr   FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".DPU_GROUP_TRACKER X " +
                "     INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS ON GR_CODE = GT_GROUP_CODE " +
                "  WHERE GT_GR_STATE = 5 AND GT_GR_MOH_STATE = 530 ) B";
        public static string elm_query { get; } =
        "   SELECT M_SERIAL_NO , ENEX_MUTAMER_ID " +
                " FROM    " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_ENEX_DATA_DELETED " +
                "INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_MUTAMER_DATA  ON M_MOH_ID = ENEX_MUTAMER_ID   " +
                "     AND FLAG = 0 ";

        public static string elm_status_query { get; } =
        " SELECT  nvl( (ROUND((SYSDATE - MAX(EE_CREATED_DATE)) * 24 * 60) ) ,0) as M_CREATED, nvl((ROUND((SYSDATE - MAX(EE_MODIFY_DATE)) * 24 * 60) ),0) as M_MODIFIED , " +
                "(TRUNC((SYSDATE - MAX(EE_CREATED_DATE)) * 24) " +
                "|| ' : ')|| (ROUND(MOD((SYSDATE - MAX(EE_CREATED_DATE)) * 24 * 60, 60)) || ' : ') || (ROUND(MOD((SYSDATE - MAX(EE_CREATED_DATE)) * 24 * 60 * 60, 60)) ||" +
                " '  ') LAST_CREATE,(TRUNC((SYSDATE - MAX(EE_MODIFY_DATE)) * 24) || ' : ') || (ROUND(MOD((SYSDATE - MAX(EE_MODIFY_DATE)) * 24 * 60, 60)) || ' : ') || (ROUND(MOD((SYSDATE - MAX(EE_MODIFY_DATE)) * 24 * 60 * 60, 60)) || '  ') LAST_MODIFY    " +
                ",MAX(EE_CREATED_DATE) CREATED ,MAX(EE_MODIFY_DATE )  MODIFIED  " +
                "                                       FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_ENTRY_EXIT_DATA ";
        public static string groups_request0_query { get; } =
 "     SELECT      (TRUNC ((SYSDATE-( X.MSG_TIME))*24)||' : ')||(ROUND(MOD((SYSDATE-( X.MSG_TIME))*24*60,60))||' : ')||(ROUND(MOD((SYSDATE-( X.MSG_TIME))*24*60*60,60))||'  ')DELAY," +
           " CNTRY_NAME_AR, GT_GROUP_CODE المجموعة,GT_GR_STATE,GT_GR_MOH_STATE  ,GR_UO_SEND ارسال_الشركة" +
            "          , GR_UO_CODE كود_الشركة,  GR_TPKG_UASP_CODE     , R_ID  , X.MSG_TIME ACTUAL_SEND_TIME" +
         " FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS A," + ConfigurationManager.AppSettings["Schema_year"] + ".DPU_GROUP_TRACKER B, " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST X ," + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES C" +
          "  WHERE B.GT_GROUP_CODE = A.GR_CODE     AND GT_GROUP_CODE = X.R_ENITITY_PK   AND A.GR_FROM_COUNTRY_ID = C.CNTRY_ID" +
           "  AND R_TYPE IN(100)        AND CNTRY_ID  not in (967, 249)     AND(GT_GR_STATE = 3)    AND GR_UO_CODE != 888" +
           "    AND NOT  EXISTS(SELECT NULL FROM   " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST_REPLY Y WHERE X.R_ID = Y.R_ID)" +
            "ORDER BY   1 desc ";

        public static string groups10_400_query { get; } =
        "     SELECT     (TRUNC ((SYSDATE-( X.MSG_TIME))*24)||' : ')||(ROUND(MOD((SYSDATE-( X.MSG_TIME))*24*60,60))||' : ')||(ROUND(MOD((SYSDATE-( X.MSG_TIME))*24*60*60,60))||'  ')DELAY,   " +
                "   GT_MOH_GROUP_CODE  رقم_المجموعة_الوزاري ,GT_GROUP_CODE المجموعة,GT_GR_STATE,GT_GR_MOH_STATE  ,GR_UO_SEND  ارسال_الشركة                ,GR_UO_CODE كود_الشركة,  GR_TPKG_UASP_CODE    " +
                " , R_ID  , X.MSG_TIME ACTUAL_SEND_TIME" +
                "   FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS A," + ConfigurationManager.AppSettings["Schema_year"] + ".DPU_GROUP_TRACKER  B ,  " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST X   " +
                " WHERE B.GT_GROUP_CODE=A.GR_CODE     AND   GT_GROUP_CODE = X.R_ENITITY_PK     " +
                "  AND (GT_GR_STATE = 10)    AND GT_GR_MOH_STATE IN (  400, 500)     AND GR_UO_CODE != 888 " +
                "   AND NOT  EXISTS ( SELECT NULL FROM   " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST_REPLY Y WHERE X.R_ID =  Y.R_ID     )    ORDER BY   1 desc";

        public static string sms_check_query { get; } =
      "   SELECT COUNT(*)  as count, (  ROUND((SYSDATE - MAX(SJM_GEN_DT))*24*60))  as MINUTES ,SJM_MSG ,  ( TRUNC ((SYSDATE - MAX(SJM_GEN_DT))*24) ||' : ')||" +
                " (  ROUND(MOD((SYSDATE - MAX(SJM_GEN_DT))*24*60,60)) ||' : ')  || (  ROUND(MOD((SYSDATE - MAX(SJM_GEN_DT))*24*60*60,60)) ||'  ')   GENERATED_SINCE" +
                "    FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".SMS_JOB_MSG " +
                "   WHERE SJM_SENT = 0    GROUP BY SJM_MSG  ,SJM_GEN_DT";

        public static string fees_query { get; } = "SELECT COUNT(*) as count, nvl (ROUND((SYSDATE-(max( M_CREATION_DATE)))*24*60) ,0) as MINUTES" +
         ",(TRUNC ((SYSDATE-(  max(M_CREATION_DATE)  ))*24)||' : ')||(ROUND(MOD((SYSDATE-(max( M_CREATION_DATE)))*24*60,60))||' : ')||(ROUND(MOD((SYSDATE-( max(M_CREATION_DATE)))*24*60*60,60))||'  ')DELAY" +
         " FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_MUTAMER_DATA        " +
         "         WHERE MUTAMER_FEES_FLAG IS NULL";


        public static string uos_left_query { get; } = "     SELECT UO_ID AS \"التصريح\" , UO_NAME_AR AS \"الشركة\", FLD_5 AS \"الحالة\"" +
         "    FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".LOOKUPS_ENTITIES A" +
         "    INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".LOOKUPS_NOTIFICATION B  ON A.ENT_ID=B.LN_ENT_ID" +
         "    INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_UAUO_CONTRACTS  ON  B.LN_COL_PK  = " + "UAUO_ID" +
         "    INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_UOS X  ON UO_ID = UAUO_UO_ID" +
         "    AND (LN_TIMESTAMP)>=(SYSDATE-1)   " +
         "    AND  (LN_ENT_ID = 67)  AND FLD_5 = 512" +
         "    ORDER BY B.MSG_TIME DESC";

        public static string uos_left_message_query { get; } = "     SELECT UO_ID AS \"التصريح\" , UO_NAME_AR AS \"الشركة\", FLD_5 AS \"الحالة\"" +
           "    FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".LOOKUPS_ENTITIES A" +
           "    INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".LOOKUPS_NOTIFICATION B  ON A.ENT_ID=B.LN_ENT_ID" +
           "    INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_UAUO_CONTRACTS  ON  B.LN_COL_PK  = " + "UAUO_ID" +
           "    INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_UOS X  ON UO_ID = UAUO_UO_ID" +
           "    AND (LN_TIMESTAMP)>=(SYSDATE-9)   " +
           "    AND  (LN_ENT_ID = 67)  AND FLD_5 = 512" +
           "    ORDER BY B.MSG_TIME DESC"
           ;

        public static string check_groups_query { get; } = "  SELECT      GT_GR_MOH_STATE,  GT_GR_STATE  ,  " +
               " DECODE(GT_GR_STATE,3,ROUND((SYSDATE-MIN( GR_UASP_SEND))*24*60),10,ROUND((SYSDATE-MIN( GR_UASP_SEND))*24*60) " +
               ",8,ROUND((SYSDATE - MIN(GR_PAYMENT_DATE)) * 24 * 60),12,ROUND((SYSDATE - MIN(GR_PAYMENT_DATE)) * 24 * 60)  , 4,ROUND((SYSDATE - MIN(GR_UASP_SEND)) * 24 * 60)        )  DELAY_MINS , " +
               //  "   DECODE(GT_GR_STATE,8,ROUND((SYSDATE-MIN( GR_PAYMENT_DATE))*24*60)" + ",12,ROUND((SYSDATE-MIN( GR_PAYMENT_DATE))*24*60)) PAYMENT_DELAY_MINS ,  " +
               "  COUNT(*) group_count ,    DECODE(GT_GR_STATE,3,'قيد التدقيق',10,'انتظار فاتورة-',8,'تم السداد', 12,'في الخارجية  ')  status   " +
               "   ,   SUM(GR_COUNT) MUTAMERS " +
               "   FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS ," + ConfigurationManager.AppSettings["Schema_year"] + ".DPU_GROUP_TRACKER   " +
               "  WHERE GT_GROUP_CODE=GR_CODE    AND GT_GR_STATE IN (3,10,8,12,13,9,45,8,4) " +
               "  AND GR_FROM_COUNTRY_ID not in ( 967 ,249)  " +
               "  AND GT_GR_MOH_STATE != 514  " +
               "  GROUP BY GT_GR_STATE,GT_GR_MOH_STATE    ORDER BY GT_GR_STATE  ";


        public static string JMS_out_query { get; } = " SELECT COUNT(*),OUT_ERROR_DESCRIPTION,OUT_STATUS " +
               " FROM JMSUMRA.JMS_MSGS_OUTGOING  " +
               " WHERE  OUT_STATUS !=8    and OUT_QUEUE_ID !=104 " +
               " GROUP BY  OUT_ERROR_DESCRIPTION, OUT_STATUS"
               ;
        public static string JMS_in_query { get; } = "  SELECT COUNT(*),ERROR_DESCRIPTION, STATUS FROM" +
               " JMSUMRA.JMS_RAW_MSGS_INCOMING " +
               "WHERE STATUS  IN(-1,0) " +
               " GROUP BY ERROR_DESCRIPTION,STATUS";

        public static string JMS_in_delay_query { get; } = "  SELECT QUEUE_ID,Q_NAME ,  " + "  round((SYSDATE - MAX(ARRIVE_TIME))*24*60) as hrs ," +
          "  (TRUNC((SYSDATE - MAX(PARSE_TIME)) * 24) || ' : ') || (ROUND(MOD((SYSDATE - MAX(PARSE_TIME)) * 24 * 60, 60)) || ' : ') ||" +
          " (ROUND(MOD((SYSDATE - MAX(PARSE_TIME)) * 24 * 60 * 60, 60)) || '  ')  LAST_PARSE_SINCE     " +
          "  ,(TRUNC((SYSDATE - MAX(ARRIVE_TIME)) * 24) || ' : ') || (ROUND(MOD((SYSDATE - MAX(ARRIVE_TIME)) * 24 * 60, 60)) || ' : ') ||" +
          " (ROUND(MOD((SYSDATE - MAX(ARRIVE_TIME)) * 24 * 60 * 60, 60)) || '  ')  LAST_Arrive_SINCE     " +
          " FROM JMSUMRA.JMS_RAW_MSGS_INCOMING,JMSUMRA.JMS_QUEUES" +
          " WHERE Q_ID = QUEUE_ID" +
          " GROUP BY QUEUE_ID,Q_NAME ORDER BY QUEUE_ID"
          ;
        public static string top10_query { get; } = "SELECT* FROM " +
                    "(" +
                    "    SELECT M_UO_CODE AS \"التصريح\", UO_NAME_AR AS \"الشركة\",COUNT(*) AS \"الموفا\" " +
                    "    FROM " + ConfigurationManager.AppSettings["Schema_year"] +
                    ".YAHAJJ_MUTAMER_DATA " +
                    "    INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] +
                    ".V_LU_UOS ON UO_CODE = M_UO_CODE" +
                    "    WHERE M_MOFA_APPROVAL_NO IS NOT NULL" +
                    "    GROUP BY M_UO_CODE ,UO_NAME_AR" +
                    "    ORDER BY 3 DESC" +
                    ")" +
                    "WHERE ROWNUM <=10";

        public static string moh_notifications_query { get; }=
         "        SELECT( TRUNC ((SYSDATE - (INSERT_MSG_TIME))*24) ||' : ')|| (  ROUND(MOD((SYSDATE - (INSERT_MSG_TIME))*24*60,60)) ||' : ')  || (  ROUND(MOD((SYSDATE - (INSERT_MSG_TIME ))*24*60*60,60)) ||' ')   LAST_ARRIVE_SINCE" +
        "        , SUBSTR (MND_MSG_BODY,1,100) MSG " + " , ROUND(TO_NUMBER(SYSDATE-INSERT_MSG_TIME)*24*60) MINS"+
        "        FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".NOTICE_NOTIFICATION A" +
        "        LEFT JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".NOTICE_NOTIFICATION_ATTACHMENT B ON A.MND_NOTIFICATION_ID  = B.MND_NOTIFICATION_ID" +
        "        WHERE  TRUNC(INSERT_MSG_TIME)>=TRUNC(SYSDATE-1 )" +
        "        AND MND_NOTIFICATION_TYPE = 1" +
        "        ORDER BY INSERT_MSG_TIME DESC"
            ;


        public static string total_mofa_query { get; } =
                                                         "      SELECT" +
                                                         "           SUM(NVL(A.TOTAL_MOFA_PREVIOUS, 0)) AS \"prev\"," +
                                                         "           SUM(NVL(B.TOTAL_MOFA_CURRENT, 0)) AS \"curr\"," +
                                                         "         (SUM(NVL(B.TOTAL_MOFA_CURRENT, 0)) - SUM(NVL(A.TOTAL_MOFA_PREVIOUS, 0))) AS \"growth\"," +
                                                         "         ROUND(((SUM(B.TOTAL_MOFA_CURRENT) - SUM(A.TOTAL_MOFA_PREVIOUS)) / SUM(A.TOTAL_MOFA_PREVIOUS)) * 100, 0) || '%' AS \"per\"" +
                                                         "      FROM (  SELECT COUNT(*) AS TOTAL_MOFA_PREVIOUS, CNTRY_NO" +
                                                         "                FROM UMRA1440.YAHAJJ_MUTAMER_DATA YMD," +
                                                         "                      UMRA1440.BAU_MUTAMER_GROUPS YMF," +
                                                         "                      UMRA1440.YAHAJJ_EXTERNAL_AGENTS YEA, " +
                                                         "                      UMRA1440.V_LU_COUNTRIES" +
                                                         "              WHERE    YMD.M_EA_CODE = YEA.EA_CODE" +
                                                         "                      AND CNTRY_ID = EA_ORIGANL_COUNTRY" +
                                                         "                      AND YMD.M_MUTAMER_GROUP = GR_CODE" +
                                                         "                        AND YMD.M_UO_CODE !=888" +
                                                         "                      AND M_MOFA_APPROVAL_NO IS NOT NULL" +
                                                         "                       AND (TRUNC(GR_MOFA_DATE) <=     TRUNC(TO_DATE( concat(TO_CHAR(SYSDATE,  'DD/MM',  'NLS_CALENDAR=''ARABIC HIJRAH'''  ) ,'/1440'), 'dd/mm/yyyy','nls_calendar=''English Hijrah'''))  )" +
                                                         "          GROUP BY CNTRY_NO) A," +
                                                         "         (  SELECT COUNT(*) AS TOTAL_MOFA_CURRENT, CNTRY_NO" +
                                                         "                FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_MUTAMER_DATA YMD, " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS YMF," +
                                                         "                      " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_EXTERNAL_AGENTS YEA, " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES" +
                                                         "              WHERE                   " +
                                                         "                      YMD    .M_EA_CODE = YEA.EA_CODE" +
                                                         "                      AND CNTRY_ID = EA_ORIGANL_COUNTRY" +
                                                         "                      AND YMD.M_MUTAMER_GROUP = GR_CODE" +
                                                         "                        AND YMD.M_UO_CODE !=888" +
                                                         "                      AND M_MOFA_APPROVAL_NO IS NOT NULL" +
                                                         "                       AND (TRUNC(GR_MOFA_DATE) <=   TRUNC(TO_DATE(    TO_CHAR(SYSDATE,  'DD/MM/YYYY ',   'NLS_CALENDAR=''ARABIC HIJRAH'''   ) , 'dd/mm/yyyy','nls_calendar=''English Hijrah'''))     )" +
                                                         "          GROUP BY CNTRY_NO) B," +
                                                         "         " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES CN" +
                                                         "    WHERE      A.CNTRY_NO(+) = CN.CNTRY_NO" +
                                                         "         AND B.CNTRY_NO(+) = CN.CNTRY_NO" +
                                                         "         AND (A.TOTAL_MOFA_PREVIOUS IS NOT NULL OR B.TOTAL_MOFA_CURRENT IS NOT NULL)" +
                                                         "          ORDER BY TOTAL_MOFA_CURRENT DESC";



        public static string queryR0 { get; } =
               "     SELECT      (TRUNC ((SYSDATE-( X.MSG_TIME))*24)||' : ')||(ROUND(MOD((SYSDATE-( X.MSG_TIME))*24*60,60))||' : ')||(ROUND(MOD((SYSDATE-( X.MSG_TIME))*24*60*60,60))||'  ')DELAY," +
               "  GT_MOH_GROUP_CODE , GT_GROUP_CODE ,GT_GR_STATE,GT_GR_MOH_STATE  ,GR_UO_SEND ارسال_الشركة" +
               "          , GR_UO_CODE كود_الشركة,  GR_TPKG_UASP_CODE     , R_ID  , X.MSG_TIME ACTUAL_SEND_TIME, CNTRY_NAME_AR" +
               ",  (ROUND ((SYSDATE-( X.MSG_TIME))*24*60)) as MINUTES" +
               " FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS A," +
               ConfigurationManager.AppSettings["Schema_year"] + ".DPU_GROUP_TRACKER B, " +
               ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST X ," +
               ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES C" +
               "  WHERE B.GT_GROUP_CODE = A.GR_CODE     AND GT_GROUP_CODE = X.R_ENITITY_PK   AND A.GR_FROM_COUNTRY_ID = C.CNTRY_ID" +
               "  AND R_TYPE IN(100)        AND CNTRY_ID  not in (967, 249)     AND(GT_GR_STATE = 3)  " +
               "  AND GR_UO_CODE != 888   " +
               " AND NOT  EXISTS(SELECT NULL FROM   " + ConfigurationManager.AppSettings["Schema_year"] +
               ".BAU_REQUEST_REPLY Y WHERE X.R_ID = Y.R_ID) " +
               //  "and not exists (select null from " + ConfigurationManager.AppSettings["Schema_year"] + ".groups_history where GR_REQ_NO = r_id)" +
               "ORDER BY   1 desc ";
        public static string queryR1 { get; } = "SELECT  X.R_ID   as R_ID         ,    (  ROUND(((SYSDATE-SUB_MAIN.LAST_REPLY)*24*60)) )    MINUTES   " +
                                                "   FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS A," + ConfigurationManager.AppSettings["Schema_year"] + ".DPU_GROUP_TRACKER  B , " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST X        ," +
                                                " (              SELECT C.R_ID AS RID ,C.R_CVC AS CVC ,C.MSG_TIME AS LAST_REPLY, ERR_DESC " +
                                                "      FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST_REPLY  C,           " +
                                                "     (  SELECT  MAX( A.MSG_TIME) AS MAX_DATE  , A.R_ID  " +
                                                "   FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST_REPLY  A   , " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST  B  " +
                                                "   WHERE A.R_ID = B.R_ID        GROUP BY A.R_ID      ) SUB    " +
                                                "         WHERE SUB.R_ID = C.R_ID AND SUB.MAX_DATE = C.MSG_TIME           ) SUB_MAIN    WHERE B.GT_GROUP_CODE=A.GR_CODE    " +
                                                "             AND ( GT_GR_STATE in (8,10,12))    " +
                                                "         AND GT_GROUP_CODE = X.R_ENITITY_PK  " +
                                                "  AND SUB_MAIN.RID = X.R_ID    AND GR_UO_CODE != 888      " +
                                                "  AND (  ERR_DESC IS NULL  AND   SUB_MAIN.CVC  !=0  )        AND GT_GR_MOH_STATE != 514    " +
                                                //"and not exists (select null from " + ConfigurationManager.AppSettings["Schema_year"] + ".groups_history where GR_REQ_NO = r_id) " +
                                                "  AND GR_FROM_COUNTRY_ID != 967 "
      ;
        public static string queryR1_400 { get; } =
                   "   SELECT    R_ID   , (ROUND((SYSDATE - (X.MSG_TIME)) * 24 * 60)) MINUTES" +
  "  FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS A, " + ConfigurationManager.AppSettings["Schema_year"] + ".DPU_GROUP_TRACKER B, " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST X" +
   "   WHERE B.GT_GROUP_CODE = A.GR_CODE    AND GT_GROUP_CODE = X.R_ENITITY_PK   " +
         "    AND(GT_GR_STATE = 10)        AND GT_GR_MOH_STATE IN(400, 500)      AND GR_UO_CODE != 888" +
              "     AND NOT  EXISTS(SELECT NULL FROM   " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_REQUEST_REPLY Y WHERE X.R_ID = Y.R_ID)" +
              //   "and not exists (select null from " + ConfigurationManager.AppSettings["Schema_year"] + ".groups_history where GR_REQ_NO = r_id)  " +
              "       ORDER BY   2 desc"
 ;



        public static string Q_UOS { get; } = "SELECT UO_CODE AS \"الشركة\",UO_NAME_AR AS \"اسم الشركة\", SUM(MUTAMERCOUNT) AS \"اجمالي موفا\", SUM(TMOI_ELM) AS \" اجمالي دخول\"," +
                                              " SUM(EXIT_ELM ) AS \"اجمالي خروج\", SUM(TMOI_ELM)-SUM(EXIT_ELM) AS \"المتواجدون\", SUM(ESCB_COUNT_ELM)  AS \"اجمالي تخلف علم\", SUM(ESCB_COUNT) AS \"اجمالي تخلف برنامج\"," +
                                              " ROUND (   (    (SUM(ESCB_COUNT_ELM)*100)  /  DECODE(     SUM(TMOI_ELM),0,1,  SUM(TMOI_ELM)     )      ),2) AS \"نسبة تخلف حسب علم\", " +
                                              " ROUND ( ((SUM(ESCB_COUNT)*100)/DECODE(SUM(TMOI_ELM),0,1,SUM(TMOI_ELM))),2) AS \"نسبة تخلف حسب برنامج\", SUM(ESCB_PACKGE_FIVE) AS \"تخلف 1 أيام\"," +
                                              "  ROUND (  ((SUM(ESCB_PACKGE_FIVE)*100)/DECODE(SUM(TMOI_ELM),0,1,SUM(TMOI_ELM))),2) AS \" نسبة تخلف 1 أيام\", DECODE(S1.CT,NULL,0,S1.CT) AS \"المحاضر\"" +
                                              ",(      SELECT NVL(SUM(QUANTITY),0) FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".GDS_RESERVATION   WHERE BOOKING_STATUS = 'Confirmed' AND  UO_ID  = UO_CODE  )  AS B2C" +
                                              " FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".TRANS_STAT_EA LEFT OUTER JOIN(SELECT COUNT(*) CT, INCR_UO_ID FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".V_INCIDENT_REPORT" +
                                              " GROUP BY INCR_UO_ID ORDER BY INCR_UO_ID )S1 ON S1.INCR_UO_ID = UO_CODE " +
                                              " WHERE MUTAMERCOUNT>0 AND UO_CODE !=888 GROUP BY UO_CODE,UO_NAME_AR  ,S1.CT " +
                                              " union" +
                                              " select null  AS \"الشركة\",null AS \"اسم الشركة\", sum(s.\"اجمالي موفا\")   AS \"اجمالي موفا\", sum(s.\" اجمالي دخول\" ) AS \" اجمالي دخول\", sum(s.\"اجمالي خروج\")  AS \" اجمالي دخول\", sum(s.\"المتواجدون\") AS \"المتواجدون\"," +
                                              " sum(s.\"اجمالي تخلف علم\") AS \"اجمالي تخلف علم\", sum(s.\"اجمالي تخلف برنامج\") AS \"اجمالي تخلف برنامج\", null AS \"نسبة تخلف حسب علم\",null AS \"نسبة تخلف حسب برنامج\",null  AS \"نسبة تخلف حسب برنامج\",null AS \"تخلف 1 أيام\",null AS \" نسبة تخلف 1 أيام\" " +
                                              "  , SUM(B2C) AS B2C " +
                                              " from(SELECT UO_CODE AS \"الشركة\", UO_NAME_AR AS \"اسم الشركة\", SUM(MUTAMERCOUNT) AS \"اجمالي موفا\", SUM(TMOI_ELM) AS \" اجمالي دخول\", SUM(EXIT_ELM ) AS \"اجمالي خروج\", SUM(TMOI_ELM)-SUM(EXIT_ELM) AS \"المتواجدون\", SUM(ESCB_COUNT_ELM)  AS \"اجمالي تخلف علم\"," +
                                              " SUM(ESCB_COUNT) AS \"اجمالي تخلف برنامج\", ROUND (         (    (SUM(ESCB_COUNT_ELM)*100)  /  DECODE(     SUM(TMOI_ELM),0,1,  SUM(TMOI_ELM)     )      ),2) AS \"نسبة تخلف حسب علم\", " +
                                              "  ROUND ( ((SUM(ESCB_COUNT)*100)/DECODE(SUM(TMOI_ELM),0,1,SUM(TMOI_ELM))),2) AS \"نسبة تخلف حسب برنامج\", SUM(ESCB_PACKGE_FIVE) AS \"تخلف 1 أيام\", " +
                                              " ROUND (  ((SUM(ESCB_PACKGE_FIVE)*100)/DECODE(SUM(TMOI_ELM),0,1,SUM(TMOI_ELM))),2) AS \" نسبة تخلف 1 أيام\", DECODE(S1.CT,NULL,0,S1.CT) AS \"المحاضر\" " +
                                              ",(      SELECT NVL(SUM(QUANTITY),0) FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".GDS_RESERVATION   WHERE BOOKING_STATUS = 'Confirmed' AND  UO_ID  = UO_CODE  )  AS B2C" +
                                              " FROM   " + ConfigurationManager.AppSettings["Schema_year"] + ".TRANS_STAT_EA  LEFT OUTER JOIN ( SELECT COUNT(*) CT,INCR_UO_ID FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".V_INCIDENT_REPORT" +
                                              " GROUP BY INCR_UO_ID ORDER BY INCR_UO_ID )S1 ON S1.INCR_UO_ID = UO_CODE  WHERE MUTAMERCOUNT>0" +
                                              " AND UO_CODE !=888 GROUP BY UO_CODE,UO_NAME_AR  ,S1.CT )s";

        public static string Q_EAS { get; } = "  SELECT DISTINCT " + ConfigurationManager.AppSettings["Schema_year"] + ".GETEAALIAC(B.EA_CODE) AS \"الوكيل\", UO_CODE AS \"الشركة\", UO_NAME_AR AS \"اسم الشركة\", " + ConfigurationManager.AppSettings["Schema_year"] + ".GETEANAME (B.EA_CODE ) AS \"اسم الوكيل\"," +
                                              " NT_NAME_AR AS \"دولة الوكيل\", SUM(MUTAMERCOUNT) AS \"اجمالي موفا\", SUM(TMOI_ELM) AS \"اجمالي دخول\", SUM(EXIT_ELM ) AS \"اجمالي خروج\"," +
                                              "SUM(TMOI_ELM)-SUM(EXIT_ELM) AS \" المتواجدون\", SUM(ESCB_COUNT_ELM) AS \"اجمالي تخلف علم\",SUM(ESCB_COUNT) AS \"اجمالي تخلف برنامج\"" +
                                              "  ,ROUND(((SUM(ESCB_COUNT_ELM) *100)/DECODE(SUM(TMOI_ELM),0,1, SUM(TMOI_ELM))),2) AS \"نسبة تخلف حسب العلم\"," +
                                              "    ROUND(((SUM(ESCB_COUNT) *100)/DECODE(SUM(TMOI_ELM),0,1, SUM(TMOI_ELM))),2) AS \"نسبة تخلف حسب برنامج\"" +
                                              "   FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".TRANS_STAT_EA A ," + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_EXTERNAL_AGENTS B" +
                                              "    WHERE MUTAMERCOUNT>0" +
                                              "    AND " + ConfigurationManager.AppSettings["Schema_year"] + ".GETEAALIAC(A.EA_CODE)= " + ConfigurationManager.AppSettings["Schema_year"] + ".GETEAALIAC(B.EA_CODE)" +
                                              "  AND UO_CODE !=888" +
                                              "   GROUP BY UO_CODE,UO_NAME_AR,B.EA_CODE,NT_NAME_AR" +
                                              "   UNION" +
                                              "   select null AS \"الوكيل\" ,null AS \"الشركة\" ,null  AS \"اسم الشركة\" ,null AS \"اسم الوكيل\" ,null AS \"دولة الوكيل\" , " +
                                              "   sum(s.\"اجمالي موفا\" ) AS \"اجمالي موفا\" , sum(s.\"اجمالي دخول\") AS \"اجمالي دخول\"  ,sum(s.\"اجمالي خروج\")  AS \"اجمالي خروج\", sum(s.\" المتواجدون\") AS \" المتواجدون\" " +
                                              "   , sum(s.\"اجمالي تخلف علم\") AS \"اجمالي تخلف علم\" , sum(s.\"اجمالي تخلف برنامج\" ) AS \"اجمالي تخلف برنامج\"  , null AS \"نسبة تخلف حسب العلم\",null AS \"نسبة تخلف حسب برنامج\"" +
                                              "   from" +
                                              "   (" +
                                              "    SELECT DISTINCT " + ConfigurationManager.AppSettings["Schema_year"] + ".GETEAALIAC(B.EA_CODE) AS \"الوكيل\", UO_CODE AS \"الشركة\", UO_NAME_AR AS \"اسم الشركة\", " + ConfigurationManager.AppSettings["Schema_year"] + ".GETEANAME (B.EA_CODE ) AS \"اسم الوكيل\"," +
                                              "   NT_NAME_AR AS \"دولة الوكيل\", SUM(MUTAMERCOUNT) AS \"اجمالي موفا\", SUM(TMOI_ELM) AS \"اجمالي دخول\", SUM(EXIT_ELM ) AS \"اجمالي خروج\"," +
                                              "   SUM(TMOI_ELM)-SUM(EXIT_ELM) AS \" المتواجدون\", SUM(ESCB_COUNT_ELM) AS \"اجمالي تخلف علم\", SUM(ESCB_COUNT) AS \"اجمالي تخلف برنامج\"" +
                                              "   , ROUND (((SUM(ESCB_COUNT_ELM) *100)/DECODE(SUM(TMOI_ELM),0,1, SUM(TMOI_ELM))),2) AS \"نسبة تخلف حسب العلم\"," +
                                              "     ROUND(((SUM(ESCB_COUNT) *100)/DECODE(SUM(TMOI_ELM),0,1, SUM(TMOI_ELM))),2) AS \"نسبة تخلف حسب برنامج\"" +
                                              "   FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".TRANS_STAT_EA A ," + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_EXTERNAL_AGENTS B" +
                                              "     WHERE MUTAMERCOUNT>0" +
                                              "     AND " + ConfigurationManager.AppSettings["Schema_year"] + ".GETEAALIAC(A.EA_CODE)= " + ConfigurationManager.AppSettings["Schema_year"] + ".GETEAALIAC(B.EA_CODE)" +
                                              "   AND UO_CODE !=888" +
                                              "   GROUP BY UO_CODE,UO_NAME_AR,B.EA_CODE,NT_NAME_AR" +
                                              "   )s" +
                                              "   ORDER BY 2,3"
            ;
        public static string Q_C_UOS { get; } = "SELECT UO_CODE AS \"الشركة\", UO_NAME_AR AS \"اسم الشركة\",CNTRY_NO AS \"رمز الدولة\" , CNTRY_NAME_AR  AS \"الدولة\",SUM(MUTAMERCOUNT)  AS \"اجمالي موفا\",SUM(TMOI_ELM) AS \"اجمالي دخول\",SUM(EXIT_ELM ) AS \"اجمالي خروج\"," +
                                                "SUM(TMOI_ELM)- SUM(EXIT_ELM) AS \"المتواجدون\",SUM(ESCB_COUNT_ELM) AS \"اجمالي تخلف علم\",SUM(ESCB_COUNT) AS \"اجمالي تخلف برنامج\"" +
                                                "  ,ROUND(((SUM(ESCB_COUNT_ELM) *100)/DECODE(SUM(TMOI_ELM),0,1, SUM(TMOI_ELM))),2) AS \"نسبة تخلف حسب العلم\",ROUND(((SUM(ESCB_COUNT) *100)/DECODE(SUM(TMOI_ELM),0,1, SUM(TMOI_ELM))),2) AS \"نسبة تخلف حسب البرنامج\"" +
                                                "  FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".TRANS_STAT_EA A ," + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_EXTERNAL_AGENTS B, " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES C" +
                                                "   WHERE MUTAMERCOUNT>0    AND B.EA_ORIGANL_COUNTRY=C.CNTRY_NO AND A.EA_CODE= B.EA_CODE AND UO_CODE !=888 GROUP BY UO_CODE, UO_NAME_AR, CNTRY_NO , CNTRY_NAME_AR" +
                                                "  UNION" +
                                                "   SELECT null  AS \"الشركة\" , null  AS \"اسم الشركة\" ,null AS \"رمز الدولة\" ,null  AS \"الدولة\", sum(S.\"اجمالي موفا\" ) AS \"اجمالي موفا\" , sum(S.\"اجمالي دخول\") AS \"اجمالي دخول\"  ,sum(S.\"اجمالي خروج\")  AS \"اجمالي خروج\" , sum(S.\"المتواجدون\") AS \"المتواجدون\"" +
                                                "   , sum(S.\"اجمالي تخلف علم\") AS \"اجمالي تخلف علم\" ,sum(S.\"اجمالي تخلف برنامج\")  AS \"اجمالي تخلف برنامج\" ,null AS \"نسبة تخلف حسب العلم\",null AS \"نسبة تخلف حسب البرنامج\"" +
                                                "  FROM" +
                                                "  (" +
                                                "    SELECT UO_CODE AS \"الشركة\", UO_NAME_AR AS \"اسم الشركة\", CNTRY_NO AS \"رمز الدولة\" , CNTRY_NAME_AR AS \"الدولة\", SUM(MUTAMERCOUNT) AS \"اجمالي موفا\", SUM(TMOI_ELM) AS \"اجمالي دخول\", SUM(EXIT_ELM ) AS \"اجمالي خروج\"," +
                                                "  SUM(TMOI_ELM)- SUM(EXIT_ELM) AS \"المتواجدون\", SUM(ESCB_COUNT_ELM) AS \"اجمالي تخلف علم\", SUM(ESCB_COUNT) AS \"اجمالي تخلف برنامج\"" +
                                                "  , ROUND (((SUM(ESCB_COUNT_ELM) *100)/DECODE(SUM(TMOI_ELM),0,1, SUM(TMOI_ELM))),2) AS \"نسبة تخلف حسب العلم\",ROUND(((SUM(ESCB_COUNT) *100)/DECODE(SUM(TMOI_ELM),0,1, SUM(TMOI_ELM))),2) AS \"نسبة تخلف حسب البرنامج\"  " +
                                                "  FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".TRANS_STAT_EA A ," + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_EXTERNAL_AGENTS B, " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES C" +
                                                "  WHERE MUTAMERCOUNT>0" +
                                                "     AND B.EA_ORIGANL_COUNTRY=C.CNTRY_NO" +
                                                "    AND A.EA_CODE= B.EA_CODE" +
                                                "  AND UO_CODE !=888 " +
                                                " GROUP BY UO_CODE, UO_NAME_AR, CNTRY_NO , CNTRY_NAME_AR" +
                                                "  )S            ORDER BY 1,2"
            ;


        public static string Q_C { get; } =
           " SELECT \"رمز الدولة\" ,\"الدولة\" ,  \"اجمالي موفا\" , \"اجمالي دخول\"  ,  \"اجمالي خروج\", \"متواجدون\", \"اجمالي تخلف علم\", \"اجمالي تخلف برنامج\"   ,B2C            FROM " +
"         (  " +
"                SELECT * FROM            " +
"                  (" +
"                        SELECT  CNTRY_NO AS \"رمز الدولة\" ,CNTRY_NAME_AR  AS \"الدولة\" , SUM(MUTAMERCOUNT) AS  \"اجمالي موفا\",SUM(TMOI_ELM) AS \"اجمالي دخول\",SUM(EXIT_ELM ) AS \"اجمالي خروج\", " +
"                        SUM(TMOI_ELM)- SUM(EXIT_ELM) AS \"متواجدون\",SUM(ESCB_COUNT_ELM) AS \"اجمالي تخلف علم\",SUM(ESCB_COUNT) AS \"اجمالي تخلف برنامج\" " +
"                           , (  SELECT  NVL(SUM(QUANTITY),0)     FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".GDS_RESERVATION   INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".COUNTRY_MAP ON ISO_COUNTRY2 = COUNTRY_ID    WHERE BOOKING_STATUS = 'Confirmed'    AND CNTRY_NO = ID) as B2C" +
"                        FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".TRANS_STAT_EA A ," + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_EXTERNAL_AGENTS B,  " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES C" +
"                        WHERE MUTAMERCOUNT>0  AND B.EA_ORIGANL_COUNTRY=C.CNTRY_NO AND A.EA_CODE= B.EA_CODE AND UO_CODE !=888  GROUP BY  CNTRY_NO , CNTRY_NAME_AR" +
"                        ORDER BY 3 DESC" +
"                  )" +
"                    " +
"                    UNION ALL" +
"                  " +
"                    SELECT * FROM " +
"                    (" +
"                        SELECT NULL AS \"رمز الدولة\" ,NULL  AS \"الدولة\" ,  SUM( S.\"اجمالي موفا\" )  AS \"اجمالي موفا\"" +
"                          , SUM(S.\"اجمالي دخول\" )  AS \"اجمالي دخول\"  ,SUM(  S.\"اجمالي خروج\") AS \"اجمالي خروج\" , SUM(  S.\"متواجدون\") AS \"متواجدون\" ," +
"                          SUM(S.\"اجمالي تخلف علم\")  AS \"اجمالي تخلف علم\", SUM( S.\"اجمالي تخلف برنامج\") AS \"اجمالي تخلف برنامج\"  , null as B2C" +
"                         FROM" +
"                        (" +
"                            SELECT CNTRY_NO AS \"رمز الدولة\" ,CNTRY_NAME_AR  AS \"الدولة\" , SUM(MUTAMERCOUNT) AS  \"اجمالي موفا\",SUM(TMOI_ELM) AS \"اجمالي دخول\",SUM(EXIT_ELM ) AS \"اجمالي خروج\"," +
"                            SUM(TMOI_ELM)- SUM(EXIT_ELM) AS \"متواجدون\",SUM(ESCB_COUNT_ELM) AS \"اجمالي تخلف علم\",SUM(ESCB_COUNT) AS \"اجمالي تخلف برنامج\"" +
"                               , (  SELECT  NVL(SUM(QUANTITY),0)     FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".GDS_RESERVATION   INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".COUNTRY_MAP ON ISO_COUNTRY2 = COUNTRY_ID    WHERE BOOKING_STATUS = 'Confirmed'    AND CNTRY_NO = ID)  as B2C" +
"                            FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".TRANS_STAT_EA A ," + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_EXTERNAL_AGENTS B, " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES C" +
"                            WHERE MUTAMERCOUNT>0  AND B.EA_ORIGANL_COUNTRY= C.CNTRY_NO   AND A.EA_CODE= B.EA_CODE AND UO_CODE !=888  GROUP BY  CNTRY_NO , CNTRY_NAME_AR" +
"                        )S                " +
"                     )" +
"         " +
"             )";



        // BuildMyString.com generated code. Please enjoy your string responsibly.


        public static string Q_DR { get; } = "select * from  (  SELECT" +
                                             " MAINQ.COUNTM AS \"رقم الشركة \" , UO_NAME_AR  AS \"الشركة\"     ,DAY1 AS \"1\",DAY2 AS \"2\",DAY3  AS \"3\" ,DAY4 AS \"4\" ,DAY5  AS \"5\",DAY6  AS \"6\",DAY7  AS \"7\",DAY8  AS \"8\",DAY9  AS \"9\",DAY10  AS \"10\",DAY11  AS \"11\",DAY12  AS \"12\",DAY13  AS \"13\",DAY14  AS \"14\",DAY15  AS \"15\",DAY16  AS \"16\",DAY17  AS \"17\",DAY18  AS \"18\"," +
                                             "        DAY19  AS \"19\",DAY20  AS \"20\",DAY21  AS \"21\",DAY22  AS \"22\",DAY23  AS \"23\",DAY24  AS \"24\",DAY25  AS \"25\",DAY26  AS \"26\",DAY27  AS \"27\",DAY28  AS \"28\",DAY29  AS \"29\",DAY30  AS \"30\"," +
                                             "       (DAY1+DAY2+DAY3+DAY4+DAY5+DAY6+DAY7+DAY8+DAY9+DAY10+DAY11+DAY12+DAY13+DAY14+DAY15+DAY16+DAY17+DAY18+DAY19+DAY20+DAY21+DAY22+DAY23+DAY24+DAY25+DAY26+DAY27+DAY28+DAY29+DAY30)  AS \"  اجمالي 30 يوم\", TOTAL_COUNT AS\" اجمالي الموفا\"" +
                                             " FROM " +
                                             "(" +
                                             "  SELECT  " +
                                             "         A.M_UO_CODE  AS COUNTM ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('01/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') || '/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END) DAY1 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('02/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY2 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('03/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END) DAY3 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('04/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY4 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('05/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY5," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('06/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY6 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('07/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END) DAY7 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('08/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY8 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('09/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY9 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('10/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY10 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('11/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY11 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('12/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY12 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('13/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY13 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('14/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY14 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('15/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY15 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('16/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY16 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('17/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY17 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('18/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY18 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('19/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY19 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('20/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY20 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('21/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY21 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('22/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY22 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('23/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY23 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('24/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY24 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('25/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY25 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('26/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY26 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('27/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY27 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('28/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY28 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('29/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY29 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('30/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY30" +
                                             "      ,             SUM(A.R_COUNT) AS TOTAL_COUNT  " +
                                             "      FROM " +
                                             "                 (" +
                                             "                     SELECT  COUNT(*) R_COUNT ,TRUNC(GR_MOFA_DATE) R_DATE, M_UO_CODE " +
                                             "                               FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_MUTAMER_DATA " +
                                             "                               INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS ON GR_CODE = M_MUTAMER_GROUP" +
                                             "                               WHERE M_MOFA_APPROVAL_NO IS NOT NULL " +
                                             "                               GROUP BY  TRUNC(GR_MOFA_DATE) ,M_UO_CODE" +
                                             "                 " +
                                             "                 ) a" +
                                             "   GROUP BY A.M_UO_CODE" +
                                             ") MAINQ" +
                                             " INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_UMRA_OPERATOR ON MAINQ.COUNTM = UO_CODE   order  by 34 desc )" +
                                             " union all " +
                                             " select null as \"رقم الشركة \", null as \"الشركة\", sum(s.\"1\")  AS \"1\", sum(s.\"2\") AS \"2\", sum(s.\"3\") AS \"3\" , sum(s.\"4\") AS \"4\" ,sum(s.\"5\")  AS \"5\", sum(s.\"6\") AS \"6\", sum(s.\"7\") AS \"7\" , sum(s.\"8\") AS \"8\",sum(s.\"9\")  AS \"9\", sum(s.\"10\") AS \"10\", sum(s.\"11\") AS \"11\" , sum(s.\"12\") AS \"12\" ,sum(s.\"13\")  AS \"13\", sum(s.\"14\") AS \"14\", sum(s.\"15\") AS \"15\" , sum(s.\"16\") AS \"16\"," +
                                             " sum(s.\"17\")  AS \"17\", sum(s.\"18\") AS \"18\", sum(s.\"19\") AS \"19\" , sum(s.\"20\") AS \"20\" ,sum(s.\"21\")  AS \"21\", sum(s.\"22\") AS \"22\", sum(s.\"23\") AS \"23\" , sum(s.\"24\") AS \"24\",sum(s.\"25\")  AS \"25\", sum(s.\"26\") AS \"26\", sum(s.\"27\") AS \"27\" , sum(s.\"28\") AS \"28\" ,sum(s.\"29\")  AS \"29\", sum(s.\"30\") AS \"30\"" +
                                             " , sum(s.\"  اجمالي 30 يوم\") as \"  اجمالي 30 يوم\" ,  sum( s.\" اجمالي الموفا\") as \" اجمالي الموفا\"" +
                                             " from" +
                                             "(" +
                                             "SELECT" +
                                             " MAINQ.COUNTM AS \"رقم الشركة \" , UO_NAME_AR  AS \"الشركة\"     ,DAY1 AS \"1\",DAY2 AS \"2\",DAY3  AS \"3\" ,DAY4 AS \"4\" ,DAY5  AS \"5\",DAY6  AS \"6\",DAY7  AS \"7\",DAY8  AS \"8\",DAY9  AS \"9\",DAY10  AS \"10\",DAY11  AS \"11\",DAY12  AS \"12\",DAY13  AS \"13\",DAY14  AS \"14\",DAY15  AS \"15\",DAY16  AS \"16\",DAY17  AS \"17\",DAY18  AS \"18\"," +
                                             "        DAY19  AS \"19\",DAY20  AS \"20\",DAY21  AS \"21\",DAY22  AS \"22\",DAY23  AS \"23\",DAY24  AS \"24\",DAY25  AS \"25\",DAY26  AS \"26\",DAY27  AS \"27\",DAY28  AS \"28\",DAY29  AS \"29\",DAY30  AS \"30\"," +
                                             "       (DAY1+DAY2+DAY3+DAY4+DAY5+DAY6+DAY7+DAY8+DAY9+DAY10+DAY11+DAY12+DAY13+DAY14+DAY15+DAY16+DAY17+DAY18+DAY19+DAY20+DAY21+DAY22+DAY23+DAY24+DAY25+DAY26+DAY27+DAY28+DAY29+DAY30)  AS \"  اجمالي 30 يوم\", TOTAL_COUNT AS\" اجمالي الموفا\"" +
                                             " FROM " +
                                             "(" +
                                             "  SELECT  " +
                                             "         A.M_UO_CODE  AS COUNTM ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('01/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END) DAY1 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('02/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY2 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('03/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END) DAY3 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('04/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY4 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('05/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY5," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('06/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY6 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('07/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END) DAY7 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('08/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY8 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('09/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY9 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('10/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY10 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('11/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY11 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('12/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY12 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('13/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY13 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('14/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY14 ," +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('15/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY15 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('16/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY16 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('17/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY17 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('18/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY18 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('19/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY19 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('20/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY20 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('21/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY21 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('22/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY22 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('23/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY23 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('24/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY24 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('25/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY25 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('26/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY26 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('27/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY27 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('28/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY28 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('29/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY29 ,    " +
                                             "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('30/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY30" +
                                             "      ,             SUM(A.R_COUNT) AS TOTAL_COUNT  " +
                                             "      FROM " +
                                             "                 (" +
                                             "                     SELECT  COUNT(*) R_COUNT ,TRUNC(GR_MOFA_DATE) R_DATE, M_UO_CODE " +
                                             "                               FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_MUTAMER_DATA " +
                                             "                               INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS ON GR_CODE = M_MUTAMER_GROUP" +
                                             "                               WHERE M_MOFA_APPROVAL_NO IS NOT NULL " +
                                             "                               GROUP BY  TRUNC(GR_MOFA_DATE) ,M_UO_CODE" +
                                             "                 " +
                                             "                 ) a" +
                                             "   GROUP BY A.M_UO_CODE" +
                                             " ) MAINQ " +
                                             " INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_UMRA_OPERATOR ON MAINQ.COUNTM = UO_CODE" +
                                             " ) S";



        public static string Q_DRC { get; } = "select * from  ( SELECT" +
                                              " MAINQ.COUNTM AS \"رقم الدولة \" , CNTRY_NAME_AR  AS \"الدولة\"     ,DAY1 AS \"1\",DAY2 AS \"2\",DAY3  AS \"3\" ,DAY4 AS \"4\" ,DAY5  AS \"5\",DAY6  AS \"6\",DAY7  AS \"7\",DAY8  AS \"8\",DAY9  AS \"9\",DAY10  AS \"10\",DAY11  AS \"11\",DAY12  AS \"12\",DAY13  AS \"13\",DAY14  AS \"14\",DAY15  AS \"15\",DAY16  AS \"16\",DAY17  AS \"17\",DAY18  AS \"18\"," +
                                              "        DAY19  AS \"19\",DAY20  AS \"20\",DAY21  AS \"21\",DAY22  AS \"22\",DAY23  AS \"23\",DAY24  AS \"24\",DAY25  AS \"25\",DAY26  AS \"26\",DAY27  AS \"27\",DAY28  AS \"28\",DAY29  AS \"29\",DAY30  AS \"30\"," +
                                              "       (DAY1+DAY2+DAY3+DAY4+DAY5+DAY6+DAY7+DAY8+DAY9+DAY10+DAY11+DAY12+DAY13+DAY14+DAY15+DAY16+DAY17+DAY18+DAY19+DAY20+DAY21+DAY22+DAY23+DAY24+DAY25+DAY26+DAY27+DAY28+DAY29+DAY30)  AS \"  اجمالي 30 يوم\", TOTAL_COUNT AS\" اجمالي الموفا\"" +
                                              " FROM " +
                                              "(" +
                                              "  SELECT  " +
                                              "         A.GR_FROM_COUNTRY_ID  AS COUNTM ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('01/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END) DAY1 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('02/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY2 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('03/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END) DAY3 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('04/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY4 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('05/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY5," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('06/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY6 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('07/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END) DAY7 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('08/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY8 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('09/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY9 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('10/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY10 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('11/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY11 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('12/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY12 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('13/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY13 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('14/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY14 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('15/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY15 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('16/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY16 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('17/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY17 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('18/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY18 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('19/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY19 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('20/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY20 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('21/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY21 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('22/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY22 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('23/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY23 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('24/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY24 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('25/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY25 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('26/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY26 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('27/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY27 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('28/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY28 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('29/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY29 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('30/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY30" +
                                              "      ,             SUM(A.R_COUNT) AS TOTAL_COUNT  " +
                                              "      FROM " +
                                              "                 (" +
                                              "                     SELECT  COUNT(*) R_COUNT ,TRUNC(GR_MOFA_DATE) R_DATE, GR_FROM_COUNTRY_ID " +
                                              "                               FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_MUTAMER_DATA " +
                                              "                               INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS ON GR_CODE = M_MUTAMER_GROUP" +
                                              "                               WHERE M_MOFA_APPROVAL_NO IS NOT NULL " +
                                              "                               GROUP BY  TRUNC(GR_MOFA_DATE) ,GR_FROM_COUNTRY_ID" +
                                              "                 " +
                                              "                 ) a" +
                                              "   GROUP BY A.GR_FROM_COUNTRY_ID" +
                                              ") MAINQ" +
                                              " INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES ON MAINQ.COUNTM = CNTRY_ID order  by 34 desc )" +
                                              " union all" +
                                              " select null as \"رقم الدولة \", null as \"الدولة\", sum(s.\"1\")  AS \"1\", sum(s.\"2\") AS \"2\", sum(s.\"3\") AS \"3\" , sum(s.\"4\") AS \"4\" ,sum(s.\"5\")  AS \"5\", sum(s.\"6\") AS \"6\", sum(s.\"7\") AS \"7\" , sum(s.\"8\") AS \"8\",sum(s.\"9\")  AS \"9\", sum(s.\"10\") AS \"10\", sum(s.\"11\") AS \"11\" , sum(s.\"12\") AS \"12\" ,sum(s.\"13\")  AS \"13\", sum(s.\"14\") AS \"14\", sum(s.\"15\") AS \"15\" , sum(s.\"16\") AS \"16\"," +
                                              " sum(s.\"17\")  AS \"17\", sum(s.\"18\") AS \"18\", sum(s.\"19\") AS \"19\" , sum(s.\"20\") AS \"20\" ,sum(s.\"21\")  AS \"21\", sum(s.\"22\") AS \"22\", sum(s.\"23\") AS \"23\" , sum(s.\"24\") AS \"24\",sum(s.\"25\")  AS \"25\", sum(s.\"26\") AS \"26\", sum(s.\"27\") AS \"27\" , sum(s.\"28\") AS \"28\" ,sum(s.\"29\")  AS \"29\", sum(s.\"30\") AS \"30\"" +
                                              " , sum(s.\"  اجمالي 30 يوم\") as \"  اجمالي 30 يوم\" ,  sum( s.\" اجمالي الموفا\") as \" اجمالي الموفا\"" +
                                              " from" +
                                              "(" +
                                              "SELECT" +
                                              " MAINQ.COUNTM AS \"رقم الدولة \" , CNTRY_NAME_AR  AS \"الدولة\"     ,DAY1 AS \"1\",DAY2 AS \"2\",DAY3  AS \"3\" ,DAY4 AS \"4\" ,DAY5  AS \"5\",DAY6  AS \"6\",DAY7  AS \"7\",DAY8  AS \"8\",DAY9  AS \"9\",DAY10  AS \"10\",DAY11  AS \"11\",DAY12  AS \"12\",DAY13  AS \"13\",DAY14  AS \"14\",DAY15  AS \"15\",DAY16  AS \"16\",DAY17  AS \"17\",DAY18  AS \"18\"," +
                                              "        DAY19  AS \"19\",DAY20  AS \"20\",DAY21  AS \"21\",DAY22  AS \"22\",DAY23  AS \"23\",DAY24  AS \"24\",DAY25  AS \"25\",DAY26  AS \"26\",DAY27  AS \"27\",DAY28  AS \"28\",DAY29  AS \"29\",DAY30  AS \"30\"," +
                                              "       (DAY1+DAY2+DAY3+DAY4+DAY5+DAY6+DAY7+DAY8+DAY9+DAY10+DAY11+DAY12+DAY13+DAY14+DAY15+DAY16+DAY17+DAY18+DAY19+DAY20+DAY21+DAY22+DAY23+DAY24+DAY25+DAY26+DAY27+DAY28+DAY29+DAY30)  AS \"  اجمالي 30 يوم\", TOTAL_COUNT AS\" اجمالي الموفا\"" +
                                              " FROM " +
                                              "(" +
                                              "  SELECT  " +
                                              "         A.GR_FROM_COUNTRY_ID  AS COUNTM ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('01/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END) DAY1 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('02/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY2 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('03/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END) DAY3 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('04/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY4 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('05/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY5," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('06/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY6 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('07/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END) DAY7 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('08/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY8 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('09/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY9 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('10/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY10 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('11/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)   DAY11 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('12/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END)  DAY12 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('13/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY13 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('14/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY14 ," +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('15/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY15 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('16/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY16 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('17/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY17 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('18/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY18 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('19/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY19 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('20/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY20 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('21/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY21 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('22/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY22 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('23/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY23 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('24/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY24 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('25/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY25 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('26/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY26 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('27/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY27 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('28/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY28 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('29/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY29 ,    " +
                                              "     SUM (CASE   WHEN  (TRUNC(A.R_DATE) =  TRUNC(UMRA1441.TO_GDATE('30/'|| UMRA1441.TO_HIJRI(SYSDATE,'MM') ||'/1441', 'DD/MM/YYYY'))  )    THEN    R_COUNT ELSE 0 END )    DAY30" +
                                              "      ,             SUM(A.R_COUNT) AS TOTAL_COUNT  " +
                                              "      FROM " +
                                              "                 (" +
                                              "                     SELECT  COUNT(*) R_COUNT ,TRUNC(GR_MOFA_DATE) R_DATE, GR_FROM_COUNTRY_ID " +
                                              "                               FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_MUTAMER_DATA " +
                                              "                               INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS ON GR_CODE = M_MUTAMER_GROUP" +
                                              "                               WHERE M_MOFA_APPROVAL_NO IS NOT NULL " +
                                              "                               GROUP BY  TRUNC(GR_MOFA_DATE) ,GR_FROM_COUNTRY_ID" +
                                              "                 " +
                                              "                 ) a" +
                                              "   GROUP BY A.GR_FROM_COUNTRY_ID" +
                                              " ) MAINQ " +
                                              " INNER JOIN " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES ON MAINQ.COUNTM = CNTRY_ID" +
                                              " ) S";

        // BuildMyString.com generated code. Please enjoy your string responsibly.

        public static string Q_UOS_PERFORMANCE { get; } = " select * from ( SELECT  EN.UO_CODE AS \"رقم الشركة\",UO_NAME_AR as \"اسم الشركة\"," +
                                                          "             NVL(A.TOTAL_MOFA_PREVIOUS, 0) AS \"الموسم السابق\"," +
                                                          "             NVL(B.TOTAL_MOFA_CURRENT, 0) AS \"الموسم الحالي\"," +
                                                          "             (NVL(B.TOTAL_MOFA_CURRENT, 0) - NVL(A.TOTAL_MOFA_PREVIOUS, 0)) AS \"زيادة\\نقص\"," +
                                                          "       NVL(      ROUND(((NVL(B.TOTAL_MOFA_CURRENT, 0) - NVL(A.TOTAL_MOFA_PREVIOUS, 0)) / A.TOTAL_MOFA_PREVIOUS) * 100, 0)  ,0)   || '%' AS \"نسبة النمو\"" +
                                                          "     FROM ( SELECT   M_UO_CODE, COUNT(*) AS TOTAL_MOFA_PREVIOUS" +
                                                          "                FROM    " + ConfigurationManager.AppSettings["previous_year"] + ".YAHAJJ_MUTAMER_DATA YMD, " +
                                                          " " + ConfigurationManager.AppSettings["previous_year"] + ".BAU_MUTAMER_GROUPS YMF" +
                                                          "              WHERE  YMD.M_MUTAMER_GROUP = GR_CODE" +
                                                          "              AND M_MOFA_APPROVAL_NO IS NOT NULL" +
                                                          "              AND M_UO_CODE ! = 888" +
                                                          "                 AND (TRUNC(GR_MOFA_DATE) <=     TRUNC(TO_DATE( CONCAT(TO_CHAR(SYSDATE,  'DD/MM',  'NLS_CALENDAR=''ARABIC HIJRAH'''  ) ,'/1440'), 'dd/mm/yyyy','nls_calendar=''English Hijrah'''))  )" +
                                                          "          GROUP BY M_UO_CODE   ) A,  " +
                                                          "         (     SELECT   M_UO_CODE, COUNT(*) AS TOTAL_MOFA_CURRENT" +
                                                          "                FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_MUTAMER_DATA YMD," + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS YMF      " +
                                                          "              WHERE YMD.M_MUTAMER_GROUP = GR_CODE" +
                                                          "               AND M_MOFA_APPROVAL_NO IS NOT NULL" +
                                                          "               AND M_UO_CODE !=888" +
                                                          "                  AND (TRUNC(GR_MOFA_DATE) <=   TRUNC(TO_DATE(    TO_CHAR(SYSDATE,  'DD/MM/YYYY ',   'NLS_CALENDAR=''ARABIC HIJRAH'''   ) , 'dd/mm/yyyy','nls_calendar=''English Hijrah'''))     )" +
                                                          "          GROUP BY M_UO_CODE  ) B  " +
                                                          "         ," + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_UMRA_OPERATOR EN" +
                                                          "     WHERE    A.M_UO_CODE(+) =  EN.UO_CODE" +
                                                          "     AND      B.M_UO_CODE(+) =  EN.UO_CODE" +
                                                          "     AND (A.TOTAL_MOFA_PREVIOUS IS NOT NULL OR B.TOTAL_MOFA_CURRENT IS NOT NULL) order by 4 desc )" +
                                                          "    UNION ALL " +
                                                          "      SELECT NULL  AS \"رقم الشركة\" , NULL AS  \"اسم الشركة\"," +
                                                          "                      SUM(NVL(A.TOTAL_MOFA_PREVIOUS, 0)) AS  \"الموسم السابق\"," +
                                                          "                      SUM(NVL(B.TOTAL_MOFA_CURRENT, 0)) AS \"الموسم الحالي\"," +
                                                          "                    (SUM(NVL(B.TOTAL_MOFA_CURRENT, 0)) - SUM(NVL(A.TOTAL_MOFA_PREVIOUS, 0))) AS \"زيادة\\نقص\" ," +
                                                          "                     ROUND(((SUM(B.TOTAL_MOFA_CURRENT) - SUM(A.TOTAL_MOFA_PREVIOUS)) / SUM(A.TOTAL_MOFA_PREVIOUS)) * 100, 0) || '%' AS \"نسبة النمو\"" +
                                                          "                  FROM (  SELECT COUNT(*) AS TOTAL_MOFA_PREVIOUS, CNTRY_NO" +
                                                          "                            FROM  " + ConfigurationManager.AppSettings["previous_year"] + ".YAHAJJ_MUTAMER_DATA YMD," +
                                                          "                                  " + ConfigurationManager.AppSettings["previous_year"] + ".BAU_MUTAMER_GROUPS YMF," +
                                                          "                                   " + ConfigurationManager.AppSettings["previous_year"] + ".YAHAJJ_EXTERNAL_AGENTS YEA, " +
                                                          "                                   " + ConfigurationManager.AppSettings["previous_year"] + ".V_LU_COUNTRIES" +
                                                          "                          WHERE    YMD.M_EA_CODE = YEA.EA_CODE" +
                                                          "                                 AND CNTRY_ID = EA_ORIGANL_COUNTRY" +
                                                          "                               AND YMD.M_MUTAMER_GROUP = GR_CODE" +
                                                          "                                  AND YMD.M_UO_CODE !=888" +
                                                          "                             AND M_MOFA_APPROVAL_NO IS NOT NULL" +
                                                          "                                 AND (TRUNC(GR_MOFA_DATE) <=     TRUNC(TO_DATE( CONCAT(TO_CHAR(SYSDATE,  'DD/MM',  'NLS_CALENDAR=''ARABIC HIJRAH'''  ) ,'/1440'), 'dd/mm/yyyy','nls_calendar=''English Hijrah'''))  )" +
                                                          "                   GROUP BY CNTRY_NO) A," +
                                                          "                  (  SELECT COUNT(*) AS TOTAL_MOFA_CURRENT, CNTRY_NO" +
                                                          "                        FROM  " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_MUTAMER_DATA YMD, " + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS YMF," +
                                                          "                             " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_EXTERNAL_AGENTS YEA, " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES" +
                                                          "                        WHERE                   " +
                                                          "                               YMD    .M_EA_CODE = YEA.EA_CODE" +
                                                          "                                AND CNTRY_ID = EA_ORIGANL_COUNTRY" +
                                                          "                               AND YMD.M_MUTAMER_GROUP = GR_CODE" +
                                                          "                                 AND YMD.M_UO_CODE !=888" +
                                                          "                                  AND M_MOFA_APPROVAL_NO IS NOT NULL" +
                                                          "                               AND (TRUNC(GR_MOFA_DATE) <=   TRUNC(TO_DATE(    TO_CHAR(SYSDATE,  'DD/MM/YYYY ',   'NLS_CALENDAR=''ARABIC HIJRAH'''   ) , 'dd/mm/yyyy','nls_calendar=''English Hijrah'''))     )" +
                                                          "                     GROUP BY CNTRY_NO) B," +
                                                          "                 " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES CN" +
                                                          "               WHERE      A.CNTRY_NO(+) = CN.CNTRY_NO" +
                                                          "                  AND B.CNTRY_NO(+) = CN.CNTRY_NO" +
                                                          "                   AND (A.TOTAL_MOFA_PREVIOUS IS NOT NULL OR B.TOTAL_MOFA_CURRENT IS NOT NULL)" +
                                                          "              ";



        public static string Q_C_PERFORMACE { get; } =
 "  select * from (   SELECT  a.GR_FROM_COUNTRY_ID  AS \"رمز الدولة\" ,cc.CNTRY_NAME_AR AS \"اسم الدولة\"," +
"             NVL(A.TOTAL_MOFA_PREVIOUS, 0) AS \"الموسم السابق\"," +
"             NVL(B.TOTAL_MOFA_CURRENT, 0) AS \"الموسم الحالي\"," +
"             (NVL(B.TOTAL_MOFA_CURRENT, 0) - NVL(A.TOTAL_MOFA_PREVIOUS, 0)) AS \"زيادة\\نقص\" ," +
"       NVL(      ROUND(((NVL(B.TOTAL_MOFA_CURRENT, 0) - NVL(A.TOTAL_MOFA_PREVIOUS, 0)) / A.TOTAL_MOFA_PREVIOUS) * 100, 0)  ,0)   || '%' AS \"نسبة النمو\"" +
"     FROM ( SELECT   GR_FROM_COUNTRY_ID, COUNT(*) AS TOTAL_MOFA_PREVIOUS" +
"                FROM    " + ConfigurationManager.AppSettings["previous_year"] + ".YAHAJJ_MUTAMER_DATA YMD,  " + ConfigurationManager.AppSettings["previous_year"] + ".BAU_MUTAMER_GROUPS YMF" +
"              WHERE  YMD.M_MUTAMER_GROUP = GR_CODE" +
"              AND M_MOFA_APPROVAL_NO IS NOT NULL" +
"              AND M_UO_CODE ! = 888" +
"             AND (TRUNC(GR_MOFA_DATE) <=     TRUNC(TO_DATE( CONCAT(TO_CHAR(SYSDATE,  'DD/MM',  'NLS_CALENDAR=''ARABIC HIJRAH'''  ) ,'/1440'), 'dd/mm/yyyy','nls_calendar=''English Hijrah'''))  )" +
"          GROUP BY GR_FROM_COUNTRY_ID   ) A" +
"                       LEFT OUTER JOIN" +
"         (     SELECT   GR_FROM_COUNTRY_ID, COUNT(*) AS TOTAL_MOFA_CURRENT" +
"                FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_MUTAMER_DATA YMD," + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS YMF                     " +
"              WHERE YMD.M_MUTAMER_GROUP = GR_CODE" +
"               AND M_MOFA_APPROVAL_NO IS NOT NULL" +
"               AND M_UO_CODE !=888" +
"                  AND (TRUNC(GR_MOFA_DATE) <=   TRUNC(TO_DATE(    TO_CHAR(SYSDATE,  'DD/MM/YYYY ',   'NLS_CALENDAR=''ARABIC HIJRAH'''   ) , 'dd/mm/yyyy','nls_calendar=''English Hijrah'''))     )" +
"          GROUP BY GR_FROM_COUNTRY_ID  ) B  " +
"             ON  A.GR_FROM_COUNTRY_ID =  B.GR_FROM_COUNTRY_ID" +
"    LEFT OUTER JOIN  " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES cc ON  cc.cntry_ID = A.GR_FROM_COUNTRY_ID" +
"     AND (A.TOTAL_MOFA_PREVIOUS IS NOT NULL OR B.TOTAL_MOFA_CURRENT IS NOT NULL)" +
            "   ORDER BY 4 DESC )" +
"    UNION  ALL" +
"    " +
"        SELECT  NULL  AS \"رمز الدولة\" ,NULL AS \"اسم الدولة\"," +
"                      SUM(NVL(A.TOTAL_MOFA_PREVIOUS, 0)) AS  \"الموسم السابق\"," +
"                      SUM(NVL(B.TOTAL_MOFA_CURRENT, 0)) AS \"الموسم الحالي\"," +
"                    (SUM(NVL(B.TOTAL_MOFA_CURRENT, 0)) - SUM(NVL(A.TOTAL_MOFA_PREVIOUS, 0))) AS \"زيادة\\نقص\" ," +
"       NVL(      ROUND(((NVL( SUM(B.TOTAL_MOFA_CURRENT), 0) - NVL( SUM(A.TOTAL_MOFA_PREVIOUS), 0)) / SUM(A.TOTAL_MOFA_PREVIOUS)) * 100, 0)  ,0)   || '%' AS \"نسبة النمو\"" +
"     FROM ( SELECT   GR_FROM_COUNTRY_ID, COUNT(*) AS TOTAL_MOFA_PREVIOUS" +
"                FROM    " + ConfigurationManager.AppSettings["previous_year"] + ".YAHAJJ_MUTAMER_DATA YMD,  " + ConfigurationManager.AppSettings["previous_year"] + ".BAU_MUTAMER_GROUPS YMF" +
"              WHERE  YMD.M_MUTAMER_GROUP = GR_CODE" +
"              AND M_MOFA_APPROVAL_NO IS NOT NULL" +
"              AND M_UO_CODE ! = 888" +
"             AND (TRUNC(GR_MOFA_DATE) <=     TRUNC(TO_DATE( CONCAT(TO_CHAR(SYSDATE,  'DD/MM',  'NLS_CALENDAR=''ARABIC HIJRAH'''  ) ,'/1440'), 'dd/mm/yyyy','nls_calendar=''English Hijrah'''))  )" +
"          GROUP BY GR_FROM_COUNTRY_ID   ) A" +
"             FULL OUTER JOIN" +
"         (     SELECT   GR_FROM_COUNTRY_ID, COUNT(*) AS TOTAL_MOFA_CURRENT" +
"                FROM " + ConfigurationManager.AppSettings["Schema_year"] + ".YAHAJJ_MUTAMER_DATA YMD," + ConfigurationManager.AppSettings["Schema_year"] + ".BAU_MUTAMER_GROUPS YMF                     " +
"              WHERE YMD.M_MUTAMER_GROUP = GR_CODE" +
"               AND M_MOFA_APPROVAL_NO IS NOT NULL" +
"               AND M_UO_CODE !=888" +
"                  AND (TRUNC(GR_MOFA_DATE) <=   TRUNC(TO_DATE(    TO_CHAR(SYSDATE,  'DD/MM/YYYY ',   'NLS_CALENDAR=''ARABIC HIJRAH'''   ) , 'dd/mm/yyyy','nls_calendar=''English Hijrah'''))     )" +
"          GROUP BY GR_FROM_COUNTRY_ID  ) B  " +
"          ON  A.GR_FROM_COUNTRY_ID =  B.GR_FROM_COUNTRY_ID" +
"          LEFT OUTER JOIN  " + ConfigurationManager.AppSettings["Schema_year"] + ".V_LU_COUNTRIES cc ON  cc.cntry_ID = A.GR_FROM_COUNTRY_ID" +
"     AND (A.TOTAL_MOFA_PREVIOUS IS NOT NULL OR B.TOTAL_MOFA_CURRENT IS NOT NULL)" +
"    "


            ;
    }
}
