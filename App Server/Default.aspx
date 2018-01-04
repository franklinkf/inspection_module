<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="ins._Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BancSoft</title>
    <link rel="Stylesheet" type="text/css" href="css/base.css"/>
    <script type="text/javascript" src="js/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="js/base.js"></script>
    <script type="text/javascript" src="js/Default.js"></script>
    <script type="text/javascript"> window.onload = function() {window.history.forward(-1);}</script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" id="TimedPanel" updatemode="Conditional">
        <ContentTemplate>
           <asp:Panel ID="Panel1" runat="server" DefaultButton="lbtnLogin">
            <table class = "tbl1000B0" align="center" border="0" cellpadding="0" width="1000">
                <tbody>
                    <tr>
                        <td class="td50center">
                            <asp:Image ID="img1" runat="server" ImageUrl="img/logo.png"/>
                        </td>
                    </tr>
	                <tr>
		                <td class="td30center2">
			                <asp:Label ID="lblHeading" runat="server" class="lbl15" Text="LOGIN"></asp:Label>
                        </td>
	                </tr>
            </table>
            <table class = "tbl400B0" width="600px" border="0">
                <tbody>
	                <tr>
		                <td class="td30left" width="40%">
                            <asp:Label ID="lblUserName" runat="server" class="lbl10" Text="User Name"></asp:Label>
                        </td>
		                <td class="td30left" width="20%" colspan="2">
                            <asp:TextBox ID="txtUserID" runat="server" class="txt130"></asp:TextBox>
                        </td>
                        <td class="td30left" width="20%">
                           <asp:LinkButton ID="lbtnGetOTP" runat="server" CausesValidation="False" class="lbtn10">Get OTP</asp:LinkButton>
                            <asp:Button runat="server" ID="btnGetOTP" style="display:none;"/>
                       </td>
	                </tr>
	                <tr>
		                <td class="td30left">
                            <asp:Label ID="lblPassword" runat="server" class="lbl10" Text="Password"></asp:Label>
                        </td>
		                <td class="td30left" colspan="2">
                            <asp:TextBox ID="txtPassword" runat="server" class="txt130" TextMode="Password">FRAN1875</asp:TextBox>
                        </td>
                        <td></td>
	                </tr>
	                <tr runat="server" id = "tRow">
		                <td class="td30left" >
                            <asp:Label ID="lblOTP" runat="server" class="lbl10" Text="OTP"></asp:Label>
                        </td>
		                <td class="td30left" colspan="2">
                            <asp:TextBox ID="txtOTP" runat="server" class="txt130" TextMode="Password" MaxLength="6">FRAN1875</asp:TextBox>
                        </td>
                        <td ></td>
	                </tr>
	                <tr>
		                <td class="td30left" >
                            <asp:Label ID="Label1" runat="server" class="lbl10" Text="Module" Visible ="False"></asp:Label>
                        </td>
		                <td class="td30left" >
                            <asp:DropDownList ID="ddlModule" runat="server" class="dd130" style="text-transform:capitalize" ></asp:DropDownList>
                        </td>
	                </tr>
                <%--</tbody>
            </table>
            <table class="tbl400B0" width="600px" border="1">
                <tbody>--%>
                   <tr>
                       <td class="td30left" >
                           <%--<asp:Label ID="lblRemarks" runat="server" class="lbl10red" ClientIDMode="Static"></asp:Label>--%>
                       </td>
                       <td class="td30left" >
                            <asp:LinkButton ID="lbtnLogin" runat="server" class="lbtn10">Login</asp:LinkButton>
                            <asp:Button runat="server" ID="btnlogin" style="display:none;"/>
                       </td>
                       <td class="td30left" >
                           <asp:LinkButton ID="lbtnClear" runat="server" CausesValidation="False" class="lbtn10">Clear</asp:LinkButton>
                       </td>
                        <td ></td>
                       
                   </tr>
                   <tr><td class="td30left" colspan="4">
                           <asp:LinkButton ID="lbtnForgotPwd" runat="server" CausesValidation="False" class="lbtn10">Forgot Password</asp:LinkButton>
                   </td></tr>
                    <tr><td class="td30left" colspan="4">
                           <asp:Label ID="lblRemarks" runat="server" class="lbl10red" ClientIDMode="Static">Refer Circular 71/2017 for Login procedure.</asp:Label>
                   </td></tr>
                </tbody>
            </table>
            <asp:TextBox ID="txtsession" runat="server" Visible = "false"></asp:TextBox>
            <asp:TextBox ID="txtUser" runat="server" Visible = "false"></asp:TextBox>
            <asp:TextBox ID="txtSOLID" runat="server" Visible = "false"></asp:TextBox>
            <asp:TextBox ID="txtDesignation" runat="server" Visible = "false"></asp:TextBox>
            <asp:TextBox ID="txtWorkClass" runat="server" Visible = "false"></asp:TextBox>
            <asp:TextBox ID="txtRoleID" runat="server" Visible = "false"></asp:TextBox>
        </asp:Panel>    
        </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>