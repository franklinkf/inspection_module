Imports basevb
Imports ins_prop
Imports ins_bus
Imports System
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports WinSCP
Partial Public Class viewreport
    Inherits basevb.basevb
    Dim objProp As New LetterApp.viewreport_prop
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
            fetch_report_path()
            If Session("navigation") = "R" Then
                subSubmit()
                Session("navigation") = ""
            End If
        Else
            If hfJQueryStatus.Value = "CLEAR" Then
                lblRemarks.Text = ""
                Repeater1.DataSource = Nothing
                Repeater1.Visible = False
                lbtnPrevious.Visible = False
                lbtnPresent.Visible = False
                lbtnNext.Visible = False
                lbtnSubmit.Enabled = True
                hfJQueryStatus.Value = ""
            ElseIf hfJQueryStatus.Value = "SUBMIT" Then
                txtReportID.Enabled = False
                txtReportSLNo.Enabled = False
                txtDateFrom.Enabled = False
                txtDateTo.Enabled = False
                lbtnSubmit.Enabled = False
                lbtnClear.Enabled = False
                lblRemarks.Text = "Processing..."
                hfJQueryStatus.Value = ""
            End If
        End If
    End Sub
    Protected Sub fetch_report_path()
        objProp = objBus.iGetReportPath(objProp)
        txtReportPath.Text = objProp.reportpath
    End Sub
    Protected Sub initialize_Controlls()
        txtReportID.Text = "ALL"
        txtReportSLNo.Text = "ALL"
        txtDateFrom.Text = "01-01-1900"
        txtDateTo.Text = Format(System.DateTime.Now, "dd-MM-yyyy")
        Repeater1.DataSource = Nothing
        Repeater1.Visible = False
        lbtnPrevious.Visible = False
        lbtnPresent.Visible = False
        lbtnNext.Visible = False
    End Sub

    Private Sub subSubmit()
        Repeater1.DataSource = Nothing
        If txtReportID.Text = "" Then
            lblRemarks.Text = "Enter Report ID. Enter ALL, if not applicable"
            txtReportID.Focus()
            Exit Sub
        End If
        If txtReportSLNo.Text = "" Then
            lblRemarks.Text = "Enter Report Serial No. Enter ALL, if not applicable"
            txtReportSLNo.Focus()
            Exit Sub
        End If
        If txtDateFrom.Text = "" Then
            lblRemarks.Text = "Enter From Date"
            txtDateFrom.Focus()
            Exit Sub
        End If
        If txtDateTo.Text = "" Then
            lblRemarks.Text = "Enter To Date"
            txtDateTo.Focus()
            Exit Sub
        End If
        If fnIsvaliddate(txtDateFrom.Text) = False Then
            lblRemarks.Text = "Enter valid date"
            txtDateFrom.Focus()
            Exit Sub
        End If
        If fnIsvaliddate(txtDateTo.Text) = False Then
            lblRemarks.Text = "Enter valid date"
            txtDateTo.Focus()
            Exit Sub
        End If
        method1()
        objProp = objBus.iGetSearchRecordCountVR(objProp)
        If objProp.recordcount = 0 Then
            lblRemarks.Text = "No records to display in search"
            Exit Sub
        End If
        method2()
        method3()
        method0()
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnSubmit.Click
        lblRemarks.Text = ""
        subSubmit()
        lbtnClear.Enabled = True
        lbtnSubmit.Enabled = True
    End Sub

    Public Sub method1()
        objProp.userid = UCase(Master.UserID)
        objProp.sessionid = Master.SessionID
        objProp.reportid = UCase(txtReportID.Text)
        objProp.reportslno = UCase(txtReportSLNo.Text)
        objProp.datefrom = fnStringtoMySQLDate(txtDateFrom.Text)
        objProp.dateto = fnStringtoMySQLDate(txtDateTo.Text)
    End Sub

    Public Sub method2()
        txtTotalCount.Text = objProp.recordcount
        objProp.searchstartcounter = 0
        objProp.searchnoofrecords = 10
        txtStartNo.Text = "1"
    End Sub

    Public Sub method3()
        lbtnPrevious.Visible = True
        lbtnPresent.Visible = True
        lbtnNext.Visible = True
        FetchData()
        lbtnPresent.Text = "1-" & Repeater1.Items.Count & " of " & txtTotalCount.Text
        lbtnPrevious.Visible = False
        If Repeater1.Items.Count < 10 Then
            lbtnNext.Visible = False
        Else
            lbtnNext.Visible = True
        End If
    End Sub

    Public Sub method0()
        Repeater1.Visible = True
    End Sub

    Public Sub FetchData()
        Repeater1.DataSource = Nothing
        objProp = objBus.iGetDSDataVR(objProp)
        Repeater1.DataSource = objProp.searchData
        Repeater1.DataBind()
    End Sub

    Protected Sub lbtnNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnNext.Click
        method1()
        Dim startno As Integer = txtStartNo.Text
        Dim totalcount As Integer = txtTotalCount.Text
        startno = startno + 10
        objProp.searchstartcounter = startno - 1
        objProp.searchnoofrecords = 10
        txtStartNo.Text = startno
        FetchData()
        If Repeater1.Items.Count + startno - 1 > txtTotalCount.Text Then
            lbtnPresent.Text = startno & "-" & txtTotalCount.Text & " of " & txtTotalCount.Text
        Else
            lbtnPresent.Text = startno & "-" & Repeater1.Items.Count + startno - 1 & " of " & txtTotalCount.Text
        End If

        If startno + 10 > txtTotalCount.Text Then
            lbtnNext.Visible = False
            lbtnPrevious.Visible = True
        Else
            lbtnPrevious.Visible = True
            lbtnNext.Visible = True
        End If
        method0()
    End Sub

    Protected Sub lbtnPrevious_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lbtnPrevious.Click
        method1()
        Dim startno As Integer = txtStartNo.Text
        Dim totalcount As Integer = txtTotalCount.Text
        startno = startno - 10
        objProp.searchstartcounter = startno - 1
        objProp.searchnoofrecords = 10
        txtStartNo.Text = startno
        FetchData()
        lbtnPresent.Text = startno & "-" & Repeater1.Items.Count + startno - 1 & " of " & txtTotalCount.Text
        If startno - 10 < 0 Then
            lbtnPrevious.Visible = False
            lbtnNext.Visible = True
        Else
            lbtnPrevious.Visible = True
            lbtnNext.Visible = True
        End If
        method0()
    End Sub

    Protected Sub Repeater1_OnItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        Dim item As RepeaterItem = e.Item
        If item.ItemType = ListItemType.AlternatingItem OrElse item.ItemType = ListItemType.Item Then
            Dim dr As DataRowView = DirectCast(e.Item.DataItem, DataRowView)
            Dim status As String = Convert.ToString(dr("GSTATUS"))
            Dim lblstatus As Label = DirectCast(item.FindControl("lblstatus"), Label)
            Dim lbtnCSV As LinkButton = DirectCast(item.FindControl("lbtnCSV"), LinkButton)
            Dim lbtnPDF As LinkButton = DirectCast(item.FindControl("lbtnPDF"), LinkButton)
            Dim lblreporttype As Label = DirectCast(item.FindControl("lblreporttype"), Label)
            If status = "C" Then

                If lblreporttype.Text = "P" Then
                    lbtnCSV.Visible = False
                    lbtnPDF.Visible = True
                ElseIf lblreporttype.Text = "T" Then
                    lbtnCSV.Visible = True
                    lbtnPDF.Visible = False
                Else
                    lbtnCSV.Visible = True
                    lbtnPDF.Visible = True
                End If
                lblstatus.Text = "Generated"
            ElseIf status = "P" Then
                lbtnCSV.Visible = False
                lbtnPDF.Visible = False
                lblstatus.Text = "Processing"
            ElseIf status = "T" Then
                lbtnCSV.Visible = False
                lbtnPDF.Visible = False
                lblstatus.Text = "Triggered"
            Else
                lbtnCSV.Visible = False
                lbtnPDF.Visible = False
                lblstatus.Text = "Error"
            End If
        End If
    End Sub

    Protected Sub Repeater1_OnItemCommand(ByVal source As Object, ByVal e As RepeaterCommandEventArgs)
        'If objprop.searchflag = "Y" Then
        '    Dim txtFilterValue As TextBox = DirectCast(e.Item.FindControl("txtFilterValue"), TextBox)
        '    Dim lblFilterID As Label = DirectCast(e.Item.FindControl("lblFilterID"), Label)
        '    txtfilterid.Text = lblFilterID.Text
        '    txtSearchHead.Text = e.CommandArgument
        '    txtSearchContent.Text = txtFilterValue.Text
        '    repeater_search()
        'End If
    End Sub

    Public Sub gcsv(ByVal sender As Object, ByVal e As EventArgs)
        'Dim pfile As String = txtReportPath.Text & Master.UserID & "/" & Master.UserID & "_" & CType(sender, LinkButton).CommandArgument & ".pdf"
        'If Not File.Exists(pfile) Then
        '    converttopdf(txtReportPath.Text & Master.UserID & "/" & Master.UserID & "_" & CType(sender, LinkButton).CommandArgument)
        'End If
        'Dim filePath As String = txtReportPath.Text & Master.UserID & "/" & Master.UserID & "_" & CType(sender, LinkButton).CommandArgument & ".pdf"
        'Dim pinfo As New ProcessStartInfo()
        'pinfo.FileName = filePath
        'Dim p As New Process()
        'p.StartInfo = pinfo
        'p.Start()
        Dim pfile As String = txtReportPath.Text & Master.UserID & "/" & Master.UserID & "_" & CType(sender, LinkButton).CommandArgument & ".txt"
        Try
            If Not File.Exists(pfile) Then
                Dim rslt = FtpDownloadFile(Master.UserID & "_" & CType(sender, LinkButton).CommandArgument & ".txt")
            End If
        Catch ex As Exception
            lblRemarks.Text = "Report download failed." + ex.Message
            Exit Sub
        End Try

        If Context.Request.QueryString("ImageName") IsNot Nothing Then
            pfile = Context.Request.QueryString("ImageName").ToString()
        End If
        Dim filename As String = pfile
        Dim fileInfo As New System.IO.FileInfo(filename)
        Context.Response.Clear()
        Context.Response.AddHeader("Content-Disposition", "attachment;filename=""" + fileInfo.Name & """")
        Context.Response.AddHeader("Content-Length", fileInfo.Length.ToString())
        Context.Response.ContentType = "application/octet-stream"
        Context.Response.TransmitFile(fileInfo.FullName)
        Context.Response.Flush()
        Context.Response.End()
    End Sub

    Public Sub gpdf(ByVal sender As Object, ByVal e As EventArgs)
        Dim pfile As String = "D:/REPORTS/" & Master.UserID & "/" & Master.UserID & "_" & CType(sender, LinkButton).CommandArgument & ".pdf"
        Try
            If Not File.Exists(pfile) Then
                Dim tfile As String = "D:/REPORTS/" & Master.UserID & "/" & Master.UserID & "_" & CType(sender, LinkButton).CommandArgument & ".txt"
                If File.Exists(tfile) Then
                    converttopdf("D:/REPORTS/" & Master.UserID & "/" & Master.UserID & "_" & CType(sender, LinkButton).CommandArgument)
                Else
                    Dim rslt = FtpDownloadFile(Master.UserID & "_" & CType(sender, LinkButton).CommandArgument & ".txt")
                    If rslt = "1" Then
                        converttopdf("D:/REPORTS/" & Master.UserID & "/" & Master.UserID & "_" & CType(sender, LinkButton).CommandArgument)
                    Else
                        lblRemarks.Text = rslt
                        Exit Sub
                    End If
                End If
            End If
        Catch ex As Exception
            lblRemarks.Text = "Report download failed." + ex.Message
            Exit Sub
        End Try

        If Context.Request.QueryString("ImageName") IsNot Nothing Then
            pfile = Context.Request.QueryString("ImageName").ToString()
        End If
        Dim filename As String = pfile
        Dim fileInfo As New System.IO.FileInfo(filename)
        Context.Response.Clear()
        Context.Response.AddHeader("Content-Disposition", "attachment;filename=""" + fileInfo.Name & """")
        Context.Response.AddHeader("Content-Length", fileInfo.Length.ToString())
        Context.Response.ContentType = "application/octet-stream"
        Context.Response.TransmitFile(fileInfo.FullName)
        Context.Response.Flush()
        Context.Response.End()
    End Sub
    Public Sub converttopdf(ByVal filepath As String)
        If Not String.IsNullOrEmpty(filepath & ".txt") Then
            Dim firstline As String = readNthLine(filepath & ".txt", 1)
            Dim orientation As String = delimitedtext(firstline, "|", 1)
            Dim repid As String = delimitedtext(firstline, "|", 2)
            Dim repname As String = delimitedtext(firstline, "|", 3)
            Dim userid As String = delimitedtext(firstline, "|", 4)
            Dim repdate As String = delimitedtext(firstline, "|", 5)
            Dim branchname As String = delimitedtext(firstline, "|", 6)
            Dim doc1 = New Document(PageSize.A4, 48, 32, 32, 32)
            Dim NoofCharPerLine As Integer = 94
            Dim Signatory As String = "Date: " & Format(System.DateTime.Now, "dd-MM-yyyy") & "                     Signatory 1                                   Signatory 2"
            If UCase(orientation) = "L" Then
                doc1.SetPageSize(PageSize.A4.Rotate())
                NoofCharPerLine = 140
                Signatory = "Date: " & Format(System.DateTime.Now, "dd-MM-yyyy") & "                                                 Signatory 1                                                    Signatory 2"
            End If

            Dim fntTitle1 As BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, False)
            Dim title1 As New Font(fntTitle1, 18, Font.BOLD, Color.BLACK)

            Dim fntTitle2 As BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, False)
            Dim title2 As New Font(fntTitle2, 10, Font.ITALIC, Color.BLACK)

            Dim fntTitle3 As BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, False)
            Dim title3 As New Font(fntTitle3, 11, Font.NORMAL, Color.BLACK)

            Dim fntCourier As BaseFont = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, False)
            Dim Courier As New Font(fntCourier, 9, Font.NORMAL, Color.BLACK)

            Dim fntCourierb As BaseFont = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1252, False)
            Dim Courierb As New Font(fntCourierb, 9, Font.NORMAL, Color.BLACK)

            PdfWriter.GetInstance(doc1, New FileStream(filepath & ".pdf", FileMode.Create))

            doc1.Open()

            Dim imagepath As String = "D:\letter_module\img\lh.png"

            Dim image As iTextSharp.text.Image = iTextSharp.text.Image.GetInstance(imagepath)
            image.ScalePercent(24.0F)

            Dim table As New PdfPTable(2)
            table.TotalWidth = 513.0F
            Dim intTblWidth() As Integer = {65, 448}
            If UCase(orientation) = "L" Then
                table.TotalWidth = 760.0F
                intTblWidth(0) = 65
                intTblWidth(1) = 695
            End If

            table.SetWidths(intTblWidth)
            table.LockedWidth = True
            table.HorizontalAlignment = Element.ALIGN_LEFT
            table.DefaultCell.Border = Rectangle.NO_BORDER

            Dim img As New PdfPCell(image)
            img.Rowspan = 3
            img.Border = Rectangle.NO_BORDER
            img.HorizontalAlignment = Element.ALIGN_CENTER
            table.AddCell(img)

            table.AddCell(New Paragraph("Kerala Gramin Bank", title1))
            table.AddCell(New Paragraph("(A Government owned scheduled bank sponsored by Canara Bank)", title2))
            If branchname = "" Then
                table.AddCell("Letter Module")
            Else
                table.AddCell(branchname)
            End If


            Dim aa As New PdfPCell(New Paragraph(""))
            aa.Colspan = 2
            aa.Border = PdfPCell.BOTTOM_BORDER
            table.AddCell(aa)
            Dim hd As New PdfPCell
            If repid = "N" Then
                hd = New PdfPCell(New Paragraph(UCase(repname), title3))
            ElseIf repid = "L" Then
                hd = New PdfPCell(New Paragraph("", title3))
            Else
                hd = New PdfPCell(New Paragraph("REPORT ID - " & repid & " : " & UCase(repname), title3))
            End If
            hd.Colspan = 2
            hd.Border = PdfPCell.NO_BORDER
            hd.HorizontalAlignment = Element.ALIGN_CENTER
            hd.VerticalAlignment = Element.ALIGN_MIDDLE
            table.AddCell(hd)
            If repid <> "L" Then

                Dim bb As New PdfPCell(New Paragraph(""))
                bb.Colspan = 2
                bb.Border = PdfPCell.BOTTOM_BORDER
                table.AddCell(bb)

            End If
            doc1.Add(table)

            Dim K As New Phrase()
            Dim K1 As New Chunk(" ", Courier)
            K.Add(Chunk.NEWLINE)
            doc1.Add(K)

            Dim readText As String() = File.ReadAllLines(filepath & ".txt")
            Dim para As New Paragraph()
            Dim slno As Integer = 0
            Dim NL As New Chunk(Environment.NewLine)
            Dim str As String
            Dim len As Integer
            For Each s As String In readText
                slno = slno + 1
                If slno > 1 Then
                    If Mid(s.ToString, 1, 7) = "@@101##" Then
                        str = StrDup(NoofCharPerLine, "_")
                        Dim c1 As New Chunk(str, Courier)
                        para.Add(c1)
                    ElseIf Mid(s.ToString, 1, 7) = "@@102##" Then
                        str = StrDup(NoofCharPerLine, " ")
                        Dim c1 As New Chunk(str, Courier)
                        para.Add(c1)
                    ElseIf Mid(s.ToString, 1, 7) = "@@103##" Then
                        str = Mid(s.ToString, 8, NoofCharPerLine)
                        len = str.Length
                        str = str & StrDup(NoofCharPerLine - len, " ")
                        Dim c1 As New Chunk(str, Courier)
                        para.Add(c1)
                    ElseIf Mid(s.ToString, 1, 7) = "@@104##" Then
                        str = Signatory
                        len = str.Length
                        str = str & StrDup(NoofCharPerLine - len, " ")
                        Dim c1 As New Chunk(str, Courierb)
                        para.Add(c1)
                    Else
                        str = s.ToString
                        len = str.Length
                        str = str & StrDup(NoofCharPerLine - len, " ")
                        Dim c1 As New Chunk(str, Courier)
                        para.Add(c1)
                    End If
                    para.Add(NL)
                End If
            Next
            para.SetLeading(10.0F, 0.0F)
            doc1.Add(para)
            doc1.Close()
        End If
    End Sub
    Private Function FtpDownloadFile(ByVal filetoget As String)
        Try
            ' Setup session options
            Dim sessionOptions As New SessionOptions
            With sessionOptions
                .Protocol = Protocol.Ftp
                .HostName = "localhost"
                .UserName = "ftpuser"
                .Password = ftppwd
                '.HostName = "localhost"
                '.UserName = "share"
                '.Password = "abcd1234"
            End With

            Dim session As New WinSCP.Session
            ' Connect
            session.Open(sessionOptions)

            ' Download files
            Dim transferOptions As New TransferOptions
            transferOptions.TransferMode = TransferMode.Binary

            Dim transferResult As TransferOperationResult
            Dim dirpath As String = "D:\REPORTS\" & Master.UserID & "\"
            If Directory.Exists(dirpath) = False Then
                Directory.CreateDirectory(dirpath)
            End If
            transferResult = session.GetFiles("/REPORTS/" + Master.UserID + "/" + filetoget, dirpath, False, transferOptions)
            session.Close()
            session.Dispose()
            ' Throw on any error
            transferResult.Check()

            ' Print results
            For Each transfer In transferResult.Transfers
                Console.WriteLine("Download of {0} succeeded", transfer.FileName)
            Next

            Return 1
        Catch e As Exception
            Return e.Message
        End Try
    End Function
End Class