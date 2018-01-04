--    TRUNCATE TABLE C_MISPRINT;
--    TRUNCATE TABLE C_MISINPUT1;
--    COMMIT;

--=============================================================================
--NAME          : SMGBFIRST_ACCT.sql
--AUTHOR        : 
--DATE          : 01-11-2011
--DESC          : SMGB FIRST ACCOUNT WISE REPORT 
--=============================================================================

    SET SERVEROUTPUT ON SIZE 1000000
    SET HEAD OFF ECHO OFF TRIMS ON VERIFY OFF FEEDBACK OFF
    SET TIMING OFF LINESIZE 1000 EMBEDDED OFF PAGES 0 termout off
    spool n.lst

--=================================================================================================================================
-- Inserting record into Input table(C_MISINPUT1)
--=================================================================================================================================

    INSERT INTO C_MISINPUT1 (FIELD01,FIELD02,FIELD03,FIELD04,FIELD05,FIELD06,FIELD07,FIELD08,FIELD09,FIELD10,FIELD11,FIELD12,FIELD13,FIELD14,FIELD15,FIELD16,FIELD17,FIELD18,FIELD19,FIELD20,USERID,  USERSOLID ,BODDATE) 
    VALUES ('&&1','&&2','&&3','&&4','&&5','&&6','&&7','&&8','&&9','&&10','&&11','&&12','&&13','&&14','&&15','&&16','&&17','&&18','&&19','&&20','&&21','&&22','&&23');
    COMMIT;

    --'&&1' -   Statement ID From - 1
    --'&&2' -   Statement ID To   - 999
    --'&&3' -   Statement Name Like - All/Specific
    
--=====================================================================================================================================================
-- Data processing 
--=====================================================================================================================================================

    DECLARE
    
        GSTATEMENTIDFROM            VARCHAR2(100):= '&&1';
        GSTATEMENTIDTO              VARCHAR2(100):= '&&2';
        GSTATEMENTNAME              VARCHAR2(100):= '&&3';
                  
    BEGIN

        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 1, 1, RPAD(' ',(496-LENGTH(PKGCOMMON.BANKNAME))/2,' ')||PKGCOMMON.BANKNAME FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 2, 1, RPAD(' ',496) FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 3, 1, RPAD(' ',(496-LENGTH('DBARPT - 9020: REPORT ON CHECKLIST PARAMETER TABLE'))/2,' ')||'DBARPT - 9020: REPORT ON CHECKLIST PARAMETER TABLE' FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 5, 1, RPAD(' ',496) FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 1, RPAD('-',496,'-') FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 2, 'Parameters:' FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 3, RPAD('-',496,'-') FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 5, 'Statement ID From                   : '||GSTATEMENTIDFROM FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 6, 'Statement ID To                     : '||GSTATEMENTIDTO FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 7, 'Statement Name Like                 : '||GSTATEMENTNAME FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 10, RPAD('-',496,'-') FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 11, 'SLNo  Stmt  Statement Name                                                          Start Date  End Date    Frequency   Printer Settings  Remarks 1                                                               Remarks 2                                                               Remarks 3                                                               Remarks 4                                                               Remarks 5' FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 15, RPAD('-',496,'-') FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA)
        SELECT 7,ROWNUM,
               LPAD(ROWNUM,4)||'  '||
               LPAD(CL_STATEMENTID,4)||'  '||
               RPAD(CL_STATEMENTNAME,70)||'  '||
               TO_CHAR(CL_STARTDATE,'DD-MM-YYYY')||'  '||
               TO_CHAR(CL_ENDDATE,'DD-MM-YYYY')||'  '||
               RPAD(CL_FREQUENCY,10)||'  '||
               RPAD(CL_PRINTSETTINGS,16)||'  '||
               RPAD(CL_REMARKS1,70)||'  '||
               RPAD(CL_REMARKS2,70)||'  '||
               RPAD(CL_REMARKS3,70)||'  '||
               RPAD(CL_REMARKS4,70)||'  '||
               RPAD(CL_REMARKS5,70)          
          FROM (                
        SELECT CL_STATEMENTID,CL_STATEMENTNAME,CL_STARTDATE,CL_ENDDATE,CL_FREQUENCY,CL_PRINTSETTINGS,CL_REMARKS1,CL_REMARKS2,CL_REMARKS3,CL_REMARKS4,CL_REMARKS5
          FROM C_MISCHECKLIST
         WHERE CL_STATEMENTID BETWEEN GSTATEMENTIDFROM AND GSTATEMENTIDTO  
           AND UPPER(DECODE(GSTATEMENTNAME,'ALL','1',CL_STATEMENTNAME)) LIKE '%'||UPPER(DECODE(GSTATEMENTNAME,'ALL','1',GSTATEMENTNAME)||'%')
           ORDER BY 1); 

        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 24, 1, RPAD('-',496,'-') FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 10000003, 1, RPAD(' ',496) FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 10000004, 1, RPAD(' ',496) FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 10000005, 1, RPAD(' ',496) FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 10000006, 1, 'Date:'||RPAD(' ',(496/2)-11)||'Signatory II'||RPAD(' ',(496/2)-17)||'Signatory I' FROM DUAL;
        COMMIT;

    END;
    /

set pages 0 colsep | lines 10000 trims on

spool &&24
select reportdata from c_misprint order by serialno, subserialno;
spool off

/
exit
