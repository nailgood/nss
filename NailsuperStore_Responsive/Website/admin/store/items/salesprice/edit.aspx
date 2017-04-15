<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_items_salesprice_Edit"  Title="Sales Price"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	
	<script language="javascript">
	<!--
	    if (window.addEventListener) {
	        window.addEventListener('load', InitializeQuery, false);
	    } else if (window.attachEvent) {
	        window.attachEvent('onload', InitializeQuery);
	    }
	    function MyCallback(mId) {
	            
	        document.getElementById('<%=MemberId1.ClientID %>').value = mId;
	  
		//GetItemEnableInfo();
	}
	
  
function SetType()
{
   
      InitQueryCode('LookupField', '/admin/ajax.aspx?f=DisplayItemEnable&Type=user&q=', MyCallback);
   
}
	function InitializeQuery() {
	
	InitQueryCode('LookupField', '/admin/ajax.aspx?f=DisplayItemEnable&Type=user&q=', MyCallback);
	
			}
		
	//-->
	</script>
<h4><% If SalesPriceId = 0 Then %>Add<% Else %>Edit<% End If %> Sales Price: <%=dbItem.ItemName%></h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan="2"><span class="smallest"><span class="red">red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="optional">Member:</td>
		<td class="field"><asp:DropDownList id="drpMemberId" runat="server" Visible="false" />
		<input type="text" id="LookupField" name="LookupField" onkeypress="javascript:SetType()" onmousedown="javascript:ResetType()" AutoCompleteType="Disabled" autocomplete="off" />
						<input type="hidden" name="MemberId1" id="MemberId1" runat="server" />
		</td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Customer Price Group:</td>
		<td class="field"><asp:dropdownlist id="drpCustomerPriceGroupId" runat="server" /></td>
		<td></td>
	</tr>
	<asp:placeholder runat="server" Visible="false">
	<tr>
		<td class="optional">Sales Code:</td>
		<td class="field"><asp:textbox id="txtSalesCode" runat="server" maxlength="20" columns="20" style="width: 139px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Currency Code:</td>
		<td class="field"><asp:textbox id="txtCurrencyCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	</asp:placeholder>
	<tr>
		<td class="optional">Starting Date:</td>
		<td class="field"><CC:DatePicker ID="dtStartingDate" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvStartingDate" CssClass="msgError" ControlToValidate="dtStartingDate" ErrorMessage="Date Field 'Starting Date' is invalid" /></td>
	</tr>
	<tr>
		<td class="optional">Ending Date:</td>
		<td class="field"><CC:DatePicker ID="dtEndingDate" runat="server"></CC:DatePicker></td>
		<td><CC:DateValidator Display="Dynamic" runat="server" id="dtvEndingDate"  CssClass="msgError" ControlToValidate="dtEndingDate" ErrorMessage="Date Field 'Ending Date' is invalid" /></td>
	</tr>
	<tr>
		<td class="required"><%=lbCase %> Sale Price:</td>
		<td class="field"><asp:textbox id="txtUnitPrice" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox><asp:Label ID="lbCasePrice" runat="server"></asp:Label></td>
		<td><asp:requiredfieldvalidator id="rvUnitPrice" runat="server" CssClass="msgError" ErrorMessage="Sale Price is required" ControlToValidate="txtUnitPrice" Display="Dynamic"></asp:requiredfieldvalidator>
		<CC:FloatValidator Display="Dynamic" runat="server" id="fvUnitPrice" CssClass="msgError" ControlToValidate="txtUnitPrice" ErrorMessage="Unit Price is invalid" /></td>
	</tr>
	<asp:placeholder ID="Placeholder1" runat="server" Visible="false">
	<tr>
		<td class="optional"><b>Price Includes VAT?</b></td>
		<td class="field">
			<asp:RadioButtonList runat="server" ID="rblPriceIncludesVAT" RepeatDirection="Horizontal">
			<asp:ListItem Text="Yes" Value="True" />
			<asp:ListItem Text="No" Value="False" Selected="True" />
			</asp:RadioButtonList>
		</td>
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
		<td class="optional">Sales Type:</td>
		<td class="field"><asp:textbox id="txtSalesType" runat="server" maxlength="4" columns="4" style="width: 43px;"></asp:textbox></td>
		<td><CC:IntegerValidator Display="Dynamic" runat="server" CssClass="msgError" id="fvSalesType" ControlToValidate="txtSalesType" ErrorMessage="Field 'Sales Type' is invalid" /></td>
	</tr>
	</asp:placeholder>
	<tr>
		<td class="required"><%=lbCase %> Minimum Quantity:</td>
		<td class="field"><asp:textbox id="txtMinimumQuantity" runat="server" maxlength="4" columns="4" style="width: 43px;"></asp:textbox><asp:Label ID="lbCaseQty" runat="server"></asp:Label></td>
		<td><CC:IntegerValidator Display="Dynamic" runat="server" CssClass="msgError" id="fvMinimumQuantity" ControlToValidate="txtMinimumQuantity" ErrorMessage="Minimum Quantity is invalid" />
		<asp:requiredfieldvalidator id="rvMinimumQuantity" CssClass="msgError" runat="server" ErrorMessage="Minimum Quantity is required" ControlToValidate="txtMinimumQuantity" Display="Dynamic"></asp:requiredfieldvalidator>
	    </td>
	</tr>
	<asp:placeholder ID="Placeholder2" runat="server" Visible="false">
	<tr>
		<td class="optional">Unit Of Measure Code:</td>
		<td class="field"><asp:textbox id="txtUnitOfMeasureCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
		<td></td>
	</tr>
	<tr>
		<td class="optional">Variant Code:</td>
		<td class="field"><asp:textbox id="txtVariantCode" runat="server" maxlength="10" columns="10" style="width: 79px;"></asp:textbox></td>
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
		<td class="optional">Unit Price Including VAT:</td>
		<td class="field"><asp:textbox id="txtUnitPriceIncludingVAT" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><CC:FloatValidator Display="Dynamic" runat="server" CssClass="msgError" id="fvUnitPriceIncludingVAT" ControlToValidate="txtUnitPriceIncludingVAT" ErrorMessage="Field 'Unit Price Including V A T' is invalid" /></td>
	</tr>
	</asp:placeholder>
	<tr>
		<td class="optional">Price Group Description:</td>
		<td class="field"><asp:textbox id="txtPriceGroupDescription" runat="server" maxlength="30" columns="30" style="width: 199px;"></asp:textbox></td>
		<td></td>
	</tr>
</table>

<%--<table border="0" cellspacing="1" cellpadding="2">
	<div style="padding-bottom:0px"><h1>Banner Sales Price</h4></div>
<tr>
		<td class="optional">Image File:<br /><span class="smaller">475 x 205</span></td>
		<td class="field"><CC:FileUpload ID="fuImage" Folder="~/assets/SalesPrice" ImageDisplayFolder="~/assets/SalesPrice" DisplayImage="False" runat="server" style="width: 370px;" /><div runat="server" id="divImg">
			<b>Preview with Map:</b><map name="hpimgmap"><asp:Literal runat="server" ID="litMap" /></map>
			<div><asp:Image runat="server" ID="hpimg" usemap="#hpimgmap" Width="475px" Height="205px" /></div>
		</div></td>
		<td><CC:FileUploadExtensionValidator CssClass="msgError" Extensions="swf,swf,jpg,jpeg,gif,bmp" ID="feImage" runat="server" Display="Dynamic" ControlToValidate="fuImage" ErrorMessage="Invalid file extension"></CC:FileUploadExtensionValidator></td>
	</tr>
	<tr>
		<td class="optional">Is Active?</td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsActive" Checked="true" /></td>
		<td></td>
	</tr>
</table>
--%>
<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Sales Price?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>

</asp:content>
