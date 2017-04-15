<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_orders_creditmemo_Edit"  Title="Sales Credit Memo Header"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If MemoId = 0 Then %>Add<% Else %>Edit<% End If %> Sales Credit Memo Header</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Order Id:</td>
		<td class="field"><asp:textbox id="txtOrderId" runat="server" maxlength="4" columns="4" style="width: 43px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvOrderId" runat="server" Display="Dynamic" CssClass="msgError" ControlToValidate="txtOrderId" ErrorMessage="Field 'Order Id' is blank"></asp:RequiredFieldValidator><CC:IntegerValidator Display="Dynamic" CssClass="msgError" runat="server" id="fvOrderId" ControlToValidate="txtOrderId" ErrorMessage="Field 'Order Id' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Sell To Customer No:</td>
		<td class="field"><asp:textbox id="txtSellToCustomerNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">No:</td>
		<td class="field"><asp:textbox id="txtNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bill To Customer No:</td>
		<td class="field"><asp:textbox id="txtBillToCustomerNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bill To Name:</td>
		<td class="field"><asp:textbox id="txtBillToName" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bill To Name 2:</td>
		<td class="field"><asp:textbox id="txtBillToName2" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bill To Address:</td>
		<td class="field"><asp:textbox id="txtBillToAddress" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bill To Address 2:</td>
		<td class="field"><asp:textbox id="txtBillToAddress2" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bill To City:</td>
		<td class="field"><asp:textbox id="txtBillToCity" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bill To Contact:</td>
		<td class="field"><asp:textbox id="txtBillToContact" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Your Reference:</td>
		<td class="field"><asp:textbox id="txtYourReference" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Ship To Code:</td>
		<td class="field"><asp:textbox id="txtShipToCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Ship To Name:</td>
		<td class="field"><asp:textbox id="txtShipToName" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Ship To Name 2:</td>
		<td class="field"><asp:textbox id="txtShipToName2" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Ship To Address:</td>
		<td class="field"><asp:textbox id="txtShipToAddress" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Ship To Address 2:</td>
		<td class="field"><asp:textbox id="txtShipToAddress2" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Ship To City:</td>
		<td class="field"><asp:textbox id="txtShipToCity" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Ship To Contact:</td>
		<td class="field"><asp:textbox id="txtShipToContact" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Posting:</td>
		<td class="field"><CC:DatePicker ID="dtPosting" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvPosting" CssClass="msgError" ControlToValidate="dtPosting" ErrorMessage="Date Field 'Posting' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Shipment:</td>
		<td class="field"><CC:DatePicker ID="dtShipment" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvShipment" ControlToValidate="dtShipment" CssClass="msgError" ErrorMessage="Date Field 'Shipment' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Posting Description:</td>
		<td class="field"><asp:textbox id="txtPostingDescription" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Payment Terms Code:</td>
		<td class="field"><asp:textbox id="txtPaymentTermsCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Due:</td>
		<td class="field"><CC:DatePicker ID="dtDue" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvDue" ControlToValidate="dtDue" CssClass="msgError" ErrorMessage="Date Field 'Due' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Payment Discount Percent:</td>
		<td class="field"><asp:textbox id="txtPaymentDiscountPercent" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" CssClass="msgError" id="fvPaymentDiscountPercent" ControlToValidate="txtPaymentDiscountPercent" ErrorMessage="Field 'Payment Discount Percent' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Pmt Discount:</td>
		<td class="field"><CC:DatePicker ID="dtPmtDiscount" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" CssClass="msgError" id="dtvPmtDiscount" ControlToValidate="dtPmtDiscount" ErrorMessage="Date Field 'Pmt Discount' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Shipment Method Code:</td>
		<td class="field"><asp:textbox id="txtShipmentMethodCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Location Code:</td>
		<td class="field"><asp:textbox id="txtLocationCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Currency Code:</td>
		<td class="field"><asp:textbox id="txtCurrencyCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Customer Price Group:</td>
		<td class="field"><asp:textbox id="txtCustomerPriceGroup" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional"><b>Prices Including V A T?</b></td>
		<td class="field">
			<asp:RadioButtonList runat="server" ID="rblPricesIncludingVAT" RepeatDirection="Horizontal">
			<asp:ListItem Text="Yes" Value="True" />
			<asp:ListItem Text="No" Value="False" Selected="True" />
			</asp:RadioButtonList>
		</td>
	</tr>
	<tr>
		<td class="optional">Invoice Disc Code:</td>
		<td class="field"><asp:textbox id="txtInvoiceDiscCode" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Customer Disc Group:</td>
		<td class="field"><asp:textbox id="txtCustomerDiscGroup" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Language Code:</td>
		<td class="field"><asp:textbox id="txtLanguageCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Salesperson Code:</td>
		<td class="field"><asp:textbox id="txtSalespersonCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Appliesto Doc Type:</td>
		<td class="field"><asp:textbox id="txtAppliestoDocType" runat="server" maxlength="19" columns="19" style="width: 133px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Appliesto Doc No:</td>
		<td class="field"><asp:textbox id="txtAppliestoDocNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bal Account No:</td>
		<td class="field"><asp:textbox id="txtBalAccountNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Job No:</td>
		<td class="field"><asp:textbox id="txtJobNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Amount:</td>
		<td class="field"><asp:textbox id="txtAmount" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" CssClass="msgError" runat="server" id="fvAmount" ControlToValidate="txtAmount" ErrorMessage="Field 'Amount' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Amount Including V A T:</td>
		<td class="field"><asp:textbox id="txtAmountIncludingVAT" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" CssClass="msgError" id="fvAmountIncludingVAT" ControlToValidate="txtAmountIncludingVAT" ErrorMessage="Field 'Amount Including V A T' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">V A T Registration No:</td>
		<td class="field"><asp:textbox id="txtVATRegistrationNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Reason Code:</td>
		<td class="field"><asp:textbox id="txtReasonCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Transaction Type:</td>
		<td class="field"><asp:textbox id="txtTransactionType" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Transport Method:</td>
		<td class="field"><asp:textbox id="txtTransportMethod" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Sell To Customer Name:</td>
		<td class="field"><asp:textbox id="txtSellToCustomerName" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Sell To Customer Name 2:</td>
		<td class="field"><asp:textbox id="txtSellToCustomerName2" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Sell To Address:</td>
		<td class="field"><asp:textbox id="txtSellToAddress" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Sell To Address 2:</td>
		<td class="field"><asp:textbox id="txtSellToAddress2" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Sell To City:</td>
		<td class="field"><asp:textbox id="txtSellToCity" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Sell To Contact:</td>
		<td class="field"><asp:textbox id="txtSellToContact" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bill To Post Code:</td>
		<td class="field"><asp:textbox id="txtBillToPostCode" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bill To County:</td>
		<td class="field"><asp:textbox id="txtBillToCounty" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bill To Country Code:</td>
		<td class="field"><asp:textbox id="txtBillToCountryCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Sell To Post Code:</td>
		<td class="field"><asp:textbox id="txtSellToPostCode" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Sell To County:</td>
		<td class="field"><asp:textbox id="txtSellToCounty" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Sell To Country Code:</td>
		<td class="field"><asp:textbox id="txtSellToCountryCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Ship To Post Code:</td>
		<td class="field"><asp:textbox id="txtShipToPostCode" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Ship To County:</td>
		<td class="field"><asp:textbox id="txtShipToCounty" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Ship To Country Code:</td>
		<td class="field"><asp:textbox id="txtShipToCountryCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bal Account Type:</td>
		<td class="field"><asp:textbox id="txtBalAccountType" runat="server" maxlength="12" columns="12" style="width: 91px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Documentdatetime:</td>
		<td class="field"><CC:DatePicker ID="dtDocumentdatetime" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvDocumentdatetime" CssClass="msgError" ControlToValidate="dtDocumentdatetime" ErrorMessage="Date Field 'Documentdatetime' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">External Document No:</td>
		<td class="field"><asp:textbox id="txtExternalDocumentNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Payment Method Code:</td>
		<td class="field"><asp:textbox id="txtPaymentMethodCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">User I D:</td>
		<td class="field"><asp:textbox id="txtUserID" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Source Code:</td>
		<td class="field"><asp:textbox id="txtSourceCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Tax Area Code:</td>
		<td class="field"><asp:textbox id="txtTaxAreaCode" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional"><b>Tax Liable?</b></td>
		<td class="field">
			<asp:RadioButtonList runat="server" ID="rblTaxLiable" RepeatDirection="Horizontal">
			<asp:ListItem Text="Yes" Value="True" />
			<asp:ListItem Text="No" Value="False" Selected="True" />
			</asp:RadioButtonList>
		</td>
	</tr>
	<tr>
		<td class="optional">Campaign No:</td>
		<td class="field"><asp:textbox id="txtCampaignNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Sell To Contact No:</td>
		<td class="field"><asp:textbox id="txtSellToContactNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bill To Contact No:</td>
		<td class="field"><asp:textbox id="txtBillToContactNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Responsibility Center:</td>
		<td class="field"><asp:textbox id="txtResponsibilityCenter" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Sales Credit Memo Header?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>
