<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_coupon_Edit"  Title="Coupon"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If CouponId = 0 Then %>Add<% Else %>Edit<% End If %> Coupon</h4>

<table border="0" cellspacing="1" cellpadding="2"> 
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Name:</td>
		<td class="field"><asp:textbox id="txtName" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvName" runat="server" CssClass="msgError" Display="Dynamic" ControlToValidate="txtName" ErrorMessage="Field 'Name' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Image:</td>
		<td class="field"><CC:FileUpload AutoResize="true" ImageWidth="200" ImageHeight="750" ID="fuImage" Folder="/assets/coupon" ImageDisplayFolder="/assets/coupon" DisplayImage="True" runat="server" style="width: 319px;" /></td>
		<td><CC:RequiredFileUploadValidator ID="rfvImage" runat="server" CssClass="msgError" Display="Dynamic" ControlToValidate="fuImage" ErrorMessage="Field 'Image' is required"></CC:RequiredFileUploadValidator><CC:FileUploadExtensionValidator Extensions="jpg,jpeg,gif,bmp" ID="feImage" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="fuImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
	<tr>
		<td class="required">Referral:</td>
		<td class="field"><asp:DropDownList id="drpReferralId" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="rfvReferralId" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="drpReferralId" ErrorMessage="Field 'Referral' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="optional">Start Date:</td>
		<td class="field"><CC:DatePicker ID="dtStartDate" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvStartDate" ControlToValidate="dtStartDate" CssClass="msgError" ErrorMessage="Date Field 'Start Date' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">End Date:</td>
		<td class="field"><CC:DatePicker ID="dtEndDate" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvEndDate" ControlToValidate="dtEndDate" CssClass="msgError" ErrorMessage="Date Field 'End Date' is invalid" /></td>
	</tr>
	<tr>
		<td class="required"><b>Is Active?</b></td>
		<td class="field">
			<asp:RadioButtonList runat="server" ID="rblIsActive" RepeatDirection="Horizontal">
			<asp:ListItem Text="Yes" Value="True" />
			<asp:ListItem Text="No" Value="False" Selected="True" />
			</asp:RadioButtonList>
		</td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Coupon?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>
