<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Sales Price" CodeFile="default.aspx.vb" Inherits="admin_store_items_salesprice_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">

<h4>Sales Price Administration: <%=dbItem.ItemName%></h4>

<p></p>
<a href="/admin/store/items/">&laquo; Back to Items</a>

<p></p>
<%--<span class="smaller">Please provide search criteria below</span>--%>
<table cellpadding="2" cellspacing="2" style="display: none;">
    <tr>
        <th valign="top">Username:</th>
        <td valign="top" class="field"><asp:DropDownList id="F_MemberId" runat="server" /></td>
        <th valign="top">Customer Price Group Code:</th>
        <td valign="top" class="field"><asp:DropDownList id="F_CustomerPriceGroupId" runat="server" /></td>
    </tr>
    <tr>
        <td colspan="4" align="right">
            <CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
            <input class="btn" type="submit" value="Clear" onclick="window.location='default.aspx?ItemId=<%=ItemId%>';return false;" />
        </td>
    </tr>
</table>
<table border="0" cellspacing="1" cellpadding="2" id="tbCase" runat="server" visible="false">
    <tr>
        <td colspan="2">
            <span class="smallest"><span class="red">red color</span> - denotes required fields</span>
        </td>
    </tr>
    <tr>
        <td class="required" style="width: 150px;">
            Case Price:
        </td>
        <td class="field">
            <asp:TextBox ID="txtCasePrice" runat="server" MaxLength="50" Columns="50" Style="width: 100px;"
                Enabled="true"></asp:TextBox>
        </td>
        <td>
            <CC:OneClickButton ID="btnSave" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
        </td>
        <td>
            <CC:IntegerValidator ID="IntegerValidator2" runat="server" ControlToValidate="txtCasePrice"
                CssClass="msgError" ErrorMessage="Please enter a valid Case Price" Display="Dynamic" />
        </td>
    </tr>
    <tr>
        <td class="required" style="width: 150px;">
            Case Qty
        </td>
        <td class="field">
            <asp:TextBox ID="txtCaseQty" runat="server" MaxLength="50" Columns="50" Style="width: 100px;"
                Enabled="true"></asp:TextBox>
        </td>
        <td>
            <CC:OneClickButton ID="btnSave1" runat="server" Text="Save" CssClass="btn"></CC:OneClickButton>
        </td>
        <td>
            <CC:IntegerValidator ID="IntegerValidator1" runat="server" ControlToValidate="txtCaseQty"
                CssClass="msgError" ErrorMessage="Please enter a valid Quantity" Display="Dynamic" />
        </td>
    </tr>
</table>
   
<p></p>
<CC:OneClickButton id="AddNew" Runat="server" Text="Add New Sales Price" cssClass="btn"></CC:OneClickButton>
<p></p>

<div class="smaller red"><b>NOTE:</b> This data may be modified on the web but changes will be overwritten by Navision.<br />
<b>NOTE:</b> New records will <b>NOT</b> be updated by Navision unless they are also created in Navision</div>
<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>
		<asp:TemplateField>
			<ItemTemplate>
			<asp:HyperLink enableviewstate="False" runat="server" NavigateUrl= '<%# "edit.aspx?salestype=" & DataBinder.Eval(Container.DataItem, "salestype") & "&SalesPriceId=" & DataBinder.Eval(Container.DataItem, "SalesPriceId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/edit.gif" ID="lnkEdit">Edit</asp:HyperLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField>
			<ItemTemplate>
			<CC:ConfirmLink enableviewstate="False" Message="Are you sure that you want to remove this Sales Price?" runat="server" NavigateUrl= '<%# "delete.aspx?salestype=" & DataBinder.Eval(Container.DataItem, "salestype") & "&SalesPriceId=" & DataBinder.Eval(Container.DataItem, "SalesPriceId") & "&" & GetPageParams(Components.FilterFieldType.All) %>' ImageUrl="/includes/theme-admin/images/delete.gif" ID="lnkDelete">Delete</CC:ConfirmLink>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:TemplateField SortExpression="Username" HeaderText="Username">
			<ItemTemplate>
				<a href="/admin/members/edit.aspx?MemberId=<%#Container.DataItem("memberid")%>"><%#Container.DataItem("username")%></a>
			</ItemTemplate>
		</asp:TemplateField>
		<asp:BoundField DataField="CustomerPriceGroupCode" SortExpression="CustomerPriceGroupCode" HeaderText="Customer Price Group Code" />
		<asp:BoundField DataField="UnitPrice" SortExpression="UnitPrice" HeaderText="Unit Price" DataFormatString="{0:c}" />
        <asp:BoundField DataField="UnitPrice" SortExpression="UnitPrice" HeaderText="Case Price" DataFormatString="{0:c}" />
		<asp:BoundField DataField="MinimumQuantity" SortExpression="MinimumQuantity" HeaderText="Minimum Quantity" />
		<asp:BoundField DataField="StartingDate" SortExpression="StartingDate" HeaderText="Start Date" DataFormatString="{0:d}" />
		<asp:BoundField DataField="EndingDate" SortExpression="EndingDate" HeaderText="End Date" DataFormatString="{0:d}" />
	</Columns>
</CC:GridView>

</asp:content>
