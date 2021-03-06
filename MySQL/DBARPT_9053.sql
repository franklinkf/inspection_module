--TRUNCATE TABLE C_MISPRINT;
--TRUNCATE TABLE C_MISADV;
--TRUNCATE TABLE C_MISINPUT1;
--TRUNCATE TABLE C_MISINPUT1;
-- Changes related to temporary fix
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

    --'&&1' - SOLID

DECLARE

    VARERROR                    VARCHAR2 (1000);
    TEMPCOUNT                   NUMBER(10);
    TEMPCOUNT1                  NUMBER(10);

BEGIN


        SELECT COUNT(1)
          INTO TEMPCOUNT
          FROM C_BACOPEN;
          
        TEMPCOUNT1:= 639163 - TEMPCOUNT;  
        
        INSERT INTO C_MISADV (SOLID)
        SELECT SOLID2
          FROM C_MISONLINEDATE
         WHERE SOLID2 NOT IN (0,40101); 
        COMMIT;
        
        UPDATE C_MISADV SET NUMBER1 = (SELECT TO_NUMBER(SOLID||'100'||TO_CHAR(LINK_VAL + 1)) FROM C_REFCODE WHERE MAIN_CODE = 107 AND SUB_CODE = SOLID);
        UPDATE C_MISADV SET NUMBER2 = TO_NUMBER(SOLID||'100999999');
        UPDATE C_MISADV SET NUMBER1 = TO_NUMBER(SOLID||'100000001') WHERE NUMBER1 IS NULL;
        UPDATE C_MISADV SET NUMBER3 = NUMBER2 - NUMBER1+1;
        UPDATE C_MISADV SET NUMBER4 = (SELECT COUNT(1) FROM C_BACOPEN WHERE BAC_SOLID = SOLID);
        UPDATE C_MISADV SET NUMBER5 = NUMBER3 - NUMBER4;
        COMMIT;

        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 1, 1, RPAD(' ',(140-LENGTH(PKGCOMMON.BANKNAME))/2,' ')||PKGCOMMON.BANKNAME FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 2, 1, RPAD(' ',140) FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 3, 1, RPAD(' ',(140-LENGTH('BACOPEN - Customer ID and Account Number Vacant Status'))/2,' ')||'BACOPEN - Customer ID and Account Number Vacant Status' FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 5, 1, RPAD(' ',140) FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 1, RPAD('-',140,'-') FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 2, 'Customer ID Number Status:' FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 3, RPAD('-',140,'-') FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 5, 'Allotted Number From                : 409360837' FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 6, 'Allotted Number To                  : 410000000' FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 7, 'Total Number                        : 639163' FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 8, 'Of which, used                      : '||TEMPCOUNT FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 9, 'Balance                             : '||TEMPCOUNT1 FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 10, RPAD('-',140,'-') FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 11, 'Account Number Status' FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 15, RPAD('-',140,'-') FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 16, 'SOLID  SOL Name                                         Start Number        End Number             Total     Of which used           Balance' FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 6, 17, RPAD('-',140,'-') FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) 
        SELECT 7, ROWNUM,
               RPAD(SOLID,5)||'  '||
               RPAD(SOLNAME,43)||'  '||
               LPAD(NUMBER1,16)||'  '||
               LPAD(NUMBER2,16)||'  '||
               LPAD(NUMBER3,16)||'  '||
               LPAD(NUMBER4,16)||'  '||
               LPAD(NUMBER5,16)
          FROM (SELECT SOLID,PKGSMGBCOMMON.SOLNAME(SOLID) SOLNAME,NUMBER1,NUMBER2,NUMBER3,NUMBER4,NUMBER5 FROM C_MISADV ORDER BY SOLID);
        COMMIT;

        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 24, 1, RPAD('-',140,'-') FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 10000003, 1, RPAD(' ',140) FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 10000004, 1, RPAD(' ',140) FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 10000005, 1, RPAD(' ',140) FROM DUAL;
        INSERT INTO C_MISPRINT (SERIALNO, SUBSERIALNO, REPORTDATA) SELECT 10000006, 1, 'Date:'||RPAD(' ',(140/2)-11)||'Signatory II'||RPAD(' ',(140/2)-17)||'Signatory I' FROM DUAL;
        COMMIT;

EXCEPTION
WHEN OTHERS THEN
VARERROR := VARERROR || ' -- '|| UPPER(SQLERRM);
INSERT INTO C_MISPRINT (REPORTDATA) VALUES (VARERROR);

END;
/

set pages 0 colsep | lines 10000 trims on

spool &&24
select reportdata from c_misprint order by serialno, subserialno;
spool off

/
exit
