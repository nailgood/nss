<%@ Page Language="VB" MasterPageFile="~/includes/masterpage/admin.master" AutoEventWireup="false" CodeFile="index.aspx.vb" Inherits="admin_password_PasswordChange" title="Change Password" %>

<asp:Content ID="Content" ContentPlaceHolderID="ph" Runat="Server">

<h4>Change Password</h4>

<TABLE cellSpacing="2" cellPadding="3" border="0">
    <TR><TD colSpan="2"><SPAN class="red">red color</SPAN> - required fields</TD></TR>
    <TR vAlign="middle">
        <td class="required"><b>Old Password:</b></td>
        <TD class="field"><asp:TextBox id="PASSWORD_OLD" Runat="server" Columns="20" MaxLength="20" Width="200" TextMode="Password"></asp:TextBox></TD>
        <TD><asp:RequiredFieldValidator id="REQ_PASSWORD_OLD" Runat="server" Display="Dynamic" ControlToValidate="PASSWORD_OLD" CssClass="msgError" ErrorMessage="Old Password is Required"></asp:RequiredFieldValidator></TD>
    </TR>
    <TR vAlign="middle">
        <td class="required"><b>New Password:</b></td>
        <TD class="fielD"><asp:TextBox id="PASSWORD_NEW" Runat="server" Columns="20" MaxLength="20" Width="200" TextMode="Password"></asp:TextBox></TD>
        <TD><asp:RequiredFieldValidator id="REQ_PASSWORD_NEW" Runat="server" Display="Dynamic" ControlToValidate="PASSWORD_NEW" CssClass="msgError" ErrorMessage="New Password is Required"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator id="RegularExpressionValidator1" Runat="server" Display="Dynamic" ControlToValidate="PASSWORD_NEW" CssClass="msgError" ErrorMessage="<br>Password must be between 8 and 32 characters long,<br> Password must contain an uppercase letter,<br> Password must contain a number,<br> Password must contain a symbol (!@#$%^&*)" ValidationExpression="(?=^.{8,32}$)(?=.*\d)(?![.\n])(?=.*[!@#$%^&*)(?=.*[A-Z]).*$"></asp:RegularExpressionValidator></TD>
    </TR>
    <TR vAlign="middle">
        <Td class="required"><b>Confirm Password:</b></Td>
        <TD class="field"><asp:TextBox id="PASSWORD_CONFIRM" Runat="server" Columns="20" MaxLength="20" Width="200" TextMode="Password"></asp:TextBox></TD>
        <TD><asp:RequiredFieldValidator id="REQ_PASSWORD_CONFIRM" Runat="server" Display="Dynamic" ControlToValidate="PASSWORD_CONFIRM" CssClass="msgError" ErrorMessage="Confirm Password is Required"></asp:RequiredFieldValidator>
            <asp:CompareValidator id="CMP_PASSWORD" Runat="server" Display="Dynamic" ControlToValidate="PASSWORD_CONFIRM" CssClass="msgError" ErrorMessage="New Passwords do not Match" ControlToCompare="PASSWORD_NEW"></asp:CompareValidator></TD>
    </TR>
</TABLE>

<p></p>
<asp:Button id="submitButton" Runat="server" CssClass="btn" Text="Save"></asp:Button>
<asp:button id="Cancel" runat="server" Text="Cancel" CausesValidation="False" cssClass="btn"></asp:button>

</asp:Content>

