<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_shippingint_Edit"  Title="International Shipping Range"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If ShippingRangeIntId = 0 Then %>Add<% Else %>Edit<% End If %> International Shipping Ranges</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Method:</td>
		<td class="field"><asp:DropDownList id="drpMethodId" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="rfvMethodId" runat="server" Display="Dynamic" ControlToValidate="drpMethodId" CssClass="msgError" ErrorMessage="Field 'Method' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Country Code:</td>
		<td class="field"><asp:DropDownList id="drpCountryCode" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="rfvCountryCode" runat="server" Display="Dynamic" ControlToValidate="drpCountryCode" CssClass="msgError" ErrorMessage="Field 'Country Code' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Order Threshold ($):<br /><span class="smaller" style="color:Black;font-weight:normal;">When order total is under this value<br />use 'First Pound Under,' otherwise<br />use 'First Pound Over'</span></td>
		<td class="field"><asp:textbox id="txtOverUnderValue" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvOverUnderValue" runat="server" Display="Dynamic" ControlToValidate="txtOverUnderValue" CssClass="msgError"  ErrorMessage="Field 'Over Under Value' is blank"></asp:RequiredFieldValidator><CC:FloatValidator Display="Dynamic" runat="server" id="fvOverUnderValue" ControlToValidate="txtOverUnderValue" CssClass="msgError" ErrorMessage="Field 'Over Under Value' is invalid" /></td>
	</tr>
	<tr>
		<td class="required">First Pound Over:</td>
		<td class="field"><asp:textbox id="txtFirstPoundOver" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvFirstPoundOver" runat="server" CssClass="msgError" Display="Dynamic" ControlToValidate="txtFirstPoundOver"  ErrorMessage="Field 'First Pound Over' is blank"></asp:RequiredFieldValidator><CC:FloatValidator Display="Dynamic" runat="server" id="fvFirstPoundOver" ControlToValidate="txtFirstPoundOver" CssClass="msgError" ErrorMessage="Field 'First Pound Over' is invalid" /></td>
	</tr>
	<tr>
		<td class="required">First Pound Under:</td>
		<td class="field"><asp:textbox id="txtFirstPoundUnder" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvFirstPoundUnder" runat="server" Display="Dynamic" ControlToValidate="txtFirstPoundUnder" CssClass="msgError" ErrorMessage="Field 'First Pound Under' is blank"></asp:RequiredFieldValidator><CC:FloatValidator Display="Dynamic" runat="server" id="fvFirstPoundUnder" ControlToValidate="txtFirstPoundUnder" CssClass="msgError" ErrorMessage="Field 'First Pound Under' is invalid" /></td>
	</tr>
	<tr>
		<td class="required">Each Additional Pound Threshold:<br /><span class="smaller" style="color:Black;font-weight:normal;">Additional pound value is used only<br />when threshold is exceeded</span></td>
		<td class="field"><asp:textbox id="txtAdditionalPound" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvAdditionalPound" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtAdditionalPound" ErrorMessage="Field 'Additional Pound' is blank"></asp:RequiredFieldValidator><CC:FloatValidator Display="Dynamic" runat="server" id="fvAdditionalPound" ControlToValidate="txtAdditionalPound" CssClass="msgError" ErrorMessage="Field 'Additional Pound' is invalid" /></td>
	</tr>
	<tr>
		<td class="required">Additional Threshold:</td>
		<td class="field"><asp:textbox id="txtAdditionalThreshold" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvAdditionalThreshold" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtAdditionalThreshold" ErrorMessage="Field 'Additional Threshold' is blank"></asp:RequiredFieldValidator><CC:FloatValidator Display="Dynamic" runat="server" id="fvAdditionalThreshold" ControlToValidate="txtAdditionalThreshold" CssClass="msgError" ErrorMessage="Field 'Additional Threshold' is invalid" /></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this International Shipping Ranges?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

