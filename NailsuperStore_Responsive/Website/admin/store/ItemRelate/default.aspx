<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Item" CodeFile="default.aspx.vb" Inherits="admin_store_itemrelate_Index"  %>
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
//-->
</script>

<h4>Item Relation</h4>

<span class="smaller">Please provide search criteria below</span>
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
<tr>
<td colspan="4" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
<input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx';return false;" />
</td>
</tr>
</table>
<p></p>
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Item" cssClass="btn"></CC:OneClickButton>
<p></p>

<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "edit.aspx?ItemId=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate=False Message="Are you sure that you want to remove this Item?" runat="server" NavigateUrl= '<%# "delete.aspx?ItemId=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/images/admin/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate=False runat="server" NavigateUrl= '<%# "/admin/store/items/images/default.aspx?F_ItemId=" & DataBinder.Eval(Container.DataItem, "ItemId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ID="lnkImages" ImageUrl="/images/admin/image.gif" cssclass="smaller">Images</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<input type="button" class="btn" style="font-size:11px;" value="Sales Price (<%#Container.DataItem("iCount")%>)" onclick="window.location='/admin/store/items/salesprice/default.aspx?ItemId=<%#Container.DataItem("itemid")%>'" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Group Name" SortExpression="GroupName">
			<ItemTemplate>
			<asp:Literal runat="server" id="litType" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="ItemName" DataField="ItemName" HeaderText="Item Name"></asp:BoundField>
		<asp:BoundField SortExpression="SKU" DataField="SKU" HeaderText="SKU"></asp:BoundField>
		<asp:TemplateField HeaderText="Price" SortExpression="Price">
			<ItemTemplate>
				<asp:Literal enableviewstate="False" runat="server" Id="ltlPrice" />
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Department(s)">
			<ItemTemplate>
				<asp:Repeater ID="Departments" Runat="server">
					<SeparatorTemplate><br /></SeparatorTemplate>
					<ItemTemplate>
						<li style="list-style-image:url(/images/minifolder.gif)">
							<%#Container.DataItem("NAME")%>
					</ItemTemplate>
				</asp:Repeater>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Image Preview">
			<ItemTemplate>
				<asp:Label runat="server" ID="imglink"><a href='javascript:expandit(<%#Container.DataItem("ItemId")%>);'><span class="smaller" id="imgtext">View Image</span><img id='IMG<%#Container.DataItem("ItemId")%>' src="/images/detail-down.gif" width="8" height="8" hspace="2" border="0" alt="Expand" align=absmiddle></a></asp:Label><asp:Label runat="server" ID="noimg" Text="N/A" CssClass="smaller" /><span style="display:none" id='SPAN<%#Container.DataItem("ItemId")%>'><asp:Literal id="img" Runat="server"></asp:Literal></span></ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField SortExpression="BrandName" DataField="BrandName" HeaderText="Brand Name"></asp:BoundField>
		<asp:Checkboxfield ItemStyle-HorizontalAlign="Center" SortExpression="IsActive" DataField="IsActive" HeaderText="Is Active"/>
		<asp:Checkboxfield ItemStyle-HorizontalAlign="Center" SortExpression="IsFeatured" DataField="IsFeatured" HeaderText="Is Featured"/>
	</Columns>
</CC:GridView>

</asp:content>