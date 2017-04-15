<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_orders_creditmemo_lines_Edit"  Title="Sales Credit Memo Line"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
<h4><% If MemoLineId = 0 Then %>Add<% Else %>Edit<% End If %> Sales Credit Memo Line</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Memo Id:</td>
		<td class="field"><asp:textbox id="txtMemoId" runat="server" maxlength="4" columns="4" style="width: 43px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvMemoId" runat="server" Display="Dynamic" ControlToValidate="txtMemoId" ErrorMessage="Field 'Memo Id' is blank"></asp:RequiredFieldValidator><CC:IntegerValidator Display="Dynamic" runat="server" id="fvMemoId" ControlToValidate="txtMemoId" ErrorMessage="Field 'Memo Id' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Sell To Customer No:</td>
		<td class="field"><asp:textbox id="txtSellToCustomerNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Document No:</td>
		<td class="field"><asp:textbox id="txtDocumentNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Line No:</td>
		<td class="field"><asp:textbox id="txtLineNo" runat="server" maxlength="4" columns="4" style="width: 43px;"></asp:textbox></td>
		<td><CC:IntegerValidator Display="Dynamic" runat="server" id="fvLineNo" ControlToValidate="txtLineNo" ErrorMessage="Field 'Line No' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Type:</td>
		<td class="field"><asp:textbox id="txtType" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">No:</td>
		<td class="field"><asp:textbox id="txtNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Location Code:</td>
		<td class="field"><asp:textbox id="txtLocationCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Shipment Date:</td>
		<td class="field"><CC:DatePicker ID="dtShipmentDate" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvShipmentDate" ControlToValidate="dtShipmentDate" ErrorMessage="Date Field 'Shipment Date' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Description:</td>
		<td class="field"><asp:textbox id="txtDescription" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Description 2:</td>
		<td class="field"><asp:textbox id="txtDescription2" runat="server" maxlength="50" columns="50" style="width: 319px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Unitof Measure:</td>
		<td class="field"><asp:textbox id="txtUnitofMeasure" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Quantity:</td>
		<td class="field"><asp:textbox id="txtQuantity" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvQuantity" ControlToValidate="txtQuantity" ErrorMessage="Field 'Quantity' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Unit Price:</td>
		<td class="field"><asp:textbox id="txtUnitPrice" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvUnitPrice" ControlToValidate="txtUnitPrice" ErrorMessage="Field 'Unit Price' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">V A T Percent:</td>
		<td class="field"><asp:textbox id="txtVATPercent" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvVATPercent" ControlToValidate="txtVATPercent" ErrorMessage="Field 'V A T Percent' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Line Discount Percent:</td>
		<td class="field"><asp:textbox id="txtLineDiscountPercent" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvLineDiscountPercent" ControlToValidate="txtLineDiscountPercent" ErrorMessage="Field 'Line Discount Percent' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Line Discount Amount:</td>
		<td class="field"><asp:textbox id="txtLineDiscountAmount" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvLineDiscountAmount" ControlToValidate="txtLineDiscountAmount" ErrorMessage="Field 'Line Discount Amount' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Amount:</td>
		<td class="field"><asp:textbox id="txtAmount" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvAmount" ControlToValidate="txtAmount" ErrorMessage="Field 'Amount' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Amount Including V A T:</td>
		<td class="field"><asp:textbox id="txtAmountIncludingVAT" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvAmountIncludingVAT" ControlToValidate="txtAmountIncludingVAT" ErrorMessage="Field 'Amount Including V A T' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional"><b>Allow Invoice Disc?</b></td>
		<td class="field">
			<asp:RadioButtonList runat="server" ID="rblAllowInvoiceDisc" RepeatDirection="Horizontal">
			<asp:ListItem Text="Yes" Value="True" />
			<asp:ListItem Text="No" Value="False" Selected="True" />
			</asp:RadioButtonList>
		</td>
	</tr>
	<tr>
		<td class="optional">Gross Weight:</td>
		<td class="field"><asp:textbox id="txtGrossWeight" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvGrossWeight" ControlToValidate="txtGrossWeight" ErrorMessage="Field 'Gross Weight' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Net Weight:</td>
		<td class="field"><asp:textbox id="txtNetWeight" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvNetWeight" ControlToValidate="txtNetWeight" ErrorMessage="Field 'Net Weight' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Unitsper Parcel:</td>
		<td class="field"><asp:textbox id="txtUnitsperParcel" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvUnitsperParcel" ControlToValidate="txtUnitsperParcel" ErrorMessage="Field 'Unitsper Parcel' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Unit Volume:</td>
		<td class="field"><asp:textbox id="txtUnitVolume" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvUnitVolume" ControlToValidate="txtUnitVolume" ErrorMessage="Field 'Unit Volume' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Appl To Item Entry:</td>
		<td class="field"><asp:textbox id="txtApplToItemEntry" runat="server" maxlength="4" columns="4" style="width: 43px;"></asp:textbox></td>
		<td><CC:IntegerValidator Display="Dynamic" runat="server" id="fvApplToItemEntry" ControlToValidate="txtApplToItemEntry" ErrorMessage="Field 'Appl To Item Entry' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Job Applies To I D:</td>
		<td class="field"><asp:textbox id="txtJobAppliesToID" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Work Type Code:</td>
		<td class="field"><asp:textbox id="txtWorkTypeCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bill To Customer No:</td>
		<td class="field"><asp:textbox id="txtBillToCustomerNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Inv Discount Amount:</td>
		<td class="field"><asp:textbox id="txtInvDiscountAmount" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvInvDiscountAmount" ControlToValidate="txtInvDiscountAmount" ErrorMessage="Field 'Inv Discount Amount' is invalid" /></td>
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
		<td class="optional">Tax Group Code:</td>
		<td class="field"><asp:textbox id="txtTaxGroupCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Blanket Order No:</td>
		<td class="field"><asp:textbox id="txtBlanketOrderNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Blanket Order Line No:</td>
		<td class="field"><asp:textbox id="txtBlanketOrderLineNo" runat="server" maxlength="4" columns="4" style="width: 43px;"></asp:textbox></td>
		<td><CC:IntegerValidator Display="Dynamic" runat="server" id="fvBlanketOrderLineNo" ControlToValidate="txtBlanketOrderLineNo" ErrorMessage="Field 'Blanket Order Line No' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">V A T Base Amount:</td>
		<td class="field"><asp:textbox id="txtVATBaseAmount" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvVATBaseAmount" ControlToValidate="txtVATBaseAmount" ErrorMessage="Field 'V A T Base Amount' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Unit Cost:</td>
		<td class="field"><asp:textbox id="txtUnitCost" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvUnitCost" ControlToValidate="txtUnitCost" ErrorMessage="Field 'Unit Cost' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Line Amount:</td>
		<td class="field"><asp:textbox id="txtLineAmount" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvLineAmount" ControlToValidate="txtLineAmount" ErrorMessage="Field 'Line Amount' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Variant Code:</td>
		<td class="field"><asp:textbox id="txtVariantCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Bin Code:</td>
		<td class="field"><asp:textbox id="txtBinCode" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Qty Per Unit Of Measure:</td>
		<td class="field"><asp:textbox id="txtQtyPerUnitOfMeasure" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvQtyPerUnitOfMeasure" ControlToValidate="txtQtyPerUnitOfMeasure" ErrorMessage="Field 'Qty Per Unit Of Measure' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Unit Of Measure Code:</td>
		<td class="field"><asp:textbox id="txtUnitOfMeasureCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Quantity Base:</td>
		<td class="field"><asp:textbox id="txtQuantityBase" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" id="fvQuantityBase" ControlToValidate="txtQuantityBase" ErrorMessage="Field 'Quantity Base' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Responsibility Center:</td>
		<td class="field"><asp:textbox id="txtResponsibilityCenter" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Cross Reference No:</td>
		<td class="field"><asp:textbox id="txtCrossReferenceNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Unitof Measure Cross Ref:</td>
		<td class="field"><asp:textbox id="txtUnitofMeasureCrossRef" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Cross Reference Type:</td>
		<td class="field"><asp:textbox id="txtCrossReferenceType" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Cross Reference Type No:</td>
		<td class="field"><asp:textbox id="txtCrossReferenceTypeNo" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Item Category Code:</td>
		<td class="field"><asp:textbox id="txtItemCategoryCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional"><b>Nonstock?</b></td>
		<td class="field">
			<asp:RadioButtonList runat="server" ID="rblNonstock" RepeatDirection="Horizontal">
			<asp:ListItem Text="Yes" Value="True" />
			<asp:ListItem Text="No" Value="False" Selected="True" />
			</asp:RadioButtonList>
		</td>
	</tr>
	<tr>
		<td class="optional">Purchasing Code:</td>
		<td class="field"><asp:textbox id="txtPurchasingCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Product Group Code:</td>
		<td class="field"><asp:textbox id="txtProductGroupCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Return Receipt No:</td>
		<td class="field"><asp:textbox id="txtReturnReceiptNo" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Return Receipt Line No:</td>
		<td class="field"><asp:textbox id="txtReturnReceiptLineNo" runat="server" maxlength="4" columns="4" style="width: 43px;"></asp:textbox></td>
		<td><CC:IntegerValidator Display="Dynamic" runat="server" id="fvReturnReceiptLineNo" ControlToValidate="txtReturnReceiptLineNo" ErrorMessage="Field 'Return Receipt Line No' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Return Reason Code:</td>
		<td class="field"><asp:textbox id="txtReturnReasonCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional"><b>Allow Line Disc?</b></td>
		<td class="field">
			<asp:RadioButtonList runat="server" ID="rblAllowLineDisc" RepeatDirection="Horizontal">
			<asp:ListItem Text="Yes" Value="True" />
			<asp:ListItem Text="No" Value="False" Selected="True" />
			</asp:RadioButtonList>
		</td>
	</tr>
	<tr>
		<td class="optional">Customer Disc Group:</td>
		<td class="field"><asp:textbox id="txtCustomerDiscGroup" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Package Tracking No:</td>
		<td class="field"><asp:textbox id="txtPackageTrackingNo" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Sales Credit Memo Line?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>
