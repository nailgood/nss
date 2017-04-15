<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_jobs_Edit"  Title="Job"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If JobId = 0 Then %>Add<% Else %>Edit<% End If %> Job</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Category:</td>
		<td class="field"><asp:DropDownList id="drpCategoryId" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="rfvCategoryId" runat="server" Display="Dynamic" ControlToValidate="drpCategoryId" CssClass="msgError" ErrorMessage="Field 'Category Id' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Company:</td>
		<td class="field"><asp:textbox id="txtCompany" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvCompany" runat="server" Display="Dynamic" ControlToValidate="txtCompany" CssClass="msgError" ErrorMessage="Field 'Company' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Job Title:</td>
		<td class="field"><asp:textbox id="txtTitle" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvCity" runat="server" Display="Dynamic" ControlToValidate="txtTitle" CssClass="msgError" ErrorMessage="Field 'Job Title' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">City:</td>
		<td class="field"><asp:textbox id="txtCity" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvTitle" runat="server" Display="Dynamic" ControlToValidate="txtCity" CssClass="msgError" ErrorMessage="Field 'City' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">State:</td>
		<td class="field"><asp:DropDownList id="drpState" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="rfvState" runat="server" Display="Dynamic" ControlToValidate="drpState" CssClass="msgError" ErrorMessage="Field 'State' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Zip:</td>
		<td class="field"><asp:textbox id="txtZip" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvZip" runat="server" Display="Dynamic" ControlToValidate="txtZip" CssClass="msgError" ErrorMessage="Field 'Zip' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Email:</td>
		<td class="field"><asp:textbox id="txtEmail" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvEmail" runat="server" Display="Dynamic" ControlToValidate="txtEmail" CssClass="msgError" ErrorMessage="Field 'Email' is blank"></asp:RequiredFieldValidator>
            <CC:EmailValidator ID="fvEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic"
                CssClass="msgError" ErrorMessage="Field 'Email' is invalid"></CC:EmailValidator></td>
	</tr>
	<tr>
		<td class="optional">Phone:</td>
		<td class="field"><asp:textbox id="txtPhone" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="required">Short Description:</td>
		<td class="field"><asp:textbox id="txtShortDescription" runat="server" maxlength="500" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvShortDescription" runat="server" Display="Dynamic" ControlToValidate="txtShortDescription" CssClass="msgError" ErrorMessage="Field 'Short Description' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Description:</td>
		<td class="field"><asp:TextBox id="txtDescription" style="width: 349px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
		<td><asp:RequiredFieldValidator ID="rfvDescription" runat="server" Display="Dynamic" ControlToValidate="txtDescription" CssClass="msgError" ErrorMessage="Field 'Description' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Full/Part Time:</td>
		<td class="field"><asp:textbox id="txtFullPartTime" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Requirements:</td>
		<td class="field"><asp:TextBox id="txtRequirements" style="width: 349px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Comments:</td>
		<td class="field"><asp:TextBox id="txtComments" style="width: 349px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Compensation:</td>
		<td class="field"><asp:TextBox id="txtCompensation" style="width: 349px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Benefits:</td>
		<td class="field"><asp:TextBox id="txtBenefits" style="width: 349px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
		<td></td>
	</tr>
	<tr>
		<td class="required">Expiration Date:</td>
		<td class="field"><CC:DatePicker ID="dtExpirationDate" runat="server"></CC:DatePicker></td>
		<td><CC:RequiredDateValidator Display="Dynamic" runat="server" id="rdtvExpirationDate" ControlToValidate="dtExpirationDate" CssClass="msgError" ErrorMessage="Date Field 'Expiration Date' is blank" /><CC:DateValidator Display="Dynamic" runat="server" id="dtvExpirationDate" ControlToValidate="dtExpirationDate" CssClass="msgError" ErrorMessage="Date Field 'Expiration Date' is invalid" /></td>
	</tr>
	<tr>
		<td class="required"><b>Is Active?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsActive" /></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Job?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

