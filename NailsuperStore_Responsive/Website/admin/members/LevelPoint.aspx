<%@ Page Language="VB" AutoEventWireup="false" CodeFile="LevelPoint.aspx.vb" Inherits="admin_members_LevelPoint" MasterPageFile="~/includes/masterpage/admin.master" Title="Level Points"%>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ph">
    <h4>Level Point</h4>
<div id="dLoad" runat="server">
<asp:Label ID="lblMsg" runat="server" CssClass="bold red"></asp:Label>
</div>
<div id="dEdit" runat="server"> 
 <table border="0" cellpadding="2" cellspacing="1">
<%-- <tr>
 <td colspan="2"></td>
             <td class="bold field">Year </td>
             <td class="smaller field"><asp:DropDownList ID="drpYear" runat="server"></asp:DropDownList></td>
             <td></td>
</tr>--%>
 <tr>
            <td class="required">
                Silver Member Discount:</td>
            <td class="field">
            <table>
            <tr>
                <td>Discount</td>
                <td><asp:TextBox ID="txtSMember" runat="server" Columns="50" MaxLength="50" Style="width: 50px;"></asp:TextBox>%</td>
                <td><CC:IntegerValidator ID="IntegerValidator2" runat="server" ControlToValidate="txtSMember"
                                CssClass="msgError" ErrorMessage="Please enter a valid Discount" Display="Dynamic" /> <asp:RequiredFieldValidator ID="rfvSMember" runat="server" ControlToValidate="txtSMember"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Field 'Sivler Member Discount' is blank"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td>Start Point</td>
                <td><asp:TextBox ID="txtSStartPoint" runat="server" Columns="50" MaxLength="50" Style="width: 50px;"></asp:TextBox></td>
                <td><CC:IntegerValidator ID="IntegerValidator1" runat="server" ControlToValidate="txtSStartPoint"
                                CssClass="msgError" ErrorMessage="Please enter a valid Start Point" Display="Dynamic" /> <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtSStartPoint"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Field 'Start Point' is blank"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td>Message</td>
                <td><asp:TextBox ID="txtSMsg" runat="server" Columns="50" MaxLength="50" Style="width: 300px;"></asp:TextBox></td>
                <td> <asp:RequiredFieldValidator ID="rfvSMsg" runat="server" ControlToValidate="txtSMsg"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Field 'Sivler Message' is blank"></asp:RequiredFieldValidator></td>
            </tr>
            </table>
                </td>
            <td>
               </td>
        </tr>
         <tr>
            <td class="required">
                Gold Member Discount:</td>
            <td class="field">
            <table>
                <tr>
                    <td>Discount</td>
                    <td><asp:TextBox ID="txtGMember" runat="server" Columns="50" MaxLength="50" Style="width: 50px;"></asp:TextBox>%</td>
                    <td><CC:IntegerValidator ID="IntegerValidator4" runat="server" ControlToValidate="txtGMember"
                                CssClass="msgError" ErrorMessage="Please enter a valid Discount" Display="Dynamic" />
                    <asp:RequiredFieldValidator ID="rfvGMember" runat="server" ControlToValidate="txtGMember"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Field 'Gold Member Discount' is blank"></asp:RequiredFieldValidator></td>
                </tr>
                 <tr>
                    <td>Start Point</td>
                    <td><asp:TextBox ID="txtGStartPoint" runat="server" Columns="50" MaxLength="50" Style="width: 50px;"></asp:TextBox></td>
                    <td><CC:IntegerValidator ID="IntegerValidator3" runat="server" ControlToValidate="txtGStartPoint"
                                CssClass="msgError" ErrorMessage="Please enter a valid Start Point" Display="Dynamic" /><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtGStartPoint"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Field 'Start Point' is blank"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td>Message</td>
                    <td><asp:TextBox ID="txtGMsg" runat="server" Columns="50" MaxLength="50" Style="width: 300px;"></asp:TextBox></td>
                    <td><asp:RequiredFieldValidator ID="rfvGMsg" runat="server" ControlToValidate="txtGMsg"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Field 'Gold Message' is blank"></asp:RequiredFieldValidator></td>
                </tr>
            </table>
                </td>
            <td>
                </td>
        </tr>
         <tr>
            <td class="required">
                Platinum Member Discount:</td>
            <td class="field">
            <table>
                <tr>
                    <td>Discount</td>
                    <td><asp:TextBox ID="txtPMember" runat="server" Columns="50" MaxLength="50" Style="width: 50px;"></asp:TextBox>%</td>
                    <td><CC:IntegerValidator ID="IntegerValidator5" runat="server" ControlToValidate="txtPMember"
                                CssClass="msgError" ErrorMessage="Please enter a valid Discount" Display="Dynamic" /><asp:RequiredFieldValidator ID="rfvPMember" runat="server" ControlToValidate="txtPMember"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Field 'Platinum Member Discount' is blank"></asp:RequiredFieldValidator></td>
                </tr>
                 <tr>
                    <td>Start Point</td>
                    <td><asp:TextBox ID="txtPStartPoint" runat="server" Columns="50" MaxLength="50" Style="width: 50px;"></asp:TextBox></td>
                    <td><CC:IntegerValidator ID="IntegerValidator6" runat="server" ControlToValidate="txtPStartPoint"
                                CssClass="msgError" ErrorMessage="Please enter a valid Discount" Display="Dynamic" /><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPStartPoint"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Field 'Start Point' is blank"></asp:RequiredFieldValidator></td>
                </tr>
                 <tr>
                    <td>Message</td>
                    <td><asp:TextBox ID="txtPMsg" runat="server" Columns="50" MaxLength="50" Style="width: 300px;"></asp:TextBox></td>
                    <td><asp:RequiredFieldValidator ID="rfvPMsg" runat="server" ControlToValidate="txtPMsg"
                    Display="Dynamic" CssClass="msgError" ErrorMessage="Field 'Platinum Message' is blank"></asp:RequiredFieldValidator></td>
                </tr>
            </table>
                </td>
            <td>
                </td>
        </tr>
      
      
       
          
        
       
</table>
<br />
  <CC:OneClickButton id="btnSave" Runat="server" Text="Save" cssClass="btn" />&nbsp;<CC:OneClickButton id="btnCancel" Runat="server" Text="Cancel" cssClass="btn" />
</div>
</asp:Content>