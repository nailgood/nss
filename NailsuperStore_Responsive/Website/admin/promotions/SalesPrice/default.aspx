<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Homepage" CodeFile="default.aspx.vb" Inherits="Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<h4>Top Sale Price</h4>

<div><input type="button" class="btn" id="Button1" value="Add new" onclick="OpenPopUp();" /></div>
<%--<table cellpadding="2" cellspacing="2">

<th valign="top">SKU:</th>
<td valign="top" class="field"><asp:textbox id="F_SKU" runat="server" Columns="50" MaxLength="50"></asp:textbox></td>
</tr>
<tr>
<th valign="top"><b>Start Date:</b></th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td class="smaller">From <CC:DatePicker id="F_StartDateLbound" runat="server" /></td><td>&nbsp;</td><td class="smaller">To  <CC:DatePicker id="F_StartDateUbound" runat="server" /></td>
</tr>
</table>
</td>
</tr>
<tr>
<th valign="top"><b>End Date:</b></th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td class="smaller">From <CC:DatePicker id="F_EndDateLbound" runat="server" /></td><td>&nbsp;</td><td class="smaller">To  <CC:DatePicker id="F_EndDateUbound" runat="server" /></td>
</tr>
</table>
</td>
</tr>

<tr>
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
<input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
</td>
</tr>
</table>--%>

<p></p>
<div style="display:none"><CC:OneClickButton id="AddNew" Runat="server" Text="Add New SalesPrice Image" cssClass="btn" ></CC:OneClickButton></div>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "../../store/items/salesprice/edit.aspx?act=y&SalesPriceId=" & DataBinder.Eval(Container.DataItem, "SalesPriceId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Image?" runat="server" NavigateUrl= '<%# "delete.aspx?SalesPriceId=" & DataBinder.Eval(Container.DataItem, "SalesPriceId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="SalesPriceId" SortExpression="SalesPriceId" HeaderText="SalesPrice Id" />
		<asp:BoundField DataField="SKU" SortExpression="SKU" HeaderText="SKU" />
		<asp:BoundField DataField="ItemName" SortExpression="ItemName" HeaderText="Item Name" />
		<asp:Checkboxfield ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive" HeaderText="Is Active"/>
		<asp:BoundField DataField="StartingDate" SortExpression="StartingDate" HeaderText="Starting Date" />
		<asp:BoundField DataField="EndingDate" SortExpression="EndingDate" HeaderText="Ending Date" />		
	</Columns>
</CC:GridView>
 <input type="hidden" runat="server" value="" id="hidPopUpSKU" />
<script>
    function OpenPopUp() {
   
        var item = document.getElementById('<%=hidPopUpSKU.ClientID %>').value;
        var url = '../ShopSave/SearchSKU.aspx?Type=0&item=' + item
             var p = window.showModalDialog(url, '', 'dialogHeight: 600px; dialogWidth: 900px; dialogTop: 50px; dialogLeft:200px; edge: Raised;help: No; resizable: No; status: No;');
        document.getElementById('<%=hidPopUpSKU.ClientID %>').value = p;
        var button = document.getElementById('<%=AddNew.ClientID %>');     
        if (button)
            button.click();

    }
</script>
</asp:content>

