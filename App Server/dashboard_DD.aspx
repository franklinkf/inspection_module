<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="dashboard_DD.aspx.vb" Inherits="ins.dashboard_DD" 
    title="BancSoft" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript"> window.onload = function() {window.history.forward(1);}</script>
    <style type="text/css">
        .auto-style1 {
            height: 20px;
        }
    </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="tbl1000B0">
        <tr>
            <td align="left">    
                <asp:ImageButton ID="ibtnBack" runat="server" class="imgSearch" ImageUrl="img/back1.png" />  
            &nbsp;</td>                                      
        </tr>    
    </table>
    <asp:Linkbutton ID="lbtest"  runat="server" class ="lbl10" OnClick='subCallDDepExpo' Visible ="False"></asp:Linkbutton>
    <table align="center" border="1" style="border-collapse:collapse;border-top:thin solid;border-bottom:thin solid;border-left:thin solid;border-right:thin solid;" cellpadding="5" width="1000">
        <tbody> 
            <asp:Repeater ID="RepeaterDB" runat="server">
                <HeaderTemplate>
                    <tr>
                        <th class ="lbl105" width = "5%" style ="text-align:center">ID</th>
                        <th class ="lbl105" width = "10%" style ="text-align:center">Date</th>
                        <th class ="lbl105" width = "12%" style ="text-align:center">Type</th>
                        <th class ="lbl105" width = "8%" style ="text-align:center">Init By</th>
                        <th class ="lbl105" width = "8%" style ="text-align:center">Dept</th>
                        <th class ="lbl105" width = "8%" style ="text-align:center">Sent To</th>
                        <th class ="lbl105" width = "8%" style ="text-align:center">Dept</th>
                        <th class ="lbl105" width = "30%" style ="text-align:center">Subject</th>
                        <th class ="lbl105" width = "5%" style ="text-align:center">Status</th>
                        <th class ="lbl105" width = "6%" style ="text-align:center">Action</th>
                    </tr>
                </HeaderTemplate>
            <ItemTemplate>
                <tr style="height:20px;vertical-align:top" runat="server" id = "tRow">
                    <td runat="server" cellpadding="5" id = "td01" align="right">
                        <asp:Label ID="lblSLNO"  runat="server" Visible="false"  Text='<%#Eval("SLNO")%>'></asp:Label>
                        <asp:Label ID="C01"  runat="server" class ="lbl10" Text='<%#Eval("DATA01")%>'></asp:Label>
                    </td>
                    <td runat="server" cellpadding="5" id = "td02" align="left"><asp:Label ID="c02"  runat="server" class ="lbl9N" Text='<%#Eval("DATA03")%>'></asp:Label></td>
                    <td runat="server" cellpadding="5" id = "td03" align="left"><asp:Label ID="c03"  runat="server" class ="lbl9N" Text='<%#Eval("DATA04")%>'></asp:Label></td>
                    <td runat="server" cellpadding="5" id = "td04" align="left"><asp:Label ID="c04"  runat="server" class ="lbl9N" Text='<%#Eval("DATA05")%>'></asp:Label></td>
                    <td runat="server" cellpadding="5" id = "td05" align="left"><asp:Label ID="c05"  runat="server" class ="lbl9N" Text='<%#Eval("DATA06")%>'></asp:Label></td>
                    <td runat="server" cellpadding="5" id = "td06" align="left"><asp:Label ID="c06"  runat="server" class ="lbl9N" Text='<%#Eval("DATA07")%>'></asp:Label></td>
                    <td runat="server" cellpadding="5" id = "td07" align="left"><asp:Label ID="C07"  runat="server" class ="lbl9N" Text='<%#Eval("DATA08")%>'></asp:Label></td>
                    <td runat="server" cellpadding="5" id = "td08" align="left"><asp:Label ID="C08"  runat="server" class ="lbl9N" Text='<%#Eval("DATA09")%>'></asp:Label></td>
                    <td runat="server" cellpadding="5" id = "td09" align="left"><asp:Label ID="C09"  runat="server" class ="lbl9N" Text='<%#Eval("DATA10")%>'></asp:Label></td>
                    <td runat="server" cellpadding="5" id = "td10" align="center"><asp:Linkbutton ID="c10"  runat="server" class ="menu" Text="View" OnClick='subCallDDepExpo' CommandArgument='<%#Eval("SLNO").ToString %>'></asp:Linkbutton></td>
                </tr>                                
             </ItemTemplate>
            </asp:Repeater>
        </tbody>   
    </table>
    <table align="center" border="0" style="border-collapse:collapse; cellpadding="5" width="1000">
        <tr>
            <td class="td30left" width="33%">
                <asp:LinkButton ID="lbtnNew" runat="server" class="lbtn9" Text = "Previous"></asp:LinkButton>
            </td>
            <td class="td30center" width="34%">
                <asp:LinkButton ID="lbtnNow" runat="server" class="lbtn9" Text = "Present" ></asp:LinkButton>
                <asp:TextBox ID="txtMaxCnt" runat="server" class="txt70" style="display:none"></asp:TextBox>
                <asp:TextBox ID="txtstartcount" runat="server" class="txt70" style="display:none"></asp:TextBox>
            </td>
            <td class="td30right" width="33%">
                <asp:LinkButton ID="lbtnOld" runat="server" class="lbtn9" Text = "Next"></asp:LinkButton>
            </td>
        </tr>
    </table>
</asp:Content>
