<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="upload.aspx.vb" Inherits="ins.upload" 
    title="BancSoft" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="Stylesheet" type="text/css" href="css/dgrid.css"/>
<%--<script type="text/javascript" src="js/upload.js"></script>--%>
<script type="text/javascript"> window.onload = function() {window.history.forward(1);}</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--<script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(JUpdatePanel);
</script>--%>
<asp:UpdatePanel runat="server" id="TimedPanel" updatemode="Conditional">
<ContentTemplate>
<asp:Panel ID="Panel1" runat="server" DefaultButton="lbtnSubmit">
    <table class = "tbl1000B0">
        <tbody>
            <tr>
                <td class="td30left" width="15%">
                    <asp:Label ID="lblMode" runat="server" class="lbl9" Text="Mode"></asp:Label>
                </td>
                <td class="td30left" width="35%">
                    <asp:DropDownList ID="ddlMode" runat="server" class="dd270">
                    </asp:DropDownList>
                </td>
                <td class="td30left" width="15%">
                    <asp:Label ID="lblUploadDataID" runat="server" class="lbl9" Text="Upload Data ID"></asp:Label>
                </td>
                <td class="td30left" width="35%">
                    <asp:TextBox ID="txtDataID" runat="server" class="txt70"></asp:TextBox>
                    <asp:ImageButton ID="imgSearchDataID" onclick="ibSearchDataID_Click" runat="server" class="imgSearch" ImageUrl="img/imgsearch.png" />
                    <asp:TextBox ID="txtUploadDataDesc" runat="server" class="txt160"></asp:TextBox>
                    <asp:LinkButton ID="lbtnValidate" runat="server" class="lbtn9">Validate</asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td class="td30left" width="15%">
                    <asp:Label ID="Label2" runat="server" class="lbl9" Text="SOL ID"></asp:Label>
                </td>
                <td class="td30left" width="35%">
                    <asp:DropDownList ID="ddlSOLID" runat="server" class="dd270">
                    </asp:DropDownList>
                </td>
                <td class="td30left" width="15%">
                    <asp:Label ID="Label1" runat="server" class="lbl9" Text="Inspection ID" Visible="False"></asp:Label>
                    <asp:Label ID="lblUploadID" runat="server" class="lbl9" Text="Upload ID"></asp:Label>
                </td>
                <td class="td30left" width="35%">
                    <asp:DropDownList ID="ddlInspectionID" runat="server" class="dd270" Visible="False">
                    </asp:DropDownList>
                      <asp:TextBox ID="txtUploadID" runat="server" class="txt70"></asp:TextBox>
                      <asp:ImageButton ID="imgUploadID" onclick="ibUloadID_Click" runat="server" class="imgSearch" ImageUrl="img/imgsearch.png" />
                </td>
            </tr>
            <tr>
                <td class="td30left" width="15%">
                    <%-- <asp:Label ID="lblUploadID" runat="server" class="lbl9" Text="Upload ID"></asp:Label> --%>
                      <asp:Label ID="lblDate" runat="server" class="lbl9" Text="Date"></asp:Label>
                </td>
                <td class="td30left" width="35%">
                    <%--  <asp:TextBox ID="txtUploadID" runat="server" class="txt70"></asp:TextBox> --%>
                    <%--  <asp:ImageButton ID="imgUploadID" onclick="ibUloadID_Click" runat="server" class="imgSearch" ImageUrl="img/imgsearch.png" /> --%>
                    <asp:TextBox ID="txtdate" runat="server" class="txt70"></asp:TextBox>
                </td>
                <td class="td30left" width="15%">       
                    <%--  <asp:Label ID="lblDate" runat="server" class="lbl9" Text="Date"></asp:Label> --%>
                       <asp:Label ID="Label4" runat="server" class="lbl9" Text="Select Upload File"></asp:Label>
                </td>
                <td class="td30left" width="35%">
                    <%--  <asp:TextBox ID="txtdate" runat="server" class="txt70"></asp:TextBox> --%>
                     <asp:FileUpload runat="server" height = "25" id="FileUpload1" Width = "275px"></asp:FileUpload>              
                </td>
            </tr>
          <%--   <tr>
                <td class="td30left" width="15%">       
                    <asp:Label ID="Label4" runat="server" class="lbl9" Text="Select Upload File"></asp:Label>
                </td>
                <td class="td30left" width="35%">
                    <asp:FileUpload runat="server" height = "25" id="FileUpload1" Width = "275px"></asp:FileUpload>              
                </td>
                <td class="td30left" width="15%">
                    <asp:Label ID="Label3" runat="server" class="lbl9" Text=""></asp:Label>
                </td>
                <td class="td30left" width="35%">
                    <asp:Label ID="Label5" runat="server" class="lbl9" Text=""></asp:Label>
                </td>
            </tr> --%>
            <tr>
                <td class="td30center" colspan = "2">
                    <asp:LinkButton ID="lbtnSubmit" runat="server" class="lbtn9">Submit</asp:LinkButton>
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
            <tr>
                <td class="td30left" colspan = "2">
                 <asp:Label ID="lblWhatNext" runat="server" class="lbl9" Text="What Next?" Font-Bold="True" ForeColor="#009900" style="display:none;"></asp:Label>
                </td>
            </tr>
             <tr>
            <td class="td30left" colspan = "2">
                <asp:LinkButton ID="lbtnWhatNext" runat="server" class="lbtn9" ></asp:LinkButton>
                <asp:Button ID="btnWhatNext" runat="server" style ="display:none"></asp:Button>
            </td>
        </tr>
         <%--    <asp:Repeater ID="Repeater2" runat="server" OnItemCommand ="Repeater2_OnItemCommand" >
            <ItemTemplate>
            <tr ID = "RepeaterRow">
                <td class="td30left" colspan = "5"">
                    <asp:LinkButton ID="lbtnWhatNext" runat="server" class="lbtn9" Text='<%#Eval("LABEL_NAME")%>'></asp:LinkButton>
                    <asp:Label ID="lblFilterID" runat="server" Visible = "false" Text='<%#Eval("LINK_VALUES")%>'></asp:Label>
                    <asp:Label ID="lblSubmitFlag" runat="server" Visible = "false" Text='<%#Eval("SUBMIT_FLAG")%>'></asp:Label>
                    
                </td>
            </tr>
            </ItemTemplate>
           </asp:Repeater> --%>
            
        </tbody>
    </table>
    <%--Search Box - Start--%>
    <asp:Panel ID="pnlSearch" runat="server" class = "pnlSearch" Width = "550px">
        <table class = "tbl_search" style="width:550px;">
            <tr>
                <td class ="td_rpt_center" width = "25%"><asp:Label ID="lblSearchID" runat="server" class="lbl9"></asp:Label></td>
                <td class ="td_rpt_center" width = "75%"><asp:Label ID="lblSearchDesc" runat="server" class="lbl9"></asp:Label></td>
            </tr>
        </table>
        <asp:Repeater ID="rptSearchContent" runat="server" onitemcommand="Repeater1_ItemCommand">
        <ItemTemplate>
        <table class = "tbl_search" style="width:550px;">
        <tr>
            <td class ="td_rpt_left" width = "25%">
                <asp:LinkButton ID="lbtnSearchValue" runat="server" class="lbtn9" CommandArgument = '<%# Eval("SEARCHVALUE") %>' Text = '<%#Eval("SEARCHVALUE")%>'></asp:LinkButton>
            </td>
            <td class ="td_rpt_left" width = "75%">
                <asp:Label ID="lblSearchDesc" runat="server" class="lbl9" Text='<%#Eval("SEARCHTEXT")%>'></asp:Label>
            </td>
        </tr>
        </table>
        </ItemTemplate>
       </asp:Repeater>
        <table class = "tbl_search" style="width:550px;">
            <tr>
                <td  class ="tf_rpt_left" width = "30%">
                    <asp:LinkButton ID="lbtnPrevious" runat="server" class="lbtn9" Text = "Previous Set"></asp:LinkButton>
                </td>
                <td  class ="tf_rpt_center" width = "40%">
                    <asp:LinkButton ID="lbtnPresent" runat="server" class="lbtn9" Text = "First "></asp:LinkButton>
                </td>
                <td  class ="tf_rpt_right" width = "30%">
                    <asp:LinkButton ID="lbtnNext" runat="server" class="lbtn9" Text = "Next Set"></asp:LinkButton>
                </td>
            </tr>
        </table>

        <asp:TextBox ID="txtTotalCount" runat="server" Visible = "false"></asp:TextBox>
        <asp:TextBox ID="txtStartNo" runat="server" Visible = "false"></asp:TextBox>
        <asp:TextBox ID="txtSearchHead" runat="server" Visible = "false"></asp:TextBox>
        <asp:TextBox ID="txtissolbased" runat="server" Visible = "false"></asp:TextBox>
        <asp:TextBox ID="txtisinspbased" runat="server" Visible = "false"></asp:TextBox>
        <asp:TextBox ID="txtisdatebased" runat="server" Visible = "false"></asp:TextBox>
        
    </asp:Panel>
    <%--Search Box - End--%>
    <cc1:ModalPopupExtender ID="mpeSearchUploadID" runat="server" TargetControlId="lblMode" PopupControlID="pnlSearch" OkControlID="lblMode"></cc1:ModalPopupExtender>
    <asp:HiddenField ID="hfJQueryStatus" runat="server" />
</asp:Panel>
</ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID = "lbtnSubmit" />
    </Triggers>
</asp:UpdatePanel>
</asp:Content>
