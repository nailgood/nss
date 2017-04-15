<%@ Page Language="VB" AutoEventWireup="false" CodeFile="edit.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_navision_mixmatch_line_Edit"  Title="Mix Match Lines"%>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
   <%-- <script language="javascript">
	<!--
	    function MyCallback(ItemId) {
	        //alert(ItemId);
	        //document.getElementById('ItemId').value = ItemId;
	        document.getElementById('<%= ItemId.ClientID %>').value = ItemId;
	        GetItemInfo();
	    }

	    if (window.addEventListener) {
	        window.addEventListener('load', InitializeQuery, false);
	    } else if (window.attachEvent) {
	        window.attachEvent('onload', InitializeQuery);
	    }

	    function InitializeQuery() {
	        InitQueryCode('ctl00$ph$LookupField', '/admin/ajax.aspx?f=DisplayItemActive&q=', MyCallback);
	    }

	    function GetItemInfo() {
	        var sItem, sConn;

	        xml = getXMLHTTP();
	        if (xml) {
	            xml.open("GET", "/admin/Ajax.aspx?f=DisplayItemActive&ItemId=" + getValue(document.getElementById('ItemId')), true);
	            xml.onreadystatechange = function () {
	                if (xml.readyState == 4 && xml.responseText) {
	                    if (xml.responseText.length > 0) {
	                        aData = xml.responseText.split('|');

	                        sItem = '';
	                        sItem += 'Item Number: ' + aData[0] + '<br>';
	                        sItem += 'Item: ' + aData[1];
	                        alert(aData[1]);
	                        sItem += '<br /><br />';
	                        var tmp = aData[2].split('[~]');
	                        for (var i = 0; i < tmp.length; i++) {
	                            sItem += tmp[i];
	                        }

	                        //alert(sItem);
	                        document.getElementById('ItemInfo').innerHTML = sItem;
	                    } else {
	                        sItem = '';
	                    }
	                }
	            }
	            xml.send(null);
	        }

	    }	
	//-->
	</script>	--%>
<h4><% If MixMatchLineId = 0 Then%>Add<% Else %>Edit<% End If %> Mix Match Line</h4>

<table border="0" cellspacing="1" cellpadding="2">
	<tr><td colspan=2><span class=smallest><span class=red>red color</span> - denotes required fields</span></td></tr>
	<tr>
		<td class="required">Mix Match Id:</td>
		<td class="field"><asp:DropDownList id="drpMixMatchId" runat="server" /></td>
		<td><asp:RequiredFieldValidator ID="rfvMixMatchId" runat="server" Display="Dynamic" ControlToValidate="drpMixMatchId" CssClass="msgError" ErrorMessage="Field 'Mix Match Id' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Line No:</td>
		<td class="field"><asp:textbox id="txtLineNo" runat="server" maxlength="8" columns="8" style="width: 43px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvLineNo" runat="server" Display="Dynamic" ControlToValidate="txtLineNo" CssClass="msgError" ErrorMessage="Field 'Line No' is blank"></asp:RequiredFieldValidator><CC:IntegerValidator Display="Dynamic" runat="server" id="fvLineNo" ControlToValidate="txtLineNo" CssClass="msgError" ErrorMessage="Field 'Line No' is invalid" /></td>
	</tr>
	<tr>
		<td class="required">SKU/Product:</td>
		<td class="field"><%--<input id="LookupField" name="LookupField" type="text" style="BORDER-RIGHT:#999999 1px solid; PADDING-RIGHT:4px; BORDER-TOP:#999999 1px solid; PADDING-LEFT:4px; BORDER-LEFT:#999999 1px solid; WIDTH:360px; BORDER-BOTTOM:#999999 1px solid" runat="server" />
                        <asp:HiddenField ID="ItemId" runat="server" value="0" />--%>
                         <input type="button" class="btn" id="Button1" value="Add Item" onclick="OpenPopUp();" />
                        <asp:TextBox ID="txtSku" runat="server" MaxLength="8" Columns="8" Enabled="false" Style="width: 67px;"></asp:TextBox>
        </td>
		<td></td>
	</tr>
	<tr>
		<td class="required">Discount Type:</td>
		<td class="field"><asp:DropDownList runat="server" ID="ddlDiscountType"><asp:ListItem Value="Disc. %" Text="Disc. %" /><asp:ListItem Value="Deal Price" Text="Deal Price" /></asp:DropDownList></td>
		<td><asp:RequiredFieldValidator ID="rfvDiscountType" runat="server" Display="Dynamic" ControlToValidate="ddlDiscountType" CssClass="msgError" ErrorMessage="Field 'Discount Type' is blank"></asp:RequiredFieldValidator></td>
	</tr>
	<tr>
		<td class="required">Value:</td>
		<td class="field"><asp:textbox id="txtValue" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><asp:RequiredFieldValidator ID="rfvValue" runat="server" Display="Dynamic" ControlToValidate="txtValue" CssClass="msgError" ErrorMessage="Field 'Value' is blank"></asp:RequiredFieldValidator><CC:FloatValidator Display="Dynamic" runat="server" id="fvValue" ControlToValidate="txtValue" CssClass="msgError" ErrorMessage="Field 'Value' is invalid" /></td>
	</tr>
    <tr>
		<td class="optional">Qty Default:</td>
		<td class="field"><asp:textbox id="txtQtydefault" runat="server" maxlength="8" columns="8" style="width: 67px;"></asp:textbox></td>
		<td><asp:Label ID="lbmsg" runat="server" CssClass="msgError"></asp:Label></td>
	</tr>
	<tr runat="server" visible="false">
		<td class="required"><b>Is Active?</b></td>
		<td class="field"><asp:CheckBox runat="server" ID="chkIsActive" /></td>
	</tr>
</table>


<p></p>
<CC:OneClickButton id="btnSave" runat="server" Text="Save" cssClass="btn"></CC:OneClickButton>
<CC:ConfirmButton id="btnDelete" runat="server" Message="Are you sure want to delete this Mix Match Line?" Text="Delete" cssClass="btn" CausesValidation="False"></CC:ConfirmButton>
<CC:OneClickButton id="btnCancel" runat="server" Text="Cancel" cssClass="btn" CausesValidation="False"></CC:OneClickButton>
<input type="hidden" runat="server" value="" id="hidPopUpSKU" />
<input type="hidden" runat="server" id="hidItemId" />


<script type="text/javascript" src="../../../../includes/theme-admin/scripts/Browser.js"></script>
    <script>
        function SetValue(save, value, isactive) {
            if (save == '1') {
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
            }
        }
        function OpenPopUp() {

            var item = document.getElementById('<%=hidPopUpSKU.ClientID %>').value;
            var url = '../../../promotions/ShopSave/SearchItem.aspx?Type=0&item=' + item

            var brow = GetBrowser();
            if (brow == 'ie') {
                var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
                document.getElementById('<%=txtSku.ClientID %>').value = p;
            }
            else {
                ShowPopUpDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
            }
        }
        function SetValue(save, value) {

            if (save == '1') {
                document.getElementById('<%=hidPopUpSKU.ClientID %>').value = value;
                document.getElementById('<%=txtSKU.ClientID %>').value = value;
            }

        }
    
    </script>
</asp:content>

