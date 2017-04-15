<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_shipping_Edit"  Title="Shipping Range"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If ShippingRangeId = 0 Then %>Add<% Else %>Edit<% End If %> Shipping Range</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Method:</td>
		<td class="field"><asp:DropDownList id="drpMethodId" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="rfvMethodId" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="drpMethodId" ErrorMessage="Field 'Method Id' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Lower Zipcode:</td>
		<td class="field"><asp:textbox id="txtLowValue" runat="server" maxlength="5" columns="5" style="width: 49px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvLowValue" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtLowValue" ErrorMessage="Field 'Low Value' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Upper Zipcode:</td>
		<td class="field"><asp:textbox id="txtHighValue" runat="server" maxlength="5" columns="5" style="width: 49px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvHighValue" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtHighValue" ErrorMessage="Field 'High Value' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Order Threshold ($):<br /><span class="smaller" style="color:Black;font-weight:normal;">When order total is under this value<br />use 'First Pound Under,' otherwise<br />use 'First Pound Over'</span></td>
		<td class="field"><asp:textbox id="txtOverUnderValue" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvOverUnderValue" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtOverUnderValue" ErrorMessage="Field 'Weight Threshold' is blank"></asp:RequiredFieldValidator><CC:FloatValidator Display="Dynamic" runat="server" id="fvOverUnderValue" CssClass="msgError" ControlToValidate="txtOverUnderValue" ErrorMessage="Field 'Weight Threshold' is invalid" /></td>
	</tr>
	<tr>
		<td class="required">First Pound Over Threshold:</td>
		<td class="field"><asp:textbox id="txtFirstPoundOver" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvFirstPoundOver" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtFirstPoundOver" ErrorMessage="Field 'First Pound Over Threshold' is blank"></asp:RequiredFieldValidator><CC:FloatValidator Display="Dynamic" runat="server" id="fvFirstPoundOver" ControlToValidate="txtFirstPoundOver" CssClass="msgError" ErrorMessage="Field 'First Pound Over Threshold' is invalid" /></td>
	</tr>
	<tr>
		<td class="required">First Pound Under Threshold:</td>
		<td class="field"><asp:textbox id="txtFirstPoundUnder" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvFirstPoundUnder" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtFirstPoundUnder" ErrorMessage="Field 'First Pound Under Threshold' is blank"></asp:RequiredFieldValidator><CC:FloatValidator Display="Dynamic" runat="server" id="fvFirstPoundUnder" ControlToValidate="txtFirstPoundUnder" CssClass="msgError" ErrorMessage="Field 'First Pound Under Threshold' is invalid" /></td>
	</tr>
	<tr>
		<td class="required">Each Additional Pound Threshold:<br /><span class="smaller" style="color:Black;font-weight:normal;">Additional pound value is used only<br />when threshold is exceeded</span></td>
		<td class="field"><asp:textbox id="txtAdditionalThreshold" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvAdditionalThreshold" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtAdditionalThreshold" ErrorMessage="Field 'Each Additional Pound Threshold' is blank"></asp:RequiredFieldValidator><CC:FloatValidator Display="Dynamic" runat="server" id="fvAdditionalThreshold" ControlToValidate="txtAdditionalThreshold" CssClass="msgError" ErrorMessage="Field 'Each Additional Pound Threshold' is invalid" /></td>
	</tr>
	<tr>
		<td class="required">Each Additional Pound:</td>
		<td class="field"><asp:textbox id="txtAdditionalPound" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvAdditionalPound" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtAdditionalPound" ErrorMessage="Field 'Each Additional Pound' is blank"></asp:RequiredFieldValidator><CC:FloatValidator Display="Dynamic" runat="server" id="fvAdditionalPound" ControlToValidate="txtAdditionalPound" CssClass="msgError" ErrorMessage="Field 'Each Additional Pound' is invalid" /></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Shipping Range?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

