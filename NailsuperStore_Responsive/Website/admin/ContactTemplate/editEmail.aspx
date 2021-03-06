<%@ Page Language="VB" AutoEventWireup="false" CodeFile="editEmail.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_emailTemplate_editEmail"  Title="Contact Us"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If EmailId = 0 Then%>Add<% Else %>Edit<% End If %> Email Template</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Subject Id:</td>
		<td class="field"><table><tr><td><asp:DropDownList id="drpSubjectId" runat="server" /></td><td><asp:RequiredFieldValidator ID="rfvdrpSubjectId" runat="server" Display="Dynamic" ControlToValidate="drpSubjectId" CssClass="msgError" ErrorMessage="Field 'Subject Id' is blank"></asp:RequiredFieldValidator></td></tr></table></td>
		<td></td>
	</tr>
	
	<tr>
		<td class="required">Name:</td>
		<td class="field"><table><tr><td><asp:textbox id="txtName" runat="server" maxlength="30" columns="30" style="width: 319px;" ></asp:textbox></td><td><asp:RequiredFieldValidator ID="rfvName" runat="server" Display="Dynamic" ControlToValidate="txtName" CssClass="msgError" ErrorMessage="Field 'Name' is blank"></asp:RequiredFieldValidator></td></tr></table></td>
		
	</tr>	
	<tr>
		<td class="required">Subject:</td>
		<td class="field"><table><tr><td><asp:textbox id="txtSubject" runat="server" maxlength="50" columns="30" style="width: 319px;" ></asp:textbox></td><td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtSubject" CssClass="msgError" ErrorMessage="Field 'Subject' is blank"></asp:RequiredFieldValidator></td></tr></table> </td>
		
	</tr>	
	<tr>
		<td class="required">Start Date:</td>
		<td class="field"><table><tr><td>  <CC:DatePicker ID="dtStartDate" runat="server"></CC:DatePicker></td><td><CC:RequiredDateValidator Display="Dynamic" runat="server" id="rdtvStartDate" ControlToValidate="dtStartDate" CssClass="msgError" ErrorMessage="Date Field 'Start Date' is blank" /><CC:DateValidator Display="Dynamic" runat="server" id="dtvStartDate" ControlToValidate="dtStartDate" CssClass="msgError" ErrorMessage="Date Field 'Start Date' is invalid" /></td></tr></table> </td>
		
	</tr>
	<tr>
		<td class="required">End Date:</td>
		<td class="field"><table><tr><td>  <CC:DatePicker ID="dtEndDate" runat="server"></CC:DatePicker></td><td><CC:RequiredDateValidator Display="Dynamic" runat="server" id="rdtvEndDate" ControlToValidate="dtEndDate" CssClass="msgError" ErrorMessage="Date Field 'End Date' is blank" /><CC:DateValidator Display="Dynamic" runat="server" id="dtvEndDate" ControlToValidate="dtEndDate" CssClass="msgError" ErrorMessage="Date Field 'End Date' is invalid" /></td></tr></table> </td>
		
	</tr>
	<tr>
		<td class="optional"><b>Is Active?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsActive" /></td>
	</tr>
	
	<tr>
		<td class="optional" ><table border="0" cellspacing="0" cellpadding="0">
		<tr><td class="optional">Contents:</td></tr>
		
		<tr><td><table border="0" cellspacing="0" cellpadding="0">
		<tr><td style="width:150px" >#FIRSTNAME#</td><td>-</td><td >Firstname</td></tr>
		<tr><td>#LASTNAME#</td><td>-</td><td>Lastname</td></tr>
		<tr><td>#USERNAME#</td><td>-</td><td>Username</td></tr>
		<tr><td>#PASSWORD#</td><td>-</td><td>Password</td></tr>
		<tr><td>#DATA#</td><td>-</td><td>Coupon</td></tr>
		</table></td></tr>
		</table></td>
		<td class="field"><asp:TextBox TextMode="MultiLine" id="txtContents" runat="server" Width="650" Height="500" /></td>
		<td></td>
	</tr>	
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Email Template?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

