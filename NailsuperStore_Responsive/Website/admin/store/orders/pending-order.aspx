<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/includes/masterpage/admin.master" Title="Pending Order" CodeFile="pending-order.aspx.vb" Inherits="admin_pending_order_Index"  %>
<asp:content ContentPlaceHolderId="ph" ID="Content" runat="server">
<script src="/includes/scripts/qtip/jquery.qtip.min.js" type="text/javascript"></script>
<h4>Pending Order Administration</h4>

<span class="smaller">Please provide search criteria below</span>
<asp:Panel ID="pnlSearch" DefaultButton="btnSearch" runat="server">
<table cellpadding="2" cellspacing="2">

<tr>
<th valign="top">Member:</th>
<td valign="top" class="field">
    <asp:DropDownList ID="F_IsGuest" runat="server">
        <asp:ListItem Value="">-- ALL --</asp:ListItem>
        <asp:ListItem Value="1">Active</asp:ListItem>
        <asp:ListItem Value="0">Guest</asp:ListItem>
        <asp:ListItem Value="-1">Unknown</asp:ListItem>
    </asp:DropDownList>
</td>
</tr>

<tr>
<th valign="top">Created Date:</th>
<td valign="top" class="field">
<table border="0" cellpadding="0" cellspacing="0">
<tr><td class="smaller">From <CC:DatePicker id="F_OrderDateLbound" runat="server" /></td><td>&nbsp;</td><td class="smaller">To  <CC:DatePicker id="F_OrderDateUbound" runat="server" /></td>
</tr>
</table>
</td>
</tr>
			
<tr>
<td colspan="2" align="right">
<CC:OneClickButton id="btnSearch" Runat="server" Text="Search" cssClass="btn" />
<input class="btn" type="submit" value="Clear" onclick="window.location='pending-order.aspx';return false;" />
</td>
</tr>
</table>
</asp:Panel>
<p></p>
<style type="text/css">
    .row
    {
        vertical-align: top;
        height: 30px;
    }

    .alternate
    {
        vertical-align: top;
        height: 30px;
    }
    .blue
    {
        color: Blue;
    }
    .ShowLog
    {
        color: Blue;
        cursor: pointer;
    }
    .LatestStepLog
    {
        border-collapse: collapse;
    }
    .LatestStepLog th, .LatestStepLog td
    {
        border: 1px solid #999999;
    }
    .hidden-span
    {
        display: none;
    }

</style>
<CC:GridView id="gvList" CellSpacing="2" CellPadding="2" runat="server" PageSize="50" AllowPaging="True" AllowSorting="True" HeaderText="In order to change display order, please use header links" EmptyDataText="There are no records that match the search criteria" AutoGenerateColumns="False" BorderWidth="0" PagerSettings-Position="Bottom">
	<AlternatingRowStyle CssClass="alternate" VerticalAlign="Top"></AlternatingRowStyle>
	<RowStyle CssClass="row" VerticalAlign="Top"></RowStyle>
	<Columns>	
        <asp:TemplateField HeaderStyle-Width="80"  HeaderText="First Name">
            <ItemTemplate>
                <asp:Literal ID="ltMember" runat="server"></asp:Literal>
            </ItemTemplate>
        </asp:TemplateField>
		<asp:BoundField DataField="ShipToName2" HeaderText="Last Name"></asp:BoundField>
		<asp:BoundField DataField="ShipToCounty" HeaderText="State"></asp:BoundField>
		<asp:BoundField DataField="ShipToCountry" HeaderText="Country"></asp:BoundField>
		<asp:BoundField DataField="Email" HeaderText="Email"></asp:BoundField>
		<asp:BoundField DataField="CreateDate" HeaderText="Create Date"></asp:BoundField>
        <asp:TemplateField HeaderText="Total" HeaderStyle-Width="40" SortExpression="Total" ItemStyle-HorizontalAlign="Right">
			<ItemTemplate>
				<asp:Literal enableviewstate="False" runat="server" Id="ltTotal"  />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:Checkboxfield SortExpression="IsActive" DataField="IsActive" HeaderText="Is Active">
            <itemstyle horizontalalign="Center"/>
        </asp:Checkboxfield>
        <asp:TemplateField HeaderText="Latest Step" ItemStyle-VerticalAlign="Middle" >
            <ItemTemplate>
                <span class="ShowLog" id="<%# Eval("OrderId") %>" onclick="getId(<%# Eval("OrderId") %>)">
                    <%# Eval("PageName") + " | " + Eval("Action")%>
                </span>
                <span class="hidden-span" id="LatestStepLog<%# Eval("OrderId") %>">&nbsp;</span>  
            </ItemTemplate>
        </asp:TemplateField>
              
	</Columns>
</CC:GridView>
<asp:HiddenField ID="hidCon" runat="server" />
<div runat="server" id="divDownload" visible="false">
<asp:HyperLink id="lnkDownload" runat="server">Download File</asp:HyperLink>
</div>


<script type="text/javascript">
    function getId(orderId){
        $.ajax({
            type: "POST",
            url: "/admin/store/orders/pending-order.aspx/GetLatestStep",
            data: "{'param1':" + orderId + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (msg) {
                $('#LatestStepLog' + orderId + '').html(msg.d);
                $('#LatestStepLog' + orderId + '').toggle();
            }
        });
    }
    
</script>

</asp:content>


