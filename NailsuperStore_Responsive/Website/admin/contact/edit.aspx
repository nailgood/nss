<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_contact_Edit"  Title="Contact Us"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If ContactId = 0 Then %>Add<% Else %>Edit<% End If %> Contact Us</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="optional">Subject Id:</td>
		<td class="field"><asp:DropDownList id="drpSubjectId" runat="server" AutoPostBack="true" /></td>
		<td></td>
	</tr>
	<tr>
		<td class="required">First Name:</td>
		<td class="field"><asp:textbox id="txtFirstName" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvFirstName" runat="server" Display="Dynamic" ControlToValidate="txtFirstName" CssClass="msgError" ErrorMessage="Field 'First Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Last Name:</td>
		<td class="field"><asp:textbox id="txtLastName" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvLastName" runat="server" Display="Dynamic" ControlToValidate="txtLastName" CssClass="msgError" ErrorMessage="Field 'Last Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Email Address:</td>
		<td class="field"><asp:textbox id="txtEmailAddress" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server" Display="Dynamic" ControlToValidate="txtEmailAddress" CssClass="msgError" ErrorMessage="Field 'Email Address' is blank"></asp:RequiredFieldValidator><br />
            <CC:EmailValidator ID="fvEmail" runat="server" ControlToValidate="txtEmailAddress" Display="Dynamic"
                CssClass="msgError" ErrorMessage="Field 'Email' is invalid"></CC:EmailValidator></td>
	</tr>
	<tr>
		<td class="optional">Phone:</td>
		<td class="field"><asp:textbox id="txtPhone" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td>
            <asp:RegularExpressionValidator ID="reMinimumQuantityPurchase" runat="server" ControlToValidate="txtPhone"
                Display="Dynamic" CssClass="msgError" ErrorMessage="Field 'Phone' is invalid." ValidationExpression="^[+-]?[0-9^-]+$"
                Visible="true"></asp:RegularExpressionValidator></td>
	</tr>
	<tr>
		<td class="optional">Order Number:</td>
		<td class="field"><asp:textbox id="txtOrderNumber" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr runat="server" Id="trAdjustmenttype">
		<td class="optional">Adjustment Type:</td>
		<td class="field">
		    <asp:RadioButtonList runat="server" ID="rblAdjustmentType">
		        <asp:ListItem Value="1" Selected="True">Sales Price</asp:ListItem>
		        <asp:ListItem Value="2">Promo Code</asp:ListItem>
		    </asp:RadioButtonList>
		</td>
		<td></td>
	</tr>
	<tr runat="server" id="trItemAdjust">
		<td class="required">item(s) to Adjust:</td>
		<td class="field"><asp:TextBox id="txtItemAdjust" style="width: 349px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
		<td><asp:RequiredFieldValidator ID="rfvItemAdjust" runat="server" Display="Dynamic" ControlToValidate="txtItemAdjust" CssClass="msgError" ErrorMessage="Field 'Item(s) to Adjust' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Comments:</td>
		<td class="field"><asp:TextBox id="txtComments" style="width: 349px;" Columns="55" rows="5" TextMode="Multiline" runat="server"></asp:TextBox></td>
		<td><asp:RequiredFieldValidator ID="rfvComments" runat="server" Display="Dynamic" ControlToValidate="txtComments" CssClass="msgError" ErrorMessage="Field 'Comments' is blank"></asp:RequiredFieldValidator></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Contact Us?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>

