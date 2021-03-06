<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_emailTemplate_Edit"  Title="Contact Us"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If EmailId = 0 Then%>Add<% Else %>Edit<% End If %> Email Template</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Subject Id:</td>
		<td class="field"><table><tr><td><asp:DropDownList id="drpSubjectId" AutoPostBack="true" runat="server" /></td><td><asp:RequiredFieldValidator ID="rfvdrpSubjectId" runat="server" Display="Dynamic" ControlToValidate="drpSubjectId" CssClass="msgError" ErrorMessage="Field 'Subject Id' is blank"></asp:RequiredFieldValidator><div style="display:none " ><asp:textbox id="txtSubjectId" runat="server" maxlength="50" columns="30" style="width: 0px;" ></asp:textbox></div></td></tr></table></td>
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
		<td class="optional" ><table border="0" cellspacing="0" cellpadding="0">
		<tr><td class="optional">Contents:</td></tr>		
		<tr><td>
		<div id="dTemplate" runat="server">
		<table border="0" cellspacing="0" cellpadding="0">
		<tr><td style="width:150px">#FIRSTNAMECONTACT#</td><td>-</td><td >Firstname</td></tr>
		<tr><td>#LASTNAMECONTACT#</td><td>-</td><td>Lastname</td></tr>
		<tr><td>#EMAILCONTACT#</td><td>-</td><td>Email</td></tr>
		<tr><td>#PHONECONTACT#</td><td>-</td><td>Phone</td></tr>
		<tr><td>#COMMENTSCONTACT#</td><td>-</td><td>Comments</td></tr>
		<tr><td>#COUNTRYCONTACT#</td><td>-</td><td>Country</td></tr>
		<tr><td>#SALONCONTACT#</td><td>-</td><td>Salon</td></tr>
		<tr><td>#SHIPPINGCONTACT#</td><td>-</td><td>Shipping</td></tr>
		<tr><td>#CITYCONTACT#</td><td>-</td><td>City</td></tr>
		<tr><td>#PROVINCECONTACT#</td><td>-</td><td>Province</td></tr>
		<tr><td>#ZIPCODECONTACT#</td><td>-</td><td>Zipcode</td></tr>
		<tr><td>#INVOICECONTACT#</td><td>-</td><td>Invoice</td></tr>
		<tr><td>#ITEMNOTRECEIVED#</td><td>-</td><td>Item Not Received</td></tr>	
		<tr><td>#DAMAGED#</td><td>-</td><td>Damaged</td></tr>
		<tr><td>#DAMAGEDITEM#</td><td>-</td><td>Damaged Item</td></tr>
		<tr><td>#PACKAGE#</td><td>-</td><td>Package</td></tr>
		<tr><td>#CARTON#</td><td>-</td><td>Carton</td></tr>
		<tr><td>#RESHIP#</td><td>-</td><td>Reship</td></tr>
		<tr><td>#ITEM#</td><td>-</td><td>Item</td></tr>
		<tr><td>#DATA#</td><td>-</td><td>Data</td></tr>
		<tr><td>#PHONECOMPANY#</td><td>-</td><td>Competitor's Phone</td></tr>
		<tr><td>#WEBSITE#</td><td>-</td><td>Website</td></tr>
	
		</table>
		</div>
			<div id="dTemplateItem" runat="server">
			<table>
		<tr><td>Template Item</td><td>-</td><td></td></tr>
		<tr><td>#ITEMID#</td><td>-</td><td>ItemId</td></tr>
		<tr><td>#ITEMNAME#</td><td>-</td><td>ItemName</td></tr>
		<tr><td>#SKU#</td><td>-</td><td>Sku</td></tr>
		<tr><td>#WEIGHT#</td><td>-</td><td>Weight</td></tr>
		<tr><td>#PRICEDESC#</td><td>-</td><td>PriceDesc</td></tr>
		<tr><td>#IMAGE#</td><td>-</td><td>Image</td></tr>
		<tr><td>#SHORTDESC#</td><td>-</td><td>ShortDesc</td></tr>
		<tr><td>#LONGDESC#</td><td>-</td><td>LongDesc</td></tr>
		<tr><td>#SHORTVIET#</td><td>-</td><td>ShortViet</td></tr>
		<tr><td>#LONGVIET#</td><td>-</td><td>LongViet</td></tr>
		</table>
		</div>
		
		</td></tr>
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

