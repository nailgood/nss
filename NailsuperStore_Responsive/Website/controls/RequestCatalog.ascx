<%@ Control Language="vb" AutoEventWireup="false" CodeFile="RequestCatalog.ascx.vb" Inherits="RequestCatalog" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<table cellSpacing="2" cellPadding="3" border="0">
	<tr>
		<td colSpan="2"><span class="red">red color</span> - required fields</td>
	</tr>
	<tr runat="server" visible="false">
		<td class="required"><b>Salutation:</b></td>
		<td class="field" width="300"><CC:DropDownListEx runat="server" id="Salutation" style="FONT-SIZE:11px" Height="17" /></CC:DropDownListEx></td>
		<td><asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" cssClass="blkten" ErrorMessage="Salutation is required"
					ControlToValidate="Salutation" Display="Dynamic" /></td>
	</tr>
	<tr>
		<td class="required"><b>First Name:</b></td>
		<td class="field" width="300"><asp:textbox id="FirstName" runat="server" columns="30" maxlength="50"></asp:textbox></td>
		<td><asp:requiredfieldvalidator id="RequiredFieldValidator2" runat="server" Display="Dynamic" ControlToValidate="FirstName"
				ErrorMessage="First Name is blank"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td class="required"><b>Last Name:</b></td>
		<td class="field" width="300"><asp:textbox id="LastName" runat="server" columns="30" maxlength="50"></asp:textbox></td>
		<td><asp:requiredfieldvalidator id="Requiredfieldvalidator3" runat="server" Display="Dynamic" ControlToValidate="LastName"
				ErrorMessage="Last Name is blank"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td class="optional"><b>Company:</b></td>
		<td class="field" width="300"><asp:textbox id="Company" runat="server" columns="50" maxlength="50"></asp:textbox></td>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td class="required"><b>Address 1:</b></td>
		<td class="field" width="300"><asp:textbox id="Address1" runat="server" columns="50" maxlength="50"></asp:textbox></td>
		<td><asp:requiredfieldvalidator id="Requiredfieldvalidator4" runat="server" Display="Dynamic" ControlToValidate="Address1"
			ErrorMessage="Address line 1 is blank"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td class="optional"><b>Address 2:</b></td>
		<td class="field" width="300"><asp:textbox id="Address2" runat="server" columns="50" maxlength="50"></asp:textbox></td>
		<td>&nbsp;</td>
	</tr>
	<tr>
		<td class="required"><b>City:</b></td>
		<td class="field" width="300"><asp:textbox id="City" runat="server" columns="30" maxlength="50"></asp:textbox></td>
		<td><asp:requiredfieldvalidator id="Requiredfieldvalidator5" runat="server" Display="Dynamic" ControlToValidate="City"
				ErrorMessage="City is blank"></asp:requiredfieldvalidator></td>
	</tr>
	<tr>
		<td class="required"><b>State:</b></td>
		<td class="field" width="300"><CC:DropDownListEx runat="server" id="State" style="FONT-SIZE:11px" Height="17"></CC:DropDownListEx></td>
		<td><asp:requiredfieldvalidator id="Requiredfieldvalidator6" runat="server" cssClass="blkten" ErrorMessage="State is required"
					ControlToValidate="State" Display="Dynamic" /></td>
	</tr>
	<tr>
		<td class="required"><b>Zip:</b></td>
		<td class="field" width="300"><CC:ZIP id="Zip" runat="server"></CC:ZIP></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional"><b>Phone:</b></td>
		<td class="field" width="300">
		 <table border=0><tr><td><asp:textbox id="Phone" runat="server" /></td><td>Ext: <asp:textbox id="Ext" columns="4" Runat="server" MaxLength="4"></asp:textbox></td></tr></table></td>
		<td></td>
	</tr>
	<tr>
		<td class="required"><b>Email:</b></td>
		<td class="field" width="300"><asp:textbox id="Email" runat="server" columns="50" maxlength="50"></asp:textbox></td>
		<td><asp:requiredfieldvalidator id="Requiredfieldvalidator7" runat="server" Display="Dynamic" ControlToValidate="Email"
				ErrorMessage="Email is blank"></asp:requiredfieldvalidator><CC:EMAILVALIDATOR id="CatalogEmailValidator" runat="server" Display="Dynamic" ControlToValidate="Email"
				ErrorMessage="Valid email address required"></CC:EMAILVALIDATOR></td>
	</tr>
	<tr>
		<td class="optional"><b>Date Requested:</b></td>
		<td class="field" width="300"><asp:Literal ID="ltlDateRequested" Runat="server" /></td>
		<td>&nbsp;</td>
	</tr>
	</table>
	<p>
	<asp:button id="Save" runat="server" Text="Save" cssClass="btn"></asp:button>
	<CC:ConfirmButton id="Delete" runat="server" Message="Are you sure want to delete this catalog request?"
		Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
	<asp:button id="Cancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></asp:button>