<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" CodeFile="attributes.aspx.vb" Inherits="admin_store_items_attributes" %>
<asp:Content runat="server" ID="Content" ContentPlaceHolderID="ph">

<p><b><%=Item.ItemName%></b>
<hr size=1>
<h4>UPDATE ITEM ATTRIBUTES</h4>
<a href="edit.aspx?ItemId=<%=Item.ItemId%>">Edit Item</a>
<br>
<a href="related.aspx?ItemId=<%=Item.ItemId%>">Edit Related Items</a>

<p>
If you want to specify special SKU numbers for combination of attributes, please specify the prefix here
<table border=0 cellspacing=2 cellpadding=3>
<tr>
	<td><b>Prefix</b></td>
	<td valign=top><asp:TextBox runat="server" size="10" MaxLength="10" ID="txtPrefix" /></td>
</tr>
</table>

<p><CC:GridView runat="server" ID="gv" GridLines="none" BorderWidth="0" AutoGenerateColumns="false" CellPadding="2" CellSpacing="1">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top" />
	<RowStyle CssClass="row" VerticalAlign="Top" />
	<Columns>
		<asp:TemplateField HeaderText="Attribute Name">
			<ItemTemplate><asp:Textbox runat="server" id="txtName" text='<%#Container.DataItem.Name%>' style="font-family:arial;font-size:11px;" /></ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Attribute Value<br /><br /><span class='smaller'>Please input each value in a separate line!</span>">
			<ItemTemplate><asp:Textbox runat="server" id="txtValue" TextMode="multiline" wrap="false" Rows="6" Columns="60" style="font-family:arial;font-size:11px;" /></ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Part of SKU<br /><span class='smaller'>Please input each value<br />in a separate line!</span>">
			<ItemTemplate><asp:Textbox runat="server" id="txtSKU" TextMode="multiline" wrap="false" Rows="6" Columns="20" style="font-family:arial;font-size:11px;" /></ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField HeaderText="Extra Price<br /><span class='smaller'>Please input each value<br />in a separate line!</span>">
			<ItemTemplate><asp:Textbox runat="server" id="txtPrice" TextMode="multiline" wrap="false" Rows="6" Columns="20" style="font-family:arial;font-size:11px;" /></ItemTemplate>
		</asp:TemplateField>
	</Columns>
</CC:GridView>

<p>
<CC:OneClickButton runat="server" ID="btnSave" Text="Save" CssClass="btn" />
<asp:Button runat="server" ID="btnCancel" Text="Cancel" CssClass="btn" />
</p>

</asp:Content>