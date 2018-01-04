<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="viewreport.aspx.vb" Inherits="ins.viewreport" 
    title="BancSoft" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="Stylesheet" type="text/css" href="css/dgrid.css"/>
<%--<script type="text/javascript" src="js/viewreports.js"></script>--%>
<script type="text/javascript"> window.onload = function() {window.history.forward(1);}</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:Panel ID="Panel1" runat="server" DefaultButton="lbtnSubmit">
    <table class = "tbl1000B0">
        <tbody>
            <tr>
                <td class="td30left" width="35%">
                    <asp:Label ID="lblReportID" runat="server" class="lbl9" Text="Report ID"></asp:Label>
                </td>
                <td class="td30left" width="15%">
                    <asp:TextBox ID="txtReportID" runat="server" class="txt100"></asp:TextBox>
                </td>
                <td class="td30left" width="35%">
                    <asp:Label ID="lblReportSLNo" runat="server" class="lbl9" Text="Report SL No"></asp:Label>
                </td>
                <td class="td30left" width="15%">
                    <asp:TextBox ID="txtReportSLNo" runat="server" class="txt100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td30left" width="35%">
                    <asp:Label ID="Label1" runat="server" class="lbl9" Text="Generated From"></asp:Label>
                </td>
                <td class="td30left" width="15%">
                    <asp:TextBox ID="txtDateFrom" runat="server" class="txt100"></asp:TextBox>
                </td>
                <td class="td30left" width="35%">
                    <asp:Label ID="Label2" runat="server" class="lbl9" Text="Generated To"></asp:Label>
                </td>
                <td class="td30left" width="15%">
                    <asp:TextBox ID="txtDateTo" runat="server" class="txt100"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="td30center" colspan = "2">
                    <asp:LinkButton ID="lbtnSubmit" runat="server" class="lbtn9">Submit</asp:LinkButton>
                    <asp:Button runat="server" ID="btnSubmit" style="display:none;"/>
                </td>
                <td class="td30center" colspan = "2">
                    <asp:LinkButton ID="lbtnClear" runat="server" class="lbtn9">Clear</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td class="td30center" colspan = "4">
                    <asp:Label ID="lblRemarks" runat="server" class="lbl9red"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
    <table class = "tbl_rpt">
       <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="Repeater1_OnItemDataBound">
        <HeaderTemplate>
        <tr>
            <th class ="dg_rpt_center" width = "6%">SL No</th>
            <th class ="dg_rpt_center" width = "6%">Rep ID</th>
            <th class ="dg_rpt_center" width = "24%">Description</th>
            <th class ="dg_rpt_center" width = "14%">Triggered</th>
            <th class ="dg_rpt_center" width = "14%">Processed</th>
            <th class ="dg_rpt_center" width = "14%">Completed</th>
            <th class ="dg_rpt_center" width = "8%">Status</th>            
            <th class ="dg_rpt_center" width = "7%">TXT</th>
            <th class ="dg_rpt_center" width = "7%">PDF</th>
        </tr>
        </HeaderTemplate>
        <ItemTemplate>
        <tr>
            <td class ="dg_rpt_left" width = "6%">
                <asp:Label ID="lblreportslno" runat="server" class="dg_lbl9" Text='<%#Eval("REPORTSLNO")%>'></asp:Label>
            </td>
            <td class ="dg_rpt_left" width = "6%">
                <asp:Label ID="lblreportid" runat="server" class="dg_lbl9" Text='<%#Eval("REPORTID")%>'></asp:Label>
            </td>
            <td class ="dg_rpt_left" width = "24%">
                <asp:Label ID="lblreportdesc" runat="server" class="dg_lbl9" Text='<%#Eval("REPORTDESC")%>'></asp:Label>
            </td>
            <td class ="dg_rpt_left" width = "14%">
                <asp:Label ID="lbltriggered" runat="server" class="dg_lbl9" Text='<%#Eval("TRIGGERED")%>'></asp:Label>
            </td>
            <td class ="dg_rpt_left" width = "14%">
                <asp:Label ID="lblprocessed" runat="server" class="dg_lbl9" Text='<%#Eval("PROCESSED")%>'></asp:Label>
            </td>
            <td class ="dg_rpt_left" width = "14%">
                <asp:Label ID="lblcompleted" runat="server" class="dg_lbl9" Text='<%#Eval("COMPLETED")%>'></asp:Label>
            </td>
            <td class ="dg_rpt_left" width = "8%">
                <asp:Label ID="lblstatus" runat="server" class="dg_lbl9" Text='<%#Eval("GSTATUS")%>'></asp:Label>
            </td>
            <td class ="dg_rpt_center" width = "7%">
                <asp:LinkButton ID="lbtnCSV" runat="server" CommandArgument ='<%#Eval("REPORTSLNO") %>' OnClick="gcsv" class="lbtn9">Download</asp:LinkButton>
            </td>
            <td class ="dg_rpt_center" width = "7%">
                <asp:LinkButton ID="lbtnPDF" runat="server" CommandArgument ='<%#Eval("REPORTSLNO") %>' OnClick="gpdf" class="lbtn9">Download</asp:LinkButton>
                <asp:Label ID="lblreporttype" Text ='<%#Eval("REPORTTYPE") %>' visible = "false" runat="server"></asp:Label>
            </td>
        </tr>
        </ItemTemplate>
        </asp:Repeater>
    </table>    
    <table class = "tbl_rpt">
        <tr>
            <td  class ="tf_rpt_center" width = "30%">
                <asp:LinkButton ID="lbtnPrevious" runat="server" class="lbtn9" Text = "Previous Set"></asp:LinkButton>
            </td>
            <td  class ="tf_rpt_center" width = "40%">
                <asp:LinkButton ID="lbtnPresent" runat="server" class="lbtn9" Text = "First "></asp:LinkButton>
            </td>
            <td  class ="tf_rpt_center" width = "30%">
                <asp:LinkButton ID="lbtnNext" runat="server" class="lbtn9" Text = "Next Set"></asp:LinkButton>
            </td>
        </tr>
    </table>    
    <asp:TextBox ID="txtTotalCount" runat="server" Visible = "false"></asp:TextBox>
    <asp:TextBox ID="txtStartNo" runat="server" Visible = "false"></asp:TextBox>
    <asp:TextBox ID="txtReportPath" runat="server" Visible = "false"></asp:TextBox>
    <asp:HiddenField ID="hfJQueryStatus" runat="server" />
</asp:Panel>
</asp:Content>
