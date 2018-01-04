Imports basevb
Imports ins_prop
Imports ins_bus
Imports contracts
Public Class dashboard_DD
    Inherits basevb.basevb
    Dim objProp As New LetterApp.Common_prop
    Dim objBus As New LetterApp.LetterApp
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objBus.Url = System.Configuration.ConfigurationManager.AppSettings("URL")
        If IsPostBack = False Then
            If fnCheckTime(CType(Master.PageLoadTime, String)) = "F" Then
                Response.Redirect("Session.aspx")
                Exit Sub
            End If
            If Master.menuname <> "" Then
                Master.ScreenHeading = Master.menuname
            Else
                Master.ScreenHeading = "DASHBOARD"
            End If
            objProp.strMenuAccessType = Session("OPT")
            lbtest.Text = objProp.strMenuAccessType
            txtMaxCnt.Text = lbtest.Text.Split("|")(2)
            gridmethod()
            fill_dashboard()
        End If
    End Sub
    Protected Sub fill_dashboard()
        objProp.strSolID = Master.SOLID
        objProp.strUserID = Master.UserID
        objProp.Text1 = "10000"
        objProp.Text2 = "1"
        objProp.Text3 = lbtest.Text.Split("|")(0)
        objProp.Text4 = lbtest.Text.Split("|")(1)
        objProp = objBus.ifnGetDashboard_DD(objProp)
        RepeaterDB.Visible = True
        RepeaterDB.DataSource = Nothing
        RepeaterDB.DataSource = objProp.dtSearchData
        RepeaterDB.DataBind()
    End Sub
    Protected Sub subCallDDepExpo(ByVal sender As Object, ByVal e As System.EventArgs)
        objProp.strSolID = Master.SOLID
        objProp.strUserID = Master.UserID
        objProp.SessionID = Master.SessionID
        objProp.Text1 = "10000"
        objProp.Text2 = "1"
        objProp.Text3 = lbtest.Text.Split("|")(0)
        objProp.Text4 = lbtest.Text.Split("|")(1)
        If objProp.Text3 = "21" Then
            objProp.Text5 = 3083
            Session("menuid") = "3083"
            Session("menuname") = "LETTER MASTER"
        Else
            objProp.Text5 = 3082
            Session("menuid") = "3082"
            Session("menuname") = "LETTER ACTION"
        End If
        objProp = objBus.ifnPopulate_Navigation_Entries(objProp)
        Session("LETTER") = CType(sender, LinkButton).CommandArgument + "|" + objProp.Text6
        'Session("menuid") = "3082"
        'Session("menuname") = "LETTER ACTION"
        Session("sessiontime") = Format(System.DateTime.Now, "yyyyMMddHHmmss")
        If objProp.Text3 = 21 Then
            Response.Redirect("intra_bank_letter_master.aspx")
        Else
            Response.Redirect("letter_action.aspx")
        End If

    End Sub
    Protected Sub ibtnBack_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnBack.Click
        Session("menuname") = "DASHBOARD"
        Session("sessiontime") = Format(System.DateTime.Now, "yyyyMMddHHmmss")
        Response.Redirect("dashboard.aspx")
    End Sub
    Public Sub gridmethod()
        If txtMaxCnt.Text = 0 Then
            lbtnOld.Visible = False
            lbtnNow.Visible = False
            lbtnNew.Visible = False
        ElseIf txtMaxCnt.Text <= 20 Then
            lbtnNow.Text = "1-" + txtMaxCnt.Text + " of " + txtMaxCnt.Text
            lbtnOld.Visible = False
            lbtnNew.Visible = False
            lbtnNow.Visible = True
        Else
            lbtnNow.Text = "1-20 of " + txtMaxCnt.Text
            lbtnNew.Visible = False
            lbtnOld.Visible = True
            lbtnNow.Visible = True
        End If
        txtstartcount.Text = 0
        objProp.strSearchStartCounter = 0
        objProp.strSearchNoOfRecords = 20
    End Sub
    Protected Sub lbtnOld_Click(sender As Object, e As EventArgs) Handles lbtnOld.Click
        Dim sno As Int32 = txtstartcount.Text
        Dim tno As Int32 = txtMaxCnt.Text
        sno = sno + 20
        lbtnNew.Visible = True
        If tno <= sno + 20 Then
            lbtnOld.Visible = False
        Else
            tno = sno + 20
        End If
        txtstartcount.Text = sno
        objProp.strSearchStartCounter = sno
        lbtnNow.Text = (sno + 1).ToString + "-" + tno.ToString + " of " + txtMaxCnt.Text
        objProp.strSearchNoOfRecords = 20
        fill_dashboard()
    End Sub
    Protected Sub lbtnNew_Click(sender As Object, e As EventArgs) Handles lbtnNew.Click
        Dim sno As Int32 = txtstartcount.Text
        Dim tno As Int32 = 0
        sno = sno - 20
        lbtnOld.Visible = True
        If tno >= sno Then
            lbtnNew.Visible = False
        End If
        tno = sno + 20
        lbtnNow.Text = (sno + 1).ToString + "-" + tno.ToString + " of " + txtMaxCnt.Text
        txtstartcount.Text = sno
        objProp.strSearchStartCounter = sno
        objProp.strSearchNoOfRecords = 20
        fill_dashboard()
    End Sub
End Class