<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" CodeFile="edit.aspx.vb" Inherits="admin_resume_edit" %>


<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If Resumeid = 0 Then %>Add<% Else %>Edit<% End If %> Job Seekers</h4>

<table border="0" cellspacing="1" cellpadding="2">
	
	<tr>
		<td class="optional">Job Category:</td>
		<td class="field"><asp:DropDownList ID="F_JobCategory" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">First Name:</td>
		<td class="field"><asp:textbox id="txtFirstName" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvFirstName" runat="server" Display="Dynamic" ControlToValidate="txtFirstName" CssClass="msgError" ErrorMessage="Field 'First Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Last Name:</td>
		<td class="field"><asp:textbox id="txtLastName" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvLastName" runat="server" Display="Dynamic" ControlToValidate="txtLastName" CssClass="msgError" ErrorMessage="Field 'Last Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Email Address:</td>
		<td class="field"><asp:textbox id="txtEmailAddress" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Phone:</td>
		<td class="field"><asp:textbox id="txtPhone" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Address1:</td>
		<td class="field"><asp:textbox id="txtAddress1" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Address2:</td>
		<td class="field"><asp:textbox id="txtAddress2" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">City:</td>
		<td class="field"><asp:textbox id="txtCity" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">ZipCode:</td>
		<td class="field"><asp:textbox id="txtZipCode" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	
	
	<tr>
		<td class="optional">Relocate:</td>
		<td class="field"><asp:CheckBox id="chkRelocate" runat="server"></asp:CheckBox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Hour Requirement:</td>
		<td class="field"><asp:textbox id="txtHourRequirement" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Licensed Salon Professional:</td>
		<td class="field"><asp:textbox id="txtLicensedSalonProfessional" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Student:</td>
		<td class="field"><asp:CheckBox ID="chkStudent" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Years Experience:</td>
		<td class="field"><asp:textbox id="txtYearsExperience" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Resume:</td>
		<td class="field"><asp:textbox id="txtResume" runat="server" columns="55" style="width: 319px;" Rows="5" TextMode="MultiLine"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Cover Letter:</td>
		<td class="field"><asp:textbox id="txtCoverLetter" runat="server" maxlength="50" columns="55" style="width: 319px;" Rows="5" TextMode="MultiLine"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Profile Type:</td>
		<td class="field"><asp:textbox id="txtProfileType" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Is Agreed:</td>
		<td class="field"><asp:CheckBox ID="chkIsAgreed" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Is Active:</td>
		<td class="field"><asp:CheckBox ID="chkIsActive" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Contact Us?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

