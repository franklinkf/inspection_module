Imports basevb
Imports ins_prop
Imports ins_bus
Imports contracts
Imports System.IO
Imports System.IO.Compression
Imports WinSCP

Partial Public Class upload
    Inherits basevb.basevb
    Dim objProp As New LetterApp.upload_prop
    Dim objBus As New LetterApp.LetterApp
    Dim ftppwd As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        objBus.Url = System.Configuration.ConfigurationManager.AppSettings("URL")
        ftppwd = System.Configuration.ConfigurationManager.AppSettings("FTP")
        If IsPostBack = False Then
            If fnCheckTime(CType(Master.PageLoadTime, String)) = "F" Then
                Response.Redirect("Session.aspx")
                Exit Sub
            End If
            Master.ScreenHeading = Master.menuname
            initialize_Controlls()
            populate_mode()
            txtUploadDataDesc.Enabled = False
            'AutoFill()
        End If
    End Sub
    Protected Sub fetch_upload_path()
        objProp = objBus.ifnGetUploadPathUPD(objProp)
        objProp.filepath = objProp.filepath & Master.UserID & "/"
    End Sub
    Protected Sub initialize_Controlls()
        enablemastercontrolls()
        disablechildcontrolls()
        ddlMode.SelectedValue = "0"
        txtDataID.Text = ""
        txtUploadDataDesc.Text = ""
        txtUploadID.Text = ""
        'ddlSOLID.Text = ""
        ddlSOLID.Items.Clear()
        'ddlInspectionID.Text = ""
        txtdate.Text = ""
        lblWhatNext.Attributes.Add("style", "display: none;")
        lbtnWhatNext.Visible = False
        'Repeater2.DataSource = Nothing
    End Sub
    Private Sub enablemastercontrolls()
        ddlMode.Enabled = True
        txtDataID.Enabled = True
        imgSearchDataID.Enabled = True
        lbtnValidate.Enabled = True
    End Sub
    Private Sub disablemastercontrolls()
        ddlMode.Enabled = False
        txtDataID.Enabled = False
        imgSearchDataID.Enabled = False
        lbtnValidate.Enabled = False
    End Sub
    Private Sub enablechildcontrolls()
        ddlSOLID.Enabled = True
        ddlInspectionID.Enabled = True
        txtUploadID.Enabled = True
        txtdate.Enabled = True
        FileUpload1.Enabled = True
        imgUploadID.Enabled = True
        lbtnSubmit.Enabled = True
        lblWhatNext.Attributes.Add("style", "display: none;")
    End Sub
    Private Sub disablechildcontrolls()
        ddlSOLID.Enabled = False
        ddlInspectionID.Enabled = False
        txtUploadID.Enabled = False
        txtdate.Enabled = False
        FileUpload1.Enabled = False
        imgUploadID.Enabled = False
        lbtnSubmit.Enabled = False
        lblWhatNext.Attributes.Add("style", "display: none;")
    End Sub

    'Protected Sub AutoFill()
    '    If Session("Upload_WhatNext") <> "" Then
    '        Dim oMessage As String = Session("Upload_WhatNext")
    '        ddlMode.Text = delimitedtext(oMessage, "|", 1)
    '        ddlMode.SelectedValue = delimitedtext(oMessage, "|", 1)
    '        'objprop.strMode = delimitedtext(oMessage, "|", 1)

    '        txtDataID.Text = delimitedtext(oMessage, "|", 2)
    '        'objprop.dataid = delimitedtext(oMessage, "|", 2)

    '        subValidate()

    '        txtUploadID.Text = delimitedtext(oMessage, "|", 3)
    '        'objprop.uploadid = delimitedtext(oMessage, "|", 3)
    '        txtUploadDataDesc.Text = delimitedtext(oMessage, "|", 4)

    '        ddlSOLID.Text = delimitedtext(oMessage, "|", 5)
    '        txtdate.Text = delimitedtext(oMessage, "|", 6)
    '        Session("Upload_WhatNext") = ""
    '        'subSubmit()
    '        lbtnClear.Enabled = True

    '    End If
    'End Sub
    Public Sub populate_mode()
        ddlMode.Items.Insert(0, New ListItem("", "0"))
        ddlMode.Items.Insert(1, New ListItem("UPLOAD FILE", "UF"))
        ddlMode.Items.Insert(2, New ListItem("UPLOAD DATA", "UD"))
        ddlMode.Items.Insert(3, New ListItem("VALIDATE DATA", "VD"))
        ddlMode.Items.Insert(4, New ListItem("DELETE UPLOAD", "DU"))
        ddlMode.Items.Insert(5, New ListItem("VERIFY UPLOAD", "VU"))
    End Sub
    Protected Sub ibSearchDataID_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        txtSearchHead.Text = "UPLOADDATAID"
        lblSearchID.Text = "Data ID"
        lblSearchDesc.Text = "Description"
        objProp.strSearchContent = UCase(txtDataID.Text.ToString)
        objProp.strMode = ddlMode.SelectedValue
        txtDataID.Text = ""
        txtUploadDataDesc.Text = ""
        subMethod1()
        If objProp.intRecordCount = 0 Then
            lblRemarks.Text = "No records to display in search"
            Exit Sub
        End If
        subMethod2()
        If objProp.intRecordCount = 1 Then
            objProp = objBus.ifnGetSearchDataUPD(objProp)
            For Each row As DataRow In objProp.dtSearchData.Rows
                txtDataID.Text = row("SEARCHVALUE")
                txtUploadDataDesc.Text = row("SEARCHTEXT")
            Next
        Else
            subMethod3()
            subMethod0()
        End If
    End Sub
    Protected Sub ibUloadID_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgUploadID.Click
        If ddlMode.SelectedValue = "0" Then
            lblRemarks.Text = "Select Mode"
            ddlMode.Focus()
            Exit Sub
        End If
        If ddlMode.SelectedValue = "UF" Then
            lblRemarks.Text = "No value needs to be entered. System will generate Upload ID"
            ddlMode.Focus()
            Exit Sub
        End If
        If txtDataID.Text = "" Then
            lblRemarks.Text = "Select Data ID"
            ddlSOLID.Focus()
            Exit Sub
        End If
        txtSearchHead.Text = "UPLOADID"
        lblSearchID.Text = "Upload ID"
        lblSearchDesc.Text = "Uploaded By"
        objProp.strSearchContent = ""
        objProp.strMode = ddlMode.SelectedValue
        objProp.dataid = txtDataID.Text
        txtUploadID.Text = ""
        subMethod1()
        If objProp.intRecordCount = 0 Then
            lblRemarks.Text = "No records to display in search"
            Exit Sub
        End If
        subMethod2()
        subMethod3()
        subMethod0()
    End Sub

    Private Sub subSubmit()
        If ddlMode.SelectedValue = "UF" Then
            If txtUploadID.Text <> "" Then
                lblRemarks.Text = "Data ID will be autogenerated under Upload File mode"
                txtUploadID.Text = ""
                Exit Sub
            End If
        Else
            If txtUploadID.Text = "" Then
                lblRemarks.Text = "Enter Upload ID"
                txtUploadID.Text = ""
                Exit Sub
            End If
            If IsNumeric(txtUploadID.Text) = False Then
                lblRemarks.Text = "Enter valid Upload ID"
                txtUploadID.Text = ""
                Exit Sub
            End If
        End If
        objProp.strMode = ddlMode.SelectedValue
        objProp.dataid = txtDataID.Text
        If txtissolbased.Text = "Y" Then
            If ddlSOLID.Text = "" Then
                lblRemarks.Text = "Select upload SOLID"
                'ddlSOLID.Text = ""
                Exit Sub
            End If
            objProp.solid = Master.SOLID
        Else
            objProp.solid = ""
        End If
        If txtisinspbased.Text = "Y" Then
            objProp.inspid = 0 'Master.inspid
            If ddlInspectionID.Text = "" Then
                lblRemarks.Text = "Select upload Inspection ID"
                'ddlInspectionID.Text = ""
                Exit Sub
            End If
        Else
            objProp.inspid = 0 'Nothing
        End If
        If txtisdatebased.Text = "Y" Then
            If txtdate.Text = "" Then
                lblRemarks.Text = "Enter data date"
                txtdate.Focus()
                Exit Sub
            End If
            If fnIsvaliddate(txtdate.Text) = False Then
                lblRemarks.Text = "Enter valid date"
                txtdate.Focus()
                Exit Sub
            End If
            objProp.datadate = fnStringtodate(txtdate.Text)
            objProp.misctext = fnStringtoMySQLDate(txtdate.Text)
        Else
            objProp.datadate = Nothing
            objProp.misctext = "NULL"
        End If
        If txtUploadID.Text = "" Then
            objProp.uploadid = Nothing
        Else
            objProp.uploadid = txtUploadID.Text
        End If
        objProp.userid = Master.UserID
        objProp.sessionid = Master.SessionID
        If ddlMode.SelectedValue = "UF" Then
            If Not FileUpload1.HasFile Then
                lblRemarks.Text = "Select file for upload"
                FileUpload1.Focus()
                Exit Sub
            End If
        End If
        objProp.isdatebased = txtisdatebased.Text
        objProp.isinspidbased = txtisinspbased.Text
        objProp.issolidbased = txtissolbased.Text
        objProp.designation = Master.Designation
        objProp.roleid = Master.RoleID
        objProp.workclass = Master.WorkClass

        ' fields to set data for whatnext
        objProp.DataDesc = txtUploadDataDesc.Text
        objProp.DataDate1 = txtdate.Text

        fetch_upload_path()
        objProp = objBus.ifnSubmitUPD(objProp)

        If objProp.status = 1 Then

            If ddlMode.SelectedValue = "UF" Then
                objProp = objBus.ifnGetUploadIDUPD(objProp)
                uploadfile()

            ElseIf ddlMode.SelectedValue = "UD" Then
                objProp = objBus.ifnvalidate_udUPD(objProp)
                If objProp.status = 1 Then
                    Dim fpath = objProp.filepath & objProp.filename
                    objProp.strSearchContent = readNthLine(Replace(fpath, "\", "/"), 1)
                    objProp = objBus.ifnvalidatefirstlineUPD(objProp)
                    If objProp.status = 1 Then
                        objProp = objBus.ifnuploaddataUPD(objProp)
                        If objProp.status = 1 Then
                            lblRemarks.Text = objProp.message
                            'initialize_Controlls()
                        Else
                            lblRemarks.Text = objProp.message
                        End If
                    Else
                        lblRemarks.Text = objProp.message
                    End If
                Else
                    lblRemarks.Text = objProp.message
                End If
            ElseIf ddlMode.SelectedValue = "VD" Then
                objProp = objBus.ifnvalidate_vdUPD(objProp)
                If objProp.status = 1 Then
                    lblRemarks.Text = objProp.message
                    'initialize_Controlls()
                Else
                    lblRemarks.Text = objProp.message
                End If
            ElseIf ddlMode.SelectedValue = "VU" Then
                objProp = objBus.ifnvalidate_vuUPD(objProp)
                If objProp.status = 1 Then
                    lblRemarks.Text = objProp.message
                    'initialize_Controlls()
                Else
                    lblRemarks.Text = objProp.message
                End If
            ElseIf ddlMode.SelectedValue = "DU" Then
                lblRemarks.Text = objProp.message
                'initialize_Controlls()
            End If
        Else
            lblRemarks.Text = objProp.message
            disablemastercontrolls()
            enablechildcontrolls()
        End If
        'ORDER - mode, dataID, dataDesc,SOL, UpldID , upldDate, Status of Submit
        lbtnWhatNext.CommandArgument = ddlMode.SelectedValue + "|" + txtDataID.Text + "|" + txtUploadDataDesc.Text + "|" + ddlSOLID.Text + "|" + txtUploadID.Text + "|" + txtdate.Text + "|" + objProp.status.ToString
        If objProp.status = 1 Then
            initialize_Controlls()
        End If
        populate_whatnext()
    End Sub

    Protected Sub lbtnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSubmit.Click
        lblRemarks.Text = ""
        subSubmit()
        lbtnClear.Enabled = True
    End Sub

    Public Sub subMethod1()
        objProp.strSearchHead = UCase(txtSearchHead.Text)
        objProp.strMode = ddlMode.Text
        objProp.ModuleName = Master.ModuleName
        objProp.userid = Master.UserID
        objProp.workclass = Master.WorkClass
        objProp.roleid = Master.RoleID
        objProp.designation = Master.Designation
        objProp.solid = Master.SOLID
        objProp = objBus.ifnGetSearchRecordCountUPD(objProp)
    End Sub

    Public Sub subMethod2()
        txtTotalCount.Text = objProp.intRecordCount
        objProp.intSearchStartCounter = 0
        objProp.intSearchNoOfRecords = 10
        txtStartNo.Text = "1"
    End Sub

    Public Sub subMethod3()
        subFetchData()
        lbtnPresent.Text = "1-" & rptSearchContent.Items.Count & " of " & txtTotalCount.Text
        lbtnPrevious.Visible = False
        If rptSearchContent.Items.Count < 10 Then
            lbtnNext.Visible = False
        Else
            lbtnNext.Visible = True
        End If
    End Sub

    Protected Sub lbtnNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnNext.Click
        Dim intStartno As Integer = txtStartNo.Text
        Dim intTotalcount As Integer = txtTotalCount.Text
        intStartno = intStartno + 10

        objProp.strMode = ddlMode.SelectedValue
        objProp.strSearchContent = UCase(txtUploadID.Text.ToString)
        objProp.strSearchHead = txtSearchHead.Text
        objProp.intSearchStartCounter = intStartno - 1
        objProp.intSearchNoOfRecords = 10
        objProp.ModuleName = Master.ModuleName
        objProp.userid = Master.UserID
        objProp.workclass = Master.WorkClass
        objProp.roleid = Master.RoleID
        objProp.designation = Master.Designation
        objProp.solid = Master.SOLID
        objProp.dataid = txtDataID.Text

        subMethod00()
        txtStartNo.Text = intStartno
        subFetchData()
        If rptSearchContent.Items.Count + intStartno - 1 > txtTotalCount.Text Then
            lbtnPresent.Text = intStartno & "-" & txtTotalCount.Text & " of " & txtTotalCount.Text
        Else
            lbtnPresent.Text = intStartno & "-" & rptSearchContent.Items.Count + intStartno - 1 & " of " & txtTotalCount.Text
        End If

        If intStartno + 10 > txtTotalCount.Text Then
            lbtnNext.Visible = False
            lbtnPrevious.Visible = True
        Else
            lbtnPrevious.Visible = True
            lbtnNext.Visible = True
        End If
        subMethod0()
    End Sub

    Protected Sub lbtnPrevious_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnPrevious.Click
        Dim intStartno As Integer = txtStartNo.Text
        Dim intTotalcount As Integer = txtTotalCount.Text
        intStartno = intStartno - 10

        objProp.strMode = ddlMode.SelectedValue
        objProp.strSearchContent = UCase(txtUploadID.Text.ToString)
        objProp.strSearchHead = txtSearchHead.Text
        objProp.intSearchStartCounter = intStartno - 1
        objProp.intSearchNoOfRecords = 10
        objProp.ModuleName = Master.ModuleName
        objProp.userid = Master.UserID
        objProp.workclass = Master.WorkClass
        objProp.roleid = Master.RoleID
        objProp.designation = Master.Designation
        objProp.solid = Master.SOLID
        objProp.dataid = txtDataID.Text

        subMethod00()
        txtStartNo.Text = intStartno
        subFetchData()
        lbtnPresent.Text = intStartno & "-" & rptSearchContent.Items.Count + intStartno - 1 & " of " & txtTotalCount.Text
        If intStartno - 10 < 0 Then
            lbtnPrevious.Visible = False
            lbtnNext.Visible = True
        Else
            lbtnPrevious.Visible = True
            lbtnNext.Visible = True
        End If
        subMethod0()
    End Sub

    Public Sub subFetchData()
        rptSearchContent.DataSource = Nothing
        objProp = objBus.ifnGetSearchDataUPD(objProp)
        rptSearchContent.DataSource = objProp.dtSearchData
        rptSearchContent.DataBind()
    End Sub

    Dim strSearchValue As String
    Dim strSearchDesc As String

    Protected Sub Repeater1_ItemCommand(ByVal source As Object, ByVal e As RepeaterCommandEventArgs)
        Dim lbtnSearchValue As LinkButton = DirectCast(e.Item.FindControl("lbtnSearchValue"), LinkButton)
        Dim lblSearchDesc As Label = DirectCast(e.Item.FindControl("lblSearchDesc"), Label)
        strSearchValue = lbtnSearchValue.Text
        strSearchDesc = lblSearchDesc.Text
        subMethod()
        rptSearchContent.DataSource = Nothing
        lbtnPrevious.Visible = True
        lbtnNext.Visible = True
    End Sub

    Public Sub subMethod()
        If txtSearchHead.Text = "UPLOADDATAID" Then
            txtDataID.Text = strSearchValue
            txtUploadDataDesc.Text = strSearchDesc
        Else
            txtUploadID.Text = strSearchValue
        End If
    End Sub

    Public Sub subMethod0()
        mpeSearchUploadID.Show()
    End Sub

    Public Sub subMethod00()
        If txtSearchHead.Text = "UPLOADDATAID" Then
            objProp.strSearchContent = txtUploadID.Text
        Else
            objProp.strSearchContent = ""
        End If
    End Sub
    Protected Sub uploadfile()
        fetch_upload_path()
        Dim fpath As String = objProp.filepath
        If Not Directory.Exists(fpath) Then
            Directory.CreateDirectory(fpath)
        End If
        Try
            Dim filename As String = Path.GetFileName(FileUpload1.FileName)
            objProp.filename = filename
            FileUpload1.SaveAs(fpath & filename)
            'objprop.filepath = objprop.filepath & "/" & filename
            FtpUploadFile(Replace(fpath & filename, "/", "\"))
            objProp = objBus.ifnMarkUploadStatusUPD(objProp)
            Session("CUPLDID") = objProp.uploadid
            lblRemarks.Text = objProp.message
            'initialize_Controlls()
        Catch ex As Exception
            lblRemarks.Text = "File upload failed. " & ex.Message
        End Try
    End Sub

    Protected Sub lbtnValidate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnValidate.Click
        lblRemarks.Text = ""
        subValidate()
    End Sub

    Protected Sub subValidate()
        lblRemarks.Text = ""
        If ddlMode.SelectedValue = "0" Then
            lblRemarks.Text = "Select Mode"
            ddlMode.Focus()
            Exit Sub
        End If
        If txtDataID.Text = "" Then
            lblRemarks.Text = "Enter Data ID"
            txtDataID.Focus()
            Exit Sub
        End If
        If IsNumeric(txtDataID.Text) = False Then
            lblRemarks.Text = "Enter valid Upload Data ID"
            txtDataID.Focus()
            Exit Sub
        End If
        objProp.userid = Master.UserID
        objProp.sessionid = Master.SessionID
        objProp.strMode = ddlMode.SelectedValue
        objProp.dataid = txtDataID.Text
        objProp.ModuleName = Master.ModuleName
        objProp.workclass = Master.WorkClass
        objProp.designation = Master.Designation
        objProp.roleid = Master.RoleID
        objProp.solid = Master.SOLID

        objProp = objBus.ifnvalidateUPD(objProp)
        If objProp.status = 1 Then
            disablemastercontrolls()
            enablechildcontrolls()
            txtisdatebased.Text = objProp.isdatebased
            txtisinspbased.Text = "N" 'objprop.isinspidbased
            txtissolbased.Text = objProp.issolidbased
            txtUploadDataDesc.Text = objProp.DataDesc


            If objProp.isdatebased = "N" Then
                txtdate.Enabled = False
            End If
            If objProp.isinspidbased = "N" Then
                ddlInspectionID.Enabled = False
            Else
                ddlInspectionID.Items.Clear()
                'ddlInspectionID.Items.Add("")
                ddlInspectionID.Items.Add(0) 'Master.inspid
                'ddlInspectionID.Text = ""
            End If
            If objProp.issolidbased = "N" Then
                ddlSOLID.Enabled = False
            Else
                objProp.solid = Master.SOLID
                objProp = objBus.ifnGetSOLNameUPD(objProp)
                ddlSOLID.Items.Clear()
                'ddlSOLID.Items.Add("")
                ddlSOLID.Items.Add(objProp.SOLDesc)
                'ddlSOLID.Text = ""
            End If
            If objProp.strMode = "UF" Then
                txtUploadID.Enabled = False
            Else
                FileUpload1.Enabled = False
            End If
        Else
            lblRemarks.Text = objProp.message
            enablemastercontrolls()
        End If
    End Sub
    Protected Sub lbtnClear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnClear.Click
        lblRemarks.Text = ""
        initialize_Controlls()
    End Sub

    Protected Sub populate_whatnext()
        lbtnWhatNext.Visible = True
        Session("sessiontime") = Format(System.DateTime.Now, "yyyyMMddHHmmss")
        Dim var As String = lbtnWhatNext.CommandArgument
        'ORDER - mode, dataID, dataDesc,SOL, UpldID , upldDate, Status of Submit
        Dim mode As String = var.Split("|")(0)
        Dim dataID As String = var.Split("|")(1)
        Dim dataDesc As String = var.Split("|")(2)
        Dim SOL As String = var.Split("|")(3)
        Dim upldID As String = var.Split("|")(4)
        Dim upldDate As String = var.Split("|")(5)
        Dim stat As String = var.Split("|")(6)

        If mode = "UF" Then
            upldID = Session("CUPLDID")
            If stat = "1" Then
                lbtnWhatNext.Text = "Upload Data"
                lbtnWhatNext.CommandArgument = "UD|" + dataID + "|" + upldID + "|" + dataDesc + "|" + SOL + "|" + upldDate
            Else
                lbtnWhatNext.Text = "Delete Upload"
                lbtnWhatNext.CommandArgument = "DU|"
            End If

        ElseIf mode = "UD" Then

            If stat = "1" Then
                lbtnWhatNext.Text = "Validate Data"
                lbtnWhatNext.CommandArgument = "VD|" + dataID + "|" + upldID + "|" + dataDesc + "|" + SOL + "|" + upldDate
            Else
                lbtnWhatNext.Text = "Delete Upload"
                lbtnWhatNext.CommandArgument = "DU|" + dataID + "|" + upldID + "|" + dataDesc + "|" + SOL + "|" + upldDate
            End If

        ElseIf mode = "VD" Then

            If stat = "1" Then
                lbtnWhatNext.Text = "Verify Upload"
                lbtnWhatNext.CommandArgument = "VU|" + dataID + "|" + upldID + "|" + dataDesc + "|" + SOL + "|" + upldDate
            Else
                lbtnWhatNext.Text = "Generate Error Report"
                lbtnWhatNext.CommandArgument = "4199^"
            End If

        ElseIf mode = "VU" Then
            If stat = "1" Then
                lbtnWhatNext.Text = "Upload File"
                lbtnWhatNext.CommandArgument = "UF|"
            Else
                lbtnWhatNext.Text = "Delete Upload"
                lbtnWhatNext.CommandArgument = "DU|" + dataID + "|" + upldID + "|" + dataDesc + "|" + SOL + "|" + upldDate
            End If


        ElseIf mode = "DU" Then

            lbtnWhatNext.Text = "Upload File"
            lbtnWhatNext.CommandArgument = "UF|"

        End If

    End Sub
    Protected Sub lbtnWhatNext_Click(sender As Object, e As EventArgs) Handles lbtnWhatNext.Click

        initialize_Controlls()

        Dim oMessage As String = lbtnWhatNext.CommandArgument

        If oMessage = "4199^" Then
            ' navigate to reports menu

            Session("reportid") = "4199^"
            Session("menuname") = "Reports"
            Response.Redirect("reports.aspx")

        Else
            ' stay in the upload menu for further options

            ddlMode.Text = delimitedtext(oMessage, "|", 1)
            ddlMode.SelectedValue = delimitedtext(oMessage, "|", 1)
            txtDataID.Text = delimitedtext(oMessage, "|", 2)

            subValidate()

            txtUploadID.Text = delimitedtext(oMessage, "|", 3)
            txtUploadDataDesc.Text = delimitedtext(oMessage, "|", 4)
            ddlSOLID.Text = delimitedtext(oMessage, "|", 5)
            txtdate.Text = delimitedtext(oMessage, "|", 6)
            Session("Upload_WhatNext") = ""
            'subSubmit()
            lbtnClear.Enabled = True
            lblWhatNext.Attributes.Add("style", "display: none;")
            lbtnWhatNext.Visible = False

        End If

    End Sub

    Private Function FtpUploadFile(ByVal filetoupload As String)
        Try
            ' Setup session options
            Dim sessionOptions As New SessionOptions
            With sessionOptions
                .Protocol = Protocol.Ftp
                .HostName = "localhost"
                .UserName = "ftpuser"
                '.HostName = "localhost"
                '.UserName = "share"
                .Password = ftppwd
            End With

            Dim session As New WinSCP.Session
            ' Connect
            session.Open(sessionOptions)

            ' Upload files
            Dim transferOptions As New TransferOptions
            transferOptions.TransferMode = TransferMode.Binary

            Dim transferResult As TransferOperationResult
            transferResult = session.PutFiles(filetoupload, "/UPLOADS/" + Master.UserID + "/", False, transferOptions)
            session.Close()
            session.Dispose()
            ' Throw on any error
            transferResult.Check()

            ' Print results
            For Each transfer In transferResult.Transfers
                Console.WriteLine("Upload of {0} succeeded", transfer.FileName)
            Next

            Return 0
        Catch e As Exception
            Return 1
        End Try
    End Function
End Class