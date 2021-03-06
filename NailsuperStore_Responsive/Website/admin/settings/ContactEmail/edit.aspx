<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_contactEmail_Edit"  Title="Contact Us"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4><% If EmailId = 0 Then%>Add<% Else %>Edit<% End If %> Contact Email</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Name:</td>
		<td class="field"><asp:textbox id="txtName" runat="server" maxlength="30" columns="30" style="width: 319px;" ></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="msgError" Display="Dynamic" ControlToValidate="txtName" ErrorMessage="Field 'Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>	
	<tr>
		<td class="required">Email:</td>
		<td class="field"><asp:textbox id="txtEmailAddress" runat="server" maxlength="50" columns="50" style="width: 319px;" ></asp:textbox></td>
		<td>
		    <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" CssClass="msgError" Display="Dynamic" ControlToValidate="txtEmailAddress" ErrorMessage="Field 'Email' is blank"></asp:RequiredFieldValidator>
		    <asp:RegularExpressionValidator runat="server" CssClass="msgError" ID="evtxtBillingEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ControlToValidate="txtEmailAddress" Display="Dynamic" ErrorMessage="Field 'Email' is invalid."  />
            <asp:Label ID="lblEmail" runat="server" CssClass="red"></asp:Label>
		</td>
	</tr>	
	<tr>
	    <td class="optional">Subject: </td>
	    <td  class="field">
	        <CC:CheckBoxList  ID="cblSubject" runat="server" RepeatColumns="1" CellPadding="0"
                    CellSpacing="1"></CC:CheckBoxList>
	    </td>
	    <td>
            <asp:Label ID="lblSubject" runat="server" CssClass="red"></asp:Label>
	    </td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Contact Us?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

