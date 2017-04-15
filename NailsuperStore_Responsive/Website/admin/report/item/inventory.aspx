<%@ Page Language="VB" AutoEventWireup="false" validateRequest="false" CodeFile="inventory.aspx.vb" MasterPageFile="~/includes/masterpage/admin.master" Inherits="admin_store_items_inventory" %>
<asp:content ContentPlaceHolderId="ph" ID="Content1" runat="server">
<script language="javascript">
<!--
	function expandit(objid){
		var span = document.getElementById('SPAN' + objid).style;
		var img  = document.getElementById('IMG' + objid);
		var imgtext = document.getElementById('imgtext');
		if (span.display=="none") {
			span.display="block"
			img.src = img.src.replace(/down/i, "up");
			imgtext.innerText = 'Hide Image';
		} else {
			span.display="none"
			img.src = img.src.replace(/up/i, "down");
			imgtext.innerText = 'View Image';
		}
	}
	function exportXls()
    {	
	 document.getElementById("ctl00_ph_hidexport").value = document.getElementById("Excel").innerHTML
     return true;	
    }
//-->
</script>

<h4>Inventory Report</h4>

<p></p>
<table cellpadding="2" cellspacing="2">
<tr>
<th valign="top">Department:</th>
<td valign="top" class="field"><asp:DropDownList id="F_DepartmentId" runat="server" /></td>
<th valign="top">Item Name:</th>
<td valign="top" class="field"><asp:textbox id="F_ItemName" runat="server" Columns="25" MaxLength="255"></asp:textbox></td>
</tr>
<tr>
<th valign="top">Item Type:</th>
<td valign="top" class="field"><asp:dropdownlist id="F_ItemType" runat="server"><asp:ListItem Text="-- ALL --" Value="" /><asp:ListItem Text="Single Items" Value="0" /><asp:ListItem Text="Item Group Items" Value="1" /></asp:dropdownlist></td>
<th valign="top">SKU:</th>
<td valign="top" class="field"><asp:textbox id="F_SKU" runat="server" Columns="20" MaxLength="20"></asp:textbox></td>
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
<th valign="top"><b>Is Featured:</b></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_IsFeatured" runat="server">
		<asp:ListItem Value="">-- ALL --</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
</td>
</tr>

<tr>
<th valign="top">&nbsp;&nbsp;&nbsp;<b>Is Out of Stock:</b></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_IsOutOfStock" runat="server">
		<asp:ListItem Value="">-- ALL --</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
</td>
<th valign="top"><b>Is Drop Ship:</b></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_IsDropShip" runat="server">
		<asp:ListItem Value="">-- ALL --</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
</td>
</tr>

<tr>
<th valign="top"><b>Brand:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_BrandId" runat="server" /></td>
<th valign="top"><b>Is New:</b></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_IsNew" runat="server">
		<asp:ListItem Value="">-- ALL --</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
</td></tr>
<tr>
<th valign="top"><b>Group Name:</b></th>
<td valign="top" class="field"><asp:DropDownList ID="F_GroupName" runat="server" /></td>
<th valign="top"><b>Has Sales Price?</b></th>
<td valign="top" class="field">
	<asp:DropDownList ID="F_HasSalesPrice" runat="server">
		<asp:ListItem Value="">-- ALL --</asp:ListItem>
		<asp:ListItem Value="1">Yes</asp:ListItem>
		<asp:ListItem Value="0">No</asp:ListItem>
	</asp:DropDownList>
</td>
</tr>
<tr><td style="padding:10px 0px 10px 0px;"><CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" /></td>
<td style="padding:10px 0px 10px 0px;"><CC:OneClickButton id="btnExcel" Runat="server" Text="Excel" cssClass="btn" /></td>
</tr>
</table>
<div id="Excel">
<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="30" AllowPaging="True" AllowSorting="True" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		
		
		<asp:TemplateField HeaderText="Group Name" SortExpression="GroupName">
			<ItemTemplate>
			<asp:Literal runat="server" id="litType" />
			
</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="ItemName" DataField="ItemName" HeaderText="Item Name"></asp:BoundField>
<asp:BoundField SortExpression="qtyonhand" DataField="qtyonhand" HeaderText="Quantity On Hand"></asp:BoundField>
<asp:BoundField SortExpression="qtOrder" DataField="qtOrder" HeaderText="Quantity order"></asp:BoundField>
		<asp:BoundField SortExpression="QtRemain" DataField="QtRemain" HeaderText="Quantity Remain"></asp:BoundField>
		<asp:BoundField SortExpression="SKU" DataField="SKU" HeaderText="SKU"></asp:BoundField>
		<asp:TemplateField HeaderText="Price" SortExpression="Price">
			<ItemTemplate>
				<asp:Literal enableviewstate="False" runat="server" Id="ltlPrice" />
			
</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="TotalPrice" SortExpression="TotalPrice">
			<ItemTemplate>
				<asp:Literal enableviewstate="False" runat="server" Id="ltlTotalPrice" />
			
</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Department(s)">
			<ItemTemplate>
				<asp:Repeater ID="Departments" Runat="server">
					<SeparatorTemplate><br /></SeparatorTemplate>
					<ItemTemplate>
						<li style="list-style-image:url(/includes/theme-admin/images/minifolder.gif)">
							<%#Container.DataItem("NAME")%>
					</ItemTemplate>
				</asp:Repeater>
			
</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Image Preview">
			<ItemTemplate>
				<asp:Label runat="server" ID="imglink"><a href='javascript:expandit(<%#Container.DataItem("ItemId")%>);'><span class="smaller" id="imgtext">View Image</span><img id='IMG<%#Container.DataItem("ItemId")%>' src="/includes/theme-admin/images/detail-down.gif" width="8" height="8" hspace="2" border="0" alt="Expand" align=absmiddle></a></asp:Label><asp:Label runat="server" ID="noimg" Text="N/A" CssClass="smaller" /><span style="display:none" id='SPAN<%#Container.DataItem("ItemId")%>'><asp:Literal id="img" Runat="server"></asp:Literal></span>
</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="BrandName" DataField="Brandname" HeaderText="Brand Name"></asp:BoundField>
		<asp:Checkboxfield SortExpression="IsActive" DataField="IsActive" HeaderText="Is Active">
            <itemstyle horizontalalign="Center" />
        </asp:Checkboxfield>
		<asp:Checkboxfield SortExpression="IsFeatured" DataField="IsFeatured" HeaderText="Is Featured">
            <itemstyle horizontalalign="Center" />
        </asp:Checkboxfield>
	</Columns>
    <HeaderStyle VerticalAlign="Top" />
</CC:GridView>
</div> 
<INPUT id="hidexport" type="hidden" name="hidexport" runat="server"/>
</asp:content>