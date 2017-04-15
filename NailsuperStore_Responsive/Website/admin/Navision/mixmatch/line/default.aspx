<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Mix Match Lines" CodeFile="default.aspx.vb" Inherits="admin_navision_mixmatch_line_Index"  %>

<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
	<%--<script language="javascript">
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
	        InitQueryCode('LookupField', '/admin/ajax.aspx?f=DisplayItemActive&q=', MyCallback);
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
	</script>--%>
<h4>Mix Match Lines Administration</h4>

<a href="/admin/navision/mixmatch/default.aspx?type=<%=Type%>">&laquo; Back to Mix Match Promotions</a>
<p></p>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">
<tr runat="server" visible="false">
<th valign="top"><b>Mix Match No:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_MixMatchId" runat="server" /></td>
</tr>
<tr>
<th valign="top"><b>SKU/Product:</b></th>
<td valign="top" class="field"><%--<input id="LookupField" name="LookupField" style="BORDER-RIGHT:#999999 1px solid; PADDING-RIGHT:4px; BORDER-TOP:#999999 1px solid; PADDING-LEFT:4px; BORDER-LEFT:#999999 1px solid; WIDTH:360px; BORDER-BOTTOM:#999999 1px solid">
                        <asp:HiddenField ID="ItemId" runat="server" value="0" />--%>
                    <%--<input type="button" class="btn" id="Button1" value="Add Item" onclick="OpenPopUp();" />--%>
                    <asp:TextBox ID="txtSku" runat="server" MaxLength="8" Columns="10"></asp:TextBox>
</td>
</tr>
		
<tr>
<th valign="top">Discount Type:</th>
<td valign="top" class="field"><asp:textbox id="F_DiscountType" runat="server" Columns="10" MaxLength="10"></asp:textbox></td>
</tr>
<tr>
<th valign="top"><b>Value:</b></th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td>From<asp:textbox id="F_ValueLBound" runat="server" Columns="5" MaxLength="10"/></td><td>To<asp:textbox id="F_ValueUbound" runat="server" Columns="5" MaxLength="10"/></td>
</tr>
</table>
</td>
</tr>
<tr runat="server" visible="false">
<th valign="top"><b>Is Active:</b></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_IsActive" runat="server">
		<asp:ListItem Value="">-- ALL --</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
</td>
</tr>
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
<input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx?type=<%=Type%>&F_MixMatchId=<%=Request("F_MixMatchId")%>';return false;" />
</td>
</tr>
</table>
</asp:Panel>
<p></p>
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Mix Match Line" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?Id=" & DataBinder.Eval(Container.DataItem, "Id") & "&type=" & Type &  "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Mix Match Line?" runat="server" NavigateUrl= '<%# "delete.aspx?Id=" & DataBinder.Eval(Container.DataItem, "Id") & "&type=" & Type &  "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="MixMatchNo" DataField="MixMatchNo" HeaderText="Mix Match No"></asp:BoundField>
		<asp:BoundField SortExpression="[LineNo]" DataField="LineNo" HeaderText="Line No"></asp:BoundField>
		<asp:BoundField SortExpression="ItemName" DataField="ItemName" HeaderText="Item"></asp:BoundField>
		<asp:BoundField SortExpression="DiscountType" DataField="DiscountType" HeaderText="Discount Type"></asp:BoundField>
		<asp:BoundField SortExpression="Value" DataField="Value" HeaderText="Value"></asp:BoundField>
		<asp:Checkboxfield visible="false" ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive" HeaderText="Is Active"/>
	</Columns>
</CC:GridView>
<%--<input type="hidden" runat="server" value="" id="hidPopUpSKU" />--%>
<asp:HiddenField ID="hidCon" runat="server" />

<%--<script type="text/javascript" src="../../../../includes/theme-admin/scripts/Browser.js"></script>
    <script>
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
    
    </script>--%>
</asp:content>