<%@ Page Language="VB"  AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" CodeFile="CheckImages.aspx.vb" Inherits="CheckImages" %>

<asp:content ID="Content" runat="server" contentplaceholderid="ph">

    <div>
    <div><h1>Check missing</h1></div>
    <br />
       <asp:CheckBoxList ID="chkMissing" runat="server" RepeatColumns="5">
        <asp:ListItem Value="i" Selected="True">Image</asp:ListItem>
        <asp:ListItem Value="sd">ShortDesc</asp:ListItem>
        <asp:ListItem Value="ld">LongDesc</asp:ListItem>
        <asp:ListItem Value="uc">UrlCode</asp:ListItem>
        <asp:ListItem Value="dp">Department</asp:ListItem>
        </asp:CheckBoxList>
        <br />
        <asp:Button runat="server" ID="btnSubmit" Text="Search" />
        </div>
        <br />
    <asp:Literal ID="ltcontent" runat="server"></asp:Literal>
    </div>
    <div>
    <asp:repeater id="rptMissing" runat="server">
	<HeaderTemplate>
		<table border="0" cellspacing="2" cellpadding="3" width="100%">
			<tr>
				<th>
					SKU</th>
				<th>
					Product Name</th>
				<th>
					LongDesc</th>
				<th>
					ShortDesc</th>
				<th>
					Image</th>
				<th>
					UrlCode</th>
			</tr>
	</HeaderTemplate>
	<ItemTemplate>
		<tr valign="top" class="row">
			<td><%#DataBinder.Eval(Container.DataItem, "SKU")%></td>
			<td><%#DataBinder.Eval(Container.DataItem, "ItemName")%></td>
			<td><%#DataBinder.Eval(Container.DataItem, "HasDescription")%></td>
			<td><%#DataBinder.Eval(Container.DataItem, "ShortDesc")%></td>
			<td><%#DataBinder.Eval(Container.DataItem, "Image")%></td>
			<td><%#DataBinder.Eval(Container.DataItem, "UrlCode")%></td>
		</tr>
	</ItemTemplate>
	<AlternatingItemTemplate>
		<tr valign="top" class="alternate">
		    <td><%#DataBinder.Eval(Container.DataItem, "SKU")%></td>
			<td><%#DataBinder.Eval(Container.DataItem, "ItemName")%></td>
			<td><%#DataBinder.Eval(Container.DataItem, "HasDescription")%></td>
			<td><%#DataBinder.Eval(Container.DataItem, "ShortDesc")%></td>
			<td><%#DataBinder.Eval(Container.DataItem, "Image")%></td>
			<td ><%#DataBinder.Eval(Container.DataItem, "UrlCode")%></td>
		</tr>
	</AlternatingItemTemplate>
	<FooterTemplate>
		</table>
	</FooterTemplate>
</asp:repeater>
    </div>

</asp:content>

